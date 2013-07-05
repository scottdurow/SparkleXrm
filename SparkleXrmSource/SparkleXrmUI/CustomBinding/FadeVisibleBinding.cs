// FadeVisibleBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;

namespace SparkleXrm.CustomBinding
{
    public class FadeVisibleBinding : BindingHandler
    {
        static FadeVisibleBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["fadeVisible"] = new FadeVisibleBinding();
            }


        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            Observable<bool> observable = (Observable<bool>)valueAccessor();


            // Initially set the element to be instantly visible/hidden depending on the value
            jQuery.FromElement(element).Toggle(KnockoutUtils.UnwrapObservable(observable));
        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            // Whenever the value subsequently changes, slowly fade the element in or out
            Observable<bool> observable = (Observable<bool>)valueAccessor();

            if (KnockoutUtils.UnwrapObservable(observable))
            {
                jQuery.FromElement(element).FadeIn();
            }
            else
            {
                jQuery.FromElement(element).FadeOut();
            }

        }

    }
}

