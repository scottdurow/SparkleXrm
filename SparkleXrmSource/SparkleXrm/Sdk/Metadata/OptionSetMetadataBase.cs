// OptionSetMetadataBase.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains data that defines a set of options.
    //[DataContract(Name = "OptionSetMetadataBase", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    //[KnownType(typeof(BooleanOptionSetMetadata))]
    //[KnownType(typeof(OptionSetMetadata))]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OptionSetMetadataBase : MetadataBase
    {

        // Summary:
        //     Gets or sets a description for the option set.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.LabelThe description for the option set.
        [PreserveCase]
        public Label Description;
        //
        // Summary:
        //     Gets or sets a display name for a global option set.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.LabelThe display name for a global option set.
        [PreserveCase]
        public Label DisplayName;
        //
        // Summary:
        //     Gets or sets a property that determines whether the option set is customizable.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the option set is customizable.
        [PreserveCase]
        public bool? IsCustomizable;
        //
        // Summary:
        //     Gets or sets whether the option set is a custom option set.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the option set is a custom
        //     option set; otherwise, false.
        [PreserveCase]
        public bool? IsCustomOptionSet ;
        //
        // Summary:
        //     Gets or sets whether the option set is a global option set.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the option set is a global
        //     option set; otherwise, false.
        [PreserveCase]
        public bool? IsGlobal ;
        //
        // Summary:
        //     Gets or sets whether the option set is part of a managed solution.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the option set is part of
        //     a managed solution; otherwise, false.
        [PreserveCase]
        public bool? IsManaged;
        //
        // Summary:
        //     Gets or sets the name of a global option set.
        //
        // Returns:
        //     Type: Returns_String The name of a global option set.
        [PreserveCase]
        public string Name ;
        //
        // Summary:
        //     Gets or sets the type of option set.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.OptionSetType> The type
        //     of option set.
        [PreserveCase]
        public OptionSetType OptionSetType ;
    }
}
