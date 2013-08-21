//! ClientUI.debug.js
//
waitForScripts("client",["mscorlib","xrm","xrmui", "jquery", "jquery-ui"],
function () {

(function($){

Type.registerNamespace('Client.MultiEntitySearch.ViewModels');

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
        builder.addEntities(entities, ['Attributes', 'DisplayName', 'DisplayCollectionName']);
        builder.addAttributes(attributes, ['DisplayName', 'AttributeType', 'IsPrimaryName']);
        builder.setLanguage(USER_LANGUAGE_CODE);
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        var $enum3 = ss.IEnumerator.getEnumerator(response.entityMetadata);
        while ($enum3.moveNext()) {
            var entityMetadata = $enum3.current;
            var entityQuery = this._entityLookup[entityMetadata.logicalName];
            entityQuery.displayName = entityMetadata.displayName.userLocalizedLabel.label;
            entityQuery.displayCollectionName = entityMetadata.displayCollectionName.userLocalizedLabel.label;
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


Client.MultiEntitySearch.ViewModels.MultiSearchViewModel.registerClass('Client.MultiEntitySearch.ViewModels.MultiSearchViewModel', SparkleXrm.ViewModelBase);
Client.MultiEntitySearch.ViewModels.QueryParser.registerClass('Client.MultiEntitySearch.ViewModels.QueryParser');
Client.MultiEntitySearch.Views.MultiSearchView.registerClass('Client.MultiEntitySearch.Views.MultiSearchView');
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
