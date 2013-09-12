// IGroupedDataView.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public interface IGroupedDataView
    {
        void ExpandGroup(string groupingKey);
        void CollapseGroup(string groupingKey);

    }
}
