using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;
using Microsoft.Xrm.Tooling.Connector;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using FakeItEasy;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class DeployPluginTests
    {
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DeployPlugins()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DeployPluginsTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\TestPlugin");

            task.Execute(path);
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void DeployWorkflowActivities()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DeployWorkflowActivitiesTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                 @"..\..\..\TestWorkflowActivity");

            task.Execute(path);
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void TestGetWorkflowActivityMetadata()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var currentAssemblyPath = Path.GetDirectoryName(path);

            // Load this assembly
            var testAssemblyPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
              @"..\..\..\TestWorkflowActivity");

            var assemblyPath = new DirectoryService().SimpleSearch(testAssemblyPathRoot, "TestWorkflowActivity.dll");

            Assembly thisAssembly = Reflection.LoadAssembly(assemblyPath);
            IEnumerable<Type> pluginTypes = Reflection.GetTypesInheritingFrom(thisAssembly, typeof(System.Activities.CodeActivity));
            var workflowActivityCustomBaseClass = Reflection.GetAttributes(pluginTypes.Where(t => t.Name == "WorkflowActivityInheritingFromWorkflowActivityBase"), typeof(CrmPluginRegistrationAttribute).Name);
            Assert.AreEqual(1, workflowActivityCustomBaseClass.Count(), "Custom Base Class Metadata");

            var codeActivityBaseClass = Reflection.GetAttributes(pluginTypes.Where(t => t.Name == "WorkflowActivity"), typeof(CrmPluginRegistrationAttribute).Name);
            Assert.AreEqual(1, codeActivityBaseClass.Count(), "CodeActiviy Base Class Metadata");
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void TestGetPluginMetadata()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var currentAssemblyPath = Path.GetDirectoryName(path);

            // Load this assembly
            var testAssemblyPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
              @"..\..\..\TestPlugin");

            var assemblyPath = new DirectoryService().SimpleSearch(testAssemblyPathRoot, "TestPlugin.dll");

            Assembly thisAssembly = Reflection.LoadAssembly(assemblyPath);
            IEnumerable<Type> pluginTypes = Reflection.GetTypesImplementingInterface(thisAssembly, typeof(Microsoft.Xrm.Sdk.IPlugin));
            var attributes = Reflection.GetAttributes(pluginTypes.Where(t => t.Name == "PreValidateaccountUpdate"), typeof(CrmPluginRegistrationAttribute).Name);
            var pluginStep = (CrmPluginRegistrationAttribute)attributes.Where(s => s.ConstructorArguments[5].Value.ToString() == "Create Step").First().CreateFromData();
            Assert.AreEqual("Description", pluginStep.Description);
            Assert.AreEqual("Some config", pluginStep.UnSecureConfiguration);
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void DuplicatePluginNameOnDownload()
        {
            // Since the name is used to uniquely identify plugins per type, we can't have existing duplicates when downloading steps

            // Arrange
            ServiceLocator.Init();
            var trace = new TraceLogger();

            var ctx = A.Fake<OrganizationServiceContext>(a => a.Strict());
            var queries = A.Fake<IQueries>();
            ServiceLocator.ServiceProvider.RemoveService(typeof(IQueries));
            ServiceLocator.ServiceProvider.AddService(typeof(IQueries), queries);

            A.CallTo(() => queries.GetPluginSteps(A<OrganizationServiceContext>.Ignored, A<string>.Ignored))
                .WithAnyArguments()
                .ReturnsLazily((OrganizationServiceContext context, string name) =>
                {
                    return new List<SdkMessageProcessingStep>()
                    {
                         new SdkMessageProcessingStep
                        {
                            Name = "step"
                        },
                        new SdkMessageProcessingStep
                        {
                            Name = "step"
                        }
                    };
                });

            var task = new DownloadPluginMetadataTask(ctx, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                 @"..\..\..\TestPlugin");

            bool exception = false;

            // Act
            try
            {
                task.Execute(path);
            }
            catch (SparkleTaskException ex)
            {
                exception = (ex.ExceptionType == SparkleTaskException.ExceptionTypes.DUPLICATE_STEP);
            }

            // Assert
            Assert.IsTrue(exception, "Duplicate step names not detected");
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void DuplicatePluginNameOnDeploy()
        {
            // Assemble
            List<Type> testType = new List<Type>()
            {
                typeof(TestPluginWithDuplicateAttributes)
            };
            bool exception = false;

            //Act
            try
            {
                var types = Reflection.GetAttributes(testType, typeof(CrmPluginRegistrationAttribute).Name).FirstOrDefault();
            }
            catch (SparkleTaskException ex)
            {
                exception = (ex.ExceptionType == SparkleTaskException.ExceptionTypes.DUPLICATE_STEP);
            }
            // Assert
            Assert.IsTrue(exception, "Duplicate step names not detected");
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void GetGetAssemblies()
        {
            #region Assemble

            ServiceLocator.Init();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
               @"..\..\..\TestPlugin");
            var configs = ServiceLocator.ConfigFileFactory.FindConfig(path);

            var config = configs[0];
            // Get plugin config
            var defaultPluginConfig = config.GetPluginsConfig("default");
            var debugConfig = config.GetPluginsConfig("debug");

            #endregion Assemble

            #region Act

            var defaultAssemblies = config.GetAssemblies(defaultPluginConfig[0]);
            var debugAssemblies = config.GetAssemblies(debugConfig[0]);

            #endregion Act

            #region Assert

            // The wildcard should return all 4 assemblies
            Assert.IsTrue(defaultAssemblies.Count > 1, "The wildcard should return all assemblies");
            // The specific assembly name should only return 1
            Assert.AreEqual(1, debugAssemblies.Count, "The specific assembly name should only return 1");

            #endregion Assert
        }

        [CrmPluginRegistrationAttribute("step", "step", "step", "step", IsolationModeEnum.Sandbox)]
        [CrmPluginRegistrationAttribute("step", "step", "step", "step", IsolationModeEnum.Sandbox)]
        public class TestPluginWithDuplicateAttributes
        {
        }
    }
}