//! ClientUI.debug.js
//

(function($){

Type.registerNamespace('ClientUI');

////////////////////////////////////////////////////////////////////////////////
// ResourceStrings

ResourceStrings = function ResourceStrings() {
}


Type.registerNamespace('ClientUI.View.GridPlugins');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.View.GridPlugins.RowHoverPlugin

ClientUI.View.GridPlugins.RowHoverPlugin = function ClientUI_View_GridPlugins_RowHoverPlugin(containerDivId) {
    this._containerId = containerDivId;
}
ClientUI.View.GridPlugins.RowHoverPlugin.prototype = {
    _grid: null,
    _hoverButtons: null,
    _containerId: null,
    _mouseOut: false,
    
    destroy: function ClientUI_View_GridPlugins_RowHoverPlugin$destroy() {
        this._hoverButtons.remove();
    },
    
    init: function ClientUI_View_GridPlugins_RowHoverPlugin$init(grid) {
        this._grid = grid;
        this._hoverButtons = $('#' + this._containerId);
        this._hoverButtons.mouseenter(ss.Delegate.create(this, function(e) {
            this._mouseOut = false;
        }));
        $('#grid').find('.slick-viewport').append(this._hoverButtons);
        (this._grid.onMouseEnter).subscribe(ss.Delegate.create(this, this.handleMouseEnter));
        (this._grid.onMouseLeave).subscribe(ss.Delegate.create(this, this.handleMouseLeave));
    },
    
    handleMouseLeave: function ClientUI_View_GridPlugins_RowHoverPlugin$handleMouseLeave(e, item) {
        this._mouseOut = true;
        window.setTimeout(ss.Delegate.create(this, function() {
            if (this._mouseOut) {
                this._hoverButtons.fadeOut();
            }
        }), 500);
    },
    
    handleMouseEnter: function ClientUI_View_GridPlugins_RowHoverPlugin$handleMouseEnter(e, item) {
        var cell = this._grid.getCellFromEvent(e);
        if (cell != null) {
            this._mouseOut = false;
            var entityRow = this._grid.getDataItem(cell.row);
            if (entityRow != null) {
                this._grid.getGridPosition();
                var viewPortRight = this._grid.getViewport().rightPx;
                var viewPortLeft = this._grid.getViewport().leftPx;
                var node = $(this._grid.getCellNode(cell.row,cell.cell));
                var buttonsWidth = this._hoverButtons.width();
                var x = node.parent().width();
                if (viewPortRight < x + buttonsWidth) {
                    x = viewPortRight - buttonsWidth;
                }
                var y = 0;
                node.parent().append(this._hoverButtons);
                this._hoverButtons.css('left', x.toString() + 'px');
                this._hoverButtons.css('top', y.toString() + 'px');
                this._hoverButtons.css('display', 'block');
                this._hoverButtons.attr('rowId', entityRow.id);
            }
        }
    }
}


Type.registerNamespace('ClientUI.Model');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.Model.Connection

ClientUI.Model.Connection = function ClientUI_Model_Connection() {
    ClientUI.Model.Connection.initializeBase(this, [ 'connection' ]);
}
ClientUI.Model.Connection.prototype = {
    connectionid: null,
    record1id: null,
    record2id: null,
    record1roleid: null,
    record2roleid: null
}


Type.registerNamespace('ClientUI.ViewModel');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.ViewModel.ConnectionsViewModel

ClientUI.ViewModel.ConnectionsViewModel = function ClientUI_ViewModel_ConnectionsViewModel(parentRecordId, connectToTypes) {
    this.Connections = new SparkleXrm.GridEditor.EntityDataViewModel(2, ClientUI.Model.Connection, true);
    this.SelectedConnection = ko.observable();
    this.ErrorMessage = ko.observable();
    this.AllowAddNew = ko.observable(true);
    ClientUI.ViewModel.ConnectionsViewModel.initializeBase(this);
    this._parentRecordId$1 = parentRecordId;
    var connection = new ClientUI.ViewModel.ObservableConnection(connectToTypes);
    connection.record2id(this._parentRecordId$1);
    this.ConnectionEdit = ko.validatedObservable(connection);
    this.Connections.onDataLoaded.subscribe(ss.Delegate.create(this, this._connections_OnDataLoaded$1));
    this.ConnectionEdit().add_onSaveComplete(ss.Delegate.create(this, this._connectionsViewModel_OnSaveComplete$1));
    ClientUI.ViewModel.ObservableConnection.registerValidation(this.Connections.validationBinder);
}
ClientUI.ViewModel.ConnectionsViewModel.prototype = {
    ConnectionEdit: null,
    _parentRecordId$1: null,
    
    _connections_OnDataLoaded$1: function ClientUI_ViewModel_ConnectionsViewModel$_connections_OnDataLoaded$1(e, data) {
        var args = data;
        for (var i = 0; i < args.to; i++) {
            var connection = this.Connections.getItem(i);
            if (connection == null) {
                return;
            }
            connection.add_propertyChanged(ss.Delegate.create(this, this._connection_PropertyChanged$1));
        }
    },
    
    _connection_PropertyChanged$1: function ClientUI_ViewModel_ConnectionsViewModel$_connection_PropertyChanged$1(sender, e) {
        if (e.propertyName === 'record1roleid') {
            var updated = sender;
            var connectionToUpdate = new ClientUI.Model.Connection();
            connectionToUpdate.connectionid = new Xrm.Sdk.Guid(updated.id);
            connectionToUpdate.record1roleid = updated.record1roleid;
            Xrm.Sdk.OrganizationServiceProxy.beginUpdate(connectionToUpdate, ss.Delegate.create(this, function(state) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endUpdate(state);
                }
                catch (ex) {
                    this.ErrorMessage(ex.message);
                }
            }));
        }
    },
    
    _connectionsViewModel_OnSaveComplete$1: function ClientUI_ViewModel_ConnectionsViewModel$_connectionsViewModel_OnSaveComplete$1(result) {
        if (result == null) {
            this.Connections.reset();
            this.Connections.refresh();
        }
        this.ErrorMessage(result);
    },
    
    search: function ClientUI_ViewModel_ConnectionsViewModel$search() {
        this.Connections.set_fetchXml("<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>\r\n                                  <entity name='connection'>\r\n                                    <attribute name='record2id' />\r\n                                    <attribute name='record2roleid' />\r\n                                    <attribute name='record1id' />\r\n                                    <attribute name='record1roleid' />\r\n                                    <attribute name='connectionid' />\r\n                                    <filter type='and'>\r\n                                      \r\n                                      <condition attribute='record2id' operator='eq' value='" + this._parentRecordId$1.id.toString().replaceAll('{', '').replaceAll('}', '') + "' />\r\n                                    </filter>\r\n                                  {3}\r\n                                  </entity>\r\n                                </fetch>");
        this.Connections.refresh();
    },
    
    RoleSearchCommand: function ClientUI_ViewModel_ConnectionsViewModel$RoleSearchCommand(term, callback) {
        ClientUI.ViewModel.ObservableConnection.RoleSearch(term, callback, this.SelectedConnection().record1id.logicalName);
    },
    
    AddNewCommand: function ClientUI_ViewModel_ConnectionsViewModel$AddNewCommand() {
        this.ErrorMessage(null);
        this.ConnectionEdit().AddNewVisible(true);
    },
    
    OpenAssociatedSubGridCommand: function ClientUI_ViewModel_ConnectionsViewModel$OpenAssociatedSubGridCommand() {
        var item = window.parent.Xrm.Page.ui.navigation.items.get('navConnections');
        item.setFocus();
    },
    
    DeleteSelectedCommand: function ClientUI_ViewModel_ConnectionsViewModel$DeleteSelectedCommand() {
        var selectedRows = SparkleXrm.GridEditor.DataViewBase.rangesToRows(this.Connections.getSelectedRows());
        if (!selectedRows.length) {
            return;
        }
        Xrm.Utility.confirmDialog(String.format(ResourceStrings.ConfirmDeleteSelectedConnection, selectedRows.length), ss.Delegate.create(this, function() {
            var itemsToRemove = [];
            var $enum1 = ss.IEnumerator.getEnumerator(selectedRows);
            while ($enum1.moveNext()) {
                var row = $enum1.current;
                itemsToRemove.add(this.Connections.getItem(row));
            }
            try {
                var $enum2 = ss.IEnumerator.getEnumerator(itemsToRemove);
                while ($enum2.moveNext()) {
                    var item = $enum2.current;
                    Xrm.Sdk.OrganizationServiceProxy.delete_(item.logicalName, new Xrm.Sdk.Guid(item.id));
                }
            }
            catch (ex) {
                this.ErrorMessage(ex.toString());
            }
            this.Connections.raiseOnSelectedRowsChanged(null);
            this.Connections.reset();
            this.Connections.refresh();
        }), null);
    },
    
    DeleteCommand: function ClientUI_ViewModel_ConnectionsViewModel$DeleteCommand(data, e) {
        Xrm.Utility.confirmDialog(ResourceStrings.ConfirmDeleteConnection, ss.Delegate.create(this, function() {
            var id = e.target.parentNode.getAttribute('rowId').toString();
            Xrm.Sdk.OrganizationServiceProxy.beginDelete(ClientUI.Model.Connection.logicalName, new Xrm.Sdk.Guid(id), ss.Delegate.create(this, function(state) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endDelete(state);
                    var $enum1 = ss.IEnumerator.getEnumerator(this.Connections.get_data());
                    while ($enum1.moveNext()) {
                        var connection = $enum1.current;
                        if (connection.id === id) {
                            this.Connections.removeItem(connection);
                            break;
                        }
                    }
                    this.Connections.refresh();
                }
                catch (ex) {
                    this.ErrorMessage(ex.message);
                }
            }));
        }), null);
    }
}


