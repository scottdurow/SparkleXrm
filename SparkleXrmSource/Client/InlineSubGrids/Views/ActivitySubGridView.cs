// ActivitySubGridView.cs
//

using Client.InlineSubGrids.ViewModels;
using Client.MultiEntitySearch.ViewModels;
using jQueryApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Html;

namespace Client.Views.InlineSubGrids
{
    public class ActivitySubGridView
    {
        public static void Init()
        {
           
            ActivitySubGridViewModel vm = new ActivitySubGridViewModel();

            // Add resize height event
            jQuery.Window.Resize(delegate(jQueryEvent e)
            {
                OnChangeHeight();
            });

            // Create Grid
            FetchQuerySettings config = vm.ViewConfig;
            GridDataViewBinder dataViewBinder = new GridDataViewBinder();
         
            Grid grid = dataViewBinder.DataBindXrmGrid(config.DataView, config.Columns, "gridcontainer", "gridpager", true, false);

           
            dataViewBinder.BindClickHandler(grid);
             
            // Data Bind
            ViewBase.RegisterViewModel(vm);
           
            Window.SetTimeout(delegate()
            {
                vm.Init();
                OnChangeHeight();
                grid.ResizeCanvas();
            }, 0);
        }

        private static void OnChangeHeight()
        {
            // Change the height
            int height = jQuery.Window.GetHeight();
            int pagerHeight = jQuery.Select("#gridpager").GetHeight();
            jQuery.Select("#gridcontainer").Height(height - pagerHeight - 2);
        }
    }
}
