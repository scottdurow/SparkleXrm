// Page.cs
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Html;

namespace Xrm
{
    public class PageEx
    {
        public static string GetCacheKey()
        {
            // Before UR8 we didn't have this constant, so we can't benefit from caching unless TODO: exract from url
            
            string cacheKey = (string)Script.Literal("WEB_RESOURCE_ORG_VERSION_NUMBER");
            if ((string)Script.Literal("typeof({0})",cacheKey) != "undefined")
                return cacheKey + "/";
            else
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

        
    }
}
