using System;
using System.Collections.Generic;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk
{
    public sealed class RetrieveRelationshipRequest : OrganizationRequest
    {
        public Guid MetadataId = Guid.Empty;
        public string Name;
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
    }
}
