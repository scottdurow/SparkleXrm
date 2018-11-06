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
    import Money = SparkleXrm.Sdk.Money;
    import OptionsetValue = SparkleXrm.Sdk.OptionSetValue;
    import XrmService = SparkleXrm.Sdk.XrmService;


    export class TestPromiseWebApi {

        public static Create_01(assert: Assert): void {
    
            assert.expect(1);
            var done = assert.async();
            var contact: Entity = new Entity("contact");
            var name = "Test " + Date.now.toString();
            contact.setAttributeValue("lastname", name);

            XrmService.Create(contact).then(function (id: Guid) {
             
                assert.ok(id != null, id.value);
                contact.id = id.value;

            })
                .catch(function (ex: Error) {
                 
                    console.log(ex.message);
                   
                })
                .then(function () {
                 
                    return XrmService.Delete(contact.logicalName, new Guid(contact.id));
                }).
                then(function () {
               
                    done();
                });
           
        }
    }
    QUnit.module("TS_PromiseTests");
    QUnit.test("TSPromise.Create_01", TestPromiseWebApi.Create_01);
}