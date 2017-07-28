// RetrieveUserPrivilegesRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{

    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveUserPrivilegesRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        public Guid UserId;

        public string Serialise()
        {
            return String.Format("<request i:type=\"b:ExecuteWorkflowRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
               + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
                 + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>UserId</c:key>"
               + "            <c:value i:type=\"e:guid\" xmlns:e=\"http://schemas.microsoft.com/2003/10/Serialization/\">" + UserId.Value + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "        </a:Parameters>"
               + "        <a:RequestId i:nil=\"true\" />"
               + "        <a:RequestName>RetrieveUserPrivileges</a:RequestName>"
               + "      </request>");
        }
        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.OperationType = OperationTypeEnum.FunctionCall;
            request.RequestName = "Microsoft.Dynamics.CRM.RetrieveUserPrivileges";
            request.BoundEntityId = UserId;
            request.BoundEntityLogicalName = "systemuser";
            return request;

        }
    }
}
