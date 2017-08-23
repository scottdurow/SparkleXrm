using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;
using Microsoft.Xrm.Tooling.Connector;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;

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
        public void TestGetPluginMetadata()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var currentAssemblyPath = Path.GetDirectoryName(path);

            // Load this assembly
            Assembly thisAssembly = Reflection.ReflectionOnlyLoadAssembly(@"C:\Repos\SparkleXRM\spkl\TestPlugin\bin\Debug\TestPlugin.dll");
            IEnumerable<Type> pluginTypes = Reflection.GetTypesImplementingInterface(thisAssembly, typeof(Microsoft.Xrm.Sdk.IPlugin));
            var attributes = Reflection.GetAttributes(pluginTypes.Where(t => t.Name == "PreValidateaccountUpdate"), typeof(CrmPluginRegistrationAttribute).Name);
            var pluginStep = (CrmPluginRegistrationAttribute)attributes.Where(s=>s.ConstructorArguments[5].Value.ToString()=="Create Step").First().CreateFromData();
            Assert.AreEqual("Description", pluginStep.Description);
            Assert.AreEqual("Some config", pluginStep.UnSecureConfiguration);
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void DuplicatePluginNameOnDownload()
        {
            // Since the name is used to uniquely identify plugins per type, we can't have existing duplicates when downloading steps

            using (ShimsContext.Create())
            {
                // Arrange
                Fakes.ShimQueries.GetPluginStepsOrganizationServiceContextString = (OrganizationServiceContext context, string name) =>
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
                };

                var trace = new TraceLogger();
                OrganizationServiceContext ctx = new Microsoft.Xrm.Sdk.Client.Fakes.ShimOrganizationServiceContext();

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
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
               @"..\..\..\TestPlugin");
            var configs = ConfigFile.FindConfig(path);

            var config = configs[0];
            // Get plugin config
            var defaultPluginConfig = config.GetPluginsConfig("default");
            var debugConfig = config.GetPluginsConfig("debug");
            #endregion

            #region Act
            var defaultAssemblies = ConfigFile.GetAssemblies(config, defaultPluginConfig[0]);
            var debugAssemblies = ConfigFile.GetAssemblies(config, debugConfig[0]);

            #endregion

            #region Assert
            // The wildcard should return all 4 assemblies
            Assert.AreEqual(4, defaultAssemblies.Count, "The wildcard should return all 4 assemblies");
            // The specific assembly name should only return 1
            Assert.AreEqual(1, debugAssemblies.Count, "The specific assembly name should only return 1");
            #endregion


        }

        [CrmPluginRegistrationAttribute("step","step","step","step",IsolationModeEnum.Sandbox)]
        [CrmPluginRegistrationAttribute("step", "step", "step", "step", IsolationModeEnum.Sandbox)]
        public class TestPluginWithDuplicateAttributes
        {

        }
    }
}
