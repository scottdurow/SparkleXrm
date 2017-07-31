/// <reference path="sparklexrm.d.ts" />
/// <reference path="node_modules/@types/qunit/index.d.ts" />

module tests {

    import OrganizationServiceProxy = SparkleXrm.Sdk.OrganizationServiceProxy;
    import WebApiOrganizationServiceProxy = SparkleXrm.Sdk.WebApiOrganizationServiceProxy;
    import Entity = SparkleXrm.Sdk.Entity;
    import EntityReference = SparkleXrm.Sdk.EntityReference;
    import EntityCollection = SparkleXrm.Sdk.EntityCollection;
    import IEnumerable = SparkleXrm.IEnumerable;
    import Guid = SparkleXrm.Sdk.Guid;
    import Money = SparkleXrm.Sdk.Money;
    import OptionsetValue = SparkleXrm.Sdk.OptionSetValue;
    
  

    export class TestWebApi {

        public static Create_01(assert: Assert): void {

            var done = assert.async();
            assert.expect(1);
            var contact: Entity = new Entity("contact");
            var name: string = "Test " + Date.now.toString();
            contact.setAttributeValue("lastname", name);
            OrganizationServiceProxy.beginCreate(contact, function (state: Object) {
                contact.id = OrganizationServiceProxy.endCreate(state).toString();
                assert.ok(contact.id, "New ID = " + contact.id);
                done();

                // Tidy up
                OrganizationServiceProxy.delete_(contact.logicalName, new Guid(contact.id));
            });
        }

        public static FetchXmlTest(assert: Assert): void {
            var done = assert.async();
            assert.expect(1);

            var fetch: string = `<fetch version='1.0' output-format='xml-platform' mapping='logical' count='2' distinct='false' returntotalrecordcount='true'>
            <entity name='account' >
                <attribute name='name' />
                    <attribute name='primarycontactid' />
                        <attribute name='telephone1' />
                            <attribute name='accountid' />
                                <order attribute='name' descending= 'false' />
                                    </entity>
                                    </fetch>`;

            OrganizationServiceProxy.beginRetrieveMultiple(fetch, function (state: Object)
            {
                var items = OrganizationServiceProxy.endRetrieveMultiple(state, Entity);
               
                assert.ok(items.entities.get_count() > 0, "Non zero return count");

                // Update on the fetch results
                var account = items.entities.get_item(0);
                account.setAttributeValue("creditlimit", new SparkleXrm.Sdk.Money(1000));
                OrganizationServiceProxy.update(account);
                for (let e of  items.entities.items())
                {
                    
                    console.log(e.getAttributeValue("name"));
                }
                done();
            });
        }

        public static EntityReference_01(assert: Assert) : void
        {
            assert.expect(4);
            WebApiOrganizationServiceProxy.addNavigationPropertyMetadata("contact", "parentcustomerid", "account,contact");
            WebApiOrganizationServiceProxy.addNavigationPropertyMetadata("contact", "parentcustomerid", "account,contact");

            var name = "Unit Test" + Date.now.toString();
            var account = new Entity("account");
            account.setAttributeValue("name", name);
            account.id = OrganizationServiceProxy.create(account).value;

            // Act - Associate
            var contact = new Entity("contact");

           

            contact.setAttributeValue("lastname", name);
            contact.setAttributeValue("parentcustomerid", account.toEntityReference());
            contact.id = OrganizationServiceProxy.create(contact).toString();


            // Assert 1
            var contact2 = OrganizationServiceProxy.retrieve(contact.logicalName, contact.id, [ "parentcustomerid" ]);
            assert.equal(contact.getAttributeValue<EntityReference>("parentcustomerid").id.value,
                contact2.getAttributeValue<EntityReference>("parentcustomerid").id.value,
                "Account Contact related: ID correct");

            assert.equal(contact.getAttributeValueEntityReference("parentcustomerid").logicalName,
                contact2.getAttributeValueEntityReference("parentcustomerid").logicalName,
                "Account Contact related: Logical Name correct");


            // Act - Disassociate (update)
            contact.setAttributeValue("parentcustomerid", null);
            contact.setAttributeValue("contactid", new Guid(contact.id));
            OrganizationServiceProxy.update(contact);

            // Assert 2
            var contact3 = OrganizationServiceProxy.retrieve(contact.logicalName, contact.id, [ "parentcustomerid" ]);
            assert.ok(contact3.getAttributeValue<EntityReference>("parentcustomerid") == null, "Nulled lookup on update");

            // Arrange 3
            // Create contact with null lookup
            var contact4 = new Entity("contact");
            contact4.setAttributeValue("lastname", name);
            contact4.setAttributeValue("parentcustomerid", null);
            contact4.id = OrganizationServiceProxy.create(contact4).value;


            // Assert 4
            // Check null lookup
            var contact5 = OrganizationServiceProxy.retrieve(contact4.logicalName, contact4.id, [ "parentcustomerid" ]);
            assert.ok(contact5.getAttributeValue<EntityReference>("parentcustomerid") == null, "Nulled lookup on create");

            // Tidy up

            OrganizationServiceProxy.delete_(contact.logicalName, new Guid(contact.id));
            OrganizationServiceProxy.delete_(account.logicalName, new Guid(account.id));
            OrganizationServiceProxy.delete_(contact4.logicalName, new Guid(contact4.id));

        }
        
        
    }
    QUnit.module("TS_CrudTests");
    QUnit.test("TS.Create_01", TestWebApi.Create_01);
    QUnit.test("TS.FetchXmlTest", TestWebApi.FetchXmlTest);
    QUnit.test("TS.EntityReference_01", TestWebApi.EntityReference_01);
}

