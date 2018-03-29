// Page.cs
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [ScriptNamespace("SparkleXrm.Xrm")]
    public static class PageEx
    {
        static PageEx()
        {
            PageEx.MajorVersion = 2011;
            if (Script.Literal("typeof(window.APPLICATION_VERSION)") != "undefined")
            {
                string applicationVersion = (string)Script.Literal("window.APPLICATION_VERSION");
                if (applicationVersion != "5.0")
                    PageEx.MajorVersion = 2013;
            }

        }
        public static string GetCacheKey()
        {

            // Try the Web UI Unsupported constant
            string cacheKey = (string)Script.Literal("window.WEB_RESOURCE_ORG_VERSION_NUMBER");
            if (!Script.IsNullOrUndefined(cacheKey))
            {
                // Web UI cache key
                return cacheKey + @"/";
            }

            // Try the url match
            RegularExpression regex = new System.RegularExpression(@"(?<=\/)%7b[0-9]*%7d(?=\/webresources)", "g");
            string[] match = regex.Exec(Window.Location.Href);
            if (match != null && match.Length > 0)
            {
                // Cache key from the url
                return match[0] + @"/";
            }

            return "";
          
        }
        public static Dictionary<string, string> GetWebResourceData()
        {
           
            string queryString = Window.Location.Search;
            if (queryString!=null && queryString!="")
            {
                string[] parameters = queryString.Substr(1).Split("&");
                foreach (string param in parameters)
                {
                    if (param.ToLowerCase().StartsWith("data="))
                    {
                        string[] dataParam = param.Replace("+"," ").Split("=");
                        return ParseDataParameter(dataParam[1]);
                    }
                }
            }
            return new Dictionary<string, string>();
        }

        private static Dictionary<string,string> ParseDataParameter(string data) 
        {
            Dictionary<string, string> nameValuePairs = new Dictionary<string,string>();
            string[] values = ((string)Script.Literal("decodeURIComponent({0})",data)).Split("&");
            foreach (string value in values)
            {
                string[] nameValuePair = value.Split("=");
                nameValuePairs[nameValuePair[0]] = nameValuePair[1];
            }

            return nameValuePairs;
        }

        public static int MajorVersion;
        
    }
}
