// RetrieveEntityResponse.cs
//

using SparkleXrm.Sdk.Metadata;
using SparkleXrm.Sdk.Metadata.Query;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk.Messages
{
   [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveEntityResponse : OrganizationResponse
    {
     
        public RetrieveEntityResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "EntityMetadata")
                {
                    XmlNode entity = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    EntityMetadata = MetadataSerialiser.DeSerialiseEntityMetadata(new EntityMetadata(), entity);
                        
                    
                }

            }
        }

        
        /// <summary>
        /// Gets the metadata for the requested entity.
        /// </summary>
        /// <returns>MetadataThe metadata for the requested entity</returns>
        public EntityMetadata EntityMetadata;
    }
}
