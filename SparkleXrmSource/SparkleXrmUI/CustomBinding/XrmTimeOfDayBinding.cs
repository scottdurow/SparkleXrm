using jQueryApi;
using jQueryApi.UI.Widgets;
using KnockoutApi;
using SparkleXrm.GridEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Xrm.Sdk;

namespace SparkleXrm.CustomBinding
{
    public class XrmTimeOfDayBinding : BindingHandler
    {
       

        static XrmTimeOfDayBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["timeofday"] = new XrmTimeOfDayBinding();
                //ValidationApi.MakeBindingHandlerValidatable("timeofday");
            }


        }
        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            
            string formatString=GetFormatString();
            jQueryObject container = jQuery.FromElement(element);

            jQueryObject inputField = container.Find(".sparkle-input-timeofday-part");
            jQueryObject selectButton = container.Find(".sparkle-input-timeofday-button-part"); 

            AutoCompleteOptions options = XrmTimeEditor.GetTimePickerAutoCompleteOptions(formatString);

            options.Position = new Dictionary<string, object>("collision", "fit");
            options.Select = delegate(jQueryEvent e, AutoCompleteSelectEvent uiEvent)
            {
               
                // Note we assume that the binding has added an array of string items
                string value = ((Dictionary)uiEvent.Item)["value"].ToString();
                
                TrySetObservable(valueAccessor, inputField, value);
            };

            inputField = inputField.Plugin<AutoCompleteObject>().AutoComplete(options);
           

            // Add the click binding to show the drop down
            selectButton.Click(delegate(jQueryEvent e)
            {
               
                inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Search,""); // Give "" to show all items
            });
            //// Set initial value
            //Observable<DateTime> dateValueAccessor = (Observable<DateTime>)valueAccessor();
            //DateTime intialValue = dateValueAccessor.GetValue();
            //FormatterUpdate(inputField, intialValue);

            //handle the field changing
            KnockoutUtils.RegisterEventHandler(inputField.GetElement(0), "change", delegate(object sender, EventArgs e)
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

            // Note: Because the time picker is always part of the date picker - we don't need to display validation messages
            //Knockout.BindingHandlers["validationCore"].Init(element, valueAccessor, allBindingsAccessor, null, null);

        }

        private static void TrySetObservable(Func<object> valueAccessor, jQueryObject inputField, string value)
        {

            Observable<DateTime> observable = (Observable<DateTime>)valueAccessor();
            bool isValid = true;

            // Test the format
            DateTime testDate = DateTimeEx.AddTimeToDate(observable.GetValue(), value);
            
            // Check if the value is different
            string newValue = (testDate==null) ? "" : testDate.ToString();
            string originalValue = (observable.GetValue() == null) ? "" : observable.GetValue().ToString();

            if (newValue == originalValue)
                return;

            if (testDate == null)
            {
                // Invalid
                // CGCHANGE - For solution checker - debug scripts error
                //Script.Alert("Invalid Time");
                inputField.Focus();
               
                DateTime currentValue = observable.GetValue();
                FormatterUpdate(inputField, currentValue);
            }
            else
            {
                observable.SetValue(testDate);

                if (((string)Script.Literal("typeof({0}.isValid)", observable)) != "undefined")
                {
                    isValid = ((IValidatedObservable)observable).IsValid() == true;
                }

                if (isValid)
                {
                    //inputField.Blur();

                }
            }
        }
        private static void FormatterUpdate(jQueryObject inputField, DateTime value)
        {
            string formatString = GetFormatString();
            string formattedValue = "";
            if (value != null)
            {
                formattedValue = value.Format(formatString);

            }
            inputField.Value(formattedValue);
        }

        private static string GetFormatString()
        {
             string timeFormatString="h:mm tt";
             if (OrganizationServiceProxy.UserSettings != null)
             {
                 timeFormatString = OrganizationServiceProxy.UserSettings.TimeFormatString;
             }
             return timeFormatString;
        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            jQueryObject container = jQuery.FromElement(element);

            jQueryObject inputField = container.Find(".sparkle-input-timeofday-part");
            DateTime value = (DateTime) KnockoutUtils.UnwrapObservable(valueAccessor());
            string formatString = GetFormatString();
            string formattedValue = DateTimeEx.FormatTimeSpecific(value, formatString);
            inputField.Value((string)formattedValue);
           
        }

    }
}
