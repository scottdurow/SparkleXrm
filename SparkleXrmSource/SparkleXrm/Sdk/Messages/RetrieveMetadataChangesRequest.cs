// RetrieveMetadataChangesRequest.cs
//

using SparkleXrm.Sdk.Metadata.Query;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Metadata;
using Xrm.Sdk.Metadata.Query;

namespace Xrm.Sdk.Messages
{
   [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveMetadataChangesRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {

        // Summary:
        //     Gets or sets a timestamp value representing when the last request was made.
        //
        // Returns:
        //     Type: Returns_String A timestamp value representing when the last request
        //     was made.
        public string ClientVersionStamp;
        //
        // Summary:
        //     Gets or sets a value to filter what deleted metadata items will be returned.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.Query.DeletedMetadataFiltersA value to filter
        //     what deleted metadata items will be returned.
        public DeletedMetadataFilters DeletedMetadataFilters;
        //
        // Summary:
        //     Gets or sets the query representing the metadata to return.
        //
        // Returns:
        //     Type:Microsoft.Xrm.Sdk.Metadata.Query.EntityQueryExpressionThe query representing
        //     the metadata to return.
        public EntityQueryExpression Query;


        public string Serialise()
        {
          

            return @"<request i:type='a:RetrieveMetadataChangesRequest' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts'>
                <a:Parameters xmlns:b='http://schemas.datacontract.org/2004/07/System.Collections.Generic'>
                  <a:KeyValuePairOfstringanyType>
                    <b:key>ClientVersionStamp</b:key>" + Attribute.SerialiseValue(ClientVersionStamp,null) + @"
                  </a:KeyValuePairOfstringanyType>
                  <a:KeyValuePairOfstringanyType>
                    <b:key>Query</b:key>
                    " + MetadataSerialiser.SerialiseEntityQueryExpression(Query) + @"
                  </a:KeyValuePairOfstringanyType>
                </a:Parameters>
                <a:RequestId i:nil='true' />
                <a:RequestName>RetrieveMetadataChanges</a:RequestName>
              </request>";
            

        }

        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.CustomImplementation = CustomWebApiImplementation;
            return request;
        }

        private void CustomWebApiImplementation(OrganizationRequest request, Action<object> callback, Action<object> errorCallback, bool async)
        {
            RetrieveMetadataChangesRequest requestTyped = (RetrieveMetadataChangesRequest)request;

            // Query:
            // api/data/v8.2/EntityDefinitions?$select=LogicalName&$filter=LogicalName eq 'account' or LogicalName eq 'contact'&$expand=Attributes($select=AttributeOf,AttributeType;$filter=LogicalName eq 'name')

            List<string> entityLogicalNames = new List<string>();
            List<string> attributeLogicalNames = new List<string>();
            List<string> relationshipSchemaNames = new List<string>();
            List<string> expands = new List<string>();
            List<string> parts = new List<string>();
            // Get the Entity Query

            if (requestTyped.Query == null)
                throw new Exception("Query not set on RetrieveMetadataChangesRequest");

            if (requestTyped.Query.Criteria != null)
            {
                foreach (MetadataConditionExpression filter in requestTyped.Query.Criteria.Conditions)
                {
                    if (filter.PropertyName == "LogicalName")
                    {
                        entityLogicalNames.Add((string)filter.Value);
                    }
                }
            }
            if (requestTyped.Query.AttributeQuery != null)
            {
                foreach (MetadataConditionExpression filter in requestTyped.Query.AttributeQuery.Criteria.Conditions)
                {
                    if (filter.PropertyName == "LogicalName")
                    {
                        attributeLogicalNames.Add((string)filter.Value);
                    }
                }
            }
            if (requestTyped.Query.RelationshipQuery != null)
            {
                foreach (MetadataConditionExpression filter in requestTyped.Query.RelationshipQuery.Criteria.Conditions)
                {
                    if (filter.PropertyName == "SchemaName")
                    {
                        relationshipSchemaNames.Add((string)filter.Value);
                    }
                }
            }
            if (requestTyped.Query.Properties.PropertyNames != null)
            {
                parts.Add("$select=" + requestTyped.Query.Properties.PropertyNames.Join(","));
            }

            if (entityLogicalNames.Count>0)
            {
                parts.Add("$filter=LogicalName eq '" + entityLogicalNames.Join("' or LogicalName eq '") + "'");
                
            }
            
         

            if (attributeLogicalNames.Count>0)
            {
                string attributeFilter = "LogicalName eq '" + attributeLogicalNames.Join("' or LogicalName eq '") + "'";
                string attributeProperties = requestTyped.Query.AttributeQuery.Properties.PropertyNames!=null ? "$select=" + requestTyped.Query.AttributeQuery.Properties.PropertyNames.Join(",") + ";" :null;

                string expandTerm = String.Format("Attributes({0}$filter={1})", attributeProperties, attributeFilter);
                expands.Add(expandTerm);
            }
            if (expands.Count > 0)
            {
                parts.Add("$expand=" + expands.Join(","));
            }

            string query = parts.Join("&");
            WebApiOrganizationServiceProxy.SendRequest("EntityDefinition", "EntityDefinitions", query, "GET", null, async, delegate (object state)
            {
                Dictionary<string, object> data = WebApiOrganizationServiceProxy.JsonParse(state);
                object[] value = (object[])data["value"];
                RetrieveMetadataChangesResponse response = new RetrieveMetadataChangesResponse(null);
                List<EntityMetadata> entityMetadata = ((List<EntityMetadata>)(object)value);
                response.EntityMetadata = entityMetadata;

                // Get all the optionsets and request the picklist data
                // TaskItterator
                // TODO


                //// If an optionset we need to make another request
                //if (response.AttributeMetadata.AttributeType == AttributeTypeCode.Picklist)
                //{
                //    string resource = string.Format("EntityDefinitions({0})/Attributes({1})/Microsoft.Dynamics.CRM.PicklistAttributeMetadata/OptionSet", entityMetadata.MetadataId, response.AttributeMetadata.MetadataId);
                //    WebApiOrganizationServiceProxy.SendRequest("Attribute", resource, "$select=Options", "GET", null, async, delegate (object picklistState)
                //    {
                //        Dictionary<string, object> picklistdata = WebApiOrganizationServiceProxy.JsonParse(picklistState);
                //        PicklistAttributeMetadata picklistMetadata = (PicklistAttributeMetadata)response.AttributeMetadata;
                //        picklistMetadata.OptionSet = (OptionSetMetadata)(object)picklistdata;

                //        callback(response);
                //    }, errorCallback);
                //}
                //else
                //{
                    callback(response);
                //}
            }, errorCallback);

        }
    }
}
