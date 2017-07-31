// BooleanOptionSetMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute of type Boolean.
    //[DataContract(Name = "BooleanOptionSetMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class BooleanOptionSetMetadata : OptionSetMetadataBase
    {

        // Summary:
        //     Gets or sets option displayed when the attribute is false.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.OptionMetadataThe option metadata for the
        //     false option..
        [PreserveCase]
        public OptionMetadata FalseOption;
        //
        // Summary:
        //     Gets or sets option displayed when the attribute is true.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.OptionMetadataThe option metadata for the
        //     true option..
        [PreserveCase]
        public OptionMetadata TrueOption;
    }
}
