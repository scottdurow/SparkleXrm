// GridDataViewBinder.cs
//

using jQueryApi;
using Slick;
using Slick.Data;
using SparkleXrm.jQueryPlugins;
using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.GridEditor
{
    public class GridDataViewBinder
    {
        public bool SelectActiveRow = true;
        public bool AddCheckBoxSelectColumn = true;
        public bool MultiSelect = true;
        public bool ValidationPopupUseFitPosition = false;
        private string _sortColumnName;
        private Grid _grid;
        /// <summary>
        /// DataBinds a DataView that inherits from DataViewBase
        /// 
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="columns"></param>
        /// <param name="gridId"></param>
        /// <param name="pagerId"></param>
        /// <param name="editable"></param>
        /// <param name="allowAddNewRow"></param>
        /// <returns></returns>
        public Grid DataBindXrmGrid(DataViewBase dataView, List<Column> columns, string gridId, string pagerId,bool editable, bool allowAddNewRow )
        {
            // Always add an empty column on the end for reszing purposes
            ArrayEx.Add(columns, new Column());

            GridOptions gridOptions = new GridOptions();
            gridOptions.EnableCellNavigation = true;
            gridOptions.AutoEdit = editable;
            gridOptions.Editable = editable;
            gridOptions.AsyncEditorLoading = true;
            gridOptions.EnableAddRow = allowAddNewRow;

            // Set non-variable options
            gridOptions.RowHeight = PageEx.MajorVersion==2013 ? 30 : 20;
            gridOptions.HeaderRowHeight = 25;
            //gridOptions.ForceFitColumns = true;
            gridOptions.EnableColumnReorder = false;

            CheckboxSelectColumn checkBoxSelector = null;
            if (AddCheckBoxSelectColumn)
            {
                CheckboxSelectColumnOptions checkboxOptions = new CheckboxSelectColumnOptions();
                checkboxOptions.cssClass = "sparkle-checkbox-column";

                // Add check box column
                checkBoxSelector = new CheckboxSelectColumn(checkboxOptions);
                Column checkBoxColumn = checkBoxSelector.GetColumnDefinition();
                columns.Insert(0, checkBoxColumn);
            }

            Grid grid = new Grid("#" + gridId, dataView, columns, gridOptions);

            if (AddCheckBoxSelectColumn)
            {
                grid.RegisterPlugin(checkBoxSelector);
            }

            this.DataBindSelectionModel(grid, dataView);
            if (!string.IsNullOrEmpty(pagerId))
            {
                CrmPagerControl pager = new CrmPagerControl(dataView, grid, jQuery.Select("#" + pagerId));
            }
            DataBindEvents(grid, dataView, gridId);
            AddValidation(grid, dataView);
            AddRefreshButton(gridId, dataView);
          
            // Add resize event
            jQuery.Window.Resize(delegate(jQueryEvent e){
                // Set each column to be non resizable while we do the resize
                FreezeColumns(grid, true);
                grid.ResizeCanvas();
                // Restore the resizing 
                FreezeColumns(grid, false);
            });

            dataView.OnDataLoaded.Subscribe(delegate(EventData e, object o)
            {
                FreezeColumns(grid,false);
            });
            _grid = grid;
            return grid;
        }

        /// <summary>
        /// Data Binds the standard Slick.DataView
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="dataView"></param>
        /// <returns></returns>
        public Grid DataBindDataViewGrid(DataView dataView, List<Column> columns, string gridId, string pagerId, bool editable, bool allowAddNewRow)
        {
            // Always add an empty column on the end for reszing purposes
            ArrayEx.Add(columns, new Column());

            GridOptions gridOptions = new GridOptions();
            gridOptions.EnableCellNavigation = true;
            gridOptions.AutoEdit = editable;
            gridOptions.Editable = editable;
            gridOptions.EnableAddRow = allowAddNewRow;

            // Set non-variable options
            gridOptions.RowHeight = 20;
            gridOptions.HeaderRowHeight = 25;
            gridOptions.EnableColumnReorder = false;

            CheckboxSelectColumn checkBoxSelector = null;
            if (AddCheckBoxSelectColumn)
            {
                CheckboxSelectColumnOptions checkboxOptions = new CheckboxSelectColumnOptions();
                checkboxOptions.cssClass = "sparkle-checkbox-column";
                // Add check box column
                checkBoxSelector = new CheckboxSelectColumn(checkboxOptions);
                Column checkBoxColumn = checkBoxSelector.GetColumnDefinition();
                columns.Insert(0, checkBoxColumn);
            }

            Grid grid = new Grid("#" + gridId, dataView, columns, gridOptions);

            grid.RegisterPlugin(checkBoxSelector);


            dataView.OnRowsChanged.Subscribe(delegate(EventData e, object a)
            {
                // Only invalided the rows that have changed
                OnRowsChangedEventArgs args = (OnRowsChangedEventArgs)a;
                if (args != null && args.Rows != null)
                {
                    grid.InvalidateRows(args.Rows);
                    grid.Render();
                }
            });


            //AddValidation(grid, dataView);


            // Add resize event
            jQuery.Window.Resize(delegate(jQueryEvent e)
            {
                // Set each column to be non resizable while we do the resize
                GridDataViewBinder.FreezeColumns(grid, true);
                grid.ResizeCanvas();
                // Restore the resizing 
                GridDataViewBinder.FreezeColumns(grid, false);
            });

            // Add Reset binding
            Action reset = delegate() { };

            Script.Literal("{0}.reset={1}", dataView, reset);

            // Add Refresh button
            AddRefreshButton(gridId, (DataViewBase)(object)dataView);


            // Add Selection Model
            RowSelectionModelOptions selectionModelOptions = new RowSelectionModelOptions();
            selectionModelOptions.SelectActiveRow = true;
            RowSelectionModel selectionModel = new RowSelectionModel(selectionModelOptions);
            grid.SetSelectionModel(selectionModel);

            // Set sorting
            Action<EventData,object> onSort = delegate (EventData e, object a)
            {
                SortColData args = (SortColData)a;
                //SortDir = args.SortAsc ? 1 : -1;
                _sortColumnName = args.SortCol.Field;
                dataView.Sort(Comparer, args.SortAsc);
            };
            grid.OnSort.Subscribe(onSort);

            return grid;

        }
        public int Comparer(object l, object r)
        {
            Dictionary<string, object> a = (Dictionary<string, object>)l;
            Dictionary<string, object> b = (Dictionary<string, object>)r;
            object x = a[_sortColumnName], y = b[_sortColumnName];
            return (x == y ? 0 : ((bool)Script.Literal("{0} > {1}", x, y) ? 1 : -1));
        }

        /// <summary>
        /// Binds the click handler for opening records from the grid attributes -see the formatters for attributes provided
        /// </summary>
        /// <param name="grid"></param>
        public void BindClickHandler(Grid grid)
        {
            Action<string, string> openEntityRecord = delegate(string logicalName, string id)
            {
                Utility.OpenEntityForm(logicalName, id, null);
            };
            grid.OnClick.Subscribe(delegate(EventData e, object sender)
            {
                CellSelection cell = (CellSelection)sender;

                bool handled = false;
                Element element = e.target;
                object logicalName = element.GetAttribute("typename");
                if (logicalName == null)
                    logicalName = element.GetAttribute("logicalName");

                object id = element.GetAttribute("entityid");
                if (id == null)
                    id = element.GetAttribute("id");

                object primaryNameLookup = element.GetAttribute("primaryNameLookup");

                if (logicalName != null & id != null)
                {
                    // Open the related record
                    handled = true;

                }
                else if (primaryNameLookup != null)
                {
                    // Open the primary entity record
                    handled = true;
                    Entity entity = (Entity)cell.Grid.GetDataItem(cell.Row.Value);
                    logicalName = entity.LogicalName;
                    // If there is an activitytypecode then use that
                    string activitytypecode = entity.GetAttributeValueString("activitytypecode");
                    if (activitytypecode != null)
                        logicalName = activitytypecode;
                    id = entity.Id;
                }

                if (handled)
                {
                    openEntityRecord((string)logicalName, (string)id);
                    e.StopImmediatePropagation();
                    e.StopPropagation();
                }

            });
            grid.OnDblClick.Subscribe(delegate(EventData e, object sender)
            {

                CellSelection cell = (CellSelection)sender;
                Entity entity = (Entity)cell.Grid.GetDataItem(cell.Row.Value);
                string logicalName = entity.LogicalName;
                // If there is an activitytypecode then use that
                string activitytypecode = entity.GetAttributeValueString("activitytypecode");
                if (activitytypecode != null)
                    logicalName = activitytypecode;
                openEntityRecord(logicalName, entity.Id);
                e.StopImmediatePropagation();
                e.StopPropagation();
            });
           

        }
        private static void FreezeColumns(Grid grid, bool freeze)
        {
            // Columns are added initially with their max and min width the same so they are not stretched to fit the width
            // Now we restore column resizing 
            Column[] cols = grid.GetColumns();
            for (int i = 0; i < cols.Length - 1; i++)
            {
                Column col = cols[i];
                if (freeze)
                {
                    col.MaxWidth = col.Width;
                    col.MinWidth = col.Width;
                }
                else
                {
                    col.MaxWidth = null;
                    col.MinWidth = null;
                }
            }
        }
        public void AddValidation(Grid grid, DataViewBase dataView)
        {
            Action<string, Column> setValidator = delegate(string attributeName, Column col)
            {
                col.Validator = delegate(object value, object item)
                {
                    Func<string,GridValidatorDelegate> indexer = dataView.GridValidationIndexer();
                    GridValidatorDelegate validationRule = indexer(attributeName);
                    if (validationRule != null)
                        return validationRule(value, item);
                    else
                    {
                        ValidationResult result = new ValidationResult();
                        result.Valid = true;
                        return result;
                    }
                };
            };

            if (dataView.GridValidationIndexer() != null)
            {
                foreach (Column col in grid.GetColumns())
                {
                    string fieldName = col.Field;
                    setValidator(fieldName, col);
                }
            }
        }
        
        public void DataBindSelectionModel(Grid grid, DataViewBase dataView)
        {
            // Set up selection model if needed
            // Create selection model
            
            RowSelectionModelOptions selectionModelOptions = new RowSelectionModelOptions();
            selectionModelOptions.SelectActiveRow = SelectActiveRow;
            selectionModelOptions.MultiRowSelect = this.MultiSelect;
            RowSelectionModel selectionModel = new RowSelectionModel(selectionModelOptions);

            // Bind two way sync of selected rows
            // NOTE: the row index on the grid is not the row index in the data view due to paging
            bool inHandler = false;
            selectionModel.OnSelectedRangesChanged.Subscribe(delegate(EventData e, object args)
            {
                //if (grid.GetEditorLock().IsActive())
                //{
                //    e.StopPropagation();
                //    return;
                //}
                if (inHandler)
                    return;
                inHandler = true;
                // Has the selected row changeD?
                SelectedRange[] selectedRows = dataView.GetSelectedRows();
                SelectedRange[] newSelectedRows = (SelectedRange[])args;
                bool changed = selectedRows.Length!=newSelectedRows.Length;
                if (!changed)
                {
                    // Compare the actual selected rows
                    for (int i = 0; i < selectedRows.Length; i++)
                    {
                        if (selectedRows[i].FromRow!=newSelectedRows[i].FromRow)
                        {
                            changed = true;
                            break;
                        }
                    }

                }
                
               
               
                if (changed)
                {
                    dataView.RaiseOnSelectedRowsChanged(newSelectedRows);
                }
                inHandler = false;
            });
            dataView.OnSelectedRowsChanged+=delegate()
            {
                //if (grid.GetEditorLock().IsActive())
                //    return;
                if (inHandler)
                    return;
                inHandler = true;
                SelectedRange[] ranges = dataView.GetSelectedRows();
                int[] selectedRows = new int[ranges.Length];
                for (int i=0;i<selectedRows.Length;i++)
                {
                    selectedRows[i] = ranges[i].FromRow.Value;
                }
               
                grid.SetSelectedRows(selectedRows);
                
                
                inHandler = false;
            };
            grid.SetSelectionModel(selectionModel);

        }

      
        public void AddRefreshButton(string gridId, DataViewBase dataView)
        {
            jQueryObject gridDiv = jQuery.Select("#" + gridId);
            jQueryObject refreshButton = jQuery.FromHtml("<div id='refreshButton' class='sparkle-grid-refresh-button' style='left: auto; right: 0px; display: inline;'><a href='#' id='refreshButtonLink' tabindex='0'><span id='grid_refresh' class='sparkle-grid-refresh-button-img' style='cursor:pointer'></span></a></div>").AppendTo(gridDiv);
            refreshButton.Find("#refreshButtonLink").Click(delegate(jQueryEvent e)
            {
                dataView.Reset();
                dataView.Refresh();
            });                        
        }
       
      
        public void DataBindEvents(Grid grid,DataViewBase dataView,string gridContainerDivId)
        {
            // Data Sorting
            grid.OnSort.Subscribe(delegate(EventData o, Object item)
            {
                SortColData sorting = (SortColData)item;
                dataView.Sort(sorting);
                grid.Invalidate();
                grid.Render();

            });

            // Session Grid DataBinding
            grid.OnAddNewRow.Subscribe(delegate(EventData o, Object item)
            {
                EditEventData data = (EditEventData)item;
                dataView.AddItem(data.item);


                Column column = data.column;
                grid.InvalidateRow(dataView.GetLength() - 1);
              
                grid.UpdateRowCount();
                grid.Render();


            });

            dataView.OnRowsChanged.Subscribe(delegate(EventData e, object a)
            {

                OnRowsChangedEventArgs args = (OnRowsChangedEventArgs)a;
                if (args != null && args.Rows != null)
                {
                    grid.InvalidateRows(args.Rows);
                    grid.Render();
                }
                else
                {
                    // Assume that a new row has been added
                    grid.InvalidateRow(dataView.GetLength());
                    grid.UpdateRowCount();
                    grid.Render();
                }
                grid.ResizeCanvas();
            });


            jQueryObject loadingIndicator = null;


            // Wire up the validation error
            jQueryObject validationIndicator = null;
            Action<EventData, object> clearValidationIndicator = delegate(EventData e, object a)
            {
                if (validationIndicator != null)
                {
                    validationIndicator.Hide();
                    validationIndicator.Remove();
                }
            };

            grid.OnCellChange.Subscribe(clearValidationIndicator);
            grid.OnActiveCellChanged.Subscribe(clearValidationIndicator);
            grid.OnBeforeCellEditorDestroy.Subscribe(clearValidationIndicator);

            grid.OnValidationError.Subscribe(delegate(EventData e, object a)
            {
                ValidationEventArgs args = (ValidationEventArgs)a;
                ValidationResult validationResult = (ValidationResult)args.ValidationResults;
                jQueryObject activeCellNode = (jQueryObject)args.CellNode;
                object editor = args.Editor;
                string errorMessage = "";
                if (validationResult.Message != null)
                    errorMessage = validationResult.Message;
                bool valid_result = validationResult.Valid;

                // Add the message to the tooltip on the cell
                if (!valid_result)
                {
                    jQuery.FromObject(activeCellNode).Attribute("title", errorMessage);
                    clearValidationIndicator(e,a);
                    validationIndicator = jQuery.FromHtml("<div class='popup-box-container'><div width='16px' height='16px' class='sparkle-imagestrip-inlineedit_warning popup-box-icon' alt='Error' id='icon'/><div class='popup-box validation-text'/></div>").AppendTo(Document.Body);
                    validationIndicator.Find(".validation-text").Text(errorMessage);

                    string colisionPosition = ValidationPopupUseFitPosition ? "fit fit" : "none none";
                    Script.Literal(@"{0}.position({{
                                            my: 'left bottom',
                                            at: 'left top',
                                            collision: '{2}',
                                            of: {1}
                                        }})
                                        .show({{
                                        effect: 'blind'
                                        }})
                                        .delay( 500000 )
                                        .hide({{
                                            effect: 'fade',
                                            duration: 'slow', 
                                        }},
                                            function() {{
                                                $( this ).remove();
                                                
                                            }});
                                        ", validationIndicator, activeCellNode, colisionPosition); 


                }
                else
                {
                    clearValidationIndicator(e, a);
                    jQuery.FromObject(activeCellNode).Attribute("title", "");
                }
            });

            // Wire up the loading spinner
            dataView.OnDataLoading.Subscribe(delegate(EventData e, object a)
            {

                loadingIndicator = ShowLoadingIndicator(loadingIndicator, gridContainerDivId);
                foreach (Column col in grid.GetColumns())
                {
                    if (col.MaxWidth != null)
                        col.MaxWidth = 400;
                }

            });

            dataView.OnDataLoaded.Subscribe(delegate(EventData e, object a)
            {
                // Sync the sorted columns
                SortColData[] sortCols = grid.GetSortColumns();
                bool noGridSort = sortCols == null || sortCols.Length == 0;

                SortCol[] viewSortCols = dataView.GetSortColumns();
                bool noViewCols = viewSortCols == null || viewSortCols.Length == 0;

                if (noGridSort && !noViewCols)
                {
                    // Set grid sort
                    grid.SetSortColumn(viewSortCols[0].AttributeName, viewSortCols[0].Ascending);
                }

                DataLoadedNotifyEventArgs args = (DataLoadedNotifyEventArgs)a;
                if (args != null)
                {
                    if (args.ErrorMessage == null)
                    {
                        for (int i = args.From; i <= args.To; i++)
                        {
                            grid.InvalidateRow(i);
                        }
                        grid.UpdateRowCount();
                        grid.Render();
                    }
                    else
                        Script.Alert("There was a problem refreshing the grid.\nPlease contact your system administrator:\n" + args.ErrorMessage);
                }
                if (loadingIndicator != null)
                    loadingIndicator.Plugin<jQueryBlockUI>().Unblock();
            });

            // Wire up edit complete to property changed
            grid.OnCellChange.Subscribe(delegate(EventData e, object data)
            {
                OnCellChangedEventData eventData = (OnCellChangedEventData)data;
                dataView.RaisePropertyChanged("");

            });
            
           
        }

        private static jQueryObject ShowLoadingIndicator(jQueryObject loadingIndicator, string gridContainerDivId)
        {

            jQueryObject g = jQuery.Select("#" + gridContainerDivId);
            jQueryObject vp = jQuery.Select("#" + gridContainerDivId + " > .slick-viewport");
            loadingIndicator = g;
            jQueryBlockUIOptions blockOpts = new jQueryBlockUIOptions();
            blockOpts.ShowOverlay = false;
            blockOpts.IgnoreIfBlocked = true;

            BlockUICSS css = new BlockUICSS();
            css.border = "0px";
            css.backgroundColor = "transparent";
            //css.width = "100px";
            //css.height = "100px";

            OverlayCSS overlayCss = new OverlayCSS();
            
            overlayCss.Opacity = "0";
            //blockOpts.OverlayCSS = overlayCss;
            blockOpts.Css = css;
            blockOpts.Message = "<span class='loading-indicator'><label>Loading...</label></span>";
            loadingIndicator.Plugin<jQueryBlockUI>().Block(blockOpts);

            return loadingIndicator;
        }
        public static Column AddColumn(List<Column> cols, string displayName, int width, string field)
        {
            Column col = NewColumn(field, displayName, width);
            ArrayEx.Add(cols, col);
            return col;
        }

        public static List<Column> ParseLayout(string layout)
        {

            string[] layoutParts = layout.Split(',');
            List<Column> cols = new List<Column>();

            for (int i = 0; i < layoutParts.Length; i = i + 3)
            {
                string field = layoutParts[i + 1];
                string name = layoutParts[i];
                int width =int.Parse(layoutParts[i + 2]);
                Column col = NewColumn(field, name, width);
                ArrayEx.Add(cols, col);
            }
            return cols;
        }

        public static Column NewColumn(string field, string name, int width)
        {
            Column col = new Column();
            col.Id = field; // The id should be the attribute name not the display label.
            col.Name = name;
            col.Width = width;
            col.MinWidth = col.Width;
            col.Field = field;
            col.Sortable = true;
            col.Formatter = ColumnFormatter;
            return col;
        }
        public static string ColumnFormatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            string typeName;
            string returnValue = String.Empty;

            if (columnDef.DataType != null)
                typeName = columnDef.DataType;
            else
                typeName =  value.GetType().Name;

            Entity entityContext = (Entity)dataContext;
            bool unchanged = (entityContext.EntityState==null) || (entityContext.EntityState == EntityStates.Unchanged);
            
            // If unchanged we can get values from the formatted values
            if (unchanged && entityContext.FormattedValues!=null && entityContext.FormattedValues.ContainsKey(columnDef.Field+"name"))
            {
                returnValue = entityContext.FormattedValues[columnDef.Field + "name"];
                return returnValue;
            }
            if (value != null)
            {
                switch (typeName.ToLowerCase())
                {

                    case "string":
                        returnValue = value.ToString();
                        break;
                    case "boolean":
                    case "bool":
                        returnValue = value.ToString();
                        break;
                    case "dateTime":
                    case "date":
                        DateTime dateValue = (DateTime)value;
                        string dateFormat = "dd/mm/yy";
                        string timeFormat = "hh:MM";
                        if (OrganizationServiceProxy.UserSettings != null)
                        {
                            dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString;
                            timeFormat = OrganizationServiceProxy.UserSettings.TimeFormatString;
                        }
                        returnValue = DateTimeEx.FormatDateSpecific(dateValue, dateFormat) + " " + DateTimeEx.FormatTimeSpecific(dateValue, timeFormat);
                        break;
                    case "decimal":
                        returnValue = value.ToString();
                        break;
                    case "double":
                        returnValue = value.ToString();
                        break;
                    case "int":
                        returnValue = value.ToString();
                        break;
                    case "guid":
                        returnValue = value.ToString();
                        break;
                    case "money":
                        Money moneyValue = (Money)value;

                        returnValue = moneyValue.Value.ToString();
                        break;
                    case "customer":
                    case "owner":
                    case "lookup":
                    case "entityreference":
                        EntityReference refValue = (EntityReference)value;
                        returnValue = @"<a class=""sparkle-grid-link"" href=""#"" logicalName=""" + refValue.LogicalName + @""" id=""" + refValue.Id + @""">" + refValue.Name + "</a>";
                        break;
                    case "picklist":
                    case "status":
                    case "state":
                    case "optionsetvalue":
                        OptionSetValue optionValue = (OptionSetValue)value;
                        returnValue = optionValue.Name;
                        break;
                    case "primarynamelookup":
                        string lookupName = value == null ? "" : value.ToString();
                        returnValue = @"<a class=""sparkle-grid-link"" href=""#"" primaryNameLookup=""1"">" + lookupName + "</a>";
                        break;
                }
            }
            return returnValue;



        }
        public static Column BindRowIcon(Column column,string entityLogicalName)
        {
            column.Formatter = RowIcon;
            column.Options = entityLogicalName;
            return column;
        }
        /// <summary>
        /// Formattor to get the icon for a row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        /// <param name="columnDef"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public static string RowIcon(int row, int cell, object value, Column columnDef, object dataContext)
        {
            // Get the icon of the row using the logical name of the specified entity reference column by the format string
            Dictionary<string, object> item = (Dictionary<string, object>)dataContext;
            if (item == null)
                return "";
            else
            {
                // Get the entity type from the specified column
                EntityReference lookup = (EntityReference)item[(string)columnDef.Options];
                if (lookup == null || lookup.LogicalName == null)
                    return "";
                else
                    return "<span class='sparkle-grid-row-img'><img src='" + MetadataCache.GetSmallIconUrl(lookup.LogicalName) + "'/></span>";
            }
        }

        /// <summary>
        /// Adds a column that shows the state of the row as edited/new
        /// </summary>
        /// <param name="columns"></param>
        public static void AddEditIndicatorColumn(List<Column> columns)
        {
            GridDataViewBinder.AddColumn(columns, "", 20, "entityState")
                .Formatter = delegate(int row, int cell, object value, Column columnDef, object dataContext)
                {
                    EntityStates state = (EntityStates)value;
                    switch (state)
                    {
                        case EntityStates.Created:
                        case EntityStates.Changed:
                            return "<span class='grid-edit-indicator'></span>";
                        case EntityStates.ReadOnly:
                            return "<span class='grid-readonly-indicator'></span>";
                        default:
                            return "";
                    }
                };
        }
 
        /// <summary>
        /// Wire up the OnCommitEdit event handler for the grid
        /// In order to ensure that all grid edits have been commited before a VM command is run,
        /// the VM must call CommitEdit on the ViewModelBase and cancel if returns false.
        /// </summary>
        /// <param name="vm"></param>
        public void BindCommitEdit(ViewModelBase vm)
        {
            vm.OnCommitEdit += delegate(ViewModelBase sender, CancelEventArgs e)
            {
                if (_grid.GetEditorLock().IsActive())
                    e.Cancel = !_grid.GetEditorLock().CommitCurrentEdit();
            };
        }
    }
}
