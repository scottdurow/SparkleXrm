// ConnectionsView.cs
//

using ClientUI.Model;
using ClientUI.View.GridPlugins;
using ClientUI.ViewModel;
using jQueryApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
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
            LocalisedContentLoader.SupportedLCIDs.Add(1033); // English
            LocalisedContentLoader.SupportedLCIDs.Add(1031); // German

            LocalisedContentLoader.LoadContent("con_/js/Res.metadata.js", lcid, delegate()
            {
                InitLocalisedContent();
            });

            
        }

        private static void InitLocalisedContent()
        {
            
            Dictionary<string, string> entityTypes;
            string id;
            string logicalName;

#if DEBUG
            id = "C489707F-B5E2-E411-80D5-080027846324";
            logicalName = "account";
            entityTypes = new Dictionary<string, string>();
            entityTypes["account"] = "name";
            entityTypes["contact"] = "fullname";
            entityTypes["opportunity"] = "name";
#else
            entityTypes = PageEx.GetWebResourceData(); // The allowed lookup types for the connections - e.g. account, contact, opportunity. This must be passed as a data parameter to the webresource 'account=name&contact=fullname&opportunity=name
            id = ParentPage.Data.Entity.GetId();  
            logicalName =  ParentPage.Data.Entity.GetEntityName(); 
#endif
            EntityReference parent = new EntityReference(new Guid(id), logicalName, null);
            vm = new ConnectionsViewModel(parent, entityTypes);
            // Bind Connections grid
            GridDataViewBinder contactGridDataBinder = new GridDataViewBinder();
            List<Column> columns = GridDataViewBinder.ParseLayout(String.Format("{0},record1id,250,{1},record1roleid,250", ResourceStrings.ConnectTo, ResourceStrings.Role));

            // Role2Id Column
            XrmLookupEditor.BindColumn(columns[1], vm.RoleSearchCommand, "connectionroleid", "name", "");

            connectionsGrid = contactGridDataBinder.DataBindXrmGrid(vm.Connections, columns, "container", "pager", true, false);

            connectionsGrid.OnActiveCellChanged.Subscribe(delegate(EventData e, object data)
            {
                OnCellChangedEventData eventData = (OnCellChangedEventData)data;
                vm.SelectedConnection.SetValue((Connection)connectionsGrid.GetDataItem(eventData.Row));
            });

            // Let's not use a hover button because it get's n the way of the editable grid!
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

            Script.Literal("Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl={0}", (object)overrideMethod);
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
