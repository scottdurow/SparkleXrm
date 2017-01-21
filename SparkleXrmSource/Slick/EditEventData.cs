// Class1.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{


    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class EditEventData
    {
        public Column column;
        public Grid grid;
        public object item;
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OnCellChangedEventData
    {
        public int Row;
        public int Cell;
        public object Item;
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class SortEventData
    {
        public SortColData[] SortCols;
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class SortColData
    {
        public string ColumnId;
        public Column SortCol;
        public bool SortAsc;
    }
}
