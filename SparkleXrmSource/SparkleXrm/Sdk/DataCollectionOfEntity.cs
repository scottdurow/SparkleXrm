

using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class DataCollectionOfEntity : IEnumerable
    {
        public DataCollectionOfEntity(List<Entity> entities)
        {
            _internalArray = entities;
        }
        public List<Entity> _internalArray;
        public List<Entity> Items()
        {
            return _internalArray;
        }
        
        public Entity this[int i]
        {
            get { return _internalArray[i]; }
            set { _internalArray[i] = value; }
            
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            // We can't use the GetEnumerator on the Array because we can't extend the array object in CRM
            return ArrayEx.GetEnumerator(_internalArray);
            
        }
        public int Count
        {
            get { return _internalArray.Count; }
        }
    }
}
