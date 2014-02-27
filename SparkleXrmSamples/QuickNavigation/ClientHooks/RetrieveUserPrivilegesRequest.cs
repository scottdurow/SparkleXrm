// RetrieveUserPrivilegesRequest.cs
//

using System;
using System.Collections.Generic;
using System.Xml;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace QuickNavigation.ClientHooks
{
    public class RetrieveUserPrivilegesRequest : OrganizationRequest
    {
        public Guid UserId;
        public string Serialise()
        {
            //<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body>
            //<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            //<request i:type="b:RetrieveUserPrivilegesRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">
            //<a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic"><a:KeyValuePairOfstringanyType>
            //<c:key>UserId</c:key>
            //<c:value i:type="d:guid" xmlns:d="http://schemas.microsoft.com/2003/10/Serialization/">@UserId</c:value>
            //</a:KeyValuePairOfstringanyType></a:Parameters><a:RequestId i:nil="true" /><a:RequestName>RetrieveUserPrivileges</a:RequestName></request></Execute></s:Body></s:Envelope>
            return "<request i:type=\"b:RetrieveUserPrivilegesRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
               + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
                 + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>UserId</c:key>"
               + "            <c:value i:type=\"d:guid\" xmlns:d=\"http://schemas.microsoft.com/2003/10/Serialization/\" >" + XmlHelper.Encode(UserId.Value) + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "        </a:Parameters>"
               + "        <a:RequestId i:nil=\"true\" />"
               + "        <a:RequestName>RetrieveUserPrivileges</a:RequestName>"
               + "      </request>";
        }
    }
    public class RetrieveUserPrivilegesResponse : OrganizationResponse
    {
        public List<RolePrivilege> RolePrivileges;
        public RetrieveUserPrivilegesResponse(XmlNode response)
        {
            
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "RolePrivileges")
                {
                    XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    this.RolePrivileges = new List<RolePrivilege>();
                    
                    foreach (XmlNode privNode in value.ChildNodes)
                    {
                        RolePrivilege priv = new RolePrivilege();
                        priv.PrivilegeId = new Guid(XmlHelper.SelectSingleNodeValue(privNode, "PrivilegeId"));
                        ArrayEx.Add(RolePrivileges,priv);
                    }
                }
            }
        }
    }
    public class RolePrivilege
    {
        public Guid PrivilegeId;

    }

}
