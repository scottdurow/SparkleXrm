using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Microsoft.Xrm.Tooling.Connector;
using System.IO;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System.Linq;
using FakeItEasy;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class ImportCodeTests
    {
        [TestMethod]
        [TestCategory("Unit Tests")]
        public void CreatePluginAttributeCode()
        {
            var attribute = new CrmPluginRegistrationAttribute(
                                   MessageNameEnum.Update, "account", StageEnum.PostOperation, ExecutionModeEnum.Synchronous,
                                   "name,address1_line1",
                                   "Delete of account", 1, IsolationModeEnum.Sandbox)
            {
                Image1Name = "PreImage",
                Image1Attributes = "name,address1_line1",
                Image1Type = ImageTypeEnum.PreImage,
                Image2Name = "PostImage",
                Image2Attributes = "name,address1_line1",
                Image2Type = ImageTypeEnum.PostImage,
                DeleteAsyncOperaton = true

            };

            var code = attribute.GetAttributeCode("");
            Debug.WriteLine(code);
            Assert.AreEqual(Normalise("[CrmPluginRegistration(\"Update\",\"account\",StageEnum.PostOperation,ExecutionModeEnum.Synchronous,\"name, address1_line1\",\"Deleteofaccount\",1,IsolationModeEnum.Sandbox,Image1Type=ImageTypeEnum.PreImage,Image1Name=\"PreImage\",Image1Attributes=\"name, address1_line1\",Image2Type=ImageTypeEnum.PostImage,Image2Name=\"PostImage\",Image2Attributes=\"name, address1_line1\")]"), Normalise(code));
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void CreateAsyncPluginAttributeCode()
        {
            var attribute = new CrmPluginRegistrationAttribute(
                MessageNameEnum.Update, "account", StageEnum.PostOperation, ExecutionModeEnum.Asynchronous,
                "name,address1_line1",
                "Delete of account", 1, IsolationModeEnum.Sandbox)
            {
                Image1Name = "PreImage",
                Image1Attributes = "name,address1_line1",
                Image1Type = ImageTypeEnum.PreImage,
                Image2Name = "PostImage",
                Image2Attributes = "name,address1_line1",
                Image2Type = ImageTypeEnum.PostImage,
                DeleteAsyncOperaton = true

            };

            var code = attribute.GetAttributeCode("");
            Debug.WriteLine(code);
            Assert.AreEqual(Normalise("[CrmPluginRegistration(\"Update\",\"account\",StageEnum.PostOperation,ExecutionModeEnum.Asynchronous,\"name, address1_line1\",\"Deleteofaccount\",1,IsolationModeEnum.Sandbox,Image1Type=ImageTypeEnum.PreImage,Image1Name=\"PreImage\",Image1Attributes=\"name, address1_line1\",Image2Type=ImageTypeEnum.PostImage,Image2Name=\"PostImage\",Image2Attributes=\"name, address1_line1\",DeleteAsyncOperaton = True)]"), Normalise(code));
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void CreateWorkflowAttributeCode()
        {
            var attribute = new CrmPluginRegistrationAttribute(
            "WorkflowActivity", "FriendlyName", "Description", "Group Name", IsolationModeEnum.Sandbox)
            {

            };

            var code = attribute.GetAttributeCode("");
            Debug.WriteLine(code);
            Assert.AreEqual(Normalise(@"[CrmPluginRegistration(
        ""WorkflowActivity"", ""FriendlyName"", ""Description"", ""Group Name"", IsolationModeEnum.Sandbox
            )]"), Normalise(code));
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void RegisterPluginCode()
        {
            // Assemble

            #region Arrange
            ServiceLocator.Init();
            var trace = new TraceLogger();
            List<Entity> created = new List<Entity>();
            IOrganizationService service = A.Fake<IOrganizationService>(a => a.Strict());
            A.CallTo(() => service.Create(A<Entity>.Ignored))
                .ReturnsLazily((Entity entity) =>
                {

                    created.Add(entity);
                    return Guid.NewGuid();
                });
            A.CallTo(() => service.Execute(A<OrganizationRequest>.Ignored))
                .ReturnsLazily((OrganizationRequest request) =>
                     {
                         if (request.GetType() == typeof(RetrieveMultipleRequest))
                         {
                             var query = request as RetrieveMultipleRequest;
                             var queryExpression = query.Query as QueryExpression;
                             var results = new List<Entity>();

                             switch (queryExpression.EntityName)
                             {
                                 case SdkMessageFilter.EntityLogicalName:
                                     results.Add(new SdkMessageFilter()
                                     {
                                         SdkMessageFilterId = Guid.NewGuid(),
                                         SdkMessageId = new EntityReference(SdkMessage.EntityLogicalName, Guid.NewGuid())
                                     });
                                     break;
                             }

                             var response = new RetrieveMultipleResponse();
                             response.Results["EntityCollection"] = new EntityCollection(results);
                             return response;
                         }
                         else
                         {
                             throw new Exception("Unexpected Call");
                         }
                     });

            #endregion

            #region Act
            using (var ctx = new OrganizationServiceContext(service))
            {
                var pluginRegistration = new PluginRegistraton(service, ctx, trace);

                var testpluginPath = new DirectoryService().Search(AppDomain.CurrentDomain.BaseDirectory, @"\..\..\..\TestPlugin\bin\**\TestPlugin.dll");
                pluginRegistration.RegisterPlugin(testpluginPath[0]);
            }
            #endregion

            #region Assert
            Assert.AreEqual(1, created.Where(a => a.GetType() == typeof(PluginAssembly)).Count(), "1 Assembly");
            Assert.AreEqual(1, created.Where(a => a.GetType() == typeof(PluginType)).Count(), "1 Type");
            Assert.AreEqual(4, created.Where(a => a.GetType() == typeof(SdkMessageProcessingStep)).Count(), "4 Steps");
            var step1 = created.Where(a => a.GetType() == typeof(SdkMessageProcessingStep)).FirstOrDefault().ToEntity<SdkMessageProcessingStep>();
            Assert.AreEqual(step1.Name, "Pre-Update of account", "Name check");
            #endregion

        }
        [TestMethod]
        [TestCategory("Unit Tests")]
        public void RemoveExistingAttributes()
        {


            var parser = new CodeParser(TestCode.CodeSnipToRemoveAdd);
            parser.RemoveExistingAttributes();

            // Check no spaces are left between class and comments
            var correct1 = @"    /// </summary>    
    public class Testplugin : Plugin";
            var contains = parser.Code.Contains(correct1);
            Debug.WriteLine(parser.Code);
            Assert.IsTrue(contains, "Incorrect spaces after remove");

            var attribute = new CrmPluginRegistrationAttribute(
                                 MessageNameEnum.Update, "account", StageEnum.PostOperation, ExecutionModeEnum.Synchronous,
                                 "name,address1_line1",
                                 "Delete of account", 1, IsolationModeEnum.Sandbox)
            {
                Image1Name = "PreImage",
                Image1Attributes = "name,address1_line1",
                Image1Type = ImageTypeEnum.PreImage,
                Image2Name = "PostImage",
                Image2Attributes = "name,address1_line1",
                Image2Type = ImageTypeEnum.PostImage

            };
            parser.AddAttribute(attribute, "TestPlugins.Testplugin");

            Debug.WriteLine(parser.Code);

            var correct2 = @"    [CrmPluginRegistration(""Update"", 
    ""account"", StageEnum.PostOperation, ExecutionModeEnum.Synchronous,
    ""name,address1_line1"",""Delete of account"", 1, 
    IsolationModeEnum.Sandbox 
    ,Image1Type = ImageTypeEnum.PreImage
    ,Image1Name = ""PreImage""
    ,Image1Attributes = ""name,address1_line1""
    ,Image2Type = ImageTypeEnum.PostImage
    ,Image2Name = ""PostImage""
    ,Image2Attributes = ""name,address1_line1"" 
    )]
    public class Testplugin : Plugin";
            var contains2 = parser.Code.Contains(correct2);

            Assert.IsTrue(contains2, "Incorrect spaces after insert");

            parser.RemoveExistingAttributes();
            var contains3 = parser.Code.Contains(correct1);
            Assert.IsTrue(contains, "Incorrect spaces after remove again");

            parser.AddAttribute(attribute, "TestPlugins.Testplugin");

            var contains4 = parser.Code.Contains(correct2);

            Assert.IsTrue(contains2, "Incorrect spaces after insert again");

        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void ParsePluginCode()
        {
            var parser = new CodeParser(TestCode.DecoratedPlugin);
            Assert.AreEqual(parser.PluginCount, 1);

            // find existing attributes and remove
            Assert.AreEqual(2, parser.RemoveExistingAttributes());

            // Remove Attributes again - none should be removed
            Assert.AreEqual(0, parser.RemoveExistingAttributes());

            var attribute = new CrmPluginRegistrationAttribute(
                                  MessageNameEnum.Update, "account", StageEnum.PostOperation, ExecutionModeEnum.Synchronous,
                                  "name,address1_line1",
                                  "Delete of account", 1, IsolationModeEnum.Sandbox)
            {
                Image1Name = "PreImage",
                Image1Attributes = "name,address1_line1",
                Image1Type = ImageTypeEnum.PreImage,
                Image2Name = "PostImage",
                Image2Attributes = "name,address1_line1",
                Image2Type = ImageTypeEnum.PostImage

            };

            var className = "TestPlugin.Plugins.PreValidateaccountUpdate";
            Assert.AreEqual(true, parser.ClassNames.Contains(className));
            // Add in a new attribute
            parser.AddAttribute(attribute, className);
            parser.AddAttribute(attribute, className);

            // Remove Attributes again - 2 should be removed
            Assert.AreEqual(2, parser.RemoveExistingAttributes());
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void ParsePluginCodeWithCustomBaseClass()
        {
            string regex = @"((public( sealed)? class (?'class'[\w]*)[\W]*?)((?'plugin':[\W]*?((IPlugin)|(PluginBase)|(Plugin)))|(?'wf':[\W]*?((CodeActivity)|(WorkFlowActivityBase)))))";
            var parser = new CodeParser(TestCode.CustomBaseClass, regex);

            Assert.AreEqual(1, parser.PluginCount);
            Assert.AreEqual(true, parser.IsWorkflowActivity(parser.ClassNames[0]));


        }
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DownloadPluginMetadata()
        {

            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DownloadPluginMetadataTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                 @"..\..\..\TestPlugin");

            task.Execute(path);
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DownloadWorkflowMetadata()
        {

            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DownloadPluginMetadataTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                   @"..\..\..\TestWorkflowActivity");

            task.Execute(path);
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DownloadWebresourceConfig()
        {

            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DownloadWebresourceConfigTask(crmSvc, trace);
            task.Prefix = "new_";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..");

            task.Execute(path);
        }
        private string Normalise(string value)
        {
            return value.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
        }
    }
}
