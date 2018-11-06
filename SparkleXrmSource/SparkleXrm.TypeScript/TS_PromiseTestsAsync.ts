/// <reference path="sparklexrm.d.ts" />
/// <reference path="node_modules/@types/qunit/index.d.ts" />

module tsTests {

    import OrganizationServiceProxy = SparkleXrm.Sdk.OrganizationServiceProxy;
    import WebApiOrganizationServiceProxy = SparkleXrm.Sdk.WebApiOrganizationServiceProxy;
    import Entity = SparkleXrm.Sdk.Entity;
    import EntityReference = SparkleXrm.Sdk.EntityReference;
    import EntityCollection = SparkleXrm.Sdk.EntityCollection;
    import IEnumerable = SparkleXrm.IEnumerable;
    import Guid = SparkleXrm.Sdk.Guid;
    import ColumnSet = SparkleXrm.Sdk.ColumnSet;
    import Money = SparkleXrm.Sdk.Money;
    import OptionsetValue = SparkleXrm.Sdk.OptionSetValue;
    import XrmService = SparkleXrm.Sdk.XrmService;


    export class TestPromiseWebApiAsync {
        // This is an async TS version of the TS_CrudTests
        // We mark the method as async and use the await keyword so that Typescript will automatically resolve promises
        public static async Create_01(assert: Assert) {
           
            assert.expect(2);
            var done = assert.async();
            var contact: Entity = new Entity("contact");
            var name = "Test " + Date.now();
            contact.setAttributeValue("lastname", name);
           
            try {
                var id = await XrmService.Create(contact);      
                assert.ok(id != null, id.value);
                contact.id = id.value;
            }
            catch (ex) {
                console.log((<Error>ex).message);
            }
            
            await XrmService.Update(contact);
            await XrmService.Delete(contact.logicalName, new Guid(contact.id));
      
            var errorMessage = "";
            // Check it doesn't exist
            try {
                var contact = await XrmService.Retrieve(contact.logicalName, contact.id, new ColumnSet(["fullname"]));
                // This should throw an error and so should not get here
            
            }
            catch (ex) {
                console.log(ex);
                errorMessage = (<Error>ex).message;           
            }
 
            assert.ok(errorMessage.indexOf("Does Not Exist") > -1, "Contact Deleted : " + errorMessage);
            done();
           
        }
    }
    QUnit.module("TS_PromiseTests.Async");
    QUnit.test("TSPromise.Async.Create_01", TestPromiseWebApiAsync.Create_01);
}