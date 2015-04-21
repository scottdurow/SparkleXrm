// Class1.cs
//

using ClientUI.ViewModel;
using QUnitApi;
using Slick;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Html;
using Xrm.Sdk;

namespace ClientUI.UnitTests
{
  
    public class ConnectionViewModelTests
    {
        static List<Entity> accounts;
        static EntityReference partnerRole = new EntityReference(new Guid("949F6099-4B45-471E-96DB-59E2DECE2AF2"), "connectionrole", "");
        public static void Run()
        {
            ModuleInfo module = new ModuleInfo();
            module.BeforeEach = SetUp;
            module.AfterEach = Teardown;
            QUnit.Module("Connection View Model Tests", module);
            QUnit.Test("Test Create Connection", TestCreateConnection);
            QUnit.Test("Test Create Connection Validation", TestCreateConnectionValidation);
            QUnit.Test("Check Connections Collection", CheckConnectionsCollection);
            
        }
        public static void SetUp()
        {
            accounts = new List<Entity>();
            // Create test account
            Entity account1 = new Entity("account");
            account1.SetAttributeValue("name", "Unit Test " + DateTime.Now.ToLocaleTimeString());
            account1.Id = OrganizationServiceProxy.Create(account1).ToString();
            accounts.Add(account1);

            Entity account2 = new Entity("account");
            account2.SetAttributeValue("name", "Unit Test " + DateTime.Now.ToLocaleTimeString());
            account2.Id = OrganizationServiceProxy.Create(account2).ToString();
            accounts.Add(account2);
        }

        public static void Teardown()
        {
            // Tidy Up
            foreach (Entity account in accounts)
            {
                OrganizationServiceProxy.Delete_(account.LogicalName, new Guid(account.Id));
            }
        }
        public static void TestCreateConnection(Assert assert)
        {

            assert.Expect(1);
            Action done = assert.Async();

            ConnectionsViewModel vm = new ConnectionsViewModel(accounts[0].ToEntityReference(), null);
            ObservableConnection connection = vm.ConnectionEdit.GetValue();
            connection.OnSaveComplete += delegate(string arg)
            {
                assert.Equal(arg, null, "Save Error " + arg);
                done();
            };

            connection.Record1Id.SetValue(accounts[1].ToEntityReference());
            connection.Record1RoleId.SetValue(partnerRole);

            // This should save ok
            connection.SaveCommand();
        }

        public static void TestCreateConnectionValidation(Assert assert)
        {
                assert.Expect(1);
               
                ConnectionsViewModel vm = new ConnectionsViewModel(accounts[0].ToEntityReference(), null);
                ObservableConnection connection = vm.ConnectionEdit.GetValue();
                connection.Record1Id.SetValue(accounts[1].ToEntityReference());

                // Check the validation doesn't allow us to save without the role
                bool isValid = ((IValidatedObservable)connection).IsValid();
                assert.Equal(false, isValid, "Validation Not Valid");
  
        }
    
        public static void CheckConnectionsCollection(Assert assert)
        {
            // Add in the connection

            Entity connection = new Entity("connection");
            connection.SetAttributeValue("record1id", accounts[0].ToEntityReference());
            connection.SetAttributeValue("record2id", accounts[1].ToEntityReference());
            OrganizationServiceProxy.Create(connection);

            assert.Expect(1);
            Action done = assert.Async();
            ConnectionsViewModel vm = new ConnectionsViewModel(accounts[0].ToEntityReference(), null);
            vm.Connections.OnDataLoaded.Subscribe(delegate(EventData data, object args)
            {
                assert.Equal(vm.Connections.GetLength(), 1, "Only 1 connection");
                done();

            });
            vm.Search();


        }

       
    }
}
