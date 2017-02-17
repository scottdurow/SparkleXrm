// IntegerFormat.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Describes the formatting of an integer attribute.
    //[DataContract(Name = "IntegerFormat", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [NamedValues]
    [ScriptNamespace("SparkleXrm.Sdk.Metadata")]
    public enum IntegerFormat
    {
        // Summary:
        //     Specifies to display an edit field for an integer. Value = 0.
        //[EnumMember(Value = "None")]
        [ScriptName("None")]
        None = 0,
        //
        // Summary:
        //     Specifies to display the integer as a drop down list of durations. Value
        //     = 1.
        //[EnumMember(Value = "Duration")]
         [ScriptName("Duration")]
        Duration = 1,
        //
        // Summary:
        //     Specifies to display the integer as a drop down list of time zones. Value
        //     = 2.
        //[EnumMember(Value = "TimeZone")]
         [ScriptName("TimeZone")]
        TimeZone = 2,
        //
        // Summary:
        //     Specifies the display the integer as a drop down list of installed languages.
        //     Value = 3.
        //[EnumMember(Value = "Language")]
         [ScriptName("Language")]
        Language = 3,
        //
        // Summary:
        //     Specifies a locale. Value = 4.
        //[EnumMember(Value = "Locale")]
         [ScriptName("Locale")]
        Locale = 4,
    }
}
