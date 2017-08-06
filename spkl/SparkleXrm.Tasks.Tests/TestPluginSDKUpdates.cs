using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using System.IO;
using Microsoft.Xrm.Sdk.Client;
using System.Linq;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class TestPluginSDKUpdates
    {
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestUpdateFilteringAttributes()
        {
            // Check that updating a plugin step updates the filtering attributes
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();

            // Find an update plugin step
            using (var ctx = new OrganizationServiceContext(crmSvc))
            {
                ctx.MergeOption = MergeOption.NoTracking;

                var step = (from s in ctx.CreateQuery<SdkMessageProcessingStep>()
                            join m in ctx.CreateQuery<SdkMessageFilter>()
                            on s.SdkMessageFilterId.Id equals m.SdkMessageFilterId
                            where s.Name == "Update Step"
                            && m.PrimaryObjectTypeCode == "account"
                            select s).FirstOrDefault();
                try
                {

                    // Check the filtering attributes
                    Assert.AreEqual("name,address1_line1", step.FilteringAttributes);

                    // Update
                    step.FilteringAttributes = "name,address1_line2";
                    crmSvc.Update(step);

                    // Get again and check
                    var step2 = (from s in ctx.CreateQuery<SdkMessageProcessingStep>()
                                 join m in ctx.CreateQuery<SdkMessageFilter>()
                                 on s.SdkMessageFilterId.Id equals m.SdkMessageFilterId
                                 where s.Name == "Update Step"
                                 && m.PrimaryObjectTypeCode == "account"
                                 select s).FirstOrDefault();

                    Assert.AreEqual("name,address1_line2", step2.FilteringAttributes);
                }
                finally
                {
                    // Revert back
                    step.FilteringAttributes = "name,address1_line1";
                    crmSvc.Update(step);
                }
            }
        }
    }
}
