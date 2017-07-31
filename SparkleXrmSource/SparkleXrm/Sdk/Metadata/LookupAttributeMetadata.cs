// LookupAttributeMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Contains the metadata for an attribute of type lookup.
    //[DataContract(Name = "LookupAttributeMetadata", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class LookupAttributeMetadata : AttributeMetadata
    {

        // Summary:
        //     Gets or sets the target entity types for the lookup.
        //
        // Returns:
        //     Type: Returns_String[] The array of target entity types for the lookup.
        [PreserveCase]
        public string[] Targets;
    }
}
