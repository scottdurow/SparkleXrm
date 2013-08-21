// BigIntAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute type BigInt.
    //[DataContract(Name = "BigIntAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class BigIntAttributeMetadata : AttributeMetadata
    {
       

       

        // Summary:
        //     Gets or sets the maximum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int64> The maximum value for the attribute.

        public long? MaxValue;
        //
        // Summary:
        //     Gets or sets the minimum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int64> The minimum value for the attribute.
        public long? MinValue;
    }
}
