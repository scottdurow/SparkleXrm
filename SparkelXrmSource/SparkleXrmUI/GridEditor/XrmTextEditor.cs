// CrmTextEditorBase.cs
//

using jQueryApi;
using Slick;
using System.Collections.Generic;

namespace SparkleXrm.GridEditor
{
    public class XrmTextEditor : GridEditorBase
    {
        public static EditorFactory TextEditor;

        private jQueryObject input;
        private string defaultValue;

        static XrmTextEditor()
        {
            TextEditor = delegate(EditorArguments args)
            {
                XrmTextEditor editor = new XrmTextEditor(args);
                return editor;
            };

        }

        public XrmTextEditor(EditorArguments args) :base(args)
        {
            
            input = jQuery.FromHtml("<INPUT type=text class='editor-text' />")
                .AppendTo(args.Container)
                .Bind("keydown.nav",delegate(jQueryEvent e){
                if (e.Which == 37 || e.Which == 39 ) //LEFT or RIGHT{
                    e.StopImmediatePropagation();
                  
            })
           .Focus()
           .Select();
       
          
          }
        public override void Destroy()
        {
            base.Destroy();
            input.Remove();
        }
        public override void Focus()
        {
            base.Focus();
            input.Focus();
        }

        public string GetValue()
        {
            return input.GetValue();
        }
        public void SetValue(string value)
        {
            input.Value(value);
        }
        public override void LoadValue(Dictionary<string, object> item)
        {
            defaultValue = (string)item[_args.Column.Field];
            if (defaultValue == null) defaultValue = "";
            input.Value(defaultValue);
            input[0].SetAttribute("defaultValue", defaultValue);
            input.Select();

        }

        public override object SerializeValue()
        {
            return input.GetValue();
        }

        public override void ApplyValue(Dictionary<string, object> item, object state)
        {
            item[_args.Column.Field] = state;
            this.RaiseOnChange(item);
        }

        public override bool IsValueChanged()
        {
            return (!(input.GetValue() == "" && defaultValue == null)) && (input.GetValue() != defaultValue);
        }


        public static Column BindColumn(Column column)
        {
            column.Editor = TextEditor;
            column.Formatter = XrmTextEditor.Formatter;
            return column;
        }
        public static Column BindReadOnlyColumn(Column column)
        {
            column.Formatter = XrmTextEditor.Formatter;
            return column;
        }
        public static string Formatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            if (value != null)
            {
                return (string)value;
            }
            else
                return "";
        }
       
    }
}
