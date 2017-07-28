// WhoAmIRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{

    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class WhoAmIRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        public string Serialise()
        {
            return String.Format("<request i:type=\"b:WhoAmIRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
              + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
              + "        </a:Parameters>"
              + "        <a:RequestId i:nil=\"true\" />"
              + "        <a:RequestName>WhoAmI</a:RequestName>"
              + "      </request>");
        }
        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.OperationType = OperationTypeEnum.FunctionCall;
            request.RequestName = "WhoAmI";
            return request;

        }

    }
}
