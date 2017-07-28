// WebAPIBatchResponse.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.SDK
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class BatchResponse
    {
        public string BatchId;
        public int HTTPResponseCode;
        public Dictionary<string, object> response;
        public List<ChangeSetResponse> changesets;
    }
}
