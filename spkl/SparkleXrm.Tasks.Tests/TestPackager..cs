using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using SparkleXrm.Tasks.Config;
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
    public class TestPackager
    {

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void LoadMappingFile()
        {
            ServiceLocator.Init();
            var files = ServiceLocator.ConfigFileFactory.FindConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"..\.."), true);

            Assert.AreEqual(1, files.Count, "1 file found");

            // Get the mapping
            Assert.AreEqual(3,files[0].solutions[0].map.Count, "3 mappings");
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void UnpackSolution()
        {
            // Assemble
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    solutions = new List<SolutionPackageConfig>{
                                new SolutionPackageConfig{
                                    solution_uniquename = "spkltestsolution",
                                    packagepath = "packager"
                                    }
                                },
                    filePath = tempFolder
                };
                CreateSolution(crmSvc);
                // Create packaging task
                var task = new SolutionPackagerTask(crmSvc, trace);
                using (var ctx = new OrganizationServiceContext(crmSvc))
                {
                    ctx.MergeOption = MergeOption.NoTracking;
                    task.UnPack(ctx, config);
                }
                // Check that the account entity.xml is present in the output folder
                bool exists = File.Exists(Path.Combine(tempFolder, @"packager\Entities\Account\Entity.xml"));
                Assert.IsTrue(exists, "Account entity exists");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void Pack()
        {

            // Assemble
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    solutions = new List<SolutionPackageConfig>{
                                new SolutionPackageConfig{
                                    solution_uniquename = "spkltestsolution",
                                    packagepath = "packager",
                                    solutionpath = "Solution_{0}_{1}_{2}_{3}.zip",
                                    increment_on_import = true
                                    }
                                },
                    filePath = tempFolder
                };
                // Create packaging task
                var task = new SolutionPackagerTask(crmSvc, trace);
                using (var ctx = new OrganizationServiceContext(crmSvc))
                {
                    ctx.MergeOption = MergeOption.NoTracking;
                    task.UnPack(ctx, config);

                    task.Pack(ctx, config, false, false);

                    
                }
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void GetVersion()
        {
            var verison = new Version("1.2.3.4");
            Assert.AreEqual(1, verison.Major);
            Assert.AreEqual(2, verison.Minor);
            Assert.AreEqual(3, verison.Build);
            Assert.AreEqual(4, verison.Revision);
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void Pack_Upload()
        {
            
            // Assemble
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    solutions = new List<SolutionPackageConfig>{
                                new SolutionPackageConfig{
                                    solution_uniquename = "spkltestsolution",
                                    packagepath = "packager",
                                    increment_on_import = true
                                    }
                                },
                    filePath = tempFolder
                };
                // Create packaging task
                var task = new SolutionPackagerTask(crmSvc, trace);
                using (var ctx = new OrganizationServiceContext(crmSvc))
                {
                    ctx.MergeOption = MergeOption.NoTracking;
                    task.UnPack(ctx, config);

                    // Get current solution version
                    var version = task.GetSolution("spkltestsolution").Version;
                    task.Pack(ctx, config, true, true);
                    var versionAfterUpload = task.GetSolution("spkltestsolution").Version;
                    Assert.AreNotEqual(version, versionAfterUpload, "Version incremented");
                }
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void Pack_Upload_ReportError()
        {
            // Assemble
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    solutions = new List<SolutionPackageConfig>{
                                new SolutionPackageConfig{
                                    solution_uniquename = "spkltestsolution",
                                    packagepath = "packager",
                                    increment_on_import = true
                                    }
                                },
                    filePath = tempFolder
                };
                // Create packaging task
                var task = new SolutionPackagerTask(crmSvc, trace);
                using (var ctx = new OrganizationServiceContext(crmSvc))
                {
                    ctx.MergeOption = MergeOption.NoTracking;
                    task.UnPack(ctx, config);
                    // Delete the Account relationship
                    File.Delete(Path.Combine(config.filePath, @"packager\Other\Relationships\Account.xml"));
                    bool correctError = false;
                    try
                    {
                        var solutionZipTempPath = Path.GetTempFileName();
                        task.Pack(ctx, config, true, true);
                    }
                    catch (Exception ex)
                    {
                        // this is expected
                        correctError = ex.Message.Contains("The element 'EntityRelationship' has incomplete content.");
                    }
                    Assert.IsTrue(correctError, "Error reported");
                }
            }
            finally
            {
                try
                {
                    Directory.Delete(tempFolder, true);
                }
                catch
                {
                }
            }
        }

        private void CreateSolution(IOrganizationService service)
        {
            Guid DefaultPublisherId = new Guid("{d22aab71-79e7-11dd-8874-00188b01e34f}");

            //Define a new publisher
            Entity _crmSdkPublisher = new Entity("publisher");
            _crmSdkPublisher["uniquename"] = "splklsamples";
            _crmSdkPublisher["friendlyname"] = "splklsamples";
            _crmSdkPublisher["customizationprefix"] = "spkl";
            _crmSdkPublisher["publisherid"] = DefaultPublisherId;
            _crmSdkPublisher.Id = DefaultPublisherId;


            //Does publisher already exist?
            QueryExpression querySDKSamplePublisher = new QueryExpression
            {
                EntityName = "publisher",
                ColumnSet = new ColumnSet("publisherid", "customizationprefix"),
                Criteria = new FilterExpression()
            };

            querySDKSamplePublisher.Criteria.AddCondition("uniquename", ConditionOperator.Equal, _crmSdkPublisher.GetAttributeValue<string>("uniquename"));
            EntityCollection querySDKSamplePublisherResults = service.RetrieveMultiple(querySDKSamplePublisher);
            Entity SDKSamplePublisherResults = null;

            Guid publisherId = Guid.Empty;
            //If it already exists, use it
            if (querySDKSamplePublisherResults.Entities.Count > 0)
            {
                SDKSamplePublisherResults = querySDKSamplePublisherResults.Entities[0];
                publisherId = (Guid)SDKSamplePublisherResults.Id;

            }
            //If it doesn't exist, create it
            else if (SDKSamplePublisherResults == null)
            {
                publisherId = service.Create(_crmSdkPublisher);
         
            }

            // Create a Solution
            //Define a solution
            Solution solution = new Solution
            {
                UniqueName = "spkltestsolution",
                FriendlyName = "spkl Unit Solution",
                PublisherId = new EntityReference("publisher", publisherId),
                Version = "1.0"
            };

            //Check whether it already exists
            QueryExpression queryCheckForSampleSolution = new QueryExpression
            {
                EntityName = Solution.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria = new FilterExpression()
            };
            queryCheckForSampleSolution.Criteria.AddCondition("uniquename", ConditionOperator.Equal, solution.UniqueName);

            //Create the solution if it does not already exist.
            EntityCollection querySampleSolutionResults = service.RetrieveMultiple(queryCheckForSampleSolution);
            Solution SampleSolutionResults = null;
            Guid sampleSolutionId;
            if (querySampleSolutionResults.Entities.Count > 0)
            {
                SampleSolutionResults = (Solution)querySampleSolutionResults.Entities[0];
                sampleSolutionId = (Guid)SampleSolutionResults.SolutionId;
            }
            else if (SampleSolutionResults == null)
            {
                sampleSolutionId = service.Create(solution);
            }   

            //Add the Account entity to the solution
            RetrieveEntityRequest retrieveForAddAccountRequest = new RetrieveEntityRequest()
            {
                LogicalName = "account"
            };
            RetrieveEntityResponse retrieveForAddAccountResponse = (RetrieveEntityResponse)service.Execute(retrieveForAddAccountRequest);
            AddSolutionComponentRequest addReq = new AddSolutionComponentRequest()
            {
                ComponentType = (int)componenttype.Entity,
                ComponentId = (Guid)retrieveForAddAccountResponse.EntityMetadata.MetadataId,
                SolutionUniqueName = solution.UniqueName
            };
            service.Execute(addReq);

        }
    }
}
