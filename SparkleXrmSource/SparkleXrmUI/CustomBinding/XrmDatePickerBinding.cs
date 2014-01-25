using jQueryApi;
using jQueryApi.UI.Widgets;
using KnockoutApi;
using System;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace SparkleXrm.CustomBinding
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class XrmDateBindingOptions
    {
        public bool DateAndTime;
    }

    public class XrmDatePickerBinding : BindingHandler
    {
        static XrmDatePickerBinding()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                Knockout.BindingHandlers["datepicker"] = new XrmDatePickerBinding();
                ValidationApi.MakeBindingHandlerValidatable("datepicker");
            }
             
            
        }
        public override void  Init(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
        {
              jQueryObject container = jQuery.FromElement(element);
              jQueryObject dateTime = container.Find(".sparkle-input-datepicker-part");
              jQueryObject dateButton = container.Find(".sparkle-input-datepicker-button-part");
                // Add Date Picker
               DatePickerOptions2 options = new DatePickerOptions2();
               options.ShowOn = "";
               options.ButtonImageOnly = true;

               options.FirstDay = OrganizationServiceProxy.OrganizationSettings!=null ? OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value : 0;
               //options.ButtonImage = @"../images/btn_off_Cal.gif";

               string dateFormat = "dd/MM/yy";
               if (OrganizationServiceProxy.UserSettings != null)
               {
                   dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString;
               }
               options.DateFormat = dateFormat;


               dateTime.Plugin<DatePickerPlugIn>().DatePicker(options);
               //// Get current value
               //Observable<DateTime> dateValueAccessor = (Observable<DateTime>)valueAccessor();
               //DateTime intialValue = dateValueAccessor.GetValue();
               //dateTime.Plugin<DatePickerObject>().DatePicker(DatePickerMethod.SetDate, intialValue);
               
               dateButton.Click(delegate(jQueryEvent e)
               {
                   // Note: This is using a custom plugin definition since the standard didn't include show
                   dateTime.Plugin<DatePickerPlugIn>().DatePicker(DatePickerMethod2.Show);
               });
             
      
      

        //handle the field changing
               KnockoutUtils.RegisterEventHandler(dateTime.GetElement(0), "change", delegate(object sender, EventArgs e)
               {
            Observable<DateTime> observable = (Observable<DateTime>)valueAccessor();
            
          
            bool isValid = true;

            if (((string)Script.Literal("typeof({0}.IsValid)",observable))!="undefined")
            {
                isValid = ((IValidatedObservable)observable).IsValid() == true;
            }

            if (isValid)
            {
                DateTime selectedDate = (DateTime)dateTime.Plugin<DatePickerObject>().DatePicker(DatePickerMethod.GetDate);
                // Get Current observable value - we only want to set the date part
                DateTime currentValue = observable.GetValue();
                DateTimeEx.SetTime(selectedDate, currentValue);
                observable.SetValue(selectedDate);
            }
            dateTime.Blur();
            
        });

        Action disposeCallBack =  delegate() {
            Script.Literal("$({0}).datepicker(\"destroy\")",element);
        };

        //handle disposal (if KO removes by the template binding)
        Script.Literal("ko.utils.domNodeDisposal.addDisposeCallback({0}, {1})", element, (object)disposeCallBack);

        //Knockout.BindingHandlers["validationCore"].Init(element, valueAccessor, allBindingsAccessor,null,null);
        }
    

   public override void  Update(System.Html.Element element, Func<object> valueAccessor, Func<System.Collections.Dictionary> allBindingsAccessor, object viewModel, object context)
    {

        jQueryObject container = jQuery.FromElement(element);
        jQueryObject dateTime = container.Find(".sparkle-input-datepicker-part");
        object value = KnockoutUtils.UnwrapObservable(valueAccessor());

    
        if ((string)Script.Literal("typeof({0})", value) == "string")
        {
            value = Date.Parse((string)value);
        }
   
        dateTime.Plugin<DatePickerObject>().DatePicker(DatePickerMethod.SetDate, value);
       
    }

}
}
