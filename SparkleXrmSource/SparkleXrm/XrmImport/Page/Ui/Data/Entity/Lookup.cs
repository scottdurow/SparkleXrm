using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    [ScriptName("Object")]
    [ScriptNamespace("")]
    [IgnoreNamespace]
    public class Lookup
    {
        public string Name;
        public string Id;
        public string EntityType;
        [ScriptName("typename")]
        public string TypeName;
    }
}
