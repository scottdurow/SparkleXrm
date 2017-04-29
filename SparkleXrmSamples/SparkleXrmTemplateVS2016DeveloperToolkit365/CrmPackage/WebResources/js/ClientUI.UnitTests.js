// ClientUI.UnitTests.js
(function(){
Type.registerNamespace('ClientUI.UnitTests');ClientUI.UnitTests.UnitTest2=function(){}
ClientUI.UnitTests.UnitTest2.run=function(){var $0={};$0.beforeEach=ClientUI.UnitTests.UnitTest2.setUp;$0.afterEach=ClientUI.UnitTests.UnitTest2.teardown;QUnit.module('Unit Tests',$0);QUnit.test('Test1',ClientUI.UnitTests.UnitTest2.test1);}
ClientUI.UnitTests.UnitTest2.setUp=function(){}
ClientUI.UnitTests.UnitTest2.teardown=function(){}
ClientUI.UnitTests.UnitTest2.test1=function(assert){assert.expect(1);assert.equal(true,false,'Message');}
ClientUI.UnitTests.Bootstrap=function(){}
ClientUI.UnitTests.Bootstrap.RunTests=function(){ClientUI.UnitTests.UnitTest2.run();}
ClientUI.UnitTests.UnitTest2.registerClass('ClientUI.UnitTests.UnitTest2');ClientUI.UnitTests.Bootstrap.registerClass('ClientUI.UnitTests.Bootstrap');})();// This script was generated using Script# v0.7.4.0
