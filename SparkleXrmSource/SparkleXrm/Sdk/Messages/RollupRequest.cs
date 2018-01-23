using System;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{
    /// <summary>
    /// Create rollup query
    /// </summary>
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RollupRequest : OrganizationRequest
    {
        public string Query;
        public string TargetLogicalName;
        public string TargetGuid;
        public string RollupType; // None, Related or Extended

        public string Serialise()
        {
            return String.Format("<request i:type=\"b:RollupRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
            + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
            + "        <a:KeyValuePairOfstringanyType>"
            + "            <c:key>Target</c:key>"
            + "            <c:value i:type=\"a:EntityReference\">"
            + "              <a:Id>" + TargetGuid + "</a:Id>"
            + "              <a:LogicalName>" + TargetLogicalName + "</a:LogicalName>"
            + "              <a:Name i:nil=\"true\" />"
            + "            </c:value>"
            + "          </a:KeyValuePairOfstringanyType>"
            + "        <a:KeyValuePairOfstringanyType>"
            +           Query
            + "        </a:KeyValuePairOfstringanyType>"
            + "        <a:KeyValuePairOfstringanyType>"
            + "            <c:key>RollupType</c:key>"
            + "            <c:value i:type=\"b:RollupType\">" + RollupType + "</c:value>"
            + "          </a:KeyValuePairOfstringanyType>"
            + "        </a:Parameters>"
            + "        <a:RequestId i:nil=\"true\" />"
            + "        <a:RequestName>Rollup</a:RequestName>"
            + "      </request>");
        }
    }
}
