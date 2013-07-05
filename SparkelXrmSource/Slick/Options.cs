// Options.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class GridOptions
    {
         public bool EnableCellNavigation;
         public bool EnableColumnReorder;
         public bool ForceFitColumns;
         public bool Editable;
         public bool AutoEdit;
         public int? RowHeight;
         public int? HeaderRowHeight;
         public bool EnableAddRow;
         public bool MultiSelect;
         public bool ExplicitInitialization;
         public int? DefaultColumnWidth;
         public int? LeaveSpaceForNewRows;
         public bool AsyncEditorLoading;
         public int? AsyncEditorLoadDelay;
         public bool EnableAsyncPostRender;
         public int? AsyncPostRenderDelay;
         public bool AutoHeight;
     
      //showHeaderRow: false,
      //headerRowHeight: 25,
      //showTopPanel: false,
      //topPanelHeight: 25,
      //formatterFactory: null,
      //editorFactory: null,
      //cellFlashingCssClass: "flashing",
      //selectedCellCssClass: "selected",
      //multiSelect: true,
      //enableTextSelectionOnCells: false,
      //dataItemColumnValueExtractor: null,
      //fullWidthRows: false,
      //multiColumnSort: false,
      //defaultFormatter: defaultFormatter,
      //forceSyncScrolling: false
         
    }
}
