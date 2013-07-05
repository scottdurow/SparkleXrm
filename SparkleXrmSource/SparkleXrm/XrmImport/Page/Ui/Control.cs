using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class Control
    {
        
        public bool? OriginalIsDisabled;

       
        public void AddOnChange(AddRemoveOnChangeHandler function)
        {
        }

        public void AddCustomView(string viewId, string entityName, string viewDisplayName, string fetchXml, string layoutXml, bool isDefault)
        {
        }

        public void AddOption(Option option, int index)
        {
        }

        public void ClearOptions()
        {
        }

        public XrmAttribute GetAttribute()
        {
            return null;
        }

        public string GetControlType()
        {
            return null;
        }

        public string GetData()
        {
            return null;
        }

        public string GetDefaultView()
        {
            return null;
        }

        public bool GetDisabled()
        {
            return false;
        }

        public string GetLabel()
        {
            return null;
        }

        public string GetName()
        {
            return null;
        }

        public Control GetParent()
        {
            return null;
        }

        public string GetSrc()
        {
            return null;
        }

        public string GetInitialUrl()
        {
            return null;
        }

        public object GetObject()
        {
            return null;
        }

        public bool GetVisible()
        {
            return false;
        }

        public void Refresh()
        {
            return;
        }

        public void RemoveOption(int value)
        {
            
        }

        public void SetData(string data)
        {
        }

        public void SetDefaultView(string viewId)
        {
        }

        public void SetDisabled(bool disable)
        {
        }

        public void SetFocus()
        {
        }

        public void SetLabel(string label)
        {
        }

        public void SetSrc(string src)
        {
        }

        public void SetVisible(bool visible)
        {
        }
    }
}
