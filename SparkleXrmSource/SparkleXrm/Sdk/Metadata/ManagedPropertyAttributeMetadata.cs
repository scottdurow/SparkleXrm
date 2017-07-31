// ManagedPropertyAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     internal
   // [DataContract(Name = "ManagedPropertyAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class ManagedPropertyAttributeMetadata : AttributeMetadata
    {


        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Returns_Stringinternal
        [PreserveCase]
        public string ManagedPropertyLogicalName;
        //
        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Returns_Stringinternal
        [PreserveCase]
        public string ParentAttributeName;
        //
        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32>internal
        [PreserveCase]
        public int? ParentComponentType;
        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.AttributeTypeCodeinternal
        [PreserveCase]
        public AttributeTypeCode ValueAttributeTypeCode;
    }
}
