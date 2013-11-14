
using System.Runtime.CompilerServices;
namespace Xrm
{
    public class TabItem
    {
        /// <summary>
        /// A collection of sections within the tab
        /// </summary>
        public TabSections Sections;

        /// <summary>
        /// Returns a value that indicates whether the tab is collapsed or expanded
        /// </summary>
        /// <remremarks>For Microsoft Dynamics CRM for tablets this method will always return expanded because tabs cannot be collapsed</remremarks>
        public DisplayState GetDisplayState()
        { 
            return DisplayState.Expanded; 
        }


        /// <summary>
        /// Returns the tab label
        /// </summary>
        public string GetLabel()
        {
            return null;
        }

        /// <summary>
        /// Returns the name of the tab
        /// </summary>
        public string GetName()
        {
            return null;
        }


        /// <summary>
        /// Returns the Xrm.Page.ui object
        /// </summary>
        public Xrm.UI GetParent()
        {
            return null;
        }

        /// <summary>
        /// Returns a value that indicates whether the tab is visible
        /// </summary>
        public bool GetVisible()
        {
            return false;
        }

        /// <summary>
        /// Sets the tab to be collapsed or expanded
        /// </summary>
        /// <remarks>This method does not work with Microsoft Dynamics CRM for tablets because tabs cannot be collapsed</remarks>
        public void SetDisplayState(DisplayState state)
        { 
        }

        /// <summary>
        /// Sets the focus on the tab
        /// </summary>
        public void SetFocus()
        {
        }

        /// <summary>
        /// Sets the label for the tab
        /// </summary>
        public void SetLabel(string label)
        {
        }

        /// <summary>
        /// Sets a value that indicates whether the control is visible
        /// </summary>
        public void SetVisible(bool visible)
        {
        }

    }
}
