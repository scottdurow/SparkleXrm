// AutocompleteResults.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.XrmImport.Page.Ui
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class AutocompleteResultSet
    {
        public List<AutocompleteResult> Results;
        public AutocompleteAction Commands;
    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class AutocompleteResult
    {
        public string Id;
        public string Icon;
        public string[] Fields;
    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class AutocompleteAction
    {
        public string Id;
        public string Icon;
        public string Label;
        public Action Action;
    }
}
