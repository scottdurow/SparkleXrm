//! SparkleXrm.debug.js
//
var scriptLoader = scriptLoader || {
    delayedLoads: [],
    load: function (name, requires, script) {
        window._loadedScripts = window._loadedScripts || {};
        // Check for loaded scripts, if not all loaded then register delayed Load
        if (requires == null || requires.length == 0 || scriptLoader.areLoaded(requires)) {
            scriptLoader.runScript(name, script);
        }
        else {
            // Register an onload check
            scriptLoader.delayedLoads.push({ name: name, requires: requires, script: script });
        }
    },
    runScript: function (name, script) {      
        script.call(window);
        window._loadedScripts[name] = true;
        scriptLoader.onScriptLoaded(name);
    },
    onScriptLoaded: function (name) {
        // Check for any registered delayed Loads
        scriptLoader.delayedLoads.forEach(function (script) {
            if (script.loaded == null && scriptLoader.areLoaded(script.requires)) {
                script.loaded = true;
                scriptLoader.runScript(script.name, script.script);
            }
        });
    },
    areLoaded: function (requires) {
        var allLoaded = true;
        for (var i = 0; i < requires.length; i++) {
			var isLoaded = (window._loadedScripts[requires[i]] != null);
            allLoaded = allLoaded && isLoaded;
            if (!allLoaded)
                break;
        }
        return allLoaded;
    }
};
 
