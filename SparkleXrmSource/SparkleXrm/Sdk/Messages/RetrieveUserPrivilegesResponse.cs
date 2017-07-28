// RetrieveUserPrivilegesResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Messages
{
    public class RolePrivilege
    {
        [PreserveCase]
        public Guid BusinessUnitId;
        [PreserveCase]
        public PrivilegeDepth Depth;
        [PreserveCase]
        public Guid PrivilegeId;
    }


    [Imported]
    [IgnoreNamespace]
    [NamedValues]
    public enum PrivilegeDepth
    {
        [PreserveCase]
        Basic = 0,
        [PreserveCase]
        Deep = 1,
        [PreserveCase]
        Global = 2,
        [PreserveCase]
        Local = 3
    }

    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveUserPrivilegesResponse : OrganizationResponse, IWebAPIOrganizationResponse
    {
        public List<RolePrivilege> RolePrivileges;

        public RetrieveUserPrivilegesResponse(XmlNode response)
        {
            if (response == null) return;
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "RolePrivileges")
                {
                    RolePrivileges = new List<RolePrivilege>();
                   
                    XmlNode queryNode = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    // Get each priviledge
                    foreach (XmlNode priv in queryNode.ChildNodes)
                    {
                        RolePrivilege priviledge = new RolePrivilege();
                        foreach (XmlNode privValue in priv.ChildNodes)
                        {
                            string nodeValue = XmlHelper.GetNodeTextValue(privValue);
                            switch (XmlHelper.GetLocalName(privValue))
                            {
                                case "BusinessUnitId":
                                    priviledge.BusinessUnitId = new Guid(nodeValue);
                                    break;
                                case "Depth":
                                    priviledge.Depth = (PrivilegeDepth)(object)nodeValue;
                                    break;
                                case "PrivilegeId":
                                    priviledge.PrivilegeId = new Guid(nodeValue);
                                    break;
                              

                            }
                        }
                        RolePrivileges.Add(priviledge);

                    }
                }

            }
        }
     

        public void DeserialiseWebApi(Dictionary<string, object> response)
        {

            RolePrivileges = (List<RolePrivilege>)response["RolePrivileges"];
            // convert strings to guids
            foreach (RolePrivilege priv in RolePrivileges)
            {
                priv.BusinessUnitId = new Guid(priv.BusinessUnitId.ToString());
                priv.PrivilegeId = new Guid(priv.PrivilegeId.ToString());
            }
        }
    }
}
