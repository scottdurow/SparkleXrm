// Class1.cs
//

using ClientUI.ViewModel;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;


namespace ClientUI.View
{

    public static class HelloWorldView
    {
        public static HelloWorldViewModel vm;

        [PreserveCase]
        public static void Init()
        {
            PageEx.MajorVersion = 2013; // Use the CRM2013/2015 styles
            vm = new HelloWorldViewModel();
            ViewBase.RegisterViewModel(vm);
        }


    }
}
