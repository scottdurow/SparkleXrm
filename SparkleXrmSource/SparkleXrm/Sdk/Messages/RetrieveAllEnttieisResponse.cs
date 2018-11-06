// RetrieveAttributeResponse.cs
//

using SparkleXrm.Sdk.Metadata;
using SparkleXrm.Sdk.Metadata.Query;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveAllEntitiesResponse : OrganizationResponse
    {
        [PreserveCase]
        public EntityMetadata[] EntityMetadata;
        public RetrieveAllEntitiesResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response,"Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "EntityMetadata")
                {
                    XmlNode values = XmlHelper.SelectSingleNode(nameValuePair,"value");
                
                    EntityMetadata = new EntityMetadata[values.ChildNodes.Count];
                    for (int i = 0; i < values.ChildNodes.Count;i++)
                    {
                        XmlNode entity = values.ChildNodes[i];
                        EntityMetadata metaData = MetadataSerialiser.DeSerialiseEntityMetadata(new EntityMetadata(), entity);
                        EntityMetadata[i] = metaData;
                    }
                }

            }            
        }
    }
}
