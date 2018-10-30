using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Microsoft.Crm.Sdk.Fakes
{
    /// <summary>
    /// Sets the stub of IOrganizationService.Associate(String entityName, Guid entityId,
    ///    Relationship relationship, EntityReferenceCollection relatedEntities)
    /// </summary>
    /// <param name="entityName"></param>
    /// <param name="entityId"></param>
    /// <param name="relationship"></param>
    /// <param name="relatedEntities"></param>
    public delegate void AssociateDelegate (string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);
    public delegate void DisassociateDelegate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);
    public delegate Guid CreateDelegate(Entity entity);
    public delegate void DeleteDelegate(string entityName, Guid id);
    public delegate OrganizationResponse ExecuteDelegate(OrganizationRequest request);
    public delegate Entity RetrieveDelegate(string entityName, Guid id, ColumnSet columnSet);
    public delegate EntityCollection RetrieveMultipleDelegate(QueryBase query);
    public delegate void UpdateDelegate(Entity entity);

    public class OrganizationServiceStep
    {
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Associate(String entityName, Guid entityId,
        //     Relationship relationship, EntityReferenceCollection relatedEntities)
        public AssociateDelegate Associate;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Create(Entity entity)
        public CreateDelegate Create;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Delete(String entityName, Guid id)
        public DeleteDelegate Delete;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Disassociate(String entityName, Guid entityId,
        //     Relationship relationship, EntityReferenceCollection relatedEntities)
        public DisassociateDelegate Disassociate;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Execute(OrganizationRequest request)
        public ExecuteDelegate Execute;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.RetrieveMultiple(QueryBase query)
        public RetrieveMultipleDelegate RetrieveMultiple;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Retrieve(String entityName, Guid id, ColumnSet
        //     columnSet)
        public RetrieveDelegate Retrieve;
        //
        // Summary:
        //     Sets the stub of IOrganizationService.Update(Entity entity)
        public UpdateDelegate Update;
    }
}
