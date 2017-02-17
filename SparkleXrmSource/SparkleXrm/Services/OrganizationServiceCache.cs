// OrganizationServiceCache.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Xrm.Services
{
    [ScriptNamespace("SparkleXrm.Services")]
    public class OrganizationServiceCache
    {
        private Dictionary<string, object> _innerCache = new Dictionary<string, object>();

        public void Remove(string entityName, Guid id)
        {

        }
        public void Insert(string key, string query, object results)
        {
            _innerCache[key + "_" + query] = results;

        }
        public object Get(string key, string query)
        {
            return _innerCache[key + "_" + query];
        }

       
    }
}
