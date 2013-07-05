// SessionsViewModel.cs
//

using System;
using System.Collections.Generic;
using Slick;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using SparkleXrm.GridEditor;

namespace Client.TimeSheet.ViewModel
{
    public class DaysViewModel : DataViewBase
    {
        #region Constructor
        public DaysViewModel(SessionsViewModel sessions)
        {
            this.sessions = sessions;
            this.sessions.OnRowsChanged.Subscribe(delegate(EventData args, object data){
                ReCalculate();

            });


            this.OnSelectedRowsChanged += delegate()
            {
                
            };

            sessions.OnRowsChanged.Subscribe(delegate(EventData args, object data)
            {
                // Ensure that when data has changed in the sessions, the days view is updated
                this.Refresh();
            });
        }
        #endregion

        #region Fields
       
       
        private SessionsViewModel sessions;
       
        private DayEntry totals;
        private Dictionary<string,DayEntry> days;
       
        private List<DayEntry> rows = new List<DayEntry>();
        public int? _selectedDay;
        #endregion
        

        /// <summary>
        /// The day selected by the user - if 0 then all days are selected
        /// </summary>
        public int? SelectedDay
        {
            get { return _selectedDay; }
            set
            {

                _selectedDay = null;
                // Load the sessions for the selected activity in the timesheet row
                // Get the selected activity
                DayEntry[] selectedItems = this.SelectedItems;
                if (selectedItems != null && selectedItems.Length > 0)
                {
                    // The item selected may be a new row
                    if (selectedItems[0] != null)
                    {

                        sessions.SetCurrentActivity(selectedItems[0].Activity, (int)value);
                        _selectedDay = value;

                    }
                }

            }
        }
        public DayEntry[] SelectedItems
        {
            get
            {
                if (this._selectedRows == null)
                    return new DayEntry[0];

                DayEntry[] items = new DayEntry[this._selectedRows.Length];
                int i = 0;
                foreach (SelectedRange row in this._selectedRows)
                {
                    items[i] = (DayEntry)this.GetItem(row.FromRow.Value);
                    i++;
                }
                return items;
            }
        }
        public override PagingInfo GetPagingInfo()
        {
            
            return null;
        }




        public void RaiseOnDateChanged()
        {
            this.OnRowsChanged.Notify(null, null, this);
        }



        public override int GetLength()
        {
            
            return rows.Count;
        }

        public override object GetItem(int index)
        {
           
            
            return rows[index];
        }

      


        public override void SetPagingOptions(PagingInfo p)
        {
            
        }

        public override void Refresh()
        {
            if (rows != null)
            {
                DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                args.From = 0;
                args.To = rows.Count - 1;
                this.OnDataLoaded.Notify(args, null, null);
                this.OnRowsChanged.Notify(null, null, this);
            }

           
        }

        public void ReCalculate()
        {
            // Calculate Totals by Activity and Day

            // Show header row of totals

            List<dev1_session> sessionData = sessions.GetCurrentWeek();
            DateTime weekStart = sessions.WeekStart;

            days = new Dictionary<string,DayEntry>();
            totals = new DayEntry();
            totals.isTotalRow = true;
            totals.ActivityName = "Total";
            totals.Activity = new EntityReference(null,null,null);
            totals.Activity.Name = "Total";
            foreach (dev1_session session in sessionData)
            {
                // Accumulate hours by Activity
                int dayOfWeek = session.dev1_StartTime.GetDay();
                string activity = session.dev1_ActivityId;

                if (days[activity] == null)
                {
                    DayEntry day = new DayEntry();
                    days[activity] = day;
                    day.Activity = new EntityReference(new Guid(session.dev1_ActivityId),null,null);
                   


                    if (session.dev1_TaskId != null)
                        day.Activity.Name = session.dev1_TaskId.Name;
                    else if (session.dev1_LetterId !=null)
                        day.Activity.Name = session.dev1_LetterId.Name;
                    else if (session.dev1_EmailId !=null)
                        day.Activity.Name = session.dev1_EmailId.Name;
                    else if (session.dev1_PhoneCallId != null)
                        day.Activity.Name = session.dev1_PhoneCallId.Name;
                   
                    day.Activity.LogicalName = session.dev1_ActivityTypeName;
                    
                }

                if (session.dev1_Duration != null)
                {
                    if (days[activity].Hours[dayOfWeek] == null)
                        days[activity].Hours[dayOfWeek] = 0;

                    days[activity].Hours[dayOfWeek] = days[activity].Hours[dayOfWeek] + session.dev1_Duration;

                    // Accumulate total hours
                    if (totals.Hours[dayOfWeek] == null)
                        totals.Hours[dayOfWeek] = 0;

                    totals.Hours[dayOfWeek] = totals.Hours[dayOfWeek] + session.dev1_Duration;
                }
            }

            // Flattern
            rows.Clear();
            rows.Add(totals);
            foreach (KeyValuePair<string,DayEntry> day in days)
            {
                day.Value.FlatternDays();
                rows.Add(day.Value);
            }
            totals.FlatternDays();

        
            Refresh();
        }



        public override void AddItem(object item)
        {
            
            
            dev1_session session = new dev1_session();
            DayEntry activity = (DayEntry)item;
            if ((activity.Activity != null) && (activity.Activity.Id!=null))
            {
                session.dev1_ActivityId = activity.Activity.Id.ToString();
                session.dev1_ActivityTypeName = activity.Activity.LogicalName;
                //session.activityName = activity.Activity.Name;
                session.dev1_ActivityId = activity.Activity.Id.ToString();
                session.dev1_StartTime = sessions.WeekStart;
                sessions.SelectedActivity = activity.Activity;
                sessions.AddItem(session);
                _selectedRows = new SelectedRange[1] { new SelectedRange() };
                _selectedRows[0].FromRow = rows.Count + 1;
                _selectedRows[0].ToRow = rows.Count + 1;
                Refresh();
            }
        }
    }
}
