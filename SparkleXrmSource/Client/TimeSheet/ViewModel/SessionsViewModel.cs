// SessionsViewModel.cs
//

using System;
using System.Collections.Generic;
using Slick;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Client.TimeSheet.Model;
using SparkleXrm.GridEditor;
using TimeSheet.Client.ViewModel;

namespace Client.TimeSheet.ViewModel
{
    public class SessionsViewModel : DataViewBase
    {
        #region Constructor
        public SessionsViewModel()
        {
         
            
        }
        #endregion

        #region Fields
      
        private Dictionary<int, List<dev1_session>> weeks = new Dictionary<int, List<dev1_session>>();
        private DateTime _selectedDate;
        private List<dev1_session> data = new List<dev1_session>();
        #endregion

        #region Public Fields
        public DateTime WeekStart;
        public DateTime WeekEnd;
        public DateTime SelectedDay;
        public Guid SelectedActivityID;
        public EntityReference SelectedActivity;
        #endregion

        #region Methods
        public List<dev1_session> GetCurrentWeek()
        {
            return weeks[WeekStart.GetTime()];
        }

        public void SetCurrentWeek(DateTime date)
        {
            
            _selectedDate = date;
            WeekStart = DateTimeEx.FirstDayOfWeek(date);
            WeekEnd= DateTimeEx.LastDayOfWeek(date);
            Refresh();
            
        }
        private int? GetSelectedDayIndex()
        {
            if (this.SelectedDay == null)
                return null;
            
            int daysDiff =  (this.SelectedDay-this.WeekStart)/(24*60*60*1000);
            return daysDiff+1;

        }
        /// <summary>
        /// When an activity row is selected in the 
        /// </summary>
        /// <param name="entityReference"></param>
        public void SetCurrentActivity(EntityReference entityReference, int day)
        {
            bool hasChanged = (entityReference!=null && entityReference.Id!=null? entityReference.Id.Value : null) != (this.SelectedActivity!=null && this.SelectedActivity.Id!=null? this.SelectedActivity.Id.Value : null);
            hasChanged = hasChanged || (day != GetSelectedDayIndex());
            if (!hasChanged)
                return;

            if (day > 0)
                this.SelectedDay = DateTimeEx.DateAdd(DateInterval.Days, day - 1, this.WeekStart);
            else
                this.SelectedDay = null;

            this.SelectedActivity = entityReference;
            if (entityReference != null && entityReference.Id != null)
                this.SelectedActivityID = entityReference.Id;
            else
                this.SelectedActivityID = null;
            RefreshActivityView();
        }

        private void AddSession(List<dev1_session> sessions, dev1_session session)
        {
            sessions.Add(session);
            // Set the account field derived from the regarding object (depending on type)
            if (session.opportunity_customerid != null)
                session.Account = session.opportunity_customerid;
            else if (session.contract_customerid != null)
                session.Account = session.contract_customerid;
            else if (session.incident_customerid != null)
                session.Account = session.incident_customerid;
            else if (session.activitypointer_regardingobjectid != null && session.activitypointer_regardingobjectid.LogicalName == "account")
                session.Account = session.activitypointer_regardingobjectid;
            
            // Calculate duration/end date
            if (session.dev1_EndTime == null && session.dev1_Duration!=null)
                OnDurationChanged(session);
            else if (session.dev1_Duration == null)
                OnStartEndDateChanged(session);
          
            // Is the session read only?
         
            if (session.StatusCode != null && session.StatusCode.Value.Value != (int)dev1_session_StatusCode.Draft)
                session.EntityState = EntityStates.ReadOnly;

            // Subscribe to the Property Changed event so we can re-calculate the duration or end date
            session.PropertyChanged += new Xrm.ComponentModel.PropertyChangedEventHandler(OnSessionPropertyChanged);
        }

