// XrmBooleanEditor.cs
//

using jQueryApi;
//using jQueryApi.UI.Widgets;
using SparkleXrm.CustomBinding;
using Slick;
using System;
using System.Collections.Generic;
using Xrm.Sdk;
using Xrm.ComponentModel;

namespace SparkleXrm.GridEditor
{
    public class XrmBooleanEditor : GridEditorBase
    {
        public static EditorFactory BooleanEditor;

        private jQueryObject input;
        private bool defaultValue = false;

        static XrmBooleanEditor()
        {
            BooleanEditor = delegate(EditorArguments args)
            {
                XrmBooleanEditor editor = new XrmBooleanEditor(args);
                return editor;
            };
        }

        public XrmBooleanEditor(EditorArguments args)
            : base(args)
        {
            input = jQuery.FromHtml("<input type='checkbox' class='editor-boolean'/>")
                .AppendTo(args.Container)
                .Bind("keydown.nav", delegate(jQueryEvent e)
            {
                if (e.Which == 37 || e.Which == 39)  //LEFT or RIGHT key
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

        /// <summary>
        /// Return the current value of the checkbox control as a Boolean.
        /// </summary>
        /// <returns></returns>
        private bool GetValue()
        {
            return input.Is(":checked");
        }

        /// <summary>
        /// Set the editor control value.
        /// </summary>
        /// <param name="item"></param>
        public override void LoadValue(Dictionary<string, object> item)
        {
            defaultValue = (bool)item[_args.Column.Field];
            if(defaultValue)
            {
                input[0].SetAttribute("checked", "checked");
            }
            else
            {
                input[0].RemoveAttribute("checked");
            }
            
            input[0].SetAttribute("defaultValue", defaultValue);
            input.Select(); 
        }

        public override object SerializeValue()
        {
            return this.GetValue();
        }

        /// <summary>
        /// Save the new value back to the view model.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        public override void ApplyValue(Dictionary<string, object> item, object state)
        {
            item[_args.Column.Field] = (string)state;
            this.RaiseOnChange(item);
        }

        
        /// <summary>
        /// Test the control value and indicate if it is different than the value that was loaded.
        /// </summary>
        /// <returns></returns>
        public override bool IsValueChanged()
        {
            bool val = this.GetValue();
            return (val != defaultValue);
        }

        public static Column BindColumn(Column column, string TrueOptionDisplayName, string FalseOptionDisplayName)
        {
            column.Editor = BooleanEditor;
            column.Formatter = XrmBooleanEditor.Formatter;

            BooleanBindingOptions opts = new BooleanBindingOptions();
            opts.TrueOptionDisplayName = TrueOptionDisplayName;
            opts.FalseOptionDisplayName = FalseOptionDisplayName;
            column.Options = opts;

            return column;
        }

        public static Column BindReadOnlyColumn(Column column, string TrueOptionDisplayName, string FalseOptionDisplayName)
        {
            column.Formatter = XrmBooleanEditor.Formatter;

            BooleanBindingOptions opts = new BooleanBindingOptions();
            opts.TrueOptionDisplayName = TrueOptionDisplayName;
            opts.FalseOptionDisplayName = FalseOptionDisplayName;
            column.Options = opts;
           
            return column;
        }

        public static string Formatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            string trueValue = "True";
            string falseValue = "False";

            BooleanBindingOptions opts = (BooleanBindingOptions)columnDef.Options;
            if (opts != null && opts.TrueOptionDisplayName != null)
                trueValue = opts.TrueOptionDisplayName;
            if (opts != null && opts.FalseOptionDisplayName != null)
                falseValue = opts.FalseOptionDisplayName;

            if (value != null)
            {
                return ((bool)value) ? trueValue : falseValue;
            }
            else
                return falseValue;
        }
    }
}
