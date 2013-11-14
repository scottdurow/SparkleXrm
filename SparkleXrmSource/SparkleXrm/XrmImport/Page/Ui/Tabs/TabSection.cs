
namespace Xrm
{
    public class TabSection
    {
        /// <summary>
        /// A collection of controls in the section
        /// </summary>
        public Controls Controls;

        /// <summary>
        /// Returns the label for the section
        /// </summary>
        public string GetLabel()
        {
            return null;
        }

        /// <summary>
        /// Method to return the name of the section
        /// </summary>
        public string GetName()
        {
            return null;
        }

        /// <summary>
        /// Method to return the tab containing the section
        /// </summary>
        public Xrm.UI GetParent()
        {
            return null;
        }

        /// <summary>
        /// Returns true if the section is visible, otherwise returns false
        /// </summary>
        public bool GetVisible()
        {
            return false;
        }

        /// <summary>
        /// Sets the label for the section
        /// </summary>
        public void SetLabel(string label)
        {
        }

        /// <summary>
        /// Sets a value to show or hide the section
        /// </summary>
        public void SetVisible(bool visible)
        {
        }
    }
}
