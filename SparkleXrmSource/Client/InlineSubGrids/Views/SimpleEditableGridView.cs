// ActivitySubGridView.cs
//

using Client.InlineSubGrids.ViewModels;
using Client.MultiEntitySearch.ViewModels;
using jQueryApi;
using Slick;
using SparkleXrm;
using SparkleXrm.CustomBinding;
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

                XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Title", 150, "title"));
                XrmTextEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Author", 150, "author"));
                XrmDateEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Published", 150, "publishdate"), true);
                XrmMoneyEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Price", 150, "price"), 0, 100);
                XrmNumberEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Copies", 150, "numberofcopies"), 0, 1000, 0);
                XrmLookupEditorOptions languageLookupOptions =
                    (XrmLookupEditorOptions)XrmLookupEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Language", 150, "language"), vm.GetLanguages, "id", "name", null).Options;
                languageLookupOptions.showImage = false;

                OptionSetBindingOptions formatBindingOptions = new OptionSetBindingOptions();
                formatBindingOptions.allowEmpty = true;
                formatBindingOptions.GetOptionSetsDelegate = vm.GetFormats;

                XrmOptionSetEditor.BindColumnWithOptions(GridDataViewBinder.AddColumn(columns, "Format", 150, "format"), formatBindingOptions);
                XrmDurationEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Audio Length", 150, "audiolength"));
                XrmTimeEditor.BindColumn(GridDataViewBinder.AddColumn(columns, "Start Time", 150, "starttime"));

                Grid grid = dataViewBinder.DataBindXrmGrid(vm.Books, columns, "booksGridContainer", null, true, true);
                ViewBase.RegisterViewModel(vm);

                Window.SetTimeout(delegate()
                {
                    vm.LoadBooks();
                    grid.ResizeCanvas();
                }, 0);
                
            });
        
        }
    }
}
