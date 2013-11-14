using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum AttributeType
    {

        [ScriptName("boolean")]
        Boolean,

        [ScriptName("datetime")]
        DateTime,

        [ScriptName("decimal")]
        Decimal,

        [ScriptName("double")]
        Double,

        [ScriptName("integer")]
        Integer,

        [ScriptName("lookup")]
        Lookup,

        [ScriptName("memo")]
        Memo,

        [ScriptName("money")]
        Money,

        [ScriptName("optionset")]
        OptionSet,

        [ScriptName("string")]
        String
 

    }

}
