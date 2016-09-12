// DatePicker.cs
//

using jQueryApi;
using System;
using System.Runtime.CompilerServices;

namespace SparkleXrm
{
    public delegate void DatePickerOnSelect(string dateString, object instance);


    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public sealed class DatePickerOptions2
    {
        public string ShowOn;
        public int? FirstDay;
        public bool ShowOtherMonths;
        public bool ButtonImageOnly;
        public string ButtonImage;
        public Action BeforeShow;
        public Action OnClose;
        public string DateFormat;
        public int NumberOfMonths;
        public DatePickerOnSelect OnSelect;
        public DatePickerOptions2()
        {
        }

        public DatePickerOptions2(params object[] nameValuePairs)
        {
        }
    }
    // Summary:
    //     Operations supported by DatePicker.
    [Imported]
    [IgnoreNamespace]
    [NamedValues]
    public enum DatePickerMethod2
    {
        // Summary:
        //     The destroy() method cleans up all common data, events, etc. and then delegates
        //     out to _destroy() for custom cleanup.
        Destroy = 0,
        //
        // Summary:
        //     Function
        Dialog = 1,
        Disable = 2,
        Enable = 3,
        //
        // Summary:
        //     Returns the current date for the datepicker or null if no date has been selected.
        GetDate = 4,
        //
        // Summary:
        //     Determine whether a date picker has been disabled.
        IsDisabled = 5,
        Option = 6,
        //
        // Summary:
        //     Redraw a date picker, after having made some external modifications.
        Refresh = 7,
        //
        // Summary:
        //     Sets the current date for the datepicker. The new date may be a Date object
        //     or a string in the current [[UI/Datepicker#option-dateFormat|date format]]
        //     (e.g. '01/26/2009'), a number of days from today (e.g. +7) or a string of
        //     values and periods ('y' for years, 'm' for months, 'w' for weeks, 'd' for
        //     days, e.g. '+1m +7d'), or null to clear the selected date.
        SetDate = 8,
        Widget = 9,
        Show = 10,
        Hide = 11
    }
    #region Script# Support
    [Imported]
    [IgnoreNamespace]
    public sealed class DatePickerPlugIn : jQueryObject
    {
        [ScriptName("datepicker")]
        public jQueryObject DatePicker()
        {
            return null;
        }

        [ScriptName("datepicker")]
        public object DatePicker(DatePickerMethod2 action)
        {
            return null;
        }
        [ScriptName("datepicker")]
        public object DatePicker(DatePickerOptions2 options)
        {
            return null;
        }
    }
    #endregion
}