


/// <reference path="qunit.js" />
/// <reference path="../../debugweb/webresources/sparkle_/js/mscorlib.js" />
/// <reference path="../../debugweb/webresources/sparkle_/js/sparklexrm.js" />
/// <reference path="sparklexrm.unittests.debug.js" />

/// <reference path="../../debugweb/webresources/entitypropertiesutil.js" />
/// <reference path="../../debugweb/webresources/global.ashx.js" />
/// <reference path="../../debugweb/webresources/windowinformation.js" />

/// <reference path="GlobalContext.js" />



var localisationtests = new SparkleXrm.UnitTests.LocalisationTests();

// Use the Chutzpah 'Run In Browser' rather than the headless browser
// This is because the headless browser doesn't support authentication

test('NumberEx.parse', function (assert) {
    localisationtests.NumberParse(assert);
});

test('LocalTimeZoneTests', function (assert) {
    localisationtests.LocalTimeZoneTests(assert);
});

test('UTCTimeZoneTests', function (assert) {
    localisationtests.UTCTimeZoneTests(assert);
});

