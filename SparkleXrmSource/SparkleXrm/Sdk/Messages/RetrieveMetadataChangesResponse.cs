// RetrieveMetadataChangesResponse.cs
//

using System;
using System.Collections.Generic;
using System.Xml;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk.Messages
{
    public class RetrieveMetadataChangesResponse : OrganizationResponse
    {

        public RetrieveMetadataChangesResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response,"Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                switch (XmlHelper.GetNodeTextValue(key))
                {
                    case "ServerVersionStamp":
                        this.ServerVersionStamp = XmlHelper.GetNodeTextValue(value);
                        break;
                    case "DeletedMetadata":
                        break;
                    case "EntityMetadata":
                        EntityMetadata = new List<EntityMetadata>();
                        for (int i = 0; i < value.ChildNodes.Count; i++)
                        {
                            XmlNode entity = value.ChildNodes[i];
                            EntityMetadata metaData = MetadataSerialiser.DeSerialiseEntityMetadata(new EntityMetadata(), entity);
                            EntityMetadata.Add(metaData);          
                        }
                        break;
                }
            }           
        }
        // Summary:
        //     Gets the deleted metadata since the last request.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.Query.DeletedMetadataCollectionThe deleted
        //     metadata since the last request.
        //public DeletedMetadataCollection DeletedMetadata;
        //
        // Summary:
        //     Gets the metadata defined by the request.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.EntityMetadataCollectionThe metadata defined
        //     by the request.
        public List<EntityMetadata> EntityMetadata;
        //
        // Summary:
        //     Gets a timestamp identifier for the metadata retrieved.
        //
        // Returns:
        //     Type: Returns_String A timestamp identifier for the metadata retrieved..
        public string ServerVersionStamp;
    }
}
