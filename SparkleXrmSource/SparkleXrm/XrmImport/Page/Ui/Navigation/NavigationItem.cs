using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class NavigationItem
    {

        /// <summary>
        /// Returns the name of the item
        /// </summary>
        public string GetId()
        { return null; }

        /// <summary>
        /// Returns the label for the item
        /// </summary>
        public string GetLabel()
        { return null; }

        /// <summary>
        /// Sets the label for the item
        /// </summary>
        /// <param name="label">The new label for the item</param>
        public void SetLabel(string label)
        { }

        /// <summary>
        /// Returns a value that indicates whether the item is currently visible
        /// </summary>
        public bool GetVisible()
        { return false; }

        /// <summary>
        /// Sets a value that indicates whether the item is visible
        /// </summary>
        /// <param name="visible">Whether the item is visible</param>
        public void SetVisible(bool visible)
        { }

        /// <summary>
        /// Sets the focus on the item
        /// </summary>
        public void SetFocus()
        { }

    }
}
