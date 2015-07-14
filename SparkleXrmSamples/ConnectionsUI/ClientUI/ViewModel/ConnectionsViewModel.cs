// ConnectionsViewModel.cs
//

using ClientUI.Model;
using ClientUI.ViewModels;
using jQueryApi;
using KnockoutApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;

namespace ClientUI.ViewModel
{
    public class ConnectionsViewModel : ViewModelBase
    {
        #region Fields
       
        [PreserveCase]
        public EntityDataViewModel Connections;
        [PreserveCase]
        public Observable<ObservableConnection> ConnectionEdit;
        [PreserveCase]
        public Observable<Connection> SelectedConnection = Knockout.Observable<Connection>();
        [PreserveCase]
        public Observable<String> ErrorMessage = Knockout.Observable<string>();
        [PreserveCase]
        public DependentObservable<bool> AllowAddNew;
        public Observable<EntityReference> ParentRecordId = Knockout.Observable<EntityReference>();
        private string _viewFetchXml;
        #endregion

        #region Constructors
        public ConnectionsViewModel(EntityReference parentRecordId, string[] connectToTypes, int pageSize, string viewFetchXml)
        {
            Connections = new EntityDataViewModel(pageSize, typeof(Connection), true);
            ParentRecordId.SetValue(parentRecordId);
            _viewFetchXml = viewFetchXml;
            ObservableConnection connection = new ObservableConnection(connectToTypes);
            connection.Record2Id.SetValue(parentRecordId);
            ConnectionEdit = (Observable<ObservableConnection>)ValidatedObservableFactory.ValidatedObservable(connection);
           
            Connections.OnDataLoaded.Subscribe(Connections_OnDataLoaded);
            ConnectionEdit.GetValue().OnSaveComplete += ConnectionsViewModel_OnSaveComplete;
            ObservableConnection.RegisterValidation(Connections.ValidationBinder);
            AllowAddNew = Knockout.DependentObservable<bool>(AllowAddNewComputed);
        }
        #endregion

        #region Event Handlers
        private void Connections_OnDataLoaded(EventData e, object data)
        {
            DataLoadedNotifyEventArgs args = (DataLoadedNotifyEventArgs) data;
           
            for (int i=0; i<args.To;i++)
            {
                Connection connection = (Connection)Connections.GetItem(i);
                if (connection==null)
                    return;
                connection.PropertyChanged+=connection_PropertyChanged;
            }
        }
    

        private void connection_PropertyChanged(object sender, Xrm.ComponentModel.PropertyChangedEventArgs e)
        {
            Connection connectionToUpdate = new Connection();
            Connection updated = (Connection)sender;
            connectionToUpdate.ConnectionID = new Guid(updated.Id);
            bool updateRequired = false;

            switch (e.PropertyName)
            {
                case "record2roleid":                  
                    connectionToUpdate.Record2RoleId = updated.Record2RoleId;
                    updateRequired = true;
                    break;
                case "description":
                    connectionToUpdate.Description = updated.Description;
                    updateRequired = true;
                    break;
                case "effectivestart":
                    connectionToUpdate.EffectiveStart = updated.EffectiveStart;
                    updateRequired = true;
                    break;
                case "effectiveend":
                    connectionToUpdate.EffectiveEnd = updated.EffectiveEnd;
                    updateRequired = true;
                    break;
            }
         

            // Auto save
            if (updateRequired)
            {
                OrganizationServiceProxy.BeginUpdate(connectionToUpdate, delegate(object state)
                {
                    try
                    {
                        OrganizationServiceProxy.EndUpdate(state);
                        ErrorMessage.SetValue(null);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.SetValue(ex.Message);
                    }

                });
            }
        } 
        private void ConnectionsViewModel_OnSaveComplete(string result)
        {
          
            if (result == null)
            {
                // Saved OK
                Connections.Reset();
                Connections.Refresh();
            }
            ErrorMessage.SetValue(result);
        }
        #endregion

        #region Commands
        public void Search()
        {
            string parentRecordId = ParentRecordId.GetValue().Id.ToString().Replace("{", "").Replace("}", "") ;
            if (_viewFetchXml == null)
            {
                Connections.FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>
                                  <entity name='connection'>
                                    <attribute name='record2id' />
                                    <attribute name='record2roleid' />
                                    <attribute name='record1id' />
                                    <attribute name='record1roleid' />
                                    <attribute name='connectionid' />
                                    <filter type='and'>
                                      
                                      <condition attribute='record2id' operator='eq' value='" + parentRecordId + @"' />
                                    </filter>
                                  {3}
                                  </entity>
                                </fetch>";
            }
            else
            {
                Connections.FetchXml = _viewFetchXml.Replace(QueryParser.ParentRecordPlaceholder, parentRecordId);

            }
            Connections.Refresh();
        }

        [PreserveCase]
        public void RoleSearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get the possible roles
            ObservableConnection.RoleSearch(term, callback, SelectedConnection.GetValue().Record2Id.LogicalName);
        }

        [PreserveCase]
        public void AddNewCommand()
        {
            ConnectionEdit.GetValue().Record2Id.SetValue(ParentRecordId.GetValue());
            ErrorMessage.SetValue(null);
            ConnectionEdit.GetValue().AddNewVisible.SetValue(true);

        }
        [PreserveCase]
        public void OpenAssociatedSubGridCommand()
        {
            NavigationItem item = ParentPage.Ui.Navigation.Items.Get("navConnections");
            item.SetFocus();

        }
        [PreserveCase]
        public void DeleteSelectedCommand()
        {
            List<int> selectedRows = DataViewBase.RangesToRows(Connections.GetSelectedRows());
            if (selectedRows.Count == 0)
                return;

            Utility.ConfirmDialog(
                String.Format(ResourceStrings.ConfirmDeleteSelectedConnection, selectedRows.Count),
                delegate()
                {
                    List<Entity> itemsToRemove = new List<Entity>();
                    foreach (int row in selectedRows)
                    {
                        itemsToRemove.Add((Entity)Connections.GetItem(row));
                    }
                    try
                    {
                        foreach (Entity item in itemsToRemove)
                        {
                            OrganizationServiceProxy.Delete_(item.LogicalName, new Guid(item.Id));
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.SetValue(ex.ToString());
                    }
                    Connections.RaiseOnSelectedRowsChanged(null);
                    Connections.Reset();
                    Connections.Refresh();
                }, null);

        }
        [PreserveCase]
        public void DeleteCommand(object data, jQueryEvent e)
        {
            Utility.ConfirmDialog(ResourceStrings.ConfirmDeleteConnection,delegate(){
            
                string id = e.Target.ParentNode.GetAttribute("rowId").ToString();
                OrganizationServiceProxy.BeginDelete(Connection.LogicalName, new Guid(id), delegate(object state)
                {
                    try
                    {
                        OrganizationServiceProxy.EndDelete(state);
                        foreach (Entity connection in Connections.Data)
                        {
                            if (connection.Id == id)
                            {
                                Connections.RemoveItem(connection);
                                break;
                            }
                        }
                        Connections.Refresh();

                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.SetValue(ex.Message);
                    }
                });
                
            },null);
        }
        #endregion

        #region Computed Observables
        public bool AllowAddNewComputed()
        {
            EntityReference parent = ParentRecordId.GetValue();
            return parent != null && parent.Id != null && parent.Id.Value != null && parent.Id.Value.Length > 0;
        }

        #endregion
    }
}
