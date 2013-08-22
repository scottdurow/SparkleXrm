// BooleanValueBinding.cs
//

using KnockoutApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.CustomBinding
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class BooleanBindingOptions
    {
        public string TrueOptionDisplayName;
        public string FalseOptionDisplayName;
    }
    
    /// <summary>
    /// Binding to allow setting boolean values as the option/checkbox values
    /// E.g.
    /// <input type="radio" name="endType" value="-1" style="width:30px" data-bind="booleanValue: noEndDate,targetBinding:'checked'"/>
    /// </summary>
    public class XrmBooleanBinding : BindingHandler
    {
        static XrmBooleanBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["booleanValue"] = new XrmBooleanBinding();
                ValidationApi.MakeBindingHandlerValidatable("booleanValue");
            }
        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            //Observable<string> observable = (Observable<string>)valueAccessor();
            Observable<bool> observableBool = (Observable<bool>)valueAccessor();

            ComputedObservable interceptor = new ComputedObservable();
            interceptor.Read = delegate()
            {
                return observableBool.GetValue().ToString();
            };
            interceptor.Write = delegate(object newValue)
            {
                observableBool.SetValue((string)newValue == "true");
            };

            // Get the target binding
            string targetBinding = (string)((object)allBindingsAccessor()["targetBinding"]);

            Dictionary<string, object> bindings = new Dictionary<string, object>();
            bindings[targetBinding] = KnockoutEx.Computed(interceptor) ;

            KnockoutEx.ApplyBindingsToNode(element, bindings);

            //Script.Literal("return { controlsDescendantBindings: true };");
        }
       
    }
}
