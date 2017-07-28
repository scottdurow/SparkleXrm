// FunctionTests.cs
//

using QUnitApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Serialization;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace SparkleXrm.UnitTests
{
    public class FunctionTests
    {
        [PreserveCase]
        public static void WhoAmI(Assert assert)
        {
            assert.Expect(1);
            WhoAmIRequest request = new WhoAmIRequest();
            OrganizationServiceProxy.RegisterExecuteMessageResponseType("WhoAmI", typeof(WhoAmIResponse));
            WhoAmIResponse response = (WhoAmIResponse)OrganizationServiceProxy.Execute(request);

            assert.Ok(response.UserId != null, "Userid=" + response.UserId.Value);
        }

        [PreserveCase]
        public static void RetrieveDuplicates_01_NoExistingContact(Assert assert)
        {
            assert.Expect(2);
            // Create a duplicate
            Entity contact1 = new Entity("contact");
            contact1.SetAttributeValue("firstname", "Foo");
            contact1.SetAttributeValue("lastname", "Bar");
            contact1.SetAttributeValue("emailaddress1", "foo@bar.com");
            contact1.Id = OrganizationServiceProxy.Create(contact1).Value;


            try
            {
                RetrieveDuplicatesRequest request = new RetrieveDuplicatesRequest();
                request.MatchingEntityName = "contact";
                request.PagingInfo = new PagingInfo();
                request.PagingInfo.PageNumber = 1;
                request.PagingInfo.Count = 10;
                request.PagingInfo.ReturnTotalRecordCount = true;
                Entity contact = new Entity("contact");
                contact.SetAttributeValue("firstname", "Foo");
                contact.SetAttributeValue("lastname", "Bar");
                contact.SetAttributeValue("emailaddress1", "foo@bar.com");

                request.BusinessEntity = contact;
                OrganizationServiceProxy.RegisterExecuteMessageResponseType("RetrieveDuplicates", typeof(RetrieveDuplicatesResponse));

                RetrieveDuplicatesResponse response = (RetrieveDuplicatesResponse)OrganizationServiceProxy.Execute(request);

                assert.Ok(response.DuplicateCollection != null && response.DuplicateCollection.Entities != null, "Duplicates returned");
                assert.Equal(response.DuplicateCollection.Entities.Count, 1, "Duplicate detected");
            }
            finally
            {
                OrganizationServiceProxy.Delete_("contact", new Guid(contact1.Id));
            }



            
        }

        [PreserveCase]
        public static void RetrieveDuplicates_02_ExistingContact(Assert assert)
        {
            assert.Expect(2);

            Guid id1 = null;
            Guid id2 = null;

            try
            {
                // Arrange

                Entity contact = new Entity("contact");
                contact.SetAttributeValue("firstname", "Foo");
                contact.SetAttributeValue("lastname", "Bar");
                contact.SetAttributeValue("emailaddress1", "foo@bar.com");

                id1 = OrganizationServiceProxy.Create(contact);
                id2 = OrganizationServiceProxy.Create(contact);

                contact.Id = id1.Value;

                // Act
                RetrieveDuplicatesRequest request = new RetrieveDuplicatesRequest();
                request.MatchingEntityName = "contact";
                request.PagingInfo = new PagingInfo();
                request.PagingInfo.PageNumber = 1;
                request.PagingInfo.Count = 10;
                request.PagingInfo.ReturnTotalRecordCount = true;
                request.BusinessEntity = contact;
                OrganizationServiceProxy.RegisterExecuteMessageResponseType("RetrieveDuplicates", typeof(RetrieveDuplicatesResponse));

                RetrieveDuplicatesResponse response = (RetrieveDuplicatesResponse)OrganizationServiceProxy.Execute(request);

                // Assert
                //assert.Equal(response.DuplicateCollection.TotalRecordCount, 1, "Expected 1 record returned");
                assert.Ok(response.DuplicateCollection.Entities.Count>0, "Expected >0 record returned");
                string firstId = response.DuplicateCollection.Entities[0].Id;
                string secondId = response.DuplicateCollection.Entities.Count > 1 ? response.DuplicateCollection.Entities[1].Id : null;

                assert.Ok(firstId == id2.Value || secondId == id2.Value, "ID of second contact returned");
            }
            finally
            {
                OrganizationServiceProxy.Delete_("contact", id1);
                OrganizationServiceProxy.Delete_("contact", id2);
            }
        }

        [PreserveCase]
        public static void RetrieveUserPrivileges_01(Assert assert)
        {
            assert.Expect(3);
            WhoAmIRequest whoAmIRequest = new WhoAmIRequest();
            OrganizationServiceProxy.RegisterExecuteMessageResponseType("WhoAmI", typeof(WhoAmIResponse));
            WhoAmIResponse whoAmIResponse = (WhoAmIResponse)OrganizationServiceProxy.Execute(whoAmIRequest);

            RetrieveUserPrivilegesRequest request = new RetrieveUserPrivilegesRequest();
            request.UserId = whoAmIResponse.UserId;

            OrganizationServiceProxy.RegisterExecuteMessageResponseType("RetrieveUserPrivileges", typeof(RetrieveUserPrivilegesResponse));

            RetrieveUserPrivilegesResponse response = (RetrieveUserPrivilegesResponse)OrganizationServiceProxy.Execute(request);

            assert.Ok(response.RolePrivileges.Count > 0, "Privileges returned");
            PrivilegeDepth depth = response.RolePrivileges[0].Depth;
            assert.Ok(depth== PrivilegeDepth.Basic || depth==PrivilegeDepth.Deep || depth==PrivilegeDepth.Global || depth==PrivilegeDepth.Local, "Privileges Depth returned");
            assert.Ok(response.RolePrivileges[0].PrivilegeId!=null && response.RolePrivileges[0].PrivilegeId.Value!=null, "Privileges Id returned");
        }
    }
}
