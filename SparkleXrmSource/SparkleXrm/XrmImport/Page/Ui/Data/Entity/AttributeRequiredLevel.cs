using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum AttributeRequiredLevel
    {

        [ScriptName("none")]
        None,

        [ScriptName("required")]
        Required,

        [ScriptName("recommended")]
        Recommended

    }

}
