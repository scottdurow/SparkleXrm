using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum ClientType
    {

        [ScriptName("Web")]
        Web,

        [ScriptName("Outlook")]
        Outlook,

        [ScriptName("Mobile")]
        Mobile

    }

}
