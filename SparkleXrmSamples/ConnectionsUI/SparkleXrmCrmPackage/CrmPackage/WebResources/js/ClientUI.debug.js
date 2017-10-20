//! ClientUI.debug.js
//

(function($){

Type.registerNamespace('ClientUI');

////////////////////////////////////////////////////////////////////////////////
// ResourceStrings

ResourceStrings = function ResourceStrings() {
}


Type.registerNamespace('ClientUI.ViewModels');

////////////////////////////////////////////////////////////////////////////////
// SearchTermOptions

SearchTermOptions = function() { };
SearchTermOptions.prototype = {
    none: 0, 
    prefixWildcard: 1, 
    suffixWildcard: 2
}
SearchTermOptions.registerEnum('SearchTermOptions', true);


////////////////////////////////////////////////////////////////////////////////
// ClientUI.ViewModels.QueryParser

ClientUI.ViewModels.QueryParser = function ClientUI_ViewModels_QueryParser(entities) {
    this.entityLookup = {};
    this._aliasEntityLookup = {};
    this._lookupAttributes = {};
    this.entities = entities;
}
ClientUI.ViewModels.QueryParser.getFetchXmlParentFilter = function ClientUI_ViewModels_QueryParser$getFetchXmlParentFilter(query, parentAttribute) {
    var fetchElement = query.fetchXml.find('fetch');
    fetchElement.attr('count', '{0}');
    fetchElement.attr('paging-cookie', '{1}');
    fetchElement.attr('page', '{2}');
    fetchElement.attr('returntotalrecordcount', 'true');
    fetchElement.attr('distinct', 'true');
    fetchElement.attr('no-lock', 'true');
    var orderByElement = fetchElement.find('order');
    query.orderByAttribute = orderByElement.attr('attribute');
    query.orderByDesending = orderByElement.attr('descending') === 'true';
    orderByElement.remove();
    var filter = fetchElement.find('entity>filter');
    if (filter != null) {
        var filterType = filter.attr('type');
        if (filterType === 'or') {
            var andFilter = $("<filter type='and'>" + filter.html() + '</filter>');
            filter.remove();
            filter = andFilter;
            fetchElement.find('entity').append(andFilter);
        }
    }
    var parentFilter = $("<condition attribute='" + parentAttribute + "' operator='eq' value='" + '#ParentRecordPlaceholder#' + "'/>");
    filter.append(parentFilter);
    return query.fetchXml.html().replaceAll('</entity>', '{3}</entity>');
}
ClientUI.ViewModels.QueryParser.prototype = {
    entities: null,
    
    getQuickFinds: function ClientUI_ViewModels_QueryParser$getQuickFinds() {
        return this._getViewDefinition(true, null);
    },
    
    getView: function ClientUI_ViewModels_QueryParser$getView(entityLogicalName, viewName) {
        return this._getViewDefinition(false, viewName);
    },
    
    _getViewDefinition: function ClientUI_ViewModels_QueryParser$_getViewDefinition(isQuickFind, viewName) {
        var getviewfetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='savedquery'>\r\n                                <attribute name='name' />\r\n                                <attribute name='fetchxml' />\r\n                                <attribute name='layoutxml' />\r\n                                <attribute name='returnedtypecode' />\r\n                                <filter type='and'>\r\n                                <filter type='or'>";
        var entityMetadata = [];
        var $enum1 = ss.IEnumerator.getEnumerator(this.entities);
        while ($enum1.moveNext()) {
            var entity = $enum1.current;
            entityMetadata.add(Xrm.Utility.getEntityMetadata(entity).then(function(metadata) {
                return Promise.resolve(metadata);
            }));
        }
        debugger;
        return Promise.all(entityMetadata).then(function(result) {
            debugger;
            var $enum1 = ss.IEnumerator.getEnumerator(result);
            while ($enum1.moveNext()) {
                var metadata = $enum1.current;
                getviewfetchXml += "<condition attribute='returnedtypecode' operator='eq' value='" + metadata.objectTypeCode.toString() + "'/>";
            }
            return Promise.resolve(true);
        }).then(ss.Delegate.create(this, function(ok) {
            getviewfetchXml += '</filter>';
            if (isQuickFind) {
                getviewfetchXml += "<condition attribute='isquickfindquery' operator='eq' value='1'/>\r\n                                    <condition attribute='isdefault' operator='eq' value='1'/>";
            }
            else if (viewName != null && viewName.length > 0) {
                getviewfetchXml += "<condition attribute='name' operator='eq' value='" + SparkleXrm.Sdk.XmlHelper.encode(viewName) + "'/>";
            }
            else {
                getviewfetchXml += "<condition attribute='querytype' operator='eq' value='2'/>\r\n                                    <condition attribute='isdefault' operator='eq' value='1'/>";
            }
            getviewfetchXml += '</filter>\r\n                              </entity>\r\n                            </fetch>';
            var quickFindQuery = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(getviewfetchXml);
            var entityLookup = {};
            var $enum1 = ss.IEnumerator.getEnumerator(quickFindQuery.get_entities());
            while ($enum1.moveNext()) {
                var view = $enum1.current;
                entityLookup[view.getAttributeValueString('returnedtypecode')] = view;
            }
            var $enum2 = ss.IEnumerator.getEnumerator(this.entities);
            while ($enum2.moveNext()) {
                var typeName = $enum2.current;
                var view = entityLookup[typeName];
                var fetchXml = view.getAttributeValueString('fetchxml');
                var layoutXml = view.getAttributeValueString('layoutxml');
                var query;
                if (Object.keyExists(this.entityLookup, typeName)) {
                    query = this.entityLookup[typeName];
                }
                else {
                    query = {};
                    query.logicalName = typeName;
                    query.views = {};
                    query.attributes = {};
                    this.entityLookup[typeName] = query;
                }
                var config = this._parse(fetchXml, layoutXml);
                query.views[view.getAttributeValueString('name')] = config;
                if (isQuickFind) {
                    query.quickFindQuery = config;
                }
            }
            return Promise.resolve(true);
        }));
    },
    
    _parse: function ClientUI_ViewModels_QueryParser$_parse(fetchXml, layoutXml) {
        var querySettings = {};
        var fetchXmlDOM = $('<query>' + fetchXml.replaceAll('{0}', '#Query#').replaceAll('{1}', '#QueryInt#').replaceAll('{2}', '#QueryCurrency#').replaceAll('{3}', '#QueryDateTime#').replaceAll('{4}', '#QueryFloat#') + '</query>');
        var fetchElement = fetchXmlDOM.find('fetch');
        querySettings.fetchXml = fetchXmlDOM;
        this._parseFetchXml(querySettings);
        querySettings.columns = this._parseLayoutXml(querySettings.rootEntity, layoutXml);
        return querySettings;
    },
    
    _parseLayoutXml: function ClientUI_ViewModels_QueryParser$_parseLayoutXml(rootEntity, layoutXml) {
        var layout = $(layoutXml);
        var cells = layout.find('cell');
        var columns = [];
        cells.each(ss.Delegate.create(this, function(index, element) {
            var cellName = element.getAttribute('name').toString();
            var logicalName = cellName;
            var entity;
            var attribute;
            var pos = cellName.indexOf('.');
            if (pos > -1) {
                var alias = cellName.substr(0, pos);
                logicalName = cellName.substr(pos + 1);
                entity = this._aliasEntityLookup[alias];
            }
            else {
                entity = rootEntity;
            }
            if (Object.keyExists(entity.attributes, logicalName)) {
                attribute = entity.attributes[logicalName];
            }
            else {
                attribute = {};
                attribute.columns = [];
                attribute.logicalName = logicalName;
                entity.attributes[attribute.logicalName] = attribute;
            }
            var widthAttribute = element.getAttribute('width');
            if (widthAttribute != null) {
                var width = parseInt(element.getAttribute('width').toString());
                var disableSorting = element.getAttribute('disableSorting');
                var col = SparkleXrm.GridEditor.GridDataViewBinder.newColumn(attribute.logicalName, attribute.logicalName, width);
                col.sortable = !(disableSorting != null && disableSorting.toString() === '1');
                attribute.columns.add(col);
                columns.add(col);
            }
        }));
        return columns;
    },
    
    queryMetadata: function ClientUI_ViewModels_QueryParser$queryMetadata() {
        var builder = new SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder();
        var entities = [];
        var attributes = [];
        var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(this.entityLookup));
        while ($enum1.moveNext()) {
            var entityLogicalName = $enum1.current;
            entities.add(entityLogicalName);
            var entity = this.entityLookup[entityLogicalName];
            var $enum2 = ss.IEnumerator.getEnumerator(Object.keys(entity.attributes));
            while ($enum2.moveNext()) {
                var attributeLogicalName = $enum2.current;
                var attribute = entity.attributes[attributeLogicalName];
                var fieldName = attribute.logicalName;
                var pos = fieldName.indexOf('.');
                if (entity.aliasName != null && pos > -1) {
                    fieldName = fieldName.substr(pos);
                }
                attributes.add(fieldName);
            }
        }
        builder.addEntities(entities, ['Attributes', 'DisplayName', 'DisplayCollectionName', 'PrimaryImageAttribute']);
        builder.addAttributes(attributes, ['DisplayName', 'AttributeType', 'IsPrimaryName']);
        builder.setLanguage(USER_LANGUAGE_CODE);
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        var $enum3 = ss.IEnumerator.getEnumerator(response.entityMetadata);
        while ($enum3.moveNext()) {
            var entityMetadata = $enum3.current;
            var entityQuery = this.entityLookup[entityMetadata.logicalName];
            entityQuery.displayName = entityMetadata.displayName.userLocalizedLabel.label;
            entityQuery.displayCollectionName = entityMetadata.displayCollectionName.userLocalizedLabel.label;
            entityQuery.primaryImageAttribute = entityMetadata.primaryImageAttribute;
            entityQuery.entityTypeCode = entityMetadata.objectTypeCode;
            var $enum4 = ss.IEnumerator.getEnumerator(entityMetadata.attributes);
            while ($enum4.moveNext()) {
                var attribute = $enum4.current;
                if (Object.keyExists(entityQuery.attributes, attribute.logicalName)) {
                    var attributeQuery = entityQuery.attributes[attribute.logicalName];
                    attributeQuery.attributeType = attribute.attributeType;
                    switch (attribute.attributeType) {
                        case 'Lookup':
                        case 'Picklist':
                        case 'Customer':
                        case 'Owner':
                        case 'Status':
                        case 'State':
                        case 'Boolean':
                            this._lookupAttributes[attribute.logicalName] = attributeQuery;
                            break;
                    }
                    attributeQuery.isPrimaryName = attribute.isPrimaryName;
                    var $enum5 = ss.IEnumerator.getEnumerator(attributeQuery.columns);
                    while ($enum5.moveNext()) {
                        var col = $enum5.current;
                        col.name = attribute.displayName.userLocalizedLabel.label;
                        col.dataType = (attribute.isPrimaryName) ? 'PrimaryNameLookup' : attribute.attributeType;
                    }
                }
            }
        }
    },
    
    _parseFetchXml: function ClientUI_ViewModels_QueryParser$_parseFetchXml(querySettings) {
        var fetchElement = querySettings.fetchXml;
        var entityElement = fetchElement.find('entity');
        var logicalName = entityElement.attr('name');
        var rootEntity;
        if (!Object.keyExists(this.entityLookup, logicalName)) {
            rootEntity = {};
            rootEntity.logicalName = logicalName;
            rootEntity.attributes = {};
            this.entityLookup[rootEntity.logicalName] = rootEntity;
        }
        else {
            rootEntity = this.entityLookup[logicalName];
        }
        var linkEntities = entityElement.find('link-entity');
        linkEntities.each(ss.Delegate.create(this, function(index, element) {
            var link = {};
            link.attributes = {};
            link.aliasName = element.getAttribute('alias').toString();
            link.logicalName = element.getAttribute('name').toString();
            link.views = {};
            if (!Object.keyExists(this.entityLookup, link.logicalName)) {
                this.entityLookup[link.logicalName] = link;
            }
            else {
                var alias = link.aliasName;
                link = this.entityLookup[link.logicalName];
                link.aliasName = alias;
            }
            if (!Object.keyExists(this._aliasEntityLookup, link.aliasName)) {
                this._aliasEntityLookup[link.aliasName] = link;
            }
        }));
        querySettings.rootEntity = rootEntity;
        var conditions = fetchElement.find("filter[isquickfindfields='1']");
        conditions.first().children().each(function(index, element) {
            logicalName = element.getAttribute('attribute').toString();
            var e = $(element);
            var p = e.parents('link-entity');
            if (!Object.keyExists(querySettings.rootEntity.attributes, logicalName)) {
                var attribute = {};
                attribute.logicalName = logicalName;
                attribute.columns = [];
                querySettings.rootEntity.attributes[logicalName] = attribute;
            }
        });
    },
    
    getFetchXmlForQuery: function ClientUI_ViewModels_QueryParser$getFetchXmlForQuery(entityLogicalName, queryName, searchTerm, searchOptions) {
        var config;
        if (queryName === 'QuickFind') {
            config = this.entityLookup[entityLogicalName].quickFindQuery;
        }
        else {
            config = this.entityLookup[entityLogicalName].views[queryName];
        }
        var fetchElement = config.fetchXml.clone().find('fetch');
        fetchElement.attr('distinct', 'true');
        fetchElement.attr('no-lock', 'true');
        var orderByElement = fetchElement.find('order');
        orderByElement.remove();
        var conditions = fetchElement.find("filter[isquickfindfields='1']");
        conditions.first().children().each(ss.Delegate.create(this, function(index, element) {
            var logicalName = element.getAttribute('attribute').toString();
            if (Object.keyExists(this._lookupAttributes, logicalName)) {
                element.setAttribute('attribute', logicalName + 'name');
            }
        }));
        if (isNaN(parseInt(searchTerm))) {
            fetchElement.find("condition[value='#QueryInt#']").remove();
        }
        if (isNaN(parseFloat(searchTerm))) {
            fetchElement.find("condition[value='#QueryCurrency#']").remove();
        }
        if (isNaN(Date.parseDate(searchTerm).getDate())) {
            fetchElement.find("condition[value='#QueryDateTime#']").remove();
        }
        if (isNaN(parseFloat(searchTerm))) {
            fetchElement.find("condition[value='#QueryFloat#']").remove();
        }
        var fetchXml = fetchElement.parent().html();
        var textSearchTerm = searchTerm;
        if (searchOptions != null && (searchOptions & 1) === 1) {
            while (textSearchTerm.startsWith('*') || textSearchTerm.startsWith('%')) {
                textSearchTerm = textSearchTerm.substring(1, textSearchTerm.length);
            }
            textSearchTerm = '%' + textSearchTerm;
        }
        if (searchOptions != null && (searchOptions & 2) === 2) {
            while (textSearchTerm.endsWith('*') || textSearchTerm.endsWith('%')) {
                textSearchTerm = textSearchTerm.substring(0, textSearchTerm.length - 1);
            }
            textSearchTerm = textSearchTerm + '%';
        }
        fetchXml = fetchXml.replaceAll('#Query#', SparkleXrm.Sdk.XmlHelper.encode(textSearchTerm)).replaceAll('#QueryInt#', parseInt(searchTerm).toString()).replaceAll('#QueryCurrency#', parseFloat(searchTerm).toString()).replaceAll('#QueryDateTime#', SparkleXrm.Sdk.XmlHelper.encode(Date.parseDate(searchTerm).format('MM/dd/yyyy'))).replaceAll('#QueryFloat#', parseFloat(searchTerm).toString());
        return fetchXml;
    }
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
    record2roleid: null,
    description: null,
    effectivestart: null,
    effectiveend: null
}


Type.registerNamespace('ClientUI.ViewModel');

////////////////////////////////////////////////////////////////////////////////
// ClientUI.ViewModel.ConnectionsViewModel

ClientUI.ViewModel.ConnectionsViewModel = function ClientUI_ViewModel_ConnectionsViewModel(parentRecordId, connectToTypes, pageSize, view) {
    this.SelectedConnection = ko.observable();
    this.ErrorMessage = ko.observable();
    this.parentRecordId = ko.observable();
    ClientUI.ViewModel.ConnectionsViewModel.initializeBase(this);
    this.Connections = new SparkleXrm.GridEditor.EntityDataViewModel(pageSize, ClientUI.Model.Connection, true);
    if (view != null) {
        this._viewFetchXml$1 = ClientUI.ViewModels.QueryParser.getFetchXmlParentFilter(view, 'record1id');
        this._defaultSortCol$1 = new SparkleXrm.GridEditor.SortCol(view.orderByAttribute, !view.orderByDesending);
    }
    this.parentRecordId(parentRecordId);
    var connection = new ClientUI.ViewModel.ObservableConnection(connectToTypes);
    connection.record2id(parentRecordId);
    this.ConnectionEdit = ko.validatedObservable(connection);
    this.Connections.onDataLoaded.subscribe(ss.Delegate.create(this, this._connections_OnDataLoaded$1));
    this.ConnectionEdit().add_onSaveComplete(ss.Delegate.create(this, this._connectionsViewModel_OnSaveComplete$1));
    ClientUI.ViewModel.ObservableConnection.registerValidation(this.Connections.validationBinder);
    this.AllowAddNew = ko.dependentObservable(ss.Delegate.create(this, this.allowAddNewComputed));
}
ClientUI.ViewModel.ConnectionsViewModel.prototype = {
    Connections: null,
    ConnectionEdit: null,
    AllowAddNew: null,
    _viewFetchXml$1: null,
    _defaultSortCol$1: null,
    
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
        var connectionToUpdate = new ClientUI.Model.Connection();
        var updated = sender;
        connectionToUpdate.connectionid = new SparkleXrm.Sdk.Guid(updated.id);
        var updateRequired = false;
        switch (e.propertyName) {
            case 'record2roleid':
                if (updated.record1id == null) {
                    var connection = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(ClientUI.Model.Connection.logicalName, updated.connectionid.value, [ 'record1id' ]);
                    updated.record1id = connection.record1id;
                }
                connectionToUpdate.record2roleid = updated.record2roleid;
                connectionToUpdate.record1roleid = ClientUI.ViewModel.ObservableConnection.getOppositeRole(updated.record2roleid, updated.record1id);
                updateRequired = true;
                break;
            case 'description':
                connectionToUpdate.description = updated.description;
                updateRequired = true;
                break;
            case 'effectivestart':
                connectionToUpdate.effectivestart = updated.effectivestart;
                updateRequired = true;
                break;
            case 'effectiveend':
                connectionToUpdate.effectiveend = updated.effectiveend;
                updateRequired = true;
                break;
        }
        if (updateRequired) {
            SparkleXrm.Sdk.OrganizationServiceProxy.beginUpdate(connectionToUpdate, ss.Delegate.create(this, function(state) {
                try {
                    SparkleXrm.Sdk.OrganizationServiceProxy.endUpdate(state);
                    this.ErrorMessage(null);
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
        var parentRecordId = this.parentRecordId().id.toString().replaceAll('{', '').replaceAll('}', '');
        if (this._viewFetchXml$1 == null) {
            this.Connections.set_fetchXml("<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>\r\n                                  <entity name='connection'>\r\n                                    <attribute name='record2id' />\r\n                                    <attribute name='record2roleid' />\r\n                                    <attribute name='record1id' />\r\n                                    <attribute name='record1roleid' />\r\n                                    <attribute name='connectionid' />\r\n                                    <filter type='and'>\r\n                                      \r\n                                      <condition attribute='record2id' operator='eq' value='" + parentRecordId + "' />\r\n                                    </filter>\r\n                                  {3}\r\n                                  </entity>\r\n                                </fetch>");
            this.Connections.refresh();
        }
        else {
            this.Connections.set_fetchXml(this._viewFetchXml$1.replaceAll('#ParentRecordPlaceholder#', parentRecordId));
            this.Connections.sortBy(this._defaultSortCol$1);
        }
    },
    
    RoleSearchCommand: function ClientUI_ViewModel_ConnectionsViewModel$RoleSearchCommand(term, callback) {
        ClientUI.ViewModel.ObservableConnection.RoleSearch(term, callback, this.SelectedConnection().record2id.logicalName);
    },
    
    AddNewCommand: function ClientUI_ViewModel_ConnectionsViewModel$AddNewCommand() {
        this.ConnectionEdit().record2id(this.parentRecordId());
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
                    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(item.logicalName, new SparkleXrm.Sdk.Guid(item.id));
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
            SparkleXrm.Sdk.OrganizationServiceProxy.beginDelete(ClientUI.Model.Connection.logicalName, new SparkleXrm.Sdk.Guid(id), ss.Delegate.create(this, function(state) {
                try {
                    SparkleXrm.Sdk.OrganizationServiceProxy.endDelete(state);
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
    },
    
    allowAddNewComputed: function ClientUI_ViewModel_ConnectionsViewModel$allowAddNewComputed() {
        var parent = this.parentRecordId();
        return parent != null && parent.id != null && parent.id.value != null && parent.id.value.length > 0;
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
    this.description = ko.observable();
    ClientUI.ViewModel.ObservableConnection.initializeBase(this);
    this._connectToTypes$1 = types;
    ClientUI.ViewModel.ObservableConnection.registerValidation(new SparkleXrm.ObservableValidationBinder(this));
}
ClientUI.ViewModel.ObservableConnection.RoleSearch = function ClientUI_ViewModel_ObservableConnection$RoleSearch(term, callback, typeName) {
    var recordTypeFilter = '';
    if (typeName != null) {
        var etc = ClientUI.ViewModel.ObservableConnection._getEntityTypeCodeFromName$1(typeName);
        recordTypeFilter = String.format("\r\n                                        <filter type='or'>\r\n                                            <condition attribute='associatedobjecttypecode' operator='eq' value='{0}' />\r\n                                            <condition attribute='associatedobjecttypecode' operator='eq' value='0' />\r\n                                        </filter>", etc);
    }
    var fetchXml = "\r\n                            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true' >\r\n                                <entity name='connectionrole' >\r\n                                    <attribute name='category' />\r\n                                    <attribute name='name' />\r\n                                    <attribute name='connectionroleid' />\r\n                                    <attribute name='statecode' />\r\n                                    <order attribute='name' descending='false' />\r\n                                    <link-entity name='connectionroleobjecttypecode' from='connectionroleid' to='connectionroleid' >\r\n                                    {1}\r\n                                    </link-entity>\r\n                                    <filter type='and'>                                     \r\n                                        <condition attribute='name' operator='like' value='%{0}%' />\r\n                                    </filter>\r\n                                </entity>\r\n                            </fetch>";
    fetchXml = String.format(fetchXml, SparkleXrm.Sdk.XmlHelper.encode(term), recordTypeFilter);
    SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
        var fetchResult = SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, SparkleXrm.Sdk.Entity);
        callback(fetchResult);
    });
}
ClientUI.ViewModel.ObservableConnection._getEntityTypeCodeFromName$1 = function ClientUI_ViewModel_ObservableConnection$_getEntityTypeCodeFromName$1(typeName) {
    var etc = Mscrm.EntityPropUtil.EntityTypeName2CodeMap[typeName];
    return etc;
}
ClientUI.ViewModel.ObservableConnection.getOppositeRole = function ClientUI_ViewModel_ObservableConnection$getOppositeRole(role, record) {
    var oppositeRole = null;
    var etc = ClientUI.ViewModel.ObservableConnection._getEntityTypeCodeFromName$1(record.logicalName);
    var getOppositeRole = String.format("<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' count='1'>\r\n                          <entity name='connectionrole'>\r\n                            <attribute name='category' />\r\n                            <attribute name='name' />\r\n                            <attribute name='connectionroleid' />\r\n                            <attribute name='statecode' />\r\n                            <filter type='and'>\r\n                              <condition attribute='statecode' operator='eq' value='0' />\r\n                            </filter>\r\n                            <link-entity name='connectionroleassociation' from='connectionroleid' to='connectionroleid' intersect='true'>\r\n                                  <link-entity name='connectionrole' from='connectionroleid' to='associatedconnectionroleid' alias='ad'>\r\n                                    <filter type='and'>\r\n                                      <condition attribute='connectionroleid' operator='eq' value='{0}' />\r\n                                    </filter>\r\n                                  </link-entity>\r\n                                 <link-entity name='connectionroleobjecttypecode' from='connectionroleid' to='connectionroleid' intersect='true' >\r\n                                    <filter type='or' >\r\n                                        <condition attribute='associatedobjecttypecode' operator='eq' value='{1}' />\r\n                                        <condition attribute='associatedobjecttypecode' operator='eq' value='0' /> <!-- All types-->\r\n                                    </filter>\r\n                                </link-entity>\r\n                            </link-entity>\r\n                          </entity>\r\n                        </fetch>", role.id.toString(), etc);
    var results = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(getOppositeRole);
    if (results.get_entities().get_count() > 0) {
        oppositeRole = results.get_entities().get_item(0).toEntityReference();
    }
    return oppositeRole;
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
    _queryParser$1: null,
    _connectToTypes$1: null,
    
    RecordSearchCommand: function ClientUI_ViewModel_ObservableConnection$RecordSearchCommand(term, callback) {
        if (this._queryParser$1 == null) {
            this._queryParser$1 = new ClientUI.ViewModels.QueryParser(this._connectToTypes$1);
            this._queryParser$1.getQuickFinds();
            this._queryParser$1.queryMetadata();
        }
        var resultsBack = 0;
        var mergedEntities = [];
        var result = ss.Delegate.create(this, function(fetchResult) {
            resultsBack++;
            var config = this._queryParser$1.entityLookup[fetchResult.get_entityName()].quickFindQuery;
            var $enum1 = ss.IEnumerator.getEnumerator(fetchResult.get_entities());
            while ($enum1.moveNext()) {
                var row = $enum1.current;
                var entityRow = row;
                var columnCount = (config.columns.length < 3) ? config.columns.length : 3;
                for (var i = 0; i < columnCount; i++) {
                    var aliasName = 'col' + i.toString();
                    row[aliasName] = row[config.columns[i].field];
                    if (Object.keyExists(entityRow.formattedValues, config.columns[i].field + 'name')) {
                        entityRow.formattedValues[aliasName + 'name'] = entityRow.formattedValues[config.columns[i].field + 'name'];
                    }
                    else {
                        entityRow.formattedValues[aliasName] = Type.safeCast(entityRow.getAttributeValue(config.columns[i].field), String);
                    }
                }
            }
            mergedEntities.addRange(fetchResult.get_entities().items());
            mergedEntities.sort(function(x, y) {
                return String.compare(x.getAttributeValueString('name'), y.getAttributeValueString('name'));
            });
            if (resultsBack === this._connectToTypes$1.length) {
                var results = new SparkleXrm.Sdk.EntityCollection(mergedEntities);
                callback(results);
            }
        });
        var $enum1 = ss.IEnumerator.getEnumerator(this._connectToTypes$1);
        while ($enum1.moveNext()) {
            var entity = $enum1.current;
            this._searchRecords$1(term, result, entity);
        }
    },
    
    _searchRecords$1: function ClientUI_ViewModel_ObservableConnection$_searchRecords$1(term, callback, entityType) {
        var fetchXml = this._queryParser$1.getFetchXmlForQuery(entityType, 'QuickFind', term, 1 | 2);
        SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, SparkleXrm.Sdk.Entity);
            fetchResult.set_entityName(entityType);
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
        var oppositeRole = ClientUI.ViewModel.ObservableConnection.getOppositeRole(connection.record1roleid, connection.record2id);
        connection.record2roleid = oppositeRole;
        SparkleXrm.Sdk.OrganizationServiceProxy.beginCreate(connection, ss.Delegate.create(this, function(state) {
            try {
                this.connectiondid(SparkleXrm.Sdk.OrganizationServiceProxy.endCreate(state));
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
    SparkleXrm.Xrm.PageEx.majorVersion = 2013;
    var lcid = SparkleXrm.Sdk.OrganizationServiceProxy.getUserSettings().uilanguageid;
    SparkleXrm.LocalisedContentLoader.fallBackLCID = 0;
    SparkleXrm.LocalisedContentLoader.supportedLCIDs.add(0);
    SparkleXrm.LocalisedContentLoader.loadContent('con_/js/Res.metadata.js', lcid, function() {
        ClientUI.View.ConnectionsView._initLocalisedContent();
    });
}
ClientUI.View.ConnectionsView._initLocalisedContent = function ClientUI_View_ConnectionsView$_initLocalisedContent() {
    var parameters;
    var id;
    var logicalName;
    var pageSize = 10;
    var defaultView = null;
    parameters = SparkleXrm.Xrm.PageEx.getWebResourceData();
    id = window.parent.Xrm.Page.data.entity.getId();
    logicalName = window.parent.Xrm.Page.data.entity.getEntityName();
    window.parent.Xrm.Page.data.entity.addOnSave(ClientUI.View.ConnectionsView._checkForSaved);
    var parent = new SparkleXrm.Sdk.EntityReference(new SparkleXrm.Sdk.Guid(id), logicalName, null);
    var entities = 'account,contact,opportunity,systemuser';
    var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(parameters));
    while ($enum1.moveNext()) {
        var key = $enum1.current;
        switch (key.toLowerCase()) {
            case 'entities':
                entities = parameters[key];
                break;
            case 'pageSize':
                pageSize = parseInt(parameters[key]);
                break;
            case 'view':
                defaultView = parameters[key];
                break;
        }
    }
    $(window).resize(ClientUI.View.ConnectionsView._onResize);
    $(function() {
        window.setTimeout(function() {
            var queryParser = new ClientUI.ViewModels.QueryParser([ 'connection' ]);
            queryParser.getView('connection', defaultView);
            queryParser.queryMetadata();
            var connectionViews = queryParser.entityLookup['connection'];
            var viewName = Object.keys(connectionViews.views)[0];
            var view = connectionViews.views[viewName];
            ClientUI.View.ConnectionsView._vm = new ClientUI.ViewModel.ConnectionsViewModel(parent, entities.split(','), pageSize, view);
            var connectionsGridDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
            var columns = view.columns;
            var $enum1 = ss.IEnumerator.getEnumerator(columns);
            while ($enum1.moveNext()) {
                var col = $enum1.current;
                switch (col.field) {
                    case 'record2roleid':
                        SparkleXrm.GridEditor.XrmLookupEditor.bindColumn(col, ss.Delegate.create(ClientUI.View.ConnectionsView._vm, ClientUI.View.ConnectionsView._vm.RoleSearchCommand), 'connectionroleid', 'name,category', '');
                        break;
                    case 'description':
                        SparkleXrm.GridEditor.XrmTextEditor.bindColumn(col);
                        break;
                    case 'effectivestart':
                    case 'effectiveend':
                        SparkleXrm.GridEditor.XrmDateEditor.bindColumn(col, true);
                        break;
                }
            }
            ClientUI.View.ConnectionsView._connectionsGrid = connectionsGridDataBinder.dataBindXrmGrid(ClientUI.View.ConnectionsView._vm.Connections, columns, 'container', 'pager', true, false);
            ClientUI.View.ConnectionsView._connectionsGrid.onActiveCellChanged.subscribe(function(e, data) {
                var eventData = data;
                ClientUI.View.ConnectionsView._vm.SelectedConnection(ClientUI.View.ConnectionsView._connectionsGrid.getDataItem(eventData.row));
            });
            connectionsGridDataBinder.bindClickHandler(ClientUI.View.ConnectionsView._connectionsGrid);
            SparkleXrm.ViewBase.registerViewModel(ClientUI.View.ConnectionsView._vm);
            ClientUI.View.ConnectionsView._overrideMetadata();
            ClientUI.View.ConnectionsView._onResize(null);
            ClientUI.View.ConnectionsView._vm.search();
        }, 15000);
    });
}
ClientUI.View.ConnectionsView._checkForSaved = function ClientUI_View_ConnectionsView$_checkForSaved() {
    var parent = new SparkleXrm.Sdk.EntityReference(new SparkleXrm.Sdk.Guid(window.parent.Xrm.Page.data.entity.getId()), window.parent.Xrm.Page.data.entity.getEntityName(), null);
    if (window.parent.Xrm.Page.ui.getFormType() !== 10*.1 && parent.id != null) {
        ClientUI.View.ConnectionsView._vm.parentRecordId(parent);
        ClientUI.View.ConnectionsView._vm.search();
    }
    else {
        window.setTimeout(ClientUI.View.ConnectionsView._checkForSaved, 1000);
    }
}
ClientUI.View.ConnectionsView._overrideMetadata = function ClientUI_View_ConnectionsView$_overrideMetadata() {
    var getSmallIconUrl = SparkleXrm.Sdk.Metadata.MetadataCache.getSmallIconUrl;
    var overrideMethod = function(typeName) {
        switch (typeName) {
            case 'connectionrole':
                return '/_imgs/ico_16_3234.gif';
            default:
                return getSmallIconUrl(typeName);
        }
    };
    SparkleXrm.Sdk.Metadata.MetadataCache.getSmallIconUrl=overrideMethod;
}
ClientUI.View.ConnectionsView._onResize = function ClientUI_View_ConnectionsView$_onResize(e) {
    var height = $(window).height();
    var width = $(window).width();
    $('#container').height(height - 64).width(width - 1);
    if (ClientUI.View.ConnectionsView._connectionsGrid != null) {
        ClientUI.View.ConnectionsView._connectionsGrid.resizeCanvas();
    }
}


ResourceStrings.registerClass('ResourceStrings');
ClientUI.ViewModels.QueryParser.registerClass('ClientUI.ViewModels.QueryParser');
ClientUI.View.GridPlugins.RowHoverPlugin.registerClass('ClientUI.View.GridPlugins.RowHoverPlugin', null, Object);
ClientUI.Model.Connection.registerClass('ClientUI.Model.Connection', SparkleXrm.Sdk.Entity);
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
ClientUI.ViewModels.QueryParser.parentRecordPlaceholder = '#ParentRecordPlaceholder#';
ClientUI.Model.Connection.logicalName = 'connection';
ClientUI.View.ConnectionsView._vm = null;
ClientUI.View.ConnectionsView._connectionsGrid = null;
})(window.xrmjQuery);


