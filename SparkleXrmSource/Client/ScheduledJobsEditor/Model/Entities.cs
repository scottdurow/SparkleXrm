// Entities.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Client.ScheduledJobsEditor.Model
{
    public class BulkDeleteOperation : Entity
    {
        public BulkDeleteOperation()
            : base("bulkdeleteoperation")
        {
        }
        public static string EntityLogicalName = "bulkdeleteoperation";

        [ScriptName("name")]
        public string Name;
        
        [ScriptName("bulkdeleteoperationid")]
        public Guid BulkDeleteOperationId;

        [ScriptName("asyncoperationid")]
        public EntityReference AsyncOperationId;

        // TIP: Shows how to use link entity attributes by aliasing
        [ScriptName("asyncoperation_statecode")]
        public OptionSetValue AsyncOperation_StateCode;

        [ScriptName("asyncoperation_statuscode")]
        public OptionSetValue AsyncOperation_StatusCode;

        [ScriptName("asyncoperation_postponeuntil")]
        public DateTime AsyncOperation_PostponeUntil;
        

            
    }
    public class dev1_ScheduledJob : Entity
    {
        public dev1_ScheduledJob()
            : base("dev1_scheduledjob")
        {
        }
        public static string EntityLogicalName = "dev1_scheduledjob";
        [ScriptName("dev1_scheduledjobid")]
        public Guid dev1_ScheduledJobId;

        [ScriptName("dev1_name")]
        public string dev1_Name;

        [ScriptName("dev1_recurrancepattern")]
        public string dev1_RecurrancePattern;

        [ScriptName("createdon")]
        public DateTime CreatedOn;

        [ScriptName("dev1_workflowname")]
        public string dev1_WorkflowName;

        [ScriptName("dev1_startdate")]
        public DateTime dev1_StartDate;

    }
   
    public class asyncoperation : Entity
            
    {
        public static string EntityLogicalName = "asyncoperation";
        public asyncoperation()
            : base("asyncoperation")
        {
        }

        [ScriptName("asyncoperationid")]
        public Guid AsyncOperationId;

        [ScriptName("name")]
        public string Name;

        [ScriptName("recurrencepattern")]
        public string RecurrancePattern;
       
        [ScriptName("postponeuntil")]
        public DateTime PostponeUntil;
        
       
        [ScriptName("recurrencestarttime")]
        public DateTime RecurrenceStartTime;

       
        [ScriptName("bulkdeleteoperationid")]
        public Guid BulkDeleteOperationId;


        [ScriptName("data")]
        public string Data;

        [ScriptName("statecode")]
        public OptionSetValue StateCode;

        [ScriptName("statuscode")]
        public OptionSetValue StatusCode;
        
    }
}
