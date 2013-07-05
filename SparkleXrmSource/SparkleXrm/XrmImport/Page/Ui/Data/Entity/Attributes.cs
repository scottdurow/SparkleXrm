using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class Attributes
    {
        public void ForEach(GetAttributeHandler function)
        {
        }

        public XrmAttribute Get(string name)
        {
            return null;
        }

        public XrmAttribute Get(int position)
        {
            return null;
        }

        public XrmAttribute[] Get(GetAttributeHandler function)
        {
            return null;
        }

        public int GetLength()
        {
            return -1;
        }
    }
}
