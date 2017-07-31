// StringAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute of type String.
    //[DataContract(Name = "StringAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class StringAttributeMetadata : AttributeMetadata
    {

        // Summary:
        //     Gets or sets the format for the string.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.StringFormat> The format
        //     of the attribute.
        [PreserveCase]
        public StringFormat Format ;
        //
        // Summary:
        //     Gets or sets the IME mode for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.ImeMode> The input method
        //     editor mode.

        //public ImeMode? ImeMode ;
        //
        // Summary:
        //     Gets or sets the maximum length for the string.
        //
        // Returns:
        //     Type: Returns_Nullable<int> The maximum length.
        [PreserveCase]
        public int? MaxLength ;
        //
        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Returns_Stringinternal
        [PreserveCase]
        public string YomiOf ;
    }
}
