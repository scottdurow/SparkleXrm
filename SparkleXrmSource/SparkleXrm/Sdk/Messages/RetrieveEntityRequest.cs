// RetrieveEntityRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveEntityRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {

        // Summary:
        //     constructor_initializesMicrosoft.Xrm.Sdk.Messages.RetrieveEntityRequest class.
        //public RetrieveEntityRequest();

        // Summary:
        //     Gets or sets a filter to control how much data for the entity is retrieved.
        //     Required.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.EntityFiltersa filter to control how much
        //     data for the entity is retrieved. Required.
        [PreserveCase]
        public EntityFilters EntityFilters;
        //
        // Summary:
        //     Gets or sets the logical name of the entity to be retrieved. Optional.
        //
        // Returns:
        //     Type: Returns_StringThe logical name of the entity to be retrieved. Optional.
        [PreserveCase]
        public string LogicalName;
        //
        // Summary:
        //     A unique identifier for the entity. Optional.
        //
        // Returns:
        //     Type: Returns_GuidThe A unique identifier for the entity. This corresponds
        //     to the Microsoft.Xrm.Sdk.Metadata.MetadataBase. Microsoft.Xrm.Sdk.Metadata.MetadataBase.MetadataId
        //     for the entity.
        [PreserveCase]
        public Guid MetadataId;
        //
        // Summary:
        //     Gets or sets whether to retrieve the metadata that has not been published.
        //     Required.
        //
        // Returns:
        //     Type: Returns_Booleantrue if the metadata that has not been published should
        //     be retrieved; otherwise, false.
        [PreserveCase]
        public bool RetrieveAsIfPublished;

        public string Serialise()
        {
            return  "<request i:type=\"a:RetrieveEntityRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\">" +
                            "<a:Parameters xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">" +
                            "<a:KeyValuePairOfstringanyType>" +
                            "<b:key>EntityFilters</b:key>" +
                            Attribute.SerialiseValue(EntityFilters,"EntityFilters") + 
                            "</a:KeyValuePairOfstringanyType>" +
                            "<a:KeyValuePairOfstringanyType>" +
                            "<b:key>MetadataId</b:key>" +
                            Attribute.SerialiseValue(MetadataId, null) +
                            "</a:KeyValuePairOfstringanyType>" +
                            "<a:KeyValuePairOfstringanyType>" +
                            "<b:key>RetrieveAsIfPublished</b:key>" +
                            Attribute.SerialiseValue(RetrieveAsIfPublished, null) +
                            "</a:KeyValuePairOfstringanyType>" +
                            "<a:KeyValuePairOfstringanyType>" +
                            "<b:key>LogicalName</b:key>" +
                            Attribute.SerialiseValue(LogicalName, null) +    
                            "</a:KeyValuePairOfstringanyType>" +
                            "</a:Parameters>" +
                            "<a:RequestId i:nil=\"true\" />" +
                            "<a:RequestName>RetrieveEntity</a:RequestName>" +
                        "</request>";
                        
        }

        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.CustomImplementation = CustomWebApiImplementation;
            return request;
        }
        private void CustomWebApiImplementation(OrganizationRequest request, Action<object> callback, Action<object> errorCallback, bool async)
        {
            RetrieveEntityRequest requestTyped = (RetrieveEntityRequest)request;
            // Make a call to http://dev04/Contoso/api/data/v8.2/EntityDefinitions?$select=LogicalName&$filter=LogicalName eq 'account'
            string select = "ActivityTypeMask,AutoRouteToOwnerQueue,CanTriggerWorkflow,Description,DisplayCollectionName,EntityHelpUrlEnabled,EntityHelpUrl,IsDocumentManagementEnabled,IsOneNoteIntegrationEnabled,IsSLAEnabled,IsBPFEntity,IsActivity,IsActivityParty,IsAuditEnabled,IsAvailableOffline,IsChildEntity,IsValidForQueue,IsConnectionsEnabled,IconLargeName,IconMediumName,IconSmallName,IsCustomEntity,IsBusinessProcessEnabled,IsCustomizable,IsDuplicateDetectionEnabled,IsIntersect,IsValidForAdvancedFind,LogicalName,ObjectTypeCode,OwnershipType,PrimaryNameAttribute,PrimaryImageAttribute,PrimaryIdAttribute,SchemaName,EntityColor,LogicalCollectionName,CollectionSchemaName,EntitySetName,MetadataId,HasChanged";
            string query = string.Format("$filter=LogicalName eq '{0}'", requestTyped.LogicalName);
            List<string> expand = new List<string>();
            if ((requestTyped.EntityFilters & EntityFilters.Attributes) == EntityFilters.Privileges)
            {
                select += ",Privileges";
            }
            if ((requestTyped.EntityFilters & EntityFilters.Attributes) == EntityFilters.Attributes)
            {
                expand.Add("Attributes($select=AttributeOf,AttributeType,AttributeTypeName,DisplayName,LogicalName,RequiredLevel,SchemaName)");
            }
            if ((requestTyped.EntityFilters & EntityFilters.Relationships) == EntityFilters.Relationships)
            {
                expand.Add("ManyToManyRelationships");
                expand.Add("ManyToOneRelationships");
                expand.Add("OneToManyRelationships");
            }
            if (expand.Count>0)
            {
                query += "&$expand=" + expand.Join(",");
            }
            WebApiOrganizationServiceProxy.SendRequest("EntityDefinition","EntityDefinitions", query, "GET", null, async, delegate (object state)
            {
                Dictionary<string, object> data = WebApiOrganizationServiceProxy.JsonParse(state);
                object[] value = (object[])data["value"];
                RetrieveEntityResponse response = new RetrieveEntityResponse(null);
                response.EntityMetadata = (EntityMetadata)value[0];
                callback(response);
               
            }, errorCallback);

        }
    }

    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    [Flags]
    public enum EntityFilters
    {
        // Summary:
        //     Use this to retrieve only entity information. Equivalent to EntityFilters.Entity.
        //     Value = 1.
        Default_ = 1,
        //
        // Summary:
        //     Use this to retrieve only entity information. Equivalent to EntityFilters.Default.
        //     Value = 1.

        Entity = 1,
        //
        // Summary:
        //     Use this to retrieve entity information plus attributes for the entity. Value
        //     = 2.

        Attributes = 2,
        //
        // Summary:
        //     Use this to retrieve entity information plus privileges for the entity. Value
        //     = 4.

        Privileges = 4,
        //
        // Summary:
        //     Use this to retrieve entity information plus entity relationships for the
        //     entity. Value = 8.

        Relationships = 8,
        //
        // Summary:
        //     Use this to retrieve all data for an entity. Value = 15.
        All = 15,
    }
}
