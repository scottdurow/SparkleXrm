// BooleanAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute type Boolean.
    //[DataContract(Name = "BooleanAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class BooleanAttributeMetadata : AttributeMetadata
    {

        // Summary:
        //     Gets or sets the default value for a Boolean option set.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean> The default value for a Boolean option
        //     set.
        [PreserveCase]
        public bool? DefaultValue;
        //
        // Summary:
        //     Gets or sets the options for a boolean attribute.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.BooleanOptionSetMetadata The definition
        //     of the options.
        [PreserveCase]
        public BooleanOptionSetMetadata OptionSet;
    }
}
