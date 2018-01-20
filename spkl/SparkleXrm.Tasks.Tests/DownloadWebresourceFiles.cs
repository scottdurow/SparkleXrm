using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using SparkleXrm.Tasks.Config;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.Text;
using Microsoft.Xrm.Tooling.Connector;
using System.IO;
using System.Configuration;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class DownloadWebresourceFilesTest
    {
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestDownloadFiles()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DownloadWebresourceFileTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Webresources");

            task.Execute(path);
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void TestDownloadnNewFiles()
        {
            // Test that the files are downloaded and added to the spkl.json
            // as well as saved to disc
            // Arrange
            var config = A.Fake<ConfigFile>(a => a.CallsBaseMethods());
            config.filePath = @"C:\tests\";
            config.solutions = new List<SolutionPackageConfig>()
                        {
                            new SolutionPackageConfig()
                            {
                                solution_uniquename="testsolution",
                                map = new List<SolutionPackageMap>()
                                {
                                    new SolutionPackageMap()
                                    {
                                        map = MapTypes.path,
                                        from = "WebResources\\*.*",
                                        to = "WebResourcesFiles\\**"
                                    }
                                }
                            }
                        };

            IDirectoryService directoryService;
            IOrganizationService service;
            TraceLogger trace;
            Arrange(config, out directoryService, out service, out trace);

            A.CallTo(() => directoryService.SaveFile(
                A<string>.Ignored,
                A<byte[]>.Ignored,
                A<bool>.Ignored)).Invokes((string name, byte[] content, bool overwrite) =>
                {
                    Assert.AreEqual(@"C:\tests\WebResourcesFiles\new_\foo\bar.js", name);
                });

            A.CallTo(() => service.Execute(A<OrganizationRequest>.Ignored))
                .ReturnsLazily((OrganizationRequest request) =>
                {
                    var query = request as RetrieveMultipleRequest;
                    Assert.IsNotNull(query);
                    var expression = query.Query as QueryExpression;
                    Assert.IsNotNull(expression);
                    Assert.AreEqual(WebResource.EntityLogicalName, expression.EntityName);

                    var list = new List<Entity>()
                    {
                        new WebResource
                        {
                            Name = @"new_\foo\bar.js",
                            Description = @"something",
                            DisplayName = @"new_\foo\bar.js",
                            Content = Convert.ToBase64String(Encoding.ASCII.GetBytes("test"))
                        }
                    };

                    var response = new RetrieveMultipleResponse();
                    response.Results["EntityCollection"] = new EntityCollection(list);
                    response.ResponseName = "RetrieveMultipleResponse";
                    return response;
                });

            // Act
            var task = new DownloadWebresourceFileTask(service, trace);
            task.Execute(config.filePath);

            // Assert
            Assert.AreEqual(1, config.webresources[0].files.Count);
            Assert.AreEqual(@"new_\foo\bar.js", config.webresources[0].files[0].uniquename);
            Assert.AreEqual(@"new_\foo\bar.js", config.webresources[0].files[0].displayname);
            Assert.AreEqual(@"WebResourcesFiles\new_\foo\bar.js", config.webresources[0].files[0].file);
            A.CallTo(() => config.Save()).MustHaveHappened(Repeated.Exactly.Once);
        }

        private static void Arrange(ConfigFile config, out IDirectoryService directoryService, out IOrganizationService service, out TraceLogger trace)
        {
            // Arrange Config
            var configService = A.Fake<IConfigFileService>(a => a.Strict());
            ServiceLocator.ServiceProvider.RemoveService(typeof(IConfigFileService));
            ServiceLocator.ServiceProvider.AddService(typeof(IConfigFileService), configService);

            A.CallTo(() => config.Save()).DoesNothing();
            A.CallTo(() => configService.FindConfig(A<string>.Ignored, A<bool>.Ignored))
                .Returns(
                new List<ConfigFile>() { config }
                );

            // Arrange Directory Service
            directoryService = A.Fake<IDirectoryService>(a => a.Strict());
            ServiceLocator.ServiceProvider.RemoveService(typeof(IDirectoryService));
            ServiceLocator.ServiceProvider.AddService(typeof(IDirectoryService), directoryService);

            // Arrange Org Service
            service = A.Fake<IOrganizationService>(a => a.Strict());
            ServiceLocator.ServiceProvider.RemoveService(typeof(IOrganizationService));
            ServiceLocator.ServiceProvider.AddService(typeof(IOrganizationService), service);

            // Arrange trace
            trace = new TraceLogger();
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void TestDownloadExistingFilesNoRoot()
        {
            // Test that when we have existing webresources, with no root folder setting
            // there are no new files added
            var config = A.Fake<ConfigFile>(a => a.CallsBaseMethods());
            config.filePath = @"C:\tests\another";
            config.solutions = new List<SolutionPackageConfig>()
                        {
                            new SolutionPackageConfig()
                            {
                                solution_uniquename="testsolution",
                                map = new List<SolutionPackageMap>()
                                {
                                    new SolutionPackageMap()
                                    {
                                        map = MapTypes.path,
                                        from = "WebResources\\*.*",
                                        to = "WebResourcesFiles\\**"
                                    }
                                }
                            }
                        };
            config.webresources = new List<WebresourceDeployConfig>()
            {
                new WebresourceDeployConfig()
                {
                    files = new List<WebResourceFile>()
                    {
                        new WebResourceFile()
                        {
                            uniquename = @"new_\foo\bar.js",
                            displayname = @"new_\foo\bar.js",
                            file = @"WebResourcesFiles\new_\foo\bar.js"
                        }
                    }
                }

            };

            IDirectoryService directoryService;
            IOrganizationService service;
            TraceLogger trace;
            Arrange(config, out directoryService, out service, out trace);

            A.CallTo(() => directoryService.SaveFile(
                A<string>.Ignored,
                A<byte[]>.Ignored,
                A<bool>.Ignored)).Invokes((string name, byte[] content, bool overwrite) =>
                {
                    Assert.AreEqual(@"C:\tests\another\WebResourcesFiles\new_\foo\bar.js", name);
                });

            A.CallTo(() => service.Execute(A<OrganizationRequest>.Ignored))
                .ReturnsLazily((OrganizationRequest request) =>
                {
                    var query = request as RetrieveMultipleRequest;
                    Assert.IsNotNull(query);
                    var expression = query.Query as QueryExpression;
                    Assert.IsNotNull(expression);
                    Assert.AreEqual(WebResource.EntityLogicalName, expression.EntityName);

                    var list = new List<Entity>()
                    {
                        new WebResource
                        {
                            Name = @"new_\foo\bar.js",
                            Description = @"something",
                            DisplayName = @"new_\foo\bar.js",
                            Content = Convert.ToBase64String(Encoding.ASCII.GetBytes("test"))
                        }
                    };

                    var response = new RetrieveMultipleResponse();
                    response.Results["EntityCollection"] = new EntityCollection(list);
                    response.ResponseName = "RetrieveMultipleResponse";
                    return response;
                });

            // Act
            var task = new DownloadWebresourceFileTask(service, trace);
            task.Execute(config.filePath);

            // Assert
            // There shouldn't be any additional files added because they were already there
            Assert.AreEqual(1, config.webresources[0].files.Count);
            Assert.AreEqual(@"new_\foo\bar.js", config.webresources[0].files[0].uniquename);
            Assert.AreEqual(@"new_\foo\bar.js", config.webresources[0].files[0].displayname);
            Assert.AreEqual(@"WebResourcesFiles\new_\foo\bar.js", config.webresources[0].files[0].file);
            A.CallTo(() => config.Save()).MustHaveHappened(Repeated.Exactly.Once);

        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void TestDownloadExistingFilesWithRoot()
        {
            // Test that if we are using a root folder path in the spkl.json then 
            // it is used when adding the file locations
            // Also check that the spkl.json isn't added to if the files are there already
            var config = A.Fake<ConfigFile>(a => a.CallsBaseMethods());
            config.filePath = @"C:\tests\another";
            config.solutions = new List<SolutionPackageConfig>()
                        {
                            new SolutionPackageConfig()
                            {
                                solution_uniquename="testsolution",
                                map = new List<SolutionPackageMap>()
                                {
                                    new SolutionPackageMap()
                                    {
                                        map = MapTypes.path,
                                        from = "WebResources\\*.*",
                                        to = "WebResourcesFiles\\**"
                                    }
                                }
                            }
                        };
            config.webresources = new List<WebresourceDeployConfig>()
            {
                new WebresourceDeployConfig()
                {
                    root = "root_folder",
                    files = new List<WebResourceFile>()
                    {
                        new WebResourceFile()
                        {
                            uniquename = @"new_\foo\bar.js",
                            displayname = @"new_\foo\bar.js",
                            file = @"WebResourcesFiles\new_\foo\bar.js"
                        }
                    }
                }

            };

            IDirectoryService directoryService;
            IOrganizationService service;
            TraceLogger trace;
            Arrange(config, out directoryService, out service, out trace);

            A.CallTo(() => directoryService.SaveFile(
                A<string>.Ignored,
                A<byte[]>.Ignored,
                A<bool>.Ignored)).Invokes((string name, byte[] content, bool overwrite) =>
                {
                    Assert.AreEqual(@"C:\tests\another\root_folder\WebResourcesFiles\new_\foo\bar.js", name);
                });

            A.CallTo(() => service.Execute(A<OrganizationRequest>.Ignored))
                .ReturnsLazily((OrganizationRequest request) =>
                {
                    var query = request as RetrieveMultipleRequest;
                    Assert.IsNotNull(query);
                    var expression = query.Query as QueryExpression;
                    Assert.IsNotNull(expression);
                    Assert.AreEqual(WebResource.EntityLogicalName, expression.EntityName);

                    var list = new List<Entity>()
                    {
                        new WebResource
                        {
                            Name = @"new_\foo\bar.js",
                            Description = @"something",
                            DisplayName = @"new_\foo\bar.js",
                            Content = Convert.ToBase64String(Encoding.ASCII.GetBytes("test"))
                        }
                    };

                    var response = new RetrieveMultipleResponse();
                    response.Results["EntityCollection"] = new EntityCollection(list);
                    response.ResponseName = "RetrieveMultipleResponse";
                    return response;
                });

            // Act
            var task = new DownloadWebresourceFileTask(service, trace);
            task.Execute(config.filePath);

            // Assert
            // There shouldn't be any additional files added because they were already there
            Assert.AreEqual(1, config.webresources[0].files.Count);
            Assert.AreEqual(@"new_\foo\bar.js", config.webresources[0].files[0].uniquename);
            Assert.AreEqual(@"new_\foo\bar.js", config.webresources[0].files[0].displayname);
            Assert.AreEqual(@"WebResourcesFiles\new_\foo\bar.js", config.webresources[0].files[0].file);
            A.CallTo(() => config.Save()).MustHaveHappened(Repeated.Exactly.Once);

        }
    }
}
