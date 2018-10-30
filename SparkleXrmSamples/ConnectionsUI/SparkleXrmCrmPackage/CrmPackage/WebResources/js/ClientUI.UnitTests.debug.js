//! ClientUI.UnitTests.debug.js
//

(function() {

Type.registerNamespace('ClientUI.UnitTests');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.UnitTests.Bootstrap

ClientUI.UnitTests.Bootstrap = function ClientUI_UnitTests_Bootstrap() {
}
ClientUI.UnitTests.Bootstrap.RunTests = function ClientUI_UnitTests_Bootstrap$RunTests() {
    ClientUI.UnitTests.ConnectionViewModelTests.run();
}


////////////////////////////////////////////////////////////////////////////////
// ClientUI.UnitTests.ConnectionViewModelTests

ClientUI.UnitTests.ConnectionViewModelTests = function ClientUI_UnitTests_ConnectionViewModelTests() {
}
ClientUI.UnitTests.ConnectionViewModelTests.run = function ClientUI_UnitTests_ConnectionViewModelTests$run() {
    var module = {};
    module.beforeEach = ClientUI.UnitTests.ConnectionViewModelTests.setUp;
    module.afterEach = ClientUI.UnitTests.ConnectionViewModelTests.teardown;
    QUnit.module('Connection View Model Tests', module);
    QUnit.test('Test Create Connection', ClientUI.UnitTests.ConnectionViewModelTests.testCreateConnection);
    QUnit.test('Test Create Connection Validation', ClientUI.UnitTests.ConnectionViewModelTests.testCreateConnectionValidation);
    QUnit.test('Check Connections Collection', ClientUI.UnitTests.ConnectionViewModelTests.checkConnectionsCollection);
}
ClientUI.UnitTests.ConnectionViewModelTests.setUp = function ClientUI_UnitTests_ConnectionViewModelTests$setUp() {
    ClientUI.UnitTests.ConnectionViewModelTests._accounts = [];
    var account1 = new SparkleXrm.Sdk.Entity('account');
    account1.setAttributeValue('name', 'Unit Test ' + Date.get_now().toLocaleTimeString());
    account1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account1).toString();
    ClientUI.UnitTests.ConnectionViewModelTests._accounts.add(account1);
    var account2 = new SparkleXrm.Sdk.Entity('account');
    account2.setAttributeValue('name', 'Unit Test ' + Date.get_now().toLocaleTimeString());
    account2.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account2).toString();
    ClientUI.UnitTests.ConnectionViewModelTests._accounts.add(account2);
}
ClientUI.UnitTests.ConnectionViewModelTests.teardown = function ClientUI_UnitTests_ConnectionViewModelTests$teardown() {
    var $enum1 = ss.IEnumerator.getEnumerator(ClientUI.UnitTests.ConnectionViewModelTests._accounts);
    while ($enum1.moveNext()) {
        var account = $enum1.current;
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(account.logicalName, new SparkleXrm.Sdk.Guid(account.id));
    }
}
ClientUI.UnitTests.ConnectionViewModelTests.testCreateConnection = function ClientUI_UnitTests_ConnectionViewModelTests$testCreateConnection(assert) {
    assert.expect(1);
    var done = assert.async();
    var vm = new ClientUI.ViewModel.ConnectionsViewModel(ClientUI.UnitTests.ConnectionViewModelTests._accounts[0].toEntityReference(), null, 10, null);
    var connection = vm.ConnectionEdit();
    connection.add_onSaveComplete(function(arg) {
        assert.equal(arg, null, 'Save Error ' + arg);
        done();
    });
    connection.record1id(ClientUI.UnitTests.ConnectionViewModelTests._accounts[1].toEntityReference());
    connection.record1roleid(ClientUI.UnitTests.ConnectionViewModelTests._partnerRole);
    connection.SaveCommand();
}
ClientUI.UnitTests.ConnectionViewModelTests.testCreateConnectionValidation = function ClientUI_UnitTests_ConnectionViewModelTests$testCreateConnectionValidation(assert) {
    assert.expect(1);
    var vm = new ClientUI.ViewModel.ConnectionsViewModel(ClientUI.UnitTests.ConnectionViewModelTests._accounts[0].toEntityReference(), null, 0, null);
    var connection = vm.ConnectionEdit();
    connection.record1id(ClientUI.UnitTests.ConnectionViewModelTests._accounts[1].toEntityReference());
    var isValid = (connection).isValid();
    assert.equal(false, isValid, 'Validation Not Valid');
}
ClientUI.UnitTests.ConnectionViewModelTests.checkConnectionsCollection = function ClientUI_UnitTests_ConnectionViewModelTests$checkConnectionsCollection(assert) {
    var connection = new SparkleXrm.Sdk.Entity('connection');
    connection.setAttributeValue('record1id', ClientUI.UnitTests.ConnectionViewModelTests._accounts[0].toEntityReference());
    connection.setAttributeValue('record2id', ClientUI.UnitTests.ConnectionViewModelTests._accounts[1].toEntityReference());
    SparkleXrm.Sdk.OrganizationServiceProxy.create(connection);
    assert.expect(1);
    var done = assert.async();
    var vm = new ClientUI.ViewModel.ConnectionsViewModel(ClientUI.UnitTests.ConnectionViewModelTests._accounts[0].toEntityReference(), null, 0, null);
    vm.Connections.onDataLoaded.subscribe(function(data, args) {
        assert.equal(vm.Connections.getLength(), 1, 'Only 1 connection');
        done();
    });
    vm.search();
}


ClientUI.UnitTests.Bootstrap.registerClass('ClientUI.UnitTests.Bootstrap');
ClientUI.UnitTests.ConnectionViewModelTests.registerClass('ClientUI.UnitTests.ConnectionViewModelTests');
ClientUI.UnitTests.ConnectionViewModelTests._accounts = null;
ClientUI.UnitTests.ConnectionViewModelTests._partnerRole = new SparkleXrm.Sdk.EntityReference(new SparkleXrm.Sdk.Guid('949F6099-4B45-471E-96DB-59E2DECE2AF2'), 'connectionrole', '');
})();

//! This script was generated using Script# v0.7.4.0
