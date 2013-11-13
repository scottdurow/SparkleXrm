


/// <reference path="qunit.js" />
/// <reference path="mscorlib.debug.js" />
/// <reference path="sparklexrm.debug.js" />
/// <reference path="sparklexrm.unittests.debug.js" />

/// <reference path="../../debugweb/webresources/entitypropertiesutil.js" />
/// <reference path="../../debugweb/webresources/global.ashx.js" />
/// <reference path="../../debugweb/webresources/windowinformation.js" />
var USER_GUID = '\x7bB9912BA3-5559-E111-B400-000C299FFE7D\x7d';
var ORG_LANGUAGE_CODE = 1033;
var ORG_UNIQUE_NAME = 'ScriptSharp';
var SERVER_URL = 'http\x3a\x2f\x2flocalhost\x3a5555\x2f' + ORG_UNIQUE_NAME;
var USER_LANGUAGE_CODE = 1033;
var USER_ROLES = ['eb660f03-7b82-e211-8d82-000c299ffe7d'];
var CRM2007_WEBSERVICE_NS = 'http\x3a\x2f\x2fschemas.microsoft.com\x2fcrm\x2f2007\x2fWebServices';
var CRM2007_CORETYPES_NS = 'http\x3a\x2f\x2fschemas.microsoft.com\x2fcrm\x2f2007\x2fCoreTypes';
var AUTHENTICATION_TYPE = 0;
var CURRENT_THEME_TYPE = 'Outlook14Silver';
var CURRENT_WEB_THEME = 'Default';
var IS_OUTLOOK_CLIENT = false;
var IS_OUTLOOK_LAPTOP_CLIENT = false;
var IS_OUTLOOK_14_CLIENT = false;
var IS_ONLINE = true;
var LOCID_UNRECOGNIZE_DOTC = '\x7b0\x7d is not a recognized Object type.';
var EDIT_PRELOAD = true;
var WEB_SERVER_HOST = 'dev01';
var WEB_SERVER_PORT = 5555;
var IS_PATHBASEDURLS = true;
var LOCID_UNRECOGNIZE_DOTC = '\x7b0\x7d is not a recognized Object type.';
var EDIT_PRELOAD = true;
var WEB_RESOURCE_ORG_VERSION_NUMBER = '\x7b635010123660003236\x7d';
function GetGlobalContext() { return Xrm.Page.context }




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
