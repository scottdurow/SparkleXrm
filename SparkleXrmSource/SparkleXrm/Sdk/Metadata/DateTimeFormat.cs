// DateTimeFormat.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Describes the formatting of a Microsoft.Xrm.Sdk.Metadata.DateTimeAttributeMetadata
    //     attribute.
    //[DataContract(Name = "DateTimeFormat", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [NamedValues]
    [ScriptNamespace("SparkleXrm.Sdk.Metadata")]
    public enum DateTimeFormat
    {
        // Summary:
        //     Display the date only. Value = 0.
        //[EnumMember(Value = "DateOnly")]
        [PreserveCase]
        DateOnly = 0,
        //
        // Summary:
        //     Display the date and time. Value = 1.
        //[EnumMember(Value = "DateAndTime")]
        [PreserveCase]
        DateAndTime = 1,
    }
}
