// ItemMetaData.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class ItemMetaData
    {
        public bool Selectable;
        public bool Focusable;
        public string CssClasses;
        public FormatterDelegate Formatter;
        public EditorFactory Editor;
        public Column[] Columns;
    }
}
