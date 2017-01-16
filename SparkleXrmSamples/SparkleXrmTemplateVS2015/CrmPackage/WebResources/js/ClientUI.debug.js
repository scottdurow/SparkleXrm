//! ClientUI.debug.js
//

(function() {

Type.registerNamespace('ClientUI.ViewModel');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.ViewModel.HelloWorldViewModel

ClientUI.ViewModel.HelloWorldViewModel = function ClientUI_ViewModel_HelloWorldViewModel() {
    ClientUI.ViewModel.HelloWorldViewModel.initializeBase(this);
    this.Message = ko.observable('Hello World');
}
ClientUI.ViewModel.HelloWorldViewModel.prototype = {
    Message: null,
    
    FooCommand: function ClientUI_ViewModel_HelloWorldViewModel$FooCommand() {
        this.Message(String.format('The time now is {0}', Date.get_now().toLocaleTimeString()));
    }
}


Type.registerNamespace('ClientUI.View');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.View.HelloWorldView

ClientUI.View.HelloWorldView = function ClientUI_View_HelloWorldView() {
}
ClientUI.View.HelloWorldView.Init = function ClientUI_View_HelloWorldView$Init() {
    Xrm.PageEx.majorVersion = 2013;
    ClientUI.View.HelloWorldView.vm = new ClientUI.ViewModel.HelloWorldViewModel();
    SparkleXrm.ViewBase.registerViewModel(ClientUI.View.HelloWorldView.vm);
}


ClientUI.ViewModel.HelloWorldViewModel.registerClass('ClientUI.ViewModel.HelloWorldViewModel', SparkleXrm.ViewModelBase);
ClientUI.View.HelloWorldView.registerClass('ClientUI.View.HelloWorldView');
ClientUI.View.HelloWorldView.vm = null;
})();

//! This script was generated using Script# v0.7.6.0
