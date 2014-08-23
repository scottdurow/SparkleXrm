//! ClientUI.debug.js
//
waitForScripts("client",["mscorlib","xrm","xrmui", "jquery", "jquery-ui"],
function () {

(function($){

Type.registerNamespace('SparkleXrm.GridEditor');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel

SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel = function SparkleXrm_GridEditor_VirtualPagedEntityDataViewModel(pageSize, entityType, lazyLoadPages) {
    this._pendingRefresh$2 = [];
    this._pagesLoaded$2 = [];
    SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel.initializeBase(this, [ pageSize, entityType, lazyLoadPages ]);
    this._entityType = entityType;
    this._lazyLoadPages = lazyLoadPages;
    this._data = [];
    this.paging.pageSize = pageSize;
    this.paging.pageNum = 0;
    this.paging.totalPages = 0;
    this.paging.totalRows = 0;
    this.paging.fromRecord = 0;
    this.paging.toRecord = 0;
}
SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel.prototype = {
    _batchSize$2: 10,
    
    getLength: function SparkleXrm_GridEditor_VirtualPagedEntityDataViewModel$getLength() {
        return this.paging.totalRows;
    },
    
    getItem: function SparkleXrm_GridEditor_VirtualPagedEntityDataViewModel$getItem(index) {
        var requestedPage = Math.floor(index / this.paging.pageSize);
        if ((this.paging.totalRows > 0) && (this._pagesLoaded$2[requestedPage] == null)) {
            if (this._suspendRefresh) {
                var singlePageIncrement = true;
                if (this._pendingRefresh$2.length > 0) {
                    singlePageIncrement = Math.abs(this._pendingRefresh$2.peek() - requestedPage) > 3;
                }
                if (!this._pendingRefresh$2.contains(requestedPage)) {
                    this._pendingRefresh$2.push(requestedPage);
                }
            }
            else {
                this.paging.pageNum = requestedPage;
                this.refresh();
            }
        }
        return this._data[index];
    },
    
    reset: function SparkleXrm_GridEditor_VirtualPagedEntityDataViewModel$reset() {
        SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel.callBaseMethod(this, 'reset');
        this._pagesLoaded$2 = [];
        this.getPagingInfo().pageNum = 0;
        this.getPagingInfo().totalRows = 0;
        this.getPagingInfo().fromRecord = 0;
        this.getPagingInfo().toRecord = 0;
        this.setPagingOptions(this.getPagingInfo());
    },
    
    refresh: function SparkleXrm_GridEditor_VirtualPagedEntityDataViewModel$refresh() {
        var requestedPage = this.paging.pageNum;
        var pageLoadState = this._pagesLoaded$2[requestedPage];
        if (!!this._suspendRefresh) {
            return;
        }
        this._suspendRefresh = true;
        var allDataDeleted = (!this.paging.totalRows) && (this.deleteData != null) && (this.deleteData.length > 0);
        var rows = [];
        var firstRowIndex = requestedPage * this.paging.pageSize;
        if (pageLoadState !== 1 && pageLoadState !== 2) {
            if (String.isNullOrEmpty(this._fetchXml)) {
                this._suspendRefresh = false;
                return;
            }
            this._pagesLoaded$2[requestedPage] = 1;
            this.onDataLoading.notify(null, null, null);
            var orderBy = this.applySorting();
            var fetchPageSize;
            fetchPageSize = this.paging.pageSize;
            var parameterisedFetchXml = String.format(this._fetchXml, fetchPageSize, Xrm.Sdk.XmlHelper.encode(this.paging.extraInfo), requestedPage + 1, orderBy);
            Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(parameterisedFetchXml, ss.Delegate.create(this, function(result) {
                try {
                    var results = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, this._entityType);
                    var i = firstRowIndex;
                    if (this._lazyLoadPages) {
                        var $enum1 = ss.IEnumerator.getEnumerator(results.get_entities());
                        while ($enum1.moveNext()) {
                            var e = $enum1.current;
                            this._data[i] = e;
                            Xrm.ArrayEx.add(rows, i);
                            i = i + 1;
                        }
                    }
                    else {
                        this._data = results.get_entities().items();
                    }
                    this._pagesLoaded$2[requestedPage] = 2;
                    this.paging.totalRows = results.get_totalRecordCount();
                    this.paging.extraInfo = results.get_pagingCookie();
                    this.paging.fromRecord = firstRowIndex + 1;
                    this.paging.totalPages = Math.ceil(results.get_totalRecordCount() / this.paging.pageSize);
                    this.paging.toRecord = Math.min(results.get_totalRecordCount(), firstRowIndex + this.paging.pageSize);
                    if (this._itemAdded) {
                        this.paging.totalRows++;
                        this.paging.toRecord++;
                        this._itemAdded = false;
                    }
                    this.onPagingInfoChanged.notify(this.getPagingInfo(), null, null);
                    var args = {};
                    args.from = firstRowIndex;
                    args.to = firstRowIndex + this.paging.pageSize - 1;
                    this.onDataLoaded.notify(args, null, null);
                    this._finishSuspend$2();
                }
                catch (ex) {
                    var quickFindLimit = ex.message.indexOf('QuickFindQueryRecordLimit') > -1;
                    this._pagesLoaded$2[requestedPage] = 2;
                    this.paging.totalRows = 5001;
                    this.onPagingInfoChanged.notify(this.getPagingInfo(), null, null);
                    var args = {};
                    if (!quickFindLimit) {
                        this.errorMessage = ex.message;
                        args.errorMessage = ex.message;
                    }
                    this.onDataLoaded.notify(args, null, null);
                    this._finishSuspend$2();
                }
            }));
        }
        else if (pageLoadState === 2) {
            var args = {};
            args.from = 0;
            args.to = this.paging.pageSize - 1;
            this.paging.fromRecord = firstRowIndex + 1;
            this.paging.toRecord = Math.min(this.paging.totalRows, firstRowIndex + this.paging.pageSize);
            this.onPagingInfoChanged.notify(this.getPagingInfo(), null, null);
            this.onDataLoaded.notify(args, null, null);
            this._itemAdded = false;
            this._finishSuspend$2();
        }
    },
    
    _finishSuspend$2: function SparkleXrm_GridEditor_VirtualPagedEntityDataViewModel$_finishSuspend$2() {
        this._suspendRefresh = false;
        if (this._pendingRefresh$2.length > 0) {
            this.paging.pageNum = this._pendingRefresh$2.pop();
            this.refresh();
        }
    }
}


Type.registerNamespace('Client.MultiEntitySearch.ViewModels');

////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.ViewModels.ExecuteFetchRequest

Client.MultiEntitySearch.ViewModels.ExecuteFetchRequest = function Client_MultiEntitySearch_ViewModels_ExecuteFetchRequest() {
}
Client.MultiEntitySearch.ViewModels.ExecuteFetchRequest.prototype = {
    fetchXml: null,
    
    serialise: function Client_MultiEntitySearch_ViewModels_ExecuteFetchRequest$serialise() {
        return String.format('<request i:type="b:ExecuteFetchRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '        <a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>FetchXml</c:key>' + '            <c:value i:type="d:string" xmlns:d="http://www.w3.org/2001/XMLSchema" >' + Xrm.Sdk.XmlHelper.encode(this.fetchXml) + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '        </a:Parameters>' + '        <a:RequestId i:nil="true" />' + '        <a:RequestName>ExecuteFetch</a:RequestName>' + '      </request>');
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.ViewModels.ExecuteFetchResponse

Client.MultiEntitySearch.ViewModels.ExecuteFetchResponse = function Client_MultiEntitySearch_ViewModels_ExecuteFetchResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (Xrm.Sdk.XmlHelper.getNodeTextValue(key) === 'FetchXmlResult') {
            var value = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.fetchXmlResult = Xrm.Sdk.XmlHelper.getNodeTextValue(value);
        }
    }
}
Client.MultiEntitySearch.ViewModels.ExecuteFetchResponse.prototype = {
    fetchXmlResult: null
}


////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.ViewModels.MultiSearchViewModel

Client.MultiEntitySearch.ViewModels.MultiSearchViewModel = function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel() {
    this.searchTerm = ko.observable();
    this.config = ko.observableArray();
    Client.MultiEntitySearch.ViewModels.MultiSearchViewModel.initializeBase(this);
    var dataConfig = Xrm.PageEx.getWebResourceData();
    var typeCodes = [];
    var typeNames = [];
    if (Object.keyExists(dataConfig, 'typeCodes')) {
        typeCodes = dataConfig['typeCodes'].split(',');
        typeNames = dataConfig['typeNames'].split(',');
    }
    else {
        typeCodes = ['1', '2', '4', '4200', '3'];
        typeNames = ['account', 'contact', 'lead', 'activitypointer', 'opportunity'];
    }
    var getviewfetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='savedquery'>\r\n                                <attribute name='name' />\r\n                                <attribute name='fetchxml' />\r\n                                <attribute name='layoutxml' />\r\n                                <attribute name='returnedtypecode' />\r\n                                <filter type='and'>\r\n                                <filter type='or'>";
    var $enum1 = ss.IEnumerator.getEnumerator(typeCodes);
    while ($enum1.moveNext()) {
        var view = $enum1.current;
        getviewfetchXml += "<condition attribute='returnedtypecode' operator='eq' value='" + view + "'/>";
    }
    getviewfetchXml += "\r\n                                    </filter>\r\n                                 <condition attribute='isquickfindquery' operator='eq' value='1'/>\r\n                                    <condition attribute='isdefault' operator='eq' value='1'/>\r\n                                </filter>\r\n                               \r\n                              </entity>\r\n                            </fetch>";
    var quickFindQuery = Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple(getviewfetchXml);
    this.parser = new Client.MultiEntitySearch.ViewModels.QueryParser();
    var entityLookup = {};
    var $enum2 = ss.IEnumerator.getEnumerator(quickFindQuery.get_entities());
    while ($enum2.moveNext()) {
        var view = $enum2.current;
        entityLookup[view.getAttributeValueString('returnedtypecode')] = view;
    }
    var $enum3 = ss.IEnumerator.getEnumerator(typeNames);
    while ($enum3.moveNext()) {
        var typeName = $enum3.current;
        var view = entityLookup[typeName];
        var fetchXml = view.getAttributeValueString('fetchxml');
        var layoutXml = view.getAttributeValueString('layoutxml');
        var config = this.parser.parse(fetchXml, layoutXml);
        config.dataView = new SparkleXrm.GridEditor.EntityDataViewModel(10, Xrm.Sdk.Entity, true);
        this.config.push(config);
    }
    this.parser.queryDisplayNames();
}
Client.MultiEntitySearch.ViewModels.MultiSearchViewModel.prototype = {
    parser: null,
    
    getEntityDisplayName: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel$getEntityDisplayName(index) {
        var settings = this.config();
        if (index >= settings.length) {
            return '';
        }
        else {
            return settings[index].rootEntity.displayCollectionName;
        }
    },
    
    searchCommand: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel$searchCommand() {
        var $enum1 = ss.IEnumerator.getEnumerator(this.config());
        while ($enum1.moveNext()) {
            var config = $enum1.current;
            config.dataView.set_fetchXml(this.parser.getFetchXmlForQuery(config, '%' + this.searchTerm() + '%'));
            config.dataView.reset();
            config.dataView.resetPaging();
            config.dataView.refresh();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013

Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013 = function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013() {
    this.searchTerm = ko.observable();
    this.config = ko.observableArray();
    Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013.initializeBase(this);
    var throttledSearchTermObservable = {};
    throttledSearchTermObservable.owner = this;
    throttledSearchTermObservable.read = ss.Delegate.create(this, function() {
        return this.searchTerm();
    });
    this.throttledSearchTerm = ko.dependentObservable(throttledSearchTermObservable).extend({ throttle: 400 });
    this.throttledSearchTerm.subscribe(ss.Delegate.create(this, function(search) {
        this.searchCommand();
    }));
    var dataConfig = Xrm.PageEx.getWebResourceData();
    this._queryQuickSearchEntities$1();
    var views = this._getViewQueries$1();
    this._parser$1 = new Client.MultiEntitySearch.ViewModels.QueryParser();
    var $enum1 = ss.IEnumerator.getEnumerator(this._entityTypeNames$1);
    while ($enum1.moveNext()) {
        var typeName = $enum1.current;
        var view = views[typeName];
        var fetchXml = view.getAttributeValueString('fetchxml');
        var layoutXml = view.getAttributeValueString('layoutxml');
        var config = this._parser$1.parse(fetchXml, layoutXml);
        config.recordCount = ko.observable();
        config.dataView = new SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel(25, Xrm.Sdk.Entity, true);
        config.recordCount(this.getResultLabel(config));
        this.config.push(config);
        config.dataView.onPagingInfoChanged.subscribe(this._onPagingInfoChanged$1(config));
    }
    this._parser$1.queryDisplayNames();
}
Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013.prototype = {
    throttledSearchTerm: null,
    _parser$1: null,
    _entityMetadata$1: null,
    _entityTypeNames$1: null,
    
    _onPagingInfoChanged$1: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013$_onPagingInfoChanged$1(config) {
        return ss.Delegate.create(this, function(e, p) {
            var paging = p;
            config.recordCount(this.getResultLabel(config));
        });
    },
    
    _getViewQueries$1: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013$_getViewQueries$1() {
        var getviewfetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='savedquery'>\r\n                                <attribute name='name' />\r\n                                <attribute name='fetchxml' />\r\n                                <attribute name='layoutxml' />\r\n                                <attribute name='returnedtypecode' />\r\n                                <filter type='and'>\r\n                                <filter type='or'>";
        var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(this._entityMetadata$1));
        while ($enum1.moveNext()) {
            var typeName = $enum1.current;
            getviewfetchXml += "<condition attribute='returnedtypecode' operator='eq' value='" + this._entityMetadata$1[typeName].objectTypeCode.toString() + "'/>";
        }
        getviewfetchXml += "\r\n                                    </filter>\r\n                                 <condition attribute='isquickfindquery' operator='eq' value='1'/>\r\n                                    <condition attribute='isdefault' operator='eq' value='1'/>\r\n                                </filter>\r\n                               \r\n                              </entity>\r\n                            </fetch>";
        var quickFindQuery = Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple(getviewfetchXml);
        var views = {};
        var $enum2 = ss.IEnumerator.getEnumerator(quickFindQuery.get_entities());
        while ($enum2.moveNext()) {
            var view = $enum2.current;
            views[view.getAttributeValueString('returnedtypecode')] = view;
        }
        return views;
    },
    
    _queryQuickSearchEntities$1: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013$_queryQuickSearchEntities$1() {
        Xrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('ExecuteFetch', Client.MultiEntitySearch.ViewModels.ExecuteFetchResponse);
        var fetchxml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                                  <entity name='multientitysearchentities'>\r\n                                    <attribute name='entityname' />\r\n                                    <order attribute='entityorder' descending='false' />\r\n                                  </entity>\r\n                                </fetch>";
        var request = new Client.MultiEntitySearch.ViewModels.ExecuteFetchRequest();
        request.fetchXml = fetchxml;
        var entityList = Xrm.Sdk.OrganizationServiceProxy.execute(request);
        var entityListDOM = $(entityList.fetchXmlResult);
        this._entityTypeNames$1 = [];
        var results = entityListDOM.first().find('result');
        results.each(ss.Delegate.create(this, function(index, element) {
            var entityName = Xrm.Sdk.XmlHelper.selectSingleNodeValue(element, 'entityname');
            this._entityTypeNames$1.add(entityName);
        }));
        var builder = new Xrm.Sdk.Metadata.Query.MetadataQueryBuilder();
        builder.addEntities(this._entityTypeNames$1, ['ObjectTypeCode', 'DisplayCollectionName']);
        builder.setLanguage(USER_LANGUAGE_CODE);
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        this._entityMetadata$1 = {};
        var $enum1 = ss.IEnumerator.getEnumerator(response.entityMetadata);
        while ($enum1.moveNext()) {
            var entity = $enum1.current;
            this._entityMetadata$1[entity.logicalName] = entity;
        }
    },
    
    getResultLabel: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013$getResultLabel(config) {
        var label = this._entityMetadata$1[config.rootEntity.logicalName].displayCollectionName.userLocalizedLabel.label;
        var totalRows = config.dataView.getLength();
        var totalRowLabel = (totalRows > 5000) ? '5000+' : totalRows.toString();
        return label + ' (' + totalRowLabel + ')';
    },
    
    getEntityDisplayName: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013$getEntityDisplayName(index) {
        var settings = this.config();
        if (index >= settings.length) {
            return '';
        }
        else {
            var config = settings[index];
            var totalRows = config.dataView.getLength();
            var label = this._entityMetadata$1[config.rootEntity.logicalName].displayCollectionName.userLocalizedLabel.label;
            return label + '(' + totalRows.toString() + ')';
        }
    },
    
    searchCommand: function Client_MultiEntitySearch_ViewModels_MultiSearchViewModel2013$searchCommand() {
        var searchTermText = this.searchTerm();
        if (String.isNullOrEmpty(searchTermText)) {
            return;
        }
        var $enum1 = ss.IEnumerator.getEnumerator(this.config());
        while ($enum1.moveNext()) {
            var config = $enum1.current;
            config.dataView.resetPaging();
            config.dataView.set_fetchXml(null);
            config.dataView.get_data().clear();
            config.dataView.refresh();
            config.dataView.set_fetchXml(this._parser$1.getFetchXmlForQuery(config, '%' + searchTermText + '%'));
            if (config.rootEntity.primaryImageAttribute != null) {
                var startofAttributes = config.dataView.get_fetchXml().indexOf('<attribute ');
                config.dataView.set_fetchXml(config.dataView.get_fetchXml().substr(0, startofAttributes) + '<attribute name="' + config.rootEntity.primaryImageAttribute + "_url\" alias='card_image_url'/>" + config.dataView.get_fetchXml().substr(startofAttributes));
            }
            config.dataView.reset();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.ViewModels.QueryParser

Client.MultiEntitySearch.ViewModels.QueryParser = function Client_MultiEntitySearch_ViewModels_QueryParser() {
    this._entityLookup = {};
    this._aliasEntityLookup = {};
    this._lookupAttributes = {};
}
Client.MultiEntitySearch.ViewModels.QueryParser.prototype = {
    
    parse: function Client_MultiEntitySearch_ViewModels_QueryParser$parse(fetchXml, layoutXml) {
        var querySettings = {};
        querySettings.columns = [];
        var fetchXmlDOM = $('<query>' + fetchXml.replaceAll('{0}', '#Query#') + '</query>');
        var fetchElement = fetchXmlDOM.find('fetch');
        querySettings.fetchXml = fetchXmlDOM;
        this._parseFetchXml(querySettings);
        querySettings.columns = this._parseLayoutXml(querySettings.rootEntity, layoutXml);
        return querySettings;
    },
    
    _parseLayoutXml: function Client_MultiEntitySearch_ViewModels_QueryParser$_parseLayoutXml(rootEntity, layoutXml) {
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
            var width = parseInt(element.getAttribute('width').toString());
            var disableSorting = element.getAttribute('disableSorting');
            var col = SparkleXrm.GridEditor.GridDataViewBinder.newColumn(attribute.logicalName, attribute.logicalName, width);
            col.sortable = !(disableSorting != null && disableSorting.toString() === '1');
            attribute.columns.add(col);
            columns.add(col);
        }));
        return columns;
    },
    
    queryDisplayNames: function Client_MultiEntitySearch_ViewModels_QueryParser$queryDisplayNames() {
        var builder = new Xrm.Sdk.Metadata.Query.MetadataQueryBuilder();
        var entities = [];
        var attributes = [];
        var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(this._entityLookup));
        while ($enum1.moveNext()) {
            var entityLogicalName = $enum1.current;
            entities.add(entityLogicalName);
            var entity = this._entityLookup[entityLogicalName];
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
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        var $enum3 = ss.IEnumerator.getEnumerator(response.entityMetadata);
        while ($enum3.moveNext()) {
            var entityMetadata = $enum3.current;
            var entityQuery = this._entityLookup[entityMetadata.logicalName];
            entityQuery.displayName = entityMetadata.displayName.userLocalizedLabel.label;
            entityQuery.displayCollectionName = entityMetadata.displayCollectionName.userLocalizedLabel.label;
            entityQuery.primaryImageAttribute = entityMetadata.primaryImageAttribute;
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
    
    _parseFetchXml: function Client_MultiEntitySearch_ViewModels_QueryParser$_parseFetchXml(querySettings) {
        var fetchElement = querySettings.fetchXml;
        var entityElement = fetchElement.find('entity');
        var logicalName = entityElement.attr('name');
        var rootEntity;
        if (!Object.keyExists(this._entityLookup, logicalName)) {
            rootEntity = {};
            rootEntity.logicalName = logicalName;
            rootEntity.attributes = {};
            this._entityLookup[rootEntity.logicalName] = rootEntity;
        }
        else {
            rootEntity = this._entityLookup[logicalName];
        }
        var linkEntities = entityElement.find('link-entity');
        linkEntities.each(ss.Delegate.create(this, function(index, element) {
            var link = {};
            link.attributes = {};
            link.aliasName = element.getAttribute('alias').toString();
            link.logicalName = element.getAttribute('name').toString();
            if (!Object.keyExists(this._entityLookup, link.logicalName)) {
                this._entityLookup[link.logicalName] = link;
            }
            else {
                var alias = link.aliasName;
                link = this._entityLookup[link.logicalName];
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
    
    getFetchXmlForQuery: function Client_MultiEntitySearch_ViewModels_QueryParser$getFetchXmlForQuery(config, searchTerm) {
        var fetchElement = config.fetchXml.find('fetch');
        fetchElement.attr('count', '{0}');
        fetchElement.attr('paging-cookie', '{1}');
        fetchElement.attr('page', '{2}');
        fetchElement.attr('returntotalrecordcount', 'true');
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
        var fetchXml = config.fetchXml.html().replaceAll('</entity>', '{3}</entity>');
        fetchXml = fetchXml.replaceAll('#Query#', Xrm.Sdk.XmlHelper.encode(searchTerm));
        return fetchXml;
    }
}


Type.registerNamespace('Client.MultiEntitySearch.Views');

////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.Views.MultiSearchView

Client.MultiEntitySearch.Views.MultiSearchView = function Client_MultiEntitySearch_Views_MultiSearchView() {
}
Client.MultiEntitySearch.Views.MultiSearchView.init = function Client_MultiEntitySearch_Views_MultiSearchView$init() {
    var vm = new Client.MultiEntitySearch.ViewModels.MultiSearchViewModel();
    Xrm.PageEx.majorVersion = 2013;
    var searches = vm.config();
    var i = 0;
    var $enum1 = ss.IEnumerator.getEnumerator(searches);
    while ($enum1.moveNext()) {
        var config = $enum1.current;
        var dataViewBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
        var grid = dataViewBinder.dataBindXrmGrid(config.dataView, config.columns, 'grid' + i.toString() + 'container', 'grid' + i.toString() + 'pager', true, false);
        dataViewBinder.bindClickHandler(grid);
        i++;
    }
    SparkleXrm.ViewBase.registerViewModel(vm);
}


////////////////////////////////////////////////////////////////////////////////
// Client.MultiEntitySearch.Views.MultiSearchView2013

Client.MultiEntitySearch.Views.MultiSearchView2013 = function Client_MultiEntitySearch_Views_MultiSearchView2013() {
}
Client.MultiEntitySearch.Views.MultiSearchView2013.init = function Client_MultiEntitySearch_Views_MultiSearchView2013$init() {
    var vm = new Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013();
    var searches = vm.config();
    var searchResultsDiv = $('#searchResults');
    $(window).resize(function(e) {
        Client.MultiEntitySearch.Views.MultiSearchView2013._onResizeSearchResults(searchResultsDiv);
    });
    Client.MultiEntitySearch.Views.MultiSearchView2013._onResizeSearchResults(searchResultsDiv);
    $('.sparkle-xrm').bind('onmousewheel mousewheel DOMMouseScroll', Client.MultiEntitySearch.Views.MultiSearchView2013._onSearchResultsMouseScroll);
    var i = 0;
    var $enum1 = ss.IEnumerator.getEnumerator(searches);
    while ($enum1.moveNext()) {
        var config = $enum1.current;
        var cardColumn = [{ id: 'card-column', options: config.columns, name: 'Name', width: 290, cssClass: 'card-column-cell' }];
        cardColumn[0].formatter = Client.MultiEntitySearch.Views.MultiSearchView2013.renderCardColumnCell;
        cardColumn[0].dataType = 'PrimaryNameLookup';
        config.columns[0].dataType = 'PrimaryNameLookup';
        var dataViewBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
        var gridOptions = {};
        gridOptions.enableCellNavigation = true;
        gridOptions.autoEdit = false;
        gridOptions.editable = false;
        gridOptions.enableAddRow = false;
        var columns = config.columns.length;
        gridOptions.rowHeight = ((columns > 3) ? 3 : columns) * 16;
        if (gridOptions.rowHeight < 70) {
            gridOptions.rowHeight = 70;
        }
        gridOptions.headerRowHeight = 0;
        var gridId = 'grid' + i.toString() + 'container';
        var dataView = config.dataView;
        var grid = new Slick.Grid('#' + gridId, dataView, cardColumn, gridOptions);
        Client.MultiEntitySearch.Views.MultiSearchView2013.grids[i] = grid;
        Client.MultiEntitySearch.Views.MultiSearchView2013._addResizeEventHandlers(grid, gridId);
        dataViewBinder.dataBindEvents(grid, dataView, gridId);
        dataViewBinder.bindClickHandler(grid);
        i++;
    }
    SparkleXrm.ViewBase.registerViewModel(vm);
}
Client.MultiEntitySearch.Views.MultiSearchView2013._onResizeSearchResults = function Client_MultiEntitySearch_Views_MultiSearchView2013$_onResizeSearchResults(searchResultsDiv) {
    var height = $(window).height();
    searchResultsDiv.height(height - 30);
}
Client.MultiEntitySearch.Views.MultiSearchView2013._onSearchResultsMouseScroll = function Client_MultiEntitySearch_Views_MultiSearchView2013$_onSearchResultsMouseScroll(e) {
    var wheelDelta = e.originalEvent.wheelDelta;
    if (wheelDelta == null) {
        wheelDelta = e.originalEvent.wheelDeltaY;
    }
    if (wheelDelta == null) {
        wheelDelta = e.originalEvent.detail * -30;
    }
    if (wheelDelta == null) {
        wheelDelta = e.originalEvent.delta * -30;
    }
    var target = $(e.target);
    var gridContainer = target.closest('.slick-cell');
    if (gridContainer.length > 0) {
        return;
    }
    var searchResultsDiv = $('#searchResults');
    var scrollLeft = searchResultsDiv.scrollLeft();
    searchResultsDiv.scrollLeft(scrollLeft -= wheelDelta);
    e.preventDefault();
}
Client.MultiEntitySearch.Views.MultiSearchView2013._addResizeEventHandlers = function Client_MultiEntitySearch_Views_MultiSearchView2013$_addResizeEventHandlers(grid, containerName) {
    $(window).resize(function(e) {
        Client.MultiEntitySearch.Views.MultiSearchView2013._resizeGrid(grid, containerName);
    });
    $(function() {
        Client.MultiEntitySearch.Views.MultiSearchView2013._resizeGrid(grid, containerName);
    });
}
Client.MultiEntitySearch.Views.MultiSearchView2013._resizeGrid = function Client_MultiEntitySearch_Views_MultiSearchView2013$_resizeGrid(grid, containerName) {
    var height = $(window).height();
    $('#' + containerName).height(height - 85);
    grid.resizeCanvas();
}
Client.MultiEntitySearch.Views.MultiSearchView2013.renderCardColumnCell = function Client_MultiEntitySearch_Views_MultiSearchView2013$renderCardColumnCell(row, cell, value, columnDef, dataContext) {
    var columns = columnDef.options;
    var record = dataContext;
    var cardHtml = '';
    var firstRow = true;
    var imageUrl = record.getAttributeValueString('card_image_url');
    var imageHtml;
    if (imageUrl != null) {
        imageHtml = "<img class='entity-image' src='" + Xrm.Page.context.getClientUrl() + imageUrl + "'/>";
    }
    else {
        var typeName = record.logicalName;
        if (typeName === 'activitypointer') {
            var activitytypecode = record.getAttributeValueString('activitytypecode');
            typeName = activitytypecode;
        }
        imageHtml = "<div class='record_card " + typeName + "_card'><img src='..\\..\\sparkle_\\css\\images\\transparent_spacer.gif'\\></dv>";
    }
    var rowIndex = 0;
    cardHtml = "<table class='contact-card-layout'><tr><td>" + imageHtml + '</td><td>';
    var $enum1 = ss.IEnumerator.getEnumerator(columns);
    while ($enum1.moveNext()) {
        var col = $enum1.current;
        if (col.field !== 'activitytypecode') {
            var fieldValue = record.getAttributeValue(col.field);
            var dataFormatted = col.formatter(row, cell, fieldValue, col, dataContext);
            cardHtml += '<div ' + ((firstRow) ? "class='first-row'" : '') + " tooltip='" + fieldValue + "'>" + dataFormatted + '</div>';
            firstRow = false;
            rowIndex++;
        }
        if (rowIndex > 3) {
            break;
        }
    }
    cardHtml += '</tr></table>';
    return cardHtml;
}


SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel.registerClass('SparkleXrm.GridEditor.VirtualPagedEntityDataViewModel', SparkleXrm.GridEditor.EntityDataViewModel);
Client.MultiEntitySearch.ViewModels.ExecuteFetchRequest.registerClass('Client.MultiEntitySearch.ViewModels.ExecuteFetchRequest', null, Object);
Client.MultiEntitySearch.ViewModels.ExecuteFetchResponse.registerClass('Client.MultiEntitySearch.ViewModels.ExecuteFetchResponse', null, Object);
Client.MultiEntitySearch.ViewModels.MultiSearchViewModel.registerClass('Client.MultiEntitySearch.ViewModels.MultiSearchViewModel', SparkleXrm.ViewModelBase);
Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013.registerClass('Client.MultiEntitySearch.ViewModels.MultiSearchViewModel2013', SparkleXrm.ViewModelBase);
Client.MultiEntitySearch.ViewModels.QueryParser.registerClass('Client.MultiEntitySearch.ViewModels.QueryParser');
Client.MultiEntitySearch.Views.MultiSearchView.registerClass('Client.MultiEntitySearch.Views.MultiSearchView');
Client.MultiEntitySearch.Views.MultiSearchView2013.registerClass('Client.MultiEntitySearch.Views.MultiSearchView2013');
Client.MultiEntitySearch.Views.MultiSearchView2013.grids = [];
})(window.xrmjQuery);

});


function waitForScripts(name, scriptNames, callback) {
    var hasLoaded = false;
    window._loadedScripts = window._loadedScripts || [];
    function checkScripts() {
        var allLoaded = true;
        for (var i = 0; i < scriptNames.length; i++) {
            var hasLoaded = true;
            var script = scriptNames[i];
            switch (script) {
                case "mscorlib":
                    hasLoaded = typeof (window.ss) != "undefined";
                    break;
                case "jquery":
                    hasLoaded = typeof (window.xrmjQuery) != "undefined";
                    break;
				 case "jquery-ui":
                    hasLoaded = typeof (window.xrmjQuery.ui) != "undefined";
                    break;
                default:
                    hasLoaded = window._loadedScripts[script];
                    break;
            }

            allLoaded = allLoaded && hasLoaded;
            if (!allLoaded) {
                setTimeout(checkScripts, 10);
                break;
            }
        }

        if (allLoaded) {
            callback();
            window._loadedScripts[name] = true;
        }
    }
	
	checkScripts();
	
}
