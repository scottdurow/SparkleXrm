// MemoAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for the attribute type Memo.
    //[DataContract(Name = "MemoAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class MemoAttributeMetadata : AttributeMetadata
    {


        // Summary:
        //     Gets or sets the format options for the memo attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.StringFormat> The format
        //     options for the memo attribute.
        [PreserveCase]
        public StringFormat Format ;
        //
        // Summary:
        //     Gets or sets the value for the input method editor mode.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.ImeMode> The value for
        //     the input method editor mode..

        //public ImeMode? ImeMode ;
        //
        // Summary:
        //     Gets or sets the maximum length for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The maximum length for the attribute.
        [PreserveCase]
        public int? MaxLength ;
    }
}
