using jQueryApi;
using jQueryApi.UI.Widgets;
using KnockoutApi;
using System;
using System.Collections;

namespace SparkleXrm.CustomBinding
{
    public class XrmDurationBinding : BindingHandler
    {
        static XrmDurationBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["duration"] = new XrmDurationBinding();
                ValidationApi.MakeBindingHandlerValidatable("duration");
            }


        }
        public override void Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            jQueryObject container = jQuery.FromElement(element);
            jQueryObject inputField = container.Find(".sparkle-input-duration-part");
            jQueryObject selectButton = container.Find(".sparkle-input-duration-button-part");  
            AutoCompleteOptions options = new AutoCompleteOptions();
            options.Source = new string[] { "1 m", "2 m", "1 h", "2 h","1 d" };
            options.Delay = 0;
            options.MinLength = 0;

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

                inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Search, "");// Give "" to show all items
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
            //Knockout.BindingHandlers["validationCore"].Init(element, valueAccessor, allBindingsAccessor, null, null);

        }

        private static void TrySetObservable(Func<object> valueAccessor, jQueryObject inputField, string value)
        {
            
            Observable<int?> observable = (Observable<int?>)valueAccessor();
            bool isValid = true;

            bool isEmpty = (value==null) || (value.Length==0);
            
            // Get the value as minutes decimal
            // ([0-9]*) ((h(our)?[s]?)|(m(inute)?[s]?)|(d(ay)?[s]?))
            string pattern = @"/([0-9]*)[ ]?((h(our)?[s]?)|(m(inute)?[s]?)|(d(ay)?[s]?))/g";
            RegularExpression regex = RegularExpression.Parse(pattern);
            string[] match = regex.Exec(value);
            if (isEmpty)
            {
                observable.SetValue(null);
            }
            else if (match!=null && match.Length > 0)
            {
                // Get value
                decimal durationNumber = decimal.Parse(match[1]);
                switch (match[2].Substr(0, 1).ToLowerCase())
                {
                    case "d":
                        durationNumber = durationNumber * 60 * 24;
                        break;
                    case "h":
                        durationNumber = durationNumber *60;
                        break;
                }

                observable.SetValue((int?)durationNumber);

                if (((string)Script.Literal("typeof({0}.isValid)", observable)) != "undefined")
                {
                    isValid = ((IValidatedObservable)observable).IsValid() == true;
                }

                if (isValid)
                {
                    inputField.Blur();
                }
            }
            else
            {
                Script.Alert("Invalid Duration Format");
                int? currentValue = observable.GetValue();
                string durationString = formatDuration(currentValue);

                inputField.Value(durationString);
                inputField.Focus();
            }
            
        }

      


        public override void Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
            jQueryObject container = jQuery.FromElement(element);
            jQueryObject inputField = container.Find(".sparkle-input-duration-part");
            object value = KnockoutUtils.UnwrapObservable(valueAccessor());

            // Get the value in duration format
            int? duration = (int?)value;
            string durationString = formatDuration(duration);

            inputField.Value(durationString);
           
        }

        private static string formatDuration(int? duration)
        {
            string durationString = null;
            if (duration != null)
            {
                if (duration > (60 * 24))
                {
                    // days
                    durationString = String.Format("{0} d", duration / (60.0 * 24));

                }
                else if (duration == (60 * 24))
                {
                    // day
                    durationString = String.Format("{0} d", duration / (60.0 * 24));

                }

                else if (duration > 60)
                {
                    // hours
                    durationString = String.Format("{0} h", duration / (60.0));
                }
                else if (duration == 60)
                {
                    // hour
                    durationString = String.Format("{0} h", duration / (60.0));
                }
                else
                {
                    // minute
                    durationString = String.Format("{0} m", duration);
                }

            }
            else
                durationString = null;
            return durationString;
        }

    }
}
