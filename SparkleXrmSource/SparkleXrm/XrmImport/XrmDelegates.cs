
namespace Xrm
{
    public delegate bool GetAttributeHandler(XrmAttribute attribute, int index);
    public delegate bool GetControlHandler(Control control, int index);
    public delegate bool GetNavigationItemHandler(NavigationItem item, int index);
    public delegate bool GetFormSelectorItemHandler(FormSelectorItem item, int index);
    public delegate bool GetTabItemHandler(TabItem tab, int index);
    public delegate bool GetTabSectionHandler(TabSection section, int index);
    public delegate void AddRemoveOnSaveHandler(ExecutionContext context);
    public delegate void AddRemoveOnChangeHandler(ExecutionContext context);
    public delegate void ErrorCallbackHandler(int errorCode, string message);
    public delegate void ParameterlessFunctionHandler();
    public delegate void ExecutionContextFunctionHandler(ExecutionContext context);
}
