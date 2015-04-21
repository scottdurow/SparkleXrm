// Bootstrap.cs
//

using QUnitApi;
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
            UnitTest1.Run();
        }
    }
}
