// OrganizationServiceProxyTests.cs
//

using QUnitApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xrm;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    public class ActivityTests
    {
        static ActivityTests()
        {
            QUnit.Module("ActivityTests", null);
            QUnit.Test("Activity_01", ActivityTests.Activity_01);
        }

        public static void Activity_01(Assert assert)
        {
            assert.Expect(1);
            string timeStamp = DateTime.Now.ToISOString() + DateTime.Now.ToTimeString();

            // Create a contact
            Entity contact1 = new Entity("contact");
            contact1.SetAttributeValue("lastname", "Test Contact1 ");// + timeStamp);
            contact1.Id = OrganizationServiceProxy.Create(contact1).ToString();

            Entity contact2 = new Entity("contact");
            contact2.SetAttributeValue("lastname", "Test Contact2 ");// + timeStamp);
            contact2.Id = OrganizationServiceProxy.Create(contact2).ToString();

            // Create the BCC list
            Entity recipient = new Entity("activityparty");
            recipient.SetAttributeValue("partyid", contact1.ToEntityReference());

          
            // Create email
            List<Entity> recipients = new List<Entity>();
            ArrayEx.Add(recipients, recipient);
            Entity email = new Entity("email");
            email.SetAttributeValue("to", new EntityCollection(recipients));
            email.SetAttributeValue("subject", "Unit Test email " + timeStamp);

            email.SetAttributeValue("id", OrganizationServiceProxy.Create(email));
            email.Id = email.GetAttributeValue("id").ToString();

            // Retrieve Email
            Entity email2 = OrganizationServiceProxy.Retrieve("email", email.Id, new string[] { "to", "subject" });
            EntityCollection to = (EntityCollection)email2.GetAttributeValue("to");
            assert.Equal(to.Entities.Count,1,"Recipient Count");

            // Update recipients
            Entity recipient2 = new Entity("activityparty");

            recipient2.SetAttributeValue("partyid", contact2.ToEntityReference());

            List<Entity> toRecipients = to.Entities.Items();
            ArrayEx.Add(toRecipients, recipient2);
            OrganizationServiceProxy.Update(email2);

            


            // Tidy up
            OrganizationServiceProxy.Delete_(email.LogicalName, new Guid(email.Id));
            OrganizationServiceProxy.Delete_(contact1.LogicalName, new Guid(contact1.Id));
            OrganizationServiceProxy.Delete_(contact2.LogicalName, new Guid(contact2.Id));

        }

        public bool Issue143_DateRetrieve(Assert assert)
        {
            assert.Expect(2);
            // Create a contact
            Entity contact1 = new Entity("contact");
            Entity contact3 = new Entity("contact");
            try
            {
                contact1.SetAttributeValue("lastname", "Test Contact1 ");// + timeStamp);
                contact1.Id = OrganizationServiceProxy.Create(contact1).ToString();

                // Get the contact to check the date
                Entity contact2 = OrganizationServiceProxy.Retrieve("contact", contact1.Id, new string[] { "createdon", "modifiedon" });

                // Get the date
                DateTime created = (DateTime)contact2.GetAttributeValue("createdon");
                assert.Equal(DateTime.Now.GetFullYear(), created.GetFullYear(), "Year must be the same");

                // Create a new Contact with the date - using BeginCreate

                contact3 = new Entity("contact");
                contact3.SetAttributeValue("birthdate", created);
                Action done = assert.Async();

                OrganizationServiceProxy.BeginCreate(contact3, delegate (object state)
                 {
                     contact3.Id = OrganizationServiceProxy.EndCreate(state).Value;
                     assert.Ok(contact3.Id != null, "ID returned 2 " + contact3.Id);

                     // Tidy up
                     OrganizationServiceProxy.Delete_(contact3.LogicalName, new Guid(contact3.Id));
                     done();
                 });
            }
            finally
            {
                // Tidy up
                OrganizationServiceProxy.Delete_(contact1.LogicalName, new Guid(contact1.Id));

            }
            return true;
        }

        
    }
}
