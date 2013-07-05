// DataEditor.cs
//

using jQueryApi;
using Slick;
using System.Collections.Generic;
using Xrm.ComponentModel;


namespace SparkleXrm.GridEditor
{
    public class GridEditorBase
    {
        protected EditorArguments _args;
        protected Dictionary<string, object> _item;
       
        public GridEditorBase(EditorArguments args)
        {
            _args = args;
        }
        public virtual void Destroy()
        {
           
        }

        public virtual void Show()
        {
            
        }

        public virtual void Hide()
        {
           
        }

        public virtual void Position(jQueryPosition position)
        {
         
        }

        public virtual void Focus()
        {
            
        }

        public virtual void LoadValue(Dictionary<string, object> item)
        {
            _item = item;
        }

        public virtual object SerializeValue()
        {
            return null;
        }

        public virtual void ApplyValue(Dictionary<string, object> item, object state)
        {
           

        }
        protected void RaiseOnChange(object item)
        {
            INotifyPropertyChanged itemObject = item as INotifyPropertyChanged;
            if (itemObject != null)
                itemObject.RaisePropertyChanged(_args.Column.Field);
            
        }

        public virtual  bool IsValueChanged() {

            return false;
        }
        public virtual ValidationResult NativeValidation(object newValue)
        {
            return null;

        }
        public virtual ValidationResult Validate()
        {
            object newValue = SerializeValue();
            ValidationResult result = this.NativeValidation(newValue);

           
            if (result==null && _args.Column.Validator != null)
            {
                ValidationResult validationResults = _args.Column.Validator(newValue, _args.Item);
                if (!validationResults.Valid)
                {
                    result = validationResults;
                }
            }

            if (result == null) 
            {
                result = new ValidationResult();
                result.Valid = true;
                result.Message = null;
            }
            
            return result;
        }
      

    }
    
}
