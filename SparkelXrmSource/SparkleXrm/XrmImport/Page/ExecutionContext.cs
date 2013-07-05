using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xrm
{
    [Imported]
    public class ExecutionContext
    {
        public Context GetContext()
        {
            return null;
        }

        public int GetDepth()
        {
            return -1;
        }

        public SaveEventArgs GetEventArgs()
        {
            return null;
        }

        public XrmAttribute GetEventSource()
        {
            return null;
        }

       
    }
}
