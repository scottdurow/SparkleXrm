// EditBulkDeleteJobsView.cs
//

using Client.ScheduledJobsEditor.ViewModels;
using SparkleXrm.GridEditor;
using jQueryApi;
using KnockoutApi;
using SparkleXrm;
using Slick;
using Slick.Controls;
using System;
using System.Collections.Generic;
using Xrm.Sdk;

namespace Client.ScheduledJobsEditor.Views
{
    public class ScheduledJobsEditorView :ViewBase
    {
        public static Grid jobsGrid;
        public static Grid bulkDeleteJobsGrid;

        public static void Init()
        {
            jQuery.OnDocumentReady(delegate()
            {

                ValidationApi.RegisterExtenders();

                // Init settings
                OrganizationServiceProxy.GetUserSettings();

                ScheduledJobsEditorViewModel vm = new ScheduledJobsEditorViewModel();


                SetUpGrids(vm);

                ViewBase.RegisterViewModel(vm);
            });
        }
        public static void SetUpGrids(ScheduledJobsEditorViewModel vm)
        {
         
            // Create Scheduled Jobs Grid
            GridDataViewBinder jobsDataBinder = new GridDataViewBinder();

            List<Column> jobCols = GridDataViewBinder.ParseLayout("dev1_name,Name,300,dev1_recurrancepattern,Pattern,300,createdon,Created On, 300");
            jobsGrid = jobsDataBinder.DataBindXrmGrid(vm.JobsViewModel, jobCols, "jobsGrid", "jobsGridPager",false,false);

            GridDataViewBinder bulkDeleteDataBinder = new GridDataViewBinder();
            List<Column> bulkDeleteCols = GridDataViewBinder.ParseLayout("name,Name,300,asyncoperation_statuscode,Status,100,asyncoperation_postponeuntil,Next Run,150,asyncoperation_recurrencepattern,Pattern,150,createdon,Created On,150");
            bulkDeleteJobsGrid = bulkDeleteDataBinder.DataBindXrmGrid(vm.bulkDeleteJobsViewModel,bulkDeleteCols, "bulkDeleteJobGrid", "bulkDeleteJobGridPager",false, false);

            // Load first page
            vm.JobsViewModel.Refresh();


        }

       

        
    }
}
