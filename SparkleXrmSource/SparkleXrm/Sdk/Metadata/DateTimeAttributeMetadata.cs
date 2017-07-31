// DateTimeAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute of type DateTime.
    //[DataContract(Name = "DateTimeAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class DateTimeAttributeMetadata : AttributeMetadata
    {


        // Summary:
        //     Gets or sets the date/time display format.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.DateTimeFormat> The date/time
        //     display format.
        [PreserveCase]
        public DateTimeFormat Format;
        //
        // Summary:
        //     Gets or sets the input method editor (IME) mode for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.ImeMode> The input method
        //     editor mode for the attribute.

        //public ImeMode? ImeMode;
        //
        // Summary:
        //     Gets the maximum supported value for this attribute.
        //
        // Returns:
        //     Type: Returns_DateTimeThe maximum supported date for this attribute.
        [PreserveCase]
        public static DateTime MaxSupportedValue;
        //
        // Summary:
        //     Gets the minimum supported value for this attribute.
        //
        // Returns:
        //     Type: Returns_DateTimeThe minimum supported date for this attribute.
        [PreserveCase]
        public static DateTime MinSupportedValue;
    }
}
