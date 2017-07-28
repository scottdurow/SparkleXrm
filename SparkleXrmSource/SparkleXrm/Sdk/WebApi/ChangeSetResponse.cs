// WebAPIBatchResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.SDK
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class ChangeSetResponse
    {
        public string ChangesetId;
        public Dictionary<string, object> response;
    }
}
