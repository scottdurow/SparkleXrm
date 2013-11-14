using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum AttributeSubmitMode
    {

        /// <summary>
        /// The data is always sent with a save
        /// </summary>
        [ScriptName("always")]
        Always,

        /// <summary>
        /// The data is never sent with a save. When this is used the field(s) in the form for this attribute cannot be edited
        /// </summary>
        [ScriptName("never")]
        Never,

        /// <summary>
        /// Default behavior. The data is sent with the save when it has changed
        /// </summary>
        [ScriptName("dirty")]
        Dirty

    }

}
