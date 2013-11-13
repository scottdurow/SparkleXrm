using System;
using System.Collections.Generic;
using System.Xml;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk
{
    public sealed class RetrieveRelationshipResponse : OrganizationResponse
    {
       

        public RetrieveRelationshipResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "RelationshipMetadata")
                {
                    XmlNode entity = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    RelationshipMetadata = MetadataSerialiser.DeSerialiseRelationshipMetadata(entity);


                }

            }
        }

        public RelationshipMetadataBase RelationshipMetadata;
        
    }
}
