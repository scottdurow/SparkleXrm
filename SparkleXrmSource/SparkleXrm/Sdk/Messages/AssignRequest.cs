// AssignRequest.cs
//


using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class AssignRequest : OrganizationRequest
    {
        public EntityReference Target;
        public EntityReference Assignee;

        public string Serialise()
        {
            return "<request i:type=\"c:AssignRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:c=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
               + "        <a:Parameters xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
                 + "          <a:KeyValuePairOfstringanyType>"
               + "            <b:key>Target</b:key>"
               + Attribute.SerialiseValue(Target,null) 
               + "          </a:KeyValuePairOfstringanyType>"
                   + "          <a:KeyValuePairOfstringanyType>"
               + "            <b:key>Assignee</b:key>"
                + Attribute.SerialiseValue(Assignee, null) 
               + "          </a:KeyValuePairOfstringanyType>"
               + "        </a:Parameters>"
               + "        <a:RequestId i:nil=\"true\" />"
               + "        <a:RequestName>Assign</a:RequestName>"
               + "      </request>";
        }
    }
}
