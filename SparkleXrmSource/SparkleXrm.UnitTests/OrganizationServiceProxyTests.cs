// OrganizationServiceProxyTests.cs
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xrm;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    public class OrganizationServiceProxyTests
    {
        public bool CRUDTests()
        {
            string timeStamp = DateTime.Now.ToISOString() + DateTime.Now.ToTimeString();

            // Create a contact
            Entity contact1 = new Entity("contact");
            contact1.SetAttributeValue("lastname", "Test Contact1 " + timeStamp);
            contact1.Id = OrganizationServiceProxy.Create(contact1).ToString();

            Entity contact2 = new Entity("contact");
            contact2.SetAttributeValue("lastname", "Test Contact2 " + timeStamp);
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
            
            email.SetAttributeValue("id",OrganizationServiceProxy.Create(email));
            email.Id = email.GetAttributeValue("id").ToString();

            // Retrieve Email
            Entity email2 = OrganizationServiceProxy.Retrieve("email", email.Id, new string[] { "to", "subject" });
            EntityCollection to = (EntityCollection)email2.GetAttributeValue("to");
            Debug.Assert(to.Entities.Count == 1);

            // Update recipients
            Entity recipient2 = new Entity("activityparty");
          
            recipient2.SetAttributeValue("partyid", contact2.ToEntityReference());
            
            List<Entity> toRecipients =  to.Entities.Items();
            ArrayEx.Add(toRecipients,recipient2);
            OrganizationServiceProxy.Update(email2);

            // Tidy up
            OrganizationServiceProxy.Delete_(email.LogicalName, new Guid(email.Id));
            OrganizationServiceProxy.Delete_(contact1.LogicalName, new Guid(contact1.Id));
            OrganizationServiceProxy.Delete_(contact2.LogicalName,new Guid(contact2.Id));

            return true;
        }



    }
}