////////////////////////////////////////////////////////////////////////////////
// ClientUI.ViewModel.ObservableConnection

ClientUI.ViewModel.ObservableConnection = function ClientUI_ViewModel_ObservableConnection(types) {
    this.AddNewVisible = ko.observable(false);
    this.connectiondid = ko.observable();
    this.record1id = ko.observable();
    this.record2id = ko.observable();
    this.record1roleid = ko.observable();
    this.record2roleid = ko.observable();
    ClientUI.ViewModel.ObservableConnection.initializeBase(this);
    this._connectToTypes$1 = types;
    ClientUI.ViewModel.ObservableConnection.registerValidation(new SparkleXrm.ObservableValidationBinder(this));
}
ClientUI.ViewModel.ObservableConnection.RoleSearch = function ClientUI_ViewModel_ObservableConnection$RoleSearch(term, callback, typeName) {
    var recordTypeFilter = '';
    if (typeName != null) {
        var etc = Mscrm.EntityPropUtil.EntityTypeName2CodeMap[typeName];
        recordTypeFilter = String.format("\r\n                                        <filter>\r\n                                            <condition attribute='associatedobjecttypecode' operator='eq' value='{0}' />\r\n                                        </filter>", etc);
    }
    var fetchXml = "\r\n                            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true' >\r\n                                <entity name='connectionrole' >\r\n                                    <attribute name='category' />\r\n                                    <attribute name='name' />\r\n                                    <attribute name='connectionroleid' />\r\n                                    <attribute name='statecode' />\r\n                                    <order attribute='name' descending='false' />\r\n                                    <link-entity name='connectionroleobjecttypecode' from='connectionroleid' to='connectionroleid' >\r\n                                    {1}\r\n                                    </link-entity>\r\n                                    <filter>\r\n                                        <condition attribute='name' operator='like' value='%{0}%' />\r\n                                    </filter>\r\n                                </entity>\r\n                            </fetch>";
    fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term), recordTypeFilter);
    Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
        var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
        callback(fetchResult);
    });
}
ClientUI.ViewModel.ObservableConnection.validateRecord1Id = function ClientUI_ViewModel_ObservableConnection$validateRecord1Id(rules, viewModel, dataContext) {
    return rules.addRule(ResourceStrings.RequiredMessage, function(value) {
        return (value != null) && (value).id != null;
    });
}
ClientUI.ViewModel.ObservableConnection.validateRecord1RoleId = function ClientUI_ViewModel_ObservableConnection$validateRecord1RoleId(rules, viewModel, dataContext) {
    return rules.addRule(ResourceStrings.RequiredMessage, function(value) {
        return (value != null) && (value).id != null;
    });
}
ClientUI.ViewModel.ObservableConnection.registerValidation = function ClientUI_ViewModel_ObservableConnection$registerValidation(binder) {
    binder.register('record1id', ClientUI.ViewModel.ObservableConnection.validateRecord1Id);
    binder.register('record1roleid', ClientUI.ViewModel.ObservableConnection.validateRecord1RoleId);
}
ClientUI.ViewModel.ObservableConnection.prototype = {
    
    add_onSaveComplete: function ClientUI_ViewModel_ObservableConnection$add_onSaveComplete(value) {
        this.__onSaveComplete$1 = ss.Delegate.combine(this.__onSaveComplete$1, value);
    },
    remove_onSaveComplete: function ClientUI_ViewModel_ObservableConnection$remove_onSaveComplete(value) {
        this.__onSaveComplete$1 = ss.Delegate.remove(this.__onSaveComplete$1, value);
    },
    
    __onSaveComplete$1: null,
    _connectToTypes$1: null,
    
    RecordSearchCommand: function ClientUI_ViewModel_ObservableConnection$RecordSearchCommand(term, callback) {
        var resultsBack = 0;
        var mergedEntities = [];
        var result = ss.Delegate.create(this, function(fetchResult) {
            resultsBack++;
            mergedEntities.addRange(fetchResult.get_entities().items());
            mergedEntities.sort(function(x, y) {
                return String.compare(x.getAttributeValueString('name'), y.getAttributeValueString('name'));
            });
            if (resultsBack === Object.getKeyCount(this._connectToTypes$1)) {
                var results = new Xrm.Sdk.EntityCollection(mergedEntities);
                callback(results);
            }
        });
        var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(this._connectToTypes$1));
        while ($enum1.moveNext()) {
            var entity = $enum1.current;
            this._searchRecords$1(term, result, entity, this._connectToTypes$1[entity]);
        }
    },
    
    _searchRecords$1: function ClientUI_ViewModel_ObservableConnection$_searchRecords$1(term, callback, entityType, entityNameAttribute) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true' count='25'>\r\n                              <entity name='{1}'>\r\n                                <attribute name='{2}' alias='name' />\r\n                                <order attribute='{2}' descending='false' />\r\n                                <filter type='and'>\r\n                                  <condition attribute='{2}' operator='like' value='%{0}%' />\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term), entityType, entityNameAttribute);
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    RoleSearchCommand: function ClientUI_ViewModel_ObservableConnection$RoleSearchCommand(term, callback) {
        var record = this.record1id();
        ClientUI.ViewModel.ObservableConnection.RoleSearch(term, callback, (record != null) ? record.logicalName : null);
    },
    
    SaveCommand: function ClientUI_ViewModel_ObservableConnection$SaveCommand() {
        if (!(this).isValid()) {
            (this).errors.showAllMessages(true);
            return;
        }
        this.isBusy(true);
        this.AddNewVisible(false);
        var connection = new ClientUI.Model.Connection();
        connection.record1id = this.record1id();
        connection.record2id = this.record2id();
        connection.record1roleid = this.record1roleid();
        connection.record2roleid = this.record2roleid();
        Xrm.Sdk.OrganizationServiceProxy.beginCreate(connection, ss.Delegate.create(this, function(state) {
            try {
                this.connectiondid(Xrm.Sdk.OrganizationServiceProxy.endCreate(state));
                this.__onSaveComplete$1(null);
                this.record1id(null);
                this.record1roleid(null);
                (this).errors.showAllMessages(false);
            }
            catch (ex) {
                this.__onSaveComplete$1(ex.message);
            }
            finally {
                this.isBusy(false);
            }
        }));
    },
    
    CancelCommand: function ClientUI_ViewModel_ObservableConnection$CancelCommand() {
        this.AddNewVisible(false);
    }
}


