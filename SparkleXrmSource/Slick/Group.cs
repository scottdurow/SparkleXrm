// Group.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class Group
    {
        public bool Collapsed;
        public int Count;
        public int Level;
        public string Title;
        public object Value;
        public object groups;
        public List<object> Rows;
        public object Totals;
        public string GroupingKey;
    }
}
