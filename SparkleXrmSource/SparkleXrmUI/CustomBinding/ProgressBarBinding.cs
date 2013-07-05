// ProgressBarBinding.cs
//

using jQueryApi;
using jQueryApi.UI.Widgets;
using KnockoutApi;
using System;

namespace SparkleXrm.CustomBinding
{
    public class ProgressBarBinding : BindingHandler
    {
        static ProgressBarBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["progressBar"] = new ProgressBarBinding();
            }


        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
          
            jQuery.FromElement(element).Plugin<ProgressBarObject>().ProgressBar(); 
        }

        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            Observable<float> observable = (Observable<float>)valueAccessor();
            float value = KnockoutUtils.UnwrapObservable(observable);
            jQuery.FromElement(element).Plugin<ProgressBarObject>().ProgressBar(ProgressBarMethod.Value, value);

        }
    }
}
