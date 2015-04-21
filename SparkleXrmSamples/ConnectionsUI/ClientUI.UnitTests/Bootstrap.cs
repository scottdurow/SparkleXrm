// Bootstrap.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ClientUI.UnitTests
{
    public static class Bootstrap
    {
        [PreserveCase]
        public static void RunTests()
        {
            //QUnit.Log(delegate(LogInfo details)
            //{
            //    if (details.Result)
            //        return;

            //    string loc = details.Module + ": " + details.Name + ": ";
            //    string output = "FAILED: " + loc + (details.Message!=null ? details.Message + ", " : "");
            //    Script.Literal("console.log({0})",output);

            //});


            ConnectionViewModelTests.Run();
        }
    }
}
