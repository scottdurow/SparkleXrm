// Views.cs
//


namespace Client.TimeSheet.Model
{
    public static class Queries
    {
        public static string CurrentRunningActivities = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                  "<entity name='activitypointer'>" +
                  
                    "<attribute name='activitytypecode' />" +
                    "<attribute name='subject' />" +
                    "<attribute name='activityid' />" +
                    "<attribute name='instancetypecode' />" +
                    "<order attribute='modifiedon' descending='false' />" +
                    "<filter type='and'>" +
                        "<condition attribute='ownerid' operator='eq-userid' />" +
                    "</filter>" +
                  "</entity>" +
                "</fetch>";


        /// <summary>
        /// Get each open activity for the current user
        /// where there is at least one session
        /// Also return if the session is running
        /// </summary>
        public static string CurrentOpenActivitesWithSessions = "<fetch version='1.0' output-format='xml-platform' mapping='logical' aggregate='true'>" +
              "<entity name='activitypointer'>" +
                "<attribute name='subject' groupby='true' alias='a.subject'/>" +
                "<attribute name='activityid' groupby='true' alias='a.activityid'/>" +
                "<filter type='and'>" +
                  "<condition attribute='ownerid' operator='eq-userid'  />" +
                  "<condition attribute='statecode' operator='not-in'>" +
                    "<value>1</value>" +
                    "<value>2</value>" +
                 "</condition>" +
                "</filter>" +
	            "<link-entity name='dev1_session' from='dev1_activityid' to='activityid' alias='s'>" +
                       "<attribute name='dev1_runningflag' aggregate='max' distinct='true' alias='isRunning'/>" +
                   "</link-entity>" +
              "</entity>" +
            "</fetch>";

        public static string SessionsByWeekStartDate = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                "<entity name='dev1_session'>" +
                "<attribute name='dev1_sessionid' />" +
                "<attribute name='dev1_description' />" +
                "<attribute name='dev1_activityid' />" +
                 "<attribute name='dev1_activitytypename' />" +
                  "<attribute name='dev1_starttime' />" +
                   "<attribute name='dev1_endtime' />" +
                    "<attribute name='dev1_duration' />" +
                     "<attribute name='dev1_taskid' />" +
                      "<attribute name='dev1_letterid' />" +
                      "<attribute name='dev1_emailid' />" +
                      "<attribute name='dev1_phonecallid' />" +
                "<order attribute='dev1_description' descending='false' />" +
                "<filter type='and'>" +
                    "<condition attribute='dev1_starttime' operator='on-or-after' value='{0}' />" +
                    "<condition attribute='dev1_starttime' operator='on-or-before' value='{1}' />" +
                "</filter>" +
                 //"<link-entity name='activitypointer' from='activityid' to='dev1_activityid' alias='a'>" +
                 //      "<attribute name='subject' alias='activityname'/>" +
                 //  "</link-entity>" +
                "</entity>" +
            "</fetch>";
    }
}
