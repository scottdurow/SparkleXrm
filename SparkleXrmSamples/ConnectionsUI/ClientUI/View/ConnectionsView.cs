// ConnectionsView.cs
//

using ClientUI.Model;
using ClientUI.View.GridPlugins;
using ClientUI.ViewModel;
using ClientUI.ViewModels;
using jQueryApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace ClientUI.View
{
    public static class ConnectionsView
    {
        #region Fields
        private static ConnectionsViewModel vm;
        private static Grid connectionsGrid;
        #endregion

        #region Methods
        [PreserveCase]
        public static void Init()
        {
            PageEx.MajorVersion = 2013; // Use the CRM2013/2015 styles
            

            
            
            int lcid = (int)OrganizationServiceProxy.GetUserSettings().UILanguageId;

            LocalisedContentLoader.FallBackLCID = 0; // Always get a resource file
            LocalisedContentLoader.SupportedLCIDs.Add(0); // Allow all LCIDs
           
            LocalisedContentLoader.LoadContent("con_/js/Res.metadata.js", lcid, delegate()
            {
                InitLocalisedContent();
            });

            
        }

        private static void InitLocalisedContent()
        {
            
            Dictionary<string, string> parameters;
            string id;
            string logicalName;
            int pageSize = 10;
            string defaultView=null;

#if DEBUG
            id = "C489707F-B5E2-E411-80D5-080027846324";
            logicalName = "account";
            parameters = new Dictionary<string, string>();         
           
#else
            parameters = PageEx.GetWebResourceData(); // The allowed lookup types for the connections - e.g. account, contact, opportunity. This must be passed as a data parameter to the webresource 'account=name&contact=fullname&opportunity=name
            id = ParentPage.Data.Entity.GetId();  
            logicalName =  ParentPage.Data.Entity.GetEntityName();
            ParentPage.Data.Entity.AddOnSave(CheckForSaved);
#endif
            EntityReference parent = new EntityReference(new Guid(id), logicalName, null);
            string entities = "account,contact,opportunity,systemuser";
            foreach (string key in parameters.Keys)
            {
                switch (key.ToLowerCase())
                {
                    case "entities":
                        entities = parameters[key];
                        break;
                    case "pageSize":
                        pageSize = int.Parse(parameters[key]);
                        break;
                    case "view":
                        defaultView = parameters[key];
                        break;
                }
            }
           
            // Get the view
            QueryParser queryParser = new QueryParser(new string[] {"connection"});
            queryParser.GetView("connection", defaultView);
            queryParser.QueryMetadata();
            EntityQuery connectionViews = queryParser.EntityLookup["connection"];
            string viewName = connectionViews.Views.Keys[0];
            FetchQuerySettings view = connectionViews.Views[viewName];

            vm = new ConnectionsViewModel(parent, entities.Split(","), pageSize, view);
            
            // Bind Connections grid
            GridDataViewBinder connectionsGridDataBinder = new GridDataViewBinder();
            List<Column> columns = view.Columns;

            // Role2Id Column - provided it is in the view!
            foreach (Column col in columns)
            {
                switch (col.Field)
                {
                    case "record2roleid":
                        XrmLookupEditor.BindColumn(col, vm.RoleSearchCommand, "connectionroleid", "name,category", "");
                        break;
                    case "description":
                        XrmTextEditor.BindColumn(col);
                        break;
                    case "effectivestart":
                    case "effectiveend":
                        XrmDateEditor.BindColumn(col, true);
                        break;
                }
            }
           

            connectionsGrid = connectionsGridDataBinder.DataBindXrmGrid(vm.Connections, columns, "container", "pager", true, false);

            connectionsGrid.OnActiveCellChanged.Subscribe(delegate(EventData e, object data)
            {
                OnCellChangedEventData eventData = (OnCellChangedEventData)data;
                vm.SelectedConnection.SetValue((Connection)connectionsGrid.GetDataItem(eventData.Row));
            });

            connectionsGridDataBinder.BindClickHandler(connectionsGrid);
            // Let's not use a hover button because it get's in the way of the editable grid!
            //RowHoverPlugin rowButtons = new RowHoverPlugin("gridButtons");
            //connectionsGrid.RegisterPlugin(rowButtons);

            ViewBase.RegisterViewModel(vm);

            OverrideMetadata();

            jQuery.Window.Resize(OnResize);
            jQuery.OnDocumentReady(delegate()
            {
                OnResize(null);
                vm.Search();
            });
        }


        private static void CheckForSaved()
        {     
            // Check if we have the id yet
            EntityReference parent = new EntityReference(new Guid(ParentPage.Data.Entity.GetId()), ParentPage.Data.Entity.GetEntityName(), null);
            if (ParentPage.Ui.GetFormType() != FormTypes.Create && parent.Id != null)
            {
                vm.ParentRecordId.SetValue(parent);
                vm.Search();
            }
            else
            {
                Window.SetTimeout(CheckForSaved, 1000);
            }
        }
        private static void OverrideMetadata()
        {

            // Override the metadata cache for the connection role entity icon because it does not have an icon!
            Func<string, string> getSmallIconUrl = MetadataCache.GetSmallIconUrl;
            Func<string, string> overrideMethod = delegate(string typeName)
            {

                switch (typeName)
                {
                    case "connectionrole":
                        return "/_imgs/ico_16_3234.gif";
                    default:
                        return getSmallIconUrl(typeName);
                }
            };

            Script.Literal("SparkleXrm.Sdk.Metadata.MetadataCache.getSmallIconUrl={0}", (object)overrideMethod);
        }
        #endregion

        #region Event Handlers
        private static void OnResize(jQueryEvent e)
        {
            int height = jQuery.Window.GetHeight();
            int width = jQuery.Window.GetWidth();

            jQuery.Select("#container").Height(height-64).Width(width-1);
            connectionsGrid.ResizeCanvas();
        }
        #endregion
    }
}
