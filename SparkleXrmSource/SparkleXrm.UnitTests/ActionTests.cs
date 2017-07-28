// ActionTests.cs
//

using QUnitApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace SparkleXrm.UnitTests
{
    
    public class ActionTests
    {

        [PreserveCase]
        public static void WinOpportunity(Assert assert)
        {
            // Assemble
            assert.Expect(1);
            string name = "Unit Test" + Date.Now.ToISOString();
            Entity account = new Entity("account");
            account.SetAttributeValue("name", name);
            account.Id = OrganizationServiceProxy.Create(account).Value;

            Entity opportunity = new Entity("opportunity");
            opportunity.SetAttributeValue("customerid", account.ToEntityReference());
            opportunity.SetAttributeValue("name", name);

            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata("opportunity", "customerid", "account,contact");

            opportunity.Id = OrganizationServiceProxy.Create(opportunity).Value;

            // Act
            WinOpportunityRequest request = new WinOpportunityRequest();
            Entity oppClose = new Entity("opportunityclose");
            oppClose.SetAttributeValue("subject", "Win!!");
            oppClose.SetAttributeValue("opportunityid", opportunity.ToEntityReference());
            request.OpportunityClose = oppClose;
            request.Status = new OptionSetValue(3);

            try
            {
                OrganizationServiceProxy.RegisterExecuteMessageResponseType("WinOpportunity", typeof(WinOpportunityResponse));
                WinOpportunityResponse winResponse = (WinOpportunityResponse)OrganizationServiceProxy.Execute(request);

                // Get the opportunity status to check it is closed
                Entity closedOpp = OrganizationServiceProxy.Retrieve(opportunity.LogicalName, opportunity.Id, new string[] { "statuscode" });
                assert.Equal(closedOpp.GetAttributeValueOptionSet("statuscode").Value, 3, "Opportunity closed");
            }
            finally
            {
                OrganizationServiceProxy.Delete_(opportunity.LogicalName, new Guid(opportunity.Id));
                OrganizationServiceProxy.Delete_(account.LogicalName, new Guid(account.Id));
            }




        }
        [PreserveCase]
        public static void AddToQueue(Assert assert)
        {
            assert.Expect(1);


            // Assemble
            Entity letter = new Entity("letter");
            letter.SetAttributeValue("subject", "Test");
            letter.Id = OrganizationServiceProxy.Create(letter).Value;

            Entity queue = new Entity("queue");
            queue.SetAttributeValue("name", "Test");
            queue.Id = OrganizationServiceProxy.Create(queue).Value;

            try
            {
                AddToQueueRequest request = new AddToQueueRequest();
                request.DestinationQueueId = new Guid(queue.Id);
                request.Target = letter.ToEntityReference();
                OrganizationServiceProxy.RegisterExecuteMessageResponseType("AddToQueue", typeof(AddToQueueResponse));
                AddToQueueResponse response = (AddToQueueResponse)OrganizationServiceProxy.Execute(request);
                assert.Ok(response.QueueItemId != null, "Queue item returned");
            }
            finally
            {
                OrganizationServiceProxy.Delete_(letter.LogicalName, new Guid(letter.Id));
                OrganizationServiceProxy.Delete_(queue.LogicalName, new Guid(queue.Id));
            }


        }
    }
}
