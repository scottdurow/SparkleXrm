using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;

namespace Microsoft.Crm.Sdk.Fakes.Tests
{
    [TestClass]
    public class FakeOrganizationServiceTests
    {
        [TestMethod]
        public void TestIncorrectCallSequences()
        {
            // Arrange emulator
            var service = new FakeOrganzationService();
            service.ExpectCreate((entity) =>
            {
                return Guid.NewGuid();
            }).ExpectUpdate((entity) =>
            {

            });

            // Act
            IOrganizationService svc = service;
            var account = new Entity("account");
            account.Id = svc.Create(account);
            try
            {
                svc.Retrieve(account.LogicalName, account.Id, new ColumnSet("name"));
                Assert.Fail("Should fail");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Fake Organization Service expected 'Update' but 'Retrieve' was called", ex.Message);
            }

            // Assert
            service.AssertExpectedCalls();
        }

        [TestMethod]
        public void TestIncorrectCallCount()
        {
            // Arrange emulator
            var service = new FakeOrganzationService();
            service.ExpectCreate((entity) =>
            {
                return Guid.NewGuid();
            }).ExpectUpdate((entity) =>
            {

            });

            // Act
            IOrganizationService svc = service;
            var account = new Entity("account");
            svc.Create(account);

            // Assert
            Assert.IsFalse(service.ExpectedCalls(), "Incorrect call count should cause assert to fail");
            
        }


        [TestMethod]
        public void TestSingleCallSequence()
        {
            // Arrange emulator
            var service = new FakeOrganzationService();
            Entity createdAccount = null;
            service.ExpectCreate((entity) =>
            {
                createdAccount = entity;
                return Guid.NewGuid();
            }).ExpectUpdate((entity) =>
            {
                // Check the ID is the same
                Assert.AreEqual(createdAccount.Id, entity.Id, "Account ids must match on update");
            }).ExpectRetrieve((entityName, id, columnSet) => { 
                return createdAccount;
            }).ExpectRetrieveMultiple((query) =>
            {
                // Check the fetch query
                FetchExpression fetch = query as FetchExpression;
                Assert.IsTrue(fetch.Query.Contains("<condition attribute='name' operator='eq' value='test' />"));
                return new EntityCollection();
            }).ExpectDelete((entityName, id) =>{
                Assert.AreEqual(createdAccount.Id, id, "Account ids must match on delete");
            });

            // Act
            IOrganizationService svc = service;
            var account = new Entity("account");
            account.Id = svc.Create(account);
            svc.Update(account);
            svc.Retrieve(account.LogicalName, account.Id, new ColumnSet(true));
            svc.RetrieveMultiple(new FetchExpression(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                                        <entity name='account'>
                                                        <attribute name='name' />
                                                        <attribute name='primarycontactid' />
                                                        <attribute name='telephone1' />
                                                        <attribute name='accountid' />
                                                        <order attribute='name' descending='false' />
                                                        <filter type='and'>
                                                            <condition attribute='name' operator='eq' value='test' />
                                                        </filter>
                                                        </entity>
                                                    </fetch>"));

            svc.Delete(account.LogicalName, account.Id);

            // Assert
            service.AssertExpectedCalls();

        }
    }
}
