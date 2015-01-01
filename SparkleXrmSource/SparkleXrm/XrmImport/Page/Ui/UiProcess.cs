using System.Runtime.CompilerServices;

namespace Xrm
{
   
    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum ProcessExpanded
    {

        [ScriptName("expanded")]
        Expanded,

        [ScriptName("collapsed")]
        Collapsed


    }

    /// <summary>
    /// With Microsoft Dynamics CRM 2015 and Microsoft Dynamics CRM Online 2015 Update, the Xrm.Page.ui.process namespace provides methods to interact with the business process flow control in a form.
    /// </summary>
    [Imported]
    public abstract class UiProcess
    {
        /// <summary>
        /// Use this method to expand or collapse the business process control.
        /// </summary>
        /// <param name="strExpanded"></param>
        public abstract void SetDisplayState(ProcessExpanded strExpanded);
       
        /// <summary>
        /// Use this method to show or hide the business process control.
        /// </summary>
        /// <param name="boolVisible"></param>
        public abstract void SetVisible(bool boolVisible);
    }
}
