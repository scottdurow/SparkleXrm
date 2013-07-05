// DateTime.cs
//

using System;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    /// <summary>
    /// This class is an import of the Javascript Date type - so that we can use normal c# code with the DateTime type
    /// </summary>
    [IgnoreNamespace]
    [Imported]
    [ScriptName("Date")]
    public class DateTime
    {
        private Date _date;

        public DateTime() { }

        public DateTime(int milliseconds) { }

        public DateTime(string date) { }

        public DateTime(int year, int month, int date) { }

        public DateTime(int year, int month, int date, int hours) { }

        public DateTime(int year, int month, int date, int hours, int minutes) { }

        public DateTime(int year, int month, int date, int hours, int minutes, int seconds) { }

        public DateTime(int year, int month, int date, int hours, int minutes, int seconds, int milliseconds) { }

        // Summary:
        //     Returns the difference in milliseconds between two dates.
        public static int operator -(DateTime a, DateTime b) { return 0; }
        //
        // Summary:
        //     Compares two dates
        public static bool operator !=(DateTime a, DateTime b) { return true; }
        //
        // Summary:
        //     Compares two dates
        public static bool operator <(DateTime a, DateTime b) { return true; }
        //
        // Summary:
        //     Compares two dates
        public static bool operator <=(DateTime a, DateTime b) { return true; }
        //
        // Summary:
        //     Compares two dates
        public static bool operator ==(DateTime a, DateTime b) { return true; }
        //
        // Summary:
        //     Compares two dates
        public static bool operator >(DateTime a, DateTime b) { return true; }
        //
        // Summary:
        //     Compares two dates
        public static bool operator >=(DateTime a, DateTime b) { return true; }
       
        public Date ToJSDate()
        {
            return _date;
        }
        private static string PadNumber(int number, int length)
        {
            string str = number.ToString();
            while (str.Length < length)
                str = '0' + str;
            return str;
        }

        public string Format(string format)
        {
            return _date.Format(format);
        }
        
        public override string ToString()
        {
            return _date.ToString();
        }

        public int GetDate()
        {
            return _date.GetDate();
        }
        public int GetDay()
        {
            return _date.GetDay();
        }

        public int GetFullYear()
        {
            return _date.GetFullYear();
        }

        public int GetHours()
        {
            return _date.GetHours();
        }
        public int GetMilliseconds()
        {
            return _date.GetMilliseconds();
        }

        public int GetMinutes()
        {
            return _date.GetMinutes();
        }
        public int GetMonth()
        {
            return _date.GetMonth();
        }
        public int GetSeconds()
        {
            return _date.GetSeconds();
        }
        public int GetTime()
        {
            return _date.GetTime();
        }
        public int GetTimezoneOffset()
        {
            return _date.GetTimezoneOffset();
        }
        public int GetUTCDate()
        {
            return _date.GetUTCDate();
        }
        public int GetUTCDay()
        {
            return _date.GetUTCDay();
        }
        public int GetUTCFullYear()
        {
            return _date.GetUTCFullYear();
        }
        public int GetUTCHours()
        {
            return _date.GetUTCHours();
        }
        public int GetUTCMilliseconds()
        {
            return _date.GetUTCMilliseconds();
        }
        public int GetUTCMinutes()
        {
            return _date.GetUTCMinutes();
        }
        public int GetUTCMonth()
        {
            return _date.GetUTCMonth();
        }
        public int GetUTCSeconds()
        {
            return _date.GetUTCSeconds();
        }
        public static bool IsEmpty(Date d)
        {
            return Date.IsEmpty(d);
        }

        public string LocaleFormat(string format)
        {
            return _date.LocaleFormat(format);
        }
        [ScriptName("parseDate")]
        public static DateTime Parse(string value)
        {
            return (DateTime)((object)Date.Parse(value));
        }
        public void SetDate(int date)
        {
            _date.SetDate(date);
        }
        public void SetFullYear(int year)
        {
            _date.SetFullYear(year);
        }
        public void SetFullYear(int year, int month)
        {
            _date.SetFullYear(year, month);
        }
        public void SetFullYear(int year, int month, int day)
        {
            _date.SetFullYear(year, month, day);
        }
        public void SetHours(int hours)
        {
            _date.SetHours(hours);
        }
        public void SetMilliseconds(int milliseconds)
        {
            _date.SetMilliseconds(milliseconds);
        }
        public void SetMinutes(int minutes)
        {
            _date.SetMinutes(minutes);
        }
        public void SetMonth(int month)
        {
            _date.SetMonth(month);
        }
        public void SetSeconds(int seconds)
        {
            _date.SetSeconds(seconds);
        }
        public void SetTime(int milliseconds)
        {
            _date.SetTime(milliseconds);
        }
        public void SetUTCDate(int date)
        {
            _date.SetUTCDate(date);
        }
        public void SetUTCFullYear(int year)
        {
            _date.SetUTCFullYear(year);
        }
        public void SetUTCHours(int hours)
        {
            _date.SetUTCHours(hours);
        }
        public void SetUTCMilliseconds(int milliseconds)
        {
            _date.SetUTCMilliseconds(milliseconds);
        }
        public void SetUTCMinutes(int minutes)
        {
            _date.SetUTCMinutes(minutes);
        }
        public void SetUTCMonth(int month)
        {
            _date.SetUTCMonth(month);
        }
        public void SetUTCSeconds(int seconds)
        {
            _date.SetUTCSeconds(seconds);
        }
        public void SetYear(int year)
        {
            _date.SetYear(year);
        }
        public string ToDateString()
        {
            return _date.ToDateString();
        }
        public string ToISOString()
        {
            return _date.ToISOString();
        }
        public string ToLocaleDateString()
        {
            return _date.ToLocaleDateString();
        }
        public string ToLocaleTimeString()
        {
            return _date.ToLocaleTimeString();
        }
        public string ToTimeString()
        {
            return _date.ToTimeString();
        }
        public string ToUTCString()
        {
            return _date.ToUTCString();
        }
        [PreserveCase]
        public static int UTC(int year, int month, int day)
        {
            return Date.UTC(year, month, day);
        }
        [PreserveCase]
        public static int UTC(int year, int month, int day, int hours)
        {
            return Date.UTC(year, month, day, hours);
        }
        [PreserveCase]
        public static int UTC(int year, int month, int day, int hours, int minutes)
        {
            return Date.UTC(year, month, day, hours, minutes);
        }
        [PreserveCase]
        public static int UTC(int year, int month, int day, int hours, int minutes, int seconds)
        {
            return Date.UTC(year, month, day, hours, minutes, seconds);
        }
        [PreserveCase]
        public static int UTC(int year, int month, int day, int hours, int minutes, int seconds, int milliseconds)
        {
            return Date.UTC(year, month, day, hours, minutes, seconds, milliseconds);
        }

        public static DateTime Now { get { return null; } }

        public static DateTime Today { get { return null; } }
    }
}
