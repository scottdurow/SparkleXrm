// AddToQueueResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class AddToQueueResponse : IWebAPIOrganizationResponse
    {
        public Guid QueueItemId;

        public AddToQueueResponse(XmlNode response)
        {
            if (response == null) return;
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "QueueItemId")
                {
                    XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    this.QueueItemId = new Guid(XmlHelper.GetNodeTextValue(value));
                }
            }
        }


        public void DeserialiseWebApi(Dictionary<string, object> response)
        {

            QueueItemId = new Guid((string)response["QueueItemId"]);
        }
    }
}
