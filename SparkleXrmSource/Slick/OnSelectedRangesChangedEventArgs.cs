// OnSelectedRangesChangedEventArgs.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{


    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class SelectedRange
    {
        public int? FromCell;
        public int? FromRow;
        public int? ToCell;
        public int? ToRow;
        public bool IsSingleCell() { return false; }
        public bool IsSingleRow() { return false; }

    }
}
