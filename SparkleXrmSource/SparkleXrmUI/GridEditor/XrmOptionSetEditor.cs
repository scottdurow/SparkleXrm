// OptionSetEditor.cs
//

using jQueryApi;
using Slick;
using SparkleXrm.CustomBinding;
using System.Collections.Generic;
using Xrm.ComponentModel;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.GridEditor
{
    public class XrmOptionSetEditor : GridEditorBase
    {
        public static EditorFactory EditorFactory;

        static XrmOptionSetEditor()
        {
            EditorFactory = delegate(EditorArguments args)
            {
                XrmOptionSetEditor editor = new XrmOptionSetEditor(args);
                return editor;
            };

        }

        public static string Formatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            OptionSetValue opt = (OptionSetValue)value;
            return opt == null ? "" : opt.Name;
                    
                    
        }

        private static List<OptionSetItem> _options;

        private jQueryObject _input;
        private OptionSetValue _defaultValue= new OptionSetValue(null);
       

        public XrmOptionSetEditor(EditorArguments args) : base(args)
        {
            
            // Get the option set values
            XrmOptionSetEditor self = this;
            OptionSetBindingOptions opts = (OptionSetBindingOptions)args.Column.Options;
            if (_options == null)
            {
                if (opts.GetOptionSetsDelegate != null)
                {
                    _options = opts.GetOptionSetsDelegate(args.Item);
                }
                else
                {
                    _options = MetadataCache.GetOptionSetValues(opts.entityLogicalName, opts.attributeLogicalName, opts.allowEmpty);
                } 
            }
         
            CreateSelect(self);
            
        }
        public void CreateSelect(XrmOptionSetEditor self)
        {
       

            string optionSet = "<SELECT>";
            // Add null value
            optionSet += string.Format("<OPTION title=\"\" value=\"\" {0}></OPTION>", self._defaultValue.Value == null ? "selected" : "");

            foreach (OptionSetItem o in _options)
            {
                optionSet += string.Format("<OPTION title=\"{0}\" value=\"{1}\" {2}>{0}</OPTION>", o.Name, o.Value, self._defaultValue.Value == o.Value ? "selected" : "");
            }
            optionSet += "</SELECT>";
            self._input = jQuery.FromHtml(optionSet);
            self._input.Bind("keydown.nav", delegate(jQueryEvent e)
            {
                if (e.Which == 40 || e.Which == 38) //Up or Down are used on the select scrolling
                    e.StopImmediatePropagation();
            });

            self._input.AppendTo(_args.Container);

            self._input.Focus().Select();
        }

        public override void Destroy()
        {
            if (_input!=null)
                _input.Remove();
        }

       

        public override void Focus()
        {
            _input.Focus();
        }

        public override void LoadValue(Dictionary<string, object> item)
        {
            OptionSetValue opt = (OptionSetValue)item[_args.Column.Field];
            _defaultValue = opt;
            SetDefaultValue();
        }



        public override object SerializeValue()
        {
            if (_input != null)
            {
                OptionSetValue opt = new OptionSetValue(GetValue());
                opt.Name = jQuery.Select("option:selected", _input).GetText();
             
                return opt;
            }
            else
                return null;
        }

        public override void ApplyValue(Dictionary<string, object> item, object state)
        {
            OptionSetValue opt=(OptionSetValue)state;
            item[_args.Column.Field] = opt;
            item[_args.Column.Field + "name"] = opt.Name;
            INotifyPropertyChanged itemObject = ((object)item) as INotifyPropertyChanged;
            if (itemObject != null)
                itemObject.RaisePropertyChanged(_args.Column.Field);
        }

        public override bool IsValueChanged()
        {
            if (_input != null)
            {

                string valueAsString = (_defaultValue != null && _defaultValue.Value != null) ? _defaultValue.Value.ToString() : "";

                return (_input.GetValue() != valueAsString);
            }
            else
                return false;
        }
        private int? GetValue()
        {
            string val =_input.GetValue();
            if (string.IsNullOrEmpty(val))
                return null;
            else
                return int.Parse(val);


        }
       
        private void SetDefaultValue()
        {
            if (_input != null)
            {
                _input.Value((_defaultValue!=null && _defaultValue.Value != null) ? _defaultValue.Value.ToString() : null);
                _input.Select();
            }
        }


        public static Column BindColumn(Column column, string entityLogicalName, string attributeLogicalName, bool allowEmpty)
        {
            column.Editor = EditorFactory;
            column.Formatter = Formatter;
            OptionSetBindingOptions opts = new OptionSetBindingOptions();
            opts.attributeLogicalName = attributeLogicalName;
            opts.entityLogicalName = entityLogicalName;
            opts.allowEmpty = allowEmpty;
            column.Options = opts;
            return column;
        }

        public static Column BindColumnWithOptions(Column column, OptionSetBindingOptions options)
        {
            column.Editor = EditorFactory;
            column.Formatter = Formatter;
            column.Options = options;
            return column;
        }
    }
   
}
