// Cell.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class CellSelection
    {
        public int? Row;
        public int? Cell;
        public Grid Grid;
    }
}
