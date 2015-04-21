//! ClientUI.UnitTests.debug.js
//

(function() {

Type.registerNamespace('ClientUI.UnitTests');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.UnitTests.Bootstrap

ClientUI.UnitTests.Bootstrap = function ClientUI_UnitTests_Bootstrap() {
}
ClientUI.UnitTests.Bootstrap.RunTests = function ClientUI_UnitTests_Bootstrap$RunTests() {
    ClientUI.UnitTests.UnitTest1.run();
}


////////////////////////////////////////////////////////////////////////////////
// ClientUI.UnitTests.UnitTest1

ClientUI.UnitTests.UnitTest1 = function ClientUI_UnitTests_UnitTest1() {
}
ClientUI.UnitTests.UnitTest1.run = function ClientUI_UnitTests_UnitTest1$run() {
    var module = {};
    module.beforeEach = ClientUI.UnitTests.UnitTest1.setUp;
    module.afterEach = ClientUI.UnitTests.UnitTest1.teardown;
    QUnit.module('Unit Tests', module);
    QUnit.test('Test1', ClientUI.UnitTests.UnitTest1.test1);
}
ClientUI.UnitTests.UnitTest1.setUp = function ClientUI_UnitTests_UnitTest1$setUp() {
}
ClientUI.UnitTests.UnitTest1.teardown = function ClientUI_UnitTests_UnitTest1$teardown() {
}
ClientUI.UnitTests.UnitTest1.test1 = function ClientUI_UnitTests_UnitTest1$test1(assert) {
    assert.expect(1);
    assert.equal(false, false, 'Message');
}


ClientUI.UnitTests.Bootstrap.registerClass('ClientUI.UnitTests.Bootstrap');
ClientUI.UnitTests.UnitTest1.registerClass('ClientUI.UnitTests.UnitTest1');
})();

//! This script was generated using Script# v0.7.4.0
