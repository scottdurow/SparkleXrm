using System.Runtime.CompilerServices;
using Xrm.Sdk;


public partial class ActivityPointer : Entity
{

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public ActivityPointer() :
        base(EntityLogicalName)
    {
    }

    public const string EntityLogicalName = "activitypointer";

    public const int EntityTypeCode = 4200;

    [ScriptName("activityid")]
    public string activityid;


    /// <summary>
    /// The name of the custom entity.
    /// </summary>
    [ScriptName("subject")]
    public string subject ;

    [ScriptName("activitytypecode")]
    public string ActivityTypeCode;

    [ScriptName("regardingobjectid")]
    public EntityReference RegardingObjectId;

    public string DisplaySubject;

    internal void UpdateCalculatedFields()
    {
        LogicalName = ActivityTypeCode;
      
    }
}

public enum dev1_session_StatusCode
{
    Draft =1
}
public partial class dev1_session :Entity
{
    /// <summary>
    /// Default Constructor.
    /// </summary>
    public dev1_session() :
        base(EntityLogicalName)
    {
        this._metaData["dev1_duration"] = AttributeTypes.Int_;
        
    }

    public const string EntityLogicalName = "dev1_session";
    public const int EntityTypeCode = 10000;

 
    /// <summary>
    /// Date and time when the record was created.
    /// </summary>
    [ScriptName("createdon")]
    public DateTime CreatedOn;

    /// <summary>
    /// The name of the custom entity.
    /// </summary>
    [ScriptName("dev1_description")]
    public string dev1_Description;
  
    
    /// <summary>
    /// 
    /// </summary>
    [ScriptName("dev1_duration")]
    public int? dev1_Duration;


    [ScriptName("dev1_activityid")]
    public string dev1_ActivityId;

    [ScriptName("dev1_activitytypename")]
    public string dev1_ActivityTypeName; 


    /// <summary>
    /// Unique identifier for E-mail associated with Session.
    /// </summary>
    [ScriptName("dev1_emailid")]
    public EntityReference dev1_EmailId;


    [ScriptName("dev1_phonecallid")]
    public EntityReference dev1_PhoneCallId;

    /// <summary>
    /// Unique identifier for Letter associated with Session.
    /// </summary>
    [ScriptName("dev1_letterid")]
    public EntityReference dev1_LetterId;

    /// <summary>
    /// Unique identifier for Task associated with Session.
    /// </summary>
    [ScriptName("dev1_taskid")]
    public EntityReference dev1_TaskId;

    /// <summary>
    /// 
    /// </summary>
    [ScriptName("dev1_endtime")]
    public DateTime dev1_EndTime;

    [ScriptName("dev1_row")]
    public int? dev1_Row;

    [ScriptName("statuscode")]
    public OptionSetValue StatusCode;

    /// <summary>
    /// Unique identifier for entity instances
    /// </summary>
    [ScriptName("dev1_sessionid")]
    public Guid dev1_sessionId;
   
  

    /// <summary>
    /// 
    /// </summary>
    [ScriptName("dev1_starttime")]
    public DateTime dev1_StartTime = null;

    [ScriptName("contract_customerid")]
    public EntityReference contract_customerid = null;

    [ScriptName("incident_customerid")]
    public EntityReference incident_customerid = null;

    [ScriptName("opportunity_customerid")]
    public EntityReference opportunity_customerid = null;

    [ScriptName("activitypointer_regardingobjectid")]
    public EntityReference activitypointer_regardingobjectid = null;

    [ScriptName("activitypointer_subject")]
    public string activitypointer_subject;
    /// <summary>
    /// This is a virtual field that stores the account as defined when filtering activities
    /// and then when retrieved it is set from the regarding entity customer id field
    /// </summary>
    [ScriptName("account")]
    public EntityReference Account;
    


}






