// RetrieveMetadataChangesRequest.cs
//

using SparkleXrm.Sdk.Metadata.Query;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Serialization;
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
            request.OperationType = OperationTypeEnum.FunctionCall;
            request.RequestName = "RetrieveMetadataChanges";

            // The "Value" JSON serialisation needs a special Object type rather than just a primative
            // Replace the Values with the Value Types
            ReplaceCriteriaValues(Query.Criteria);
            
            if (this.Query.AttributeQuery!=null)
            {
                ReplaceCriteriaValues(Query.AttributeQuery.Criteria);
            }
            if (this.Query.RelationshipQuery != null)
            {
                ReplaceCriteriaValues(Query.RelationshipQuery.Criteria);
            }

            request.AdditionalProperties["Query"] = this.Query; 
            return request;
        }

        private void ReplaceCriteriaValues(MetadataFilterExpression criteria)
        {
            if (criteria!=null && criteria.Conditions != null)
            {
                foreach (MetadataConditionExpression expression in criteria.Conditions)
                {
                    // Check if the value is already an ObjectValue
                    if (Script.IsUndefined(((ObjectValue)expression.Value).Type))
                    {
                        ObjectValue value = new ObjectValue();
                        value.Value = expression.Value;
                        value.Type = "System.String";
                        expression.Value = value;
                    }
                }
                if (criteria.Filters!=null)
                {
                    foreach (MetadataFilterExpression filter in criteria.Filters)
                    {
                        ReplaceCriteriaValues(filter);
                    }
                }
            }
        }

    }
}
