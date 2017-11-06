using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xrm.Sdk;
using SparkleXrm.Tasks.Config;
using FakeItEasy;

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
            var config = A.Fake<ConfigFile>(a => a.Strict());

            config.webresources = new List<WebresourceDeployConfig>{
                                new WebresourceDeployConfig{
                                    root="webresources",
                                    files = new List<WebResourceFile>()
                                    }
                                };
            config.filePath = @"C:\code\solution";

            var queries = A.Fake<IQueries>(a=>a.Strict());
            ServiceLocator.ServiceProvider.RemoveService(typeof(IQueries));
            ServiceLocator.ServiceProvider.AddService(typeof(IQueries), queries);

            A.CallTo(() => config.GetWebresourceConfig(A<string>.Ignored))
                .WithAnyArguments()
                .Returns(config.webresources.ToArray());

            var existingWebresource = new WebResource
            {
                DisplayName = "new_/js/somefile.js",
                Name = "new_/js/somefile.js"
            };
            A.CallTo(() => config.Save()).DoesNothing();

            // Act
            var task = GetWebresourceTestTask(config, existingWebresource);

            // Assert
            // Check that there is a webresource matched with the correct path
            Assert.AreEqual(@"new_\js\somefile.js", config.webresources[0].files[0].file);
            Assert.AreEqual(@"new_/js/somefile.js", config.webresources[0].files[0].uniquename);
            A.CallTo(() => config.Save()).MustHaveHappened();
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void GetWebresourcesWithNoRootFolder()
        {
            // Arrange
            var config = A.Fake<ConfigFile>(a=>a.Strict());

            config.webresources = new List<WebresourceDeployConfig>{
                                new WebresourceDeployConfig{
                                    root=null,
                                    files = new List<WebResourceFile>()
                                    }
                                };
            config.filePath = @"C:\code\solution";
            A.CallTo(() => config.Save()).DoesNothing();
            A.CallTo(() => config.GetWebresourceConfig(A<string>.Ignored))
                .Returns(config.webresources.ToArray());

            var existingWebresource = new WebResource
            {
                DisplayName = "new_/js/somefile.js",
                Name = "new_/js/somefile.js"
            };

            var queries = A.Fake<IQueries>(a => a.Strict());
            ServiceLocator.ServiceProvider.RemoveService(typeof(IQueries));
            ServiceLocator.ServiceProvider.AddService(typeof(IQueries), queries);



            // Act
            var task = GetWebresourceTestTask(config, existingWebresource);

            // Assert
            // Check that there is a webresource matched with the correct path
            Assert.AreEqual(@"webresources\new_\js\somefile.js", config.webresources[0].files[0].file);
            Assert.AreEqual(@"new_/js/somefile.js", config.webresources[0].files[0].uniquename);
            A.CallTo(() => config.Save()).MustHaveHappened();
        }
        private static DownloadWebresourceConfigTask GetWebresourceTestTask(ConfigFile config, WebResource existingWebresource)
        {

            // Arrange
            OrganizationServiceContext ctx = A.Fake<OrganizationServiceContext>();

            A.CallTo(()=> ServiceLocator.Queries.GetWebresources(ctx))
                .ReturnsLazily(() =>
            {
                return new List<WebResource>
                {
                    existingWebresource
                };

            });

            var configFactoryInstance = A.Fake<IConfigFileService>();
            ServiceLocator.ServiceProvider.RemoveService(typeof(IConfigFileService));
            ServiceLocator.ServiceProvider.AddService(typeof(IConfigFileService), configFactoryInstance);
           
            A.CallTo(() => configFactoryInstance.FindConfig(A<string>.Ignored, A<bool>.Ignored))
                .WithAnyArguments()
                .Returns(new List<ConfigFile>
                {
                    config
                });


            var directoryService = A.Fake<IDirectoryService>();
            ServiceLocator.ServiceProvider.RemoveService(typeof(IDirectoryService));
            ServiceLocator.ServiceProvider.AddService(typeof(IDirectoryService), directoryService);
            A.CallTo(()=>directoryService.Search(A<string>.Ignored,A<string>.Ignored))
                .WithAnyArguments()
                .Returns(
                new List<string>()
                {
                    @"C:\code\solution\webresources\new_\js\somefile.js"
                }
            );

            var trace = new TraceLogger();
            
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
