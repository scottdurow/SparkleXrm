using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using QuickNavigation.Plugins;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Client;

namespace Plugins.UnitTests
{
    [TestClass]
    public class SiteMapCachePluginTests
    {
        [TestMethod]
        public void TestSiteMapCacheRequest()
        {
            IOrganizationService service = new OrganizationService(new CrmConnection("CRM"));

            StringWriter mapJson = new StringWriter();
            SiteMapLoader loader = new SiteMapLoader(1033);
            ITracingService tace = new debugTrace();
            loader.ParseSiteMapToJson(service,tace, mapJson);
            Console.WriteLine(mapJson.ToString());
        }
    }
    public class debugTrace : ITracingService
    {
        public void Trace(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }

}
