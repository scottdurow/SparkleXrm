using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class Data
    {
        /// <summary>
        /// Provides methods to retrieve information specific to the record displayed on the page, the save method, and a collection of all the attributes included in the form. Attribute data is limited to attributes represented by fields on the form
        /// </summary>
        public XrmEntity Entity;

        /// <summary>
        /// CRM2013 Only: Asynchronously refreshes and optionally saves all the data of the form without reloading the page
        /// </summary>
        /// <param name="save">A Boolean value to indicate if data should be saved after it is refreshed</param>
        public AsyncCallback Refresh(bool save)
        { 
            return null; 
        }

        /// <summary>
        /// CRM2013 Only: Saves the record asynchronously with the option to set callback functions to be executed after the save operation is completed
        /// </summary>
        public AsyncCallback Save()
        { 
            return null; 
        }

        /// <summary>
        /// With Microsoft Dynamics CRM 2015 and Microsoft Dynamics CRM Online 2015 Update, the Xrm.Page.data.process namespace provides events, methods, and objects to interact with the business process flow data in a form.
        /// </summary>
        public DataProcess Process;
    }
}
