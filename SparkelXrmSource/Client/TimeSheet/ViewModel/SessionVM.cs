// Session.cs
//

using System;
using System.Collections.Generic;
using KnockoutApi;
using Xrm.Sdk;
using System.Collections;
using SparkleXrm;
using System.Runtime.CompilerServices;

namespace TimeSheet.Client.ViewModel
{
    public class SessionVM
    {
        private bool _supressEvents = false;
      
        public Observable<Guid> dev1_sessionid = Knockout.Observable<Guid>();
        public Observable<EntityReference> Activity = Knockout.Observable<EntityReference>();

       
        public Observable<DateTime> dev1_starttime = Knockout.Observable<DateTime>();
        public Observable<DateTime> dev1_endtime = Knockout.Observable<DateTime>();
        public Observable<string> dev1_description = Knockout.Observable<string>();
        public Observable<int?> dev1_duration = Knockout.Observable<int?>();
        public Observable<string> StartTimeTime = Knockout.Observable<string>();
        public Observable<string> EndTimeTime = Knockout.Observable<string>();
        public Observable<string> TimeSoFar = Knockout.Observable<string>();

        public Observable<string> test = Knockout.Observable<string>("test");

        public SessionVM(StartStopSessionViewModel vm, dev1_session data)
        {
            if (data != null)
            {
                

              
                this.dev1_sessionid.SetValue(data.dev1_sessionId);
                this.dev1_description.SetValue(data.dev1_Description);
                this.dev1_duration.SetValue(data.dev1_Duration);
                this.dev1_endtime.SetValue(data.dev1_EndTime);
                this.dev1_starttime.SetValue(data.dev1_StartTime);
                // Calculate the time elements
                GetTime(dev1_starttime, StartTimeTime);
                GetTime(dev1_endtime, EndTimeTime);

               

                // Set activity
                if (data.dev1_LetterId != null)
                {
                   

                    this.Activity.SetValue(data.dev1_LetterId);
                }
                else if (data.dev1_TaskId != null)
                {
                    this.Activity.SetValue(data.dev1_TaskId);
                    
                }
                 // TODO: Add more activity types
            }

            // Add events to calculate duration
            dev1_endtime.Subscribe(delegate(DateTime value) { OnStartEndDateChanged(); });

            dev1_starttime.Subscribe(delegate(DateTime value) { OnStartEndDateChanged(); });

            StartTimeTime.Subscribe(delegate(string value) { OnStartEndDateChanged(); });
            EndTimeTime.Subscribe(delegate(string value) { OnStartEndDateChanged(); });
            dev1_duration.Subscribe(delegate(int? value) { OnDurationChanged(); });
            AddValidation();
        }
        public void OnStartEndDateChanged()
        {
            if (_supressEvents) return;
            _supressEvents = true;

            DateTime startTime = dev1_starttime.GetValue();
            DateTime endTime = dev1_endtime.GetValue();
            int? duration = null;
            startTime = DateTimeEx.AddTimeToDate(startTime, StartTimeTime.GetValue());
            endTime = DateTimeEx.AddTimeToDate(endTime, EndTimeTime.GetValue());

            if (endTime != null && startTime != null)
            {
                endTime.SetDate(startTime.GetDate());
                endTime.SetMonth(startTime.GetMonth());
                endTime.SetFullYear(startTime.GetFullYear());
            }
            duration= CalculateDuration(startTime, endTime);
            this.dev1_duration.SetValue(duration);
            _supressEvents = false;
        }

        public static int? CalculateDuration( DateTime startTime,  DateTime endTime)
        {
            int? duration = null;
            if ((startTime != null) && (endTime != null))
            {
                
                if ((startTime != null) && (endTime != null))
                {
                    duration = (endTime.GetTime() - startTime.GetTime()) / (1000 * 60);
                }
            }
            return duration;
        }
        public void OnDurationChanged()
        {
             if (_supressEvents) return;
            _supressEvents = true;


            DateTime startTime = dev1_starttime.GetValue();
            string startTimeTime = StartTimeTime.GetValue();
            if ((startTime != null) && (startTimeTime!=null))
            {
                startTime = DateTimeEx.AddTimeToDate(startTime, startTimeTime);
                int? duration = dev1_duration.GetValue();
                int? startTimeMilliSeconds = startTime.GetTime();
                startTimeMilliSeconds = startTimeMilliSeconds + (duration * 1000 * 60);

                DateTime newEndDate = new DateTime((int)startTimeMilliSeconds);
                dev1_endtime.SetValue(newEndDate);
                GetTime(dev1_endtime, EndTimeTime);
            }
            _supressEvents = false;
        }
        public dev1_session GetUpdatedModel()
        {
         
            dev1_session session = new dev1_session();

            SetTime(dev1_starttime, StartTimeTime);
            SetTime(dev1_endtime, EndTimeTime);

            KnockoutMappingSpecification mapping = new KnockoutMappingSpecification();
            mapping.Ignore = new string[] { "dev1_letterid", "dev1_taskid", "dev1_emailid" };

            if ( this.dev1_sessionid.GetValue()!=null)
                session.dev1_sessionId = this.dev1_sessionid.GetValue();
            session.dev1_Description = this.dev1_description.GetValue();
            session.dev1_StartTime = this.dev1_starttime.GetValue();
            session.dev1_EndTime = this.dev1_endtime.GetValue();
            session.dev1_Duration = this.dev1_duration.GetValue();
           
            return session;
        }

        private void GetTime(Observable<DateTime> dateProperty,Observable<string> time)
        {
            DateTime dateValue = dateProperty.GetValue();
            if (dateValue != null)
            {
                string timeFormatted = dateValue.Format("h:mm tt");
                time.SetValue(timeFormatted);
            }
        }

        private void SetTime(Observable<DateTime> dateProperty, Observable<string> time)
        {

            DateTime currentDate = dateProperty.GetValue();
            currentDate = DateTimeEx.AddTimeToDate(currentDate, time.GetValue());
     
            dateProperty.SetValue(currentDate);
        }
       
            
        #region Methods
        public void AddValidation()
        {
            SessionVM self = this;

            ValidationRules.CreateRules()
                   .AddRequiredMsg("Enter the activity you wish to stop")
                   .Register(Activity);
               
            ValidationRules.CreateRules()
                    .AddRequiredMsg("Enter the start date of the session")
                    .AddRule("Start date must be before the end date",delegate(object value) {

                        bool isValid = true;
                        DateTime endTime = self.dev1_endtime.GetValue();
                        if ((endTime != null) && (value!=null))
                            isValid = endTime > (DateTime)value;

                        return isValid;
                    })
                    .Register(dev1_starttime);
                  
            ValidationRules.CreateRules()
                    .AddRequiredMsg("Enter the end time of the session")
                    .Register(dev1_endtime);
                   
            ValidationRules.CreateRules()
                    .AddRequiredMsg("Enter the duration of the session")
                    .Register(dev1_duration);
                    

               
        }
        #endregion
    }
}
