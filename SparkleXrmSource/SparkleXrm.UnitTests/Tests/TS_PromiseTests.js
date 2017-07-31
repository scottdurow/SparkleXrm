/// <reference path="sparklexrm.d.ts" />
/// <reference path="node_modules/@types/qunit/index.d.ts" />
var tsTests;
(function (tsTests) {
    var Entity = SparkleXrm.Sdk.Entity;
    var Guid = SparkleXrm.Sdk.Guid;
    var XrmService = SparkleXrm.Sdk.XrmService;
    var TestPromiseWebApi = (function () {
        function TestPromiseWebApi() {
        }
        TestPromiseWebApi.Create_01 = function (assert) {
            assert.expect(1);
            var done = assert.async();
            var contact = new Entity("contact");
            var name = "Test " + Date.now.toString();
            contact.setAttributeValue("lastname", name);
            XrmService.create(contact).then(function (id) {
                assert.ok(id != null, id.value);
                contact.id = id.value;
            })
                .catch(function (ex) {
                console.log(ex.message);
            })
                .then(function () {
                return XrmService.delete_(contact.logicalName, new Guid(contact.id));
            }).
                then(function () {
                done();
            });
        };
        return TestPromiseWebApi;
    }());
    tsTests.TestPromiseWebApi = TestPromiseWebApi;
    QUnit.module("TS_PromiseTests");
    QUnit.test("TSPromise.Create_01", TestPromiseWebApi.Create_01);
})(tsTests || (tsTests = {}));
//# sourceMappingURL=TS_PromiseTests.js.map