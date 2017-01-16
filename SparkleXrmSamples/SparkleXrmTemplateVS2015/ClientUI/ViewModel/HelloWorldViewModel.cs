// HelloWorldViewModel.cs
//

using KnockoutApi;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace ClientUI.ViewModel
{
    public class HelloWorldViewModel : ViewModelBase
    {
        #region Fields
        [PreserveCase]
        public Observable<string> Message;
        #endregion

        #region Constructors
        public HelloWorldViewModel()
        {
            Message = Knockout.Observable<string>("Hello World");
        }
        #endregion

        #region Commands
        [PreserveCase]
        public void FooCommand()
        {
            Message.SetValue(String.Format("The time now is {0}", DateTime.Now.ToLocaleTimeString()));
        }
        #endregion
    }
}
