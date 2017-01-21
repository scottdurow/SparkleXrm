// EntityQueryExpression.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Metadata.Query;

namespace SparkleXrm.Sdk.Metadata.Query
{
   
    public static partial class MetadataSerialiser
    {
        public static string SerialiseEntityQueryExpression(EntityQueryExpression item)
        {
            if (item != null)
            {
                string xml =
                @"<b:value i:type='c:EntityQueryExpression' xmlns:c='http://schemas.microsoft.com/xrm/2011/Metadata/Query'>"
                + MetadataSerialiser.SerialiseMetadataQueryExpression(item);
                if (item.AttributeQuery!=null)
                {
                xml += @"<c:AttributeQuery>"
                + MetadataSerialiser.SerialiseAttributeQueryExpression(item.AttributeQuery)
                + @"</c:AttributeQuery>";
                }
                xml +=@"<c:LabelQuery>"
                + MetadataSerialiser.SerialiseLabelQueryExpression(item.LabelQuery)
                + @"</c:LabelQuery>
                <c:RelationshipQuery i:nil='true' />
                </b:value>";
                return xml;
            }
            else
                return "<b:value i:nil='true'/>";
        }

    }
}

namespace Xrm.Sdk.Metadata.Query
{
    // Summary:
    //     Defines a complex query to retrieve entity metadata.
    //[DataContract(Name = "EntityQueryExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class EntityQueryExpression : MetadataQueryExpression
    {
       

        // Summary:
        //     Gets or sets a query expression for the entity attribute metadata to return.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.Query.AttributeQueryExpression.

        public AttributeQueryExpression AttributeQuery;
        //
        // Summary:
        //     Gets or sets a query expression for the labels to return.
        //
        // Returns:
        //     Type:Microsoft.Xrm.Sdk.Metadata.Query.LabelQueryExpression.

        public LabelQueryExpression LabelQuery;
        //
        // Summary:
        //     Gets or sets a query expression for the entity relationship metadata to return.
        //
        // Returns:
        //     Type:Microsoft.Xrm.Sdk.Metadata.Query.RelationshipQueryExpression.

        public RelationshipQueryExpression RelationshipQuery;
    }
}
