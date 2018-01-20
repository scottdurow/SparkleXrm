using Microsoft.Crm.Sdk.Messages;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SparkleXrm.Tasks.Tests
{
    /// <summary>
    /// Summary description for TestWebResources
    /// </summary>
    [TestClass]
    public class TestWebResources
    {
        public TestWebResources()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestDeployWebresources()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            var task = new DeployWebResourcesTask(crmSvc, trace);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                 @"..\..\..\Webresources");

            task.Execute(path);

        }


        [TestMethod]
        [TestCategory("Unit Tests")]
        public void DeployHtmlJSWebresources()
        {
            using (ShimsContext.Create())
            {
                #region Arrange
                Fakes.ShimQueries.GetPluginStepsOrganizationServiceContextString = (OrganizationServiceContext context, string name) =>
                {
                    // Return no existing steps
                    return new List<SdkMessageProcessingStep>();
                };

                var trace = new TraceLogger();
                List<Entity> created = new List<Entity>();
                List<Entity> updated = new List<Entity>();
                int publishCount = 0;
                IOrganizationService service = new Microsoft.Xrm.Sdk.Fakes.StubIOrganizationService()
                {
                    CreateEntity = delegate (Entity entity)
                    {
                        created.Add(entity);

                        return Guid.NewGuid();
                    },

                    //for dependency test
                    UpdateEntity = delegate (Entity entity)
                    {
                        updated.Add(entity);
                    },

                    ExecuteOrganizationRequest = delegate (OrganizationRequest request)
                    {

                        if (request.GetType() == typeof(RetrieveMultipleRequest))
                        {
                            var query = request as RetrieveMultipleRequest;
                            var queryExpression = query.Query as QueryExpression;
                            var results = new List<Entity>();

                            switch (queryExpression.EntityName)
                            {
                                case WebResource.EntityLogicalName:

                                    break;
                            }

                            return new Microsoft.Xrm.Sdk.Messages.Fakes.ShimRetrieveMultipleResponse()
                            {
                                EntityCollectionGet = delegate () { return new EntityCollection(results); }
                            };
                        }
                        else if (request.GetType() == typeof(PublishXmlRequest))
                        {
                            publishCount++;
                            return new PublishXmlResponse();
                        }
                        else if (request.GetType() == typeof(RetrieveAttributeRequest))
                        {
                            return new Microsoft.Xrm.Sdk.Messages.Fakes.ShimRetrieveAttributeResponse()
                            {
                                AttributeMetadataGet = delegate () {
                                    return new AttributeMetadata()
                                    {
                                        LogicalName = "accountid",
                                        MetadataId = new Guid(),
                                    };
                                }
                            };
                        }
                        else
                        {
                            throw new Exception("Unexpected Call");
                        }
                    }
                };
                #endregion 

                #region Act

                var config = new ConfigFile();
                Guid id = Guid.NewGuid();
                var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
                Directory.CreateDirectory(tempFolder);

                config.filePath = tempFolder;

                Directory.CreateDirectory(Path.Combine(tempFolder, @"new_"));
                // Create an html webresorce
                File.WriteAllText(Path.Combine(tempFolder, @"new_\page.htm"), @"<html><body>test</body></html>");

                /// Create a js webresource
                File.WriteAllText(Path.Combine(tempFolder, @"new_\script.js"), @"<html><body>test</body></html>");

                config.webresources = new List<WebresourceDeployConfig>();
                config.webresources.Add(new WebresourceDeployConfig
                {
                    files = new List<WebResourceFile>()
                });

                //library dependencies
                var libraryDependencies = new List<LibraryDependency>();
                libraryDependencies.Add(new LibraryDependency() { uniquename = "new_/js/contact.js", file = @"new_\script.js", description = "dd", displayname = @"new_/script.js" });

                //attribute dependencies
                var attributeDependencies = new List<AttributeDependency>();
                attributeDependencies.Add(new AttributeDependency() { attributename = "accountid", entityname = "account" });

                config.webresources[0].files.Add(new WebResourceFile()
                {
                    file = @"new_\page.htm",
                    displayname = @"new_/page.htm",
                    dependencies = new WebresourceDependencies() { libraries = libraryDependencies }
                });
                config.webresources[0].files.Add(new WebResourceFile()
                {
                    file = @"new_\script.js",
                    displayname = @"new_/script.js",
                    dependencies = new WebresourceDependencies() { libraries = libraryDependencies, attributes = attributeDependencies }
                });
                using (var ctx = new OrganizationServiceContext(service))
                {
                    var webresourceDeploy = new DeployWebResourcesTask(service, trace);
                    var guids = webresourceDeploy.DeployWebresources(ctx, config, config.webresources[0]);
                    webresourceDeploy.PublishWebresources(guids);
                }
                #endregion

                #region Assert
                //test dependency
                Assert.IsNotNull(updated[0].ToEntity<WebResource>().DependencyXml);
                StringAssert.Contains(updated[1].ToEntity<WebResource>().DependencyXml, "attributeId=\"00000000-0000-0000-0000-000000000000\"");

                Assert.AreEqual(4, created.Where(a => a.GetType() == typeof(WebResource)).Count(), "4 Webresources created"); //changed to 4, because I create 1 js file for dependency test
                Assert.AreEqual((int)WebResourceWebResourceType.Webpage_HTML, created[1].ToEntity<WebResource>().WebResourceType.Value, "html file");
                Assert.AreEqual(@"new_/page.htm", created[1].ToEntity<WebResource>().DisplayName, "html display name");
                Assert.AreEqual(@"new_/page.htm", created[1].ToEntity<WebResource>().Name, "html name");
                Assert.AreEqual((int)WebResourceWebResourceType.Script_JScript, created[3].ToEntity<WebResource>().WebResourceType.Value, "javascript file"); //changed to created[3], because I create 1 js file for dependency test
                Assert.AreEqual(@"new_/script.js", created[3].ToEntity<WebResource>().DisplayName, "javascript display name"); //changed to created[3], because I create 1 js file for dependency test
                Assert.AreEqual(@"new_/script.js", created[3].ToEntity<WebResource>().Name, "javascript name"); //changed to created[3], because I create 1 js file for dependency test
                Assert.AreEqual(1, publishCount, "files published");

                #endregion
            }


        }
    }
}
