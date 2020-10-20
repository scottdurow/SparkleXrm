using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class DeployPluginAndWorkflowTests
    {
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DeployPlugins()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var pluginTask = new DeployPluginsTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\TestPluginWorkflowCombined");

            pluginTask.Execute(path);
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DeployWorfklows()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\TestPluginWorkflowCombined");

            var wfTask = new DeployWorkflowActivitiesTask(crmSvc, trace);

            wfTask.Execute(path);
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void DeployPluginsUnChangedActivities()
        {
            var fakeService = A.Fake<IOrganizationService>(a => a.Strict());

            var trace = new TraceLogger();
            var task = new DeployPluginsTask(fakeService, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\TestPluginWorkflowCombined");

            //var messageRequestBuilder = A.Fake<IUserMessagesCollectionRequestBuilder>();

            var plugin = new PluginAssembly()
            {
                Id = Guid.NewGuid()
            };

            var pluginTypeToBeRemoved = new PluginType
            {
                Id = Guid.NewGuid(),
                Name = "Type",
                PluginAssemblyId = plugin.ToEntityReference(),
                TypeName = "TypeName",
                IsWorkflowActivity = false
            };

            var existingWFActivityNotToBeRemoved = new PluginType
            {
                Id = Guid.NewGuid(),
                Name = "Type",
                PluginAssemblyId = plugin.ToEntityReference(),
                TypeName = "WFActivity",
                IsWorkflowActivity = true
            };

            A.CallTo(() => fakeService.Execute(A<OrganizationRequest>.Ignored)).ReturnsLazily((a) =>
            {
                var response = new RetrieveMultipleResponse();
                var logicalName = ((a.Arguments[0] as RetrieveMultipleRequest).Query as Microsoft.Xrm.Sdk.Query.QueryExpression).EntityName;
                switch (logicalName)
                {
                    case PluginAssembly.EntityLogicalName:
                        response["EntityCollection"] = new EntityCollection(new[] { plugin });
                        break;

                    case PluginType.EntityLogicalName:
                        response["EntityCollection"] = new EntityCollection(new[] { pluginTypeToBeRemoved, existingWFActivityNotToBeRemoved });
                        break;

                    case SdkMessageProcessingStepImage.EntityLogicalName:
                    case SdkMessageProcessingStep.EntityLogicalName:
                        response["EntityCollection"] = new EntityCollection();
                        break;

                    case SdkMessageFilter.EntityLogicalName:
                        response["EntityCollection"] = new EntityCollection(new[] { new SdkMessageFilter(){
                            SdkMessageFilterId = Guid.NewGuid(),
                            SdkMessageId = new EntityReference(SdkMessage.EntityLogicalName,Guid.NewGuid())
                        } });
                        break;
                }

                return response;
            });

            A.CallTo(() => fakeService.Update(A<Entity>.Ignored)).DoesNothing();
            A.CallTo(() => fakeService.Create(A<Entity>.Ignored)).ReturnsLazily((a) =>
            {
                return Guid.NewGuid();
            });
            A.CallTo(() => fakeService.Delete(
                A<string>.That.Matches(a => a == PluginType.EntityLogicalName),
                A<Guid>.That.Matches(a => a == pluginTypeToBeRemoved.Id))).DoesNothing();
            task.Execute(path);
        }
    }
}