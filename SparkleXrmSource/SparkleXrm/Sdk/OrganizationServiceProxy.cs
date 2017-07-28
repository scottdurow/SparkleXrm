// OrganizationServiceProxy.cs
//


using System;
using System.Collections.Generic;
using System.Html;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk
{
    [NamedValues]
    public enum ServiceType
    {
        Soap2011 = 1,
        WebApi= 2
    }
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class OrganizationServiceProxy
    {
        public static UserSettings UserSettings = null;
        public static OrganizationSettings OrganizationSettings = null;

        private static IOrganizationService _service;
        static OrganizationServiceProxy()
        {
            _service = new SoapOrganizationServiceProxy();
        }
        public static void SetImplementation(ServiceType type)
        {
            switch (type)
            {
                case ServiceType.Soap2011:
                    _service = new SoapOrganizationServiceProxy();
                    break;
                case ServiceType.WebApi:
                    _service = new WebApiOrganizationServiceProxy();
                    break;
            }
        }

        public static void RegisterExecuteMessageResponseType(string responseTypeName, Type organizationResponseType)
        {
            _service.RegisterExecuteMessageResponseType(responseTypeName, organizationResponseType);
        }

        public static UserSettings GetUserSettings()
        {
            return _service.GetUserSettings();
        }

        /// <summary>
        /// Checks for an existing N:N relationship between two records by executing a fetch against the relationship
        /// association table.
        /// </summary>
        /// <param name="relationship">The Relationship to evaluate.</param>
        /// <param name="Entity1">EntityReference for the one of the entities to test.</param>
        /// <param name="Entity2">EntityReference for the second entity to test.</param>
        /// <returns>Boolean true if Entity1 and Entity2 have an existing relationship.</returns>
        public static bool DoesNNAssociationExist(Relationship relationship, EntityReference Entity1, EntityReference Entity2)
        {
            return _service.DoesNNAssociationExist(relationship, Entity1, Entity2);
        }


        /// <summary>
        /// Associate one or more related entites to a target entity.
        /// </summary>
        /// <param name="entityName">The Logical Name of the target entity.</param>
        /// <param name="entityId">The GUID that uniquely defines the target entity.</param>
        /// <param name="relationship">The Relationship to use for the association.</param>
        /// <param name="relatedEntities">A list of related EntityReference records to associate to the target.</param>
        public static void Associate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {
            _service.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public static void BeginAssociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            _service.BeginAssociate(entityName, entityId, relationship, relatedEntities, callBack);
        }

        public static void EndAssociate(object asyncState)
        {
            _service.EndAssociate(asyncState);
        }

        public static void Disassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {
            _service.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public static void BeginDisassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            _service.BeginDisassociate(entityName, entityId, relationship, relatedEntities, callBack);
        }

        public static void EndDisassociate(object asyncState)
        {
            _service.EndDisassociate(asyncState);
        }

        public static EntityCollection RetrieveMultiple(string fetchXml)
        {
            return _service.RetrieveMultiple(fetchXml);
        }

        public static void BeginRetrieveMultiple(string fetchXml, Action<object> callBack)
        {
            _service.BeginRetrieveMultiple(fetchXml, callBack);
        }

        public static EntityCollection EndRetrieveMultiple(object asyncState, Type entityType)
        {
            return _service.EndRetrieveMultiple(asyncState, entityType);
        }

        public static Entity Retrieve(string entityName, string entityId, string[] attributesList)
        {
            return _service.Retrieve(entityName, entityId, attributesList);
        }

        public static void BeginRetrieve(string entityName, string entityId, string[] attributesList, Action<object> callBack)
        {
            _service.BeginRetrieve(entityName, entityId, attributesList, callBack);
        }

        public static Entity EndRetrieve(object asyncState, Type entityType)
        {
            return _service.EndRetrieve(asyncState, entityType);
        }

        /// <summary>
        /// Creates a new entity record using the supplied Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Guid Create(Entity entity)
        {
            return _service.Create(entity);
        }

        public static void BeginCreate(Entity entity, Action<object> callBack)
        {
            _service.BeginCreate(entity, callBack);
        }

        public static Guid EndCreate(object asyncState)
        {
            return _service.EndCreate(asyncState);
        }

        public static void SetState(Guid id, string entityName, int stateCode, int statusCode)
        {
            _service.SetState(id, entityName, stateCode, statusCode);
        }

        public static void BeginSetState(Guid id, string entityName, int stateCode, int statusCode, Action<object> callBack)
        {
            _service.BeginSetState(id, entityName, stateCode, statusCode, callBack);
        }

        public static void EndSetState(object asyncState)
        {
            _service.EndSetState(asyncState);
        }

        public static string Delete_(string entityName, Guid id)
        {
            return _service.Delete_(entityName, id);
        }

        public static void BeginDelete(string entityName, Guid id, Action<object> callBack)
        {
            _service.BeginDelete(entityName, id, callBack);
        }

        public static void EndDelete(object asyncState)
        {
            _service.EndDelete(asyncState);
        }

        public static void Update(Entity entity)
        {
            _service.Update(entity);
        }

        public static void BeginUpdate(Entity entity, Action<object> callBack)
        {
            _service.BeginUpdate(entity, callBack);
        }

        public static void EndUpdate(object asyncState)
        {
            _service.EndUpdate(asyncState);
        }

        public static OrganizationResponse Execute(OrganizationRequest request)
        {
            return _service.Execute(request);
        }

        public static void BeginExecute(OrganizationRequest request, Action<object> callBack)
        {
            _service.BeginExecute(request, callBack);
        }

        public static OrganizationResponse EndExecute(object asyncState)
        {
            return _service.EndExecute(asyncState);
        }

    }
}
