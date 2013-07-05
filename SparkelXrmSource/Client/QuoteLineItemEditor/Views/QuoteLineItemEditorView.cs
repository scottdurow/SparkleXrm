using Client.QuoteLineItemEditor.ViewModels;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System.Collections.Generic;

namespace Client.QuoteLineItemEditor.Views
{
    public class QuoteLineItemEditorView : ViewBase
    {
        #region Methods
        public static void init()
        {
            QuoteLineItemEditorViewModel vm = new QuoteLineItemEditorViewModel();

            List<Column> columns = new List<Column>(); 
            GridDataViewBinder.AddEditIndicatorColumn(columns);
            
            XrmNumberEditor.BindReadOnlyColumn(
                GridDataViewBinder.AddColumn(columns,"#",40,"lineitemnumber"),
                0);

            XrmLookupEditor.BindColumn(
                GridDataViewBinder.AddColumn(columns, "Existing Product", 200, "productid"), 
                vm.ProductSearchCommand, "productid", "name", "");

            XrmLookupEditor.BindColumn(
                GridDataViewBinder.AddColumn(columns, "Units", 100, "uomid"),
                vm.UoMSearchCommand, "uomid", "name", "");

            XrmTextEditor.BindColumn(
                GridDataViewBinder.AddColumn(columns, "Write-In Product", 200, "productdescription"));

            XrmMoneyEditor.BindColumn(
                GridDataViewBinder.AddColumn(columns, "Price Per Unit", 200, "priceperunit"),
                0,
                1000);

            XrmNumberEditor.BindColumn(
                GridDataViewBinder.AddColumn(columns, "Quantity", 200, "quantity"),
                0,
                1000,
                2);

            XrmMoneyEditor.BindReadOnlyColumn(
                GridDataViewBinder.AddColumn(columns, "Extended Amount", 100, "extendedamount"));

            GridDataViewBinder contactGridDataBinder = new GridDataViewBinder();
            Grid contactsGrid = contactGridDataBinder.DataBindXrmGrid(vm.Lines, columns, "quoteproductGrid", "quoteproductPager",true,true);

            ViewBase.RegisterViewModel(vm);
        }
        #endregion
    }
}