Type.registerNamespace('ClientUI.View');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.View.ConnectionsView

ClientUI.View.ConnectionsView = function ClientUI_View_ConnectionsView() {
}
ClientUI.View.ConnectionsView.Init = function ClientUI_View_ConnectionsView$Init() {
    Xrm.PageEx.majorVersion = 2013;
    var lcid = Xrm.Sdk.OrganizationServiceProxy.getUserSettings().uilanguageid;
    SparkleXrm.LocalisedContentLoader.fallBackLCID = 0;
    SparkleXrm.LocalisedContentLoader.supportedLCIDs.add(1033);
    SparkleXrm.LocalisedContentLoader.supportedLCIDs.add(1031);
    SparkleXrm.LocalisedContentLoader.loadContent('con_/js/Res.metadata.js', lcid, function() {
        ClientUI.View.ConnectionsView._initLocalisedContent();
    });
}
ClientUI.View.ConnectionsView._initLocalisedContent = function ClientUI_View_ConnectionsView$_initLocalisedContent() {
    var entityTypes;
    var id;
    var logicalName;
    entityTypes = Xrm.PageEx.getWebResourceData();
    id = window.parent.Xrm.Page.data.entity.getId();
    logicalName = window.parent.Xrm.Page.data.entity.getEntityName();
    var parent = new Xrm.Sdk.EntityReference(new Xrm.Sdk.Guid(id), logicalName, null);
    ClientUI.View.ConnectionsView._vm = new ClientUI.ViewModel.ConnectionsViewModel(parent, entityTypes);
    var contactGridDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var columns = SparkleXrm.GridEditor.GridDataViewBinder.parseLayout(String.format('{0},record1id,250,{1},record1roleid,250', ResourceStrings.ConnectTo, ResourceStrings.Role));
    SparkleXrm.GridEditor.XrmLookupEditor.bindColumn(columns[1], ss.Delegate.create(ClientUI.View.ConnectionsView._vm, ClientUI.View.ConnectionsView._vm.RoleSearchCommand), 'connectionroleid', 'name', '');
    ClientUI.View.ConnectionsView._connectionsGrid = contactGridDataBinder.dataBindXrmGrid(ClientUI.View.ConnectionsView._vm.Connections, columns, 'container', 'pager', true, false);
    ClientUI.View.ConnectionsView._connectionsGrid.onActiveCellChanged.subscribe(function(e, data) {
        var eventData = data;
        ClientUI.View.ConnectionsView._vm.SelectedConnection(ClientUI.View.ConnectionsView._connectionsGrid.getDataItem(eventData.row));
    });
    SparkleXrm.ViewBase.registerViewModel(ClientUI.View.ConnectionsView._vm);
    ClientUI.View.ConnectionsView._overrideMetadata();
    $(window).resize(ClientUI.View.ConnectionsView._onResize);
    $(function() {
        ClientUI.View.ConnectionsView._onResize(null);
        ClientUI.View.ConnectionsView._vm.search();
    });
}
ClientUI.View.ConnectionsView._overrideMetadata = function ClientUI_View_ConnectionsView$_overrideMetadata() {
    var getSmallIconUrl = Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl;
    var overrideMethod = function(typeName) {
        switch (typeName) {
            case 'connectionrole':
                return '/_imgs/ico_16_3234.gif';
            default:
                return getSmallIconUrl(typeName);
        }
    };
    Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl=overrideMethod;
}
ClientUI.View.ConnectionsView._onResize = function ClientUI_View_ConnectionsView$_onResize(e) {
    var height = $(window).height();
    var width = $(window).width();
    $('#container').height(height - 64).width(width - 1);
    ClientUI.View.ConnectionsView._connectionsGrid.resizeCanvas();
}


