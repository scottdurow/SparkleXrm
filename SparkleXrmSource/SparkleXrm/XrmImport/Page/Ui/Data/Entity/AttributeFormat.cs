using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum AttributeFormat
    {

        [ScriptName("date")]
        Date,

        [ScriptName("datetime")]
        DateTime,

        [ScriptName("duration")]
        Duration,

        [ScriptName("email")]
        Email,

        [ScriptName("language")]
        Language,

        [ScriptName("none")]
        None,

        [ScriptName("phone")]
        Phone,

        [ScriptName("text")]
        Text,

        [ScriptName("textarea")]
        TextArea,

        [ScriptName("tickersymbol")]
        TickerSymbol,

        [ScriptName("timezone")]
        Timezone,

        [ScriptName("url")]
        Url

    }

}
