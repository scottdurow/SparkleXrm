// FetchXmlToQueryExpressionResponse.cs
//

using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class FetchXmlToQueryExpressionResponse : OrganizationResponse
    {
        public FetchXmlToQueryExpressionResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                if (XmlHelper.GetNodeTextValue(key) == "Query")
                {
                    XmlNode queryNode = XmlHelper.SelectSingleNode(nameValuePair, "value");
                   

                    // strip <c:value xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic" i:type="a:QueryExpression" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
                    string queryXml = XmlHelper.SerialiseNode(queryNode).Substr(165);
                    // strip ending  </c:value>
                    queryXml = queryXml.Substr(0, queryXml.Length - 10);
                    Query = queryXml;
                }

            }
        }
        public string Query;
    }
}
