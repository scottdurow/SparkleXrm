// Column.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Slick;

namespace Slick
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class Column
    {
        public string Id;
        public string Name;
        public string Field;
        public int? Width;
        public int? MinWidth;
        public int? MaxWidth;
        public EditorFactory Editor;
        public FormatterDelegate Formatter;
        public GridValidatorDelegate Validator;
        public string ColSpan;
        public bool Sortable;
        public object Options;
    }
    
    [Imported]
    public delegate string FormatterDelegate(int row, int cell, object value, Column columnDef, object dataContext);

    [Imported]
    public delegate ValidationResult GridValidatorDelegate(object value,object item);

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class ValidationResult
    {
        public bool Valid;
        public string Message;
        
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class ValidationEventArgs
    {
        public object Editor;
        public object CellNode;
        public ValidationResult ValidationResults;
        public int Row;
        public int Cell;
        public Column Column;
    }
}
