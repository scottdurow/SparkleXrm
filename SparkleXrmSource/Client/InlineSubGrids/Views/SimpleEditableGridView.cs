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
using Xrm;
using Xrm.Sdk;

namespace Client.Views.InlineSubGrids
{
    public class SimpleEditableGridView
    {
        public static void Init()
        {

            PageEx.MajorVersion = 2013;

            jQuery.OnDocumentReady(delegate()
            {

                ValidationApi.RegisterExtenders();

                // Init settings
                OrganizationServiceProxy.GetUserSettings();
                SimpleEditableGridViewModel vm = new SimpleEditableGridViewModel();

                // Create Grid
                GridDataViewBinder dataViewBinder = new GridDataViewBinder();
                dataViewBinder.AddCheckBoxSelectColumn = true;
                dataViewBinder.SelectActiveRow = true;
                dataViewBinder.MultiSelect = false;
                List<Column> columns = new List<Column>();
                EditorFactory textEditor = (EditorFactory)Script.Literal("Slick.Editors.Text");

                XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Title", 300, "title"));
                XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Author", 300, "author"));

                dataViewBinder.DataBindXrmGrid(vm.Books, columns, "booksGridContainer", null, true, true);
                ViewBase.RegisterViewModel(vm);
            });
        
        }
    }
}
