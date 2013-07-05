// XrmCurrencySymbolBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;
using System.Collections.Generic;
using Xrm;
using Xrm.Sdk;
using Xrm.Services;

namespace SparkleXrm.CustomBinding
{
    public class XrmCurrencySymbolBinding : BindingHandler
    {
        static XrmCurrencySymbolBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["xrmcurrencysymbol"] = new XrmCurrencySymbolBinding();

                ValidationApi.MakeBindingHandlerValidatable("xrmcurrencysymbol");
            }


        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {


        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            jQueryObject currencyIndicatorSpan = jQuery.FromElement(element).Find(".sparkle-input-currencyprefix-part");

            Func<object> interceptAccesor = delegate()
            {
                string currencySymbol = getCurrencySymbol(valueAccessor);

                return currencySymbol;

            };

           
            Script.Literal("ko.bindingHandlers.text.update({0},{1},{2},{3},{4})", currencyIndicatorSpan.GetElement(0), interceptAccesor, allBindingsAccessor, viewModel, context);
        }


        public static string getCurrencySymbol(Func<object> valueAccessor)
        {
           

            EntityReference value = (EntityReference)KnockoutUtils.UnwrapObservable(valueAccessor());
            if (value != null)
            {
                return NumberEx.GetCurrencySymbol(value.Id);
            }

            return string.Empty;

        }

        

    }
}
