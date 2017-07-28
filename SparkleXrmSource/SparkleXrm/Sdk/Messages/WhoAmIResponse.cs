// WhoAmIResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Messages
{

    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class WhoAmIResponse : OrganizationResponse, IWebAPIOrganizationResponse
    {
        public Guid OrganizationId;
        public Guid UserId;

        public WhoAmIResponse(XmlNode response)
        {
            if (response == null) return;
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                string keyName = XmlHelper.GetNodeTextValue(key);
                XmlNode valueNode = XmlHelper.SelectSingleNode(nameValuePair, "value");
                string value = XmlHelper.GetNodeTextValue(valueNode);
                switch (keyName)
                {
                    case "OrganizationId":
                        OrganizationId = new Guid(value);
                        break;
                    case "UserId":
                        UserId = new Guid(value);
                        break;

                }
                

            }
        }
        public void DeserialiseWebApi(Dictionary<string, object> response)
        {
            OrganizationId = new Guid((string)response["OrganizationId"]);
            UserId = new Guid((string)response["UserId"]);
        }
    }
}