scriptLoader.load("xrm", ["mscorlib"], function () {


Type.registerNamespace('SparkleXrm');

Type.registerNamespace('SparkleXrm.Xrm');

Type.registerNamespace('Xrm');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ArrayEx

SparkleXrm.ArrayEx = function SparkleXrm_ArrayEx() {
}
SparkleXrm.ArrayEx.add = function SparkleXrm_ArrayEx$add(list, item) {
    list[list.length]=item;
}
SparkleXrm.ArrayEx.getEnumerator = function SparkleXrm_ArrayEx$getEnumerator(list) {
    return new ss.ArrayEnumerator(list);
}
SparkleXrm.ArrayEx.join = function SparkleXrm_ArrayEx$join(list, delimeter) {
    var result = '';
    for (var i = 0; i < list.length; i++) {
        if (i > 0) {
            result += delimeter;
        }
        result += list[i];
    }
    return result;
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.DelegateItterator

SparkleXrm.DelegateItterator = function SparkleXrm_DelegateItterator() {
}
SparkleXrm.DelegateItterator.callbackItterate = function SparkleXrm_DelegateItterator$callbackItterate(action, numberOfTimes, completeCallBack, errorCallBack) {
    SparkleXrm.DelegateItterator._callbackItterateAction(action, 0, numberOfTimes, completeCallBack, errorCallBack);
}
SparkleXrm.DelegateItterator._callbackItterateAction = function SparkleXrm_DelegateItterator$_callbackItterateAction(action, index, numberOfTimes, completeCallBack, errorCallBack) {
    if (index < numberOfTimes) {
        try {
            action(index, function() {
                index++;
                SparkleXrm.DelegateItterator._callbackItterateAction(action, index, numberOfTimes, completeCallBack, errorCallBack);
            }, function(ex) {
                errorCallBack(ex);
            });
        }
        catch (ex) {
            errorCallBack(ex);
        }
    }
    else {
        completeCallBack();
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.NumberEx

SparkleXrm.NumberEx = function SparkleXrm_NumberEx() {
}
SparkleXrm.NumberEx.parse = function SparkleXrm_NumberEx$parse(value, format) {
    if (String.isNullOrEmpty(value)) {
        return null;
    }
    value = value.replaceAll(' ', '');
    value = value.replaceAll(format.numberSepartor, '');
    if (format.decimalSymbol !== '.') {
        value = value.replaceAll(format.decimalSymbol, '.');
    }
    if (value.startsWith('(')) {
        value = '-' + value.replaceAll('(', '').replaceAll(')', '');
    }
    else if (value.endsWith('-')) {
        value = '-' + value.substring(0, value.length - 1);
    }
    var numericValue = Number.parse(value);
    return numericValue;
}
SparkleXrm.NumberEx.getNumberFormatInfo = function SparkleXrm_NumberEx$getNumberFormatInfo() {
    var format = {};
    if (SparkleXrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        format.decimalSymbol = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.decimalsymbol;
        format.numberGroupFormat = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.numbergroupformat;
        format.numberSepartor = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.numberseparator;
        format.negativeFormatCode = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.negativeformatcode;
    }
    else {
        format.decimalSymbol = '.';
        format.numberGroupFormat = '3';
        format.numberSepartor = ',';
        format.negativeFormatCode = 0;
    }
    format.precision = 2;
    format.minValue = -2147483648;
    format.maxValue = 2147483648;
    return format;
}
SparkleXrm.NumberEx.getCurrencyEditFormatInfo = function SparkleXrm_NumberEx$getCurrencyEditFormatInfo() {
    var format = {};
    if (SparkleXrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        format.decimalSymbol = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.decimalsymbol;
        format.numberGroupFormat = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.numbergroupformat;
        format.numberSepartor = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.numberseparator;
        format.negativeFormatCode = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.negativecurrencyformatcode;
        format.precision = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.currencydecimalprecision;
        format.currencySymbol = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.currencysymbol;
    }
    else {
        format.decimalSymbol = '.';
        format.numberGroupFormat = '3';
        format.numberSepartor = ',';
        format.negativeFormatCode = 0;
        format.precision = 2;
        format.currencySymbol = '$';
    }
    return format;
}
SparkleXrm.NumberEx.getCurrencyFormatInfo = function SparkleXrm_NumberEx$getCurrencyFormatInfo() {
    var format = {};
    if (SparkleXrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        format.decimalSymbol = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.decimalsymbol;
        format.numberGroupFormat = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.numbergroupformat;
        format.numberSepartor = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.numberseparator;
        format.negativeFormatCode = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.negativecurrencyformatcode;
        format.precision = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.currencydecimalprecision;
        format.currencySymbol = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.currencysymbol;
    }
    else {
        format.decimalSymbol = '.';
        format.numberGroupFormat = '3';
        format.numberSepartor = ',';
        format.negativeFormatCode = 0;
        format.precision = 2;
        format.currencySymbol = '$';
    }
    return format;
}
SparkleXrm.NumberEx.format = function SparkleXrm_NumberEx$format(actualValue, format) {
    if (actualValue == null) {
        return '';
    }
    var value = actualValue;
    var numberGroupFormats = format.numberGroupFormat.split(',');
    var formattedNumber = '';
    if (format.precision != null) {
        value.toFixed(format.precision);
    }
    var wholeNumber = Math.floor(Math.abs(value));
    var wholeNumberString = wholeNumber.toString();
    var decimalPartString = value.toString().substr(wholeNumberString.length + 1 + ((value < 0) ? 1 : 0));
    var i = wholeNumberString.length;
    var j = 0;
    while (i > 0) {
        var groupSize = parseInt(numberGroupFormats[j]);
        if (j < (numberGroupFormats.length - 1)) {
            j++;
        }
        if (!groupSize) {
            groupSize = i + 1;
        }
        formattedNumber = wholeNumberString.substring(i, i - groupSize) + formattedNumber;
        if (i > groupSize) {
            formattedNumber = format.numberSepartor + formattedNumber;
        }
        i = i - groupSize;
    }
    var neg = (value < 0);
    if (format.precision > 0) {
        var paddingRequired = format.precision - decimalPartString.length;
        for (var d = 0; d < paddingRequired; d++) {
            decimalPartString = decimalPartString + '0';
        }
        formattedNumber = formattedNumber + format.decimalSymbol + decimalPartString;
    }
    if (neg) {
        switch (format.negativeFormatCode) {
            case 0:
                formattedNumber = '(' + formattedNumber + ')';
                break;
            case 2:
                formattedNumber = '- ' + formattedNumber;
                break;
            case 3:
                formattedNumber = formattedNumber + '-';
                break;
            case 4:
                formattedNumber = formattedNumber + ' -';
                break;
            case 1:
            default:
                formattedNumber = '-' + formattedNumber;
                break;
        }
    }
    return formattedNumber;
}
SparkleXrm.NumberEx.round = function SparkleXrm_NumberEx$round(numericValue, precision) {
    var precisionMultiplier = 1;
    if (precision > 0) {
        precisionMultiplier = Math.pow(10, precision);
    }
    return Math.round(numericValue * precisionMultiplier) / precisionMultiplier;
}
SparkleXrm.NumberEx.getCurrencySymbol = function SparkleXrm_NumberEx$getCurrencySymbol(currencyId) {
    var orgSettings = SparkleXrm.Services.CachedOrganizationService.retrieveMultiple("<fetch distinct='false' no-lock='false' mapping='logical'><entity name='organization'><attribute name='currencydisplayoption' /><attribute name='currencysymbol' /></entity></fetch>");
    var orgSetting = orgSettings.get_entities().get_item(0);
    var currency = SparkleXrm.Services.CachedOrganizationService.retrieve('transactioncurrency', currencyId.toString(), [ 'currencysymbol', 'isocurrencycode' ]);
    if (!orgSetting.getAttributeValueOptionSet('currencydisplayoption').value) {
        return currency.getAttributeValueString('currencysymbol') + ' ';
    }
    else {
        return currency.getAttributeValueString('isocurrencycode') + ' ';
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Xrm.PageEx

SparkleXrm.Xrm.PageEx = function SparkleXrm_Xrm_PageEx() {
}
SparkleXrm.Xrm.PageEx.getCacheKey = function SparkleXrm_Xrm_PageEx$getCacheKey() {
    var cacheKey = WEB_RESOURCE_ORG_VERSION_NUMBER;
    if (typeof(cacheKey) !== 'undefined') {
        return cacheKey + '/';
    }
    else {
        return '';
    }
}
SparkleXrm.Xrm.PageEx.getWebResourceData = function SparkleXrm_Xrm_PageEx$getWebResourceData() {
    var queryString = window.location.search;
    if (queryString != null && !!queryString) {
        var parameters = queryString.substr(1).split('&');
        var $enum1 = ss.IEnumerator.getEnumerator(parameters);
        while ($enum1.moveNext()) {
            var param = $enum1.current;
            if (param.toLowerCase().startsWith('data=')) {
                var dataParam = param.replaceAll('+', ' ').split('=');
                return SparkleXrm.Xrm.PageEx._parseDataParameter(dataParam[1]);
            }
        }
    }
    return {};
}
SparkleXrm.Xrm.PageEx._parseDataParameter = function SparkleXrm_Xrm_PageEx$_parseDataParameter(data) {
    var nameValuePairs = {};
    var values = (decodeURIComponent(data)).split('&');
    var $enum1 = ss.IEnumerator.getEnumerator(values);
    while ($enum1.moveNext()) {
        var value = $enum1.current;
        var nameValuePair = value.split('=');
        nameValuePairs[nameValuePair[0]] = nameValuePair[1];
    }
    return nameValuePairs;
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.StringEx

SparkleXrm.StringEx = function SparkleXrm_StringEx() {
}
SparkleXrm.StringEx.IN = function SparkleXrm_StringEx$IN(value, values) {
    if (value != null) {
        var $enum1 = ss.IEnumerator.getEnumerator(values);
        while ($enum1.moveNext()) {
            var val = $enum1.current;
            if (value === val) {
                return true;
            }
        }
    }
    return false;
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.TaskIterrator

SparkleXrm.TaskIterrator = function SparkleXrm_TaskIterrator() {
    this._tasks = [];
}
SparkleXrm.TaskIterrator.prototype = {
    _errorCallBack: null,
    _successCallBack: null,
    
    addTask: function SparkleXrm_TaskIterrator$addTask(task) {
        this._tasks.add(task);
    },
    
    start: function SparkleXrm_TaskIterrator$start(successCallBack, errorCallBack) {
        this._errorCallBack = errorCallBack;
        this._successCallBack = successCallBack;
        this._completeCallBack();
    },
    
    _completeCallBack: function SparkleXrm_TaskIterrator$_completeCallBack() {
        var nextAction = this._tasks[0];
        if (nextAction != null) {
            this._tasks.remove(nextAction);
            nextAction(ss.Delegate.create(this, this._completeCallBack), this._errorCallBack);
        }
        else {
            if (this._successCallBack != null) {
                this._successCallBack();
            }
        }
    }
}


Type.registerNamespace('SparkleXrm.ComponentModel');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ComponentModel.INotifyPropertyChanged

SparkleXrm.ComponentModel.INotifyPropertyChanged = function() { };
SparkleXrm.ComponentModel.INotifyPropertyChanged.prototype = {
    add_propertyChanged : null,
    remove_propertyChanged : null,
    raisePropertyChanged : null
}
SparkleXrm.ComponentModel.INotifyPropertyChanged.registerInterface('SparkleXrm.ComponentModel.INotifyPropertyChanged');


Type.registerNamespace('SparkleXrm.Sdk');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.EntityStates

SparkleXrm.Sdk.EntityStates = function() { };
SparkleXrm.Sdk.EntityStates.prototype = {
    unchanged: 0, 
    created: 1, 
    changed: 2, 
    deleted: 3, 
    readOnly: 4
}
SparkleXrm.Sdk.EntityStates.registerEnum('SparkleXrm.Sdk.EntityStates', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.EntityRole

SparkleXrm.Sdk.EntityRole = function() { };
SparkleXrm.Sdk.EntityRole.prototype = {
    referencing: 0, 
    referenced: 1
}
SparkleXrm.Sdk.EntityRole.registerEnum('SparkleXrm.Sdk.EntityRole', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Attribute

SparkleXrm.Sdk.Attribute = function SparkleXrm_Sdk_Attribute(attributeName, typeName) {
    this.attributeName = attributeName;
    this.typeName = typeName;
    this.formattedValue = null;
    this.value = null;
    this.id = null;
    this.logicalName = null;
    this.name = null;
}
SparkleXrm.Sdk.Attribute.deSerialise = function SparkleXrm_Sdk_Attribute$deSerialise(node, overrideType) {
    var isNil = (SparkleXrm.Sdk.XmlHelper.getAttributeValue(node, 'i:nil') === 'true');
    var value = null;
    if (!isNil) {
        var typeName = overrideType;
        if (typeName == null) {
            typeName = SparkleXrm.Sdk.Attribute._removeNsPrefix(SparkleXrm.Sdk.XmlHelper.getAttributeValue(node, 'i:type'));
        }
        var stringValue = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
        switch (typeName) {
            case 'EntityReference':
                var entityReferenceValue = new SparkleXrm.Sdk.EntityReference(new SparkleXrm.Sdk.Guid(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Id')), SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'LogicalName'), SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Name'));
                value = entityReferenceValue;
                break;
            case 'AliasedValue':
                value = SparkleXrm.Sdk.Attribute.deSerialise(SparkleXrm.Sdk.XmlHelper.selectSingleNode(node, 'Value'), null);
                break;
            case 'boolean':
                value = (stringValue === 'true');
                break;
            case 'double':
                value = parseFloat(stringValue);
                break;
            case 'decimal':
                value = parseFloat(stringValue);
                break;
            case 'dateTime':
                var dateValue = SparkleXrm.Sdk.DateTimeEx.parse(stringValue);
                var settings = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings;
                if (settings != null) {
                    dateValue.setTime(dateValue.getTime() + (dateValue.getTimezoneOffset() * 60 * 1000));
                    var localDateValue = SparkleXrm.Sdk.DateTimeEx.utcToLocalTimeFromSettings(dateValue, settings);
                    value = localDateValue;
                }
                else {
                    value = dateValue;
                }
                break;
            case 'guid':
                value = new SparkleXrm.Sdk.Guid(stringValue);
                break;
            case 'int':
                value = parseInt(stringValue);
                break;
            case 'OptionSetValue':
                value = SparkleXrm.Sdk.OptionSetValue.parse(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Value'));
                break;
            case 'Money':
                value = new SparkleXrm.Sdk.Money(parseFloat(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Value')));
                break;
            case 'EntityCollection':
                value = SparkleXrm.Sdk.EntityCollection.deSerialise(node);
                break;
            default:
                value = stringValue;
                break;
        }
    }
    return value;
}
SparkleXrm.Sdk.Attribute.serialise = function SparkleXrm_Sdk_Attribute$serialise(attributeName, value, metaData) {
    var xml = '<a:KeyValuePairOfstringanyType><b:key>' + attributeName + '</b:key>';
    var typeName = Type.getInstanceType(value).get_name();
    if (value != null && metaData != null && Object.keyExists(metaData, attributeName)) {
        typeName = metaData[attributeName];
    }
    xml += SparkleXrm.Sdk.Attribute.serialiseValue(value, typeName);
    xml += '</a:KeyValuePairOfstringanyType>';
    return xml;
}
SparkleXrm.Sdk.Attribute.serialiseValue = function SparkleXrm_Sdk_Attribute$serialiseValue(value, overrideTypeName) {
    var valueXml = '';
    var typeName = overrideTypeName;
    if (typeName == null) {
        typeName = Type.getInstanceType(value).get_name();
    }
    switch (typeName) {
        case 'String':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('string') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            valueXml += SparkleXrm.Sdk.XmlHelper.encode(value);
            valueXml += '</b:value>';
            break;
        case 'Boolean':
        case 'bool':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('boolean') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            valueXml += SparkleXrm.Sdk.XmlHelper.encode(value.toString());
            valueXml += '</b:value>';
            break;
        case 'Date':
            var dateValue = value;
            var dateString = null;
            var settings = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings;
            if (settings != null) {
                var utcDateValue = SparkleXrm.Sdk.DateTimeEx.localTimeToUTCFromSettings(dateValue, settings);
                dateString = SparkleXrm.Sdk.DateTimeEx.toXrmString(utcDateValue);
            }
            else {
                dateString = SparkleXrm.Sdk.DateTimeEx.toXrmStringUTC(dateValue);
            }
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('dateTime') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            valueXml += SparkleXrm.Sdk.XmlHelper.encode(dateString);
            valueXml += '</b:value>';
            break;
        case 'decimal':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('decimal') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            var decStringValue = null;
            if (value != null) {
                decStringValue = value.toString();
            }
            valueXml += SparkleXrm.Sdk.XmlHelper.encode(decStringValue);
            valueXml += '</b:value>';
            break;
        case 'double':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('double') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            var doubleStringValue = null;
            if (value != null) {
                doubleStringValue = value.toString();
            }
            valueXml += SparkleXrm.Sdk.XmlHelper.encode(doubleStringValue);
            valueXml += '</b:value>';
            break;
        case 'int':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('int') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            var intStringValue = null;
            if (value != null) {
                intStringValue = value.toString();
            }
            valueXml += SparkleXrm.Sdk.XmlHelper.encode(intStringValue);
            valueXml += '</b:value>';
            break;
        case 'Guid':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix('guid') + '" xmlns:c="http://schemas.microsoft.com/2003/10/Serialization/">';
            valueXml += (value).value;
            valueXml += '</b:value>';
            break;
        case 'EntityReference':
            var entityReferenceValue = value;
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
            valueXml += '<a:Id>' + entityReferenceValue.id + '</a:Id><a:LogicalName>' + entityReferenceValue.logicalName + '</a:LogicalName>';
            valueXml += '</b:value>';
            break;
        case 'OptionSetValue':
            var opt = value;
            if (opt.value != null) {
                valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
                valueXml += '<a:Value>' + opt.value + '</a:Value>';
                valueXml += '</b:value>';
            }
            else {
                valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix(typeName) + '" i:nil="true"/>';
            }
            break;
        case 'EntityCollection':
            valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
            valueXml += SparkleXrm.Sdk.EntityCollection.serialise(value);
            valueXml += '</b:value>';
            break;
        case 'Money':
            var money = value;
            if (money != null && money.value != null) {
                valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
                valueXml += '<a:Value>' + money.value.toString() + '</a:Value>';
                valueXml += '</b:value>';
            }
            else {
                valueXml += '<b:value i:type="' + SparkleXrm.Sdk.Attribute._addNsPrefix(typeName) + '" i:nil="true"/>';
            }
            break;
        case 'EntityFilters':
            var entityFilterValue = value;
            var entityFilterValues = [];
            if ((1 & entityFilterValue) === 1) {
                entityFilterValues.add('Entity');
            }
            if ((2 & entityFilterValue) === 2) {
                entityFilterValues.add('Attributes');
            }
            if ((4 & entityFilterValue) === 4) {
                entityFilterValues.add('Privileges');
            }
            if ((8 & entityFilterValue) === 8) {
                entityFilterValues.add('Relationships');
            }
            valueXml += '<b:value i:type="c:EntityFilters" xmlns:c="http://schemas.microsoft.com/xrm/2011/Metadata">' + SparkleXrm.Sdk.XmlHelper.encode(entityFilterValues.join(' ')) + '</b:value>';
            break;
        default:
            valueXml += '<b:value i:nil="true"/>';
            break;
    }
    return valueXml;
}
SparkleXrm.Sdk.Attribute._removeNsPrefix = function SparkleXrm_Sdk_Attribute$_removeNsPrefix(node) {
    var i = node.indexOf(':');
    return node.substring(i + 1, node.length - i + 1);
}
SparkleXrm.Sdk.Attribute._addNsPrefix = function SparkleXrm_Sdk_Attribute$_addNsPrefix(type) {
    switch (type) {
        case 'String':
        case 'Guid':
        case 'DateTime':
        case 'string':
        case 'decimal':
        case 'double':
        case 'boolean':
        case 'dateTime':
        case 'guid':
        case 'int':
            return 'c:' + type;
        case 'EntityReference':
        case 'OptionSetValue':
        case 'AliasedValue':
        case 'EntityCollection':
        case 'Money':
            return 'a:' + type;
    }
    throw new Error('Could not add node prefix for type ' + type);
}
SparkleXrm.Sdk.Attribute.prototype = {
    attributeName: null,
    typeName: null,
    formattedValue: null,
    value: null,
    id: null,
    logicalName: null,
    name: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.AttributeTypes

SparkleXrm.Sdk.AttributeTypes = function SparkleXrm_Sdk_AttributeTypes() {
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.OrganizationSettings

SparkleXrm.Sdk.OrganizationSettings = function SparkleXrm_Sdk_OrganizationSettings() {
    SparkleXrm.Sdk.OrganizationSettings.initializeBase(this, [ SparkleXrm.Sdk.OrganizationSettings.entityLogicalName ]);
}
SparkleXrm.Sdk.OrganizationSettings.prototype = {
    weekstartdaycode: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.UserSettingsAttributes

SparkleXrm.Sdk.UserSettingsAttributes = function SparkleXrm_Sdk_UserSettingsAttributes() {
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.UserSettings

SparkleXrm.Sdk.UserSettings = function SparkleXrm_Sdk_UserSettings() {
    SparkleXrm.Sdk.UserSettings.initializeBase(this, [ SparkleXrm.Sdk.UserSettings.entityLogicalName ]);
}
SparkleXrm.Sdk.UserSettings.prototype = {
    usersettingsid: null,
    businessunitid: null,
    calendartype: null,
    currencydecimalprecision: null,
    currencyformatcode: null,
    currencysymbol: null,
    dateformatcode: null,
    dateformatstring: null,
    dateseparator: null,
    decimalsymbol: null,
    defaultcalendarview: null,
    defaultdashboardid: null,
    localeid: null,
    longdateformatcode: null,
    negativecurrencyformatcode: null,
    negativeformatcode: null,
    numbergroupformat: null,
    numberseparator: null,
    offlinesyncinterval: null,
    pricingdecimalprecision: null,
    showweeknumber: null,
    systemuserid: null,
    timeformatcodestring: null,
    timeformatstring: null,
    timeseparator: null,
    timezonebias: null,
    timezonecode: null,
    timezonedaylightbias: null,
    timezonedaylightday: null,
    timezonedaylightdayofweek: null,
    timezonedaylighthour: null,
    timezonedaylightminute: null,
    timezonedaylightmonth: null,
    timezonedaylightsecond: null,
    timezonedaylightyear: null,
    timezonestandardbias: null,
    timezonestandardday: null,
    timezonestandarddayofweek: null,
    timezonestandardhour: null,
    timezonestandardminute: null,
    timezonestandardmonth: null,
    timezonestandardsecond: null,
    timezonestandardyear: null,
    transactioncurrencyid: null,
    uilanguageid: null,
    workdaystarttime: null,
    workdaystoptime: null,
    
    getNumberFormatString: function SparkleXrm_Sdk_UserSettings$getNumberFormatString(decimalPlaces) {
        return '###,###,###.000';
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.DataCollectionOfEntity

SparkleXrm.Sdk.DataCollectionOfEntity = function SparkleXrm_Sdk_DataCollectionOfEntity(entities) {
    this._internalArray = entities;
}
SparkleXrm.Sdk.DataCollectionOfEntity.prototype = {
    _internalArray: null,
    
    items: function SparkleXrm_Sdk_DataCollectionOfEntity$items() {
        return this._internalArray;
    },
    
    getEnumerator: function SparkleXrm_Sdk_DataCollectionOfEntity$getEnumerator() {
        return SparkleXrm.ArrayEx.getEnumerator(this._internalArray);
    },
    
    get_count: function SparkleXrm_Sdk_DataCollectionOfEntity$get_count() {
        return this._internalArray.length;
    },
    get_item: function SparkleXrm_Sdk_DataCollectionOfEntity$get_item(i) {
        return this._internalArray[i];
    },
    set_item: function SparkleXrm_Sdk_DataCollectionOfEntity$set_item(i, value) {
        this._internalArray[i] = value;
        return value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.DateTimeEx

SparkleXrm.Sdk.DateTimeEx = function SparkleXrm_Sdk_DateTimeEx() {
}
SparkleXrm.Sdk.DateTimeEx.toXrmString = function SparkleXrm_Sdk_DateTimeEx$toXrmString(date) {
    var month = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getMonth() + 1, 2);
    var day = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getDate(), 2);
    var hours = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getHours(), 2);
    var mins = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getMinutes(), 2);
    var secs = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getSeconds(), 2);
    return String.format('{0}-{1}-{2}T{3}:{4}:{5}Z', date.getFullYear(), month, day, hours, mins, secs);
}
SparkleXrm.Sdk.DateTimeEx.toXrmStringUTC = function SparkleXrm_Sdk_DateTimeEx$toXrmStringUTC(date) {
    var month = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getUTCMonth() + 1, 2);
    var day = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getUTCDate(), 2);
    var hours = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getUTCHours(), 2);
    var mins = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getUTCMinutes(), 2);
    var secs = SparkleXrm.Sdk.DateTimeEx._padNumber(date.getUTCSeconds(), 2);
    return String.format('{0}-{1}-{2}T{3}:{4}:{5}Z', date.getUTCFullYear(), month, day, hours, mins, secs);
}
SparkleXrm.Sdk.DateTimeEx._padNumber = function SparkleXrm_Sdk_DateTimeEx$_padNumber(number, length) {
    var str = number.toString();
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}
SparkleXrm.Sdk.DateTimeEx.parse = function SparkleXrm_Sdk_DateTimeEx$parse(dateString) {
    if (!String.isNullOrEmpty(dateString)) {
        var dateTimeParsed = (Date.parseDate(dateString));
        return dateTimeParsed;
    }
    else {
        return null;
    }
}
SparkleXrm.Sdk.DateTimeEx.formatDuration = function SparkleXrm_Sdk_DateTimeEx$formatDuration(totalMinutes) {
    if (totalMinutes != null) {
        var toatalSeconds = totalMinutes * 60;
        var days = Math.floor(toatalSeconds / 86400);
        var hours = Math.floor((toatalSeconds % 86400) / 3600);
        var minutes = Math.floor(((toatalSeconds % 86400) % 3600) / 60);
        var seconds = ((toatalSeconds % 86400) % 3600) % 60;
        var formatString = [];
        if (days > 0) {
            SparkleXrm.ArrayEx.add(formatString, '{0}d');
        }
        if (hours > 0) {
            SparkleXrm.ArrayEx.add(formatString, '{1}h');
        }
        if (minutes > 0) {
            SparkleXrm.ArrayEx.add(formatString, '{2}m');
        }
        if (!days && !hours && !minutes) {
            SparkleXrm.ArrayEx.add(formatString, '{2}m');
        }
        return String.format(SparkleXrm.ArrayEx.join(formatString, ' '), days, hours, minutes);
    }
    else {
        return '';
    }
}
SparkleXrm.Sdk.DateTimeEx.parseDuration = function SparkleXrm_Sdk_DateTimeEx$parseDuration(duration) {
    var isEmpty = (duration == null) || (!duration.length);
    if (isEmpty) {
        return null;
    }
    var pattern = '/([0-9.]*)[ ]?((h(our)?[s]?)|(m(inute)?[s]?)|(d(ay)?[s]?))/g';
    var regex = RegExp.parse(pattern);
    var matched = false;
    var thisMatch = false;
    var totalDuration = 0;
    do {
        var match = regex.exec(duration);
        thisMatch = (match != null && match.length > 0);
        matched = matched || thisMatch;
        if (thisMatch) {
            var durationNumber = parseFloat(match[1]);
            switch (match[2].substr(0, 1).toLowerCase()) {
                case 'd':
                    durationNumber = durationNumber * 60 * 24;
                    break;
                case 'h':
                    durationNumber = durationNumber * 60;
                    break;
            }
            totalDuration += Math.floor(durationNumber);
            duration.replaceAll(match[0], '');
        }
    } while (thisMatch);
    if (matched) {
        return totalDuration;
    }
    else {
        return null;
    }
}
SparkleXrm.Sdk.DateTimeEx.addTimeToDate = function SparkleXrm_Sdk_DateTimeEx$addTimeToDate(date, time) {
    if (date == null) {
        date = Date.get_now();
    }
    if (time != null) {
        var dateWithTime = Date.parseDate('01 Jan 2000 ' + time.replaceAll('.', ':').replaceAll('-', ':').replaceAll(',', ':'));
        var newDate = new Date(date.getTime());
        if (!isNaN((dateWithTime))) {
            newDate.setHours(dateWithTime.getHours());
            newDate.setMinutes(dateWithTime.getMinutes());
            newDate.setSeconds(dateWithTime.getSeconds());
            newDate.setMilliseconds(dateWithTime.getMilliseconds());
            return newDate;
        }
        return null;
    }
    return date;
}
SparkleXrm.Sdk.DateTimeEx.localTimeToUTCFromSettings = function SparkleXrm_Sdk_DateTimeEx$localTimeToUTCFromSettings(LocalTime, settings) {
    return SparkleXrm.Sdk.DateTimeEx.localTimeToUTC(LocalTime, settings.timezonebias, settings.timezonedaylightbias, settings.timezonedaylightyear, settings.timezonedaylightmonth, settings.timezonedaylightday, settings.timezonedaylighthour, settings.timezonedaylightminute, settings.timezonedaylightsecond, 0, settings.timezonedaylightdayofweek, settings.timezonestandardbias, settings.timezonestandardyear, settings.timezonestandardmonth, settings.timezonestandardday, settings.timezonestandardhour, settings.timezonestandardminute, settings.timezonestandardsecond, 0, settings.timezonestandarddayofweek);
}
SparkleXrm.Sdk.DateTimeEx.localTimeToUTC = function SparkleXrm_Sdk_DateTimeEx$localTimeToUTC(LocalTime, Bias, DaylightBias, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday, StandardBias, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday) {
    var TimeZoneBias;
    var NewTimeZoneBias;
    var LocalCustomBias;
    var StandardTime;
    var DaylightTime;
    var ComputedUniversalTime;
    var bDaylightTimeZone;
    NewTimeZoneBias = Bias;
    if ((!!StandardMonth) && (!!DaylightMonth)) {
        StandardTime = SparkleXrm.Sdk.DateTimeEx._getCutoverTime(LocalTime, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday);
        if (StandardTime == null) {
            return null;
        }
        DaylightTime = SparkleXrm.Sdk.DateTimeEx._getCutoverTime(LocalTime, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday);
        if (DaylightTime == null) {
            return null;
        }
        if (DaylightTime < StandardTime) {
            if ((LocalTime >= DaylightTime) && (LocalTime < StandardTime)) {
                bDaylightTimeZone = true;
            }
            else {
                bDaylightTimeZone = false;
            }
        }
        else {
            if ((LocalTime >= StandardTime) && (LocalTime < DaylightTime)) {
                bDaylightTimeZone = false;
            }
            else {
                bDaylightTimeZone = true;
            }
        }
        if (bDaylightTimeZone) {
            LocalCustomBias = DaylightBias;
        }
        else {
            LocalCustomBias = StandardBias;
        }
        TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
    }
    else {
        TimeZoneBias = NewTimeZoneBias;
    }
    ComputedUniversalTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias, LocalTime);
    return ComputedUniversalTime;
}
SparkleXrm.Sdk.DateTimeEx.utcToLocalTimeFromSettings = function SparkleXrm_Sdk_DateTimeEx$utcToLocalTimeFromSettings(UTCTime, settings) {
    return SparkleXrm.Sdk.DateTimeEx.utcToLocalTime(UTCTime, settings.timezonebias, settings.timezonedaylightbias, settings.timezonedaylightyear, settings.timezonedaylightmonth, settings.timezonedaylightday, settings.timezonedaylighthour, settings.timezonedaylightminute, settings.timezonedaylightsecond, 0, settings.timezonedaylightdayofweek, settings.timezonestandardbias, settings.timezonestandardyear, settings.timezonestandardmonth, settings.timezonestandardday, settings.timezonestandardhour, settings.timezonestandardminute, settings.timezonestandardsecond, 0, settings.timezonestandarddayofweek);
}
SparkleXrm.Sdk.DateTimeEx.utcToLocalTime = function SparkleXrm_Sdk_DateTimeEx$utcToLocalTime(UTCTime, Bias, DaylightBias, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday, StandardBias, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday) {
    var TimeZoneBias = 0;
    var NewTimeZoneBias = 0;
    var LocalCustomBias = 0;
    var StandardTime;
    var DaylightTime;
    var UtcStandardTime;
    var UtcDaylightTime;
    var ComputedLocalTime;
    var bDaylightTimeZone;
    NewTimeZoneBias = Bias;
    if ((!!StandardMonth) && (!!DaylightMonth)) {
        StandardTime = SparkleXrm.Sdk.DateTimeEx._getCutoverTime(UTCTime, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday);
        if (StandardTime == null) {
            return null;
        }
        DaylightTime = SparkleXrm.Sdk.DateTimeEx._getCutoverTime(UTCTime, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday);
        if (DaylightTime == null) {
            return null;
        }
        LocalCustomBias = StandardBias;
        TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
        UtcDaylightTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias, DaylightTime);
        LocalCustomBias = DaylightBias;
        TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
        UtcStandardTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias, StandardTime);
        if (UtcDaylightTime < UtcStandardTime) {
            if ((UTCTime >= UtcDaylightTime) && (UTCTime < UtcStandardTime)) {
                bDaylightTimeZone = true;
            }
            else {
                bDaylightTimeZone = false;
            }
        }
        else {
            if ((UTCTime >= UtcStandardTime) && (UTCTime < UtcDaylightTime)) {
                bDaylightTimeZone = false;
            }
            else {
                bDaylightTimeZone = true;
            }
        }
        if (bDaylightTimeZone) {
            LocalCustomBias = DaylightBias;
        }
        else {
            LocalCustomBias = StandardBias;
        }
        TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
    }
    else {
        TimeZoneBias = NewTimeZoneBias;
    }
    ComputedLocalTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias * -1, UTCTime);
    return ComputedLocalTime;
}
SparkleXrm.Sdk.DateTimeEx._getCutoverTime = function SparkleXrm_Sdk_DateTimeEx$_getCutoverTime(CurrentTime, Year, Month, Day, Hour, Minute, Second, Milliseconds, Weekday) {
    if (!!Year) {
        return null;
    }
    var WorkingTime;
    var ScratchTime;
    var BestWeekdayDate;
    var WorkingWeekdayNumber;
    var TargetWeekdayNumber;
    var TargetYear;
    var TargetMonth;
    var TargetWeekday;
    TargetWeekdayNumber = Day;
    if ((TargetWeekdayNumber > 5) || (!TargetWeekdayNumber)) {
        return null;
    }
    TargetWeekday = Weekday;
    TargetMonth = Month;
    TargetYear = CurrentTime.getFullYear();
    BestWeekdayDate = 0;
    WorkingTime = SparkleXrm.Sdk.DateTimeEx.firstDayOfMonth(CurrentTime, TargetMonth);
    WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('hours', Hour, WorkingTime);
    WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('minutes', Minute, WorkingTime);
    WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('seconds', Second, WorkingTime);
    WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('milliseconds', Milliseconds, WorkingTime);
    ScratchTime = WorkingTime;
    if (ScratchTime.getDay() > TargetWeekday) {
        WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('days', (7 - (ScratchTime.getDay() - TargetWeekday)), WorkingTime);
    }
    else if (ScratchTime.getDay() < TargetWeekday) {
        WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('days', (TargetWeekday - ScratchTime.getDay()), WorkingTime);
    }
    BestWeekdayDate = WorkingTime.getDay();
    WorkingWeekdayNumber = 1;
    ScratchTime = WorkingTime;
    while (WorkingWeekdayNumber < TargetWeekdayNumber) {
        WorkingTime = SparkleXrm.Sdk.DateTimeEx.dateAdd('days', 7, WorkingTime);
        if (WorkingTime.getMonth() !== ScratchTime.getMonth()) {
            break;
        }
        ScratchTime = WorkingTime;
        WorkingWeekdayNumber = WorkingWeekdayNumber + 1;
    }
    return ScratchTime;
}
SparkleXrm.Sdk.DateTimeEx.firstDayOfMonth = function SparkleXrm_Sdk_DateTimeEx$firstDayOfMonth(date, Month) {
    var startOfMonth = new Date(date.getTime());
    startOfMonth.setMonth(Month - 1);
    startOfMonth.setDate(1);
    startOfMonth.setHours(0);
    startOfMonth.setMinutes(0);
    startOfMonth.setSeconds(0);
    startOfMonth.setMilliseconds(0);
    return startOfMonth;
}
SparkleXrm.Sdk.DateTimeEx.dateAdd = function SparkleXrm_Sdk_DateTimeEx$dateAdd(interval, value, date) {
    var ms = date.getTime();
    var result;
    switch (interval) {
        case 'milliseconds':
            result = new Date(ms + value);
            break;
        case 'seconds':
            result = new Date(ms + (value * 1000));
            break;
        case 'minutes':
            result = new Date(ms + (value * 1000 * 60));
            break;
        case 'hours':
            result = new Date(ms + (value * 1000 * 60 * 60));
            break;
        case 'days':
            result = new Date(ms + (value * 1000 * 60 * 60 * 24));
            break;
        default:
            result = date;
            break;
    }
    return result;
}
SparkleXrm.Sdk.DateTimeEx.firstDayOfWeek = function SparkleXrm_Sdk_DateTimeEx$firstDayOfWeek(date) {
    var weekStartOffset = 0;
    if (SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings != null) {
        weekStartOffset = SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings.weekstartdaycode.value;
    }
    var startOfWeek = new Date(date.getTime());
    var dayOfWeek = startOfWeek.getDay();
    dayOfWeek = dayOfWeek - weekStartOffset;
    if (dayOfWeek < 0) {
        dayOfWeek = 7 + dayOfWeek;
    }
    if (dayOfWeek > 0) {
        startOfWeek = SparkleXrm.Sdk.DateTimeEx.dateAdd('days', (dayOfWeek * -1), startOfWeek);
    }
    startOfWeek.setHours(0);
    startOfWeek.setMinutes(0);
    startOfWeek.setSeconds(0);
    startOfWeek.setMilliseconds(0);
    return startOfWeek;
}
SparkleXrm.Sdk.DateTimeEx.lastDayOfWeek = function SparkleXrm_Sdk_DateTimeEx$lastDayOfWeek(date) {
    var weekStartOffset = 0;
    if (SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings != null) {
        weekStartOffset = SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings.weekstartdaycode.value;
    }
    var endOfWeek = new Date(date.getTime());
    var dayOfWeek = endOfWeek.getDay();
    dayOfWeek = dayOfWeek - weekStartOffset;
    if (dayOfWeek < 0) {
        dayOfWeek = 7 + dayOfWeek;
    }
    endOfWeek = SparkleXrm.Sdk.DateTimeEx.dateAdd('days', (6 - dayOfWeek), endOfWeek);
    endOfWeek.setHours(23);
    endOfWeek.setMinutes(59);
    endOfWeek.setSeconds(59);
    endOfWeek.setMilliseconds(999);
    return endOfWeek;
}
SparkleXrm.Sdk.DateTimeEx.formatTimeSpecific = function SparkleXrm_Sdk_DateTimeEx$formatTimeSpecific(dateValue, formatString) {
    formatString = formatString.replaceAll('.', ':').replaceAll('-', ':').replaceAll(',', ':');
    if (dateValue != null && (Date === Type.getInstanceType(dateValue))) {
        return dateValue.format(formatString);
    }
    else {
        return '';
    }
}
SparkleXrm.Sdk.DateTimeEx.formatDateSpecific = function SparkleXrm_Sdk_DateTimeEx$formatDateSpecific(dateValue, formatString) {
    if (dateValue != null) {
        return xrmjQuery.datepicker.formatDate( formatString, dateValue );
    }
    else {
        return '';
    }
}
SparkleXrm.Sdk.DateTimeEx.parseDateSpecific = function SparkleXrm_Sdk_DateTimeEx$parseDateSpecific(dateValue, formatString) {
    return xrmjQuery.datepicker.parseDate( formatString, dateValue );
}
SparkleXrm.Sdk.DateTimeEx.setTime = function SparkleXrm_Sdk_DateTimeEx$setTime(date, time) {
    if (date != null && time != null) {
        date.setHours(time.getHours());
        date.setMinutes(time.getMinutes());
        date.setSeconds(time.getSeconds());
        date.setMilliseconds(time.getMilliseconds());
    }
}
SparkleXrm.Sdk.DateTimeEx.setUTCTime = function SparkleXrm_Sdk_DateTimeEx$setUTCTime(date, time) {
    if (date != null && time != null) {
        date.setUTCHours(time.getUTCHours());
        date.setUTCMinutes(time.getUTCMinutes());
        date.setUTCSeconds(time.getUTCSeconds());
        date.setUTCMilliseconds(time.getUTCMilliseconds());
    }
}
SparkleXrm.Sdk.DateTimeEx.getTimeDuration = function SparkleXrm_Sdk_DateTimeEx$getTimeDuration(date) {
    return (date.getHours() * (60 * 60)) + (date.getMinutes() * 60) + date.getSeconds();
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Entity

SparkleXrm.Sdk.Entity = function SparkleXrm_Sdk_Entity(entityName) {
    this._metaData = {};
    this.logicalName = entityName;
    this._attributes = {};
    this.formattedValues = {};
}
SparkleXrm.Sdk.Entity.sortDelegate = function SparkleXrm_Sdk_Entity$sortDelegate(attributeName, a, b) {
    var l = a.getAttributeValue(attributeName);
    var r = b.getAttributeValue(attributeName);
    var result = 0;
    var typeName = '';
    if (l != null) {
        typeName = Type.getInstanceType(l).get_name();
    }
    else if (r != null) {
        typeName = Type.getInstanceType(r).get_name();
    }
    if (l !== r) {
        switch (typeName.toLowerCase()) {
            case 'string':
                l = (l != null) ? (l).toLowerCase() : null;
                r = (r != null) ? (r).toLowerCase() : null;
                if (l<r) {
                    result = -1;
                }
                else {
                    result = 1;
                }
                break;
            case 'date':
                if (l == null) {
                    result = -1;
                }
                else if (r == null) {
                    result = 1;
                }
                else if (l<r) {
                    result = -1;
                }
                else {
                    result = 1;
                }
                break;
            case 'number':
                var ln = (l != null) ? (l) : 0;
                var rn = (r != null) ? (r) : 0;
                result = (ln - rn);
                break;
            case 'money':
                var lm = (l != null) ? (l).value : 0;
                var rm = (r != null) ? (r).value : 0;
                result = (lm - rm);
                break;
            case 'optionsetvalue':
                var lo = (l != null) ? (l).value : 0;
                lo = (lo != null) ? lo : 0;
                var ro = (r != null) ? (r).value : 0;
                ro = (ro != null) ? ro : 0;
                result = (lo - ro);
                break;
            case 'entityreference':
                var le = ((l != null) && ((l).name != null)) ? (l).name : '';
                var re = (r != null && ((r).name != null)) ? (r).name : '';
                if (le<re) {
                    result = -1;
                }
                else {
                    result = 1;
                }
                break;
        }
    }
    return result;
}
SparkleXrm.Sdk.Entity.prototype = {
    logicalName: null,
    id: null,
    entityState: 0,
    _attributes: null,
    formattedValues: null,
    relatedEntities: null,
    
    deSerialise: function SparkleXrm_Sdk_Entity$deSerialise(entityNode) {
        this.logicalName = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(entityNode, 'LogicalName');
        this.id = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(entityNode, 'Id');
        var attributes = SparkleXrm.Sdk.XmlHelper.selectSingleNode(entityNode, 'Attributes');
        var attributeCount = attributes.childNodes.length;
        for (var i = 0; i < attributeCount; i++) {
            var node = attributes.childNodes[i];
            try {
                var attributeName = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'key');
                var attributeValue = SparkleXrm.Sdk.Attribute.deSerialise(SparkleXrm.Sdk.XmlHelper.selectSingleNode(node, 'value'), null);
                this._attributes[attributeName] = attributeValue;
                this._setDictionaryValue(attributeName, attributeValue);
            }
            catch (e) {
                throw new Error('Invalid Attribute Value :' + SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node) + ':' + e.message);
            }
        }
        var formattedValues = SparkleXrm.Sdk.XmlHelper.selectSingleNode(entityNode, 'FormattedValues');
        if (formattedValues != null) {
            for (var i = 0; i < formattedValues.childNodes.length; i++) {
                var node = formattedValues.childNodes[i];
                var key = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'key');
                var value = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'value');
                this._setDictionaryValue(key + 'name', value);
                this.formattedValues[key + 'name'] = value;
                var att = this._attributes[key];
                if (att != null) {
                    att.name=value;
                }
            }
        }
        var relatedEntities = SparkleXrm.Sdk.XmlHelper.selectSingleNode(entityNode, 'RelatedEntities');
        if (relatedEntities != null) {
            var relatedEntitiesColection = {};
            for (var i = 0; i < relatedEntities.childNodes.length; i++) {
                var node = relatedEntities.childNodes[i];
                var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(node, 'key');
                var schemaName = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(key, 'SchemaName');
                var relationship = new SparkleXrm.Sdk.Relationship(schemaName);
                var value = SparkleXrm.Sdk.XmlHelper.selectSingleNode(node, 'value');
                var entities = SparkleXrm.Sdk.EntityCollection.deSerialise(value);
                relatedEntitiesColection[relationship.schemaName] = entities;
            }
            this.relatedEntities = relatedEntitiesColection;
        }
    },
    
    _setDictionaryValue: function SparkleXrm_Sdk_Entity$_setDictionaryValue(key, value) {
        var self = this;
        var thisAsDictionary = Type.safeCast(self, Object);
        thisAsDictionary[key] = value;
    },
    
    serialise: function SparkleXrm_Sdk_Entity$serialise(ommitRoot) {
        var xml = '';
        if (ommitRoot == null || !ommitRoot) {
            xml += '<a:Entity>';
        }
        xml += '<a:Attributes>';
        var record = (this);
        if (record[this.logicalName + 'id'] == null) {
            delete record[this.logicalName + 'id'];
        }
        var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(record));
        while ($enum1.moveNext()) {
            var key = $enum1.current;
            if (typeof(record[key])!="function" && Object.prototype.hasOwnProperty.call(this, key) && !SparkleXrm.StringEx.IN(key, [ 'id', 'logicalName', 'entityState', 'formattedValues', 'relatedEntities' ]) && !key.startsWith('$') && !key.startsWith('_')) {
                var attributeValue = record[key];
                if (!Object.keyExists(this.formattedValues, key)) {
                    xml += SparkleXrm.Sdk.Attribute.serialise(key, attributeValue, this._metaData);
                }
            }
        }
        xml += '</a:Attributes>';
        xml += '<a:LogicalName>' + this.logicalName + '</a:LogicalName>';
        if (this.id != null) {
            xml += '<a:Id>' + this.id + '</a:Id>';
        }
        if (ommitRoot == null || !ommitRoot) {
            xml += '</a:Entity>';
        }
        return xml;
    },
    
    setAttributeValue: function SparkleXrm_Sdk_Entity$setAttributeValue(name, value) {
        this._attributes[name] = value;
        this._setDictionaryValue(name, value);
    },
    
    getAttributeValue: function SparkleXrm_Sdk_Entity$getAttributeValue(attributeName) {
        return this[attributeName];
    },
    
    getAttributeValueOptionSet: function SparkleXrm_Sdk_Entity$getAttributeValueOptionSet(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueGuid: function SparkleXrm_Sdk_Entity$getAttributeValueGuid(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueInt: function SparkleXrm_Sdk_Entity$getAttributeValueInt(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueFloat: function SparkleXrm_Sdk_Entity$getAttributeValueFloat(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueString: function SparkleXrm_Sdk_Entity$getAttributeValueString(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueEntityReference: function SparkleXrm_Sdk_Entity$getAttributeValueEntityReference(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    raisePropertyChanged: function SparkleXrm_Sdk_Entity$raisePropertyChanged(propertyName) {
        var args = {};
        args.propertyName = propertyName;
        if (this.__propertyChanged != null) {
            this.__propertyChanged(this, args);
        }
        if (propertyName !== 'EntityState' && !this.entityState && this.entityState !== 1) {
            this.entityState = 2;
        }
    },
    
    toEntityReference: function SparkleXrm_Sdk_Entity$toEntityReference() {
        return new SparkleXrm.Sdk.EntityReference(new SparkleXrm.Sdk.Guid(this.id), this.logicalName, '');
    },
    
    add_propertyChanged: function SparkleXrm_Sdk_Entity$add_propertyChanged(value) {
        this.__propertyChanged = ss.Delegate.combine(this.__propertyChanged, value);
    },
    remove_propertyChanged: function SparkleXrm_Sdk_Entity$remove_propertyChanged(value) {
        this.__propertyChanged = ss.Delegate.remove(this.__propertyChanged, value);
    },
    
    __propertyChanged: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.EntityCollection

SparkleXrm.Sdk.EntityCollection = function SparkleXrm_Sdk_EntityCollection(entities) {
    this._entities = new SparkleXrm.Sdk.DataCollectionOfEntity(entities);
}
SparkleXrm.Sdk.EntityCollection.serialise = function SparkleXrm_Sdk_EntityCollection$serialise(value) {
    var valueXml = '';
    if (Type.getInstanceType(value) !== SparkleXrm.Sdk.EntityCollection) {
        throw new Error("An attribute value of type 'EntityCollection' must contain an EntityCollection instance");
    }
    var arrayValue = Type.safeCast(value, SparkleXrm.Sdk.EntityCollection);
    valueXml += '<a:Entities>';
    for (var i = 0; i < arrayValue._entities.get_count(); i++) {
        valueXml += (arrayValue.get_item(i)).serialise(false);
    }
    valueXml += '</a:Entities>';
    return valueXml;
}
SparkleXrm.Sdk.EntityCollection.deSerialise = function SparkleXrm_Sdk_EntityCollection$deSerialise(node) {
    var entities = [];
    var collection = new SparkleXrm.Sdk.EntityCollection(entities);
    collection.set_entityName(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'EntityName'));
    var entitiesNode = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(node, 'Entities');
    var $enum1 = ss.IEnumerator.getEnumerator(entitiesNode.childNodes);
    while ($enum1.moveNext()) {
        var entityNode = $enum1.current;
        var entity = new SparkleXrm.Sdk.Entity(collection.get_entityName());
        entity.deSerialise(entityNode);
        SparkleXrm.ArrayEx.add(entities, entity);
    }
    return collection;
}
SparkleXrm.Sdk.EntityCollection.prototype = {
    _entities: null,
    
    get_entities: function SparkleXrm_Sdk_EntityCollection$get_entities() {
        return this._entities;
    },
    set_entities: function SparkleXrm_Sdk_EntityCollection$set_entities(value) {
        this._entities = value;
        return value;
    },
    
    _entityName: null,
    
    get_entityName: function SparkleXrm_Sdk_EntityCollection$get_entityName() {
        return this._entityName;
    },
    set_entityName: function SparkleXrm_Sdk_EntityCollection$set_entityName(value) {
        this._entityName = value;
        return value;
    },
    
    _minActiveRowVersion: null,
    
    get_minActiveRowVersion: function SparkleXrm_Sdk_EntityCollection$get_minActiveRowVersion() {
        return this._minActiveRowVersion;
    },
    set_minActiveRowVersion: function SparkleXrm_Sdk_EntityCollection$set_minActiveRowVersion(value) {
        this._minActiveRowVersion = value;
        return value;
    },
    
    _moreRecords: false,
    
    get_moreRecords: function SparkleXrm_Sdk_EntityCollection$get_moreRecords() {
        return this._moreRecords;
    },
    set_moreRecords: function SparkleXrm_Sdk_EntityCollection$set_moreRecords(value) {
        this._moreRecords = value;
        return value;
    },
    
    _pagingCookie: null,
    
    get_pagingCookie: function SparkleXrm_Sdk_EntityCollection$get_pagingCookie() {
        return this._pagingCookie;
    },
    set_pagingCookie: function SparkleXrm_Sdk_EntityCollection$set_pagingCookie(value) {
        this._pagingCookie = value;
        return value;
    },
    
    _totalRecordCount: 0,
    
    get_totalRecordCount: function SparkleXrm_Sdk_EntityCollection$get_totalRecordCount() {
        return this._totalRecordCount;
    },
    set_totalRecordCount: function SparkleXrm_Sdk_EntityCollection$set_totalRecordCount(value) {
        this._totalRecordCount = value;
        return value;
    },
    
    _totalRecordCountLimitExceeded: false,
    
    get_totalRecordCountLimitExceeded: function SparkleXrm_Sdk_EntityCollection$get_totalRecordCountLimitExceeded() {
        return this._totalRecordCountLimitExceeded;
    },
    set_totalRecordCountLimitExceeded: function SparkleXrm_Sdk_EntityCollection$set_totalRecordCountLimitExceeded(value) {
        this._totalRecordCountLimitExceeded = value;
        return value;
    },
    get_item: function SparkleXrm_Sdk_EntityCollection$get_item(index) {
        return this.get_entities().get_item(index);
    },
    set_item: function SparkleXrm_Sdk_EntityCollection$set_item(index, value) {
        this.get_entities().set_item(index, value);
        return value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.EntityReference

SparkleXrm.Sdk.EntityReference = function SparkleXrm_Sdk_EntityReference(Id, LogicalName, Name) {
    this.id = Id;
    this.logicalName = LogicalName;
    this.name = Name;
}
SparkleXrm.Sdk.EntityReference.prototype = {
    name: null,
    id: null,
    logicalName: null,
    
    toString: function SparkleXrm_Sdk_EntityReference$toString() {
        return String.format('[EntityReference: {0},{1},{2}]', this.name, this.id, this.logicalName);
    },
    
    toSoap: function SparkleXrm_Sdk_EntityReference$toSoap(NameSpace) {
        if (NameSpace == null || !NameSpace) {
            NameSpace = 'a';
        }
        return String.format('<{0}:EntityReference><{0}:Id>{1}</{0}:Id><{0}:LogicalName>{2}</{0}:LogicalName><{0}:Name i:nil="true" /></{0}:EntityReference>', NameSpace, this.id.value, this.logicalName);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Guid

SparkleXrm.Sdk.Guid = function SparkleXrm_Sdk_Guid(Value) {
    this.value = Value;
}
SparkleXrm.Sdk.Guid.prototype = {
    value: null,
    
    toString: function SparkleXrm_Sdk_Guid$toString() {
        return this.value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Money

SparkleXrm.Sdk.Money = function SparkleXrm_Sdk_Money(value) {
    this.value = value;
}
SparkleXrm.Sdk.Money.prototype = {
    value: 0
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.OptionSetValue

SparkleXrm.Sdk.OptionSetValue = function SparkleXrm_Sdk_OptionSetValue(value) {
    this.value = value;
}
SparkleXrm.Sdk.OptionSetValue.parse = function SparkleXrm_Sdk_OptionSetValue$parse(value) {
    if (String.isNullOrEmpty(value)) {
        return new SparkleXrm.Sdk.OptionSetValue(null);
    }
    else {
        return new SparkleXrm.Sdk.OptionSetValue(parseInt(value));
    }
}
SparkleXrm.Sdk.OptionSetValue.prototype = {
    name: null,
    value: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.OrganizationServiceProxy

SparkleXrm.Sdk.OrganizationServiceProxy = function SparkleXrm_Sdk_OrganizationServiceProxy() {
}
SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType = function SparkleXrm_Sdk_OrganizationServiceProxy$registerExecuteMessageResponseType(responseTypeName, organizationResponseType) {
    SparkleXrm.Sdk.OrganizationServiceProxy.executeMessageResponseTypes[responseTypeName] = organizationResponseType;
}
SparkleXrm.Sdk.OrganizationServiceProxy.getUserSettings = function SparkleXrm_Sdk_OrganizationServiceProxy$getUserSettings() {
    if (SparkleXrm.Sdk.OrganizationServiceProxy.userSettings == null) {
        SparkleXrm.Sdk.OrganizationServiceProxy.userSettings = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(SparkleXrm.Sdk.UserSettings.entityLogicalName, Xrm.Page.context.getUserId(), [ 'AllColumns' ]);
        SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring.replaceAll(':', SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.timeseparator);
        SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring.replaceAll('/', SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.dateseparator);
        SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring = SparkleXrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring.replaceAll('MM', 'mm').replaceAll('yyyy', 'UU').replaceAll('yy', 'y').replaceAll('UU', 'yy').replaceAll('M', 'm');
    }
    if (SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings == null) {
        var fetchXml = "<fetch>\r\n                                    <entity name='organization' >\r\n                                        <attribute name='weekstartdaycode' />\r\n                                    </entity>\r\n                                </fetch>";
        SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchXml).get_entities().get_item(0);
    }
    return SparkleXrm.Sdk.OrganizationServiceProxy.userSettings;
}
SparkleXrm.Sdk.OrganizationServiceProxy.doesNNAssociationExist = function SparkleXrm_Sdk_OrganizationServiceProxy$doesNNAssociationExist(relationship, Entity1, Entity2) {
    var fetchXml = "<fetch mapping='logical'>" + "  <entity name='" + relationship.schemaName + "'>" + '    <all-attributes />' + '    <filter>' + "      <condition attribute='" + Entity1.logicalName + "id' operator='eq' value ='" + Entity1.id.value + "' />" + "      <condition attribute='" + Entity2.logicalName + "id' operator='eq' value='" + Entity2.id.value + "' />" + '    </filter>' + '  </entity>' + '</fetch>';
    var result = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchXml);
    if (result.get_entities().get_count() > 0) {
        return true;
    }
    return false;
}
SparkleXrm.Sdk.OrganizationServiceProxy.associate = function SparkleXrm_Sdk_OrganizationServiceProxy$associate(entityName, entityId, relationship, relatedEntities) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getAssociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', null);
    delete resultXml;
    resultXml = null;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginAssociate = function SparkleXrm_Sdk_OrganizationServiceProxy$beginAssociate(entityName, entityId, relationship, relatedEntities, callBack) {
    SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getAssociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endAssociate = function SparkleXrm_Sdk_OrganizationServiceProxy$endAssociate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getAssociateRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getAssociateRequest(entityName, entityId, relationship, relatedEntities) {
    var entityReferences = '';
    var $enum1 = ss.IEnumerator.getEnumerator(relatedEntities);
    while ($enum1.moveNext()) {
        var item = $enum1.current;
        entityReferences += item.toSoap('a');
    }
    var xmlSoapBody = '<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">';
    xmlSoapBody += '      <request i:type="a:AssociateRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">';
    xmlSoapBody += '        <a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">';
    xmlSoapBody += '          <a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '            <b:key>Target</b:key>';
    xmlSoapBody += '            <b:value i:type="a:EntityReference">';
    xmlSoapBody += '              <a:Id>' + entityId.value + '</a:Id>';
    xmlSoapBody += '              <a:LogicalName>' + entityName + '</a:LogicalName>';
    xmlSoapBody += '              <a:Name i:nil="true" />';
    xmlSoapBody += '            </b:value>';
    xmlSoapBody += '          </a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '          <a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '            <b:key>Relationship</b:key>';
    xmlSoapBody += '            <b:value i:type="a:Relationship">';
    xmlSoapBody += '              <a:PrimaryEntityRole i:nil="true" />';
    xmlSoapBody += '              <a:SchemaName>' + relationship.schemaName + '</a:SchemaName>';
    xmlSoapBody += '            </b:value>';
    xmlSoapBody += '          </a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '          <a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '            <b:key>RelatedEntities</b:key>';
    xmlSoapBody += '            <b:value i:type="a:EntityReferenceCollection">' + entityReferences + '</b:value>';
    xmlSoapBody += '          </a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '        </a:Parameters>';
    xmlSoapBody += '        <a:RequestId i:nil="true" />';
    xmlSoapBody += '        <a:RequestName>Associate</a:RequestName>';
    xmlSoapBody += '      </request>';
    xmlSoapBody += '    </Execute>';
    return xmlSoapBody;
}
SparkleXrm.Sdk.OrganizationServiceProxy.disassociate = function SparkleXrm_Sdk_OrganizationServiceProxy$disassociate(entityName, entityId, relationship, relatedEntities) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getDisassociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', null);
    delete resultXml;
    resultXml = null;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginDisassociate = function SparkleXrm_Sdk_OrganizationServiceProxy$beginDisassociate(entityName, entityId, relationship, relatedEntities, callBack) {
    SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getDisassociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endDisassociate = function SparkleXrm_Sdk_OrganizationServiceProxy$endDisassociate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getDisassociateRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getDisassociateRequest(entityName, entityId, relationship, relatedEntities) {
    var entityReferences = '';
    var $enum1 = ss.IEnumerator.getEnumerator(relatedEntities);
    while ($enum1.moveNext()) {
        var item = $enum1.current;
        entityReferences += item.toSoap('a');
    }
    var xmlSoapBody = '<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">';
    xmlSoapBody += '      <request i:type="a:DisassociateRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">';
    xmlSoapBody += '        <a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">';
    xmlSoapBody += '          <a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '            <b:key>Target</b:key>';
    xmlSoapBody += '            <b:value i:type="a:EntityReference">';
    xmlSoapBody += '              <a:Id>' + entityId.value + '</a:Id>';
    xmlSoapBody += '              <a:LogicalName>' + entityName + '</a:LogicalName>';
    xmlSoapBody += '              <a:Name i:nil="true" />';
    xmlSoapBody += '            </b:value>';
    xmlSoapBody += '          </a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '          <a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '            <b:key>Relationship</b:key>';
    xmlSoapBody += '            <b:value i:type="a:Relationship">';
    xmlSoapBody += '              <a:PrimaryEntityRole i:nil="true" />';
    xmlSoapBody += '              <a:SchemaName>' + relationship.schemaName + '</a:SchemaName>';
    xmlSoapBody += '            </b:value>';
    xmlSoapBody += '          </a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '          <a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '            <b:key>RelatedEntities</b:key>';
    xmlSoapBody += '            <b:value i:type="a:EntityReferenceCollection">' + entityReferences + '</b:value>';
    xmlSoapBody += '          </a:KeyValuePairOfstringanyType>';
    xmlSoapBody += '        </a:Parameters>';
    xmlSoapBody += '        <a:RequestId i:nil="true" />';
    xmlSoapBody += '        <a:RequestName>Disassociate</a:RequestName>';
    xmlSoapBody += '      </request>';
    xmlSoapBody += '    </Execute>';
    return xmlSoapBody;
}
SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple = function SparkleXrm_Sdk_OrganizationServiceProxy$retrieveMultiple(fetchXml) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getRetrieveMultipleRequest(fetchXml), 'RetrieveMultiple', null);
    var entities = SparkleXrm.Sdk.OrganizationServiceProxy._getEntityCollectionResults(resultXml, SparkleXrm.Sdk.Entity);
    delete resultXml;
    resultXml = null;
    return entities;
}
SparkleXrm.Sdk.OrganizationServiceProxy._getRetrieveMultipleRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getRetrieveMultipleRequest(fetchXml) {
    var xml = '<RetrieveMultiple xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><query i:type="a:FetchExpression" ><a:Query>';
    xml += SparkleXrm.Sdk.XmlHelper.encode(fetchXml);
    xml += '</a:Query></query></RetrieveMultiple>';
    return xml;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple = function SparkleXrm_Sdk_OrganizationServiceProxy$beginRetrieveMultiple(fetchXml, callBack) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getRetrieveMultipleRequest(fetchXml), 'RetrieveMultiple', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple = function SparkleXrm_Sdk_OrganizationServiceProxy$endRetrieveMultiple(asyncState, entityType) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        var entities = SparkleXrm.Sdk.OrganizationServiceProxy._getEntityCollectionResults(xmlDocument, entityType);
        return entities;
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getEntityCollectionResults = function SparkleXrm_Sdk_OrganizationServiceProxy$_getEntityCollectionResults(resultXml, entityType) {
    var soapBody = resultXml.firstChild.firstChild;
    var resultNode = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(soapBody, 'RetrieveMultipleResult');
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(resultNode, 'Entities');
    var resultCount = 0;
    if (results != null) {
        resultCount = results.childNodes.length;
    }
    var businessEntities = [];
    for (var i = 0; i < resultCount; i++) {
        var entityNode = results.childNodes[i];
        var entity = new entityType(null);
        entity.deSerialise(entityNode);
        businessEntities[i] = entity;
    }
    var entities = new SparkleXrm.Sdk.EntityCollection(businessEntities);
    entities.set_moreRecords(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'MoreRecords') === 'true');
    entities.set_pagingCookie(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'PagingCookie'));
    entities.set_totalRecordCount(parseInt(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'TotalRecordCount')));
    entities.set_totalRecordCountLimitExceeded(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'TotalRecordCountLimitExceeded') === 'true');
    return entities;
}
SparkleXrm.Sdk.OrganizationServiceProxy.retrieve = function SparkleXrm_Sdk_OrganizationServiceProxy$retrieve(entityName, entityId, attributesList) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getRetrieveRequest(entityName, entityId, attributesList), 'Retrieve', null);
    var entityNode = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(resultXml, 'RetrieveResult');
    var entity = new SparkleXrm.Sdk.Entity(entityName);
    entity.deSerialise(entityNode);
    delete resultXml;
    resultXml = null;
    return entity;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieve = function SparkleXrm_Sdk_OrganizationServiceProxy$beginRetrieve(entityName, entityId, attributesList, callBack) {
    SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getRetrieveRequest(entityName, entityId, attributesList), 'Retrieve', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieve = function SparkleXrm_Sdk_OrganizationServiceProxy$endRetrieve(asyncState, entityType) {
    var resultXml = asyncState;
    var entityNode = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(resultXml, 'RetrieveResult');
    var entity = new SparkleXrm.Sdk.Entity(null);
    entity.deSerialise(entityNode);
    return entity;
}
SparkleXrm.Sdk.OrganizationServiceProxy._getRetrieveRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getRetrieveRequest(entityName, entityId, attributesList) {
    var xml = '<Retrieve xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services">';
    xml += '<entityName>' + entityName + '</entityName>';
    xml += '<id>' + entityId + '</id>';
    xml += '<columnSet xmlns:d4p1="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">';
    if ((attributesList != null) && (attributesList[0] !== 'AllColumns')) {
        xml += '<d4p1:AllColumns>false</d4p1:AllColumns>';
        xml += '<d4p1:Columns xmlns:d5p1="http://schemas.microsoft.com/2003/10/Serialization/Arrays">';
        for (var i = 0; i < attributesList.length; i++) {
            xml += '<d5p1:string>' + attributesList[i] + '</d5p1:string>';
        }
        xml += '</d4p1:Columns>';
    }
    else {
        xml += '<d4p1:AllColumns>true</d4p1:AllColumns>';
    }
    xml += '</columnSet></Retrieve>';
    return xml;
}
SparkleXrm.Sdk.OrganizationServiceProxy.create = function SparkleXrm_Sdk_OrganizationServiceProxy$create(entity) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getCreateRequest(entity), 'Create', null);
    var newGuid = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValueDeep(resultXml, 'CreateResult');
    delete resultXml;
    resultXml = null;
    return new SparkleXrm.Sdk.Guid(newGuid);
}
SparkleXrm.Sdk.OrganizationServiceProxy._getCreateRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getCreateRequest(entity) {
    var xml = '<Create xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entity>';
    xml += entity.serialise(true);
    xml += '</entity></Create>';
    return xml;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginCreate = function SparkleXrm_Sdk_OrganizationServiceProxy$beginCreate(entity, callBack) {
    SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getCreateRequest(entity), 'Create', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endCreate = function SparkleXrm_Sdk_OrganizationServiceProxy$endCreate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        var newGuid = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValueDeep(xmlDocument, 'CreateResult');
        return new SparkleXrm.Sdk.Guid(newGuid);
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy.setState = function SparkleXrm_Sdk_OrganizationServiceProxy$setState(id, entityName, stateCode, statusCode) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getSetStateRequest(id, entityName, stateCode, statusCode), 'Execute', null);
    delete resultXml;
    resultXml = null;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginSetState = function SparkleXrm_Sdk_OrganizationServiceProxy$beginSetState(id, entityName, stateCode, statusCode, callBack) {
    SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getSetStateRequest(id, entityName, stateCode, statusCode), 'Execute', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endSetState = function SparkleXrm_Sdk_OrganizationServiceProxy$endSetState(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getSetStateRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getSetStateRequest(id, entityName, stateCode, statusCode) {
    return String.format('<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services">' + '<request i:type="b:SetStateRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '<a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<c:key>EntityMoniker</c:key>' + '<c:value i:type="a:EntityReference">' + '<a:Id>{0}</a:Id>' + '<a:LogicalName>{1}</a:LogicalName>' + '<a:Name i:nil="true" />' + '</c:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<c:key>State</c:key>' + '<c:value i:type="a:OptionSetValue">' + '<a:Value>{2}</a:Value>' + '</c:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<c:key>Status</c:key>' + '<c:value i:type="a:OptionSetValue">' + '<a:Value>{3}</a:Value>' + '</c:value>' + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>SetState</a:RequestName>' + '</request></Execute>', id.value, entityName, stateCode, statusCode);
}
SparkleXrm.Sdk.OrganizationServiceProxy.delete_ = function SparkleXrm_Sdk_OrganizationServiceProxy$delete_(entityName, id) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getDeleteRequest(entityName, id), 'Delete', null);
    var newGuid = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValueDeep(resultXml, 'DeleteResult');
    delete resultXml;
    resultXml = null;
    return newGuid;
}
SparkleXrm.Sdk.OrganizationServiceProxy._getDeleteRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getDeleteRequest(entityName, id) {
    var xml = String.format('<Delete xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entityName>{0}</entityName><id>{1}</id></Delete>', entityName, id.value);
    return xml;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginDelete = function SparkleXrm_Sdk_OrganizationServiceProxy$beginDelete(entityName, id, callBack) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getDeleteRequest(entityName, id), 'Delete', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endDelete = function SparkleXrm_Sdk_OrganizationServiceProxy$endDelete(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        return;
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy.update = function SparkleXrm_Sdk_OrganizationServiceProxy$update(entity) {
    var xml = '<Update xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entity>';
    xml += entity.serialise(true);
    xml += '</entity></Update>';
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(xml, 'Update', null);
    delete resultXml;
    resultXml = null;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginUpdate = function SparkleXrm_Sdk_OrganizationServiceProxy$beginUpdate(entity, callBack) {
    var xml = '<Update xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entity>';
    xml += entity.serialise(true);
    xml += '</entity></Update>';
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(xml, 'Update', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endUpdate = function SparkleXrm_Sdk_OrganizationServiceProxy$endUpdate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        return;
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy.execute = function SparkleXrm_Sdk_OrganizationServiceProxy$execute(request) {
    var resultXml = SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getExecuteRequest(request), 'Execute', null);
    return SparkleXrm.Sdk.OrganizationServiceProxy.endExecute(resultXml);
}
SparkleXrm.Sdk.OrganizationServiceProxy._getExecuteRequest = function SparkleXrm_Sdk_OrganizationServiceProxy$_getExecuteRequest(request) {
    var xml = '<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services">';
    xml += request.serialise();
    xml += '</Execute>';
    return xml;
}
SparkleXrm.Sdk.OrganizationServiceProxy.beginExecute = function SparkleXrm_Sdk_OrganizationServiceProxy$beginExecute(request, callBack) {
    SparkleXrm.Sdk.OrganizationServiceProxy._getResponse(SparkleXrm.Sdk.OrganizationServiceProxy._getExecuteRequest(request), 'Execute', callBack);
}
SparkleXrm.Sdk.OrganizationServiceProxy.endExecute = function SparkleXrm_Sdk_OrganizationServiceProxy$endExecute(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        var response = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(xmlDocument, 'ExecuteResult');
        var type = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(response, 'ResponseName');
        switch (type) {
            case 'RetrieveAttribute':
                return new SparkleXrm.Sdk.Messages.RetrieveAttributeResponse(response);
            case 'RetrieveAllEntities':
                return new SparkleXrm.Sdk.Messages.RetrieveAllEntitiesResponse(response);
            case 'RetrieveEntity':
                return new SparkleXrm.Sdk.Messages.RetrieveEntityResponse(response);
            case 'BulkDeleteResponse':
                return new SparkleXrm.Sdk.Messages.BulkDeleteResponse(response);
            case 'FetchXmlToQueryExpression':
                return new SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionResponse(response);
            case 'RetrieveMetadataChanges':
                return new SparkleXrm.Sdk.Messages.RetrieveMetadataChangesResponse(response);
            case 'RetrieveRelationship':
                return new SparkleXrm.Sdk.RetrieveRelationshipResponse(response);
            case 'ExecuteWorkflow':
                return new SparkleXrm.Sdk.Messages.ExecuteWorkflowResponse(response);
            case 'Assign':
                return new SparkleXrm.Sdk.Messages.AssignResponse(response);
            default:
                if (Object.keyExists(SparkleXrm.Sdk.OrganizationServiceProxy.executeMessageResponseTypes, type)) {
                    var responseType = SparkleXrm.Sdk.OrganizationServiceProxy.executeMessageResponseTypes[type];
                    var exectueResponse = new responseType(response);
                    return exectueResponse;
                }
                else {
                    return null;
                }
        }
    }
    else {
        throw asyncState;
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getSoapEnvelope = function SparkleXrm_Sdk_OrganizationServiceProxy$_getSoapEnvelope(payLoadXml) {
    var xml = '<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" xmlns:d="http://schemas.microsoft.com/xrm/2011/Contracts/Services"  xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<s:Body>' + payLoadXml + '</s:Body>' + '</s:Envelope>';
    return xml;
}
SparkleXrm.Sdk.OrganizationServiceProxy._getServerUrl = function SparkleXrm_Sdk_OrganizationServiceProxy$_getServerUrl() {
    if (typeof(Xrm.Page.context.getClientUrl) === 'undefined') {
        var context = Xrm.Page.context;
        var crmServerUrl;
        if (context.isOutlookClient() && !context.isOutlookOnline()) {
            crmServerUrl = window.location.protocol + '//' + window.location.hostname;
        }
        else {
            crmServerUrl = Xrm.Page.context.getServerUrl();
            crmServerUrl = crmServerUrl.replace(new RegExp('/^(http|https):\\/\\/([_a-zA-Z0-9\\-\\.]+)(:([0-9]{1,5}))?/'), window.location.protocol + '//' + window.location.hostname);
            crmServerUrl = crmServerUrl.replace(new RegExp('/\\/$/'), '');
        }
        return crmServerUrl;
    }
    else {
        return Xrm.Page.context.getClientUrl();
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getResponse = function SparkleXrm_Sdk_OrganizationServiceProxy$_getResponse(soapXmlPacket, action, asyncCallback) {
    var isAsync = (asyncCallback != null);
    var xml = SparkleXrm.Sdk.OrganizationServiceProxy._getSoapEnvelope(soapXmlPacket);
    var msg = null;
    var xmlHttpRequest = new XMLHttpRequest();
    xmlHttpRequest.open('POST', SparkleXrm.Sdk.OrganizationServiceProxy._getServerUrl() + '/XRMServices/2011/Organization.svc/web', isAsync);
    xmlHttpRequest.setRequestHeader('SOAPAction', 'http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/' + action);
    xmlHttpRequest.setRequestHeader('Content-Type', 'text/xml; charset=utf-8');
    if (SparkleXrm.Sdk.OrganizationServiceProxy.withCredentials) {
        xmlHttpRequest.withCredentials = true;;
    }
    if (isAsync) {
        xmlHttpRequest.onreadystatechange = function() {
            if (xmlHttpRequest == null) {
                return;
            }
            if (xmlHttpRequest.readyState === 4) {
                var resultXml = xmlHttpRequest.responseXML;
                var errorMsg = null;
                if (xmlHttpRequest.status !== 200) {
                    errorMsg = SparkleXrm.Sdk.OrganizationServiceProxy._getSoapFault(resultXml);
                }
                delete xmlHttpRequest;
                xmlHttpRequest = null;
                if (errorMsg != null) {
                    asyncCallback(errorMsg);
                }
                else {
                    asyncCallback(resultXml);
                }
            }
        };
        xmlHttpRequest.send(xml);
        return null;
    }
    else {
        xmlHttpRequest.send(xml);
        var resultXml = xmlHttpRequest.responseXML;
        if (xmlHttpRequest.status !== 200) {
            msg = SparkleXrm.Sdk.OrganizationServiceProxy._getSoapFault(resultXml);
        }
        delete xmlHttpRequest;;
        xmlHttpRequest = null;
        if (msg != null) {
            throw msg;
        }
        else {
            return resultXml;
        }
    }
}
SparkleXrm.Sdk.OrganizationServiceProxy._getSoapFault = function SparkleXrm_Sdk_OrganizationServiceProxy$_getSoapFault(response) {
    var errorMsg = null;
    var traceDetails = null;
    var errorCode = null;
    if (response == null || response.firstChild.nodeName !== 's:Envelope') {
        return new Error('No SOAP Envelope in response');
    }
    var soapResponseBody = response.firstChild.firstChild;
    var errorNode = SparkleXrm.Sdk.XmlHelper.selectSingleNode(soapResponseBody, 'Fault');
    if (errorNode != null) {
        var details = SparkleXrm.Sdk.XmlHelper.selectSingleNode(errorNode, 'detail');
        if (details != null) {
            var serviceFaultNode = SparkleXrm.Sdk.XmlHelper.selectSingleNode(details, 'OrganizationServiceFault');
            if (serviceFaultNode != null) {
                errorMsg = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(serviceFaultNode, 'Message');
                traceDetails = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(serviceFaultNode, 'TraceText');
                errorCode = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(serviceFaultNode, 'ErrorCode');
            }
        }
        if (errorMsg == null) {
            var faultMessage = SparkleXrm.Sdk.XmlHelper.selectSingleNode(errorNode, 'faultstring');
            if (faultMessage != null) {
                errorMsg = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(faultMessage);
            }
        }
    }
    var info = {};
    info['Trace'] = traceDetails;
    info['ErrorCode'] = errorCode;
    return Error.createError(errorMsg, info);
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Relationship

SparkleXrm.Sdk.Relationship = function SparkleXrm_Sdk_Relationship(schemaName) {
    this.schemaName = schemaName;
}
SparkleXrm.Sdk.Relationship.prototype = {
    primaryEntityRole: 0,
    schemaName: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.RetrieveRelationshipRequest

SparkleXrm.Sdk.RetrieveRelationshipRequest = function SparkleXrm_Sdk_RetrieveRelationshipRequest() {
    this.metadataId = SparkleXrm.Sdk.Guid.empty;
}
SparkleXrm.Sdk.RetrieveRelationshipRequest.prototype = {
    name: null,
    retrieveAsIfPublished: false,
    
    serialise: function SparkleXrm_Sdk_RetrieveRelationshipRequest$serialise() {
        return '<request i:type="a:RetrieveRelationshipRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">' + '<a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<b:key>MetadataId</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.metadataId, null) + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>Name</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.name, null) + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>RetrieveAsIfPublished</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.retrieveAsIfPublished, null) + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>RetrieveRelationship</a:RequestName>' + '</request>';
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.RetrieveRelationshipResponse

SparkleXrm.Sdk.RetrieveRelationshipResponse = function SparkleXrm_Sdk_RetrieveRelationshipResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (SparkleXrm.Sdk.XmlHelper.getNodeTextValue(key) === 'RelationshipMetadata') {
            var entity = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.relationshipMetadata = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseRelationshipMetadata(entity);
        }
    }
}
SparkleXrm.Sdk.RetrieveRelationshipResponse.prototype = {
    relationshipMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.XmlHelper

SparkleXrm.Sdk.XmlHelper = function SparkleXrm_Sdk_XmlHelper() {
}
SparkleXrm.Sdk.XmlHelper.encode = function SparkleXrm_Sdk_XmlHelper$encode(value) {
    if (value == null) {
        return value;
    }
    return value.replace(new RegExp("([\\&\"<>'])", 'g'), SparkleXrm.Sdk.XmlHelper.replaceCallBackEncode);
}
SparkleXrm.Sdk.XmlHelper.serialiseNode = function SparkleXrm_Sdk_XmlHelper$serialiseNode(node) {
    if (typeof(node.xml) === 'undefined') {
        return new XMLSerializer().serializeToString(node);
    }
    else {
        return node.xml;
    }
}
SparkleXrm.Sdk.XmlHelper.Decode = function SparkleXrm_Sdk_XmlHelper$Decode(value) {
    if (value == null) {
        return null;
    }
    return value.replace(new RegExp('(&quot;|&lt;|&gt;|&amp;|&#39;)', 'g'), SparkleXrm.Sdk.XmlHelper.replaceCallBackDecode);
}
SparkleXrm.Sdk.XmlHelper.replaceCallBackEncode = function SparkleXrm_Sdk_XmlHelper$replaceCallBackEncode(item) {
    return SparkleXrm.Sdk.XmlHelper._encode_map[item];
}
SparkleXrm.Sdk.XmlHelper.replaceCallBackDecode = function SparkleXrm_Sdk_XmlHelper$replaceCallBackDecode(item) {
    return SparkleXrm.Sdk.XmlHelper._decode_map[item];
}
SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue = function SparkleXrm_Sdk_XmlHelper$selectSingleNodeValue(doc, baseName) {
    var node = SparkleXrm.Sdk.XmlHelper.selectSingleNode(doc, baseName);
    if (node != null) {
        return SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
    }
    else {
        return null;
    }
}
SparkleXrm.Sdk.XmlHelper.selectSingleNode = function SparkleXrm_Sdk_XmlHelper$selectSingleNode(doc, baseName) {
    var $enum1 = ss.IEnumerator.getEnumerator(doc.childNodes);
    while ($enum1.moveNext()) {
        var n = $enum1.current;
        if (SparkleXrm.Sdk.XmlHelper.getLocalName(n) === baseName) {
            return n;
        }
    }
    return null;
}
SparkleXrm.Sdk.XmlHelper.getLocalName = function SparkleXrm_Sdk_XmlHelper$getLocalName(node) {
    if (node.baseName != null) {
        return node.baseName;
    }
    else {
        return node.localName;
    }
}
SparkleXrm.Sdk.XmlHelper.selectSingleNodeValueDeep = function SparkleXrm_Sdk_XmlHelper$selectSingleNodeValueDeep(doc, baseName) {
    var node = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(doc, baseName);
    if (node != null) {
        return SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
    }
    else {
        return null;
    }
}
SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep = function SparkleXrm_Sdk_XmlHelper$selectSingleNodeDeep(doc, baseName) {
    var $enum1 = ss.IEnumerator.getEnumerator(doc.childNodes);
    while ($enum1.moveNext()) {
        var n = $enum1.current;
        if (SparkleXrm.Sdk.XmlHelper.getLocalName(n) === baseName) {
            return n;
        }
        var resultDeep = SparkleXrm.Sdk.XmlHelper.selectSingleNodeDeep(n, baseName);
        if (resultDeep != null) {
            return resultDeep;
        }
    }
    return null;
}
SparkleXrm.Sdk.XmlHelper.nsResolver = function SparkleXrm_Sdk_XmlHelper$nsResolver(prefix) {
    switch (prefix) {
        case 's':
            return 'http://schemas.xmlsoap.org/soap/envelope/';
        case 'a':
            return 'http://schemas.microsoft.com/xrm/2011/Contracts';
        case 'i':
            return 'http://www.w3.org/2001/XMLSchema-instance';
        case 'b':
            return 'http://schemas.datacontract.org/2004/07/System.Collections.Generic';
        case 'c':
            return 'http://schemas.microsoft.com/xrm/2011/Metadata';
        default:
            return null;
    }
}
SparkleXrm.Sdk.XmlHelper.isSelectSingleNodeUndefined = function SparkleXrm_Sdk_XmlHelper$isSelectSingleNodeUndefined(value) {
    return typeof (value.selectSingleNode) === 'undefined';
}
SparkleXrm.Sdk.XmlHelper.loadXml = function SparkleXrm_Sdk_XmlHelper$loadXml(xml) {
    if (typeof (ActiveXObject) === 'undefined') {
        var domParser = new DOMParser();
        return domParser.parseFromString(xml, 'text/xml');
    }
    else {
        var xmlDOM = (new ActiveXObject('Msxml2.DOMDocument'));
        xmlDOM.async = false;
        xmlDOM.loadXML(xml);
        xmlDOM.setProperty('SelectionLanguage', 'XPath');
        return xmlDOM;
    }
}
SparkleXrm.Sdk.XmlHelper.selectSingleNodeXpath = function SparkleXrm_Sdk_XmlHelper$selectSingleNodeXpath(node, xpath) {
    if (!SparkleXrm.Sdk.XmlHelper.isSelectSingleNodeUndefined(node)) {
        return node.selectSingleNode(xpath);
    }
    else {
        var xpe = new XPathEvaluator();
        var xPathNode = xpe.evaluate(xpath, node, SparkleXrm.Sdk.XmlHelper.nsResolver, 9, null);
        return (xPathNode != null) ? xPathNode.singleNodeValue : null;
    }
}
SparkleXrm.Sdk.XmlHelper.getNodeTextValue = function SparkleXrm_Sdk_XmlHelper$getNodeTextValue(node) {
    if ((node != null) && (node.firstChild != null)) {
        return node.firstChild.nodeValue;
    }
    else {
        return null;
    }
}
SparkleXrm.Sdk.XmlHelper.getAttributeValue = function SparkleXrm_Sdk_XmlHelper$getAttributeValue(node, attributeName) {
    var attribute = node.attributes.getNamedItem(attributeName);
    if (attribute != null) {
        return attribute.nodeValue;
    }
    else {
        return null;
    }
}


Type.registerNamespace('SparkleXrm.Sdk.Messages');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.EntityFilters

SparkleXrm.Sdk.Messages.EntityFilters = function() { };
SparkleXrm.Sdk.Messages.EntityFilters.prototype = {
    default_: 1, 
    entity: 1, 
    attributes: 2, 
    privileges: 4, 
    relationships: 8, 
    all: 15
}
SparkleXrm.Sdk.Messages.EntityFilters.registerEnum('SparkleXrm.Sdk.Messages.EntityFilters', true);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.AssignRequest

SparkleXrm.Sdk.Messages.AssignRequest = function SparkleXrm_Sdk_Messages_AssignRequest() {
}
SparkleXrm.Sdk.Messages.AssignRequest.prototype = {
    target: null,
    assignee: null,
    
    serialise: function SparkleXrm_Sdk_Messages_AssignRequest$serialise() {
        return '<request i:type="c:AssignRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:c="http://schemas.microsoft.com/crm/2011/Contracts">' + '        <a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '          <a:KeyValuePairOfstringanyType>' + '            <b:key>Target</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.target, null) + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <b:key>Assignee</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.assignee, null) + '          </a:KeyValuePairOfstringanyType>' + '        </a:Parameters>' + '        <a:RequestId i:nil="true" />' + '        <a:RequestName>Assign</a:RequestName>' + '      </request>';
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.AssignResponse

SparkleXrm.Sdk.Messages.AssignResponse = function SparkleXrm_Sdk_Messages_AssignResponse(response) {
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.BulkDeleteRequest

SparkleXrm.Sdk.Messages.BulkDeleteRequest = function SparkleXrm_Sdk_Messages_BulkDeleteRequest() {
}
SparkleXrm.Sdk.Messages.BulkDeleteRequest.prototype = {
    
    serialise: function SparkleXrm_Sdk_Messages_BulkDeleteRequest$serialise() {
        var recipientsXml = '';
        if (this.toRecipients != null) {
            var $enum1 = ss.IEnumerator.getEnumerator(this.toRecipients);
            while ($enum1.moveNext()) {
                var id = $enum1.current;
                recipientsXml += ('<d:guid>' + id.toString() + '</d:guid>');
            }
        }
        var ccRecipientsXml = '';
        if (this.ccRecipients != null) {
            var $enum2 = ss.IEnumerator.getEnumerator(this.ccRecipients);
            while ($enum2.moveNext()) {
                var id = $enum2.current;
                ccRecipientsXml += ('<d:guid>' + id.toString() + '</d:guid>');
            }
        }
        return String.format('<request i:type="b:BulkDeleteRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '        <a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>QuerySet</c:key>' + '            <c:value i:type="a:ArrayOfQueryExpression">' + '              <a:QueryExpression>' + this.querySet + '              </a:QueryExpression>' + '            </c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>JobName</c:key>' + '            <c:value i:type="d:string" xmlns:d="http://www.w3.org/2001/XMLSchema">' + this.jobName + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>SendEmailNotification</c:key>' + '            <c:value i:type="d:boolean" xmlns:d="http://www.w3.org/2001/XMLSchema">' + this.sendEmailNotification.toString() + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>ToRecipients</c:key>' + '            <c:value i:type="d:ArrayOfguid" xmlns:d="http://schemas.microsoft.com/2003/10/Serialization/Arrays">' + recipientsXml + '            </c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>CCRecipients</c:key>' + '            <c:value i:type="d:ArrayOfguid" xmlns:d="http://schemas.microsoft.com/2003/10/Serialization/Arrays">' + ccRecipientsXml + '            </c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>RecurrencePattern</c:key>' + '            <c:value i:type="d:string" xmlns:d="http://www.w3.org/2001/XMLSchema" >' + this.recurrencePattern + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>StartDateTime</c:key>' + '            <c:value i:type="d:dateTime" xmlns:d="http://www.w3.org/2001/XMLSchema">' + SparkleXrm.Sdk.DateTimeEx.toXrmStringUTC(SparkleXrm.Sdk.DateTimeEx.localTimeToUTCFromSettings(this.startDateTime, SparkleXrm.Sdk.OrganizationServiceProxy.getUserSettings())) + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '        </a:Parameters>' + '        <a:RequestId i:nil="true" />' + '        <a:RequestName>BulkDelete</a:RequestName>' + '      </request>');
    },
    
    ccRecipients: null,
    jobName: null,
    querySet: null,
    recurrencePattern: null,
    sendEmailNotification: false,
    sourceImportId: null,
    startDateTime: null,
    toRecipients: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.BulkDeleteResponse

SparkleXrm.Sdk.Messages.BulkDeleteResponse = function SparkleXrm_Sdk_Messages_BulkDeleteResponse(response) {
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.ExecuteWorkflowRequest

SparkleXrm.Sdk.Messages.ExecuteWorkflowRequest = function SparkleXrm_Sdk_Messages_ExecuteWorkflowRequest() {
}
SparkleXrm.Sdk.Messages.ExecuteWorkflowRequest.prototype = {
    entityId: null,
    workflowId: null,
    
    serialise: function SparkleXrm_Sdk_Messages_ExecuteWorkflowRequest$serialise() {
        return String.format('<request i:type="b:ExecuteWorkflowRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '        <a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>EntityId</c:key>' + '            <c:value i:type="e:guid" xmlns:e="http://schemas.microsoft.com/2003/10/Serialization/">' + this.entityId + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>WorkflowId</c:key>' + '            <c:value i:type="e:guid" xmlns:e="http://schemas.microsoft.com/2003/10/Serialization/">' + this.workflowId + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '        </a:Parameters>' + '        <a:RequestId i:nil="true" />' + '        <a:RequestName>ExecuteWorkflow</a:RequestName>' + '      </request>');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.ExecuteWorkflowResponse

SparkleXrm.Sdk.Messages.ExecuteWorkflowResponse = function SparkleXrm_Sdk_Messages_ExecuteWorkflowResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (SparkleXrm.Sdk.XmlHelper.getNodeTextValue(key) === 'Id') {
            var value = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.id = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(value);
        }
    }
}
SparkleXrm.Sdk.Messages.ExecuteWorkflowResponse.prototype = {
    id: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionRequest

SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionRequest = function SparkleXrm_Sdk_Messages_FetchXmlToQueryExpressionRequest() {
}
SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionRequest.prototype = {
    fetchXml: null,
    
    serialise: function SparkleXrm_Sdk_Messages_FetchXmlToQueryExpressionRequest$serialise() {
        var requestXml = '';
        requestXml += '      <request i:type="b:FetchXmlToQueryExpressionRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">';
        requestXml += '        <a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">';
        requestXml += '          <a:KeyValuePairOfstringanyType>';
        requestXml += '            <c:key>FetchXml</c:key>';
        requestXml += '            <c:value i:type="d:string" xmlns:d="http://www.w3.org/2001/XMLSchema">{0}</c:value>';
        requestXml += '          </a:KeyValuePairOfstringanyType>';
        requestXml += '        </a:Parameters>';
        requestXml += '        <a:RequestId i:nil="true" />';
        requestXml += '        <a:RequestName>FetchXmlToQueryExpression</a:RequestName>';
        requestXml += '      </request>';
        return String.format(requestXml, SparkleXrm.Sdk.XmlHelper.encode(this.fetchXml));
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionResponse

SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionResponse = function SparkleXrm_Sdk_Messages_FetchXmlToQueryExpressionResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (SparkleXrm.Sdk.XmlHelper.getNodeTextValue(key) === 'Query') {
            var queryNode = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            var queryXml = SparkleXrm.Sdk.XmlHelper.serialiseNode(queryNode).substr(165);
            queryXml = queryXml.substr(0, queryXml.length - 10);
            this.query = queryXml;
        }
    }
}
SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionResponse.prototype = {
    query: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveAllEntitiesRequest

SparkleXrm.Sdk.Messages.RetrieveAllEntitiesRequest = function SparkleXrm_Sdk_Messages_RetrieveAllEntitiesRequest() {
}
SparkleXrm.Sdk.Messages.RetrieveAllEntitiesRequest.prototype = {
    
    serialise: function SparkleXrm_Sdk_Messages_RetrieveAllEntitiesRequest$serialise() {
        return '\r\n                              <request i:type="a:RetrieveAllEntitiesRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">\r\n                                <a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">\r\n                                  <a:KeyValuePairOfstringanyType>\r\n                                    <b:key>EntityFilters</b:key>\r\n                                    <b:value i:type="c:EntityFilters" xmlns:c="http://schemas.microsoft.com/xrm/2011/Metadata">Entity</b:value>\r\n                                  </a:KeyValuePairOfstringanyType>\r\n                                  <a:KeyValuePairOfstringanyType>\r\n                                    <b:key>RetrieveAsIfPublished</b:key>\r\n                                    <b:value i:type="c:boolean" xmlns:c="http://www.w3.org/2001/XMLSchema">true</b:value>\r\n                                  </a:KeyValuePairOfstringanyType>\r\n                                </a:Parameters>\r\n                                <a:RequestId i:nil="true" />\r\n                                <a:RequestName>RetrieveAllEntities</a:RequestName>\r\n                              </request>\r\n                            ';
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveAllEntitiesResponse

SparkleXrm.Sdk.Messages.RetrieveAllEntitiesResponse = function SparkleXrm_Sdk_Messages_RetrieveAllEntitiesResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (SparkleXrm.Sdk.XmlHelper.getNodeTextValue(key) === 'EntityMetadata') {
            var values = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.entityMetadata = new Array(values.childNodes.length);
            for (var i = 0; i < values.childNodes.length; i++) {
                var entity = values.childNodes[i];
                var metaData = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseEntityMetadata({}, entity);
                this.entityMetadata[i] = metaData;
            }
        }
    }
}
SparkleXrm.Sdk.Messages.RetrieveAllEntitiesResponse.prototype = {
    entityMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveAttributeRequest

SparkleXrm.Sdk.Messages.RetrieveAttributeRequest = function SparkleXrm_Sdk_Messages_RetrieveAttributeRequest() {
}
SparkleXrm.Sdk.Messages.RetrieveAttributeRequest.prototype = {
    entityLogicalName: null,
    logicalName: null,
    retrieveAsIfPublished: false,
    
    serialise: function SparkleXrm_Sdk_Messages_RetrieveAttributeRequest$serialise() {
        return String.format('<request i:type="a:RetrieveAttributeRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">' + '<a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<b:key>EntityLogicalName</b:key>' + '<b:value i:type="c:string" xmlns:c="http://www.w3.org/2001/XMLSchema">{0}</b:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>MetadataId</b:key>' + '<b:value i:type="ser:guid"  xmlns:ser="http://schemas.microsoft.com/2003/10/Serialization/">00000000-0000-0000-0000-000000000000</b:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>RetrieveAsIfPublished</b:key>' + '<b:value i:type="c:boolean" xmlns:c="http://www.w3.org/2001/XMLSchema">{2}</b:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>LogicalName</b:key>' + '<b:value i:type="c:string" xmlns:c="http://www.w3.org/2001/XMLSchema">{1}</b:value>' + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>RetrieveAttribute</a:RequestName>' + '</request>', this.entityLogicalName, this.logicalName, this.retrieveAsIfPublished);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveAttributeResponse

SparkleXrm.Sdk.Messages.RetrieveAttributeResponse = function SparkleXrm_Sdk_Messages_RetrieveAttributeResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var metaData = SparkleXrm.Sdk.XmlHelper.selectSingleNode(results.firstChild, 'value');
    var type = SparkleXrm.Sdk.XmlHelper.getAttributeValue(metaData, 'i:type');
    switch (type) {
        case 'c:PicklistAttributeMetadata':
            this.attributeMetadata = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialisePicklistAttributeMetadata({}, metaData);
            break;
    }
}
SparkleXrm.Sdk.Messages.RetrieveAttributeResponse.prototype = {
    attributeMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveEntityRequest

SparkleXrm.Sdk.Messages.RetrieveEntityRequest = function SparkleXrm_Sdk_Messages_RetrieveEntityRequest() {
}
SparkleXrm.Sdk.Messages.RetrieveEntityRequest.prototype = {
    entityFilters: 0,
    logicalName: null,
    metadataId: null,
    retrieveAsIfPublished: false,
    
    serialise: function SparkleXrm_Sdk_Messages_RetrieveEntityRequest$serialise() {
        return '<request i:type="a:RetrieveEntityRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">' + '<a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<b:key>EntityFilters</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.entityFilters, 'EntityFilters') + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>MetadataId</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.metadataId, null) + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>RetrieveAsIfPublished</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.retrieveAsIfPublished, null) + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>LogicalName</b:key>' + SparkleXrm.Sdk.Attribute.serialiseValue(this.logicalName, null) + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>RetrieveEntity</a:RequestName>' + '</request>';
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveEntityResponse

SparkleXrm.Sdk.Messages.RetrieveEntityResponse = function SparkleXrm_Sdk_Messages_RetrieveEntityResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (SparkleXrm.Sdk.XmlHelper.getNodeTextValue(key) === 'EntityMetadata') {
            var entity = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.entityMetadata = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseEntityMetadata({}, entity);
        }
    }
}
SparkleXrm.Sdk.Messages.RetrieveEntityResponse.prototype = {
    entityMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest

SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest = function SparkleXrm_Sdk_Messages_RetrieveMetadataChangesRequest() {
}
SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest.prototype = {
    clientVersionStamp: null,
    deletedMetadataFilters: null,
    query: null,
    
    serialise: function SparkleXrm_Sdk_Messages_RetrieveMetadataChangesRequest$serialise() {
        return "<request i:type='a:RetrieveMetadataChangesRequest' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts'>\r\n                <a:Parameters xmlns:b='http://schemas.datacontract.org/2004/07/System.Collections.Generic'>\r\n                  <a:KeyValuePairOfstringanyType>\r\n                    <b:key>ClientVersionStamp</b:key>" + SparkleXrm.Sdk.Attribute.serialiseValue(this.clientVersionStamp, null) + '\r\n                  </a:KeyValuePairOfstringanyType>\r\n                  <a:KeyValuePairOfstringanyType>\r\n                    <b:key>Query</b:key>\r\n                    ' + SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseEntityQueryExpression(this.query) + "\r\n                  </a:KeyValuePairOfstringanyType>\r\n                </a:Parameters>\r\n                <a:RequestId i:nil='true' />\r\n                <a:RequestName>RetrieveMetadataChanges</a:RequestName>\r\n              </request>";
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Messages.RetrieveMetadataChangesResponse

SparkleXrm.Sdk.Messages.RetrieveMetadataChangesResponse = function SparkleXrm_Sdk_Messages_RetrieveMetadataChangesResponse(response) {
    var results = SparkleXrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        var value = SparkleXrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
        switch (SparkleXrm.Sdk.XmlHelper.getNodeTextValue(key)) {
            case 'ServerVersionStamp':
                this.serverVersionStamp = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(value);
                break;
            case 'DeletedMetadata':
                break;
            case 'EntityMetadata':
                this.entityMetadata = [];
                for (var i = 0; i < value.childNodes.length; i++) {
                    var entity = value.childNodes[i];
                    var metaData = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseEntityMetadata({}, entity);
                    this.entityMetadata.add(metaData);
                }
                break;
        }
    }
}
SparkleXrm.Sdk.Messages.RetrieveMetadataChangesResponse.prototype = {
    entityMetadata: null,
    serverVersionStamp: null
}


Type.registerNamespace('SparkleXrm.Sdk.Metadata.Query');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.Query.DeletedMetadataFilters

SparkleXrm.Sdk.Metadata.Query.DeletedMetadataFilters = function() { };
SparkleXrm.Sdk.Metadata.Query.DeletedMetadataFilters.prototype = {
    default_: 'default_', 
    entity: 'entity', 
    attribute: 'attribute', 
    relationship: 'relationship', 
    label: 'label', 
    optionSet: 'optionSet', 
    all: 'all'
}
SparkleXrm.Sdk.Metadata.Query.DeletedMetadataFilters.registerEnum('SparkleXrm.Sdk.Metadata.Query.DeletedMetadataFilters', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser

SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser() {
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseAttributeMetadata = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseAttributeMetadata(item, attribute) {
    var $enum1 = ss.IEnumerator.getEnumerator(attribute.childNodes);
    while ($enum1.moveNext()) {
        var node = $enum1.current;
        var itemValues = item;
        var localName = SparkleXrm.Sdk.XmlHelper.getLocalName(node);
        var fieldName = localName.substr(0, 1).toLowerCase() + localName.substr(1);
        if (node.attributes.length === 1 && node.attributes[0].nodeName === 'i:nil') {
            continue;
        }
        switch (localName) {
            case 'AttributeOf':
            case 'DeprecatedVersion':
            case 'EntityLogicalName':
            case 'LogicalName':
            case 'SchemaName':
            case 'CalculationOf':
                itemValues[fieldName] = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
                break;
            case 'CanBeSecuredForCreate':
            case 'CanBeSecuredForRead':
            case 'CanBeSecuredForUpdate':
            case 'CanModifyAdditionalSettings':
            case 'IsAuditEnabled':
            case 'IsCustomAttribute':
            case 'IsCustomizable':
            case 'IsManaged':
            case 'IsPrimaryId':
            case 'IsPrimaryName':
            case 'IsRenameable':
            case 'IsSecured':
            case 'IsValidForAdvancedFind':
            case 'IsValidForCreate':
            case 'IsValidForRead':
            case 'IsValidForUpdate':
            case 'DefaultValue':
                itemValues[fieldName] = SparkleXrm.Sdk.Attribute.deSerialise(node, 'boolean');
                break;
            case 'ColumnNumber':
            case 'Precision':
            case 'DefaultFormValue':
            case 'MaxLength':
            case 'PrecisionSource':
                itemValues[fieldName] = SparkleXrm.Sdk.Attribute.deSerialise(node, 'int');
                break;
            case 'Description':
            case 'DisplayName':
                var label = {};
                itemValues[fieldName] = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLabel(label, node);
                break;
            case 'OptionSet':
                var options = {};
                itemValues[fieldName] = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseOptionSetMetadata(options, node);
                break;
            case 'AttributeType':
                item.attributeType = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
                break;
        }
    }
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseEntityMetadata = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseEntityMetadata(item, entity) {
    var $enum1 = ss.IEnumerator.getEnumerator(entity.childNodes);
    while ($enum1.moveNext()) {
        var node = $enum1.current;
        var itemValues = item;
        var localName = SparkleXrm.Sdk.XmlHelper.getLocalName(node);
        var fieldName = localName.substr(0, 1).toLowerCase() + localName.substr(1);
        if (node.attributes.length === 1 && node.attributes[0].nodeName === 'i:nil') {
            continue;
        }
        switch (localName) {
            case 'IconLargeName':
            case 'IconMediumName':
            case 'IconSmallName':
            case 'LogicalName':
            case 'PrimaryIdAttribute':
            case 'PrimaryNameAttribute':
            case 'RecurrenceBaseEntityLogicalName':
            case 'ReportViewName':
            case 'SchemaName':
            case 'PrimaryImageAttribute':
                itemValues[fieldName] = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
                break;
            case 'AutoRouteToOwnerQueue':
            case 'CanBeInManyToMany':
            case 'CanBePrimaryEntityInRelationship':
            case 'CanBeRelatedEntityInRelationship':
            case 'CanCreateAttributes':
            case 'CanCreateCharts':
            case 'CanCreateForms':
            case 'CanCreateViews':
            case 'CanModifyAdditionalSettings':
            case 'CanTriggerWorkflow':
            case 'IsActivity':
            case 'IsActivityParty':
            case 'IsAuditEnabled':
            case 'IsAvailableOffline':
            case 'IsChildEntity':
            case 'IsConnectionsEnabled':
            case 'IsCustomEntity':
            case 'IsCustomizable':
            case 'IsDocumentManagementEnabled':
            case 'IsDuplicateDetectionEnabled':
            case 'IsEnabledForCharts':
            case 'IsImportable':
            case 'IsIntersect':
            case 'IsMailMergeEnabled':
            case 'IsManaged':
            case 'IsReadingPaneEnabled':
            case 'IsRenameable':
            case 'IsValidForAdvancedFind':
            case 'IsValidForQueue':
            case 'IsVisibleInMobile':
                itemValues[fieldName] = SparkleXrm.Sdk.Attribute.deSerialise(node, 'boolean');
                break;
            case 'ActivityTypeMask':
            case 'ObjectTypeCode':
                itemValues[fieldName] = SparkleXrm.Sdk.Attribute.deSerialise(node, 'int');
                break;
            case 'Attributes':
                item.attributes = [];
                var $enum2 = ss.IEnumerator.getEnumerator(node.childNodes);
                while ($enum2.moveNext()) {
                    var childNode = $enum2.current;
                    var a = {};
                    item.attributes.add(SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseAttributeMetadata(a, childNode));
                }
                break;
            case 'Description':
            case 'DisplayCollectionName':
            case 'DisplayName':
                var label = {};
                itemValues[fieldName] = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLabel(label, node);
                break;
        }
    }
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLabel = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseLabel(item, metaData) {
    item.localizedLabels = [];
    var labels = SparkleXrm.Sdk.XmlHelper.selectSingleNode(metaData, 'LocalizedLabels');
    if (labels != null && labels.childNodes != null) {
        var $enum1 = ss.IEnumerator.getEnumerator(labels.childNodes);
        while ($enum1.moveNext()) {
            var label = $enum1.current;
            item.localizedLabels.add(SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLocalizedLabel({}, label));
        }
        item.userLocalizedLabel = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLocalizedLabel({}, SparkleXrm.Sdk.XmlHelper.selectSingleNode(metaData, 'UserLocalizedLabel'));
    }
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLocalizedLabel = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseLocalizedLabel(item, metaData) {
    item.label = SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(metaData, 'Label');
    item.languageCode = parseInt(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(metaData, 'LanguageCode'));
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseAttributeQueryExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseAttributeQueryExpression(item) {
    return SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataQueryExpression(item);
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseEntityQueryExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseEntityQueryExpression(item) {
    if (item != null) {
        var xml = "<b:value i:type='c:EntityQueryExpression' xmlns:c='http://schemas.microsoft.com/xrm/2011/Metadata/Query'>" + SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataQueryExpression(item);
        if (item.attributeQuery != null) {
            xml += '<c:AttributeQuery>' + SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseAttributeQueryExpression(item.attributeQuery) + '</c:AttributeQuery>';
        }
        xml += '<c:LabelQuery>' + SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseLabelQueryExpression(item.labelQuery) + "</c:LabelQuery>\r\n                <c:RelationshipQuery i:nil='true' />\r\n                </b:value>";
        return xml;
    }
    else {
        return "<b:value i:nil='true'/>";
    }
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseLabelQueryExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseLabelQueryExpression(item) {
    if (item != null) {
        var xml = "<c:FilterLanguages xmlns:d='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>";
        var $enum1 = ss.IEnumerator.getEnumerator(item.filterLanguages);
        while ($enum1.moveNext()) {
            var lcid = $enum1.current;
            xml = xml + '<d:int>' + lcid.toString() + '</d:int>';
        }
        xml = xml + '</c:FilterLanguages>';
        return xml;
    }
    else {
        return '';
    }
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataConditionExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataConditionExpression(item) {
    return '<c:MetadataConditionExpression>\r\n                            <c:ConditionOperator>' + item.conditionOperator + '</c:ConditionOperator>\r\n                            <c:PropertyName>' + item.propertyName + "</c:PropertyName>\r\n                            <c:Value i:type='d:string' xmlns:d='http://www.w3.org/2001/XMLSchema'>" + item.value + '</c:Value>\r\n                          </c:MetadataConditionExpression>';
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataFilterExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataFilterExpression(item) {
    if (item != null) {
        var xml = '<c:Conditions>';
        var $enum1 = ss.IEnumerator.getEnumerator(item.conditions);
        while ($enum1.moveNext()) {
            var ex = $enum1.current;
            xml += SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataConditionExpression(ex);
        }
        xml = xml + '</c:Conditions>\r\n                        <c:FilterOperator>' + item.filterOperator + '</c:FilterOperator>\r\n                        <c:Filters />';
        return xml;
    }
    return '';
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataPropertiesExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataPropertiesExpression(item) {
    if (item != null) {
        var xml = '\r\n                <c:AllProperties>' + ((item.allProperties != null) ? item.allProperties.toString().toLowerCase() : 'false') + "</c:AllProperties>\r\n                <c:PropertyNames xmlns:d='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>";
        if (item.propertyNames != null) {
            var $enum1 = ss.IEnumerator.getEnumerator(item.propertyNames);
            while ($enum1.moveNext()) {
                var value = $enum1.current;
                xml = xml + '<d:string>' + value + '</d:string>';
            }
        }
        xml = xml + '\r\n                </c:PropertyNames>\r\n              ';
        return xml;
    }
    return '';
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataQueryExpression = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataQueryExpression(item) {
    if (item != null) {
        var xml = '<c:Criteria>' + SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataFilterExpression(item.criteria) + '</c:Criteria>\r\n                    <c:Properties>' + SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataPropertiesExpression(item.properties) + ' </c:Properties>';
        return xml;
    }
    return '';
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseRelationshipMetadata = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseRelationshipMetadata(attribute) {
    var item;
    var type = SparkleXrm.Sdk.XmlHelper.getAttributeValue(attribute, 'i:type');
    switch (type) {
        case 'c:OneToManyRelationshipMetadata':
            item = {};
            break;
        case 'c:ManyToManyRelationshipMetadata':
            item = {};
            break;
        default:
            throw new Error('Unknown relationship type');
    }
    var $enum1 = ss.IEnumerator.getEnumerator(attribute.childNodes);
    while ($enum1.moveNext()) {
        var node = $enum1.current;
        var itemValues = item;
        var localName = SparkleXrm.Sdk.XmlHelper.getLocalName(node);
        var fieldName = localName.substr(0, 1).toLowerCase() + localName.substr(1);
        if (node.attributes.length === 1 && node.attributes[0].nodeName === 'i:nil') {
            continue;
        }
        switch (localName) {
            case 'SchemaName':
            case 'ReferencedAttribute':
            case 'ReferencedEntity':
            case 'ReferencingAttribute':
            case 'ReferencingEntity':
            case 'Entity1IntersectAttribute':
            case 'Entity1LogicalName':
            case 'Entity2IntersectAttribute':
            case 'Entity2LogicalName':
            case 'IntersectEntityName':
                itemValues[fieldName] = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
                break;
            case 'IsCustomRelationship':
            case 'IsManaged':
            case 'IsValidForAdvancedFind':
                itemValues[fieldName] = SparkleXrm.Sdk.Attribute.deSerialise(node, 'boolean');
                break;
            case 'RelationshipType':
                itemValues[fieldName] = SparkleXrm.Sdk.XmlHelper.getNodeTextValue(node);
                break;
        }
    }
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseOptionMetadata = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseOptionMetadata(item, metaData) {
    item.value = parseInt(SparkleXrm.Sdk.XmlHelper.selectSingleNodeValue(metaData, 'Value'));
    item.label = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseLabel({}, SparkleXrm.Sdk.XmlHelper.selectSingleNode(metaData, 'Label'));
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseOptionSetMetadata = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialiseOptionSetMetadata(item, metaData) {
    var options = SparkleXrm.Sdk.XmlHelper.selectSingleNode(metaData, 'Options');
    if (options != null) {
        item.options = [];
        var $enum1 = ss.IEnumerator.getEnumerator(options.childNodes);
        while ($enum1.moveNext()) {
            var option = $enum1.current;
            item.options.add(SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseOptionMetadata({}, option));
        }
    }
    return item;
}
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialisePicklistAttributeMetadata = function SparkleXrm_Sdk_Metadata_Query_MetadataSerialiser$deSerialisePicklistAttributeMetadata(item, metaData) {
    var options = SparkleXrm.Sdk.XmlHelper.selectSingleNode(metaData, 'OptionSet');
    if (options != null) {
        item.optionSet = SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.deSerialiseOptionSetMetadata({}, options);
    }
    return item;
}


Type.registerNamespace('SparkleXrm.Sdk.Metadata');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.AttributeRequiredLevel

SparkleXrm.Sdk.Metadata.AttributeRequiredLevel = function() { };
SparkleXrm.Sdk.Metadata.AttributeRequiredLevel.prototype = {
    None: 'None', 
    SystemRequired: 'SystemRequired', 
    ApplicationRequired: 'ApplicationRequired', 
    Recommended: 'Recommended'
}
SparkleXrm.Sdk.Metadata.AttributeRequiredLevel.registerEnum('SparkleXrm.Sdk.Metadata.AttributeRequiredLevel', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.AttributeTypeCode

SparkleXrm.Sdk.Metadata.AttributeTypeCode = function() { };
SparkleXrm.Sdk.Metadata.AttributeTypeCode.prototype = {
    Boolean: 'Boolean', 
    Customer: 'Customer', 
    DateTime: 'DateTime', 
    Decimal: 'Decimal', 
    Double: 'Double', 
    Integer: 'Integer', 
    Lookup: 'Lookup', 
    Memo: 'Memo', 
    None: 'None', 
    Owner: 'Owner', 
    PartyList: 'PartyList', 
    Picklist: 'Picklist', 
    State: 'State', 
    Status: 'Status', 
    String: 'String', 
    Uniqueidentifier: 'Uniqueidentifier', 
    CalendarRules: 'CalendarRules', 
    Virtual: 'Virtual', 
    BigInt: 'BigInt', 
    ManagedProperty: 'ManagedProperty', 
    EntityName: 'EntityName'
}
SparkleXrm.Sdk.Metadata.AttributeTypeCode.registerEnum('SparkleXrm.Sdk.Metadata.AttributeTypeCode', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.DateTimeFormat

SparkleXrm.Sdk.Metadata.DateTimeFormat = function() { };
SparkleXrm.Sdk.Metadata.DateTimeFormat.prototype = {
    DateOnly: 'DateOnly', 
    DateAndTime: 'DateAndTime'
}
SparkleXrm.Sdk.Metadata.DateTimeFormat.registerEnum('SparkleXrm.Sdk.Metadata.DateTimeFormat', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.IntegerFormat

SparkleXrm.Sdk.Metadata.IntegerFormat = function() { };
SparkleXrm.Sdk.Metadata.IntegerFormat.prototype = {
    None: 'None', 
    Duration: 'Duration', 
    TimeZone: 'TimeZone', 
    Language: 'Language', 
    Locale: 'Locale'
}
SparkleXrm.Sdk.Metadata.IntegerFormat.registerEnum('SparkleXrm.Sdk.Metadata.IntegerFormat', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.OptionSetType

SparkleXrm.Sdk.Metadata.OptionSetType = function() { };
SparkleXrm.Sdk.Metadata.OptionSetType.prototype = {
    Picklist: 'Picklist', 
    State: 'State', 
    Status: 'Status', 
    Boolean: 'Boolean'
}
SparkleXrm.Sdk.Metadata.OptionSetType.registerEnum('SparkleXrm.Sdk.Metadata.OptionSetType', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.RelationshipType

SparkleXrm.Sdk.Metadata.RelationshipType = function() { };
SparkleXrm.Sdk.Metadata.RelationshipType.prototype = {
    OneToManyRelationship: 'OneToManyRelationship', 
    Default: 'Default', 
    ManyToManyRelationship: 'ManyToManyRelationship'
}
SparkleXrm.Sdk.Metadata.RelationshipType.registerEnum('SparkleXrm.Sdk.Metadata.RelationshipType', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.StringFormat

SparkleXrm.Sdk.Metadata.StringFormat = function() { };
SparkleXrm.Sdk.Metadata.StringFormat.prototype = {
    Email: 'Email', 
    Text: 'Text', 
    TextArea: 'TextArea', 
    Url: 'Url', 
    TickerSymbol: 'TickerSymbol', 
    PhoneticGuide: 'PhoneticGuide', 
    VersionNumber: 'VersionNumber', 
    Phone: 'Phone'
}
SparkleXrm.Sdk.Metadata.StringFormat.registerEnum('SparkleXrm.Sdk.Metadata.StringFormat', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.MetadataCache

SparkleXrm.Sdk.Metadata.MetadataCache = function SparkleXrm_Sdk_Metadata_MetadataCache() {
}
SparkleXrm.Sdk.Metadata.MetadataCache.get_entityMetaData = function SparkleXrm_Sdk_Metadata_MetadataCache$get_entityMetaData() {
    return SparkleXrm.Sdk.Metadata.MetadataCache._entityMetaData;
}
SparkleXrm.Sdk.Metadata.MetadataCache.get_attributeMetaData = function SparkleXrm_Sdk_Metadata_MetadataCache$get_attributeMetaData() {
    return SparkleXrm.Sdk.Metadata.MetadataCache._attributeMetaData;
}
SparkleXrm.Sdk.Metadata.MetadataCache.get_optionsetMetaData = function SparkleXrm_Sdk_Metadata_MetadataCache$get_optionsetMetaData() {
    return SparkleXrm.Sdk.Metadata.MetadataCache._optionsCache;
}
SparkleXrm.Sdk.Metadata.MetadataCache.getOptionSetValues = function SparkleXrm_Sdk_Metadata_MetadataCache$getOptionSetValues(entityLogicalName, attributeLogicalName, allowEmpty) {
    if (allowEmpty == null) {
        allowEmpty = false;
    }
    var cacheKey = entityLogicalName + '.' + attributeLogicalName + '.' + allowEmpty.toString();
    if (Object.keyExists(SparkleXrm.Sdk.Metadata.MetadataCache._optionsCache, cacheKey)) {
        return SparkleXrm.Sdk.Metadata.MetadataCache._optionsCache[cacheKey];
    }
    else {
        var attribute = SparkleXrm.Sdk.Metadata.MetadataCache._loadAttributeMetadata(entityLogicalName, attributeLogicalName);
        var pickList = attribute;
        var opts = [];
        if (allowEmpty) {
            opts.add({});
        }
        var $enum1 = ss.IEnumerator.getEnumerator(pickList.optionSet.options);
        while ($enum1.moveNext()) {
            var o = $enum1.current;
            var a = {};
            a.name = o.label.userLocalizedLabel.label;
            a.value = o.value;
            opts.add(a);
        }
        SparkleXrm.Sdk.Metadata.MetadataCache._optionsCache[cacheKey] = opts;
        return opts;
    }
}
SparkleXrm.Sdk.Metadata.MetadataCache.getEntityTypeCodeFromName = function SparkleXrm_Sdk_Metadata_MetadataCache$getEntityTypeCodeFromName(typeName) {
    var entity = SparkleXrm.Sdk.Metadata.MetadataCache._loadEntityMetadata(typeName);
    return entity.objectTypeCode;
}
SparkleXrm.Sdk.Metadata.MetadataCache.getSmallIconUrl = function SparkleXrm_Sdk_Metadata_MetadataCache$getSmallIconUrl(typeName) {
    var entity = SparkleXrm.Sdk.Metadata.MetadataCache._loadEntityMetadata(typeName);
    if (entity.isCustomEntity != null && !!entity.isCustomEntity) {
        if (entity.iconSmallName != null) {
            return '../../' + entity.iconSmallName;
        }
        else {
            return '../../../../_Common/icon.aspx?cache=1&iconType=NavigationIcon&objectTypeCode=' + entity.objectTypeCode.toString();
        }
    }
    else {
        return '/_imgs/ico_16_' + entity.objectTypeCode.toString() + '.gif';
    }
}
SparkleXrm.Sdk.Metadata.MetadataCache._loadEntityMetadata = function SparkleXrm_Sdk_Metadata_MetadataCache$_loadEntityMetadata(entityLogicalName) {
    var cacheKey = entityLogicalName;
    var metaData = SparkleXrm.Sdk.Metadata.MetadataCache._entityMetaData[cacheKey];
    if (metaData == null) {
        var request = new SparkleXrm.Sdk.Messages.RetrieveEntityRequest();
        request.entityFilters = 1;
        request.logicalName = entityLogicalName;
        request.retrieveAsIfPublished = true;
        request.metadataId = new SparkleXrm.Sdk.Guid('00000000-0000-0000-0000-000000000000');
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        metaData = response.entityMetadata;
        SparkleXrm.Sdk.Metadata.MetadataCache._entityMetaData[cacheKey] = metaData;
    }
    return metaData;
}
SparkleXrm.Sdk.Metadata.MetadataCache._loadAttributeMetadata = function SparkleXrm_Sdk_Metadata_MetadataCache$_loadAttributeMetadata(entityLogicalName, attributeLogicalName) {
    var cacheKey = entityLogicalName + '|' + attributeLogicalName;
    var metaData = SparkleXrm.Sdk.Metadata.MetadataCache._attributeMetaData[cacheKey];
    if (metaData == null) {
        var request = new SparkleXrm.Sdk.Messages.RetrieveAttributeRequest();
        request.entityLogicalName = entityLogicalName;
        request.logicalName = attributeLogicalName;
        request.retrieveAsIfPublished = true;
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        metaData = response.attributeMetadata;
        SparkleXrm.Sdk.Metadata.MetadataCache._attributeMetaData[cacheKey] = metaData;
    }
    return metaData;
}
SparkleXrm.Sdk.Metadata.MetadataCache.AddOptionsetMetadata = function SparkleXrm_Sdk_Metadata_MetadataCache$AddOptionsetMetadata(entityLogicalName, attributeLogicalName, allowEmpty, metatdata) {
    var cacheKey = entityLogicalName + '.' + attributeLogicalName + '.' + allowEmpty.toString();
    var opts = [];
    if (allowEmpty) {
        opts.add({});
    }
    var $enum1 = ss.IEnumerator.getEnumerator(metatdata);
    while ($enum1.moveNext()) {
        var o = $enum1.current;
        var a = {};
        a.name = o['label'];
        a.value = o['value'];
        opts.add(a);
    }
    SparkleXrm.Sdk.Metadata.MetadataCache.get_optionsetMetaData()[cacheKey] = opts;
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.Query.MetadataConditionOperator

SparkleXrm.Sdk.Metadata.Query.MetadataConditionOperator = function() { };
SparkleXrm.Sdk.Metadata.Query.MetadataConditionOperator.prototype = {
    Equals: 'Equals', 
    NotEquals: 'NotEquals', 
    In: 'In', 
    NotIn: 'NotIn', 
    GreaterThan: 'GreaterThan', 
    LessThan: 'LessThan'
}
SparkleXrm.Sdk.Metadata.Query.MetadataConditionOperator.registerEnum('SparkleXrm.Sdk.Metadata.Query.MetadataConditionOperator', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.Query.LogicalOperator

SparkleXrm.Sdk.Metadata.Query.LogicalOperator = function() { };
SparkleXrm.Sdk.Metadata.Query.LogicalOperator.prototype = {
    And: 'And', 
    Or: 'Or'
}
SparkleXrm.Sdk.Metadata.Query.LogicalOperator.registerEnum('SparkleXrm.Sdk.Metadata.Query.LogicalOperator', false);


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder

SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder = function SparkleXrm_Sdk_Metadata_Query_MetadataQueryBuilder() {
    this.request = new SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest();
    this.request.query = {};
    this.request.query.criteria = {};
    this.request.query.criteria.filterOperator = 'Or';
    this.request.query.criteria.conditions = [];
}
SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder.prototype = {
    request: null,
    
    addEntities: function SparkleXrm_Sdk_Metadata_Query_MetadataQueryBuilder$addEntities(entityLogicalNames, propertiesToReturn) {
        this.request.query.criteria = {};
        this.request.query.criteria.filterOperator = 'Or';
        this.request.query.criteria.conditions = [];
        var $enum1 = ss.IEnumerator.getEnumerator(entityLogicalNames);
        while ($enum1.moveNext()) {
            var entity = $enum1.current;
            var condition = {};
            condition.conditionOperator = 'Equals';
            condition.propertyName = 'LogicalName';
            condition.value = entity;
            this.request.query.criteria.conditions.add(condition);
        }
        this.request.query.properties = {};
        this.request.query.properties.propertyNames = propertiesToReturn;
    },
    
    addAttributes: function SparkleXrm_Sdk_Metadata_Query_MetadataQueryBuilder$addAttributes(attributeLogicalNames, propertiesToReturn) {
        var attributeQuery = {};
        attributeQuery.properties = {};
        attributeQuery.properties.propertyNames = propertiesToReturn;
        this.request.query.attributeQuery = attributeQuery;
        var critiera = {};
        attributeQuery.criteria = critiera;
        critiera.filterOperator = 'Or';
        critiera.conditions = [];
        var $enum1 = ss.IEnumerator.getEnumerator(attributeLogicalNames);
        while ($enum1.moveNext()) {
            var attribute = $enum1.current;
            var condition = {};
            condition.propertyName = 'LogicalName';
            condition.conditionOperator = 'Equals';
            condition.value = attribute;
            critiera.conditions.add(condition);
        }
    },
    
    setLanguage: function SparkleXrm_Sdk_Metadata_Query_MetadataQueryBuilder$setLanguage(lcid) {
        this.request.query.labelQuery = {};
        this.request.query.labelQuery.filterLanguages = [];
        this.request.query.labelQuery.filterLanguages.add(lcid);
    }
}


Type.registerNamespace('SparkleXrm.Sdk.Ribbon');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Ribbon.RibbonButton

SparkleXrm.Sdk.Ribbon.RibbonButton = function SparkleXrm_Sdk_Ribbon_RibbonButton(Id, Sequence, LabelText, Command, Image16, Image32) {
    SparkleXrm.Sdk.Ribbon.RibbonButton.initializeBase(this, [ Id, Sequence, LabelText, Command, Image16, Image32 ]);
}
SparkleXrm.Sdk.Ribbon.RibbonButton.prototype = {
    
    serialiseToRibbonXml: function SparkleXrm_Sdk_Ribbon_RibbonButton$serialiseToRibbonXml(sb) {
        sb.appendLine('<Button Id="' + SparkleXrm.Sdk.XmlHelper.encode(this.Id) + '" LabelText="' + SparkleXrm.Sdk.XmlHelper.encode(this.LabelText) + '" Sequence="' + this.Sequence.toString() + '" Command="' + SparkleXrm.Sdk.XmlHelper.encode(this.Command) + '"' + ((this.Image32by32 != null) ? (' Image32by32="' + SparkleXrm.Sdk.XmlHelper.encode(this.Image32by32) + '"') : '') + ((this.Image16by16 != null) ? (' Image16by16="' + SparkleXrm.Sdk.XmlHelper.encode(this.Image16by16) + '"') : '') + ' />');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Ribbon.RibbonControl

SparkleXrm.Sdk.Ribbon.RibbonControl = function SparkleXrm_Sdk_Ribbon_RibbonControl(Id, Sequence, LabelText, Command, Image16, Image32) {
    this.Id = Id;
    this.Sequence = Sequence;
    this.LabelText = LabelText;
    this.Command = Command;
    this.Image16by16 = Image16;
    this.Image32by32 = Image32;
}
SparkleXrm.Sdk.Ribbon.RibbonControl.prototype = {
    Id: null,
    LabelText: null,
    Sequence: 0,
    Command: null,
    Image16by16: null,
    Image32by32: null,
    
    serialiseToRibbonXml: function SparkleXrm_Sdk_Ribbon_RibbonControl$serialiseToRibbonXml(sb) {
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Ribbon.RibbonFlyoutAnchor

SparkleXrm.Sdk.Ribbon.RibbonFlyoutAnchor = function SparkleXrm_Sdk_Ribbon_RibbonFlyoutAnchor(Id, Sequence, LabelText, Command, Image16, Image32) {
    SparkleXrm.Sdk.Ribbon.RibbonFlyoutAnchor.initializeBase(this, [ Id, Sequence, LabelText, Command, Image16, Image32 ]);
}
SparkleXrm.Sdk.Ribbon.RibbonFlyoutAnchor.prototype = {
    menu: null,
    
    serialiseToRibbonXml: function SparkleXrm_Sdk_Ribbon_RibbonFlyoutAnchor$serialiseToRibbonXml(sb) {
        sb.appendLine('<FlyoutAnchor Id="' + SparkleXrm.Sdk.XmlHelper.encode(this.Id) + '" LabelText="' + SparkleXrm.Sdk.XmlHelper.encode(this.LabelText) + '" Sequence="' + this.Sequence.toString() + '" Command="' + SparkleXrm.Sdk.XmlHelper.encode(this.Command) + '"' + ((this.Image32by32 != null) ? (' Image32by32="' + SparkleXrm.Sdk.XmlHelper.encode(this.Image32by32) + '"') : '') + ((this.Image16by16 != null) ? (' Image16by16="' + SparkleXrm.Sdk.XmlHelper.encode(this.Image16by16) + '"') : '') + ' PopulateDynamically="false">');
        sb.appendLine(this.menu.serialiseToRibbonXml());
        sb.appendLine('</FlyoutAnchor>');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Ribbon.RibbonMenu

SparkleXrm.Sdk.Ribbon.RibbonMenu = function SparkleXrm_Sdk_Ribbon_RibbonMenu(Id) {
    this.sections = [];
    this.Id = Id;
}
SparkleXrm.Sdk.Ribbon.RibbonMenu.prototype = {
    Id: null,
    
    serialiseToRibbonXml: function SparkleXrm_Sdk_Ribbon_RibbonMenu$serialiseToRibbonXml() {
        var sb = new ss.StringBuilder();
        sb.appendLine('<Menu Id="' + this.Id + '">');
        var $enum1 = ss.IEnumerator.getEnumerator(this.sections);
        while ($enum1.moveNext()) {
            var section = $enum1.current;
            section.serialiseToRibbonXml(sb);
        }
        sb.appendLine('</Menu>');
        return sb.toString();
    },
    
    addSection: function SparkleXrm_Sdk_Ribbon_RibbonMenu$addSection(section) {
        SparkleXrm.ArrayEx.add(this.sections, section);
        return this;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Sdk.Ribbon.RibbonMenuSection

SparkleXrm.Sdk.Ribbon.RibbonMenuSection = function SparkleXrm_Sdk_Ribbon_RibbonMenuSection(Id, LabelText, Sequence, DisplayMode) {
    this.buttons = [];
    this.Id = Id;
    this.Title = LabelText;
    this.Sequence = Sequence;
    this.DisplayMode = DisplayMode;
}
SparkleXrm.Sdk.Ribbon.RibbonMenuSection.prototype = {
    Id: null,
    Title: null,
    Sequence: 0,
    DisplayMode: null,
    
    serialiseToRibbonXml: function SparkleXrm_Sdk_Ribbon_RibbonMenuSection$serialiseToRibbonXml(sb) {
        sb.appendLine('<MenuSection Id="' + SparkleXrm.Sdk.XmlHelper.encode(this.Id) + ((this.Title != null) ? '" Title="' + this.Title : '') + '" Sequence="' + this.Sequence.toString() + '" DisplayMode="' + this.DisplayMode + '">');
        sb.appendLine('<Controls Id="' + SparkleXrm.Sdk.XmlHelper.encode(this.Id + '.Controls') + '">');
        var $enum1 = ss.IEnumerator.getEnumerator(this.buttons);
        while ($enum1.moveNext()) {
            var button = $enum1.current;
            button.serialiseToRibbonXml(sb);
        }
        sb.appendLine('</Controls>');
        sb.appendLine('</MenuSection>');
    },
    
    addButton: function SparkleXrm_Sdk_Ribbon_RibbonMenuSection$addButton(button) {
        SparkleXrm.ArrayEx.add(this.buttons, button);
        return this;
    }
}


Type.registerNamespace('SparkleXrm.Services');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Services.CachedOrganizationService

SparkleXrm.Services.CachedOrganizationService = function SparkleXrm_Services_CachedOrganizationService() {
}
SparkleXrm.Services.CachedOrganizationService.retrieve = function SparkleXrm_Services_CachedOrganizationService$retrieve(entityName, entityId, attributesList) {
    var result = SparkleXrm.Services.CachedOrganizationService.cache.get(entityName, entityId);
    if (result == null) {
        result = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(entityName, entityId, attributesList);
        SparkleXrm.Services.CachedOrganizationService.cache.insert(entityName, entityId, result);
        return result;
    }
    else {
        return result;
    }
}
SparkleXrm.Services.CachedOrganizationService.retrieveMultiple = function SparkleXrm_Services_CachedOrganizationService$retrieveMultiple(fetchXml) {
    var result = SparkleXrm.Services.CachedOrganizationService.cache.get('query', fetchXml);
    if (result == null) {
        result = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchXml);
        SparkleXrm.Services.CachedOrganizationService.cache.insert('query', fetchXml, result);
        return result;
    }
    else {
        return result;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Services.OrganizationServiceCache

SparkleXrm.Services.OrganizationServiceCache = function SparkleXrm_Services_OrganizationServiceCache() {
    this._innerCache = {};
}
SparkleXrm.Services.OrganizationServiceCache.prototype = {
    
    remove: function SparkleXrm_Services_OrganizationServiceCache$remove(entityName, id) {
    },
    
    insert: function SparkleXrm_Services_OrganizationServiceCache$insert(key, query, results) {
        this._innerCache[key + '_' + query] = results;
    },
    
    get: function SparkleXrm_Services_OrganizationServiceCache$get(key, query) {
        return this._innerCache[key + '_' + query];
    }
}


SparkleXrm.ArrayEx.registerClass('SparkleXrm.ArrayEx');
SparkleXrm.DelegateItterator.registerClass('SparkleXrm.DelegateItterator');
SparkleXrm.NumberEx.registerClass('SparkleXrm.NumberEx');
SparkleXrm.Xrm.PageEx.registerClass('SparkleXrm.Xrm.PageEx');
SparkleXrm.StringEx.registerClass('SparkleXrm.StringEx');
SparkleXrm.TaskIterrator.registerClass('SparkleXrm.TaskIterrator');
SparkleXrm.Sdk.Attribute.registerClass('SparkleXrm.Sdk.Attribute');
SparkleXrm.Sdk.AttributeTypes.registerClass('SparkleXrm.Sdk.AttributeTypes');
SparkleXrm.Sdk.Entity.registerClass('SparkleXrm.Sdk.Entity', null, SparkleXrm.ComponentModel.INotifyPropertyChanged);
SparkleXrm.Sdk.OrganizationSettings.registerClass('SparkleXrm.Sdk.OrganizationSettings', SparkleXrm.Sdk.Entity);
SparkleXrm.Sdk.UserSettingsAttributes.registerClass('SparkleXrm.Sdk.UserSettingsAttributes');
SparkleXrm.Sdk.UserSettings.registerClass('SparkleXrm.Sdk.UserSettings', SparkleXrm.Sdk.Entity);
SparkleXrm.Sdk.DataCollectionOfEntity.registerClass('SparkleXrm.Sdk.DataCollectionOfEntity', null, ss.IEnumerable);
SparkleXrm.Sdk.DateTimeEx.registerClass('SparkleXrm.Sdk.DateTimeEx');
SparkleXrm.Sdk.EntityCollection.registerClass('SparkleXrm.Sdk.EntityCollection');
SparkleXrm.Sdk.EntityReference.registerClass('SparkleXrm.Sdk.EntityReference');
SparkleXrm.Sdk.Guid.registerClass('SparkleXrm.Sdk.Guid');
SparkleXrm.Sdk.Money.registerClass('SparkleXrm.Sdk.Money');
SparkleXrm.Sdk.OptionSetValue.registerClass('SparkleXrm.Sdk.OptionSetValue');
SparkleXrm.Sdk.OrganizationServiceProxy.registerClass('SparkleXrm.Sdk.OrganizationServiceProxy');
SparkleXrm.Sdk.Relationship.registerClass('SparkleXrm.Sdk.Relationship');
SparkleXrm.Sdk.RetrieveRelationshipRequest.registerClass('SparkleXrm.Sdk.RetrieveRelationshipRequest', null, Object);
SparkleXrm.Sdk.RetrieveRelationshipResponse.registerClass('SparkleXrm.Sdk.RetrieveRelationshipResponse', null, Object);
SparkleXrm.Sdk.XmlHelper.registerClass('SparkleXrm.Sdk.XmlHelper');
SparkleXrm.Sdk.Messages.AssignRequest.registerClass('SparkleXrm.Sdk.Messages.AssignRequest', null, Object);
SparkleXrm.Sdk.Messages.AssignResponse.registerClass('SparkleXrm.Sdk.Messages.AssignResponse', null, Object);
SparkleXrm.Sdk.Messages.BulkDeleteRequest.registerClass('SparkleXrm.Sdk.Messages.BulkDeleteRequest', null, Object);
SparkleXrm.Sdk.Messages.BulkDeleteResponse.registerClass('SparkleXrm.Sdk.Messages.BulkDeleteResponse', null, Object);
SparkleXrm.Sdk.Messages.ExecuteWorkflowRequest.registerClass('SparkleXrm.Sdk.Messages.ExecuteWorkflowRequest', null, Object);
SparkleXrm.Sdk.Messages.ExecuteWorkflowResponse.registerClass('SparkleXrm.Sdk.Messages.ExecuteWorkflowResponse', null, Object);
SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionRequest.registerClass('SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionRequest', null, Object);
SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionResponse.registerClass('SparkleXrm.Sdk.Messages.FetchXmlToQueryExpressionResponse', null, Object);
SparkleXrm.Sdk.Messages.RetrieveAllEntitiesRequest.registerClass('SparkleXrm.Sdk.Messages.RetrieveAllEntitiesRequest', null, Object);
SparkleXrm.Sdk.Messages.RetrieveAllEntitiesResponse.registerClass('SparkleXrm.Sdk.Messages.RetrieveAllEntitiesResponse', null, Object);
SparkleXrm.Sdk.Messages.RetrieveAttributeRequest.registerClass('SparkleXrm.Sdk.Messages.RetrieveAttributeRequest', null, Object);
SparkleXrm.Sdk.Messages.RetrieveAttributeResponse.registerClass('SparkleXrm.Sdk.Messages.RetrieveAttributeResponse', null, Object);
SparkleXrm.Sdk.Messages.RetrieveEntityRequest.registerClass('SparkleXrm.Sdk.Messages.RetrieveEntityRequest', null, Object);
SparkleXrm.Sdk.Messages.RetrieveEntityResponse.registerClass('SparkleXrm.Sdk.Messages.RetrieveEntityResponse', null, Object);
SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest.registerClass('SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest', null, Object);
SparkleXrm.Sdk.Messages.RetrieveMetadataChangesResponse.registerClass('SparkleXrm.Sdk.Messages.RetrieveMetadataChangesResponse', null, Object);
SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser.registerClass('SparkleXrm.Sdk.Metadata.Query.MetadataSerialiser');
SparkleXrm.Sdk.Metadata.MetadataCache.registerClass('SparkleXrm.Sdk.Metadata.MetadataCache');
SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder.registerClass('SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder');
SparkleXrm.Sdk.Ribbon.RibbonControl.registerClass('SparkleXrm.Sdk.Ribbon.RibbonControl');
SparkleXrm.Sdk.Ribbon.RibbonButton.registerClass('SparkleXrm.Sdk.Ribbon.RibbonButton', SparkleXrm.Sdk.Ribbon.RibbonControl);
SparkleXrm.Sdk.Ribbon.RibbonFlyoutAnchor.registerClass('SparkleXrm.Sdk.Ribbon.RibbonFlyoutAnchor', SparkleXrm.Sdk.Ribbon.RibbonControl);
SparkleXrm.Sdk.Ribbon.RibbonMenu.registerClass('SparkleXrm.Sdk.Ribbon.RibbonMenu');
SparkleXrm.Sdk.Ribbon.RibbonMenuSection.registerClass('SparkleXrm.Sdk.Ribbon.RibbonMenuSection');
SparkleXrm.Services.CachedOrganizationService.registerClass('SparkleXrm.Services.CachedOrganizationService');
SparkleXrm.Services.OrganizationServiceCache.registerClass('SparkleXrm.Services.OrganizationServiceCache');
SparkleXrm.Xrm.PageEx.majorVersion = 0;
(function () {
    SparkleXrm.Xrm.PageEx.majorVersion = 2011;
    if (typeof(window.APPLICATION_VERSION) !== 'undefined') {
        var applicationVersion = window.APPLICATION_VERSION;
        if (applicationVersion !== '5.0') {
            SparkleXrm.Xrm.PageEx.majorVersion = 2013;
        }
    }
})();
SparkleXrm.Sdk.AttributeTypes.string_ = 'string';
SparkleXrm.Sdk.AttributeTypes.decimal_ = 'decimal';
SparkleXrm.Sdk.AttributeTypes.int_ = 'int';
SparkleXrm.Sdk.AttributeTypes.double_ = 'double';
SparkleXrm.Sdk.AttributeTypes.dateTime_ = 'dateTime';
SparkleXrm.Sdk.AttributeTypes.boolean_ = 'boolean';
SparkleXrm.Sdk.AttributeTypes.entityReference = 'EntityReference';
SparkleXrm.Sdk.AttributeTypes.guid_ = 'guid';
SparkleXrm.Sdk.AttributeTypes.optionSetValue = 'OptionSetValue';
SparkleXrm.Sdk.AttributeTypes.aliasedValue = 'AliasedValue';
SparkleXrm.Sdk.AttributeTypes.entityCollection = 'EntityCollection';
SparkleXrm.Sdk.AttributeTypes.money = 'Money';
SparkleXrm.Sdk.OrganizationSettings.entityLogicalName = 'organization';
SparkleXrm.Sdk.UserSettingsAttributes.userSettingsId = 'usersettingsid';
SparkleXrm.Sdk.UserSettingsAttributes.businessUnitId = 'businessunitid';
SparkleXrm.Sdk.UserSettingsAttributes.calendarType = 'calendartype';
SparkleXrm.Sdk.UserSettingsAttributes.currencyDecimalPrecision = 'currencydecimalprecision';
SparkleXrm.Sdk.UserSettingsAttributes.currencyFormatCode = 'currencyformatcode';
SparkleXrm.Sdk.UserSettingsAttributes.currencySymbol = 'currencysymbol';
SparkleXrm.Sdk.UserSettingsAttributes.dateFormatCode = 'dateformatcode';
SparkleXrm.Sdk.UserSettingsAttributes.dateFormatString = 'dateformatstring';
SparkleXrm.Sdk.UserSettingsAttributes.dateSeparator = 'dateseparator';
SparkleXrm.Sdk.UserSettingsAttributes.decimalSymbol = 'decimalsymbol';
SparkleXrm.Sdk.UserSettingsAttributes.defaultCalendarView = 'defaultcalendarview';
SparkleXrm.Sdk.UserSettingsAttributes.defaultDashboardId = 'defaultdashboardid';
SparkleXrm.Sdk.UserSettingsAttributes.localeId = 'localeid';
SparkleXrm.Sdk.UserSettingsAttributes.longDateFormatCode = 'longdateformatcode';
SparkleXrm.Sdk.UserSettingsAttributes.negativeCurrencyFormatCode = 'negativecurrencyformatcode';
SparkleXrm.Sdk.UserSettingsAttributes.negativeFormatCode = 'negativeformatcode';
SparkleXrm.Sdk.UserSettingsAttributes.numberGroupFormat = 'numbergroupformat';
SparkleXrm.Sdk.UserSettingsAttributes.numberSeparator = 'numberseparator';
SparkleXrm.Sdk.UserSettingsAttributes.offlineSyncInterval = 'offlinesyncinterval';
SparkleXrm.Sdk.UserSettingsAttributes.pricingDecimalPrecision = 'pricingdecimalprecision';
SparkleXrm.Sdk.UserSettingsAttributes.showWeekNumber = 'showweeknumber';
SparkleXrm.Sdk.UserSettingsAttributes.systemUserId = 'systemuserid';
SparkleXrm.Sdk.UserSettingsAttributes.timeFormatCodestring = 'timeformatcodestring';
SparkleXrm.Sdk.UserSettingsAttributes.timeFormatString = 'timeformatstring';
SparkleXrm.Sdk.UserSettingsAttributes.timeSeparator = 'timeseparator';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneBias = 'timezonebias';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneCode = 'timezonecode';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightBias = 'timezonedaylightbias';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightDay = 'timezonedaylightday';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightDayOfWeek = 'timezonedaylightdayofweek';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightHour = 'timezonedaylighthour';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightMinute = 'timezonedaylightminute';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightMonth = 'timezonedaylightmonth';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightSecond = 'timezonedaylightsecond';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneDaylightYear = 'timezonedaylightyear';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardBias = 'timezonestandardbias';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardDay = 'timezonestandardday';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardDayOfWeek = 'timezonestandarddayofweek';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardHour = 'timezonestandardhour';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardMinute = 'timezonestandardminute';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardMonth = 'timezonestandardmonth';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardSecond = 'timezonestandardsecond';
SparkleXrm.Sdk.UserSettingsAttributes.timeZoneStandardYear = 'timezonestandardyear';
SparkleXrm.Sdk.UserSettingsAttributes.transactionCurrencyId = 'transactioncurrencyid';
SparkleXrm.Sdk.UserSettingsAttributes.uiLanguageId = 'uilanguageid';
SparkleXrm.Sdk.UserSettingsAttributes.workdayStartTime = 'workdaystarttime';
SparkleXrm.Sdk.UserSettingsAttributes.workdayStopTime = 'workdaystoptime';
SparkleXrm.Sdk.UserSettings.entityLogicalName = 'usersettings';
SparkleXrm.Sdk.Guid.empty = new SparkleXrm.Sdk.Guid('00000000-0000-0000-0000-000000000000');
SparkleXrm.Sdk.OrganizationServiceProxy.withCredentials = false;
SparkleXrm.Sdk.OrganizationServiceProxy.userSettings = null;
SparkleXrm.Sdk.OrganizationServiceProxy.executeMessageResponseTypes = {};
SparkleXrm.Sdk.OrganizationServiceProxy.organizationSettings = null;
SparkleXrm.Sdk.XmlHelper._encode_map = { '&': '&amp;', '"': '&quot;', '<': '&lt;', '>': '&gt;', "'": '&#39;' };
SparkleXrm.Sdk.XmlHelper._decode_map = { '&amp;': '&', '&quot;': '"', '&lt;': '<', '&gt;': '>', '&#39;': "'" };
SparkleXrm.Sdk.Metadata.MetadataCache._attributeMetaData = {};
SparkleXrm.Sdk.Metadata.MetadataCache._entityMetaData = {};
SparkleXrm.Sdk.Metadata.MetadataCache._optionsCache = {};
SparkleXrm.Services.CachedOrganizationService.cache = new SparkleXrm.Services.OrganizationServiceCache();
});

