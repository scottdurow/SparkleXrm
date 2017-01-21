// CachedOrganizationService.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Xrm.Services
{
    [ScriptNamespace("SparkleXrm.Services")]
    public class CachedOrganizationService
    {
        public static OrganizationServiceCache Cache = new OrganizationServiceCache();
        public static Entity Retrieve(string entityName, string entityId, string[] attributesList)
        {
            // Does the entity exist in the cahce?
            Entity result = (Entity)Cache.Get(entityName, entityId);
            if (result == null)
            {
                result = OrganizationServiceProxy.Retrieve(entityName, entityId, attributesList);
                Cache.Insert(entityName, entityId, (object)result);
                return result;
            }
            else
            {
                return result;
            }
        }
        public static EntityCollection RetrieveMultiple(string fetchXml)
        {
            // Does the entity exist in the cahce?
            EntityCollection result = (EntityCollection)Cache.Get("query", fetchXml);
            if (result == null)
            {
                result = OrganizationServiceProxy.RetrieveMultiple(fetchXml);
                Cache.Insert("query", fetchXml, (object)result);
                return result;
            }
            else
            {
                return result;
            }
        }
    }
}
