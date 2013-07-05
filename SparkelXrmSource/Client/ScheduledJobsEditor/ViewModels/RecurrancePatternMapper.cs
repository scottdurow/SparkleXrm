// RecurranceInterval.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;

namespace Client.ScheduledJobsEditor.ViewModels
{
    public class RecurranceFrequencyNames
    {
        public const string YEARLY = "YEARLY";
        public const string WEEKLY = "WEEKLY";
        public const string DAILY = "DAILY";
        public const string HOURLY = "HOURLY";
        public const string MINUTELY = "MINUTELY";
    }
    public class DayNames
    {
        public const string Monday = "MO";
        public const string Tuesday = "TU";
        public const string Wednesday = "WE";
        public const string Thursday = "TH";
        public const string Friday = "FR";
        public const string Saturday = "SA";
        public const string Sunday = "SU";
    }
    [ScriptName("Object")]
    [IgnoreNamespace]
    [Imported]
    public class RecurranceFrequency
    {
        public string Name;
        public string Value;
        
    }
    public class RecurrancePatternMapper
    {
        public static RecurranceFrequency[] _frequencies;

        public static RecurranceFrequency[] RecurranceFrequencies
        {
            get{
                if (_frequencies==null)
                {
                    RecurranceFrequency[] values = new RecurranceFrequency[5];


                    values[0] = new RecurranceFrequency();
                    values[0].Value = RecurranceFrequencyNames.YEARLY;
                    values[0].Name = "Yearly";

                    values[1] = new RecurranceFrequency();
                    values[1].Value = RecurranceFrequencyNames.WEEKLY;
                    values[1].Name = "Week";

                    values[2] = new RecurranceFrequency();
                    values[2].Value = RecurranceFrequencyNames.DAILY;
                    values[2].Name = "Day";

                    values[3] = new RecurranceFrequency();
                    values[3].Value = RecurranceFrequencyNames.HOURLY;
                    values[3].Name = "Hour";

                    values[4] = new RecurranceFrequency();
                    values[4].Value = RecurranceFrequencyNames.MINUTELY;
                    values[4].Name = "Minute";
                    _frequencies = values;
                }
                return _frequencies;
            }
        }

        public static string Serialise(ScheduledJob scheduledJob)
        {
           
            List<string> parts = new List<string>();

            ArrayEx.Add(parts, "FREQ=" + scheduledJob.Recurrance.GetValue().Value);
            ArrayEx.Add(parts, "INTERVAL=" + scheduledJob.RecurEvery.GetValue().ToString());

            if (scheduledJob.Recurrance.GetValue().Value == RecurranceFrequencyNames.WEEKLY)
            {
                List<string> days = new List<string>();
                if (scheduledJob.Monday.GetValue()) days.Add(DayNames.Monday);
                if (scheduledJob.Tuesday.GetValue()) days.Add(DayNames.Tuesday);
                if (scheduledJob.Wednesday.GetValue()) days.Add(DayNames.Wednesday);
                if (scheduledJob.Thursday.GetValue()) days.Add(DayNames.Thursday);
                if (scheduledJob.Friday.GetValue()) days.Add(DayNames.Friday);
                if (scheduledJob.Saturday.GetValue()) days.Add(DayNames.Saturday);
                if (scheduledJob.Sunday.GetValue()) days.Add(DayNames.Sunday);
                if (days.Count > 0)
                    ArrayEx.Add(parts, "BYDAY=" + days.Join(","));

            }

            if (!scheduledJob.NoEndDate.GetValue())
            {
                ArrayEx.Add(parts, "COUNT=" + scheduledJob.Count.GetValue());
            }
            string pattern = parts.Join(";");
            return pattern;
        }
        

        public static void DeSerialise(ScheduledJob scheduledJob, string patternString)
        {
             
            // Unpack the pattern
            string[] pattern = patternString.Split(";");
            bool mon = false, tue = false, wed = false, thu = false, fri = false, sat = false, sun = false;
            int interval = 1;
            int? count = null;
            RecurranceFrequency frequency = null;

            foreach (string part in pattern)
            {
                string[] value = part.Split("=");

                switch (value[0])
                {
                    case "FREQ":
                        for (int i = 0; i < RecurrancePatternMapper.RecurranceFrequencies.Length; i++)
                        {
                            if (value[1] == RecurrancePatternMapper.RecurranceFrequencies[i].Value)
                            {
                                frequency = RecurrancePatternMapper.RecurranceFrequencies[i];
                                break;
                            }
                        }
                        break;
                    case "COUNT":
                        count = int.Parse(value[1]);
                        break;
                    case "INTERVAL":
                        interval = int.Parse(value[1]);
                        break;
                    case "BYDAY":
                        string[] days = value[1].Split(",");

                        foreach (string day in days)
                        {
                            switch (day)
                            {
                                case DayNames.Monday:
                                    mon = true;
                                    break;
                                case DayNames.Tuesday:
                                    tue = true;
                                    break;
                                case DayNames.Wednesday:
                                    wed = true;
                                    break;
                                case DayNames.Thursday:
                                    thu = true;
                                    break;
                                case DayNames.Friday:
                                    fri = true;
                                    break;
                                case DayNames.Saturday:
                                    sat = true;
                                    break;
                                case DayNames.Sunday:
                                    sun = true;
                                    break;

                            }


                        }
                        break;
                }


            }
            // Set Values

            
            scheduledJob.Recurrance.SetValue(frequency);
            scheduledJob.Monday.SetValue(mon);
            scheduledJob.Tuesday.SetValue(tue);
            scheduledJob.Wednesday.SetValue(wed);
            scheduledJob.Thursday.SetValue(thu);
            scheduledJob.Friday.SetValue(fri);
            scheduledJob.Saturday.SetValue(sat);
            scheduledJob.Sunday.SetValue(sun);
            scheduledJob.RecurEvery.SetValue(interval);
            scheduledJob.Count.SetValue(count);
            scheduledJob.NoEndDate.SetValue(count == null);
        
        }

    }
}
