using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public sealed class RetrieveRelationshipRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        [PreserveCase]
        public Guid MetadataId = Guid.Empty;
        [PreserveCase]
        public string Name;
        [PreserveCase]
        public bool RetrieveAsIfPublished;

        public string Serialise()
        {
            return "<request i:type=\"a:RetrieveRelationshipRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\">" +
                         "<a:Parameters xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">" +
                          "<a:KeyValuePairOfstringanyType>" +
                           "<b:key>MetadataId</b:key>" +
                           Attribute.SerialiseValue(MetadataId, null) +
                          "</a:KeyValuePairOfstringanyType>" +
                          "<a:KeyValuePairOfstringanyType>" +
                           "<b:key>Name</b:key>" +
                            Attribute.SerialiseValue(Name, null) +
                          "</a:KeyValuePairOfstringanyType>" +
                          "<a:KeyValuePairOfstringanyType>" +
                           "<b:key>RetrieveAsIfPublished</b:key>" +
                           Attribute.SerialiseValue(RetrieveAsIfPublished, null) +
                          "</a:KeyValuePairOfstringanyType>" +
                         "</a:Parameters>" +
                         "<a:RequestId i:nil=\"true\" />" +
                         "<a:RequestName>RetrieveRelationship</a:RequestName>" +
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
            RetrieveRelationshipRequest requestTyped = (RetrieveRelationshipRequest)request;
            string query = "$filter=SchemaName eq '" + requestTyped.Name + "'";
            WebApiOrganizationServiceProxy.SendRequest("RelationshipDefinition", "RelationshipDefinitions", query, "GET", null, async, delegate (object state)
            {
                Dictionary<string, object> data = WebApiOrganizationServiceProxy.JsonParse(state);
                object[] value = (object[])data["value"];
                RetrieveRelationshipResponse response = new RetrieveRelationshipResponse(null);
                RelationshipMetadataBase[] entityMetadata = (RelationshipMetadataBase[])(object)value;
                response.RelationshipMetadata = entityMetadata[0];
                callback(response);

            }, errorCallback);

        }
    }
}
