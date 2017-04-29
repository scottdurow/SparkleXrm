//! ClientUI.UnitTests.debug.js
//

(function() {

Type.registerNamespace('ClientUI.UnitTests');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.UnitTests.UnitTest2

ClientUI.UnitTests.UnitTest2 = function ClientUI_UnitTests_UnitTest2() {
}
ClientUI.UnitTests.UnitTest2.run = function ClientUI_UnitTests_UnitTest2$run() {
    var module = {};
    module.beforeEach = ClientUI.UnitTests.UnitTest2.setUp;
    module.afterEach = ClientUI.UnitTests.UnitTest2.teardown;
    QUnit.module('Unit Tests', module);
    QUnit.test('Test1', ClientUI.UnitTests.UnitTest2.test1);
}
ClientUI.UnitTests.UnitTest2.setUp = function ClientUI_UnitTests_UnitTest2$setUp() {
}
ClientUI.UnitTests.UnitTest2.teardown = function ClientUI_UnitTests_UnitTest2$teardown() {
}
ClientUI.UnitTests.UnitTest2.test1 = function ClientUI_UnitTests_UnitTest2$test1(assert) {
    assert.expect(1);
    assert.equal(true, false, 'Message');
}


////////////////////////////////////////////////////////////////////////////////
// ClientUI.UnitTests.Bootstrap

ClientUI.UnitTests.Bootstrap = function ClientUI_UnitTests_Bootstrap() {
}
ClientUI.UnitTests.Bootstrap.RunTests = function ClientUI_UnitTests_Bootstrap$RunTests() {
    ClientUI.UnitTests.UnitTest2.run();
}


ClientUI.UnitTests.UnitTest2.registerClass('ClientUI.UnitTests.UnitTest2');
ClientUI.UnitTests.Bootstrap.registerClass('ClientUI.UnitTests.Bootstrap');
})();

//! This script was generated using Script# v0.7.4.0
