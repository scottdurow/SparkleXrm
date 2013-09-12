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
    [NamedValues]
    public enum ColumnProperties
    {
        [ScriptName("id")]
        Id,
        [ScriptName("name")]
        Name,
        [ScriptName("field")]
        Field,
        [ScriptName("cssClass")]
        CssClass,
        [ScriptName("width")]
        Width,
        [ScriptName("minWidth")]
        MinWidth,
        [ScriptName("maxWidth")]
        MaxWidth,
        [ScriptName("resizable")]
        Resizable,
        [ScriptName("selectable")]
        Selectable,
        [ScriptName("focusable")]
        Focusable,
        [ScriptName("editor")]
        Editor,
        [ScriptName("groupTotalsFormatter")]
        GroupTotalsFormatter,
        [ScriptName("sortable")]
        Sortable,
        [ScriptName("formatter")]
        Formatter,
        [ScriptName("options")]
        Options,
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class Column
    {
        public Column(params object[] nameValuePairs)
        {

        }
        public string Id;
        public string Name;
        public string Field;
        public int? Width;
        public int? MinWidth;
        public int? MaxWidth;
        public EditorFactory Editor;
        public FormatterDelegate Formatter;
        public GridValidatorDelegate Validator;
        [ScriptName("colspan")]
        public string ColSpan;
        public bool? Sortable;
        public object Options;
        public string DataType;
        public bool? Selectable;
        public string CssClass;
        public bool? Resizable;
        public SubTotalsFormatterDelegate GroupTotalsFormatter;

    }
    [Imported]
    public delegate string SubTotalsFormatterDelegate(Dictionary<string, object> totals, Column columnDef);
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
