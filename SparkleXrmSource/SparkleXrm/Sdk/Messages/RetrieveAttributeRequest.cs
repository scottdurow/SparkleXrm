// RetrieveAttributeRequest.cs
//
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveAttributeRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        public string EntityLogicalName;
        public string LogicalName;
        public bool RetrieveAsIfPublished;

        public string Serialise()
        {
            return String.Format("<request i:type=\"a:RetrieveAttributeRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\">" +
                  "<a:Parameters xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">" +
                   "<a:KeyValuePairOfstringanyType>" +
                    "<b:key>EntityLogicalName</b:key>" +
                    "<b:value i:type=\"c:string\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">{0}</b:value>" +
                    "</a:KeyValuePairOfstringanyType>" + 
                   "<a:KeyValuePairOfstringanyType>" + 
                    "<b:key>MetadataId</b:key>" +
                    "<b:value i:type=\"ser:guid\"  xmlns:ser=\"http://schemas.microsoft.com/2003/10/Serialization/\">00000000-0000-0000-0000-000000000000</b:value>" +
                   "</a:KeyValuePairOfstringanyType>" +
                    "<a:KeyValuePairOfstringanyType>" +
                    "<b:key>RetrieveAsIfPublished</b:key>" +
                  "<b:value i:type=\"c:boolean\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">{2}</b:value>" +
                   "</a:KeyValuePairOfstringanyType>" +
                   "<a:KeyValuePairOfstringanyType>" +
                    "<b:key>LogicalName</b:key>" +
                    "<b:value i:type=\"c:string\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">{1}</b:value>" +
                   "</a:KeyValuePairOfstringanyType>" +
                  "</a:Parameters>" +
                  "<a:RequestId i:nil=\"true\" />" +
                  "<a:RequestName>RetrieveAttribute</a:RequestName>" +
                 "</request>",this.EntityLogicalName,this.LogicalName,this.RetrieveAsIfPublished);
        }


        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.CustomImplementation = CustomWebApiImplementation;
            return request;
        }

        private void CustomWebApiImplementation(OrganizationRequest request, Action<object> callback, Action<object> errorCallback, bool async)
        {
            RetrieveAttributeRequest requestTyped = (RetrieveAttributeRequest)request;

            //api/data/v8.2/EntityDefinitions?$select=LogicalName&$filter=LogicalName%20eq%20%27account%27&$expand=Attributes($filter=LogicalName%20eq%20%27name%27)
            string query = string.Format("$select=LogicalName&$filter=LogicalName eq '{0}'&$expand=Attributes($filter=LogicalName eq '{1}')", requestTyped.EntityLogicalName, requestTyped.LogicalName);
            List<string> expand = new List<string>();

            WebApiOrganizationServiceProxy.SendRequest("EntityDefinition", "EntityDefinitions", query, "GET", null, async, delegate (object state)
            {
                Dictionary<string, object> data = WebApiOrganizationServiceProxy.JsonParse(state);
                object[] value = (object[])data["value"];
                RetrieveAttributeResponse response = new RetrieveAttributeResponse(null);
                EntityMetadata entityMetadata = ((EntityMetadata)value[0]);
                response.AttributeMetadata = entityMetadata.Attributes[0];
                // If an optionset we need to make another request
                if (response.AttributeMetadata.AttributeType == AttributeTypeCode.Picklist)
                {
                    string resource = string.Format("EntityDefinitions({0})/Attributes({1})/Microsoft.Dynamics.CRM.PicklistAttributeMetadata/OptionSet", entityMetadata.MetadataId, response.AttributeMetadata.MetadataId);
                    WebApiOrganizationServiceProxy.SendRequest("Attribute", resource, "$select=Options", "GET", null, async, delegate (object picklistState)
                    {
                        Dictionary<string, object> picklistdata = WebApiOrganizationServiceProxy.JsonParse(picklistState);
                        PicklistAttributeMetadata picklistMetadata = (PicklistAttributeMetadata)response.AttributeMetadata;
                        picklistMetadata.OptionSet = (OptionSetMetadata)(object)picklistdata;

                        callback(response);
                    }, errorCallback);
                }
                else
                {
                    callback(response);
                }
            }, errorCallback);

        }
    }
}
