// EnterKeyBinding.cs
//

using jQueryApi;
using KnockoutApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.CustomBinding
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class KeyDownEventArgs : EventArgs
    {
        public int KeyCode;
        public jQueryObject Target;
        public void PreventDefault()
        {

        }

    }
    public class EnterKeyBinding : BindingHandler
    {
        static EnterKeyBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["enterKey"] = new EnterKeyBinding();
            }
        }

        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
           
            KnockoutUtils.RegisterEventHandler(element,"keydown",delegate(object sender, EventArgs e)
            {
                KeyDownEventArgs eventTyped = (KeyDownEventArgs)sender;
                if (eventTyped.KeyCode == 13)
                {
                    eventTyped.PreventDefault();
                    eventTyped.Target.Blur();
                    Script.Literal("{0}.call({1})", valueAccessor(), viewModel);
                }

            });
          
        }
    }
}
