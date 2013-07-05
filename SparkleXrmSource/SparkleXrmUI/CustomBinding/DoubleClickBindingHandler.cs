// DoubleClickBindingHandler.cs
//

using jQueryApi;
using KnockoutApi;
using System;
using System.Html;

namespace SparkleXrm
{
    public class DoubleClickBindingHandler : BindingHandler
    {
        static DoubleClickBindingHandler()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["singleClick"] = new DoubleClickBindingHandler();
            }
        }
        private int delay = 400;
       
        private int clickTimeoutId =0;
        public override void  Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            Func<object> hander = valueAccessor;
 	         jQuery.Select(null,element).Click( delegate(jQueryEvent e) {
                 if (clickTimeoutId!=0){
                     Window.ClearTimeout(0);
                     clickTimeoutId=0;
                 }
                 else
                 {
                     clickTimeoutId = Window.SetTimeout(delegate() {
                         clickTimeoutId=0;
                         hander();

                     },delay);
                 }

             });
        }

       
    }

    //ko.bindingHandlers.singleClick= {
    //init: function(element, valueAccessor) {
    //    var handler = valueAccessor(),
    //        delay = 400,
    //        clickTimeout = false;

    //    $(element).click(function() {
    //        if(clickTimeout !== false) {
    //            clearTimeout(clickTimeout);
    //            clickTimeout = false;
    //        } else {        
    //            clickTimeout = setTimeout(function() {
    //                clickTimeout = false;
    //                handler();
    //            }, delay);
    //        }
    //    });
    //}

}