ResourceStrings.registerClass('ResourceStrings');
ClientUI.View.GridPlugins.RowHoverPlugin.registerClass('ClientUI.View.GridPlugins.RowHoverPlugin', null, Object);
ClientUI.Model.Connection.registerClass('ClientUI.Model.Connection', Xrm.Sdk.Entity);
ClientUI.ViewModel.ConnectionsViewModel.registerClass('ClientUI.ViewModel.ConnectionsViewModel', SparkleXrm.ViewModelBase);
ClientUI.ViewModel.ObservableConnection.registerClass('ClientUI.ViewModel.ObservableConnection', SparkleXrm.ViewModelBase);
ClientUI.View.ConnectionsView.registerClass('ClientUI.View.ConnectionsView');
ResourceStrings.ConfirmDeleteSelectedConnection = null;
ResourceStrings.ConfirmDeleteConnection = null;
ResourceStrings.RequiredMessage = null;
ResourceStrings.SaveButton = null;
ResourceStrings.CancelButton = null;
ResourceStrings.Connection_CollectionName = null;
ResourceStrings.ConnectTo = null;
ResourceStrings.Role = null;
ClientUI.Model.Connection.logicalName = 'connection';
ClientUI.View.ConnectionsView._vm = null;
ClientUI.View.ConnectionsView._connectionsGrid = null;
})(window.xrmjQuery);


