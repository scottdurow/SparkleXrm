using SparkleXrm.Sdk.Metadata;
using SparkleXrm.Sdk.Metadata.Query;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public sealed class RetrieveRelationshipResponse : OrganizationResponse
    {
       
        public RetrieveRelationshipResponse(XmlNode response)
        {
            if (response == null)
                return;

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
        [PreserveCase]
        public RelationshipMetadataBase RelationshipMetadata;

       
    }
}
