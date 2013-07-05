// Page.cs
//

using System;
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

        
    }
}
