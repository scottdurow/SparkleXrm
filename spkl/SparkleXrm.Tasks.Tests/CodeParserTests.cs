using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkleXrm.Tasks;
using System.Text;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class CodeParserTests
    {
        private CrmPluginRegistrationAttribute _crmPluginRegistrationAttr = new CrmPluginRegistrationAttribute(
                "Create",
                "opportunity",
                StageEnum.PostOperation,
                ExecutionModeEnum.Synchronous,
                "",
                "PostOpportunityCreate",
                1,
                IsolationModeEnum.Sandbox);

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void AddAttributeTest_SingleLine()
        {            
            var codeParser = new CodeParser(string.Format(TestCode.SingleLinePluginDefinitionTemplate, ""));
            codeParser.AddAttribute(_crmPluginRegistrationAttr, "MyCompany.Plugins.OpportunityPlugin");

            var expectedPluginDeifintionSB = string.Format(
                TestCode.SingleLinePluginDefinitionTemplate,
                _crmPluginRegistrationAttr.GetAttributeCode("\r\n") + "\r\n");

            Assert.AreEqual(codeParser.Code, expectedPluginDeifintionSB.ToString());
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void AddAttributeTest_MultiLine()
        {
            var codeParser = new CodeParser(string.Format(TestCode.MultiLinePluginDefinitionTemplate, ""));
            codeParser.AddAttribute(_crmPluginRegistrationAttr, "MyCompany.Plugins.OpportunityPlugin");

            var expectedPluginDeifintionSB = string.Format(
                TestCode.MultiLinePluginDefinitionTemplate, 
                _crmPluginRegistrationAttr.GetAttributeCode("\r\n    "));

            Assert.AreEqual(codeParser.Code, expectedPluginDeifintionSB.ToString());
        }
    }
}
