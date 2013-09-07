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
using System.Html;
using Xrm;
using Xrm.Sdk;

namespace Client.MultiEntitySearch.Views
{
    public class MultiSearchView
    {
        public static void init()
        {
            MultiSearchViewModel vm = new MultiSearchViewModel();

            // Create Grids
            FetchQuerySettings[] searches = vm.Config.GetItems();

            int i = 0;
            foreach (FetchQuerySettings config in searches)
            {
                GridDataViewBinder dataViewBinder = new GridDataViewBinder();
                Grid grid = dataViewBinder.DataBindXrmGrid(config.DataView, config.Columns, "grid" + i.ToString() + "container", "grid" + i.ToString() + "pager", true, false);
                dataViewBinder.BindClickHandler(grid);
                i++;
            }

            // Data Bind
            ViewBase.RegisterViewModel(vm);

        }
    }
}