        private void OnSessionPropertyChanged(object sender, Xrm.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "dev1_starttime") || (e.PropertyName == "dev1_endtime"))
            {
                OnStartEndDateChanged(sender);
            }
            else if (e.PropertyName == "dev1_duration")
            {
                OnDurationChanged(sender);
            }
            // TODO: This should really provide the row that is changing
            Refresh();
        }
        public override bool OnBeforeEdit(object item)
        {
            Entity session = (Entity)item;
            if (item!=null)
                return (session.EntityState != EntityStates.ReadOnly);
            else
                return true;
        }

        private static void OnDurationChanged(object sender)
        {
            dev1_session session = (dev1_session)sender;
            DateTime startTime = session.dev1_StartTime;

            if (startTime != null)
            {

                int? duration = session.dev1_Duration;
                int? startTimeMilliSeconds = startTime.GetTime();
                startTimeMilliSeconds = startTimeMilliSeconds + (duration * 1000 * 60);

                DateTime newEndDate = new DateTime((int)startTimeMilliSeconds);
                session.dev1_EndTime = newEndDate;

            }
        }

        private static void OnStartEndDateChanged(object sender)
        {
            // Calculate the duration
            dev1_session session = (dev1_session)sender;
            // Ensure that the end date is the same as the start date
            if (session.dev1_StartTime != null && session.dev1_EndTime != null)
            {
                session.dev1_EndTime.SetDate(session.dev1_StartTime.GetUTCDate());
                session.dev1_EndTime.SetMonth(session.dev1_StartTime.GetUTCMonth());
                session.dev1_EndTime.SetFullYear(session.dev1_StartTime.GetUTCFullYear());
            }
            session.dev1_Duration = SessionVM.CalculateDuration(session.dev1_StartTime, session.dev1_EndTime);
        }

        /// <summary>
        /// Only show the view of selected activity
        /// </summary>
        private void RefreshActivityView()
        {
            // filter sessions by activity and seleced day


            data = new List<dev1_session>();
            foreach (dev1_session session in GetCurrentWeek())
            {
                if (
                    (
                    (SelectedActivityID == null)
                    ||
                    (session.dev1_ActivityId == SelectedActivityID.ToString())
                    )
                    &&
                    (
                    (SelectedDay == null)
                    ||
                    (session.dev1_StartTime.GetDay() == SelectedDay.GetDay())
                    )
                    ) // TODO: filter day
                {
                    data.Add(session);
                }
            }
            // Notify
            DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
            args.From = 0;
            args.To = data.Count - 1;

            this.OnDataLoaded.Notify(args, null, null);
            this.OnRowsChanged.Notify(null, null, this);
        }

        /// <summary>
        /// Get all the new and inserted sessions
        /// </summary>
        /// <returns></returns>
        public List<dev1_session> GetEditedSessions()
        {
            List<dev1_session> edited = new List<dev1_session>();
            foreach (dev1_session session in weeks[WeekStart.GetTime()])
            {
                if (session.dev1_sessionId == null || session.EntityState == EntityStates.Changed || session.EntityState == EntityStates.Created)
                {
                    edited.Add(session);
                }

            }
            return edited;
        }
        #endregion

        #region Overridden Methods
        public override PagingInfo GetPagingInfo()
        {
            
            return null;
        }

        public override int GetLength()
        {
            if (data != null)
                return data.Count;
            else
                return 0;
        }
        public override void RemoveItem(object id)
        {
            // Only allow removing from current week selected
            weeks[WeekStart.GetTime()].Remove((dev1_session)id);
        }
        public override object GetItem(int index)
        {
            
            return data[index];
        }

        public override void SetPagingOptions(PagingInfo p)
        {
            
        }

        public override void Refresh()
        {

            if (weeks[WeekStart.GetTime()] == null)
            {
                this.OnDataLoading.Notify(null, null, null);

                // We need to load the data from the server
                OrganizationServiceProxy.BeginRetrieveMultiple(String.Format(Queries.SessionsByWeekStartDate, DateTimeEx.ToXrmString(WeekStart), DateTimeEx.ToXrmString(WeekEnd)), delegate(object result)
                {
                    try
                    {
                        EntityCollection results = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(dev1_session));

                        // Set data
                        List<dev1_session> sessions = new List<dev1_session>();
                        weeks[WeekStart.GetTime()] = sessions;
                        foreach (Entity e in results.Entities)
                        {
                            AddSession(sessions, (dev1_session)e);
                        }

                        RefreshActivityView();
                    }
                    catch (Exception ex)
                    {
                        // Show error
                        Script.Alert("There was an error loading the Timesheet sessions\n" + ex.Message);
                    }
                });

            }
            else
            {
                RefreshActivityView();
            }
            // Only show the selected 
          
        }
     
        public override void AddItem(object item)
        {
          
            // TODO: Set current week from the datetime on the new session
            if (this.SelectedActivity != null)
            {
                List<dev1_session> sessions = GetCurrentWeek();
                dev1_session itemAdding = (dev1_session)item;
                dev1_session newItem = new dev1_session();
              
                newItem.dev1_Description = itemAdding.dev1_Description;
                newItem.dev1_StartTime = itemAdding.dev1_StartTime;
                newItem.dev1_Duration = itemAdding.dev1_Duration;
                newItem.Account = itemAdding.Account;
                newItem.activitypointer_regardingobjectid = itemAdding.activitypointer_regardingobjectid == null ? this.SelectedActivity : itemAdding.activitypointer_regardingobjectid;
                newItem.activitypointer_subject = itemAdding.activitypointer_regardingobjectid == null ? this.SelectedActivity.Name : itemAdding.activitypointer_subject;
                newItem.dev1_ActivityId = this.SelectedActivity.Id.ToString();
                newItem.dev1_ActivityTypeName = this.SelectedActivity.LogicalName;
                
                // Set the activity reference
                switch (this.SelectedActivity.LogicalName)
                {
                    case "phonecall":
                        newItem.dev1_PhoneCallId = this.SelectedActivity;
                        break;
                    case "task":
                        newItem.dev1_TaskId = this.SelectedActivity;
                        break;
                    case "letter":
                        newItem.dev1_LetterId = this.SelectedActivity;
                        break;
                    case "email":
                        newItem.dev1_EmailId = this.SelectedActivity;
                        break;

                }
               
                newItem.EntityState = EntityStates.Created;
                // If no date set, set to the currently selected date
                if (newItem.dev1_StartTime == null)
                {
                    newItem.dev1_StartTime = SelectedDay == null ? WeekStart : SelectedDay;
                }

                // Add to the sessions view cache
                AddSession(sessions, newItem);

                // Add to the data view as well
                data.Add(newItem);

                // refresh
                DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                args.From = data.Count - 1;
                args.To = data.Count - 1;

                this.OnDataLoaded.Notify(args, null, null);
                this.OnRowsChanged.Notify(null, null, null);
            }
        }
        #endregion



        
    }
}
