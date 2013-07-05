// LocalisedContentLoader.cs
//

using jQueryApi;
using System;
using System.Collections.Generic;

namespace SparkleXrm
{
    public class LocalisedContentLoader
    {
        // The supported locales must be added to this list, otherwise the fallback locale is used
        public static List<string> SupportedLCIDs = new List<string>();
        public int FallBackLCID = 1033;

        /// <summary>
        /// Load the content by inserting the lcid after the filename, before the extension
        /// E.g. someResource.html is replaced with someResource_1033.js
        /// Ideally this url should be relative so to benefit from caching without having
        /// to explicitly specify the cache key
        /// </summary>
        /// <param name="webresourceFileName"></param>
        /// <param name="lcid"></param>
        /// <param name="callback"></param>
        public void LoadContent(string webresourceFileName, int lcid, Action callback)
        {
            
            // If the filename starts in '/Webresources/' we must prefix with the cache key
            // TODO 
            // Xrm.Sparkle.Page.GetCacheKey()

            // Check the locale is in the supported locales
            if (!SupportedLCIDs.Contains(lcid))
                lcid = FallBackLCID;

            int pos = webresourceFileName.LastIndexOf('.');
            string resourceFileName = webresourceFileName.Substr(0,pos-1) + "_" + lcid.ToString() + webresourceFileName.Substr(pos);

            jQueryAjaxOptions options = new jQueryAjaxOptions();
            options.Type = "GET";
            options.Url = resourceFileName;
            options.DataType = "script";
            options.Success = delegate(object data, string textStatus, jQueryXmlHttpRequest request)
            {
                callback();
            };
            options.Error = delegate(jQueryXmlHttpRequest request, string textStatus, Exception error)
            {
                Script.Alert(String.Format("Could not load resource file '{0}'. Please contact your system adminsitrator.",resourceFileName));
            };
            jQuery.Ajax(options);
        }

    }
}
