using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum SaveOption
    {

        /// <summary>
        /// This is the equivalent of using the Save and Close command
        /// </summary>
        [ScriptName("saveandclose")]
        SaveAndClose,

        /// <summary>
        /// This is the equivalent of the using the Save and New command
        /// </summary>
        [ScriptName("saveandnew")]
        SaveAndNew

    }

}
