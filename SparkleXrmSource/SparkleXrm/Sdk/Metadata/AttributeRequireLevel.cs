// AttributeRequireLevel.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Describes the requirement level for an attribute.
   // [DataContract(Name = "AttributeRequiredLevel", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [NamedValues]
    public enum AttributeRequiredLevel
    {
        // Summary:
        //     No requirements are specified. Value = 0.
        [ScriptName("None")]
        None = 0,
        //
        // Summary:
        //     The attribute is required to have a value. Value = 1.
       [ScriptName("SystemRequired")]
        SystemRequired = 1,
        //
        // Summary:
        //     The attribute is required to have a value. Value = 2.
      [ScriptName("ApplicationRequired")]
        ApplicationRequired = 2,
        //
        // Summary:
        //     It is recommended that the attribute has a value. Value = 3.
       [ScriptName("Recommended")]
        Recommended = 3,
    }
}
