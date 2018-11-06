// RetrieveDuplicatesResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Messages
{
    public class RetrieveDuplicatesResponse : OrganizationResponse,IWebAPIOrganizationResponse
    {
        [PreserveCase]
        public EntityCollection DuplicateCollection;

        public RetrieveDuplicatesResponse(XmlNode response)
        {
            if (response == null) return;
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");  

                if (XmlHelper.GetNodeTextValue(key) == "DuplicateCollection")
                {
                    XmlNode queryNode = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    DuplicateCollection = EntityCollection.DeSerialise(queryNode);
                }

            }
        }

        public void DeserialiseWebApi(Dictionary<string, object> response)
        {
            DuplicateCollection = EntityCollection.DeserialiseWebApi(typeof(Entity), null, response);
        }
    }
}
