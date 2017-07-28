// WebAPIRequiredEntityMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class WebApiEntityMetadata
    {
        public string LogicalName;
        public string EntitySetName;
        public string PrimaryAttributeLogicalName;
    }
}
