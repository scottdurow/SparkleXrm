using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xrm
{
    [Imported]
    public class ExecutionContext
    {
        /// <summary>
        /// Returns the Xrm.Page context object. See Client-side context (client-side reference) for more information
        /// </summary>
        public Page GetFormContext()
        {
            return null;
        }

        public Context GetContext()
        {
            return null;
        }

        /// <summary>
        /// Returns a value indicating the order in which this handler is executed
        /// </summary>
        public int GetDepth()
        {
            return -1;
        }

        /// <summary>
        /// Method that returns an object with methods to manage the Save event
        /// </summary>
        public SaveEventArgs GetEventArgs()
        {
            return null;
        }

        /// <summary>
        /// Method that returns a reference to the object that the event occurred on
        /// </summary>
        /// <remarks>This method returns the object from the Xrm.Page object model that is the source of the event, not an HTML DOM object. For example, in an OnChange event, this method returns the Xrm.Page.data.entity Attribute object that represents the changed attribute</remarks>
        public XrmAttribute GetEventSource()
        {
            return null;
        }

        /// <summary>
        /// Sets the value of a variable to be used by a handler after the current handler completes
        /// </summary>
        /// <param name="key">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void SetSharedVariable<T>(string key, T value)
        { 
        }

        /// <summary>
        /// Retrieves a variable set using setSharedVariable
        /// </summary>
        /// <param name="key">The name of the variable</param>
        /// <remarks>The specific type depends on what the value object is</remarks>
        public object GetSharedVariable(string key)
        { 
            return null; 
        }
       
    }
}
