// MultiSearchView.cs
//

using Client.MultiEntitySearch.ViewModels;
using jQueryApi;
using KnockoutApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Html;
using Xrm;
using Xrm.Sdk;

namespace Client.MultiEntitySearch.Views
{
    public class MultiSearchView2013
    {
        public static void init()
        {
            MultiSearchViewModel2013 vm = new MultiSearchViewModel2013();
           

            // Create Grids
            FetchQuerySettings[] searches = vm.Config.GetItems();
          
            int i = 0;
            foreach (FetchQuerySettings config in searches)
            {
                
                

                List<Column> cardColumn = new List<Column>(new Column(ColumnProperties.Id, "card-column", ColumnProperties.Options,config.Columns, ColumnProperties.Name, "Name", ColumnProperties.Width, 290, ColumnProperties.CssClass, "card-column-cell"));
                cardColumn[0].Formatter = RenderCardColumnCell;
                cardColumn[0].DataType = "PrimaryNameLookup"; // This is so that clicking on the column opens the record
                GridDataViewBinder dataViewBinder = new GridDataViewBinder();
                GridOptions gridOptions = new GridOptions();
                gridOptions.EnableCellNavigation = true;
                gridOptions.AutoEdit = false;
                gridOptions.Editable = false;
                gridOptions.EnableAddRow = false;
                // Set height to the number of columns
                int columns = config.Columns.Count;             
                gridOptions.RowHeight = (columns>3? 3 : columns) * 16; ;
                if (gridOptions.RowHeight < 70) gridOptions.RowHeight = 70;
                gridOptions.HeaderRowHeight = 0;
                
                string gridId = "grid" + i.ToString() + "container";
                DataViewBase dataView = config.DataView;

                Grid grid = new Grid("#" + gridId, dataView, cardColumn, gridOptions);
                AddResizeEventHandlers(grid, gridId);
                dataViewBinder.DataBindEvents(grid, dataView, gridId);
                dataViewBinder.BindClickHandler(grid);
                i++;

              
            
        
            }
          
            // Data Bind
            ViewBase.RegisterViewModel(vm);

        }

        private static void AddResizeEventHandlers(Grid grid,string containerName)
        {
            // Add resize height event
            jQuery.Window.Resize(delegate(jQueryEvent e)
            {
                ResizeGrid(grid, containerName);
            });
            jQuery.OnDocumentReady(delegate()
            {
                ResizeGrid(grid, containerName);
            });
        }

        private static void ResizeGrid(Grid grid,string containerName)
        {
            // Change the height
            int height = jQuery.Window.GetHeight();
            jQuery.Select("#" + containerName).Height(height - 60);
            grid.ResizeCanvas();
        }
        public static string RenderCardColumnCell(int row, int cell, object value, Column columnDef, object dataContext)
        {
            List<Column> columns = (List<Column>)columnDef.Options;
            Entity record = (Entity)dataContext;
            string cardHtml = "";
            bool firstRow = true;
            string imageUrl = record.GetAttributeValueString("card_image_url");
            string imageHtml;
            if (imageUrl != null)
            {
                imageHtml = @"<img class='entity-image' src='" + Page.Context.GetClientUrl() + imageUrl + @"'/>";
            }
            else
            {
                string typeName = record.LogicalName;
                if (typeName=="activitypointer")
                {
                    string activitytypecode = record.GetAttributeValueString("activitytypecode");
                    typeName = activitytypecode;
                }
                imageHtml = @"<div class='record_card " + typeName + @"_card'><img src='..\..\sparkle_\css\images\transparent_spacer.gif'\></dv>";
            }


            // Only show the first 3 columns from the Quick Find View
            int rowIndex = 0;
            cardHtml = "<table class='contact-card-layout'><tr><td>" + imageHtml +"</td><td>";
            foreach (Column col in columns)
            {
                if (col.Field != "activitytypecode")
                {
                    object fieldValue = record.GetAttributeValue(col.Field);
                    string dataFormatted = col.Formatter(row, cell, fieldValue,col, dataContext);

                    cardHtml += "<div " + (firstRow ? "class='first-row'" : "") + " tooltip='" + fieldValue + "'>" + dataFormatted + "</div>";
                    firstRow = false;
                    rowIndex++;
                }
                if (rowIndex > 3)
                    break;
            }
            cardHtml += "</tr></table>";
            return cardHtml;
        }
        
    }
}
