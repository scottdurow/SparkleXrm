// NotifyEventArgs.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class DataLoadedNotifyEventArgs
    {
        public int From;
        public int To;
        public string ErrorMessage;
    }
}
