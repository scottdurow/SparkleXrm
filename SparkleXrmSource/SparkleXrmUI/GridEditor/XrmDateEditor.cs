// DataEditor.cs
//

using jQueryApi;
using jQueryApi.UI.Widgets;
using Slick;
using System;
using System.Collections.Generic;
using Xrm.Sdk;


namespace SparkleXrm.GridEditor
{
    public class XrmDateEditor : GridEditorBase
    {
        public static EditorFactory CrmDateEditor;

        static XrmDateEditor()
        {
            CrmDateEditor = delegate(EditorArguments args)
            {
                XrmDateEditor editor = new XrmDateEditor(args);
                return editor;
            };

        }

        public static string FormatterDateOnly(int row, int cell, object value, Column columnDef, object dataContext)
        {
            string dateFormat = (string)columnDef.Options;
            if (OrganizationServiceProxy.UserSettings != null)
            {
                dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString;
            }
            DateTime dateValue = (DateTime)value;
            return DateTimeEx.FormatDateSpecific(dateValue, dateFormat);
        }
        public static string FormatterDateAndTime(int row, int cell, object value, Column columnDef, object dataContext)
        {
            string dateFormat = (string)columnDef.Options;
            if (OrganizationServiceProxy.UserSettings != null)
            {
                dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString + " " + OrganizationServiceProxy.UserSettings.TimeFormatString;
            }
            DateTime dateValue = (DateTime)value;
            return DateTimeEx.FormatDateSpecific(dateValue, dateFormat);
        }

        private jQueryObject _input;
        private jQueryObject _container;
        private DateTime _defaultValue=null;
        private bool _calendarOpen = false;
        
        private DateTime _selectedValue = null;
        private string _dateFormat = "dd/mm/yy";

        public XrmDateEditor(EditorArguments args) : base(args)
        {
       
           
            _container = jQuery.FromHtml("<div ><table class='inline-edit-container' cellspacing='0' cellpadding='0'><tr>" + 
                "<td><INPUT type=text class='sparkle-input-inline' /></td>" +
                "<td class='lookup-button-td'><input type=button class='sparkle-imagestrip-inlineedit_calendar_icon' /></td></tr></table></div>");
            _container.AppendTo(_args.Container);
            
            _input = _container.Find(".sparkle-input-inline");
            jQueryObject selectButton = _container.Find(".sparkle-imagestrip-inlineedit_calendar_icon");
           
            
            _input.Focus().Select();
  
            DatePickerOptions2 options2 = new DatePickerOptions2();
            options2.ShowOtherMonths = true;
            options2.FirstDay = OrganizationServiceProxy.OrganizationSettings != null ? OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value : 0;
            options2.BeforeShow = delegate()
            {
                this._calendarOpen = true;
            };

            options2.OnClose = delegate()
            {
                this._calendarOpen = false;
                _selectedValue = GetSelectedValue();
            };
             
            if (OrganizationServiceProxy.UserSettings != null)
            {
                _dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString;
            }

            options2.DateFormat = _dateFormat;

            _input.Plugin<DatePickerPlugIn>().DatePicker(options2);

            // Wire up the date picker button
            selectButton.Click(delegate(jQueryEvent e){
              
                _input.Plugin<DatePickerPlugIn>().DatePicker(DatePickerMethod2.Show);
            });

            //_input.Width(_input.GetWidth() - 24);
        }

        public override void Destroy()
        {
            ((jQueryObject)Script.Literal("$.datepicker.dpDiv")).Stop(true, true);
            _input.Plugin<DatePickerPlugIn>().DatePicker(DatePickerMethod2.Hide);
            _input.Plugin<DatePickerPlugIn>().DatePicker(DatePickerMethod2.Destroy);
            Hide(); // Ensure the calendar is hidden when ending an edit
            _container.Remove();
        }

        public override void Show()
        {
            if (_calendarOpen) {
                ((jQueryObject)Script.Literal("$.datepicker.dpDiv")).Stop(true, true).Show();
            }
        }

        public override void Hide()
        {
            if (_calendarOpen) {
                ((jQueryObject)Script.Literal("s.datepicker.dpDiv")).Stop(true, true).Hide();
            }
        }

        public override void Position(jQueryPosition position)
        {
            if (!_calendarOpen) {
                return;
            }
          
            ((jQueryObject)Script.Literal("$.datepicker.dpDiv")).CSS("top", (position.Top + 30).ToString()).CSS("left", position.Left.ToString());
           
        }

        public override void Focus()
        {
            _input.Focus();
        }

        public override void LoadValue(Dictionary<string, object> item)
        {
            DateTime currentValue = (DateTime)item[_args.Column.Field];
           _defaultValue = currentValue!=null ? currentValue : null;
            string valueFormated = _defaultValue != null ? _defaultValue.ToLocaleDateString() : String.Empty;

            if (_args.Column.Formatter != null) {
                valueFormated = _args.Column.Formatter(0, 0, _defaultValue, _args.Column, null);
            }
            

            //_input[0].defaultValue = defaultValue;
            SetSelectedValue(_defaultValue);
            //_input.Value(valueFormated);
            _input.Select();
        }

        public override object SerializeValue()
        {
            //string stringVal = _input.GetValue();
            //Date dateVal = Date.Parse(stringVal);
            return GetSelectedValue();
        }

        public override void ApplyValue(Dictionary<string, object> item, object state)
        {
            DateTime previousValue = (DateTime)item[_args.Column.Field];
            DateTime newValue = (DateTime)state;
            DateTimeEx.SetTime(newValue, previousValue);
            

            item[_args.Column.Field] = newValue;
            this.RaiseOnChange(item);
            

        }

        public override bool IsValueChanged() {

            DateTime selectedValue = GetSelectedValue();
            string selected = selectedValue == null ? "" : selectedValue.ToString();
            string defaultValueString = _defaultValue == null ? "" : _defaultValue.ToString(); 

            return (selected!=defaultValueString);
        }
        private DateTime GetSelectedValue()
        {
            DateTime selectedValue = (DateTime)_input.Plugin<DatePickerObject>().DatePicker(DatePickerMethod.GetDate);

            return selectedValue;
        }
        private void SetSelectedValue(DateTime date)
        {
            _input.Plugin<DatePickerObject>().DatePicker(DatePickerMethod.SetDate, date);

           
        }




        public static Column BindColumn(Column column, bool dateOnly)
        {
            column.Editor = CrmDateEditor;
            column.Formatter = FormatterDateOnly;
            return column;
        }
        public static Column BindReadOnlyColumn(Column column, bool dateOnly)
        {
            column.Formatter = FormatterDateOnly;
            return column;
        }
    }
    
}
