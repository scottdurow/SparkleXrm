// OrganisationServiceMetadata.cs
//

using System;
using System.Collections.Generic;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk
{
    public static class OrganisationServiceMetadata
    {
        static OrganisationServiceMetadata()
        {
            // All the built in Request/Response types
            // More can be added for custom actions
            RegisterExecuteMessageResponseType("RetrieveAttribute", typeof(RetrieveAttributeResponse));
            RegisterExecuteMessageResponseType("RetrieveAllEntities", typeof(RetrieveAllEntitiesResponse));
            RegisterExecuteMessageResponseType("RetrieveEntity", typeof(RetrieveEntityResponse));
            RegisterExecuteMessageResponseType("BulkDeleteResponse", typeof(BulkDeleteResponse));
            RegisterExecuteMessageResponseType("FetchXmlToQueryExpression", typeof(FetchXmlToQueryExpressionResponse));
            RegisterExecuteMessageResponseType("RetrieveMetadataChanges", typeof(RetrieveMetadataChangesResponse));
            RegisterExecuteMessageResponseType("RetrieveRelationship", typeof(RetrieveRelationshipResponse));
            RegisterExecuteMessageResponseType("ExecuteWorkflow", typeof(ExecuteWorkflowResponse));
            RegisterExecuteMessageResponseType("Assign", typeof(AssignResponse));
          
        }
        public static Dictionary<string, Type> ExecuteMessageResponseTypes = new Dictionary<string, Type>();
        public static void RegisterExecuteMessageResponseType(string responseTypeName, Type organizationResponseType)
        {
            ExecuteMessageResponseTypes[responseTypeName] = organizationResponseType;

        }
    }
}
