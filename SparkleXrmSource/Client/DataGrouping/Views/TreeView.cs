// TreeView.cs
//

using Client.DataGrouping.ViewModels;
using jQueryApi;
using Slick;
using Slick.Data;
using SparkleXrm;
using SparkleXrm.CustomBinding;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using Xrm;

namespace Client.DataGrouping.Views
{
    public class TreeView
    {
        public static void init()
        {
            TreeDataViewModel vm = new TreeDataViewModel();
            GridDataViewBinder binder = new GridDataViewBinder();

             List<Column> columns = new List<Column>(
               
                new Column(
                    ColumnProperties.Id, "status",
                    ColumnProperties.Name, "Status",
                    ColumnProperties.Field, "status",
                    ColumnProperties.Width, 70,
                    ColumnProperties.MinWidth, 50,
                    ColumnProperties.CssClass, "cell-title",
                    ColumnProperties.Sortable, true
                   ),
                 new Column(
                    ColumnProperties.Id, "project",
                    ColumnProperties.Name, "Project",
                    ColumnProperties.Field, "project",
                    ColumnProperties.Width, 70,
                    ColumnProperties.MinWidth, 50,
                    ColumnProperties.CssClass, "cell-title",
                    ColumnProperties.Sortable, true,
                    ColumnProperties.Editor, XrmTextEditor.TextEditor),
                   new Column(
                    ColumnProperties.Id, "date",
                    ColumnProperties.Name, "Date",
                    ColumnProperties.Field, "date",
                    ColumnProperties.Width, 70,
                    ColumnProperties.Sortable, true
                    ),
                new Column(
                    ColumnProperties.Id, "employee",
                    ColumnProperties.Name, "Employee",
                    ColumnProperties.Field, "employee",
                    ColumnProperties.Width, 80,
                    ColumnProperties.Sortable, true
                  ),
                new Column(
                    ColumnProperties.Id, "duration",
                    ColumnProperties.Name, "Duration",
                    ColumnProperties.Field, "duration",
                    ColumnProperties.MinWidth, 60,
                    ColumnProperties.Sortable, true)
               );

            vm.Items.OnGetItemMetaData += Items_OnGetItemMetaData;
            Grid grid = binder.DataBindXrmGrid(vm.Items,columns,"projectsGrid",null,true,false);
            grid.RegisterPlugin(new GroupGridRowPlugin());
            // Data Bind
            ViewBase.RegisterViewModel(vm);
        }

        static ItemMetaData Items_OnGetItemMetaData(object item)
        {
           
            Group group = (Group)item;
            if (group.Level != null)
            {
                return GetGroupRowMetadata(item);
            }
            else return null;
        }

        public static ItemMetaData GetGroupRowMetadata(object item)
        {

             ItemMetaData metaData = new ItemMetaData();
             metaData.Selectable = false;
             metaData.Focusable = true;
             metaData.CssClasses =  "slick-group";
             metaData.Columns = new Dictionary<object,Column>();
             Column col = new Column();
             metaData.Columns[0] = col;
             col.ColSpan = "*";
             col.Formatter = GroupCellFormatter;
             col.Editor = null;
             return metaData;
         }

        public static string GroupCellFormatter(int row, int cell, object value, Column columnDef, object dataContext)
        {
            Group item = (Group)dataContext;

            string indentation = (item.Level * 15).ToString() + "px";
            return "<span class='slick-group-toggle " +
                (item.Collapsed ? "collapsed" : "expanded") +
                "' style='margin-left:" + indentation + "'>" +
                "</span>" +
                "<span class='slick-group-title' level='" + item.Level + "'>" +
                  item.Title +
                "</span>";
        }
        
    }
}
