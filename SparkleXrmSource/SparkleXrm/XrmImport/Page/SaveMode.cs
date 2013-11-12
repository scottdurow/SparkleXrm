using System;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [IgnoreNamespace]
    [Imported]
    [NumericValues]
    public enum SaveMode
    {

        /// <summary>
        /// Supported by all entities
        /// </summary>
        Save = 1,

        /// <summary>
        /// Supported by all entities
        /// </summary>
        SaveAndClose = 2,

        /// <summary>
        /// Supported by all entities
        /// </summary>
        SaveAndNew = 59,

        /// <summary>
        /// Supported by all entities
        /// </summary>
        AutoSave = 70,

        /// <summary>
        /// Supported by Activities only
        /// </summary>
        SaveAsCompleted = 58,

        /// <summary>
        /// Supported by all entities
        /// </summary>
        Deactivate = 5,

        /// <summary>
        /// Supported by all entities
        /// </summary>
        Reactivate = 6,

        /// <summary>
        /// supported by User or Team owned entities
        /// </summary>
        Assign = 47,

        /// <summary>
        /// Supported by the Email entity only
        /// </summary>
        Send = 7,

        /// <summary>
        /// Supported by the Lead entity only
        /// </summary>
        Qualify = 16,

        /// <summary>
        /// Supported by the Lead entity only
        /// </summary>
        Disqualify = 16

    }
}
