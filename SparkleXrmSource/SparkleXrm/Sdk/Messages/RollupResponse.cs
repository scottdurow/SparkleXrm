using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RollupResponse : OrganizationResponse
    {
        public EntityCollection EntityCollection;

        public RollupResponse(XmlNode response)
        {
            XmlNode results = XmlHelper.SelectSingleNode(response, "Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");

                if (XmlHelper.GetNodeTextValue(key) == "EntityCollection")
                {
                    XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                    this.EntityCollection = EntityCollection.DeSerialise(value);
                }
            }
        }

    }
}
