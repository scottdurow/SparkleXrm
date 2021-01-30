using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin
{
    // Test Custom API plugin registration
    [CrmPluginRegistration("spkl_CustomApiTest")]
    public class CustomApiPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
           
        }
    }
}
