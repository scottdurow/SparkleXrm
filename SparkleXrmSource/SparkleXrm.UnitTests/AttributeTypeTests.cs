// AttributeTypeTests.cs
//

using QUnitApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    public class AttributeTypeTests
    {
        [PreserveCase]
        public static void EntityReference_01(Assert assert)
        {
            // Arrange
            assert.Expect(4);

            string name = "Unit Test" + Date.Now.ToISOString();
            Entity account = new Entity("account");
            account.SetAttributeValue("name", name);
            account.Id = OrganizationServiceProxy.Create(account).Value;

            // Act - Associate
            Entity contact = new Entity("contact");

            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata("contact", "parentcustomerid", "account,contact");

            contact.SetAttributeValue("lastname", name);
            contact.SetAttributeValue("parentcustomerid", account.ToEntityReference());
            contact.Id = OrganizationServiceProxy.Create(contact).ToString();


            // Assert 1
            Entity contact2 = OrganizationServiceProxy.Retrieve(contact.LogicalName, contact.Id, new string[] { "parentcustomerid" });
            assert.Equal(contact.GetAttributeValueEntityReference("parentcustomerid").Id.Value,
                contact2.GetAttributeValueEntityReference("parentcustomerid").Id.Value,
                "Account Contact related: ID correct");

            assert.Equal(contact.GetAttributeValueEntityReference("parentcustomerid").LogicalName,
                contact2.GetAttributeValueEntityReference("parentcustomerid").LogicalName,
                "Account Contact related: Logical Name correct");


            // Act - Disassociate (update)
            contact.SetAttributeValue("parentcustomerid", null);
            contact.SetAttributeValue("contactid", new Guid(contact.Id));
            OrganizationServiceProxy.Update(contact);

            // Assert 2
            Entity contact3 = OrganizationServiceProxy.Retrieve(contact.LogicalName, contact.Id, new string[] { "parentcustomerid" });
            assert.Ok(contact3.GetAttributeValueEntityReference("parentcustomerid") == null, "Nulled lookup on update");

            // Arrange 3
            // Create contact with null lookup
            Entity contact4 = new Entity("contact");
            contact4.SetAttributeValue("lastname", name);
            contact4.SetAttributeValue("parentcustomerid", null);
            contact4.Id = OrganizationServiceProxy.Create(contact4).Value;


            // Assert 4
            // Check null lookup
            Entity contact5 = OrganizationServiceProxy.Retrieve(contact4.LogicalName, contact4.Id, new string[] { "parentcustomerid" });
            assert.Ok(contact5.GetAttributeValueEntityReference("parentcustomerid") == null, "Nulled lookup on create");

            // Tidy up

            OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));
            OrganizationServiceProxy.Delete_(account.LogicalName, new Guid(account.Id));
            OrganizationServiceProxy.Delete_(contact4.LogicalName, new Guid(contact4.Id));


        }

        [PreserveCase]
        public static void EntityReference_02_SetPrimarContactToNull(Assert assert)
        {
            // Arrange
            assert.Expect(2);

            string name = "Unit Test" + Date.Now.ToISOString();
            Entity account = new Entity("account");
            account.SetAttributeValue("name", name);
            account.Id = OrganizationServiceProxy.Create(account).Value;
            Entity contact = new Entity("contact");

            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata("contact", "parentcustomerid", "account,contact");
            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata("account", "primarycontactid", "contact");

            contact.SetAttributeValue("lastname", name);
            contact.SetAttributeValue("parentcustomerid", account.ToEntityReference());
            contact.Id = OrganizationServiceProxy.Create(contact).ToString();

            // Act 1

            // Set primary contact on account
            account.SetAttributeValue("primarycontactid", contact.ToEntityReference());
            account.SetAttributeValue("accountid", new Guid(account.Id));
            OrganizationServiceProxy.Update(account);

            // Assert
            Entity account2 = OrganizationServiceProxy.Retrieve(account.LogicalName, account.Id, new string[] { "primarycontactid" });
            assert.Equal(account2.GetAttributeValueEntityReference("primarycontactid").Id.Value, contact.Id, "Primary Contact Set");


            // Act 2
            // Set primary contact to null
            account.SetAttributeValue("primarycontactid", null);
            
            OrganizationServiceProxy.Update(account);
            // Assert 2
            Entity account3 = OrganizationServiceProxy.Retrieve(account.LogicalName, account.Id, new string[] { "primarycontactid" });
            assert.Equal(account3.GetAttributeValueEntityReference("primarycontactid"), null, "Primary Contact Set to null");



            // Tidy
            OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));
            OrganizationServiceProxy.Delete_(account.LogicalName, new Guid(account.Id));
        }

        [PreserveCase]
        public static void EntityReference_03_CustomerId(Assert assert)
        {
            // Arrange
            assert.Expect(2);
            string name = "Unit Test" + Date.Now.ToISOString();
            Entity account = new Entity("account");
            account.SetAttributeValue("name", name);
            account.Id = OrganizationServiceProxy.Create(account).Value;

            Entity opportunity = new Entity("opportunity");
            opportunity.SetAttributeValue("customerid", account.ToEntityReference());
            opportunity.SetAttributeValue("name", name);

            try
            {
                WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata("opportunity", "customerid", "account,contact");

                opportunity.Id = OrganizationServiceProxy.Create(opportunity).Value;

                // Get the opportunity and check the lookup
                Entity opportunity2 = OrganizationServiceProxy.Retrieve(opportunity.LogicalName, opportunity.Id, new string[] { "customerid" });
                assert.Equal(opportunity2.GetAttributeValueEntityReference("customerid").LogicalName, account.LogicalName, "Logical Name correct");
                assert.Equal(opportunity2.GetAttributeValueEntityReference("customerid").Id.Value, account.Id, "Id correct : " + account.Id);
            }
            finally
            {
                // Tidy
                OrganizationServiceProxy.Delete_(opportunity.LogicalName, new Guid(opportunity.Id));
                OrganizationServiceProxy.Delete_(account.LogicalName, new Guid(account.Id));
            }
        }

        [PreserveCase]
        public static void Money_01(Assert assert)
        {
            // Arrange
            assert.Expect(2);

            string name = "Unit Test" + Date.Now.ToISOString();
            Entity account = new Entity("account");
            account.SetAttributeValue("name", name);

            Money creditlimit = new Money(123456);
            account.SetAttributeValue("creditlimit", creditlimit);

            account.Id = OrganizationServiceProxy.Create(account).Value;

            // query 
            Entity account2 = OrganizationServiceProxy.Retrieve(account.LogicalName, account.Id, new string[] { "creditlimit","transactioncurrencyid" });
            assert.Equal(((Money)account2.GetAttributeValue("creditlimit")).Value,
                creditlimit.Value,
                "Money value correct");

            EntityReference currency = account2.GetAttributeValueEntityReference("transactioncurrencyid");
            assert.Ok(currency!=null && currency.LogicalName == "transactioncurrency", "Transaction Currency = " + currency.Name);
            // tidy up
            OrganizationServiceProxy.Delete_(account.LogicalName, new Guid(account.Id));
        }

    }
}
