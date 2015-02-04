// CrmTextBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;
using Xrm;
using Xrm.Sdk;

namespace SparkleXrm.CustomBinding
{
    public class XrmMoneyBinding: BindingHandler
    {
        static XrmMoneyBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["xrmmoney"] = new XrmMoneyBinding();

                ValidationApi.MakeBindingHandlerValidatable("xrmmoney");
            }


        }
        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            // Get the text box element
            jQueryObject textBox = jQuery.FromElement(element).Find(".sparkle-input-textbox-part");
            
            NumberFormatInfo format = GetNumberFormatInfo(allBindingsAccessor);
            
            object[] args = Arguments.Current;

            jQueryEventHandler onChangeHandler = delegate(jQueryEvent e)
            {
                Observable<string> observable = (Observable<string>)valueAccessor();
                string newValue = textBox.GetValue();
                TrySetObservable(valueAccessor, textBox, newValue, format);
               
            };

            textBox.Change(onChangeHandler);
                 
            // Stop further binding
            //Script.Literal("return { controlsDescendantBindings: true };");
        }

        private static NumberFormatInfo GetNumberFormatInfo(Func<System.Collections.Dictionary> allBindingsAccessor)
        {

            NumberFormatInfo format = NumberEx.GetCurrencyEditFormatInfo();
           

            if ((Number)allBindingsAccessor()["minvalue"] == null)
                format.MinValue = -2147483648;
            else
                format.MinValue = (Number)allBindingsAccessor()["minvalue"];

            if ((Number)allBindingsAccessor()["maxvalue"] == null)
                format.MaxValue = 2147483647;
            else
                format.MaxValue = (Number)allBindingsAccessor()["maxvalue"];
            return format;
        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            
            jQueryObject textBox = jQuery.FromElement(element).Find(".sparkle-input-textbox-part");
            NumberFormatInfo format = GetNumberFormatInfo(allBindingsAccessor);
            Func<object> interceptAccesor = delegate()
            {
                Money value = ((Observable<Money>)valueAccessor()).GetValue();
                if (value != null)
                {
                    return NumberEx.Format(value.Value,format);
                }
                else
                return String.Empty;

            };
            // Use the standard value binding from ko
            Script.Literal("ko.bindingHandlers.value.update({0},{1},{2},{3},{4})", textBox.GetElement(0), interceptAccesor, allBindingsAccessor, viewModel, context);
        }
        private static string FormatNumber(Money value, NumberFormatInfo format)
        {
            if (value != null)
            {
                return NumberEx.Format(value.Value,format);
            }
            else
            {
                return String.Empty;
            }

        }
        private static void TrySetObservable(Func<object> valueAccessor, jQueryObject inputField, string value, NumberFormatInfo format)
        {

            Observable<Money> observable = (Observable<Money>)valueAccessor();
            bool isValid = true;


            Number numericValue = NumberEx.Parse(value, format);

            if (!Number.IsNaN(numericValue) && numericValue>=format.MinValue && numericValue<=format.MaxValue)
            {
                Money newValue = null;
                if (numericValue != null) // Issue #46
                {
                    // Set to precision
                    numericValue = NumberEx.Round(numericValue, format.Precision);
                    newValue = new Money((decimal)numericValue);
                }

                observable.SetValue(newValue);

                if (((string)Script.Literal("typeof({0}.isValid)", observable)) != "undefined")
                {
                    isValid = ((IValidatedObservable)observable).IsValid() == true;
                }

                if (isValid)
                {
                    string formattedNumber = FormatNumber(newValue, format);
                    inputField.Value(formattedNumber);
                    inputField.Blur();
                }
            }
            else
            {
                Script.Alert(String.Format("You must enter a number between {0} and {1}",format.MinValue,format.MaxValue));
                Money currentValue = observable.GetValue();
              
                string formattedNumber = FormatNumber(currentValue, format);
                inputField.Value(formattedNumber);
                inputField.Focus();
            }

        }
    }
}
