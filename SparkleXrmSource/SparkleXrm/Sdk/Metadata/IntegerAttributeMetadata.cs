// IntegerAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute type Integer.
    //[DataContract(Name = "IntegerAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class IntegerAttributeMetadata : AttributeMetadata
    {
      

        // Summary:
        //     Gets or sets the format options for the integer attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.IntegerFormat> The format
        //     options for the integer attribute.

        public IntegerFormat Format;
        //
        // Summary:
        //     Gets or sets the maximum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The maximum value for the attribute.
       
        public int? MaxValue ;
        //
        // Summary:
        //     Gets or sets the minimum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The minimum value for the attribute.

        public int? MinValue;
    }
}
