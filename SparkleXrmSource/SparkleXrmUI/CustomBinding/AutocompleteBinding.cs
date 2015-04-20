using jQueryApi;
using jQueryApi.UI.Widgets;
using KnockoutApi;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SparkleXrm.CustomBinding
{
    public class AutocompleteBinding : BindingHandler
    {
        static AutocompleteBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["autocomplete"] = new AutocompleteBinding();
            }


        }
        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {

            jQueryObject inputField = jQuery.FromElement(element);
            AutoCompleteOptions options = (AutoCompleteOptions)((object)allBindingsAccessor()["autocompleteOptions"]);
            options.Position = new Dictionary<string, object>("collision", "fit");
            options.Select = delegate(jQueryEvent e, AutoCompleteSelectEvent uiEvent)
            {
               
                // Note we assume that the binding has added an array of string items
                string value = ((Dictionary)uiEvent.Item)["value"].ToString();
                
                TrySetObservable(valueAccessor, inputField, value);
            };

            inputField = inputField.Plugin<AutoCompleteObject>().AutoComplete(options);
            jQueryObject selectButton = inputField.Siblings(".timeSelectButton"); 
            // Add the click binding to show the drop down
            selectButton.Click(delegate(jQueryEvent e)
            {
               
                inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Search);
            });

            //handle the field changing
            KnockoutUtils.RegisterEventHandler(element, "change", delegate(object sender, EventArgs e)
            {
                string value = inputField.GetValue();
                TrySetObservable(valueAccessor, inputField, value);
            });

            Action disposeCallBack = delegate()
            {
                Script.Literal("$({0}).autocomplete(\"destroy\")", element);
            };

            //handle disposal (if KO removes by the template binding)
            Script.Literal("ko.utils.domNodeDisposal.addDisposeCallback({0}, {1})", element, (object)disposeCallBack);
            Knockout.BindingHandlers["validationCore"].Init(element, valueAccessor, allBindingsAccessor, null, null);

        }

        private static void TrySetObservable(Func<object> valueAccessor, jQueryObject inputField, string value)
        {
           
            Observable<string> observable = (Observable<string>)valueAccessor();
            bool isValid = true;
            observable.SetValue(value);

            if (((string)Script.Literal("typeof({0}.isValid)", observable)) != "undefined")
            {
                isValid = ((IValidatedObservable)observable).IsValid() == true;
            }

            if (isValid)
            {
                inputField.Blur();
                
            }
            
        }

      


        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {

            object value = KnockoutUtils.UnwrapObservable(valueAccessor());

            // //handle date data coming via json from Microsoft
            // if (String(value).indexOf('/Date(') == 0) {
            //     value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
            // }

            // current = $(element).datepicker("getDate");
            jQuery.FromElement(element).Value((string)value);
            // if (value - current !== 0) {
            //     $(element).datepicker("setDate", value);
            // }
        }

    }
}
