// FetchXmlToQueryExpressionRequest.cs
//


namespace Xrm.Sdk.Messages
{
    public class FetchXmlToQueryExpressionRequest : OrganizationRequest
    {
        public string FetchXml;
        public string Serialise()
        {
            string requestXml = "";
        
            requestXml += "      <request i:type=\"b:FetchXmlToQueryExpressionRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">";
            requestXml += "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">";
            requestXml += "          <a:KeyValuePairOfstringanyType>";
            requestXml += "            <c:key>FetchXml</c:key>";
            requestXml += "            <c:value i:type=\"d:string\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\">{0}</c:value>";
            requestXml += "          </a:KeyValuePairOfstringanyType>";
            requestXml += "        </a:Parameters>";
            requestXml += "        <a:RequestId i:nil=\"true\" />";
            requestXml += "        <a:RequestName>FetchXmlToQueryExpression</a:RequestName>";
            requestXml += "      </request>";
  
            return string.Format(requestXml,XmlHelper.Encode(FetchXml));

        }
    }
}
