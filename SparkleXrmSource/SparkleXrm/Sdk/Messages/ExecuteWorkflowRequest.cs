// ExecuteWorkflowRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk.Messages
{
    /// <summary>
    /// Although depricated - this request is the only way to query the MultiEntitySearchEntities records
    /// </summary>
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class ExecuteWorkflowRequest : OrganizationRequest
    {
        [PreserveCase]
        public string EntityId;
        [PreserveCase]
        public string WorkflowId;

        public string Serialise()
        {
            return String.Format("<request i:type=\"b:ExecuteWorkflowRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
               + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
                 + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>EntityId</c:key>"
               + "            <c:value i:type=\"e:guid\" xmlns:e=\"http://schemas.microsoft.com/2003/10/Serialization/\">" + EntityId + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
                   + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>WorkflowId</c:key>"
               + "            <c:value i:type=\"e:guid\" xmlns:e=\"http://schemas.microsoft.com/2003/10/Serialization/\">" + WorkflowId + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "        </a:Parameters>"
               + "        <a:RequestId i:nil=\"true\" />"
               + "        <a:RequestName>ExecuteWorkflow</a:RequestName>"
               + "      </request>");
        }
    }
}
