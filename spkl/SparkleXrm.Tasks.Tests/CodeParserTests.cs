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
            var pluginDefinitionTemplate = "namespace MyCompany.Plugins{{{0}public class OpportunityPlugin : Plugin {{ }} }}";
            var code = string.Format(pluginDefinitionTemplate, "");
            
            var codeParser = new CodeParser(code);
            codeParser.AddAttribute(_crmPluginRegistrationAttr, "MyCompany.Plugins.OpportunityPlugin");

            var expectedPluginDeifintionSB = string.Format(pluginDefinitionTemplate, _crmPluginRegistrationAttr.GetAttributeCode("\r\n") + "\r\n");

            Assert.AreEqual(codeParser.Code, expectedPluginDeifintionSB.ToString());
        }

        [TestMethod]
        [TestCategory("Unit Tests")]
        public void AddAttributeTest_MultiLine()
        {
            var pluginDefinitionTemplate = "namespace MyCompany.Plugins {{{0}\r\n    public class OpportunityPlugin : Plugin {{\r\n    }}\r\n}}";
            var code = string.Format(pluginDefinitionTemplate, "");

            var codeParser = new CodeParser(code);
            codeParser.AddAttribute(_crmPluginRegistrationAttr, "MyCompany.Plugins.OpportunityPlugin");

            var expectedPluginDeifintionSB = string.Format(pluginDefinitionTemplate, _crmPluginRegistrationAttr.GetAttributeCode("\r\n    "));

            Assert.AreEqual(codeParser.Code, expectedPluginDeifintionSB.ToString());
        }
    }
}
