// Page1.cs
//

using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;
using Slick;
using SparkleXrm.jQueryPlugins;
using Slick.Controls;
using AddressSearch;
using KnockoutApi;

using jQueryApi.UI;
using jQueryApi.UI.Effects;
using SparkleXrm.GridEditor;
using Xrm.Sdk.Messages;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;
using System.Collections;
using SparkleXrm;
using SparkleXrm.CustomBinding;
using Client.ContactEditor.ViewModels;
using Xrm;
using Client.ContactEditor.Model;

namespace Client.Views
{

    public class ContactEditorView : ViewBase
    {

        public static void init()
        {
            ContactsEditorViewModel vm = new ContactsEditorViewModel();

            OrganizationServiceProxy.GetUserSettings();
            // Data Bind Grid
            List<Column> columns = GridDataViewBinder.ParseLayout(",entityState,20,First Name,firstname,200,Last Name,lastname,200,Birth Date,birthdate,200,Account Role Code,accountrolecode,200,Number of Children,numberofchildren,100,Currency,transactioncurrencyid,200,Credit Limit,creditlimit,100,Gender,gendercode,100,Owner,ownerid,100,Parent Customer,parentcustomerid,100");

            // Set Column formatters and editors
            columns[0].Formatter = delegate(int row, int cell, object value, Column columnDef, object dataContext)
            {
                EntityStates state = (EntityStates)value;
                return ((state == EntityStates.Changed) || (state == EntityStates.Created)) ? "<span class='grid-edit-indicator'></span>" : "";
            };

            // First Name Column
            XrmTextEditor.BindColumn(columns[1]);
          
            // Last Name Column
            XrmTextEditor.BindColumn(columns[2]);

            // Birth Date Column
            XrmDateBindingOptions dateOptions = new XrmDateBindingOptions();
            dateOptions.Hour = 9;
            dateOptions.Minute = 0;
            XrmDateEditor.BindColumn(columns[3], false).Options = dateOptions;

            // Account Code Column
            XrmOptionSetEditor.BindColumn(columns[4], "contact", columns[4].Field, true);
           
            // Number of Children Column
            XrmNumberEditor.BindColumn(columns[5], 0, 100, 0);

            // Currency Column
            XrmLookupEditor.BindColumn(columns[6], vm.TransactionCurrencySearchCommand, "transactioncurrencyid", "currencyname", "");
           
            // Credit Limit Column
            XrmMoneyEditor.BindColumn(columns[7], -10000, 10000);

            // Another optionset
            XrmOptionSetEditor.BindColumn(columns[8], "contact", columns[8].Field, true);

            // Owner Column
            XrmLookupEditorOptions options = (XrmLookupEditorOptions)XrmLookupEditor.BindColumn(columns[9], vm.OwnerSearchCommand, "id", "name", "").Options;
            options.showFooter = true;
          

            // Account Column
            XrmLookupEditorOptions accountLookupOptions = (XrmLookupEditorOptions)XrmLookupEditor.BindColumn(columns[10], vm.AccountSearchCommand, "id", "name", "").Options;
            accountLookupOptions.showFooter = true;
            accountLookupOptions.footerButton = new XrmLookupEditorButton();
            accountLookupOptions.footerButton.Label = "Add New";
            accountLookupOptions.footerButton.Image = "/_imgs/add_10.png";
            accountLookupOptions.footerButton.OnClick = vm.AddNewAccountInLine;

            // Create Grid
            GridDataViewBinder contactGridDataBinder = new GridDataViewBinder();
            Grid contactsGrid = contactGridDataBinder.DataBindXrmGrid(vm.Contacts, columns, "container", "pager",true,false);

            // Data Bind
            ViewBase.RegisterViewModel(vm);

            Window.SetTimeout(delegate()
            {
                vm.Init();
            }, 0);

        }
    }
}
