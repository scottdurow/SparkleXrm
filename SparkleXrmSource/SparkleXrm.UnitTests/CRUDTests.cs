// CRUDTestscs.cs
//

using QUnitApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    public class CRUDTests
    {
        static CRUDTests()
        {
            QUnit.Module("CRUD Tests", null);
            QUnit.Test("CRUDTests.Create_01", CRUDTests.Create_01);
            QUnit.Test("CRUDTests.Create_01_Sync", CRUDTests.Create_01_Sync);
            QUnit.Test("CRUDTests.Update_01", CRUDTests.Update_01);
            QUnit.Test("CRUDTests.Delete_01_Sync", CRUDTests.Delete_01_Sync);
            QUnit.Test("CRUDTests.Retrieve_02_UnknownLogicalName", CRUDTests.Retrieve_02_UnknownLogicalName);
            QUnit.Test("CRUDTests.Retrieve_02_UnknownAttribute", CRUDTests.Retrieve_02_UnknownAttribute);
            QUnit.Test("CRUDTests.Asscoiate_01_Sync",CRUDTests.Asscoiate_01_Sync);
        }
        [PreserveCase]
        public static void Create_01(Assert assert)
        {
          

            Action done = assert.Async();
            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());

          
            OrganizationServiceProxy.BeginCreate(contact, delegate (object state)
             {

                 contact.Id = OrganizationServiceProxy.EndCreate(state).ToString();
                 assert.Ok(contact.Id, "New ID = " + contact.Id);
                 done();
                 // Retrieve the new contact

                 OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));

             });

        }
        [PreserveCase]
        public static void Create_01_Sync(Assert assert)
        {

            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());

            contact.Id = OrganizationServiceProxy.Create(contact).ToString();

            assert.Ok(contact.Id, "New ID = " + contact.Id);
            OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));
        }

        [PreserveCase]
        public static void Update_01(Assert assert)
        {
            assert.Expect(2);

            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());

            contact.Id = OrganizationServiceProxy.Create(contact).ToString();
            contact.SetAttributeValue("contactid",new Guid(contact.Id));
            assert.Ok(contact.Id, "New ID = " + contact.Id);


            contact.SetAttributeValue("lastname", "Update");

            OrganizationServiceProxy.Update(contact);

            // Get the contact to check the update worked

            Entity contact2 = OrganizationServiceProxy.Retrieve(contact.LogicalName, contact.Id, new string[] { "lastname" });

            assert.Equal(contact2.GetAttributeValue("lastname"), contact.GetAttributeValue("lastname"), "Contact update");

            OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));
        }

        [PreserveCase]
        public static void Update_02_UnknownEntity(Assert assert)
        {
        }

        [PreserveCase]
        public static void Update_03_UnknownAttribute(Assert assert)
        {
        }
        [PreserveCase]
        public static void Retrieve_01(Assert assert)
        {
        }
        [PreserveCase]
        public static void Retrieve_02_UnknownLogicalName(Assert assert)
        {
            assert.Expect(1);

            try
            {
                Entity contact2 = OrganizationServiceProxy.Retrieve("unknown_logicalname", "00000000-0000-0000-0000-000000000001", new string[] { "lastname" });
            }
            catch (Exception ex)
            {
                assert.Ok(ex.Message.IndexOf("unknown_logicalname") > -1 , "Exception thrown:" + ex.Message);
            }

        }

        [PreserveCase]
        public static void Retrieve_02_UnknownAttribute(Assert assert)
        {
            assert.Expect(1);


            // Arrange
            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());

            contact.Id = OrganizationServiceProxy.Create(contact).ToString();

            try
            {
                // Act
                Entity contact2 = OrganizationServiceProxy.Retrieve("contact", contact.Id, new string[] { "unknown_attribute" });
            }
            catch (Exception ex)
            {
                // Assert
                assert.Ok(ex.Message.IndexOf("unknown_attribute") > -1, "Exception thrown:" + ex.Message);

            }
            finally
            {
                // Tidy up
            }
        }


        [PreserveCase]
        public static void Delete_01_Sync(Assert assert)
        {
            // Assemble
            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());

            contact.Id = OrganizationServiceProxy.Create(contact).ToString();

            assert.Expect(1);

            OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));

            // Try and get the entity
            try
            {
                Entity deletedContact = OrganizationServiceProxy.Retrieve("contact", contact.Id, new string[] { "fullname" });
                assert.NotOk(true, "Contact not detailed");
            }
            catch (Exception ex)
            {
                // Error should contain guid saying it doesn't exist
                assert.Ok(ex.Message.IndexOf(contact.Id) > -1, ex.Message);
            }
        }

        [PreserveCase]
        public static void Delete_02_Async(Assert assert)
        {
            // Assemble
            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());

            contact.Id = OrganizationServiceProxy.Create(contact).ToString();
            Action done = assert.Async();
            assert.Expect(1);

            OrganizationServiceProxy.BeginDelete(contact.LogicalName, new Guid(contact.Id), delegate (object state)
             {
                 OrganizationServiceProxy.EndDelete(state);

                // Try and get the entity
                try
                 {
                     Entity deletedContact = OrganizationServiceProxy.Retrieve("contact", contact.Id, new string[] { "fullname" });
                 }
                 catch (Exception ex)
                 {
                    // Error should contain guid saying it doesn't exist
                    assert.Ok(ex.Message.IndexOf(contact.Id) > -1, ex.Message);
                 }

                 done();
             });


        }

        [PreserveCase]
        public static void PerformanceTest(Assert assert)
        {
            assert.Expect(1);
            DateTime start = DateTime.Now;
            int itterations = 50;
            for (int i=0; i< itterations; i++)
            {
                Entity contact = new Entity("contact");
                contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());
                contact.SetAttributeValue("firstname", "Test " + Date.Now.ToISOString());
                contact.Id = OrganizationServiceProxy.Create(contact).ToString();
                Entity contact2 = OrganizationServiceProxy.Retrieve(contact.LogicalName, contact.Id, new string[] { "lastname","firstname" });
                OrganizationServiceProxy.Delete_(contact.LogicalName, new Guid(contact.Id));
            }
            decimal averagetime = (DateTime.Now - start) / itterations;
            assert.Ok(averagetime<500, "Avg:" + averagetime.ToString());

        }

        [PreserveCase]
        public static void Asscoiate_01_Sync(Assert assert)
        {
            assert.Expect(3);

            // Assemble
            Entity acc1 = new Entity("account");
            acc1.SetAttributeValue("name", "Test " + Date.Now.ToISOString());

            Entity lead1 = new Entity("lead");

            acc1.Id = OrganizationServiceProxy.Create(acc1).Value;
            lead1.Id = OrganizationServiceProxy.Create(lead1).Value;

            try
            {
                OrganizationServiceProxy.Associate(acc1.LogicalName, new Guid(acc1.Id), new Relationship("accountleads_association"), new List<EntityReference>(lead1.ToEntityReference()));

                // Check association
                string fetchxml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
  <entity name='lead'>  
    <attribute name='leadid' />
    <order attribute='fullname' descending='false' />
    <link-entity name='accountleads' from='leadid' to='leadid' visible='false' intersect='true'>
      <link-entity name='account' from='accountid' to='accountid' alias='ag'>
        <filter type='and'>
          <condition attribute='accountid' operator='eq' value='{" + acc1.Id + @"}' />
        </filter>
      </link-entity>
    </link-entity>
  </entity>
</fetch>";
                EntityCollection results = OrganizationServiceProxy.RetrieveMultiple(fetchxml);

                assert.Equal(results.Entities.Count, 1, "1 lead returned");
                assert.Equal(results.Entities[0].GetAttributeValueGuid("leadid"), lead1.Id, "Lead ID correct")
;
                OrganizationServiceProxy.Disassociate(acc1.LogicalName, new Guid(acc1.Id), new Relationship("accountleads_association"), new List<EntityReference>(lead1.ToEntityReference()));

                EntityCollection results2 = OrganizationServiceProxy.RetrieveMultiple(fetchxml);
                assert.Equal(results2.Entities.Count, 0, "0 leads returned");

            }
            finally
            {
                OrganizationServiceProxy.Delete_(lead1.LogicalName, new Guid(lead1.Id));
                OrganizationServiceProxy.Delete_(acc1.LogicalName, new Guid(acc1.Id));
            }


        }
    }
}
