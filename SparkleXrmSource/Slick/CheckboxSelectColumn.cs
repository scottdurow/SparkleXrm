// CheckboxSelectColumn.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    public class CheckboxSelectColumn : IPlugin
    {
        public CheckboxSelectColumn(CheckboxSelectColumnOptions options)
        {

        }
        public Column GetColumnDefinition()
        {
            return null;
        }
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class CheckboxSelectColumnOptions
    {
        public string cssClass; 
    }
}
