using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class Control
    {
        
        public bool? OriginalIsDisabled;

       
        public void AddOnChange(AddRemoveOnChangeHandler function)
        {
        }

        /// <summary>
        /// Adds a new view for the lookup dialog box
        /// </summary>
        /// <param name="viewId">The string representation of a GUID for a view.  Note: This value is never saved and only needs to be unique among the other available views for the lookup. A string for a non-valid GUID will work, for example “{00000000-0000-0000-0000-000000000001}”. It is recommended that you use a tool like guidgen.exe to generate a valid GUID</param>
        /// <param name="entityName">The name of the entity</param>
        /// <param name="viewDisplayName">The name of the view</param>
        /// <param name="fetchXml">The fetchXml query for the view</param>
        /// <param name="layoutXml">The XML that defines the layout of the view</param>
        /// <param name="isDefault">Whether the view should be the default view</param>
        /// <remarks>This method does not work with Owner lookups. Owner lookups are used to assign user-owned records</remarks>
        public void AddCustomView(string viewId, string entityName, string viewDisplayName, string fetchXml, string layoutXml, bool isDefault)
        {
        }

        /// <summary>
        /// CRM2013 Only: Use add additional filters to the results displayed in the lookup. Each filter will be combined with any previously added filters as an ‘AND’ condition
        /// </summary>
        /// <param name="filter">The fetchXml filter element to apply.  Refer to SDK For example.</param>
        /// <param name="entityLogicaName">If this is set the filter will only apply to that entity type. Otherwise it will apply to all types of entities returned</param>
        public void AddCustomFilter(string filter, string entityLogicaName)
        { 
        }

        /// <summary>
        /// CRM2013 Only: Use add additional filters to the results displayed in the lookup. Each filter will be combined with any previously added filters as an ‘AND’ condition
        /// </summary>
        /// <param name="filter">The fetchXml filter element to apply.  Refer to SDK For example.</param>
        public void AddCustomFilter(string filter)
        {
        }

        /// <summary>
        /// CRM2013 Only: Use this method to apply changes to lookups based on values current just as the user is about to view results for the lookup
        /// </summary>
        /// <param name="AddPreSearchHandler">Function to add.  The argument is a function that will be run just before the search to provide results for a lookup occurs. You can use this handler to call one of the other lookup control functions and improve the results to be displayed in the lookup.</param>
        public void AddPreSearch(ParameterlessFunctionHandler AddPreSearchHandler)
        {
        }

        /// <summary>
        /// Adds an option to an option set control
        /// </summary>
        /// <param name="option">An option object to add to the OptionSet</param>
        /// <param name="index">The index position to place the new option. If not provided the option will be added to the end</param>
        public void AddOption(Option option, int index)
        {
        }
        
        /// <summary>
        /// Adds an option to an option set control
        /// </summary>
        /// <param name="option">An option object to add to the OptionSet</param>
        public void AddOption(Option option)
        {
        }

        /// <summary>
        /// Clears all options from an Option Set control
        /// </summary>
        public void ClearOptions()
        {
        }

        /// <summary>
        /// Returns the attribute that the control is bound to
        /// </summary>
        /// <remarks>Controls that are not bound to an attribute (subgrid, web resource, and IFRAME) do not have this method. An error will be thrown if you attempt to use this method on one of these controls.</remarks>
        public XrmAttribute GetAttribute()
        {
            return null;
        }

        /// <summary>
        /// Returns a value that categorizes controls
        /// </summary>
        /// <returns>
        /// Possible return values of getControlType
        ///     standard: A Standard control
        ///     iframe: An IFRAME control
        ///     lookup: A Lookup control
        ///     optionset: An OptionSet control
        ///     subgrid: A subgrid control
        ///     webresource: A web resource control
        ///     notes: A Notes control
        /// </returns>
        public string GetControlType()
        {
            return null;
        }

        /// <summary>
        /// Returns the value of the data query string parameter passed to a Silverlight web resource
        /// </summary>
        /// <returns>The data value passed to the Silverlight web resource</returns>
        public string GetData()
        {
            return null;
        }

        /// <summary>
        /// Returns the Id value of the default lookup dialog view
        /// </summary>
        public string GetDefaultView()
        {
            return null;
        }

        /// <summary>
        /// Returns whether the control is disabled
        /// </summary>
        /// <returns></returns>
        public bool GetDisabled()
        {
            return false;
        }

        /// <summary>
        /// Returns the label for the control
        /// </summary>
        public string GetLabel()
        {
            return null;
        }

        /// <summary>
        /// Returns the name assigned to the control
        /// </summary>
        public string GetName()
        {
            return null;
        }

        /// <summary>
        /// Returns a reference to the section object that contains the control
        /// </summary>
        public Control GetParent()
        {
            return null;
        }

        /// <summary>
        /// Returns the current URL being displayed in an IFRAME or web resource
        /// </summary>
        public string GetSrc()
        {
            return null;
        }

        /// <summary>
        /// Returns the default URL that an IFRAME control is configured to display. This method is not available for web resources
        /// </summary>
        public string GetInitialUrl()
        {
            return null;
        }

        /// <summary>
        /// Returns the object in the form that represents an I-frame or web resource
        /// </summary>
        /// <returns>
        /// Which object depends on the type of control:
        ///     An IFRAME will return the IFrame element from the Document Object Model (DOM)
        ///     A Silverlight web resource will return the Object element from the DOM that represents the embedded Silverlight plug-in
        /// </returns>
        public object GetObject()
        {
            return null;
        }

        /// <summary>
        /// Returns a value that indicates whether the control is currently visible
        /// </summary>
        /// <returns>True if the control is visible, otherwise false</returns>
        /// <remarks>Note: If the containing section or tab for this control is not visible, this method can still return true. To make certain that the control is actually visible; you need to also check the visibility of the containing elements.</remarks>
        public bool GetVisible()
        {
            return false;
        }

        /// <summary>
        /// Refreshes the data displayed in a Sub-Grid
        /// </summary>
        public void Refresh()
        {
            return;
        }

        /// <summary>
        /// Removes an option from an Option Set control
        /// </summary>
        /// <param name="value">The value of the option you want to remove</param>
        public void RemoveOption(int value)
        {
            
        }

        /// <summary>
        /// Sets the value of the data query string parameter passed to a Silverlight web resource
        /// </summary>
        /// <param name="data">The data value to pass to the Silverlight web resource</param>
        public void SetData(string data)
        {
        }

        /// <summary>
        /// Sets the default view for the lookup control dialog
        /// </summary>
        /// <param name="viewId">The Id of the view to be set as the default view</param>
        public void SetDefaultView(string viewId)
        {
        }

        /// <summary>
        /// Sets whether the control is disabled
        /// </summary>
        /// <param name="disable">True if the control should be disabled, otherwise false</param>
        public void SetDisabled(bool disable)
        {
        }

        /// <summary>
        /// Sets the focus on the control
        /// </summary>
        public void SetFocus()
        {
        }

        /// <summary>
        /// CRM2013 Only: Specify whether a date control should show the time portion of the date
        /// </summary>
        public void SetShowTime(bool showTime)
        { 
        }

        /// <summary>
        /// Sets the label for the control
        /// </summary>
        /// <param name="label">The new label for the control</param>
        public void SetLabel(string label)
        {
        }

        /// <summary>
        /// CRM2013 Only: Display a message near the control to indicate that data is not valid. When this method is used on Microsoft Dynamics CRM for tablets a red "X" icon appears next to the control. Tapping on the icon will display the message
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <remarks>Setting a notification on a control will block the form from saving</remarks>
        public void SetNotification(string message)
        { 
        }

        /// <summary>
        /// CRM2013 Only: Remove a message already displayed for a control
        /// </summary>
        /// <returns>Indicates whether the method succeeded</returns>
        public bool ClearNotification()
        { 
            return false; 
        }

        /// <summary>
        /// Sets the URL to be displayed in an IFRAME or web resource
        /// </summary>
        /// <param name="src">The URL</param>
        public void SetSrc(string src)
        {
        }

        /// <summary>
        /// Sets a value that indicates whether the control is visible
        /// </summary>
        /// <param name="visible">True if the control should be visible, otherwise false</param>
        public void SetVisible(bool visible)
        {
        }
    }
}
