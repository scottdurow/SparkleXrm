

using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;




//[Imported]
//[IgnoreNamespace]
//[ScriptName("Object")]
//public sealed class AutocompleteOptions
//{

  
//    public string[] Source;
//    public Action<object, jQueryObject> Select; //TODO: correct delegate sig
//    public Action<object, jQueryObject> Change; //TODO: correct delegate sig
//    public int? minLength;
//    public AutocompleteOptions()
//    {
//    }

//    public AutocompleteOptions(params object[] nameValuePairs)
//    {
//    }
//}

#region Script# Support
[Imported]
[IgnoreNamespace]
public sealed class AutoCompletePlugIn : jQueryObject
{
     [ScriptName("autocomplete")]
    public jQueryObject Autocomplete()
    {
        return null;
    }
    [ScriptName("autocomplete")]
     public jQueryObject Autocomplete(AutocompleteOptions options)
     {
        return null;
    }
     [ScriptName("autocomplete")]
    public object Autocomplete(string action)
    {
        return null;
    }

}
#endregion
