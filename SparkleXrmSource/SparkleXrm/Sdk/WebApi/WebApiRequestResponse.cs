// RequestResponse.cs
//

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class WebApiRequestResponse
    {
        public string Response;
        public string Headers;
        public string LogicalName;
        private Dictionary<string, string> _headers;

        public WebApiRequestResponse(XmlHttpRequest request, string logicalName)
        {
            Response = request.ResponseText;
            Headers = request.GetAllResponseHeaders();
            LogicalName = logicalName;

        }

        public string GetHeader(string key)
        {
            if (_headers == null)
            {
                _headers = new Dictionary<string, string>();
                string[] pairs = Headers.Split("\u000d\u000a");
                foreach (string pair in pairs)
                {
                    string[] tokens = pair.Split("\u003a\u0020");

                    if (tokens.Length == 2)
                    {
                        _headers[tokens[0]] = tokens[1];
                    }
                }
            }

            return _headers[key];

        }
    }
}
