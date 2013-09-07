// MetadataBase.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class MetadataBase
    {
       
        //
        // Summary:
        //     Gets whether the item of metadata has changed.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if metadata item has changed
        //     since the last query; otherwise, false.
   
        public bool? HasChanged ;
        //
        // Summary:
        //     Gets or sets a unique identifier for the metadata item.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Guid> The unique identifier for the metadata
        //     item.
   
        public Guid MetadataId ;
    }
}
