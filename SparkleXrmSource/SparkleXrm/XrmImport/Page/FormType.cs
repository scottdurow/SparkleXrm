using System;
using System.Runtime.CompilerServices;

namespace Xrm
{

    /// <summary>
    /// Represents a FormType enumeration.
    /// </summary>
    /// <remarks>I have removed the QuickCreate (5) and ReadOptimized (11) options as they are depreciated</remarks>
    [IgnoreNamespace]
    [Imported]
    [NumericValues]
    public enum FormType
    {
        Undefined = 0,
        Create = 1,
        Update = 2,
        ReadOnly = 3,
        Disabled = 4,
        BulkEdit = 6,
    }

}