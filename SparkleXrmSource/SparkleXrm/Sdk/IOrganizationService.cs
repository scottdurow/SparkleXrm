// IOrganizationService.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    [Imported]
    public interface IOrganizationService
    {

        void RegisterExecuteMessageResponseType(string responseTypeName, Type organizationResponseType);

        UserSettings GetUserSettings();

        /// <summary>
        /// Checks for an existing N:N relationship between two records by executing a fetch against the relationship
        /// association table.
        /// </summary>
        /// <param name="relationship">The Relationship to evaluate.</param>
        /// <param name="Entity1">EntityReference for the one of the entities to test.</param>
        /// <param name="Entity2">EntityReference for the second entity to test.</param>
        /// <returns>Boolean true if Entity1 and Entity2 have an existing relationship.</returns>
        bool DoesNNAssociationExist(Relationship relationship, EntityReference Entity1, EntityReference Entity2);

        /// <summary>
        /// Associate one or more related entites to a target entity.
        /// </summary>
        /// <param name="entityName">The Logical Name of the target entity.</param>
        /// <param name="entityId">The GUID that uniquely defines the target entity.</param>
        /// <param name="relationship">The Relationship to use for the association.</param>
        /// <param name="relatedEntities">A list of related EntityReference records to associate to the target.</param>
        void Associate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities);

        void BeginAssociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack);

        void EndAssociate(object asyncState);

        void Disassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities);

        void BeginDisassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack);

        void EndDisassociate(object asyncState);

        EntityCollection RetrieveMultiple(string fetchXml);

        void BeginRetrieveMultiple(string fetchXml, Action<object> callBack);

        EntityCollection EndRetrieveMultiple(object asyncState, Type entityType);

        Entity Retrieve(string entityName, string entityId, string[] attributesList);

        void BeginRetrieve(string entityName, string entityId, string[] attributesList, Action<object> callBack);

        Entity EndRetrieve(object asyncState, Type entityType);

        /// <summary>
        /// Creates a new entity record using the supplied Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Guid Create(Entity entity);

        void BeginCreate(Entity entity, Action<object> callBack);

        Guid EndCreate(object asyncState);

        void SetState(Guid id, string entityName, int stateCode, int statusCode);

        void BeginSetState(Guid id, string entityName, int stateCode, int statusCode, Action<object> callBack);

        void EndSetState(object asyncState);

        string Delete_(string entityName, Guid id);

        void BeginDelete(string entityName, Guid id, Action<object> callBack);

        void EndDelete(object asyncState);

        void Update(Entity entity);

        void BeginUpdate(Entity entity, Action<object> callBack);

        void EndUpdate(object asyncState);

        OrganizationResponse Execute(OrganizationRequest request);

        void BeginExecute(OrganizationRequest request, Action<object> callBack);

        OrganizationResponse EndExecute(object asyncState);

    }
}
