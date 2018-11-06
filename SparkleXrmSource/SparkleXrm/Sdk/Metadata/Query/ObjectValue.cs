// ObjectValue.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata.Query
{
    // This type is used for the serialisation of Metdata Queries into JSON where you cannot pass a Primitve type to object value field.
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class ObjectValue
    {
        [PreserveCase]
        public object Value;
        [PreserveCase]
        public string Type;
    }
}
