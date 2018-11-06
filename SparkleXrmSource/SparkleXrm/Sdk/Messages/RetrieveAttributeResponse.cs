// RetrieveAttributeResponse.cs
//

using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk.Metadata;
using SparkleXrm.Sdk.Metadata;
using SparkleXrm.Sdk.Metadata.Query;
namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveAttributeResponse : OrganizationResponse
    {
        [PreserveCase]
        public AttributeMetadata AttributeMetadata;
        public RetrieveAttributeResponse(XmlNode response)
        {
            if (response == null)
                return;
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
