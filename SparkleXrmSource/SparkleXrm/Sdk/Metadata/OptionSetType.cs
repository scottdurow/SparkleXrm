// OptionSetType.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Indicates the type of option set.
    //[DataContract(Name = "OptionSetType", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [NamedValues]
    [ScriptNamespace("SparkleXrm.Sdk.Metadata")]
    public enum OptionSetType
    {
        // Summary:
        //     The option set provides a list of options. Value = 0.
        [ScriptName("Picklist")]
        Picklist = 0,
        //
        // Summary:
        //     The option set represents state options for a Microsoft.Xrm.Sdk.Metadata.StateAttributeMetadata
        //     attribute. Value = 1.
        [ScriptName("State")]
        State = 1,
        //
        // Summary:
        //     The option set represents status options for a Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata
        //     attribute. Value = 2.
        [ScriptName("Status")]
        Status = 2,
        //
        // Summary:
        //     The option set provides two options for a Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata
        //     attribute. Value = 3.
        [ScriptName("Boolean")]
        Boolean_ = 3
    }
}
