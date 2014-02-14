// OrganizationResponse.cs
//

using System;
using System.Collections.Generic;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace Client.MultiEntitySearch.ViewModels
{
    public class ExecuteFetchResponse : OrganizationResponse
    {
        public ExecuteFetchResponse(XmlNode response)
        {

            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "FetchXmlResult")
                {
                    XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    this.FetchXmlResult = XmlHelper.GetNodeTextValue(value);
                }
            }
        }
        public string FetchXmlResult;
    }
}
