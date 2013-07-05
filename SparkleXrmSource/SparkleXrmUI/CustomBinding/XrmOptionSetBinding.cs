// CrmTextBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.CustomBinding
{
   
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OptionSetBindingOptions
    {
        public string entityLogicalName;
        public string attributeLogicalName;
        public bool allowEmpty;

    }
    public class XrmOptionSetBinding: BindingHandler
    {
        
        static XrmOptionSetBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["optionset"] = new XrmOptionSetBinding();

                ValidationApi.MakeBindingHandlerValidatable("optionset");
            }
        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            // Get the text box element
            jQueryObject select = jQuery.FromElement(element).Find(".sparkle-input-optionset-part");

            jQueryEventHandler onChangeHandler = delegate(jQueryEvent e)
            {
                Observable<OptionSetValue> observable = (Observable<OptionSetValue>)valueAccessor();
                string newValue = select.GetValue();
                int? newValueInt = null;
                if (!String.IsNullOrEmpty(newValue))
                {
                    newValueInt = int.Parse(newValue);
                }
                // Set the optionset value
                OptionSetValue newValueOptionSetValue = new OptionSetValue(newValueInt);
                newValueOptionSetValue.Name = select.Find("option:selected").GetText();
                observable.SetValue(newValueOptionSetValue);
            };

            select.Change(onChangeHandler);

            allBindingsAccessor()["optionsValue"] = "value";
            allBindingsAccessor()["optionsText"] = "name";

            OptionSetBindingOptions optionSetOptions = (OptionSetBindingOptions)((object)allBindingsAccessor()["optionSetOptions"]);
            // Create a value accessor for the optionset options
            Func<List<OptionSetItem>> optionsValueAccessor = delegate() {
                return  MetadataCache.GetOptionSetValues(optionSetOptions.entityLogicalName, optionSetOptions.attributeLogicalName,optionSetOptions.allowEmpty);
            };

            Script.Literal("ko.bindingHandlers.options.update({0},{1},{2},{3},{4})", select.GetElement(0), optionsValueAccessor, allBindingsAccessor, viewModel, context);
           
            //Script.Literal("return { controlsDescendantBindings: true };");
        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {

            jQueryObject select = jQuery.FromElement(element).Find(".sparkle-input-optionset-part");
            Observable<OptionSetValue> observable = (Observable<OptionSetValue>)valueAccessor();
            OptionSetValue value = observable.GetValue();
            string newValue = "";
            if (value != null && value.Value != null)
                newValue = value.Value.ToString();

            select.Value(newValue);
           
        }
        
    }
}
