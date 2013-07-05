// CrmTextEditorBase.cs
//

using jQueryApi;
using Slick;
using System;
using System.Collections.Generic;
using Xrm;
using Xrm.Sdk;

namespace SparkleXrm.GridEditor
{
    
    public class XrmMoneyEditor : GridEditorBase
    {
        public static EditorFactory MoneyEditor;

        private jQueryObject input;
        private jQueryObject currencySymbol;
        private string defaultValue;
        private NumberFormatInfo numberFormatInfo;

        static XrmMoneyEditor()
        {
            MoneyEditor = delegate(EditorArguments args)
            {
                XrmMoneyEditor editor = new XrmMoneyEditor(args);
                return editor;

            };

        }

        public XrmMoneyEditor(EditorArguments args)
            : base(args)
        {

            numberFormatInfo = (NumberFormatInfo)args.Column.Options;
            currencySymbol = jQuery.FromHtml("<SPAN/>").AppendTo(args.Container);
           

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
            currencySymbol.Remove();
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
            // Get currency symbol
            string currencySymbolString = getCurrencySymbol((EntityReference)((EntityBuiltInAttributes)((object)item)).TransactionCurrencyId);
            currencySymbol.Text(currencySymbolString + " ");
            Money value = (Money)item[_args.Column.Field];
            defaultValue = "";
            if (value!=null)
            {
                defaultValue = NumberEx.Format(value.Value, numberFormatInfo);
            }
          
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

            Money money = new Money((decimal)NumberEx.Parse((string)state, numberFormatInfo));
            item[_args.Column.Field] = money;
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

                string currencySymbol = getCurrencySymbol((EntityReference)((EntityBuiltInAttributes)dataContext).TransactionCurrencyId);
                Money numeric = (Money)value;
                return currencySymbol + " " + NumberEx.Format(numeric.Value, (NumberFormatInfo)columnDef.Options);
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

        public static Column BindColumn(Column column, int minValue, int maxValue)
        {
            column.Editor = MoneyEditor;
            column.Formatter = Formatter;
            NumberFormatInfo numberFormatInfo = NumberEx.GetCurrencyEditFormatInfo();
            numberFormatInfo.MinValue = minValue;
            numberFormatInfo.MaxValue = maxValue;
            column.Options = numberFormatInfo;
            return column;
        }
        public static Column BindReadOnlyColumn(Column column)
        {
            column.Formatter = Formatter;
            NumberFormatInfo numberFormatInfo = NumberEx.GetCurrencyEditFormatInfo();
            column.Options = numberFormatInfo;
            return column;
        }

        public static string getCurrencySymbol(EntityReference currencyid)
        {

            if (currencyid != null)
            {
                return NumberEx.GetCurrencySymbol(currencyid.Id);
            }

            return string.Empty;

        }
    }
}
