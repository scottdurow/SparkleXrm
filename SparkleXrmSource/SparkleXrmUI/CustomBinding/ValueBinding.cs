// ValueBinding.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.CustomBinding
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class Bindings
    {
        public object Value;
        public object Text;
    }
}
