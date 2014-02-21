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
            this.Sessions = sessions;
            bool inHander = false;
            this.Sessions.OnRowsChanged.Subscribe(delegate(EventData args, object data){
                ReCalculate();

            });


            this.OnSelectedRowsChanged += delegate()
            {
                _newRow=null;
            };

            this.OnRowsChanged.Subscribe(delegate(EventData args, object data)
            {
                if (inHander)
                    return;
                inHander = true;
                // Ensure if we are in progress of adding a new row, we complete it once we've got the activity
                if (_newRow != null && _newRow.Activity != null)
                {
                    DayEntry day = _newRow;
                    _newRow = null;

                    AddDefaultSession(day);
                }
                inHander = false;
            });

            sessions.OnRowsChanged.Subscribe(delegate(EventData args, object data)
            {

                // Ensure that when data has changed in the sessions, the days view is updated
                this.Refresh();

            });
        }

        #endregion

        #region Fields
       
       
        private SessionsViewModel Sessions;
        private DayEntry _newRow;
        private DayEntry totals;
        private Dictionary<string,DayEntry> days;
        private SortColData _sortCol = null;
        private List<DayEntry> rows = new List<DayEntry>();
        public int? _selectedDay;
        #endregion

        public void SetCurrentWeek(DateTime date)
        {
            // Cancel new row
            _newRow = null;
            this.Sessions.SetCurrentWeek(date);

        }
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

                        Sessions.SetCurrentActivity(selectedItems[0].Activity, (int)value);
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
                this.OnRowsChanged.Notify(args, null, this);
            }

           
        }

        public void ReCalculate()
        {
            if (_newRow != null)
                return;
            // Calculate Totals by Activity and Day

            // Show header row of totals

            List<dev1_session> sessionData = Sessions.GetCurrentWeek();
            DateTime weekStart = Sessions.WeekStart;

            days = new Dictionary<string,DayEntry>();
            totals = new DayEntry();
            totals.isTotalRow = true;
            totals.ActivityName = "Total";
            totals.Activity = new EntityReference(null,null,null);
            totals.Activity.Name = "Total";

           

            foreach (dev1_session session in sessionData)
            {
                // Accumulate hours by Activity
                if (session.dev1_StartTime == null)
                    continue;
                int dayOfWeek = session.dev1_StartTime.GetDay()-OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value;
                if (dayOfWeek < 0) dayOfWeek = 7 + dayOfWeek;
                string activity = session.dev1_ActivityId;

                if (days[activity] == null)
                {
                    DayEntry day = new DayEntry();
                    days[activity] = day;
                    day.Activity = new EntityReference(new Guid(session.dev1_ActivityId),null,null);
                    day.Activity.Name = session.activitypointer_subject;
                    day.RegardingObjectId = session.activitypointer_regardingobjectid;
                    
                    // Set the account name 
                    if (session.Account != null && day.Account == null)
                        day.Account = session.Account;

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

            SortData();
            Refresh();
        }



        public override void AddItem(object item)
        {

            DayEntry activity = (DayEntry)item;
            if ((activity.Activity != null) && (activity.Activity.Id != null))
            {
                AddDefaultSession(activity);
               
            }
            else
            {
                _newRow = activity;
                rows.Add(_newRow);
               
              
            }
            Refresh();
        }

        private void AddDefaultSession(DayEntry activity)
        {
            // Clear the sessions from the previously selected row, and show a blank row
            Sessions.SetCurrentActivity(activity.Activity, 0);

            dev1_session session = new dev1_session();
            session.Account = activity.Account;
            session.dev1_ActivityId = activity.Activity.Id.ToString();
            session.dev1_ActivityTypeName = activity.Activity.LogicalName;
            session.dev1_ActivityId = activity.Activity.Id.ToString();
            session.dev1_StartTime = Sessions.WeekStart;
            session.activitypointer_subject = activity.Activity.Name;
            session.activitypointer_regardingobjectid = activity.RegardingObjectId;
            Sessions.SelectedActivity = activity.Activity;
            session.dev1_Row = this.Sessions.GetCurrentWeek().Count;
            // Has the account been set - if not we need to look it up from the selected activity
            if (session.Account == null || session.activitypointer_regardingobjectid == null)
                SetAccountAndRegardingFromActivity(session);


            Sessions.AddItem(session);
            _selectedRows = new SelectedRange[1] { new SelectedRange() };
            _selectedRows[0].FromRow = rows.Count + 1;
            _selectedRows[0].ToRow = rows.Count + 1;
            
        }
        private void SetAccountAndRegardingFromActivity(dev1_session session)
        {
            string fetchXml = @"
                    <fetch>
                        <entity name='activitypointer' >
                                <attribute name='regardingobjectid' />
                                <filter type='and'>
                                    <condition attribute='activityid' operator='eq' value='{0}' />
                                </filter>
                                <link-entity name='contract' from='contractid' to='regardingobjectid' visible='false' link-type='outer' alias='contract' >
                                    <attribute name='customerid' alias='contract_customerid'/>
                                </link-entity>
                                <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' visible='false' link-type='outer' alias='opportunity' >
                                    <attribute name='customerid' alias='opportunity_customerid'/>
                                </link-entity>
                                <link-entity name='incident' from='incidentid' to='regardingobjectid' visible='false' link-type='outer' alias='incident' >
                                    <attribute name='customerid' alias='incident_customerid'/>
                                </link-entity>                        
                        </entity>
                    </fetch>";

            EntityCollection activities = OrganizationServiceProxy.RetrieveMultiple(String.Format(fetchXml, session.dev1_ActivityId));

            if (activities.Entities.Count > 0)
            {
                EntityReference account = null;
                // Get the account either via the regarding object, or the related contract, opportunity, incident
                Entity activity = activities.Entities[0];
                EntityReference incidentCustomerId = activity.GetAttributeValueEntityReference("incident_customerid");
                EntityReference opportunityCustomerId = activity.GetAttributeValueEntityReference("opportunity_customerid");
                EntityReference contractCustomerId = activity.GetAttributeValueEntityReference("contract_customerid");
                EntityReference regarding = activity.GetAttributeValueEntityReference("regardingobjectid");
                if (incidentCustomerId != null)
                    account= incidentCustomerId;
                else if (opportunityCustomerId != null)
                    account=  opportunityCustomerId;
                else if (contractCustomerId != null)
                    account=  contractCustomerId;
                else if (regarding != null && regarding.LogicalName == "account")
                    account=  regarding;

                session.Account = account;
                session.activitypointer_regardingobjectid = activity.GetAttributeValueEntityReference("regardingobjectid");

            }


        }
        public override void Sort(SortColData sorting)
        {
            _sortCol = sorting;
            SortData();
            Refresh();
        }
        private void SortData()
        {
            if (_sortCol == null)
                return;
            // Remove the total row
            DayEntry totalRow = rows[0];
            rows.RemoveAt(0);
            if (_sortCol.SortAsc == false)
            {
                rows.Reverse();
            }
            rows.Sort(delegate(DayEntry a, DayEntry b) { return Entity.SortDelegate(_sortCol.SortCol.Field, a, b); });

            if (_sortCol.SortAsc == false)
            {
                rows.Reverse();
            }

            // Add total row
            rows.Insert(0, totalRow);
        }
    }
}
