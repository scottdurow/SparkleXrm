


/// <reference path="qunit.js" />
/// <reference path="mscorlib.debug.js" />
/// <reference path="sparklexrm.debug.js" />
/// <reference path="sparklexrm.unittests.debug.js" />


/// <reference path="../../debugweb/webresources/entitypropertiesutil.js" />
/// <reference path="../../debugweb/webresources/global.ashx.js" />
/// <reference path="../../debugweb/webresources/windowinformation.js" />
/// <reference path="GlobalContext.js" />

var tests = new SparkleXrm.UnitTests.OrganizationServiceProxyTests();


test('CRUD', function () {  
    ok(tests.crudTests());
});


test('Issue143_DateRetrieve', function (assert) {
    ok(tests.issue143_DateRetrieve(assert));
});

