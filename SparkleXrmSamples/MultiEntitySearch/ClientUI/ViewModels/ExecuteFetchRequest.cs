// ExecuteFetchRequest.cs
//

using System;
using System.Collections.Generic;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace Client.MultiEntitySearch.ViewModels
{
    /// <summary>
    /// Although depricated - this request is the only way to query the MultiEntitySearchEntities records
    /// </summary>
    public class ExecuteFetchRequest : OrganizationRequest
    {
        public string FetchXml;

        public string Serialise()
        {
            return String.Format("<request i:type=\"b:ExecuteFetchRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
               + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
                 + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>FetchXml</c:key>"
               + "            <c:value i:type=\"d:string\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\" >" + XmlHelper.Encode(FetchXml) + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "        </a:Parameters>"
               + "        <a:RequestId i:nil=\"true\" />"
               + "        <a:RequestName>ExecuteFetch</a:RequestName>"
               + "      </request>");
        }
    }
}
