using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class TestPackager
    {
     
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void ExtractSolution_Simple()
        {
            // Assemble
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();

            // Find an update plugin step
            using (var ctx = new OrganizationServiceContext(crmSvc))
            {
                ctx.MergeOption = MergeOption.NoTracking;

            }
                // Create packaging task

                // Act

                // Extract solution

                // Assert
            }
    }
}
