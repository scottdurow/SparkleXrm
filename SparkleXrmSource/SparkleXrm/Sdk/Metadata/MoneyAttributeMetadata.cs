// MoneyAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute type Money.
    //[DataContract(Name = "MoneyAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class MoneyAttributeMetadata : AttributeMetadata
    {

        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Returns_Stringinternal
        [PreserveCase]
        public string CalculationOf ;
        //
        // Summary:
        //     Gets or sets the input method editor (IME) mode for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.ImeMode> The input method
        //     editor (IME) mode for the attribute..

        //public ImeMode? ImeMode ;
        //
        // Summary:
        //     Gets or sets the maximum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Double> The maximum value for the attribute.
        [PreserveCase]
        public double? MaxValue ;
        //
        // Summary:
        //     Gets or sets the minimum value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Double> The minimum value for the attribute.
        [PreserveCase]
        public double? MinValue ;
        //
        // Summary:
        //     Gets or sets the precision for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The precision for the attribute.
        [PreserveCase]
        public int? Precision ;
        //
        // Summary:
        //     Gets or sets the precision source for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The precision source for the attribute.
        [PreserveCase]
        public int? PrecisionSource ;
    }
}
