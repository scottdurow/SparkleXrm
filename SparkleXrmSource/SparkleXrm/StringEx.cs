
using System.Runtime.CompilerServices;
namespace Xrm
{
    [ScriptNamespace("SparkleXrm")]
    public static class StringEx
    {
        public static bool IN(string value,string[] values)
        {
            if (value != null)
            {
                foreach (string val in values)
                {
                    if (value == val)
                        return true;
                }
            }
            return false;
        }
    }
}
