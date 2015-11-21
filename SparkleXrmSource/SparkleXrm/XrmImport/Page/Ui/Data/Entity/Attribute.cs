using System.Runtime.CompilerServices;
using Xrm.XrmImport.Page.Ui;

namespace Xrm
{
    [Imported]
    public class XrmAttribute
    {
       
        public Controls Controls;

        /// <summary>
        /// Sets a function to be called when the attribute value is changed
        /// </summary>
        public void AddOnChange(AddRemoveOnChangeHandler function)
        {
        }

        /// <summary>
        /// Sets a function to be called when the attribute value is changed
        /// </summary>
        public void AddOnChange(ParameterlessFunctionHandler function)
        { 
        }

        /// <summary>
        /// Causes the OnChange event to occur on the attribute so that any script associated to that event can execute
        /// </summary>
        public void FireOnChange()
        {
        }

        /// <summary>
        /// Returns a string value that represents the type of attribute
        /// </summary>
        /// <returns>
        /// This method will return one of the following string values:
        ///     boolean
        ///     datetime
        ///     decimal
        ///     double
        ///     integer
        ///     lookup
        ///     memo
        ///     money
        ///     optionset
        ///     string
        /// </returns>
        public string GetAttributeType()
        {
            return null;
        }

        /// <summary>
        /// Returns a string value that represents the type of attribute
        /// </summary>
        /// <returns>
        /// This method will return one of the following string values:
        ///     boolean
        ///     datetime
        ///     decimal
        ///     double
        ///     integer
        ///     lookup
        ///     memo
        ///     money
        ///     optionset
        ///     string
        /// </returns>
        [ScriptName("getAttributeType")]
        public AttributeType GetAttributeTypeEnum()
        {
            return AttributeType.Boolean;
        }

        /// <summary>
        /// Returns a string value that represents formatting options for the attribute
        /// </summary>
        /// <returns>
        /// This method will return one of the following string values or null:
        ///     date
        ///     datetime
        ///     duration
        ///     email
        ///     language
        ///     none
        ///     phone
        ///     text
        ///     textarea
        ///     tickersymbol
        ///     timezone
        ///     url
        /// </returns>
        public string GetFormat()
        {
            return null;
        }

        /// <summary>
        /// Returns a string value that represents formatting options for the attribute
        /// </summary>
        /// <returns>
        /// This method will return one of the following string values or null:
        ///     date
        ///     datetime
        ///     duration
        ///     email
        ///     language
        ///     none
        ///     phone
        ///     text
        ///     textarea
        ///     tickersymbol
        ///     timezone
        ///     url
        /// </returns>
        [ScriptName("getFormat")]
        public AttributeFormat GetFormatEnum()
        {
            return AttributeFormat.Date;
        }

        /// <summary>
        /// Returns a value that represents the value set for an optionset or Boolean attribute when the form opened
        /// </summary>
        /// <returns>The initial value for the attribute</returns>
        /// <remarks>Optionset and Boolean attributes only</remarks>
        public object GetInitialValue()
        {
            return null;
        }

        /// <summary>
        /// Returns a Boolean value indicating if there are unsaved changes to the attribute value
        /// </summary>
        /// <returns>True if there are unsaved changes, otherwise false</returns>
        public bool GetIsDirty()
        {
            return false;
        }

        /// <summary>
        /// CRM2013 Only: Returns a Boolean value indicating whether the lookup represents a partylist lookup. Partylist lookups allow for multiple records to be set, such as the To: field for an e-mail entity record
        /// </summary>
        /// <returns>True if the lookup attribute is a partylist, otherwise false</returns>
        public bool GetIsPartyList()
        { return false; }

        /// <summary>
        /// Returns a number indicating the maximum allowed value for an attribute
        /// </summary>
        /// <returns>The maximum allowed value for the attribute</returns>
        public int GetMax()
        {
            return -1;
        }

        /// <summary>
        /// Returns a number indicating the maximum length of a string or memo attribute
        /// </summary>
        public int GetMaxLength()
        {
            return -1;
        }

        /// <summary>
        /// Returns a number indicating the minimum allowed value for an attribute
        /// </summary>
        /// <returns>The minimum allowed value for the attribute</returns>
        public int GetMin()
        {
            return -1;
        }

        /// <summary>
        /// Returns an option object with the value matching the argument passed to the method
        /// </summary>
        /// <param name="value">String Value</param>
        public Option GetOption(string value)
        {
            return null;
        }

        /// <summary>
        /// Returns an option object with the value matching the argument passed to the method
        /// </summary>
        /// <param name="value">Number value</param>
        public Option GetOption(int value)
        {
            return null;
        }

        /// <summary>
        /// Returns an array of option objects representing the valid options for an optionset attribute
        /// </summary>
        public Option[] GetOptions()
        {
            return null;
        }

        /// <summary>
        /// Returns the Xrm.Page.data.entity object that is the parent to all attributes.  This function exists to provide a consistent interface with other objects. In this case, because every attribute returns the same object, there are not many situations where it is useful
        /// </summary>
        /// <returns></returns>
        public XrmEntity GetParent()
        {
            return null;
        }

        /// <summary>
        /// Returns the number of digits allowed to the right of the decimal point
        /// </summary>
        /// <returns>The number of digits allowed to the right of the decimal point</returns>
        public int GetPrecision()
        {
            return -1;
        }

