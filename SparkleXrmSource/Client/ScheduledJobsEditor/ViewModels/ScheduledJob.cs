// BulkDelete.cs
//

using KnockoutApi;
using SparkleXrm;
using System;
using System.Collections.Generic;
using Xrm.Sdk;

namespace Client.ScheduledJobsEditor.ViewModels
{
    public class ScheduledJob
    {
        public Observable<Guid> ScheduledJobId = Knockout.Observable<Guid>();
        public Observable<string> Name = Knockout.Observable<string>();
        public Observable<DateTime> StartDate = Knockout.Observable<DateTime>();
        public Observable<DateTime> StartTime = Knockout.Observable<DateTime>();
        public Observable<DateTime> EndTime = Knockout.Observable<DateTime>();
        public Observable<RecurranceFrequency> Recurrance = Knockout.Observable<RecurranceFrequency>();
        public Observable<string> Data = Knockout.Observable<string>(); 
        public Observable<int> RecurEvery = Knockout.Observable<int>(0);
        public Observable<bool> Sunday = Knockout.Observable<bool>(false);
        public Observable<bool> Monday = Knockout.Observable<bool>(false);
        public Observable<bool> Tuesday = Knockout.Observable<bool>(false);
        public Observable<bool> Wednesday = Knockout.Observable<bool>(false);
        public Observable<bool> Thursday = Knockout.Observable<bool>(false);
        public Observable<bool> Friday = Knockout.Observable<bool>(false);
        public Observable<bool> Saturday = Knockout.Observable<bool>(false);
        public Observable<int?> Count = Knockout.Observable<int?>();
        public Observable<bool> NoEndDate = Knockout.Observable<bool>(true);
       
        public Observable<String> RecurrancePattern = Knockout.Observable<String>();
      
        public Observable<EntityReference> WorkflowId = Knockout.Observable<EntityReference>();

        public ScheduledJob()
        {
            AddValidation();

        }

        public void Reset()
        {
            this.ScheduledJobId.SetValue(null);
            this.Name.SetValue("");
            this.WorkflowId.SetValue(null);
            this.Monday.SetValue(false);
            this.Tuesday.SetValue(false);
            this.Wednesday.SetValue(false);
            this.Thursday.SetValue(false);
            this.Friday.SetValue(false);
            this.Saturday.SetValue(false);
            this.Sunday.SetValue(false);
            this.StartDate.SetValue(DateTime.Now);
            this.RecurEvery.SetValue(1);
            this.Recurrance.SetValue(RecurrancePatternMapper.RecurranceFrequencies[0]);

        }
        public void AddValidation()
        {
            ScheduledJob that = this;

            ValidationRules.CreateRules()
                   .AddRequiredMsg("Enter the name of the scheduled job")
                   .Register(Name);

            ValidationRules.CreateRules()
                   .AddRequiredMsg("Enter the name of the workflow to run")
                   .Register(WorkflowId);

            ValidationRules.CreateRules()
                  .AddRequiredMsg("Enter the start date of the scheduled")
                  .Register(StartDate);

            ValidationRules.CreateRules()
                  .AddRequiredMsg("Enter the recurrance frequency of the job")
                  .Register(Recurrance);

            ValidationRules.CreateRules()
                  .AddRequiredMsg("Enter the interval of the recurrance.")
                  .AddRule("Interval must be greater than zero", delegate(object value)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {

                    return (int.Parse(value.ToString()) > 0);
                }
                else
                    return true;

                
            })
            .Register(RecurEvery);
        }
    }
}
