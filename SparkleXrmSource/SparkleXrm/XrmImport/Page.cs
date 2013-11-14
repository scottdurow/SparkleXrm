using System.Runtime.CompilerServices;
using System;

namespace Xrm
{
    [Imported]
    public class Page
    {
        /// <summary>
        /// Contains methods to retrieve information about the user interface, in addition to collections for several sub components of the form
        /// </summary>
        public static UI Ui;

        /// <summary>
        /// Provides methods to retrieve information specific to an organization, a user, or parameters that were passed to the form in a query string
        /// </summary>
        public static Context Context;

        /// <summary>
        /// Provides access to the entity data and methods to manage the data in the form
        /// </summary>
        public static Data Data;

      

        [ScriptAlias("GetGlobalContext")]
        public static Context GetGlobalContext()
        {
            return null;
        }

        #region static getControl methods
        /// <summary>
        /// Returns a control where the logical name matches the string
        /// </summary>
        public static Control GetControl(string name)
        {
            return null;
        }

        /// <summary>
        /// Returns the control where the Xrm.Page.ui.controls collection index matches the number
        /// </summary>
        public static Control GetControl(int position)
        {
            return null;
        }

        /// <summary>
        /// Returns an array of all the controls
        /// </summary>
        public static Control[] GetControl()
        {
            return null;
        }

        /// <summary>
        /// Returns an array of any controls from the Xrm.Page.ui.controls collection that cause the delegate function to return true
        /// </summary>
        public static Control[] GetControl(GetControlHandler function)
        {
            return null;
        }
        #endregion
    

        #region static getAttribute methods
        /// <summary>
        /// Returns an attribute object where the attribute name matches the string
        /// </summary
        public static XrmAttribute GetAttribute(string name)
        {
            return null;
        }

        /// <summary>
        /// Returns the attribute object where the Xrm.Page.data.entity.attributes collection index matches the number
        /// </summary>
        public static XrmAttribute GetAttribute(int position)
        {
            return null;
        }

        /// <summary>
        /// Returns an array of all the attributes
        /// </summary>
        public static XrmAttribute[] GetAttribute()
        {
            return null;
        }

        /// <summary>
        /// Returns an array of any attributes from the Xrm.Page.data.entity.attributes collection that cause the delegate function to return true
        /// </summary>
        public static XrmAttribute[] GetAttribute(GetAttributeHandler function)
        {
            return null;
        }
        #endregion
    
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("window.parent.Xrm.Page")]
    public class ParentPage
    {
        public static UI Ui;
        public static Context Context;
        public static Data Data;



        [ScriptAlias("GetGlobalContext")]
        public static Context GetGlobalContext()
        {
            return null;
        }

        #region static getControl methods
        public static Control GetControl(string name)
        {
            return null;
        }

        public static Control GetControl(int position)
        {
            return null;
        }

        public static Control[] GetControl(GetControlHandler function)
        {
            return null;
        }
        #endregion


        #region static getAttribute methods
        public static XrmAttribute GetAttribute(string name)
        {
            return null;
        }

        public static XrmAttribute GetAttribute(int position)
        {
            return null;
        }

        public static XrmAttribute[] GetAttribute(GetAttributeHandler function)
        {
            return null;
        }
        #endregion

    }


}
