using System.Runtime.CompilerServices;

namespace Xrm
{

    [IgnoreNamespace]
    [Imported]
    [NamedValues]
    public enum FormNotificationLevel
    {

        /// <summary>
        /// Notification will use the system info icon
        /// </summary>
        [ScriptName("INFO")]
        Information,

        /// <summary>
        /// Notification will use the system warning icon
        /// </summary>
        [ScriptName("WARNING")]
        Warning,

        /// <summary>
        /// Notification will use the system error icon
        /// </summary>
        [ScriptName("ERROR")]
        Error

    }

}