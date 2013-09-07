// AttributeQueryExpression.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata.Query
{
    public static partial class MetadataSerialiser
    {
        public static string SerialiseAttributeQueryExpression(AttributeQueryExpression item)
        {
            return MetadataSerialiser.SerialiseMetadataQueryExpression(item);
        }
    }
    // Summary:
    //     Defines a complex query to retrieve attribute metadata for entities retrieved
    //     using an Microsoft.Xrm.Sdk.Metadata.Query.EntityQueryExpression
    //[DataContract(Name = "AttributeQueryExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class AttributeQueryExpression : MetadataQueryExpression
    {
        
    }
}
