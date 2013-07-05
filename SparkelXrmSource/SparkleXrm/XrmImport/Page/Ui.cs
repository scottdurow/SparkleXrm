using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("10*")]
    public enum FormTypes
    {
        [ScriptName("0")]
         Undefined =0,
        [ScriptName("1")]
         Create = 1,
        [ScriptName("2")]
         Update = 2,
        [ScriptName("3")]
         ReadOnly = 3,
        [ScriptName("4")]
         Disabled = 4,
        [ScriptName("5")]
         QuickCreate=5,
        [ScriptName("6")]
         BulkEdit = 6
    }

    [Imported]
    public class UI
    {
        public Controls Controls;
        public Navigation Navigation;
        public FormSelector FormSelector;
        public Tabs Tabs;

        public void Close()
        {
        }

        public Control GetCurrentControl()
        {
            return null;
        }

        public FormTypes GetFormType()
        {
            return FormTypes.Undefined;
        }

        public int GetViewPortHeight()
        {
            return -1;
        }

        public int GetViewPortWidth()
        {
            return -1;
        }

        public void RefreshRibbon()
        {
        }
    }
}
