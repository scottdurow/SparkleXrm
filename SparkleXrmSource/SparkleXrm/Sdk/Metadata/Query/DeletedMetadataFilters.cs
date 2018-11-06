// DeletedMetadataFilters.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.Sdk.Metadata.Query
{
    // Summary:
    //     An enumeration that specifies the type of deleted metadata to retrieve.
    //[DataContract(Name = "DeletedMetadataFilters", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    //[Flags]
   
    [NamedValues]
    public enum DeletedMetadataFilters
    {
        // Summary:
        //     The value used if not set. Equals Microsoft.Xrm.Sdk.Metadata.Query.DeletedMetadataFilters.Entity
        [ScriptName("Default")]
        Default_ = 1,
        //
        // Summary:
        //     Deleted Entity metadata. Value = 1.
        [PreserveCase]
        Entity = 1,
        //
        // Summary:
        //     Deleted Attribute metadata. Value = 2.
        [PreserveCase]
        Attribute = 2,
        //
        // Summary:
        //     Deleted Relationship metadata. Value = 4.
        [PreserveCase]
        Relationship = 4,
        //
        // Summary:
        //     Deleted Label metadata. Value = 8.
        [PreserveCase]
        Label = 8,
        //
        // Summary:
        //     Deleted OptionSet metadata. Value = 16.
        [PreserveCase]
        OptionSet = 16,
        //
        // Summary:
        //     All deleted metadata. Value = 31.
        [PreserveCase]
        All = 31,
    }
}
