using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xrm
{
    [Imported]
    public class SaveEventArgs
    {

    #region methods

        /// <summary>
        /// Returns a value indicating how the save event was initiated by the user
        /// </summary>
        public SaveMode GetSaveMode()
        { return SaveMode.Save; }

        /// <summary>
        /// Returns a value indicating whether the save event has been canceled because the preventDefault method was used in this event hander or a previous event handler
        /// </summary>
        public bool IsDefaultPrevented()
        { return false;  }

        /// <summary>
        /// Cancels the save operation, but all remaining handlers for the event will still be executed
        /// </summary>
        public void PreventDefault()
        { }

    #endregion

    }
}
