// WebAPIError.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm.SDK
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class WebApiErrorResponse
    {
        [PreserveCase]
        public string Message;
        public WebApiError error;
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class WebApiError
    {
        public string code;
        public string message;
        public string stacktrace;
        public string type;
        public WebApiError innererror;
    }
}
