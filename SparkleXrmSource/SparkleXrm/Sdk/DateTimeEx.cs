// DateTimeEx.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    public class DateTimeEx
    {
      
        public static string ToXrmString(DateTime date)
        {
            // Get a date string from the Date object
            //2011-04-20T14:00:00Z
            // NOTE: we assume that the local date is in fact converted to UTC already
            // This avoids any local browser timezone missmatches with the user settings timezone
            string month = DateTimeEx.PadNumber(date.GetMonth() + 1, 2);
            string day = DateTimeEx.PadNumber(date.GetDate(), 2);
            string hours = DateTimeEx.PadNumber(date.GetHours(), 2);
            string mins = DateTimeEx.PadNumber(date.GetMinutes(), 2);
            string secs = DateTimeEx.PadNumber(date.GetSeconds(), 2);
            return String.Format("{0}-{1}-{2}T{3}:{4}:{5}Z", date.GetFullYear(), month, day, hours, mins, secs);
        }
        public static string ToXrmStringUTC(DateTime date)
        {
            // Get a date string from the Date object
            //2011-04-20T14:00:00Z
            // NOTE: we assume that the local date is in fact converted to UTC already
            // This avoids any local browser timezone missmatches with the user settings timezone
            string month = DateTimeEx.PadNumber(date.GetUTCMonth() + 1, 2);
            string day = DateTimeEx.PadNumber(date.GetUTCDate(), 2);
            string hours = DateTimeEx.PadNumber(date.GetUTCHours(), 2);
            string mins = DateTimeEx.PadNumber(date.GetUTCMinutes(), 2);
            string secs = DateTimeEx.PadNumber(date.GetUTCSeconds(), 2);
            return String.Format("{0}-{1}-{2}T{3}:{4}:{5}Z", date.GetUTCFullYear(), month, day, hours, mins, secs);
        }
        private static string PadNumber(int number, int length)
        {
            string str = number.ToString();
            while (str.Length < length)
                str = '0' + str;
            return str;
        }
        public static DateTime Parse(string dateString)
        {
            if (!String.IsNullOrEmpty(dateString))
            {
                DateTime dateTimeParsed = (DateTime)((object)Date.Parse(dateString));

                return dateTimeParsed;
            }
            else
                return null;
        }
        /// <summary>
        /// Formats a duration in minutes to a string
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string FormatDuration(int? totalMinutes)
        {
            if (totalMinutes != null)
            {

                int? toatalSeconds = totalMinutes * 60;

                int? days = Math.Floor(toatalSeconds / 86400);
                int? hours = Math.Floor((toatalSeconds % 86400) / 3600);
                int? minutes = Math.Floor(((toatalSeconds % 86400) % 3600) / 60);
                int? seconds = ((toatalSeconds % 86400) % 3600) % 60;
                List<string> formatString = new List<string>();
                if (days>0)
                    ArrayEx.Add(formatString,"{0}d");
                
                if (hours>0)
                    ArrayEx.Add(formatString,"{1}h");

                if (minutes > 0)
                    ArrayEx.Add(formatString, "{2}m");

                // LED: 9/26/2013: If days and hours are 0 and minutes are 0 we want to at least show 0m so the value can be editited.
                if (days == 0 && hours == 0 && minutes == 0)
                {
                    ArrayEx.Add(formatString, "{2}m");
                }

                return String.Format(ArrayEx.Join((Array)formatString," "), days, hours, minutes);
            }
            else
                return "";
        }
        public static int? ParseDuration(string duration)
        {
            bool isEmpty = (duration == null) || (duration.Length == 0);
            if (isEmpty)
            {
                return null;
            }
            // Get the value as minutes decimal
            // ([0-9]*) ((h(our)?[s]?)|(m(inute)?[s]?)|(d(ay)?[s]?))
            string pattern = @"/([0-9.]*)[ ]?((h(our)?[s]?)|(m(inute)?[s]?)|(d(ay)?[s]?))/g";

            RegularExpression regex = RegularExpression.Parse(pattern);
            bool matched = false;
            bool thisMatch = false;
            int totalDuration = 0;
            do
            {
                string[] match = regex.Exec(duration);
               
                thisMatch = (match != null && match.Length > 0);
                matched = matched || thisMatch;
                if (thisMatch)
                {

                    // Get value
                    decimal durationNumber = decimal.Parse(match[1]);
                    switch (match[2].Substr(0, 1).ToLowerCase())
                    {
                        case "d":
                            durationNumber = durationNumber * 60 * 24;
                            break;
                        case "h":
                            durationNumber = durationNumber * 60;
                            break;

                    }

                    totalDuration += Math.Floor(durationNumber);
                    // Remove matched item
                    duration.Replace(match[0], "");
                }
            } while (thisMatch);

            if (matched)
                return (int?)totalDuration;
            else
                return null;
        }

        public static DateTime AddTimeToDate(DateTime date, string time)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            if (time != null)
            {

                DateTime dateWithTime = DateTime.Parse("01 Jan 2000 " + time.Replace(".", ":").Replace("-", ":").Replace(",", ":"));
                DateTime newDate = new DateTime(date.GetTime());
                if (!Number.IsNaN((Number)((object)dateWithTime)))
                {
                    newDate.SetHours(dateWithTime.GetHours());
                    newDate.SetMinutes(dateWithTime.GetMinutes());
                    newDate.SetSeconds(dateWithTime.GetSeconds());
                    newDate.SetMilliseconds(dateWithTime.GetMilliseconds());
                    return newDate;
                }
                return null;
            }
            return date;

        }
        
        public static DateTime LocalTimeToUTCFromSettings(DateTime LocalTime, UserSettings settings)
        {
            return LocalTimeToUTC(LocalTime,
                settings.TimeZoneBias,
                settings.TimeZoneDaylightBias,
                settings.TimeZoneDaylightYear,
                settings.TimeZoneDaylightMonth,
                settings.TimeZoneDaylightDay,
                settings.TimeZoneDaylightHour,
                settings.TimeZoneDaylightMinute,
                settings.TimeZoneDaylightSecond,
                0,
                settings.TimeZoneDaylightDayOfWeek,
                settings.TimeZoneStandardBias,
                settings.TimeZoneStandardYear,
                settings.TimeZoneStandardMonth,
                settings.TimeZoneStandardDay,
                settings.TimeZoneStandardHour,
                settings.TimeZoneStandardMinute,
                settings.TimeZoneStandardSecond,
                0,
                settings.TimeZoneStandardDayOfWeek);
            
        }
        public static DateTime LocalTimeToUTC(DateTime LocalTime,
            int? Bias,
            int? DaylightBias,
            int? DaylightYear,
            int? DaylightMonth,
            int? DaylightDay,
            int? DaylightHour,
            int? DaylightMinute,
            int? DaylightSecond,
            int? DaylightMilliseconds,
            int? DaylightWeekday,
            int? StandardBias,
            int? StandardYear,
            int? StandardMonth,
            int? StandardDay,
            int? StandardHour,
            int? StandardMinute,
            int? StandardSecond,
            int? StandardMilliseconds,
            int? StandardWeekday)
        {
           
            int? TimeZoneBias;
            int? NewTimeZoneBias;
            int? LocalCustomBias;
            DateTime StandardTime;
            DateTime DaylightTime;
            DateTime ComputedUniversalTime;
            bool bDaylightTimeZone;

            // Get the new timezone bias
            NewTimeZoneBias = Bias;


            // Now see if we have stored cutover times


            if ((StandardMonth != 0) && (DaylightMonth != 0))
            {
                // We have timezone cutover information. Compute the
                // cutover dates and compute what our current bias
                // is
                StandardTime = GetCutoverTime(LocalTime,
                                                        StandardYear,
                                                        StandardMonth,
                                                        StandardDay,
                                                        StandardHour,
                                                        StandardMinute,
                                                        StandardSecond,
                                                        StandardMilliseconds,
                                                        StandardWeekday);

                if (StandardTime == null)
                {
                    return null;
                }

                DaylightTime = GetCutoverTime(LocalTime,
                                                        DaylightYear,
                                                        DaylightMonth,
                                                        DaylightDay,
                                                        DaylightHour,
                                                        DaylightMinute,
                                                        DaylightSecond,
                                                        DaylightMilliseconds,
                                                        DaylightWeekday);

                if (DaylightTime == null)
                {
                    return null;
                }
                // If daylight < standard, then time >= daylight and
                // less than standard is daylight
                if (DaylightTime < StandardTime)
                {
                    // If today is >= DaylightTime and < StandardTime, then
                    // We are in daylight savings time
                    if ((LocalTime >= DaylightTime) &&
                         (LocalTime < StandardTime))
                    {
                        bDaylightTimeZone = true;
                    }
                    else
                    {
                        bDaylightTimeZone = false;
                    }
                }
                else
                {

                    // If today is >= StandardTime and < DaylightTime, then
                    // We are in standard time
                    if ((LocalTime >= StandardTime) &&
                         (LocalTime < DaylightTime))
                    {
                        bDaylightTimeZone = false;
                    }
                    else
                    {
                        bDaylightTimeZone = true;
                    }
                }

                // At this point, we know our current timezone and the
                // local time of the next cutover.
                if (bDaylightTimeZone)
                {
                    LocalCustomBias = DaylightBias;
                }
                else
                {
                    LocalCustomBias = StandardBias;
                }

                TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
            }
            else
            {
                TimeZoneBias = NewTimeZoneBias;
            }

            ComputedUniversalTime = DateAdd(DateInterval.Minutes, (int)TimeZoneBias, LocalTime);

            return ComputedUniversalTime;

        }
        public static DateTime UTCToLocalTimeFromSettings(DateTime UTCTime, UserSettings settings)
        {
            return UTCToLocalTime(UTCTime,
                settings.TimeZoneBias,
                settings.TimeZoneDaylightBias,
                settings.TimeZoneDaylightYear,
                settings.TimeZoneDaylightMonth,
                settings.TimeZoneDaylightDay,
                settings.TimeZoneDaylightHour,
                settings.TimeZoneDaylightMinute,
                settings.TimeZoneDaylightSecond,
                0,
                settings.TimeZoneDaylightDayOfWeek,
                settings.TimeZoneStandardBias,
                settings.TimeZoneStandardYear,
                settings.TimeZoneStandardMonth,
                settings.TimeZoneStandardDay,
                settings.TimeZoneStandardHour,
                settings.TimeZoneStandardMinute,
                settings.TimeZoneStandardSecond,
                0,
                settings.TimeZoneStandardDayOfWeek);
            
        }

        public static DateTime UTCToLocalTime(DateTime UTCTime,
            int? Bias,
            int? DaylightBias,
            int? DaylightYear,
            int? DaylightMonth,
            int? DaylightDay,
            int? DaylightHour,
            int? DaylightMinute,
            int? DaylightSecond,
            int? DaylightMilliseconds,
            int? DaylightWeekday,
            int? StandardBias,
            int? StandardYear,
            int? StandardMonth,
            int? StandardDay,
            int? StandardHour,
            int? StandardMinute,
            int? StandardSecond,
            int? StandardMilliseconds,
            int? StandardWeekday)
        {

            int? TimeZoneBias = 0;
            int? NewTimeZoneBias = 0;
            int? LocalCustomBias = 0;
            DateTime StandardTime;
            DateTime DaylightTime;
            DateTime UtcStandardTime;
            DateTime UtcDaylightTime;
            DateTime ComputedLocalTime;
            bool bDaylightTimeZone;

            // Get the timezone information into a useful format
            // Get the new timezone bias
            NewTimeZoneBias = Bias;

            // Now see if we have stored cutover times
            if ((StandardMonth != 0) && (DaylightMonth != 0))
            {
                // We have timezone cutover information. Compute the
                // cutover dates and compute what our current bias
                // is
                StandardTime = GetCutoverTime(UTCTime,
                                 StandardYear,
                                 StandardMonth,
                                 StandardDay,
                                 StandardHour,
                                 StandardMinute,
                                 StandardSecond,
                                 StandardMilliseconds,
                                 StandardWeekday);

                if (StandardTime == null)
                {
                    return null;
                }

                DaylightTime = GetCutoverTime(UTCTime,
                                DaylightYear,
                                DaylightMonth,
                                DaylightDay,
                                DaylightHour,
                                DaylightMinute,
                                DaylightSecond,
                                DaylightMilliseconds,
                                DaylightWeekday);

                if (DaylightTime == null)
                {
                    return null;
                }

                // Convert standard time and daylight time to utc
                LocalCustomBias = StandardBias;
                TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
                UtcDaylightTime = DateTimeEx.DateAdd(DateInterval.Minutes, (int)TimeZoneBias, DaylightTime);

                LocalCustomBias = DaylightBias;
                TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
                UtcStandardTime = DateTimeEx.DateAdd(DateInterval.Minutes, (int)TimeZoneBias, StandardTime);


                //If daylight < standard, then time >= daylight and
                //less than standard is daylight
                if (UtcDaylightTime < UtcStandardTime)
                {

                    // If today is >= DaylightTime and < StandardTime, then
                    // We are in daylight savings time
                    if ((UTCTime >= UtcDaylightTime) &&
                         (UTCTime < UtcStandardTime))
                    {
                        bDaylightTimeZone = true;
                    }
                    else
                    {
                        bDaylightTimeZone = false;
                    }
                }
                else
                {
                    // If today is >= StandardTime and < DaylightTime, then
                    // We are in standard time
                    if ((UTCTime >= UtcStandardTime) &&
                         (UTCTime < UtcDaylightTime))
                    {
                        bDaylightTimeZone = false;
                    }
                    else
                    {
                        bDaylightTimeZone = true;
                    }
                }

                // At this point, we know our current timezone and the
                // Universal time of the next cutover.
                if (bDaylightTimeZone)
                {
                    LocalCustomBias = DaylightBias;
                }
                else
                {
                    LocalCustomBias = StandardBias;
                }

                TimeZoneBias = NewTimeZoneBias + LocalCustomBias;

            }
            else
            {
                TimeZoneBias = NewTimeZoneBias;
            }

            ComputedLocalTime = DateTimeEx.DateAdd(DateInterval.Minutes, (int)TimeZoneBias * -1, UTCTime);

            return ComputedLocalTime;
        }

        private static DateTime GetCutoverTime(DateTime CurrentTime,
            int? Year,
            int? Month,
            int? Day,
            int? Hour,
            int? Minute,
            int? Second,
            int? Milliseconds,
            int? Weekday
            )
        {

            if (Year != 0)
                return null;

            DateTime WorkingTime;
            DateTime ScratchTime;
            int? BestWeekdayDate;
            int? WorkingWeekdayNumber;
            int? TargetWeekdayNumber;
            int? TargetYear;
            int? TargetMonth;
            int? TargetWeekday;      // range [0..6] == [Sunday..Saturday]

            // The time is an day in the month style time
            //   the convention is the Day is 1-5 specifying 1st, 2nd... Last
            //   day within the month. The day is WeekDay.

            // Compute the target month and year      
            TargetWeekdayNumber = Day;
            if ((TargetWeekdayNumber > 5) || (TargetWeekdayNumber == 0))
            {
                return null;
            }

            TargetWeekday = Weekday;
            TargetMonth = Month;
            TargetYear = CurrentTime.GetFullYear();

            BestWeekdayDate = 0;

            WorkingTime = DateTimeEx.FirstDayOfMonth(CurrentTime, (int)TargetMonth);
            WorkingTime = DateTimeEx.DateAdd(DateInterval.Hours, (int)Hour, WorkingTime);
            WorkingTime = DateTimeEx.DateAdd(DateInterval.Minutes, (int)Minute, WorkingTime);
            WorkingTime = DateTimeEx.DateAdd(DateInterval.Seconds, (int)Second, WorkingTime);
            WorkingTime = DateTimeEx.DateAdd(DateInterval.Milliseconds, (int)Milliseconds, WorkingTime);

            ScratchTime = WorkingTime;

            // Compute bias to target weekday      
            if (ScratchTime.GetDay() > TargetWeekday)
            {
                WorkingTime = DateTimeEx.DateAdd(DateInterval.Days, (int)(7 - (ScratchTime.GetDay() - TargetWeekday)), WorkingTime);
            }
            else if (ScratchTime.GetDay() < TargetWeekday)
            {
                WorkingTime = DateTimeEx.DateAdd(DateInterval.Days, (int)(TargetWeekday - ScratchTime.GetDay()), WorkingTime);
            }

            //  We are now at the first weekday that matches our target weekday     
            BestWeekdayDate = WorkingTime.GetDay();
            WorkingWeekdayNumber = 1;

            // Keep going one week at a time until we either pass the
            // target weekday, or we match exactly       
            ScratchTime = WorkingTime;

            while (WorkingWeekdayNumber < TargetWeekdayNumber)
            {
                WorkingTime = DateTimeEx.DateAdd(DateInterval.Days, 7, WorkingTime);
                if (WorkingTime.GetMonth() != ScratchTime.GetMonth())
                    break;
                ScratchTime = WorkingTime;
                WorkingWeekdayNumber = WorkingWeekdayNumber + 1;
            }

            return ScratchTime;
        }

        public static DateTime FirstDayOfMonth(DateTime date, int Month)
        {
            DateTime startOfMonth = new DateTime(date.GetTime());
            startOfMonth.SetMonth(Month-1);
            startOfMonth.SetDate(1);
            startOfMonth.SetHours(0);
            startOfMonth.SetMinutes(0);
            startOfMonth.SetSeconds(0);
            startOfMonth.SetMilliseconds(0);
            return startOfMonth;
        }

        public static DateTime DateAdd(DateInterval interval, int value, DateTime date)
        {
            int ms = date.GetTime();
            DateTime result;
            switch (interval)
            {
                case DateInterval.Milliseconds:
                    result = new DateTime(ms + value);
                    break;
                case DateInterval.Seconds:
                    result = new DateTime(ms + (value * 1000));
                    break;
                case DateInterval.Minutes:
                    result = new DateTime(ms + (value * 1000 * 60));
                    break;
                case DateInterval.Hours:
                    result = new DateTime(ms + (value * 1000 * 60 * 60));
                    break;
                case DateInterval.Days:
                    result = new DateTime(ms + (value * 1000 * 60 * 60 * 24));
                    break;
                default:
                    result = date;
                    break;
            }

            return result;
        }






        public static DateTime FirstDayOfWeek(DateTime date)
        {
            int weekStartOffset = 0;

            if (OrganizationServiceProxy.OrganizationSettings != null)
            {
                weekStartOffset = OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value;
            }

            DateTime startOfWeek = new DateTime(date.GetTime());
            int dayOfWeek = startOfWeek.GetDay();
            dayOfWeek = dayOfWeek - weekStartOffset;
            if (dayOfWeek < 0)
                dayOfWeek = 7+dayOfWeek;

            if (dayOfWeek > 0)
            {
                startOfWeek = DateTimeEx.DateAdd(DateInterval.Days, (int)(dayOfWeek*-1), startOfWeek);
            }
            
            startOfWeek.SetHours(0);
            startOfWeek.SetMinutes(0);
            startOfWeek.SetSeconds(0);
            startOfWeek.SetMilliseconds(0);
            return startOfWeek;
        }

        public static DateTime LastDayOfWeek(DateTime date)
        {
            int weekStartOffset = 0;

            if (OrganizationServiceProxy.OrganizationSettings != null)
            {
                weekStartOffset = OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value;
            }

            DateTime endOfWeek = new DateTime(date.GetTime());
           
            int dayOfWeek = endOfWeek.GetDay();
            dayOfWeek = dayOfWeek - weekStartOffset;
            if (dayOfWeek < 0)
                dayOfWeek = 7 + dayOfWeek;
            
          
            endOfWeek = DateTimeEx.DateAdd(DateInterval.Days, (int)(6 - dayOfWeek), endOfWeek);
            

            endOfWeek.SetHours(23);
            endOfWeek.SetMinutes(59);
            endOfWeek.SetSeconds(59);
            endOfWeek.SetMilliseconds(999);
            return endOfWeek;
        }
        public static string FormatTimeSpecific(DateTime dateValue, string formatString)
        {
            formatString = formatString.Replace(".", ":").Replace("-", ":").Replace(",", ":");
            if (dateValue != null && (typeof(DateTime) == dateValue.GetType()))
                return dateValue.Format(formatString);
            else
                return "";
        }
        public static string FormatDateSpecific(DateTime dateValue, string formatString)
        {
            if (dateValue != null)
            {
                return (string)Script.Literal(@"xrmjQuery.datepicker.formatDate( {0}, {1} )", formatString, dateValue);
            }
            else
                return "";
        }
        public static DateTime ParseDateSpecific(string dateValue, string formatString)
        {
            return (DateTime)Script.Literal(@"xrmjQuery.datepicker.parseDate( {0}, {1} )", formatString, dateValue);
        }
        public static void SetTime(DateTime date, DateTime time)
        {
            if (date != null && time != null)
            {
                date.SetHours(time.GetHours());
                date.SetMinutes(time.GetMinutes());
                date.SetSeconds(time.GetSeconds());
                date.SetMilliseconds(time.GetMilliseconds());
            }
        }
        public static void SetUTCTime(DateTime date, DateTime time)
        {
            if (date != null && time != null)
            {
                date.SetUTCHours(time.GetUTCHours());
                date.SetUTCMinutes(time.GetUTCMinutes());
                date.SetUTCSeconds(time.GetUTCSeconds());
                date.SetUTCMilliseconds(time.GetUTCMilliseconds());
            }
        }
        /// <summary>
        /// Gets the duration since midnight in seconds
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetTimeDuration(DateTime date)
        {
            return (date.GetHours() * (60 * 60)) + (date.GetMinutes() * 60) + (date.GetSeconds());
        }
    }

    [Imported]
    [IgnoreNamespace]
    [NamedValues]
    public enum DateInterval
    {
        Milliseconds = 0,
        Seconds = 1,
        Minutes = 2,
        Hours = 3,
        Days = 4

    }
}
