// DataGroupingView.cs
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
    public class DataGroupingView
    {
        public static void init()
        {
            DataGroupingViewModel vm = new DataGroupingViewModel();
            NumberFormatInfo numberFormatInfo = NumberEx.GetNumberFormatInfo();
            numberFormatInfo.MinValue = 0;
            numberFormatInfo.MaxValue = 1000;
            numberFormatInfo.Precision = 2;
            BooleanBindingOptions boolFormatInfo = new BooleanBindingOptions();
            boolFormatInfo.FalseOptionDisplayName = "No";
            boolFormatInfo.TrueOptionDisplayName = "Yes";

            // Add Columns
            SubTotalsFormatterDelegate sumTotalsFormatterDelegate = SumTotalsFormatter;
            SubTotalsFormatterDelegate avgTotalsFormatterDelegate = AvgTotalsFormatter;
            FormatterDelegate percentageFormatter = XrmNumberEditor.Formatter;
            FormatterDelegate checkboxFormatter = XrmBooleanEditor.Formatter;

            List<Column> columns = new List<Column>(
                new Column(
                    ColumnProperties.Id, "sel",
                    ColumnProperties.Name, "#",
                    ColumnProperties.Field, "num",
                    ColumnProperties.CssClass, "cell-selection",
                    ColumnProperties.Width, 40,
                    ColumnProperties.Resizable, false,
                    ColumnProperties.Selectable, false,
                    ColumnProperties.Focusable, false),
                new Column(
                    ColumnProperties.Id, "title",
                    ColumnProperties.Name, "Title",
                    ColumnProperties.Field, "title",
                    ColumnProperties.Width, 70,
                    ColumnProperties.MinWidth, 50,
                    ColumnProperties.CssClass, "cell-title",
                    ColumnProperties.Sortable, true,
                    ColumnProperties.Editor, XrmTextEditor.TextEditor),
                   new Column(
                    ColumnProperties.Id, "duration",
                    ColumnProperties.Name, "Duration",
                    ColumnProperties.Field, "duration",
                    ColumnProperties.Width, 70,
                    ColumnProperties.Sortable, true,
                    ColumnProperties.GroupTotalsFormatter, sumTotalsFormatterDelegate),
                new Column(
                    ColumnProperties.Id, "%",
                    ColumnProperties.Name, "% Complete",
                    ColumnProperties.Field, "percentComplete",
                    ColumnProperties.Width, 80,
                    ColumnProperties.Formatter, percentageFormatter,
                    ColumnProperties.Options, numberFormatInfo,
                    ColumnProperties.Sortable, true,
                    ColumnProperties.GroupTotalsFormatter, avgTotalsFormatterDelegate),
                new Column(
                    ColumnProperties.Id, "start",
                    ColumnProperties.Name, "Start",
                    ColumnProperties.Field, "start",
                    ColumnProperties.MinWidth, 60,
                    ColumnProperties.Sortable, true),
                new Column(
                    ColumnProperties.Id, "finish",
                    ColumnProperties.Name, "Finish",
                    ColumnProperties.Field, "finish",
                    ColumnProperties.MinWidth, 60,
                    ColumnProperties.Sortable, true),
                new Column(
                    ColumnProperties.Id, "cost",
                    ColumnProperties.Name, "Cost",
                    ColumnProperties.Field, "cost",
                    ColumnProperties.Width, 90,
                    ColumnProperties.Sortable, true,
                    ColumnProperties.GroupTotalsFormatter, sumTotalsFormatterDelegate),
                new Column(
                    ColumnProperties.Id, "effort-driven",
                    ColumnProperties.Name, "Effort Driven",
                    ColumnProperties.Width, 80,
                    ColumnProperties.MinWidth, 20,
                    ColumnProperties.CssClass, "cell-effort-driven",
                    ColumnProperties.Field, "effortDriven",
                    ColumnProperties.Formatter, checkboxFormatter,
                    ColumnProperties.Options, boolFormatInfo,
                    ColumnProperties.Sortable, true)
               );

            GridOptions options = new GridOptions();

            options.EnableCellNavigation = true;
            options.Editable = true;
            DataViewBase view = (DataViewBase)(object)vm.Projects;

            GridDataViewBinder binder = new GridDataViewBinder();
            Grid grid = binder.DataBindDataViewGrid(vm.Projects, columns, "myGrid", null, true, false);

            // register the group item metadata provider to add expand/collapse group handlers
            grid.RegisterPlugin(new GroupItemMetadataProvider());
            
            // Data Bind
            ViewBase.RegisterViewModel(vm);
        }
       
        public static string SumTotalsFormatter(Dictionary<string,object> totals, Column columnDef) {

            Dictionary<string, object> sum = (Dictionary<string, object>)totals["sum"];

            float? val = (sum != null) ? (float?)sum[columnDef.Field] : null;
            if (val != null)
            {
                return "avg: " + Math.Round(val).ToString() + " %" ;
            }
            return "";
            
        }

        public static string AvgTotalsFormatter(Dictionary<string, object> totals, Column columnDef)
        {
            Dictionary<string, object> avg = (Dictionary<string, object>)totals["avg"];

            float? val = (avg!=null) ? (float?)avg[columnDef.Field] : null;
            if (val != null)
            {
                return "total: " + ((Math.Round(val * 100) / 100));
            }
            return "";
            
        }
       
    }
}
