// EnumAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the data for an attribute that provides options.
    //[DataContract(Name = "EnumAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    //[KnownType(typeof(EntityNameAttributeMetadata))]
    //[KnownType(typeof(PicklistAttributeMetadata))]
    //[KnownType(typeof(StateAttributeMetadata))]
    //[KnownType(typeof(StatusAttributeMetadata))]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class EnumAttributeMetadata : AttributeMetadata
    {
       

        // Summary:
        //     Gets or sets the default form value for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The default form value for the attribute..

        public int? DefaultFormValue;
        //
        // Summary:
        //     Gets or sets the available options for the attribute.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.OptionSetMetadataThe available options for
        //     the attribute.

        public OptionSetMetadata OptionSet;
    }
}
