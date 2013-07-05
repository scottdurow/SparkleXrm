using Client.TimeSheet.Model;
using System;
using System.Collections;
using System.Html;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Ribbon;
using Xrm.XrmImport.Ribbon;


namespace Client.TimeSheet.RibbonCommands
{
    public static class Global
    {
       
       
        public static void NewAccount()
        {
            Utility.OpenEntityForm("account", null, null);
           
        }
        public static void AccountOnSave()
        {
            if (Page.Ui.GetFormType() == FormTypes.Create)
            {
                object parentOpener = (Object)Window.Top.Opener;
                if ((string)Script.Literal("typeof({0}.Client)",parentOpener) != "undefined")
                {
                    Script.Literal("{0}.Client.TimeSheet.RibbonCommands.Global.newAccountCallBack()", parentOpener);
                }
            }

        }
        public static void NewAccountCallBack()
        {
            Script.Alert("Refresh!");
          
        }
        public static void GetRunningActivities(CommandProperties commandProperties)
        {
            // Get a list of all running activities
            // TODO: This list could be filtered to only 'billable' activities
            EntityCollection runningActivities = OrganizationServiceProxy.RetrieveMultiple(Queries.CurrentOpenActivitesWithSessions);
            RibbonMenuSection section = new RibbonMenuSection("dev1.Activities.Section", "Activities", 1, RibbonDisplayMode.Menu16);
            int i=0;

            foreach (Dictionary activity in runningActivities.Entities)
            {
                string image = "WebResources/dev1_/images/start.gif";

                bool isRunning = activity["isRunning"] != null && (activity["isRunning"].ToString() == "1");
                if (isRunning)
                    image = "WebResources/dev1_/images/stop.gif";


                section.AddButton(
                    new RibbonButton("dev1.Activity." + activity["a.activityid"].ToString(),
                        i,
                        activity["a.subject"].ToString(),
                        "dev1.ApplicationRibbon.StartStopActivity.Command",
                        image,
                        image));
                    i++;
            }

            RibbonMenu activities = new RibbonMenu("dev1.Activities").AddSection(section);

        

            commandProperties.PopulationXML = activities.SerialiseToRibbonXml();

        }
        public static void StartStopActivity(CommandProperties commandProperties)
        {
            Script.Alert(commandProperties.SourceControlId);
            string values = "activityid=" + commandProperties.SourceControlId.Replace("dev1.Activity.","");
            string parameters = GlobalFunctions.encodeURIComponent(values);
            Utility.OpenWebResource("dev1_/scripts/StartStopSession.htm", parameters, 400, 300);
        }
    }
}
