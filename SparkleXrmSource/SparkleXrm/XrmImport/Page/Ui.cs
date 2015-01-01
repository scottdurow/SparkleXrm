using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("10*")]
    public enum FormTypes
    {
        [ScriptName("0")]
         Undefined =0,
        [ScriptName("1")]
         Create = 1,
        [ScriptName("2")]
         Update = 2,
        [ScriptName("3")]
         ReadOnly = 3,
        [ScriptName("4")]
         Disabled = 4,
        [ScriptName("5")]
         QuickCreate=5,
        [ScriptName("6")]
         BulkEdit = 6
    }

    [Imported]
    public class UI
    {
        public Controls Controls;
        public Navigation Navigation;
        public FormSelector FormSelector;
        public Tabs Tabs;

        /// <summary>
        /// Method to close the form
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Method to get the control object that currently has focus on the form. Web Resource and IFRAME controls are not returned by this method
        /// </summary>
        public Control GetCurrentControl()
        {
            return null;
        }

        /// <summary>
        /// Method to get the form context for the record
        /// </summary>
        public FormTypes GetFormType()
        {
            return FormTypes.Undefined;
        }

        /// <summary>
        /// Method to get the width of the viewport in pixels
        /// </summary>
        public int GetViewPortHeight()
        {
            return -1;
        }

        /// <summary>
        /// Method to get the height of the viewport in pixels
        /// </summary>
        public int GetViewPortWidth()
        {
            return -1;
        }

        /// <summary>
        /// Method to cause the ribbon to re-evaluate data that controls what is displayed in it
        /// </summary>
        /// <remarks>This function is typically used when a ribbon EnableRule (RibbonDiffXml) depends on a value in the form. After your code changes a value that is used by a rule, use this method to force the ribbon to re-evaluate the data in the form so that the rule can be applied.  Note: This method does not work with Microsoft Dynamics CRM for tablets</remarks>
        public void RefreshRibbon()
        {
        }

        /// <summary>
        /// CRM2013 Only: Use this method to display form level notifications
        /// </summary>
        /// <param name="message">The text of the message</param>
        /// <param name="level">The level of the message</param>
        /// <param name="uniqueId">A unique identifier for the message used with clearFormNotification to remove the notification</param>
        /// <returns>True if the method succeeded, otherwise false</returns>
        /// <remarks>You can display any number of notifications and they will be displayed until they are removed using clearFormNotification. The height of the notification area is limited so each new message will be added to the top. Users can scroll down to view older messages that have not yet been removed.</remarks>
        public bool SetFormNotification(string message, FormNotificationLevel level, string uniqueId)
        { 
            return false; 
        }

        /// <summary>
        /// CRM2013 Only: Use this method to remove form level notifications
        /// </summary>
        /// <param name="uniqueId">A unique identifier for the message used with SetFormNotification to set the notification</param>
        /// <returns>True if the method succeeded, otherwise false</returns>
        public bool ClearFormNotification(string uniqueId)
        { 
            return false;
        }

        /// <summary>
        /// With Microsoft Dynamics CRM 2015 and Microsoft Dynamics CRM Online 2015 Update, the Xrm.Page.ui.process namespace provides methods to interact with the business process flow control in a form.
        /// </summary>
        public UiProcess Process;


    }
}
