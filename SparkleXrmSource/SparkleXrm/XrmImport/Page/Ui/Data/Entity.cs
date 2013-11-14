using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class XrmEntity
    {
        /// <summary>
        /// The collection of attributes for the entity
        /// </summary>
        public Attributes Attributes;

        
        /// <summary>
        /// Adds a function to be called when the record is saved
        /// </summary>
        public void AddOnSave(AddRemoveOnSaveHandler function)
        {
        }
        
        /// <summary>
        /// Adds a function to be called when the record is saved
        /// </summary>
        public void AddOnSave(ParameterlessFunctionHandler function)
        {
        }

        /// <summary>
        /// Adds a function to be called when the record is saved
        /// </summary>
        public void RemoveOnSave(AddRemoveOnSaveHandler function)
        {
        }

        /// <summary>
        /// Removes a function to be called when the record is saved
        /// </summary>
        public void RemoveOnSave(ParameterlessFunctionHandler function)
        {
        }

        /// <summary>
        /// Returns a string representing the xml that will be sent to the server when the record is saved
        /// </summary>
        public string GetDataXml()
        {
            return null;
        }

        /// <summary>
        /// Returns a string representing the logical name of the entity for the record
        /// </summary>
        /// <returns></returns>
        public string GetEntityName()
        {
            return null;
        }

        /// <summary>
        /// Returns a string representing the GUID id value for the record
        /// </summary>
        public string GetId()
        {
            return null;
        }

        /// <summary>
        /// Returns a Boolean value that indicates if any fields in the form have been modified
        /// </summary>
        public bool GetIsDirty()
        {
            return false;
        }

        /// <summary>
        /// Saves the record
        /// </summary>
        public void Save()
        {
        }

        /// <summary>
        /// Saves the record with the options to close the form or open a new form after the save is completed
        /// </summary>
        public void Save(string option)
        {
        }

        /// <summary>
        /// Saves the record with the options to close the form or open a new form after the save is completed
        /// </summary>
        public void Save(SaveOption SaveOption)
        { 
        }
    }
}
