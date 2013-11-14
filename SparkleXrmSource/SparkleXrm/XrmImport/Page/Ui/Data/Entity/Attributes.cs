using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class Attributes
    {

        /// <summary>
        /// Applies the action contained in a delegate function
        /// </summary>
        /// <param name="function">Delegate function with parameters for attribute and index</param>
        public void ForEach(GetAttributeHandler function)
        { }

        /// <summary>
        /// Get all the objects in the collection 
        /// </summary>
        public XrmAttribute[] Get()
        { return null; }

        /// <summary>
        /// Get one object from the collection 
        /// </summary>
        /// <param name="name">The object where the name matches the argument</param>
        public XrmAttribute Get(string name)
        { return null; }

        /// <summary>
        /// Get one object from the collection
        /// </summary>
        /// <param name="position">The object where the index matches the number</param>
        public XrmAttribute Get(int position)
        { return null; }

        /// <summary>
        /// Get any objects that cause the delegate function to return true
        /// </summary>
        public XrmAttribute[] Get(GetAttributeHandler function)
        { return null; }

        /// <summary>
        /// Get the number of items in the collection
        /// </summary>
        public int GetLength()
        { return -1; }

    }
}
