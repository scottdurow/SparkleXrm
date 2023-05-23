
using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;
using System.Xml;
namespace Xrm.Sdk
{
   [ScriptNamespace("SparkleXrm.Sdk")]
    public class XmlHelper
    {

        public static Dictionary _encode_map = new Dictionary("&", "&amp;", @"""", "&quot;", "<", "&lt;", ">", "&gt;", "'", "&#39;");
        public static Dictionary _decode_map = new Dictionary("&amp;", "&", "&quot;", @"""", "&lt;", "<", "&gt;", ">", "&#39;","'");
        
        public static string Encode(string value)
        {
            if (value == null)
                return value;
            return value.ReplaceRegex(new System.RegularExpression(@"([\&""<>'])","g"), replaceCallBackEncode);
            
        }
        public static string SerialiseNode(XmlNode node)
        {
            if ((string)Script.Literal("typeof({0})", node.OuterXml) == "undefined")
            {
                return (string)Script.Literal("new XMLSerializer().serializeToString({0})",node);
            }
            else
                return node.OuterXml; 
        }

        [PreserveCase]
        public static string Decode(string value)
        {
            if (value == null)
                return null;
            return value.ReplaceRegex(new System.RegularExpression(@"(&quot;|&lt;|&gt;|&amp;|&#39;)", "g"), replaceCallBackDecode);
        }
        
        public static string replaceCallBackEncode(string item)
        {
            return (string) XmlHelper._encode_map[item];
        }
        public static string replaceCallBackDecode(string item)
        {
            return (string) XmlHelper._decode_map[item];
        }

        public static string SelectSingleNodeValue(XmlNode doc, string baseName)
        {
            XmlNode node = SelectSingleNode(doc, baseName);
            if (node != null)
                return GetNodeTextValue(node);
            else
                return null;
        }
        public static XmlNode SelectSingleNode(XmlNode doc, string baseName)
        {
            foreach (XmlNode n in doc.ChildNodes)
            {
                if (GetLocalName(n) == baseName)
                {
                    return n;
                }
                
            }
            return null;
        }
        public static string GetLocalName(XmlNode node)
        {
            // Use baseName for IE - otherwise localName
            if (node.BaseName != null)
            {
                return node.BaseName;
            }
            else
                return (string)Script.Literal("{0}.localName",node);
        }
        public static string SelectSingleNodeValueDeep(XmlNode doc, string baseName)
        {
            XmlNode node = SelectSingleNodeDeep(doc, baseName);
            if (node != null)
                return GetNodeTextValue(node);
            else
                return null;
        }
        public static XmlNode SelectSingleNodeDeep(XmlNode doc, string baseName)
        {
           
            foreach (XmlNode n in doc.ChildNodes)
            {
                if (GetLocalName(n) == baseName)
                {
                    return n;
                }

                XmlNode resultDeep = SelectSingleNodeDeep(n, baseName);
                if (resultDeep != null)
                    return resultDeep;
            }
            return null;
        }

        public static string NSResolver(string prefix)
        {
            switch (prefix)
            {
                case "s":
                    return @"http://schemas.xmlsoap.org/soap/envelope/";
                case "a":
                    return @"http://schemas.microsoft.com/xrm/2011/Contracts";
                case "i":
                    return @"http://www.w3.org/2001/XMLSchema-instance";
                case "b":
                    return @"http://schemas.datacontract.org/2004/07/System.Collections.Generic";
                case "c":
                    return @"http://schemas.microsoft.com/xrm/2011/Metadata";
                default:
                    return null;
            }
          
        }
      
        public static bool IsSelectSingleNodeUndefined(object value)
        {
            return (string)Script.Literal("typeof ({0}.selectSingleNode)", value) == "undefined";
        }

        public static bool IsXPathEvaluatorUndefined()
        {
            return (string)Script.Literal("typeof XPathEvaluator") == "undefined";
        }

        public static XmlDocument LoadXml(string xml)
        {
            // CGCHANGE - Removed this for solution checker
            if ((string)Script.Literal("typeof (ActiveXObject)") == "undefined")
            {
                DOMParser domParser = new DOMParser();
                return domParser.parseFromString(xml, "text/xml");
            }
            else
            {
                XmlDocument xmlDOM = (XmlDocument)((object)new ActiveXObject("Msxml2.DOMDocument"));
                Script.Literal("{0}.async = false", xmlDOM);
                Script.Literal("{0}.loadXML({1})", xmlDOM, xml);
                Script.Literal("{0}.setProperty('SelectionLanguage', 'XPath')", xmlDOM);

                return xmlDOM;
            }

        }

        public static XmlNode SelectSingleNodeXpath(XmlNode node, string xpath)
        {
           if (!IsSelectSingleNodeUndefined(node) || IsXPathEvaluatorUndefined())
           {
                return node.SelectSingleNode(xpath);
           }
           else
           {
               XPathEvaluator xpe = new XPathEvaluator();
               XPathResult xPathNode = xpe.Evaluate(xpath, node, NSResolver, 9, null); // FIRST_ORDERED_NODE_TYPE = 9
               return (xPathNode != null) ? xPathNode.SingleNodeValue : null;
           }
 
        }

        public static string GetNodeTextValue(XmlNode node)
        {
            if ((node != null) && (node.FirstChild != null))
            {
                return node.FirstChild.Value;
            }
            else
                return null;
        }
        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            XmlNode attribute = node.Attributes.GetNamedItem(attributeName);
            if (attribute != null)
                return attribute.Value;
            else
                return null;
        }
    }
    [Imported]
    [IgnoreNamespace]
    public class XPathEvaluator
    {
        public XPathResult Evaluate(string xpathExpression, XmlNode node, Func<string, string> nsResolver, int resultType, XPathResult result)
        {
            return null;
        }
    }

    [Imported]
    [IgnoreNamespace]
    public class XPathResult
    {
        public XmlNode SingleNodeValue;

    }

    [Imported]
    [IgnoreNamespace]
    public class DOMParser
    {
        public XmlDocument parseFromString(string xml, string type)
        {
            return null;
        }
    }
}
