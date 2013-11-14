using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class NavigationItems
    {

        /// <summary>
        /// Applies the action contained in a delegate function
        /// </summary>
        /// <param name="function">Delegate function with parameters for attribute and index</param>
        public void ForEach(GetNavigationItemHandler function)
        { }

        /// <summary>
        /// Get all the objects in the collection 
        /// </summary>
        public NavigationItem[] Get()
        { return null; }

        /// <summary>
        /// Get one object from the collection 
        /// </summary>
        /// <param name="name">The object where the name matches the argument</param>
        public NavigationItem Get(string name)
        { return null; }

        /// <summary>
        /// Get one object from the collection
        /// </summary>
        /// <param name="position">The object where the index matches the number</param>
        public NavigationItem Get(int position)
        { return null; }

        /// <summary>
        /// Get any objects that cause the delegate function to return true
        /// </summary>
        public NavigationItem[] Get(GetNavigationItemHandler function)
        { return null; }

        /// <summary>
        /// Get the number of items in the collection
        /// </summary>
        public int GetLength()
        { return -1; }

    }
}
