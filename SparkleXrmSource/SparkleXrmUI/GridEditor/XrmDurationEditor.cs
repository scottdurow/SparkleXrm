// CrmDurationEditor.cs
//

using jQueryApi;
using Slick;
using System.Collections.Generic;
using Xrm.Sdk;

namespace SparkleXrm.GridEditor {
  public class XrmDurationEditor : GridEditorBase {
    public static EditorFactory DurationEditor;

    static XrmDurationEditor() {
      DurationEditor = delegate(EditorArguments args) {
        XrmDurationEditor editor = new XrmDurationEditor(args);
        return editor;
      };

    }

    public static string Formatter(int row, int cell, object value, Column columnDef, object dataContext) {
      int? durationValue = (int?) value;
      return DateTimeEx.FormatDuration(durationValue);
    }

    private jQueryObject _input;
    private int? _originalValue;

    public XrmDurationEditor(EditorArguments args) : base(args) {
      _args = args;
      _input = jQuery.FromHtml("<INPUT type=text class='editor-text' />");

      _input.AppendTo(_args.Container);
      Focus();
    }

    public override void Destroy() {
      _input.Remove();
    }

    public override void Focus() {
      _input.Focus().Select();
    }

    public override void LoadValue(Dictionary<string, object> item) {
      int? value = (int?) item[_args.Column.Field];
      _input.Value(DateTimeEx.FormatDuration(value));
      _originalValue = value;
      Focus();
    }

    public override object SerializeValue() {
      string durationString = _input.GetValue();
      if (durationString == "") {
        return null;
      }
      int? duration = DateTimeEx.ParseDuration(durationString);
      return duration;
    }

    public override void ApplyValue(Dictionary<string, object> item, object state) {
      item[_args.Column.Field] = state;
      this.RaiseOnChange(item);
    }

    public override bool IsValueChanged() {
      string durationString = _input.GetValue();
      string originalDurationString = DateTimeEx.FormatDuration(_originalValue);
      return originalDurationString != durationString;
    }

    public static Column BindColumn(Column column) {
      column.Editor = DurationEditor;
      column.Formatter = Formatter;
      return column;
    }
  }
}
