using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xrm
{
    [Imported]
    public class SaveEventArgs
    {
        public int GetSaveMode()
        {
            return -1;
        }

        public bool IsDefaultPrevented()
        {
            return false;
        }

        public void PreventDefault()
        {
        }
    }
}
