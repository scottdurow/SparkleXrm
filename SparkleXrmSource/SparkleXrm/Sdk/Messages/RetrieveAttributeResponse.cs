// RetrieveAttributeResponse.cs
//

using System.Xml;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk.Messages
{
    public class RetrieveAttributeResponse : OrganizationResponse
    {
        public AttributeMetadata AttributeMetadata;
        public RetrieveAttributeResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response,"Results");
            XmlNode metaData= XmlHelper.SelectSingleNode(results.FirstChild,"value");

            // Just serialise the option sets
            // Get the type
            string type = XmlHelper.GetAttributeValue(metaData, "i:type");
            switch (type)
            {
                case "c:PicklistAttributeMetadata":
                    AttributeMetadata = MetadataSerialiser.DeSerialisePicklistAttributeMetadata(new PicklistAttributeMetadata(), metaData);
                    break;
                // TODO: Add in more attributes types
            }
        }
    }
}
