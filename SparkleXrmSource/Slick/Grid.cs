
using jQueryApi;
using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    public class EditorLock
    {

        public bool IsActive()
        {
            return false;
        }
        public void CancelCurrentEdit()
        {

        }
        public bool CommitCurrentEdit()
        {
            return false;
        }
        public void Activate()
        {

        }
        public void Deactivate()
        {

        }
    
    }

    [Imported]
    public class Grid
    {
        #region Constructors
        public Grid(string containerId, object data, List<Column> columns, GridOptions options)
        {

        }
        #endregion

        #region Events
        public Event OnViewportChanged;
        public Event OnValidationError;
        public Event OnBeforeCellEditorDestroy;
        
        public Event OnAddNewRow;
        public Event OnCellChange;
        public Event OnActiveCellChanged;
        public Event OnSort;
        public Event OnKeyDown;
        public Event OnClick;
        public Event OnDblClick;
        //public Event OnRowsChanged;
        #endregion

        #region Methods
        public void SetCellCssStyles(string p, Dictionary<int, Dictionary<string, string>> highlightCells)
        {

        }
        public EditorLock GetEditorLock()
        {
            return null;
        }
        public EditController GetEditController()
        {
            return null;
        }
        public void SetSelectedRows(int[] rows)
        {

        }
        public void SetActiveCell(int row, int cell)
        {

        }
        public int[] GetSelectedRows()
        {
            return new int[0];
        }
        public Column[] GetColumns()
        {
            return null;
        }
        public void RegisterPlugin(IPlugin plugin)
        {

        }
        public void SetSelectionModel(ISelectionModel selectionModel)
        {

        }
        public ViewPort GetViewport()
        {
            return null;
        }
        public void InvalidateRow(int i)
        {
            
        }
        public void InvalidateRows(List<int> rows)
        {

        }
        public void UpdateRowCount()
        {
           
        }

        public void Invalidate()
        {

        }
        public void Render()
        {

        }
        public CellSelection GetActiveCell()
        {
            return null;
        }
        public void ResizeCanvas()
        {

        }
        public jQueryObject GetCellFromEvent(EventData e)
        {
            return null;
        }
        public object GetDataItem(int row)
        {
            return null;
        }
        public object GetData()
        {
            // Returns the associated dataview
            return null;
        }
        #endregion

    }
}
