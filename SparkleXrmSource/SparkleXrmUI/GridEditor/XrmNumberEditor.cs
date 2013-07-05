// CrmTextEditorBase.cs
//

using jQueryApi;
using Slick;
using System;
using System.Collections.Generic;
using Xrm;

namespace SparkleXrm.GridEditor
{
    
    public class XrmNumberEditor : GridEditorBase
    {
        public static EditorFactory NumberEditor;

        private jQueryObject input;
        private string defaultValue;
        private NumberFormatInfo numberFormatInfo;

        static XrmNumberEditor()
        {
            NumberEditor = delegate(EditorArguments args)
            {
                XrmNumberEditor editor = new XrmNumberEditor(args);
                return editor;

            };

        }

        public XrmNumberEditor(EditorArguments args)
            : base(args)
        {

            numberFormatInfo = (NumberFormatInfo)args.Column.Options;     
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
            defaultValue = NumberEx.Format((Number)item[_args.Column.Field], numberFormatInfo);
           
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


            item[_args.Column.Field] = NumberEx.Parse((string)state, numberFormatInfo);
            this.RaiseOnChange(item);
        }

        public override bool IsValueChanged()
        {
            return (!(input.GetValue() == "" && defaultValue == null)) && (input.GetValue() != defaultValue);
        }

        // Formatter to format number
        public static string Formatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            if (value != null)
            {
                Number numeric = (Number)value;
                return NumberEx.Format(numeric,(NumberFormatInfo)columnDef.Options);
            }
            else
                return "";
        }

        public override ValidationResult NativeValidation(object newValue)
        {
            bool isValid = true;
         

            Number newValueNumber = NumberEx.Parse((string)newValue,numberFormatInfo);
            isValid = !(Number.IsNaN(newValueNumber));

            isValid = isValid && (newValueNumber >= numberFormatInfo.MinValue) && (newValueNumber <= numberFormatInfo.MaxValue);

            if (!isValid)
            {
                ValidationResult result = new ValidationResult();
                result.Valid = false;
                result.Message = String.Format("Please enter a number between {0} and {1}.",numberFormatInfo.MinValue,numberFormatInfo.MaxValue);
                return result;
            }
            return null;
        }

        public static Column BindColumn(Column column, int minValue, int maxValue, int precision)
        {
            column.Editor = NumberEditor;
            column.Formatter = XrmNumberEditor.Formatter;
            NumberFormatInfo numberFormatInfo = NumberEx.GetNumberFormatInfo();
            numberFormatInfo.MinValue = minValue;
            numberFormatInfo.MaxValue = maxValue;
            numberFormatInfo.Precision = precision;
            column.Options = numberFormatInfo;
            return column;
        }
        public static Column BindReadOnlyColumn(Column column, int precision)
        {
            column.Formatter = XrmNumberEditor.Formatter;
            NumberFormatInfo numberFormatInfo = NumberEx.GetNumberFormatInfo();  
            numberFormatInfo.Precision = precision;
            column.Options = numberFormatInfo;
            return column;
        }
    }
}
