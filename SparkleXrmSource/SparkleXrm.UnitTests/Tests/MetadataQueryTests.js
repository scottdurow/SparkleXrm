


/// <reference path="qunit.js" />
/// <reference path="mscorlib.debug.js" />
/// <reference path="sparklexrm.debug.js" />
/// <reference path="sparklexrm.unittests.debug.js" />

/// <reference path="../../debugweb/webresources/entitypropertiesutil.js" />
/// <reference path="../../debugweb/webresources/global.ashx.js" />
/// <reference path="../../debugweb/webresources/windowinformation.js" />
/// <reference path="GlobalContext.js" />




var metadataQueryTests = new  SparkleXrm.UnitTests.MetadataQueryTests();

// Use the Chutzpah 'Run In Browser' rather than the headless browser
// This is because the headless browser doesn't support authentication

test('queryMetaDataNames', function() {
    ok(metadataQueryTests.queryAttributeDisplayNamesForTwoEntities());
});

test('queryNameAttributeForAccount', function () {
    ok(metadataQueryTests.queryNameAttributeForAccount());
});

test('queryOnetoManyRelationship', function () {
    ok(metadataQueryTests.queryOneToManyRelationship());
});

test('queryManytoManyRelationship', function () {
    ok(metadataQueryTests.queryManyToManyRelationship());
});
