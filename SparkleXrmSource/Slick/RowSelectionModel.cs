// RowSelectionModel.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    public class RowSelectionModel : ISelectionModel
    {
       
        public RowSelectionModel(RowSelectionModelOptions options)
        {

        }
        public Event OnSelectedRangesChanged;
    }

   


    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class RowSelectionModelOptions
    {
        public bool SelectActiveRow;
        public bool MultiRowSelect;
      
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public interface ISelectionModel
    {

    }
    

}
