using System.Runtime.CompilerServices;
using System;

namespace Xrm
{
    [Imported]
    public class Page
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
