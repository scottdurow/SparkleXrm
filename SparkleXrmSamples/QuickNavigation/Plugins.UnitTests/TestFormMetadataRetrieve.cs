using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Crm.Sdk.Messages;

namespace Plugins.UnitTests
{
    /// <summary>
    /// Summary description for TestFormMetadataRetrieve
    /// </summary>
    [TestClass]
    public class TestFormMetadataRetrieve
    {
        public TestFormMetadataRetrieve()
        {
            
        }

       

        [TestMethod]
        public void TestMethod1()
        {
            IOrganizationService service = new OrganizationService(new CrmConnection("CRM"));

            RetrieveFormXmlRequest request = new RetrieveFormXmlRequest();
            request.EntityName = "contact";
            
            var response = service.Execute<RetrieveFormXmlResponse>(request);
            Console.WriteLine(response.FormXml);

        }
    }
}
