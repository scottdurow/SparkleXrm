// ClientUI.js
(function($){
Type.registerNamespace('ClientUI.ViewModel');ClientUI.ViewModel.HelloWorldViewModel=function(){ClientUI.ViewModel.HelloWorldViewModel.initializeBase(this);this.Message=ko.observable('Hello World');}
ClientUI.ViewModel.HelloWorldViewModel.prototype={Message:null,FooCommand:function(){this.Message(String.format('The time now is {0}',Date.get_now().toLocaleTimeString()));}}
Type.registerNamespace('ClientUI.View');ClientUI.View.HelloWorldView=function(){}
ClientUI.View.HelloWorldView.Init=function(){Xrm.PageEx.majorVersion=2013;ClientUI.View.HelloWorldView.vm=new ClientUI.ViewModel.HelloWorldViewModel();SparkleXrm.ViewBase.registerViewModel(ClientUI.View.HelloWorldView.vm);}
ClientUI.ViewModel.HelloWorldViewModel.registerClass('ClientUI.ViewModel.HelloWorldViewModel',SparkleXrm.ViewModelBase);ClientUI.View.HelloWorldView.registerClass('ClientUI.View.HelloWorldView');ClientUI.View.HelloWorldView.vm=null;})(window.xrmjQuery);