// FadeVisibleBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;

namespace SparkleXrm.CustomBinding
{
    public class AnimateVisible : BindingHandler
    {
        
        static AnimateVisible()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["animateVisible"] = new AnimateVisible();
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
            string effectIn = (string)((object)allBindingsAccessor()["effectIn"]);
            string effectOut = (string)((object)allBindingsAccessor()["effectOut"]);
            jQueryObject item = jQuery.FromElement(element);
            string effect = KnockoutUtils.UnwrapObservable(observable) ? effectIn : effectOut;
           
            switch (effect)
            {
                case "fadeIn":
                    item.FadeIn();
                    break;
                case "fadeOut":
                    item.FadeOut();
                    break;
                case "slideUp":
                    item.SlideUp();
                    break;
                case "slideDown":
                    item.SlideDown();
                    break;
                
            }
             
          


        }

    }
}

