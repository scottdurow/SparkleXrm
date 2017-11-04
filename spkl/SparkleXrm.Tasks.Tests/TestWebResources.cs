using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Tooling.Connector;
using System.IO;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using FakeItEasy;

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

            #region Arrange
            ServiceLocator.Init();
            var trace = new TraceLogger();
            List<Entity> created = new List<Entity>();
            int publishCount = 0;
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
                        case WebResource.EntityLogicalName:

                            break;
                    }

                    var response = new RetrieveMultipleResponse();
                    response.Results["EntityCollection"] = new EntityCollection(results);

                    return response;

                }
                else if (request.GetType() == typeof(PublishXmlRequest))
                {
                    publishCount++;
                    return new PublishXmlResponse();
                }
                else
                {
                    throw new Exception("Unexpected Call");
                }
            });

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
            config.webresources[0].files.Add(new WebResourceFile()
            {
                file = @"new_\page.htm",
                displayname = @"new_/page.htm"
            });
            config.webresources[0].files.Add(new WebResourceFile()
            {
                file = @"new_\script.js",
                displayname = @"new_/script.js"
            });
            using (var ctx = new OrganizationServiceContext(service))
            {
                var webresourceDeploy = new DeployWebResourcesTask(service, trace);
                var guids = webresourceDeploy.DeployWebresources(ctx, config, config.webresources[0]);
                webresourceDeploy.PublishWebresources(guids);
            }
            #endregion

            #region Assert
            Assert.AreEqual(2, created.Where(a => a.GetType() == typeof(WebResource)).Count(), "2 Webresources created");
            Assert.AreEqual((int)WebResourceWebResourceType.Webpage_HTML, created[0].ToEntity<WebResource>().WebResourceType.Value, "html file");
            Assert.AreEqual(@"new_/page.htm", created[0].ToEntity<WebResource>().DisplayName, "html display name");
            Assert.AreEqual(@"new_/page.htm", created[0].ToEntity<WebResource>().Name, "html name");
            Assert.AreEqual((int)WebResourceWebResourceType.Script_JScript, created[1].ToEntity<WebResource>().WebResourceType.Value, "javascript file");
            Assert.AreEqual(@"new_/script.js", created[1].ToEntity<WebResource>().DisplayName, "javascript display name");
            Assert.AreEqual(@"new_/script.js", created[1].ToEntity<WebResource>().Name, "javascript name");
            Assert.AreEqual(1, publishCount, "files published");

            #endregion
        }


    }

}
