// DayEntry.cs
//

using System;
using System.Collections.Generic;
using Xrm;
using Xrm.Sdk;

namespace Client.TimeSheet.ViewModel
{
    public class DayEntry : Entity
    {
        public DayEntry() : base("dayentry")  { }

        public DateTime Date;
        public EntityReference Activity;
        public EntityReference Account;
        public EntityReference RegardingObjectId;
        public string ActivityName;
        public int?[] Hours = new int?[6];
        public bool isTotalRow;
        public string icon;
        public int? Day0;
        public int? Day1;
        public int? Day2;
        public int? Day3;
        public int? Day4;
        public int? Day5;
        public int? Day6;

        public void FlatternDays()
        {
            Day0 = Hours[0];
            Day1 = Hours[1];
            Day2 = Hours[2];
            Day3 = Hours[3];
            Day4 = Hours[4];
            Day5 = Hours[5];
            Day6 = Hours[6];
          
        }
    }
}
