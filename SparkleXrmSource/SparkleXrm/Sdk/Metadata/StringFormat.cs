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
        [PreserveCase]
        Email = 0,
        //
        // Summary:
        //     Specifies to display the string as text. Value = 1.
        //[EnumMember(Value = "Text")]
        [PreserveCase]
        Text = 1,
        //
        // Summary:
        //     Specifies to display the string as a text area. Value = 2.
        //[EnumMember(Value = "TextArea")]
        [PreserveCase]
        TextArea = 2,
        //
        // Summary:
        //     Specifies to display the string as a URL. Value = 3.
        //[EnumMember(Value = "Url")]
        [PreserveCase]
        Url = 3,
        //
        // Summary:
        //     Specifies to display the string as a ticker symbol. Value = 4.
        //[EnumMember(Value = "TickerSymbol")]
        [PreserveCase]
        TickerSymbol = 4,
        //
        // Summary:
        //     Specifies to display the string as a phonetic guide. Value = 5.
        //[EnumMember(Value = "PhoneticGuide")]
        [PreserveCase]
        PhoneticGuide = 5,
        //
        // Summary:
        //     Specifies to display the string as a version number. Value = 6.
        //[EnumMember(Value = "VersionNumber")]
        [PreserveCase]
        VersionNumber = 6,
        //[EnumMember(Value = "Phone")]
        [PreserveCase]
        Phone = 7,
    }
}
