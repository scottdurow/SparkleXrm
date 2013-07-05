// CrmTextBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;

namespace SparkleXrm.CustomBinding
{
    public class XrmTextBinding: BindingHandler
    {
        static XrmTextBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["xrmtextbox"] = new XrmTextBinding();
             
               ValidationApi.MakeBindingHandlerValidatable("xrmtextbox");
            }


        }
        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            // Get the text box element
            jQueryObject textBox = jQuery.FromElement(element).Find(".sparkle-input-textbox-part");

            jQueryEventHandler onChangeHandler = delegate(jQueryEvent e)
            {
                Observable<string> observable = (Observable<string>)valueAccessor();
                string newValue = textBox.GetValue();
                observable.SetValue(newValue);
            };

            textBox.Change(onChangeHandler);
            textBox.Keyup(onChangeHandler);
          
            // Stop further binding
            //Script.Literal("return { controlsDescendantBindings: true };");
        }
        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
         
            jQueryObject textBox = jQuery.FromElement(element).Find(".sparkle-input-textbox-part");
            // Use the standard value binding from ko
            Script.Literal("ko.bindingHandlers.value.update({0},{1},{2},{3},{4})", textBox.GetElement(0), valueAccessor, allBindingsAccessor, viewModel, context);
        }
        
    }
}
