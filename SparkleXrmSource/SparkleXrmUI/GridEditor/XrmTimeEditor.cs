// DataEditor.cs
//

using jQueryApi;
using jQueryApi.UI.Widgets;
using Slick;
using System.Collections.Generic;
using Xrm.Sdk;


namespace SparkleXrm.GridEditor
{
    public class XrmTimeEditor : GridEditorBase
    {
        public static EditorFactory TimeEditor;
        static XrmTimeEditor()
        {
            TimeEditor = delegate(EditorArguments args)
            {
                XrmTimeEditor editor = new XrmTimeEditor(args);
                return editor;
            };

        }
        public static string Formatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            DateTime dateValue = (DateTime)value;
            return formatTime(dateValue, (string)columnDef.Options);
        }

        private static string formatTime(DateTime dateValue, string format)
        {
            string timeFormatted = "";
            if (dateValue != null)
            {
                timeFormatted = dateValue.Format(format);

            }
            return timeFormatted;
        }

        private jQueryObject _input;
        private jQueryObject _container;
        private bool _searchOpen = false;
        private DateTime _dateTimeValue;
        private DateTime _originalDateTimeValue;
        private string _formatString = "h:mm tt";
        public XrmTimeEditor(EditorArguments args) : base(args)
        {
            bool justSelected = false;
        
            XrmTimeEditor self = this;
            if (OrganizationServiceProxy.UserSettings != null)
            {
                _formatString = OrganizationServiceProxy.UserSettings.TimeFormatString;
            }

          
            _container = jQuery.FromHtml("<div ><table class='inline-edit-container' cellspacing='0' cellpadding='0'><tr><td><INPUT type=text class='sparkle-input-inline' /></td><td class='lookup-button-td'><input type=button class='autocompleteButton' /></td></tr></table></div>");

            _container.AppendTo(_args.Container);

            jQueryObject inputField = _container.Find(".sparkle-input-inline");
           
            _input = inputField;
            _input.Focus().Select();
            string timeFormatString = _formatString;
            AutoCompleteOptions options = GetTimePickerAutoCompleteOptions(timeFormatString);

            options.Select = delegate(jQueryEvent e, AutoCompleteSelectEvent uiEvent)
            {
                justSelected = true;
            };
            options.Open = delegate(jQueryEvent e, jQueryObject o)
            {
                self._searchOpen = true;
            };

            options.Close = delegate(jQueryEvent e, jQueryObject o)
            {
                self._searchOpen = false;
            };

            inputField = inputField.Plugin<AutoCompleteObject>().AutoComplete(options);
            jQueryObject selectButton = _container.Find(".autocompleteButton");
            // Add the click binding to show the drop down
            selectButton.Click(delegate(jQueryEvent e)
            {
                inputField.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Search, "");
            });

            // Bind return to searching 
           
            _input.Keydown(delegate(jQueryEvent e)
            {
               
                if (self._searchOpen)
                {
                    switch (e.Which)
                    {
                        case 13: // Return
                        case 38: // Up - don't navigate - but use the dropdown to select search results
                        case 40: // Down - don't navigate - but use the dropdown to select search results
                            e.PreventDefault();
                            e.StopPropagation();
                            break;
                             
                    }
                }
               
                justSelected = false;
            });


        }

        public static AutoCompleteOptions GetTimePickerAutoCompleteOptions(string timeFormatString)
        {
            AutoCompleteOptions options = new AutoCompleteOptions();
            string[] timeOptions = new string[48];
            DateTime autoCompleteDate = DateTime.Parse("2000-01-01T00:00:00");
            for (int i = 0; i < 48; i++)
            {
                timeOptions[i] = formatTime(autoCompleteDate, timeFormatString);

                autoCompleteDate = DateTimeEx.DateAdd(DateInterval.Minutes, 30, autoCompleteDate);
            }
            options.Source = timeOptions;
            options.MinLength = 0;
            options.Delay = 0;
            options.Position = new Dictionary<string, object>("collision", "fit");
            return options;
        }

        public override void Destroy()
        {

            _input.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Close);
            _input.Plugin<AutoCompleteObject>().AutoComplete(AutoCompleteMethod.Destroy);
            _container.Remove();
        }

        public override void Show()
        {
        }

        public override void Hide()
        {

        }

        public override void Position(jQueryPosition position)
        {
        }

        public override void Focus()
        {
            _input.Focus();
        }

        public override void LoadValue(Dictionary<string, object> item)
        {
            base.LoadValue(item);
            _dateTimeValue = (DateTime)item[_args.Column.Field];
            _originalDateTimeValue = _dateTimeValue;
            _input.Value(formatTime(_dateTimeValue, _formatString));
            _input.Select();
        }

        public override object SerializeValue()
        {



            string timeString = _input.GetValue();
            if (timeString == "")
            {
                return null;
            }

            DateTime currentDate = DateTimeEx.AddTimeToDate(_dateTimeValue, timeString);
            return currentDate;




        }

        public override void ApplyValue(Dictionary<string, object> item, object state)
        {
            item[_args.Column.Field] = state;

            this.RaiseOnChange(item);


        }

        public override bool IsValueChanged()
        {
            string timeString = _input.GetValue();
            string originalDateString = formatTime(_originalDateTimeValue, _formatString);
            string newDateString = "";
            if (timeString != "")
            {
                DateTime currentDate = DateTimeEx.AddTimeToDate(_dateTimeValue, timeString);
                newDateString = formatTime(currentDate, _formatString);
            }
            return originalDateString != newDateString;
        }



        public static Column BindColumn(Column column)
        {
            column.Editor = TimeEditor;
            column.Formatter = Formatter;
            column.Options =OrganizationServiceProxy.UserSettings.TimeFormatString;
            return column;
        }
        public static Column BindReadOnlyColumn(Column column)
        {
            column.Formatter = Formatter;
            column.Options = OrganizationServiceProxy.UserSettings.TimeFormatString;
            return column;
        }
    }

}
