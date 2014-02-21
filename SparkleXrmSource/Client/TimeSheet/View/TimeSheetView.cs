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
using Xrm;
using jQueryApi.UI.Interactions;

namespace Client.TimeSheet.View
{
    public class TimeSheetView : ViewBase
    {
        private static Grid daysGrid;
        private static Grid sessionsGrid;
        private static int StartDaysColumnIndex = 4;
        public static void Init()
        {

            PageEx.MajorVersion = 2013;

          jQuery.OnDocumentReady(delegate()
          {

              ValidationApi.RegisterExtenders();

              // Init settings
              OrganizationServiceProxy.GetUserSettings();

              TimeSheetViewModel vm = new TimeSheetViewModel();


              SetUpGrids(vm);
              SetUpDatePicker(vm);

              // All Resize Grid

              jQuery.Select("#timesheetGridContainer").Plugin<ResizableObject>().Resizable();

              ViewBase.RegisterViewModel(vm);
          });
        }

        private static void SetUpDatePicker(TimeSheetViewModel vm)
        {
            jQueryObject element = jQuery.Select("#datepicker");
 
            string dateFormat = "dd/MM/yy";
            if (OrganizationServiceProxy.UserSettings != null)
            {
                dateFormat = OrganizationServiceProxy.UserSettings.DateFormatString;
            }

            DatePickerOptions2 options2 = new DatePickerOptions2();
            options2.NumberOfMonths = 3;
            options2.FirstDay = OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value;
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
                    vm.Days.SetCurrentWeek(date);
                }
            };

            element.Plugin<DatePickerPlugIn>().DatePicker(options2);


        }

        public static void SetUpGrids(TimeSheetViewModel vm)
        {
           
            // Create Timesheet Grid
            DataViewBase daysDataView = vm.Days;

            List<Column> columns = new List<Column>();
            GridDataViewBinder.BindRowIcon(GridDataViewBinder.AddColumn(columns, "", 50, "icon"), "activity");
            XrmLookupEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Account", 300, "account"), vm.AccountSeachCommand, "accountid", "name", null);
            XrmLookupEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Regarding", 300, "regardingObjectId"), vm.RegardingObjectSearchCommand, "id", "displayName", null);
            XrmLookupEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Activity", 300, "activity"), vm.ActivitySearchCommand, "activityid", "subject", "activitytypecode");

            string[] daysOfWeek = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat","Sun","Mon","Tue","Wed","Thu","Fri","Sat" };
            int firstDayOfWeek = OrganizationServiceProxy.OrganizationSettings.WeekStartDayCode.Value.Value;

            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek], 50, "day0");
            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek + 1], 50, "day1");
            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek + 2], 50, "day2");
            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek + 3], 50, "day3");
            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek + 4], 50, "day4");
            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek + 5], 50, "day5");
            GridDataViewBinder.AddColumn(columns, daysOfWeek[firstDayOfWeek + 6], 50, "day6");

            GridDataViewBinder daysDataBinder = new GridDataViewBinder();
            daysDataBinder.SelectActiveRow = true;
            daysDataBinder.AddCheckBoxSelectColumn = false;
            daysGrid = daysDataBinder.DataBindXrmGrid(daysDataView, columns, "timesheetGridContainer", null, true, true);

            // Set the totals row meta data
            daysDataView.OnGetItemMetaData += delegate(object item)
            {
                ItemMetaData metaData = new ItemMetaData();
                DayEntry day = (DayEntry)item;
                if (day != null && day.isTotalRow)
                {

                    metaData.Editor = delegate(EditorArguments args) { return null; };
                    metaData.Columns = new Dictionary<object, Column>();
                    metaData.Columns["Account"] = new Column("editor", null);
                    metaData.Columns["Regarding"] = new Column("editor", null);
                    metaData.Columns["Activity"] = new Column("editor", null);
                    metaData.Formatter = delegate(int row, int cell, object value, Column columnDef, object dataContext)
                    {
                        switch (columnDef.Field)
                        {
                            case "account":
                            case "regardingObjectId":
                                return "";
                            case "activity":
                                return "Total";
                            default:
                                return XrmDurationEditor.Formatter(row, cell, value, columnDef, dataContext);
                        }
                       
                    };
                           
                    metaData.CssClasses = "days_total_row";
                   
                }

                else
                {
                    metaData.Formatter = delegate(int row, int cell, object value, Column columnDef, object dataContext)
                    {
                        switch (columnDef.Field)
                        {
                            case "account":
                            case "regardingObjectId":
                            case "activity":
                                return XrmLookupEditor.Formatter(row, cell, value, columnDef, dataContext);
                            default:
                                return XrmDurationEditor.Formatter(row, cell, value, columnDef, dataContext);
                        }
                    };

                }

                return metaData;
            };
          
            daysDataBinder.DataBindSelectionModel(daysGrid, daysDataView);


            // ---------------------------------------
            // Sessions Grid
            // ---------------------------------------
            DataViewBase sessionsDataView = vm.SessionDataView;

            List<Column> sessionGridCols = new List<Column>();

            GridDataViewBinder.AddEditIndicatorColumn(sessionGridCols);

            XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Activity", 300, "activitypointer_subject")).Editor = null;

           

            XrmDateEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Date", 100, "dev1_starttime"), true);

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


            XrmDurationEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Duration", 70, "dev1_duration"));
            XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(sessionGridCols, "Description", 300, "dev1_description"));


            GridDataViewBinder sessionsDataBinder = new GridDataViewBinder();
            sessionsDataBinder.SelectActiveRow = false;
            sessionsDataBinder.AddCheckBoxSelectColumn = true;

            sessionsGrid = sessionsDataBinder.DataBindXrmGrid(sessionsDataView, sessionGridCols, "sessionsGridContainer", null, true, true);
            
            sessionsGrid.OnBeforeEditCell.Subscribe(delegate(EventData e, object args)
            {
             
                // Stop the row from being edited if locked
                Entity item = (Entity)((EditEventData)args).item;
                bool result = sessionsDataView.OnBeforeEdit(item);
                Script.Literal("return {0}", result);
                

            });

            daysGrid.OnActiveCellChanged.Subscribe(delegate(EventData e, object args)
            {
                CellSelection activeCell = daysGrid.GetActiveCell();
                if (activeCell != null)
                {
                    if (activeCell.Cell < StartDaysColumnIndex)
                    {
                        // Whole activity is selected
                        vm.Days.SelectedDay = null;

                    }
                    else
                    {
                        vm.Days.SelectedDay = activeCell.Cell - (StartDaysColumnIndex-1);

                    }
                }
            });




        }

    }
}
