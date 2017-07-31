// DecimalAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute type Decimal.
    //[DataContract(Name = "DecimalAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class DecimalAttributeMetadata : AttributeMetadata
    {




        // Summary:
        //     Gets or sets the input method editor (IME) mode for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.ImeMode> The input method
        //     editor (IME) mode for the attribute..

        //public ImeMode? ImeMode { get; set; }
        //
        // Summary:
        //     Gets or sets the maximum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Decimal> The maximum value for the attribute.
        [PreserveCase]
        public decimal? MaxValue;
        //
        // Summary:
        //     Gets or sets the minimum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Decimal> The minimum value for the attribute.
        [PreserveCase]
        public decimal? MinValue;
        //
        // Summary:
        //     Gets or sets the precision for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The precision for the attribute.
        [PreserveCase]
        public int? Precision;
    }
}
