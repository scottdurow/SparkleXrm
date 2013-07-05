// TimeSheetView.cs
//

using System;
using System.Collections.Generic;
using SparkleXrm;
using Client.TimeSheet.ViewModel;
using KnockoutApi;
using jQueryApi;
using Slick;
using jQueryApi.UI.Widgets;
using Xrm.Sdk;
using SparkleXrm.jQueryPlugins;
using SparkleXrm.GridEditor;
using System.Html;
using System.Runtime.CompilerServices;
using System.Collections;

namespace Client.TimeSheet.View
{
    public class TimeSheetView : ViewBase
    {
        private static Grid daysGrid;
        private static Grid sessionsGrid;
        public static void Init()
        {
            jQuery.OnDocumentReady(delegate()
          {

              ValidationApi.RegisterExtenders();

              // Init settings
              OrganizationServiceProxy.GetUserSettings();

              TimeSheetViewModel vm = new TimeSheetViewModel();


              SetUpGrids(vm);
              SetUpDatePicker(vm);

              ViewBase.RegisterViewModel(vm);
          });
        }

        private static void SetUpDatePicker(TimeSheetViewModel vm)
        {
            jQueryObject element = jQuery.Select("#datepicker");
            DatePickerOptions options = new DatePickerOptions();
            options.NumberOfMonths = 3;
            options.CalculateWeek = true;

            string dateFormat = "dd/MM/yy";
            if (OrganizationServiceProxy.UserSettings != null)
            {
                dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString;
            }
            options.DateFormat = dateFormat.Replace("MM", "mm").Replace("yyyy", "yy").Replace("M", "m");

            DatePickerOptions2 options2 = new DatePickerOptions2();
            options2.NumberOfMonths = 3;
            options2.DateFormat = dateFormat.Replace("MM", "mm").Replace("yyyy", "yy").Replace("M", "m");


            // Wire up onSelect event
            options2.OnSelect = delegate(string dateText, object instance)
            {
                // Commit any of the current edits
                EditController controller = sessionsGrid.GetEditController();
                bool editCommited = controller.commitCurrentEdit();
                if (editCommited)
                {
                    DateTime date = (DateTime)element.Plugin<DatePickerObject>().DatePicker(DatePickerMethod.GetDate);
                    vm.SessionDataView.SetCurrentWeek(date);
                }
            };

            element.Plugin<DatePickerPlugIn>().DatePicker(options2);


        }

        public static void SetUpGrids(TimeSheetViewModel vm)
        {
            GridOptions daysGridOpts = new GridOptions();
            daysGridOpts.EnableCellNavigation = true;
            daysGridOpts.EnableColumnReorder = false;
            daysGridOpts.AutoEdit = false;
            daysGridOpts.Editable = true;
            daysGridOpts.RowHeight = 20;
            daysGridOpts.HeaderRowHeight = 25;
            daysGridOpts.ForceFitColumns = false;
            daysGridOpts.EnableAddRow = true;



            // Create Timesheet Grid
            DataViewBase daysDataView = vm.Days;

            List<Column> columns = new List<Column>();
            GridDataViewBinder.BindRowIcon(GridDataViewBinder.AddColumn(columns, "", 50, "icon"), "activity");
            XrmLookupEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Activity", 300, "activity"), vm.ActivitySearchCommand, "activityid", "subject", "activitytypecode");

            GridDataViewBinder.AddColumn(columns, "Mon", 50, "day0");
            GridDataViewBinder.AddColumn(columns, "Tue", 50, "day1");
            GridDataViewBinder.AddColumn(columns, "Wed", 50, "day2");
            GridDataViewBinder.AddColumn(columns, "Thu", 50, "day3");
            GridDataViewBinder.AddColumn(columns, "Fri", 50, "day4");
            GridDataViewBinder.AddColumn(columns, "Sat", 50, "day5");
            GridDataViewBinder.AddColumn(columns, "Sun", 50, "day6");

            daysGrid = new Grid("#timesheetGridContainer", daysDataView, columns, daysGridOpts);

            GridDataViewBinder daysDataBinder = new GridDataViewBinder();
            daysDataBinder.DataBindEvents(daysGrid, daysDataView, "timesheetGridContainer");

            // Set the totals row meta data
            daysDataView.OnGetItemMetaData += delegate(object item)
            {
                DayEntry day = (DayEntry)item;
                if (day != null && day.isTotalRow)
                {
                    ItemMetaData metaData = new ItemMetaData();
                    metaData.Editor = null;
                    metaData.Formatter = delegate(int row, int cell, object value, Column columnDef, object dataContext)
                    {
                        if (columnDef.Field == "activity")
                            return "Total";
                        else
                            return Formatters.DefaultFormatter(row, cell, value, columnDef, dataContext);
                    };
                    metaData.CssClasses = "days_total_row";
                    return metaData;
                }

                else
                    return null;
            };

            daysDataBinder.DataBindSelectionModel(daysGrid, daysDataView);


            // ---------------------------------------
            // Sessions Grid
            // ---------------------------------------
            DataViewBase sessionsDataView = vm.SessionDataView;

            List<Column> sessionGridCols = new List<Column>();

            GridDataViewBinder.AddEditIndicatorColumn(sessionGridCols);

            XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Description", 200, "dev1_description"));

            XrmDateEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Date", 200, "dev1_starttime"), true);

            XrmTimeEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Start", 100, "dev1_starttime")).Validator =
                delegate(object value, object item)
                {
                    dev1_session session = (dev1_session)item;
                    DateTime newStartTime = (DateTime)value;
                    ValidationResult result = new ValidationResult();

                    if (session.dev1_EndTime != null)
                    {

                        result.Valid = true;
                        string valueText = (string)value;
                        // Check if the end time is before the start time

                        bool isValid = DateTimeEx.GetTimeDuration(newStartTime) < DateTimeEx.GetTimeDuration(session.dev1_EndTime);

                        result.Valid = isValid;
                        result.Message = "The start time must be before the end time";

                    }
                    else
                        result.Valid = true;
                    return result;


                };

            XrmTimeEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "End", 100, "dev1_endtime")).Validator =
                delegate(object value, object item)
                {
                    dev1_session session = (dev1_session)item;
                    DateTime newEndTime = (DateTime)value;

                    ValidationResult result = new ValidationResult();

                    if (session.dev1_StartTime != null)
                    {
                        result.Valid = true;
                        string valueText = (string)value;
                        // Check if the end time is before the start time

                        bool isValid = DateTimeEx.GetTimeDuration(session.dev1_StartTime) < DateTimeEx.GetTimeDuration(newEndTime);

                        result.Valid = isValid;
                        result.Message = "The end time must be after the start time";
                    }
                    else
                        result.Valid = true;

                    return result;

                };


            XrmDurationEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Duration", 200, "dev1_duration"));



            GridDataViewBinder sessionsDataBinder = new GridDataViewBinder();
            sessionsGrid = sessionsDataBinder.DataBindXrmGrid(sessionsDataView, sessionGridCols, "sessionsGridContainer", null, true, true);
            sessionsDataBinder.DataBindSelectionModel(sessionsGrid, sessionsDataView);


            daysGrid.OnActiveCellChanged.Subscribe(delegate(EventData e, object args)
            {
                CellSelection activeCell = daysGrid.GetActiveCell();
                if (activeCell != null)
                {
                    if (activeCell.Cell < 2)
                    {
                        // Whole activity is selected
                        vm.Days.SelectedDay = null;

                    }
                    else
                    {
                        vm.Days.SelectedDay = activeCell.Cell - 1;

                    }
                }
            });




        }

    }
}
