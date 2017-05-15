using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xrm.Sdk;
using SparkleXrm.Tasks.Config;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class GetWebresources
    {
        [TestMethod]
        [TestCategory("Unit Tests")]
        public void GetWebresourcesWithRootFolder()
        {
            // Arrange
            var config = new ConfigFile
            {
                webresources = new List<WebresourceDeployConfig>{
                                new WebresourceDeployConfig{
                                    root="webresources",
                                    files = new List<WebResourceFile>()
                                    }
                                },
                filePath = @"C:\code\solution"
            };

            var existingWebresource = new WebResource
            {
                DisplayName = "new_/js/somefile.js",
                Name = "new_/js/somefile.js"
            };

            // Act
            var task = GetWebresourceTestTask(config, existingWebresource);

            // Assert
            // Check that there is a webresource matched with the correct path
            Assert.AreEqual(@"new_\js\somefile.js", config.webresources[0].files[0].file);
            Assert.AreEqual(@"new_/js/somefile.js", config.webresources[0].files[0].uniquename);
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void GetWebresourcesWithNoRootFolder()
        {
            // Arrange
            var config = new ConfigFile
            {
                webresources = new List<WebresourceDeployConfig>{
                                new WebresourceDeployConfig{
                                    root=null,
                                    files = new List<WebResourceFile>()
                                    }
                                },
                filePath = @"C:\code\solution"
            };

            var existingWebresource = new WebResource
            {
                DisplayName = "new_/js/somefile.js",
                Name = "new_/js/somefile.js"
            };

            // Act
            var task = GetWebresourceTestTask(config, existingWebresource);

            // Assert
            // Check that there is a webresource matched with the correct path
            Assert.AreEqual(@"webresources\new_\js\somefile.js", config.webresources[0].files[0].file);
            Assert.AreEqual(@"new_/js/somefile.js", config.webresources[0].files[0].uniquename);
        }
        private static DownloadWebresourceConfigTask GetWebresourceTestTask(ConfigFile config, WebResource existingWebresource)
        {
            using (ShimsContext.Create())
            {
                // Arrange
                Fakes.ShimQueries.GetWebresourcesOrganizationServiceContext = (OrganizationServiceContext context) =>
                {
                    return new List<WebResource>
                    {
                        existingWebresource
                    };

                };

                SparkleXrm.Tasks.Config.Fakes.ShimConfigFile.FindConfigStringBoolean = (string folder, bool raiseEror) =>
                {
                    return new List<ConfigFile>
                    {
                        config
                    };
                };

                SparkleXrm.Tasks.Fakes.ShimDirectoryEx.SearchStringStringListOfString = (string folder, string search, List<string> paths) =>
                {
                    return new List<string>()
                    {
                        @"C:\code\solution\webresources\new_\js\somefile.js"
                    };
                };

                SparkleXrm.Tasks.Config.Fakes.ShimConfigFile.AllInstances.Save = (ConfigFile c) => { };

                var trace = new TraceLogger();
                OrganizationServiceContext ctx = new Microsoft.Xrm.Sdk.Client.Fakes.ShimOrganizationServiceContext();

                var task = new DownloadWebresourceConfigTask(ctx, trace);
                task.Prefix = "new";

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                     @"..\..\..\SparkleXrm.Tasks.Tests\Resources");
                // Act
                task.Execute(path);
                return task;
            }
        }
    }
}
