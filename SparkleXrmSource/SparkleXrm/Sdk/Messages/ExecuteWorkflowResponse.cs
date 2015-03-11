// ExecuteWorkflowResponse.cs
//

using System;
using System.Collections.Generic;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk.Messages
{
    public class ExecuteWorkflowResponse : OrganizationResponse
    {
        public string Id;

        public ExecuteWorkflowResponse(XmlNode response)
        {

            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "Id")
                {
                    XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    this.Id = XmlHelper.GetNodeTextValue(value);
                }
            }
        }
       
    }
}
