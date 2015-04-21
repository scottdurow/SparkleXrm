// Class1.cs
//

using System;
using System.Collections.Generic;
using System.Html;
using Xrm;


namespace ClientHooks
{
    
    public class Class1
    {
        public static void HelloWorld()
        {
           
            string url = Page.Context.GetClientUrl();

        }
    }
}