        /// <summary>
        /// Returns a string value indicating whether a value for the attribute is required or recommended
        /// </summary>
        /// <returns>
        /// Returns one of the three possible values
        ///     none
        ///     required
        ///     recommended
        /// </returns>
        public AttributeRequiredLevel GetRequiredLevel()
        { 
            return AttributeRequiredLevel.None; 
        }

        /// <summary>
        /// Returns the option object that is selected in an optionset attribute
        /// </summary>
        public Option GetSelectedOption()
        {
            return null;
        }

        /// <summary>
        /// Returns a string indicating when data from the attribute will be submitted when the record is saved
        /// </summary>
        /// <returns>
        /// Returns one of the three possible values:
        ///     always
        ///     never
        ///     dirty
        /// </returns>
        public string GetSubmitMode()
        {
            return null;
        }

        /// <summary>
        /// Returns a string indicating when data from the attribute will be submitted when the record is saved
        /// </summary>
        /// <returns>
        /// Returns one of the three possible values:
        ///     always
        ///     never
        ///     dirty
        /// </returns>
        [ScriptName("getSubmitMode")]
        public AttributeSubmitMode GetSubmitModeEnum()
        {
            return AttributeSubmitMode.Dirty;
        }

        /// <summary>
        /// Returns a string representing the logical name of the attribute
        /// </summary>
        public string GetName()
        {
            return null;
        }

        /// <summary>
        /// Returns a string value of the text for the currently selected option for an optionset attribute
        /// </summary>
        public string GetText()
        {
            return null;
        }

        /// <summary>
        /// Returns an object with three Boolean properties corresponding to privileges indicating if the user can create, read or update data values for an attribute. This function is intended for use when Field Level Security modifies a user’s privileges for a particular attribute
        /// </summary>
        public UserPrivilege GetUserPrivilege()
        {
            return null;
        }

        /// <summary>
        /// Retrieves the data value for an attribute
        /// </summary>
        /// <returns>Depends on type of attribute</returns>
        public object GetValue()
        {
            
            return null;
        }

        
        /// <summary>
        /// Retrieves the data value for an attribute
        /// </summary>
        /// <returns>Depends on type of attribute</returns>
        public T GetValue<T>()
        {
            object o = new object();
            return (T)o;
        }

        public void RemoveOnChange(AddRemoveOnChangeHandler function)
        {
            
        }

        /// <summary>
        /// Sets whether data is required or recommended for the attribute before the record can be saved
        /// </summary>
        /// <param name="requirementLevel">
        /// One of the following values:
        ///     none
        ///     required
        ///     recommended
        /// </param>
        /// <remarks>Reducing the required level of an attribute can cause an error when the page is saved. If the attribute is required by the server an error will occur if the there is no value for the attribute</remarks>
        public void SetRequiredLevel(string requirementLevel)
        {
            
        }

        /// <summary>
        /// Sets whether data is required or recommended for the attribute before the record can be saved
        /// </summary>
        /// <param name="requirementLevel">
        /// One of the following values:
        ///     none
        ///     required
        ///     recommended
        /// </param>
        /// <remarks>Reducing the required level of an attribute can cause an error when the page is saved. If the attribute is required by the server an error will occur if the there is no value for the attribute</remarks>
        public void SetRequiredLevel(AttributeRequiredLevel requirementLevel)
        {

        }

        /// <summary>
        /// Sets whether data from the attribute will be submitted when the record is saved
        /// </summary>
        /// <param name="mode">
        /// One of the following values:
        ///     always: The data is always sent with a save
        ///     never: The data is never sent with a save. When this is used the field(s) in the form for this attribute cannot be edited
        ///     dirty: Default behavior. The data is sent with the save when it has changed
        /// </param>
        public void SetSubmitMode(string mode)
        {

        }

        /// <summary>
        /// Sets whether data from the attribute will be submitted when the record is saved
        /// </summary>
        /// <param name="mode">
        /// One of the following values:
        ///     always: The data is always sent with a save
        ///     never: The data is never sent with a save. When this is used the field(s) in the form for this attribute cannot be edited
        ///     dirty: Default behavior. The data is sent with the save when it has changed
        /// </param>
        public void SetSubmitMode(AttributeSubmitMode mode)
        {

        }

        /// <summary>
        /// Sets the data value for an attribute
        /// </summary>
        /// <param name="value">Depends on the type of attribute</param>
        public void SetValue<T>(T value)
        {

        }

        /// <summary>
        /// Use this to show up to 10 matching strings in a drop-down list as users press keys to type character
        /// in a specific text field. You can also add a custom command with an icon at the bottom of the drop-down list.
        /// On selecting an item in the drop-down list, the value in the text field changes to the selected item,
        /// the drop-down list disappears, and the OnChange event for the text field is invoked.
        /// </summary>
        /// <param name="results">Object that defines the result set, which includes results and commands, to be displayed in the auto-completion drop-down list.</param>
        public void ShowAutoComplete(AutocompleteResultSet results)
        {

        }

        /// <summary>
        /// Use this function to hide the auto-completion drop-down list you configured for a specific text field.
        /// </summary>
        public void HideAutoComplete()
        {
        }
    }
}
