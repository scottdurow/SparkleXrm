using System.Runtime.CompilerServices;
using System;

namespace Xrm
{
    [Imported]
    public class Utility
    {
        public static object OpenEntityForm(string name, string id, object parameters)
        {
            return null;
        }

        public static object OpenWebResource(string webResourceName, string webResourceData, int width, int height)
        {
            return null;
        }
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptNamespace("")]
    public static class GlobalFunctions
    {
        
       
        [ScriptAlias("encodeURIComponent")]
        public static string encodeURIComponent(string values)
        {
            return null;
        }

    }
}