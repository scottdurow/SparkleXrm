// RetrieveAttributeRequest.cs
//
using System;
namespace Xrm.Sdk.Messages
{
    public class RetrieveAttributeRequest : OrganizationRequest
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
    }
}
