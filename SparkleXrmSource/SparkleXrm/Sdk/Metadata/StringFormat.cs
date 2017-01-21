// StringFormat.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    [NamedValues]
    [ScriptNamespace("SparkleXrm.Sdk.Metadata")]
    public enum StringFormat
    {
        // Summary:
        //     Specifies to display the string as an e-mail. Value = 0.
        //[EnumMember(Value = "Email")]
        [ScriptName("Email")]
        Email = 0,
        //
        // Summary:
        //     Specifies to display the string as text. Value = 1.
        //[EnumMember(Value = "Text")]
        [ScriptName("Text")]
        Text = 1,
        //
        // Summary:
        //     Specifies to display the string as a text area. Value = 2.
        //[EnumMember(Value = "TextArea")]
        [ScriptName("TextArea")]
        TextArea = 2,
        //
        // Summary:
        //     Specifies to display the string as a URL. Value = 3.
        //[EnumMember(Value = "Url")]
        [ScriptName("Url")]
        Url = 3,
        //
        // Summary:
        //     Specifies to display the string as a ticker symbol. Value = 4.
        //[EnumMember(Value = "TickerSymbol")]
        [ScriptName("TickerSymbol")]
        TickerSymbol = 4,
        //
        // Summary:
        //     Specifies to display the string as a phonetic guide. Value = 5.
        //[EnumMember(Value = "PhoneticGuide")]
        [ScriptName("PhoneticGuide")]
        PhoneticGuide = 5,
        //
        // Summary:
        //     Specifies to display the string as a version number. Value = 6.
        //[EnumMember(Value = "VersionNumber")]
        [ScriptName("VersionNumber")]
        VersionNumber = 6,
        //[EnumMember(Value = "Phone")]
        [ScriptName("Phone")]
        Phone = 7,
    }
}
