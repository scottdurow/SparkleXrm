// LocalisedContentLoader.cs
//

using jQueryApi;
using System;
using System.Collections.Generic;
using Xrm;

namespace SparkleXrm
{
    public static class LocalisedContentLoader
    {
        // The supported locales must be added to this list, otherwise the fallback locale is used
        public static List<int> SupportedLCIDs = new List<int>(1033);
        public static int FallBackLCID = 1033;

        /// <summary>
        /// Load the content by inserting the lcid after the filename, before the extension
        /// E.g. someResource.html is replaced with someResource_1033.js
        /// Ideally this url should be relative so to benefit from caching without having
        /// to explicitly specify the cache key
        /// </summary>
        /// <param name="webresourceFileName"></param>
        /// <param name="lcid"></param>
        /// <param name="callback"></param>
        public static void LoadContent(string webresourceFileName, int lcid, Action callback)
        {

            // Check the locale is in the supported locales
            if ((!SupportedLCIDs.Contains(lcid)) || (lcid == FallBackLCID))
            {
                callback();
                return;
            }

            int pos = webresourceFileName.LastIndexOf('.');
            string resourceFileName = Page.Context.GetClientUrl() + "/" + PageEx.GetCacheKey() + "WebResources/" + webresourceFileName.Substr(0, pos) + "_" + lcid.ToString() + webresourceFileName.Substr(pos);

            jQueryAjaxOptions options = new jQueryAjaxOptions();
            options.Type = "GET";
            options.Url = resourceFileName;
            options.DataType = "script";
            options.Cache = true;
            options.Success = delegate(object data, string textStatus, jQueryXmlHttpRequest request)
            {
                callback();
            };
            options.Error = delegate(jQueryXmlHttpRequest request, string textStatus, Exception error)
            {
                Script.Alert(String.Format("Could not load resource file '{0}'. Please contact your system adminsitrator.\n\n{1}:{2}", resourceFileName, textStatus, error.Message));
                callback();
            };
            jQuery.Ajax(options);


        }

    }
}
