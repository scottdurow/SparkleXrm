using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class XrmAttribute
    {
       
        public Controls Controls;

        public void AddOnChange(AddRemoveOnChangeHandler function)
        {
        }

        public void FireOnChange()
        {
        }

        public string GetAttributeType()
        {
            return null;
        }

        public string GetFormat()
        {
            return null;
        }

        public object GetInitialValue()
        {
            return null;
        }

        public bool GetIsDirty()
        {
            return false;
        }

        public int GetMax()
        {
            return -1;
        }

        public int GetMaxLength()
        {
            return -1;
        }

        public int GetMin()
        {
            return -1;
        }

        public Option GetOption(string value)
        {
            return null;
        }

        public Option GetOption(int value)
        {
            return null;
        }

        public Option[] GetOptions()
        {
            return null;
        }

        public XrmEntity GetParent()
        {
            return null;
        }

        public int GetPrecision()
        {
            return -1;
        }

        public string GetRequirementLevel()
        {
            return null;
        }

        public Option GetSelectedOption()
        {
            return null;
        }

        public string GetSubmitMode()
        {
            return null;
        }
        // Develop1 
        public string GetName()
        {
            return null;
        }
        public string GetText()
        {
            return null;
        }

        public UserPrivilege GetUserPrivilege()
        {
            return null;
        }

        public object GetValue()
        {

            return null;
        }

      

        public void RemoveOnChange(AddRemoveOnChangeHandler function)
        {
            
        }

        public void SetRequiredLevel(string requirementLevel)
        {
            
        }

        public void SetSubmitMode(string mode)
        {

        }

        public void SetValue<T>(T value)
        {

        }
    }
}
