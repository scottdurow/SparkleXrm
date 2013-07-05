using System.Runtime.CompilerServices;

namespace Xrm.XrmImport.Ribbon
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class CommandEntityReference
    {
        [ScriptName("Id")]
        public string Id;
        [ScriptName("Name")]
        public string Name;
        [ScriptName("TypeName")]
        public string TypeName;
        [ScriptName("TypeCode")]
        public int TypeCode;

    }
}
