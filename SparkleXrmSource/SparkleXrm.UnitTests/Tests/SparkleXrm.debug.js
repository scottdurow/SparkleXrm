//! SparkleXrm.debug.js
//
waitForScripts("xrm",["mscorlib"],
function () {



Type.registerNamespace('Xrm');

////////////////////////////////////////////////////////////////////////////////
// Xrm.ArrayEx

Xrm.ArrayEx = function Xrm_ArrayEx() {
}
Xrm.ArrayEx.add = function Xrm_ArrayEx$add(list, item) {
    list[list.length]=item;
}
Xrm.ArrayEx.getEnumerator = function Xrm_ArrayEx$getEnumerator(list) {
    return new ss.ArrayEnumerator(list);
}
Xrm.ArrayEx.join = function Xrm_ArrayEx$join(list, delimeter) {
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
// Xrm.DelegateItterator

Xrm.DelegateItterator = function Xrm_DelegateItterator() {
}
Xrm.DelegateItterator.callbackItterate = function Xrm_DelegateItterator$callbackItterate(action, numberOfTimes, completeCallBack, errorCallBack) {
    Xrm.DelegateItterator._callbackItterateAction(action, 0, numberOfTimes, completeCallBack, errorCallBack);
}
Xrm.DelegateItterator._callbackItterateAction = function Xrm_DelegateItterator$_callbackItterateAction(action, index, numberOfTimes, completeCallBack, errorCallBack) {
    if (index < numberOfTimes) {
        try {
            action(index, function() {
                index++;
                Xrm.DelegateItterator._callbackItterateAction(action, index, numberOfTimes, completeCallBack, errorCallBack);
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
// Xrm.NumberEx

Xrm.NumberEx = function Xrm_NumberEx() {
}
Xrm.NumberEx.parse = function Xrm_NumberEx$parse(value, format) {
    if (String.isNullOrEmpty(value)) {
        return null;
    }
    value = value.replaceAll(' ', '');
    if (format.decimalSymbol !== '.') {
        value = value.replaceAll(format.decimalSymbol, '.');
    }
    value = value.replaceAll(format.numberSepartor, '');
    if (value.startsWith('(')) {
        value = '-' + value.replaceAll('(', '').replaceAll(')', '');
    }
    else if (value.endsWith('-')) {
        value = '-' + value.substring(0, value.length - 1);
    }
    var numericValue = Number.parse(value);
    return numericValue;
}
Xrm.NumberEx.getNumberFormatInfo = function Xrm_NumberEx$getNumberFormatInfo() {
    var format = {};
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        format.decimalSymbol = Xrm.Sdk.OrganizationServiceProxy.userSettings.decimalsymbol;
        format.numberGroupFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.numbergroupformat;
        format.numberSepartor = Xrm.Sdk.OrganizationServiceProxy.userSettings.numberseparator;
        format.negativeFormatCode = Xrm.Sdk.OrganizationServiceProxy.userSettings.negativeformatcode;
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
Xrm.NumberEx.getCurrencyEditFormatInfo = function Xrm_NumberEx$getCurrencyEditFormatInfo() {
    var format = {};
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        format.decimalSymbol = Xrm.Sdk.OrganizationServiceProxy.userSettings.decimalsymbol;
        format.numberGroupFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.numbergroupformat;
        format.numberSepartor = Xrm.Sdk.OrganizationServiceProxy.userSettings.numberseparator;
        format.negativeFormatCode = Xrm.Sdk.OrganizationServiceProxy.userSettings.negativecurrencyformatcode;
        format.precision = Xrm.Sdk.OrganizationServiceProxy.userSettings.currencydecimalprecision;
        format.currencySymbol = Xrm.Sdk.OrganizationServiceProxy.userSettings.currencysymbol;
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
Xrm.NumberEx.getCurrencyFormatInfo = function Xrm_NumberEx$getCurrencyFormatInfo() {
    var format = {};
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        format.decimalSymbol = Xrm.Sdk.OrganizationServiceProxy.userSettings.decimalsymbol;
        format.numberGroupFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.numbergroupformat;
        format.numberSepartor = Xrm.Sdk.OrganizationServiceProxy.userSettings.numberseparator;
        format.negativeFormatCode = Xrm.Sdk.OrganizationServiceProxy.userSettings.negativecurrencyformatcode;
        format.precision = Xrm.Sdk.OrganizationServiceProxy.userSettings.currencydecimalprecision;
        format.currencySymbol = Xrm.Sdk.OrganizationServiceProxy.userSettings.currencysymbol;
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
Xrm.NumberEx.format = function Xrm_NumberEx$format(value, format) {
    if (value == null) {
        return '';
    }
    var numberGroupFormats = format.numberGroupFormat.split(',');
    var formattedNumber = '';
    var wholeNumber = Math.floor(Math.abs(value));
    var wholeNumberString = wholeNumber.toString();
    var decimalPartString = value.toString().substr(wholeNumberString.length + 1);
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
Xrm.NumberEx.round = function Xrm_NumberEx$round(numericValue, precision) {
    var precisionMultiplier = 1;
    if (precision > 0) {
        precisionMultiplier = Math.pow(10, precision);
    }
    return Math.round(numericValue * precisionMultiplier) / precisionMultiplier;
}
Xrm.NumberEx.getCurrencySymbol = function Xrm_NumberEx$getCurrencySymbol(currencyId) {
    var orgSettings = Xrm.Services.CachedOrganizationService.retrieveMultiple("<fetch distinct='false' no-lock='false' mapping='logical'><entity name='organization'><attribute name='currencydisplayoption' /><attribute name='currencysymbol' /></entity></fetch>");
    var orgSetting = orgSettings.get_entities().get_item(0);
    var currency = Xrm.Services.CachedOrganizationService.retrieve('transactioncurrency', currencyId.toString(), [ 'currencysymbol', 'isocurrencycode' ]);
    if (!orgSetting.getAttributeValueInt('currencydisplayoption')) {
        return currency.getAttributeValueString('currencysymbol') + ' ';
    }
    else {
        return currency.getAttributeValueString('isocurrencycode') + ' ';
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.PageEx

Xrm.PageEx = function Xrm_PageEx() {
}
Xrm.PageEx.getCacheKey = function Xrm_PageEx$getCacheKey() {
    var cacheKey = WEB_RESOURCE_ORG_VERSION_NUMBER;
    if (typeof(cacheKey) !== 'undefined') {
        return cacheKey + '/';
    }
    else {
        return '';
    }
}
Xrm.PageEx.getWebResourceData = function Xrm_PageEx$getWebResourceData() {
    var queryString = window.location.search;
    if (queryString != null && !!queryString) {
        var parameters = queryString.substr(1).split('&');
        var $enum1 = ss.IEnumerator.getEnumerator(parameters);
        while ($enum1.moveNext()) {
            var param = $enum1.current;
            if (param.toLowerCase().startsWith('data=')) {
                var dataParam = param.replaceAll('+', ' ').split('=');
                return Xrm.PageEx._parseDataParameter(dataParam[1]);
            }
        }
    }
    return {};
}
Xrm.PageEx._parseDataParameter = function Xrm_PageEx$_parseDataParameter(data) {
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
// Xrm.StringEx

Xrm.StringEx = function Xrm_StringEx() {
}
Xrm.StringEx.IN = function Xrm_StringEx$IN(value, values) {
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
// Xrm.TabItem

Xrm.TabItem = function Xrm_TabItem() {
}
Xrm.TabItem.prototype = {
    sections: null,
    
    getDisplayState: function Xrm_TabItem$getDisplayState() {
        return null;
    },
    
    getLabel: function Xrm_TabItem$getLabel() {
        return null;
    },
    
    getName: function Xrm_TabItem$getName() {
        return null;
    },
    
    getParent: function Xrm_TabItem$getParent() {
        return null;
    },
    
    getVisible: function Xrm_TabItem$getVisible() {
        return false;
    },
    
    setDisplayState: function Xrm_TabItem$setDisplayState(state) {
    },
    
    setFocus: function Xrm_TabItem$setFocus() {
    },
    
    setLabel: function Xrm_TabItem$setLabel(label) {
    },
    
    setVisible: function Xrm_TabItem$setVisible(visible) {
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.TabSection

Xrm.TabSection = function Xrm_TabSection() {
}
Xrm.TabSection.prototype = {
    controls: null,
    
    getLabel: function Xrm_TabSection$getLabel() {
        return null;
    },
    
    getName: function Xrm_TabSection$getName() {
        return null;
    },
    
    getParent: function Xrm_TabSection$getParent() {
        return null;
    },
    
    getVisible: function Xrm_TabSection$getVisible() {
        return false;
    },
    
    setLabel: function Xrm_TabSection$setLabel(label) {
    },
    
    setVisible: function Xrm_TabSection$setVisible(visible) {
    }
}


Type.registerNamespace('Xrm.ComponentModel');

////////////////////////////////////////////////////////////////////////////////
// Xrm.ComponentModel.INotifyPropertyChanged

Xrm.ComponentModel.INotifyPropertyChanged = function() { };
Xrm.ComponentModel.INotifyPropertyChanged.prototype = {
    add_propertyChanged : null,
    remove_propertyChanged : null,
    raisePropertyChanged : null
}
Xrm.ComponentModel.INotifyPropertyChanged.registerInterface('Xrm.ComponentModel.INotifyPropertyChanged');


Type.registerNamespace('Xrm.Sdk');

////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.EntityStates

Xrm.Sdk.EntityStates = function() { };
Xrm.Sdk.EntityStates.prototype = {
    unchanged: 0, 
    created: 1, 
    changed: 2, 
    deleted: 3
}
Xrm.Sdk.EntityStates.registerEnum('Xrm.Sdk.EntityStates', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Attribute

Xrm.Sdk.Attribute = function Xrm_Sdk_Attribute(attributeName, typeName) {
    this.attributeName = attributeName;
    this.typeName = typeName;
    this.formattedValue = null;
    this.value = null;
    this.id = null;
    this.logicalName = null;
    this.name = null;
}
Xrm.Sdk.Attribute.deSerialise = function Xrm_Sdk_Attribute$deSerialise(node, overrideType) {
    var isNil = (Xrm.Sdk.XmlHelper.getAttributeValue(node, 'i:nil') === 'true');
    var value = null;
    if (!isNil) {
        var typeName = overrideType;
        if (typeName == null) {
            typeName = Xrm.Sdk.Attribute._removeNsPrefix(Xrm.Sdk.XmlHelper.getAttributeValue(node, 'i:type'));
        }
        var stringValue = Xrm.Sdk.XmlHelper.getNodeTextValue(node);
        switch (typeName) {
            case 'EntityReference':
                var entityReferenceValue = new Xrm.Sdk.EntityReference(new Xrm.Sdk.Guid(Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Id')), Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'LogicalName'), Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Name'));
                value = entityReferenceValue;
                break;
            case 'AliasedValue':
                value = Xrm.Sdk.Attribute.deSerialise(Xrm.Sdk.XmlHelper.selectSingleNode(node, 'Value'), null);
                break;
            case 'boolean':
                value = (stringValue === 'true');
                break;
            case 'decimal':
                value = parseFloat(stringValue);
                break;
            case 'dateTime':
                var dateValue = Xrm.Sdk.DateTimeEx.parse(stringValue);
                var settings = Xrm.Sdk.OrganizationServiceProxy.userSettings;
                if (settings != null) {
                    dateValue.setTime(dateValue.getTime() + (dateValue.getTimezoneOffset() * 60 * 1000));
                    var localDateValue = Xrm.Sdk.DateTimeEx.utcToLocalTimeFromSettings(dateValue, settings);
                    value = localDateValue;
                }
                else {
                    value = dateValue;
                }
                break;
            case 'guid':
                value = new Xrm.Sdk.Guid(stringValue);
                break;
            case 'int':
                value = parseInt(stringValue);
                break;
            case 'OptionSetValue':
                value = Xrm.Sdk.OptionSetValue.parse(Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Value'));
                break;
            case 'Money':
                value = new Xrm.Sdk.Money(parseFloat(Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'Value')));
                break;
            default:
                value = stringValue;
                break;
        }
    }
    return value;
}
Xrm.Sdk.Attribute.serialise = function Xrm_Sdk_Attribute$serialise(attributeName, value, metaData) {
    var xml = '<a:KeyValuePairOfstringanyType><b:key>' + attributeName + '</b:key>';
    var typeName = Type.getInstanceType(value).get_name();
    if (value != null && metaData != null && Object.keyExists(metaData, attributeName)) {
        typeName = metaData[attributeName];
    }
    xml += Xrm.Sdk.Attribute.serialiseValue(value, typeName);
    xml += '</a:KeyValuePairOfstringanyType>';
    return xml;
}
Xrm.Sdk.Attribute.serialiseValue = function Xrm_Sdk_Attribute$serialiseValue(value, overrideTypeName) {
    var valueXml = '';
    var typeName = overrideTypeName;
    if (typeName == null) {
        typeName = Type.getInstanceType(value).get_name();
    }
    switch (typeName) {
        case 'String':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('string') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            valueXml += Xrm.Sdk.XmlHelper.encode(value);
            valueXml += '</b:value>';
            break;
        case 'Boolean':
        case 'bool':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('boolean') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            valueXml += Xrm.Sdk.XmlHelper.encode(value.toString());
            valueXml += '</b:value>';
            break;
        case 'Date':
            var dateValue = value;
            var dateString = null;
            var settings = Xrm.Sdk.OrganizationServiceProxy.userSettings;
            if (settings != null) {
                var utcDateValue = Xrm.Sdk.DateTimeEx.localTimeToUTCFromSettings(dateValue, settings);
                dateString = Xrm.Sdk.DateTimeEx.toXrmString(utcDateValue);
            }
            else {
                dateString = Xrm.Sdk.DateTimeEx.toXrmStringUTC(dateValue);
            }
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('dateTime') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            valueXml += Xrm.Sdk.XmlHelper.encode(dateString);
            valueXml += '</b:value>';
            break;
        case 'decimal':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('decimal') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            var decStringValue = null;
            if (value != null) {
                decStringValue = value.toString();
            }
            valueXml += Xrm.Sdk.XmlHelper.encode(decStringValue);
            valueXml += '</b:value>';
            break;
        case 'double':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('double') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            var doubleStringValue = null;
            if (value != null) {
                doubleStringValue = value.toString();
            }
            valueXml += Xrm.Sdk.XmlHelper.encode(doubleStringValue);
            valueXml += '</b:value>';
            break;
        case 'int':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('int') + '" xmlns:c="http://www.w3.org/2001/XMLSchema">';
            var intStringValue = null;
            if (value != null) {
                intStringValue = value.toString();
            }
            valueXml += Xrm.Sdk.XmlHelper.encode(intStringValue);
            valueXml += '</b:value>';
            break;
        case 'Guid':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix('guid') + '" xmlns:c="http://schemas.microsoft.com/2003/10/Serialization/">';
            valueXml += (value).value;
            valueXml += '</b:value>';
            break;
        case 'EntityReference':
            var entityReferenceValue = value;
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
            valueXml += '<a:Id>' + entityReferenceValue.id + '</a:Id><a:LogicalName>' + entityReferenceValue.logicalName + '</a:LogicalName>';
            valueXml += '</b:value>';
            break;
        case 'OptionSetValue':
            var opt = value;
            if (opt.value != null) {
                valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
                valueXml += '<a:Value>' + opt.value + '</a:Value>';
                valueXml += '</b:value>';
            }
            else {
                valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix(typeName) + '" i:nil="true"/>';
            }
            break;
        case 'EntityCollection':
            valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
            if (Type.getInstanceType(value) !== Array) {
                throw new Error("An attribute value of type 'EntityCollection' must contain an Array() of Entity instances");
            }
            var arrayValue = Type.safeCast(value, Array);
            valueXml += '<a:Entities>';
            for (var i = 0; i < arrayValue.length; i++) {
                if (Type.getInstanceType(arrayValue[i]) !== Xrm.Sdk.Entity) {
                    throw new Error("An attribute value of type 'EntityCollection' must contain an Array() of Entity instances");
                }
                valueXml += (arrayValue[i]).serialise(false);
            }
            valueXml += '</a:Entities>';
            valueXml += '</b:value>';
            break;
        case 'Money':
            var money = value;
            if (money != null) {
                valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix(typeName) + '">';
                valueXml += '<a:Value>' + money.value.toString() + '</a:Value>';
                valueXml += '</b:value>';
            }
            else {
                valueXml += '<b:value i:type="' + Xrm.Sdk.Attribute._addNsPrefix(typeName) + '" i:nil="true"/>';
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
            valueXml += '<b:value i:type="c:EntityFilters" xmlns:c="http://schemas.microsoft.com/xrm/2011/Metadata">' + Xrm.Sdk.XmlHelper.encode(entityFilterValues.join(' ')) + '</b:value>';
            break;
        default:
            valueXml += '<b:value i:nil="true"/>';
            break;
    }
    return valueXml;
}
Xrm.Sdk.Attribute._removeNsPrefix = function Xrm_Sdk_Attribute$_removeNsPrefix(node) {
    var i = node.indexOf(':');
    return node.substring(i + 1, node.length - i + 1);
}
Xrm.Sdk.Attribute._addNsPrefix = function Xrm_Sdk_Attribute$_addNsPrefix(type) {
    switch (type) {
        case 'String':
        case 'Guid':
        case 'DateTime':
        case 'string':
        case 'decimal':
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
    throw new Error('Could add node prefix for type ' + type);
}
Xrm.Sdk.Attribute.prototype = {
    attributeName: null,
    typeName: null,
    formattedValue: null,
    value: null,
    id: null,
    logicalName: null,
    name: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.AttributeTypes

Xrm.Sdk.AttributeTypes = function Xrm_Sdk_AttributeTypes() {
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.UserSettingsAttributes

Xrm.Sdk.UserSettingsAttributes = function Xrm_Sdk_UserSettingsAttributes() {
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.UserSettings

Xrm.Sdk.UserSettings = function Xrm_Sdk_UserSettings() {
    Xrm.Sdk.UserSettings.initializeBase(this, [ Xrm.Sdk.UserSettings.entityLogicalName ]);
}
Xrm.Sdk.UserSettings.prototype = {
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
    
    getNumberFormatString: function Xrm_Sdk_UserSettings$getNumberFormatString(decimalPlaces) {
        return '###,###,###.000';
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.DataCollectionOfEntity

Xrm.Sdk.DataCollectionOfEntity = function Xrm_Sdk_DataCollectionOfEntity(entities) {
    this._internalArray = entities;
}
Xrm.Sdk.DataCollectionOfEntity.prototype = {
    _internalArray: null,
    
    items: function Xrm_Sdk_DataCollectionOfEntity$items() {
        return this._internalArray;
    },
    
    getEnumerator: function Xrm_Sdk_DataCollectionOfEntity$getEnumerator() {
        return Xrm.ArrayEx.getEnumerator(this._internalArray);
    },
    
    get_count: function Xrm_Sdk_DataCollectionOfEntity$get_count() {
        return this._internalArray.length;
    },
    get_item: function Xrm_Sdk_DataCollectionOfEntity$get_item(i) {
        return this._internalArray[i];
    },
    set_item: function Xrm_Sdk_DataCollectionOfEntity$set_item(i, value) {
        this._internalArray[i] = value;
        return value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.DateTimeEx

Xrm.Sdk.DateTimeEx = function Xrm_Sdk_DateTimeEx() {
}
Xrm.Sdk.DateTimeEx.toXrmString = function Xrm_Sdk_DateTimeEx$toXrmString(date) {
    var month = Xrm.Sdk.DateTimeEx._padNumber(date.getMonth() + 1, 2);
    var day = Xrm.Sdk.DateTimeEx._padNumber(date.getDate(), 2);
    var hours = Xrm.Sdk.DateTimeEx._padNumber(date.getHours(), 2);
    var mins = Xrm.Sdk.DateTimeEx._padNumber(date.getMinutes(), 2);
    var secs = Xrm.Sdk.DateTimeEx._padNumber(date.getSeconds(), 2);
    return String.format('{0}-{1}-{2}T{3}:{4}:{5}Z', date.getFullYear(), month, day, hours, mins, secs);
}
Xrm.Sdk.DateTimeEx.toXrmStringUTC = function Xrm_Sdk_DateTimeEx$toXrmStringUTC(date) {
    var month = Xrm.Sdk.DateTimeEx._padNumber(date.getUTCMonth() + 1, 2);
    var day = Xrm.Sdk.DateTimeEx._padNumber(date.getUTCDate(), 2);
    var hours = Xrm.Sdk.DateTimeEx._padNumber(date.getUTCHours(), 2);
    var mins = Xrm.Sdk.DateTimeEx._padNumber(date.getUTCMinutes(), 2);
    var secs = Xrm.Sdk.DateTimeEx._padNumber(date.getUTCSeconds(), 2);
    return String.format('{0}-{1}-{2}T{3}:{4}:{5}Z', date.getUTCFullYear(), month, day, hours, mins, secs);
}
Xrm.Sdk.DateTimeEx._padNumber = function Xrm_Sdk_DateTimeEx$_padNumber(number, length) {
    var str = number.toString();
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}
Xrm.Sdk.DateTimeEx.parse = function Xrm_Sdk_DateTimeEx$parse(dateString) {
    if (!String.isNullOrEmpty(dateString)) {
        var dateTimeParsed = (Date.parseDate(dateString));
        return dateTimeParsed;
    }
    else {
        return null;
    }
}
Xrm.Sdk.DateTimeEx.formatDuration = function Xrm_Sdk_DateTimeEx$formatDuration(totalMinutes) {
    if (totalMinutes != null) {
        var toatalSeconds = totalMinutes * 60;
        var days = Math.floor(toatalSeconds / 86400);
        var hours = Math.floor((toatalSeconds % 86400) / 3600);
        var minutes = Math.floor(((toatalSeconds % 86400) % 3600) / 60);
        var seconds = ((toatalSeconds % 86400) % 3600) % 60;
        var formatString = [];
        if (days > 0) {
            Xrm.ArrayEx.add(formatString, '{0}d');
        }
        if (hours > 0) {
            Xrm.ArrayEx.add(formatString, '{1}h');
        }
        if (minutes > 0) {
            Xrm.ArrayEx.add(formatString, '{2}m');
        }
        return String.format(Xrm.ArrayEx.join(formatString, ' '), days, hours, minutes);
    }
    else {
        return '';
    }
}
Xrm.Sdk.DateTimeEx.parseDuration = function Xrm_Sdk_DateTimeEx$parseDuration(duration) {
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
Xrm.Sdk.DateTimeEx.addTimeToDate = function Xrm_Sdk_DateTimeEx$addTimeToDate(date, time) {
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
Xrm.Sdk.DateTimeEx.localTimeToUTCFromSettings = function Xrm_Sdk_DateTimeEx$localTimeToUTCFromSettings(LocalTime, settings) {
    return Xrm.Sdk.DateTimeEx.localTimeToUTC(LocalTime, settings.timezonebias, settings.timezonedaylightbias, settings.timezonedaylightyear, settings.timezonedaylightmonth, settings.timezonedaylightday, settings.timezonedaylighthour, settings.timezonedaylightminute, settings.timezonedaylightsecond, 0, settings.timezonedaylightdayofweek, settings.timezonestandardbias, settings.timezonestandardyear, settings.timezonestandardmonth, settings.timezonestandardday, settings.timezonestandardhour, settings.timezonestandardminute, settings.timezonestandardsecond, 0, settings.timezonestandarddayofweek);
}
Xrm.Sdk.DateTimeEx.localTimeToUTC = function Xrm_Sdk_DateTimeEx$localTimeToUTC(LocalTime, Bias, DaylightBias, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday, StandardBias, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday) {
    var TimeZoneBias;
    var NewTimeZoneBias;
    var LocalCustomBias;
    var StandardTime;
    var DaylightTime;
    var ComputedUniversalTime;
    var bDaylightTimeZone;
    NewTimeZoneBias = Bias;
    if ((!!StandardMonth) && (!!DaylightMonth)) {
        StandardTime = Xrm.Sdk.DateTimeEx._getCutoverTime(LocalTime, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday);
        if (StandardTime == null) {
            return null;
        }
        DaylightTime = Xrm.Sdk.DateTimeEx._getCutoverTime(LocalTime, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday);
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
    ComputedUniversalTime = Xrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias, LocalTime);
    return ComputedUniversalTime;
}
Xrm.Sdk.DateTimeEx.utcToLocalTimeFromSettings = function Xrm_Sdk_DateTimeEx$utcToLocalTimeFromSettings(UTCTime, settings) {
    return Xrm.Sdk.DateTimeEx.utcToLocalTime(UTCTime, settings.timezonebias, settings.timezonedaylightbias, settings.timezonedaylightyear, settings.timezonedaylightmonth, settings.timezonedaylightday, settings.timezonedaylighthour, settings.timezonedaylightminute, settings.timezonedaylightsecond, 0, settings.timezonedaylightdayofweek, settings.timezonestandardbias, settings.timezonestandardyear, settings.timezonestandardmonth, settings.timezonestandardday, settings.timezonestandardhour, settings.timezonestandardminute, settings.timezonestandardsecond, 0, settings.timezonestandarddayofweek);
}
Xrm.Sdk.DateTimeEx.utcToLocalTime = function Xrm_Sdk_DateTimeEx$utcToLocalTime(UTCTime, Bias, DaylightBias, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday, StandardBias, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday) {
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
        StandardTime = Xrm.Sdk.DateTimeEx._getCutoverTime(UTCTime, StandardYear, StandardMonth, StandardDay, StandardHour, StandardMinute, StandardSecond, StandardMilliseconds, StandardWeekday);
        if (StandardTime == null) {
            return null;
        }
        DaylightTime = Xrm.Sdk.DateTimeEx._getCutoverTime(UTCTime, DaylightYear, DaylightMonth, DaylightDay, DaylightHour, DaylightMinute, DaylightSecond, DaylightMilliseconds, DaylightWeekday);
        if (DaylightTime == null) {
            return null;
        }
        LocalCustomBias = StandardBias;
        TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
        UtcDaylightTime = Xrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias, DaylightTime);
        LocalCustomBias = DaylightBias;
        TimeZoneBias = NewTimeZoneBias + LocalCustomBias;
        UtcStandardTime = Xrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias, StandardTime);
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
    ComputedLocalTime = Xrm.Sdk.DateTimeEx.dateAdd('minutes', TimeZoneBias * -1, UTCTime);
    return ComputedLocalTime;
}
Xrm.Sdk.DateTimeEx._getCutoverTime = function Xrm_Sdk_DateTimeEx$_getCutoverTime(CurrentTime, Year, Month, Day, Hour, Minute, Second, Milliseconds, Weekday) {
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
    WorkingTime = Xrm.Sdk.DateTimeEx.firstDayOfMonth(CurrentTime, TargetMonth);
    WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('hours', Hour, WorkingTime);
    WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('minutes', Minute, WorkingTime);
    WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('seconds', Second, WorkingTime);
    WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('milliseconds', Milliseconds, WorkingTime);
    ScratchTime = WorkingTime;
    if (ScratchTime.getDay() > TargetWeekday) {
        WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('days', (7 - (ScratchTime.getDay() - TargetWeekday)), WorkingTime);
    }
    else if (ScratchTime.getDay() < TargetWeekday) {
        WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('days', (TargetWeekday - ScratchTime.getDay()), WorkingTime);
    }
    BestWeekdayDate = WorkingTime.getDay();
    WorkingWeekdayNumber = 1;
    ScratchTime = WorkingTime;
    while (WorkingWeekdayNumber < TargetWeekdayNumber) {
        WorkingTime = Xrm.Sdk.DateTimeEx.dateAdd('days', 7, WorkingTime);
        if (WorkingTime.getMonth() !== ScratchTime.getMonth()) {
            break;
        }
        ScratchTime = WorkingTime;
        WorkingWeekdayNumber = WorkingWeekdayNumber + 1;
    }
    return ScratchTime;
}
Xrm.Sdk.DateTimeEx.firstDayOfMonth = function Xrm_Sdk_DateTimeEx$firstDayOfMonth(date, Month) {
    var startOfMonth = new Date(date.getTime());
    startOfMonth.setMonth(Month - 1);
    startOfMonth.setDate(1);
    startOfMonth.setHours(0);
    startOfMonth.setMinutes(0);
    startOfMonth.setSeconds(0);
    startOfMonth.setMilliseconds(0);
    return startOfMonth;
}
Xrm.Sdk.DateTimeEx.dateAdd = function Xrm_Sdk_DateTimeEx$dateAdd(interval, value, date) {
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
Xrm.Sdk.DateTimeEx.firstDayOfWeek = function Xrm_Sdk_DateTimeEx$firstDayOfWeek(date) {
    var startOfWeek = new Date(date.getTime());
    var dayOfWeek = startOfWeek.getDay();
    if (dayOfWeek > 0) {
        startOfWeek = Xrm.Sdk.DateTimeEx.dateAdd('days', (dayOfWeek * -1), startOfWeek);
    }
    startOfWeek.setHours(0);
    startOfWeek.setMinutes(0);
    startOfWeek.setSeconds(0);
    startOfWeek.setMilliseconds(0);
    return startOfWeek;
}
Xrm.Sdk.DateTimeEx.lastDayOfWeek = function Xrm_Sdk_DateTimeEx$lastDayOfWeek(date) {
    var endOfWeek = new Date(date.getTime());
    var dayOfWeek = endOfWeek.getDay();
    if (dayOfWeek > 0) {
        endOfWeek = Xrm.Sdk.DateTimeEx.dateAdd('days', (7 - dayOfWeek), endOfWeek);
    }
    endOfWeek.setHours(23);
    endOfWeek.setMinutes(59);
    endOfWeek.setSeconds(59);
    endOfWeek.setMilliseconds(999);
    return endOfWeek;
}
Xrm.Sdk.DateTimeEx.formatTimeSpecific = function Xrm_Sdk_DateTimeEx$formatTimeSpecific(dateValue, formatString) {
    formatString = formatString.replaceAll('.', ':').replaceAll('-', ':').replaceAll(',', ':');
    if (dateValue != null && (Date === Type.getInstanceType(dateValue))) {
        return dateValue.format(formatString);
    }
    else {
        return '';
    }
}
Xrm.Sdk.DateTimeEx.formatDateSpecific = function Xrm_Sdk_DateTimeEx$formatDateSpecific(dateValue, formatString) {
    if (dateValue != null) {
        return xrmjQuery.datepicker.formatDate( formatString, dateValue );
    }
    else {
        return '';
    }
}
Xrm.Sdk.DateTimeEx.setTime = function Xrm_Sdk_DateTimeEx$setTime(date, time) {
    if (date != null && time != null) {
        date.setHours(time.getHours());
        date.setMinutes(time.getMinutes());
        date.setSeconds(time.getSeconds());
        date.setMilliseconds(time.getMilliseconds());
    }
}
Xrm.Sdk.DateTimeEx.setUTCTime = function Xrm_Sdk_DateTimeEx$setUTCTime(date, time) {
    if (date != null && time != null) {
        date.setUTCHours(time.getUTCHours());
        date.setUTCMinutes(time.getUTCMinutes());
        date.setUTCSeconds(time.getUTCSeconds());
        date.setUTCMilliseconds(time.getUTCMilliseconds());
    }
}
Xrm.Sdk.DateTimeEx.getTimeDuration = function Xrm_Sdk_DateTimeEx$getTimeDuration(date) {
    return (date.getHours() * (60 * 60)) + (date.getMinutes() * 60) + date.getSeconds();
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Entity

Xrm.Sdk.Entity = function Xrm_Sdk_Entity(entityName) {
    this._metaData = {};
    this.logicalName = entityName;
    this._attributes = {};
    this.formattedValues = {};
}
Xrm.Sdk.Entity.prototype = {
    logicalName: null,
    id: null,
    entityState: 0,
    _attributes: null,
    formattedValues: null,
    
    deSerialise: function Xrm_Sdk_Entity$deSerialise(entityNode) {
        this.logicalName = Xrm.Sdk.XmlHelper.selectSingleNodeValue(entityNode, 'LogicalName');
        this.id = Xrm.Sdk.XmlHelper.selectSingleNodeValue(entityNode, 'Id');
        var attributes = Xrm.Sdk.XmlHelper.selectSingleNode(entityNode, 'Attributes');
        var attributeCount = attributes.childNodes.length;
        for (var i = 0; i < attributeCount; i++) {
            var node = attributes.childNodes[i];
            try {
                var attributeName = Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'key');
                var attributeValue = Xrm.Sdk.Attribute.deSerialise(Xrm.Sdk.XmlHelper.selectSingleNode(node, 'value'), null);
                this._attributes[attributeName] = attributeValue;
                this._setDictionaryValue(attributeName, attributeValue);
            }
            catch (e) {
                throw new Error('Invalid Attribute Value :' + Xrm.Sdk.XmlHelper.getNodeTextValue(node) + ':' + e.message);
            }
        }
        var formattedValues = Xrm.Sdk.XmlHelper.selectSingleNode(entityNode, 'FormattedValues');
        if (formattedValues != null) {
            for (var i = 0; i < formattedValues.childNodes.length; i++) {
                var node = formattedValues.childNodes[i];
                var key = Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'key');
                var value = Xrm.Sdk.XmlHelper.selectSingleNodeValue(node, 'value');
                this._setDictionaryValue(key + 'name', value);
                this.formattedValues[key + 'name'] = value;
                var att = this._attributes[key];
                if (att != null) {
                    att.name=value;
                }
            }
        }
    },
    
    _setDictionaryValue: function Xrm_Sdk_Entity$_setDictionaryValue(key, value) {
        var self = this;
        var thisAsDictionary = Type.safeCast(self, Object);
        thisAsDictionary[key] = value;
    },
    
    serialise: function Xrm_Sdk_Entity$serialise(ommitRoot) {
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
            if (typeof(record[key])!="function" && Object.prototype.hasOwnProperty.call(this, key) && !Xrm.StringEx.IN(key, [ 'id', 'logicalName', 'entityState', 'formattedValues' ]) && !key.startsWith('$') && !key.startsWith('_')) {
                var attributeValue = record[key];
                if (!Object.keyExists(this.formattedValues, key)) {
                    xml += Xrm.Sdk.Attribute.serialise(key, attributeValue, this._metaData);
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
    
    setAttributeValue: function Xrm_Sdk_Entity$setAttributeValue(name, value) {
        this._attributes[name] = value;
        this._setDictionaryValue(name, value);
    },
    
    getAttributeValue: function Xrm_Sdk_Entity$getAttributeValue(attributeName) {
        return this[attributeName];
    },
    
    getAttributeValueOptionSet: function Xrm_Sdk_Entity$getAttributeValueOptionSet(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueGuid: function Xrm_Sdk_Entity$getAttributeValueGuid(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueInt: function Xrm_Sdk_Entity$getAttributeValueInt(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueFloat: function Xrm_Sdk_Entity$getAttributeValueFloat(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueString: function Xrm_Sdk_Entity$getAttributeValueString(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    getAttributeValueEntityReference: function Xrm_Sdk_Entity$getAttributeValueEntityReference(attributeName) {
        return this.getAttributeValue(attributeName);
    },
    
    raisePropertyChanged: function Xrm_Sdk_Entity$raisePropertyChanged(propertyName) {
        var args = {};
        args.propertyName = propertyName;
        if (this.__propertyChanged != null) {
            this.__propertyChanged(this, args);
        }
        if (propertyName !== 'EntityState' && !this.entityState && this.entityState !== 1) {
            this.entityState = 2;
        }
    },
    
    toEntityReference: function Xrm_Sdk_Entity$toEntityReference() {
        return new Xrm.Sdk.EntityReference(new Xrm.Sdk.Guid(this.id), this.logicalName, '');
    },
    
    add_propertyChanged: function Xrm_Sdk_Entity$add_propertyChanged(value) {
        this.__propertyChanged = ss.Delegate.combine(this.__propertyChanged, value);
    },
    remove_propertyChanged: function Xrm_Sdk_Entity$remove_propertyChanged(value) {
        this.__propertyChanged = ss.Delegate.remove(this.__propertyChanged, value);
    },
    
    __propertyChanged: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.EntityCollection

Xrm.Sdk.EntityCollection = function Xrm_Sdk_EntityCollection(entities) {
    this._entities = new Xrm.Sdk.DataCollectionOfEntity(entities);
}
Xrm.Sdk.EntityCollection.prototype = {
    _entities: null,
    
    get_entities: function Xrm_Sdk_EntityCollection$get_entities() {
        return this._entities;
    },
    set_entities: function Xrm_Sdk_EntityCollection$set_entities(value) {
        this._entities = value;
        return value;
    },
    
    _entityName: null,
    
    get_entityName: function Xrm_Sdk_EntityCollection$get_entityName() {
        return this._entityName;
    },
    set_entityName: function Xrm_Sdk_EntityCollection$set_entityName(value) {
        this._entityName = value;
        return value;
    },
    
    _minActiveRowVersion: null,
    
    get_minActiveRowVersion: function Xrm_Sdk_EntityCollection$get_minActiveRowVersion() {
        return this._minActiveRowVersion;
    },
    set_minActiveRowVersion: function Xrm_Sdk_EntityCollection$set_minActiveRowVersion(value) {
        this._minActiveRowVersion = value;
        return value;
    },
    
    _moreRecords: false,
    
    get_moreRecords: function Xrm_Sdk_EntityCollection$get_moreRecords() {
        return this._moreRecords;
    },
    set_moreRecords: function Xrm_Sdk_EntityCollection$set_moreRecords(value) {
        this._moreRecords = value;
        return value;
    },
    
    _pagingCookie: null,
    
    get_pagingCookie: function Xrm_Sdk_EntityCollection$get_pagingCookie() {
        return this._pagingCookie;
    },
    set_pagingCookie: function Xrm_Sdk_EntityCollection$set_pagingCookie(value) {
        this._pagingCookie = value;
        return value;
    },
    
    _totalRecordCount: 0,
    
    get_totalRecordCount: function Xrm_Sdk_EntityCollection$get_totalRecordCount() {
        return this._totalRecordCount;
    },
    set_totalRecordCount: function Xrm_Sdk_EntityCollection$set_totalRecordCount(value) {
        this._totalRecordCount = value;
        return value;
    },
    
    _totalRecordCountLimitExceeded: false,
    
    get_totalRecordCountLimitExceeded: function Xrm_Sdk_EntityCollection$get_totalRecordCountLimitExceeded() {
        return this._totalRecordCountLimitExceeded;
    },
    set_totalRecordCountLimitExceeded: function Xrm_Sdk_EntityCollection$set_totalRecordCountLimitExceeded(value) {
        this._totalRecordCountLimitExceeded = value;
        return value;
    },
    get_item: function Xrm_Sdk_EntityCollection$get_item(index) {
        return this.get_entities().get_item(index);
    },
    set_item: function Xrm_Sdk_EntityCollection$set_item(index, value) {
        this.get_entities().set_item(index, value);
        return value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.EntityReference

Xrm.Sdk.EntityReference = function Xrm_Sdk_EntityReference(Id, LogicalName, Name) {
    this.id = Id;
    this.logicalName = LogicalName;
    this.name = Name;
}
Xrm.Sdk.EntityReference.prototype = {
    name: null,
    id: null,
    logicalName: null,
    
    toString: function Xrm_Sdk_EntityReference$toString() {
        return String.format('[EntityReference: {0},{1},{2}]', this.name, this.id, this.logicalName);
    },
    
    toSoap: function Xrm_Sdk_EntityReference$toSoap(NameSpace) {
        if (NameSpace == null || !NameSpace) {
            NameSpace = 'a';
        }
        return String.format('<{0}:EntityReference><{0}:Id>{1}</{0}:Id><{0}:LogicalName>{2}</{0}:LogicalName><{0}:Name i:nil="true" /></{0}:EntityReference>', NameSpace, this.id.value, this.logicalName);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Guid

Xrm.Sdk.Guid = function Xrm_Sdk_Guid(Value) {
    this.value = Value;
}
Xrm.Sdk.Guid.prototype = {
    value: null,
    
    toString: function Xrm_Sdk_Guid$toString() {
        return this.value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Money

Xrm.Sdk.Money = function Xrm_Sdk_Money(value) {
    this.value = value;
}
Xrm.Sdk.Money.prototype = {
    value: 0
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.OptionSetValue

Xrm.Sdk.OptionSetValue = function Xrm_Sdk_OptionSetValue(value) {
    this.value = value;
}
Xrm.Sdk.OptionSetValue.parse = function Xrm_Sdk_OptionSetValue$parse(value) {
    if (String.isNullOrEmpty(value)) {
        return new Xrm.Sdk.OptionSetValue(null);
    }
    else {
        return new Xrm.Sdk.OptionSetValue(parseInt(value));
    }
}
Xrm.Sdk.OptionSetValue.prototype = {
    name: null,
    value: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.OrganizationServiceProxy

Xrm.Sdk.OrganizationServiceProxy = function Xrm_Sdk_OrganizationServiceProxy() {
}
Xrm.Sdk.OrganizationServiceProxy.getUserSettings = function Xrm_Sdk_OrganizationServiceProxy$getUserSettings() {
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings == null) {
        Xrm.Sdk.OrganizationServiceProxy.userSettings = Xrm.Sdk.OrganizationServiceProxy.retrieve(Xrm.Sdk.UserSettings.entityLogicalName, Xrm.Page.context.getUserId(), [ 'AllColumns' ]);
    }
    Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring = Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring.replaceAll(':', Xrm.Sdk.OrganizationServiceProxy.userSettings.timeseparator);
    Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring.replaceAll('/', Xrm.Sdk.OrganizationServiceProxy.userSettings.dateseparator);
    Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring.replaceAll('MM', 'mm').replaceAll('yyyy', 'UU').replaceAll('yy', 'y').replaceAll('UU', 'yy').replaceAll('M', 'm');
    return Xrm.Sdk.OrganizationServiceProxy.userSettings;
}
Xrm.Sdk.OrganizationServiceProxy.doesNNAssociationExist = function Xrm_Sdk_OrganizationServiceProxy$doesNNAssociationExist(relationship, Entity1, Entity2) {
    var fetchXml = "<fetch mapping='logical'>" + "  <entity name='" + relationship.schemaName + "'>" + '    <all-attributes />' + '    <filter>' + "      <condition attribute='" + Entity1.logicalName + "id' operator='eq' value ='" + Entity1.id.value + "' />" + "      <condition attribute='" + Entity2.logicalName + "id' operator='eq' value='" + Entity2.id.value + "' />" + '    </filter>' + '  </entity>' + '</fetch>';
    var result = Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchXml);
    if (result.get_entities().get_count() > 0) {
        return true;
    }
    return false;
}
Xrm.Sdk.OrganizationServiceProxy.associate = function Xrm_Sdk_OrganizationServiceProxy$associate(entityName, entityId, relationship, relatedEntities) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getAssociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', null);
    delete resultXml;
    resultXml = null;
}
Xrm.Sdk.OrganizationServiceProxy.beginAssociate = function Xrm_Sdk_OrganizationServiceProxy$beginAssociate(entityName, entityId, relationship, relatedEntities, callBack) {
    Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getAssociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endAssociate = function Xrm_Sdk_OrganizationServiceProxy$endAssociate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy._getAssociateRequest = function Xrm_Sdk_OrganizationServiceProxy$_getAssociateRequest(entityName, entityId, relationship, relatedEntities) {
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
Xrm.Sdk.OrganizationServiceProxy.disassociate = function Xrm_Sdk_OrganizationServiceProxy$disassociate(entityName, entityId, relationship, relatedEntities) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getDisassociateRequest(entityName, entityId, relationship, relatedEntities), 'Execute', null);
    delete resultXml;
    resultXml = null;
}
Xrm.Sdk.OrganizationServiceProxy._getDisassociateRequest = function Xrm_Sdk_OrganizationServiceProxy$_getDisassociateRequest(entityName, entityId, relationship, relatedEntities) {
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
Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple = function Xrm_Sdk_OrganizationServiceProxy$retrieveMultiple(fetchXml) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getRetrieveMultipleRequest(fetchXml), 'RetrieveMultiple', null);
    var entities = Xrm.Sdk.OrganizationServiceProxy._getEntityCollectionResults(resultXml, Xrm.Sdk.Entity);
    delete resultXml;
    resultXml = null;
    return entities;
}
Xrm.Sdk.OrganizationServiceProxy._getRetrieveMultipleRequest = function Xrm_Sdk_OrganizationServiceProxy$_getRetrieveMultipleRequest(fetchXml) {
    var xml = '<RetrieveMultiple xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><query i:type="a:FetchExpression" ><a:Query>';
    xml += Xrm.Sdk.XmlHelper.encode(fetchXml);
    xml += '</a:Query></query></RetrieveMultiple>';
    return xml;
}
Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple = function Xrm_Sdk_OrganizationServiceProxy$beginRetrieveMultiple(fetchXml, callBack) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getRetrieveMultipleRequest(fetchXml), 'RetrieveMultiple', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple = function Xrm_Sdk_OrganizationServiceProxy$endRetrieveMultiple(asyncState, entityType) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        var entities = Xrm.Sdk.OrganizationServiceProxy._getEntityCollectionResults(xmlDocument, entityType);
        return entities;
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy._getEntityCollectionResults = function Xrm_Sdk_OrganizationServiceProxy$_getEntityCollectionResults(resultXml, entityType) {
    var soapBody = resultXml.firstChild.firstChild;
    var resultNode = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(soapBody, 'RetrieveMultipleResult');
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(resultNode, 'Entities');
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
    var entities = new Xrm.Sdk.EntityCollection(businessEntities);
    entities.set_moreRecords(Xrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'MoreRecords') === 'true');
    entities.set_pagingCookie(Xrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'PagingCookie'));
    entities.set_totalRecordCount(parseInt(Xrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'TotalRecordCount')));
    entities.set_totalRecordCountLimitExceeded(Xrm.Sdk.XmlHelper.selectSingleNodeValue(resultNode, 'TotalRecordCountLimitExceeded') === 'true');
    return entities;
}
Xrm.Sdk.OrganizationServiceProxy.retrieve = function Xrm_Sdk_OrganizationServiceProxy$retrieve(entityName, entityId, attributesList) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getRetrieveRequest(entityName, entityId, attributesList), 'Retrieve', null);
    var entityNode = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(resultXml, 'RetrieveResult');
    var entity = new Xrm.Sdk.Entity(entityName);
    entity.deSerialise(entityNode);
    delete resultXml;
    resultXml = null;
    return entity;
}
Xrm.Sdk.OrganizationServiceProxy.beginRetrieve = function Xrm_Sdk_OrganizationServiceProxy$beginRetrieve(entityName, entityId, attributesList, callBack) {
    Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getRetrieveRequest(entityName, entityId, attributesList), 'Retrieve', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endRetrieve = function Xrm_Sdk_OrganizationServiceProxy$endRetrieve(asyncState, entityType) {
    var resultXml = asyncState;
    var entityNode = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(resultXml, 'RetrieveResult');
    var entity = new Xrm.Sdk.Entity(null);
    entity.deSerialise(entityNode);
    return entity;
}
Xrm.Sdk.OrganizationServiceProxy._getRetrieveRequest = function Xrm_Sdk_OrganizationServiceProxy$_getRetrieveRequest(entityName, entityId, attributesList) {
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
Xrm.Sdk.OrganizationServiceProxy.create = function Xrm_Sdk_OrganizationServiceProxy$create(entity) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getCreateRequest(entity), 'Create', null);
    var newGuid = Xrm.Sdk.XmlHelper.selectSingleNodeValueDeep(resultXml, 'CreateResult');
    delete resultXml;
    resultXml = null;
    return new Xrm.Sdk.Guid(newGuid);
}
Xrm.Sdk.OrganizationServiceProxy._getCreateRequest = function Xrm_Sdk_OrganizationServiceProxy$_getCreateRequest(entity) {
    var xml = '<Create xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entity>';
    xml += entity.serialise(true);
    xml += '</entity></Create>';
    return xml;
}
Xrm.Sdk.OrganizationServiceProxy.beginCreate = function Xrm_Sdk_OrganizationServiceProxy$beginCreate(entity, callBack) {
    Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getCreateRequest(entity), 'Create', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endCreate = function Xrm_Sdk_OrganizationServiceProxy$endCreate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        var newGuid = Xrm.Sdk.XmlHelper.selectSingleNodeValueDeep(xmlDocument, 'CreateResult');
        return new Xrm.Sdk.Guid(newGuid);
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy.setState = function Xrm_Sdk_OrganizationServiceProxy$setState(id, entityName, stateCode, statusCode) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getSetStateRequest(id, entityName, stateCode, statusCode), 'Execute', null);
    delete resultXml;
    resultXml = null;
}
Xrm.Sdk.OrganizationServiceProxy.beginSetState = function Xrm_Sdk_OrganizationServiceProxy$beginSetState(id, entityName, stateCode, statusCode, callBack) {
    Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getSetStateRequest(id, entityName, stateCode, statusCode), 'Execute', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endSetState = function Xrm_Sdk_OrganizationServiceProxy$endSetState(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy._getSetStateRequest = function Xrm_Sdk_OrganizationServiceProxy$_getSetStateRequest(id, entityName, stateCode, statusCode) {
    return String.format('<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services">' + '<request i:type="b:SetStateRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '<a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<c:key>EntityMoniker</c:key>' + '<c:value i:type="a:EntityReference">' + '<a:Id>{0}</a:Id>' + '<a:LogicalName>{1}</a:LogicalName>' + '<a:Name i:nil="true" />' + '</c:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<c:key>State</c:key>' + '<c:value i:type="a:OptionSetValue">' + '<a:Value>{2}</a:Value>' + '</c:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<c:key>Status</c:key>' + '<c:value i:type="a:OptionSetValue">' + '<a:Value>{3}</a:Value>' + '</c:value>' + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>SetState</a:RequestName>' + '</request></Execute>', id.value, entityName, stateCode, statusCode);
}
Xrm.Sdk.OrganizationServiceProxy.delete_ = function Xrm_Sdk_OrganizationServiceProxy$delete_(entityName, id) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getDeleteRequest(entityName, id), 'Delete', null);
    var newGuid = Xrm.Sdk.XmlHelper.selectSingleNodeValueDeep(resultXml, 'DeleteResult');
    delete resultXml;
    resultXml = null;
    return newGuid;
}
Xrm.Sdk.OrganizationServiceProxy._getDeleteRequest = function Xrm_Sdk_OrganizationServiceProxy$_getDeleteRequest(entityName, id) {
    var xml = String.format('<Delete xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entityName>{0}</entityName><id>{1}</id></Delete>', entityName, id.value);
    return xml;
}
Xrm.Sdk.OrganizationServiceProxy.beginDelete = function Xrm_Sdk_OrganizationServiceProxy$beginDelete(entityName, id, callBack) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getDeleteRequest(entityName, id), 'Delete', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endDelete = function Xrm_Sdk_OrganizationServiceProxy$endDelete(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        return;
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy.update = function Xrm_Sdk_OrganizationServiceProxy$update(entity) {
    var xml = '<Update xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entity>';
    xml += entity.serialise(true);
    xml += '</entity></Update>';
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(xml, 'Update', null);
    delete resultXml;
    resultXml = null;
}
Xrm.Sdk.OrganizationServiceProxy.beginUpdate = function Xrm_Sdk_OrganizationServiceProxy$beginUpdate(entity, callBack) {
    var xml = '<Update xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services" ><entity>';
    xml += entity.serialise(true);
    xml += '</entity></Update>';
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(xml, 'Update', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endUpdate = function Xrm_Sdk_OrganizationServiceProxy$endUpdate(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        return;
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy.execute = function Xrm_Sdk_OrganizationServiceProxy$execute(request) {
    var resultXml = Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getExecuteRequest(request), 'Execute', null);
    return Xrm.Sdk.OrganizationServiceProxy.endExecute(resultXml);
}
Xrm.Sdk.OrganizationServiceProxy._getExecuteRequest = function Xrm_Sdk_OrganizationServiceProxy$_getExecuteRequest(request) {
    var xml = '<Execute xmlns="http://schemas.microsoft.com/xrm/2011/Contracts/Services">';
    xml += request.serialise();
    xml += '</Execute>';
    return xml;
}
Xrm.Sdk.OrganizationServiceProxy.beginExecute = function Xrm_Sdk_OrganizationServiceProxy$beginExecute(request, callBack) {
    Xrm.Sdk.OrganizationServiceProxy._getResponse(Xrm.Sdk.OrganizationServiceProxy._getExecuteRequest(request), 'Execute', callBack);
}
Xrm.Sdk.OrganizationServiceProxy.endExecute = function Xrm_Sdk_OrganizationServiceProxy$endExecute(asyncState) {
    var xmlDocument = asyncState;
    if (xmlDocument.childNodes != null) {
        var response = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(xmlDocument, 'ExecuteResult');
        var type = Xrm.Sdk.XmlHelper.selectSingleNodeValue(response, 'ResponseName');
        switch (type) {
            case 'RetrieveAttribute':
                return new Xrm.Sdk.Messages.RetrieveAttributeResponse(response);
            case 'RetrieveAllEntities':
                return new Xrm.Sdk.Messages.RetrieveAllEntitiesResponse(response);
            case 'RetrieveEntity':
                return new Xrm.Sdk.Messages.RetrieveEntityResponse(response);
            case 'BulkDeleteResponse':
                return new Xrm.Sdk.Messages.BulkDeleteResponse(response);
            case 'FetchXmlToQueryExpression':
                return new Xrm.Sdk.Messages.FetchXmlToQueryExpressionResponse(response);
            case 'RetrieveMetadataChanges':
                return new Xrm.Sdk.Messages.RetrieveMetadataChangesResponse(response);
        }
        return null;
    }
    else {
        throw new Error(asyncState);
    }
}
Xrm.Sdk.OrganizationServiceProxy._getSoapEnvelope = function Xrm_Sdk_OrganizationServiceProxy$_getSoapEnvelope(payLoadXml) {
    var xml = '<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" xmlns:d="http://schemas.microsoft.com/xrm/2011/Contracts/Services"  xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<s:Body>' + payLoadXml + '</s:Body>' + '</s:Envelope>';
    return xml;
}
Xrm.Sdk.OrganizationServiceProxy._getServerUrl = function Xrm_Sdk_OrganizationServiceProxy$_getServerUrl() {
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
Xrm.Sdk.OrganizationServiceProxy._getResponse = function Xrm_Sdk_OrganizationServiceProxy$_getResponse(soapXmlPacket, action, asyncCallback) {
    var isAsync = (asyncCallback != null);
    var xml = Xrm.Sdk.OrganizationServiceProxy._getSoapEnvelope(soapXmlPacket);
    var msg = null;
    var xmlHttpRequest = new XMLHttpRequest();
    xmlHttpRequest.open('POST', Xrm.Sdk.OrganizationServiceProxy._getServerUrl() + '/XRMServices/2011/Organization.svc/web', isAsync);
    xmlHttpRequest.setRequestHeader('SOAPAction', 'http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/' + action);
    xmlHttpRequest.setRequestHeader('Content-Type', 'text/xml; charset=utf-8');
    xmlHttpRequest.withCredentials = true;;
    if (isAsync) {
        xmlHttpRequest.onreadystatechange = function() {
            if (xmlHttpRequest == null) {
                return;
            }
            if (xmlHttpRequest.readyState === 4) {
                var resultXml = xmlHttpRequest.responseXML;
                var errorMsg = null;
                if (xmlHttpRequest.status !== 200) {
                    errorMsg = Xrm.Sdk.OrganizationServiceProxy._getSoapFault(resultXml);
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
            msg = Xrm.Sdk.OrganizationServiceProxy._getSoapFault(resultXml);
        }
        delete xmlHttpRequest;;
        xmlHttpRequest = null;
        if (msg != null) {
            throw new Error("CRM SDK Call returned error: '" + msg + "'");
        }
        else {
            return resultXml;
        }
    }
}
Xrm.Sdk.OrganizationServiceProxy._getSoapFault = function Xrm_Sdk_OrganizationServiceProxy$_getSoapFault(response) {
    var errorMsg = null;
    if (response == null || response.firstChild.nodeName !== 's:Envelope') {
        return 'No SOAP Envelope in response';
    }
    var soapResponseBody = response.firstChild.firstChild;
    var errorNode = Xrm.Sdk.XmlHelper.selectSingleNode(soapResponseBody, 'Fault');
    if (errorNode != null) {
        var detailMessageNode = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(errorNode, 'Message');
        if (detailMessageNode != null) {
            errorMsg = Xrm.Sdk.XmlHelper.getNodeTextValue(detailMessageNode);
        }
        else {
            var faultMessage = Xrm.Sdk.XmlHelper.selectSingleNode(errorNode, 'faultstring');
            if (faultMessage != null) {
                errorMsg = Xrm.Sdk.XmlHelper.getNodeTextValue(faultMessage);
            }
        }
    }
    return errorMsg;
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Relationship

Xrm.Sdk.Relationship = function Xrm_Sdk_Relationship(schemaName) {
    this.schemaName = schemaName;
}
Xrm.Sdk.Relationship.prototype = {
    schemaName: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.XmlHelper

Xrm.Sdk.XmlHelper = function Xrm_Sdk_XmlHelper() {
}
Xrm.Sdk.XmlHelper.encode = function Xrm_Sdk_XmlHelper$encode(value) {
    if (value == null) {
        return value;
    }
    return value.replace(new RegExp('([\\&"<>])', 'g'), Xrm.Sdk.XmlHelper.replaceCallBackEncode);
}
Xrm.Sdk.XmlHelper.serialiseNode = function Xrm_Sdk_XmlHelper$serialiseNode(node) {
    if (typeof(node.xml) === 'undefined') {
        return new XMLSerializer().serializeToString(node);
    }
    else {
        return node.xml;
    }
}
Xrm.Sdk.XmlHelper.Decode = function Xrm_Sdk_XmlHelper$Decode(value) {
    if (value == null) {
        return null;
    }
    return value.replace(new RegExp('(&quot;|&lt;|&gt;|&amp;)', 'g'), Xrm.Sdk.XmlHelper.replaceCallBackDecode);
}
Xrm.Sdk.XmlHelper.replaceCallBackEncode = function Xrm_Sdk_XmlHelper$replaceCallBackEncode(item) {
    return Xrm.Sdk.XmlHelper._encode_map[item];
}
Xrm.Sdk.XmlHelper.replaceCallBackDecode = function Xrm_Sdk_XmlHelper$replaceCallBackDecode(item) {
    return Xrm.Sdk.XmlHelper._decode_map[item];
}
Xrm.Sdk.XmlHelper.selectSingleNodeValue = function Xrm_Sdk_XmlHelper$selectSingleNodeValue(doc, baseName) {
    var node = Xrm.Sdk.XmlHelper.selectSingleNode(doc, baseName);
    if (node != null) {
        return Xrm.Sdk.XmlHelper.getNodeTextValue(node);
    }
    else {
        return null;
    }
}
Xrm.Sdk.XmlHelper.selectSingleNode = function Xrm_Sdk_XmlHelper$selectSingleNode(doc, baseName) {
    var $enum1 = ss.IEnumerator.getEnumerator(doc.childNodes);
    while ($enum1.moveNext()) {
        var n = $enum1.current;
        if (Xrm.Sdk.XmlHelper.getLocalName(n) === baseName) {
            return n;
        }
    }
    return null;
}
Xrm.Sdk.XmlHelper.getLocalName = function Xrm_Sdk_XmlHelper$getLocalName(node) {
    if (node.baseName != null) {
        return node.baseName;
    }
    else {
        return node.localName;
    }
}
Xrm.Sdk.XmlHelper.selectSingleNodeValueDeep = function Xrm_Sdk_XmlHelper$selectSingleNodeValueDeep(doc, baseName) {
    var node = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(doc, baseName);
    if (node != null) {
        return Xrm.Sdk.XmlHelper.getNodeTextValue(node);
    }
    else {
        return null;
    }
}
Xrm.Sdk.XmlHelper.selectSingleNodeDeep = function Xrm_Sdk_XmlHelper$selectSingleNodeDeep(doc, baseName) {
    var $enum1 = ss.IEnumerator.getEnumerator(doc.childNodes);
    while ($enum1.moveNext()) {
        var n = $enum1.current;
        if (Xrm.Sdk.XmlHelper.getLocalName(n) === baseName) {
            return n;
        }
        var resultDeep = Xrm.Sdk.XmlHelper.selectSingleNodeDeep(n, baseName);
        if (resultDeep != null) {
            return resultDeep;
        }
    }
    return null;
}
Xrm.Sdk.XmlHelper.nsResolver = function Xrm_Sdk_XmlHelper$nsResolver(prefix) {
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
Xrm.Sdk.XmlHelper.isSelectSingleNodeUndefined = function Xrm_Sdk_XmlHelper$isSelectSingleNodeUndefined(value) {
    return typeof (value.selectSingleNode) === 'undefined';
}
Xrm.Sdk.XmlHelper.loadXml = function Xrm_Sdk_XmlHelper$loadXml(xml) {
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
Xrm.Sdk.XmlHelper.selectSingleNodeXpath = function Xrm_Sdk_XmlHelper$selectSingleNodeXpath(node, xpath) {
    if (!Xrm.Sdk.XmlHelper.isSelectSingleNodeUndefined(node)) {
        return node.selectSingleNode(xpath);
    }
    else {
        var xpe = new XPathEvaluator();
        var xPathNode = xpe.evaluate(xpath, node, Xrm.Sdk.XmlHelper.nsResolver, 9, null);
        return (xPathNode != null) ? xPathNode.singleNodeValue : null;
    }
}
Xrm.Sdk.XmlHelper.getNodeTextValue = function Xrm_Sdk_XmlHelper$getNodeTextValue(node) {
    if ((node != null) && (node.firstChild != null)) {
        return node.firstChild.nodeValue;
    }
    else {
        return null;
    }
}
Xrm.Sdk.XmlHelper.getAttributeValue = function Xrm_Sdk_XmlHelper$getAttributeValue(node, attributeName) {
    var attribute = node.attributes.getNamedItem(attributeName);
    if (attribute != null) {
        return attribute.nodeValue;
    }
    else {
        return null;
    }
}


Type.registerNamespace('Xrm.Sdk.Messages');

////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.EntityFilters

Xrm.Sdk.Messages.EntityFilters = function() { };
Xrm.Sdk.Messages.EntityFilters.prototype = {
    default_: 1, 
    entity: 1, 
    attributes: 2, 
    privileges: 4, 
    relationships: 8, 
    all: 15
}
Xrm.Sdk.Messages.EntityFilters.registerEnum('Xrm.Sdk.Messages.EntityFilters', true);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.BulkDeleteRequest

Xrm.Sdk.Messages.BulkDeleteRequest = function Xrm_Sdk_Messages_BulkDeleteRequest() {
}
Xrm.Sdk.Messages.BulkDeleteRequest.prototype = {
    
    serialise: function Xrm_Sdk_Messages_BulkDeleteRequest$serialise() {
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
        return String.format('<request i:type="b:BulkDeleteRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '        <a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>QuerySet</c:key>' + '            <c:value i:type="a:ArrayOfQueryExpression">' + '              <a:QueryExpression>' + this.querySet + '              </a:QueryExpression>' + '            </c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>JobName</c:key>' + '            <c:value i:type="d:string" xmlns:d="http://www.w3.org/2001/XMLSchema">' + this.jobName + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>SendEmailNotification</c:key>' + '            <c:value i:type="d:boolean" xmlns:d="http://www.w3.org/2001/XMLSchema">' + this.sendEmailNotification.toString() + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>ToRecipients</c:key>' + '            <c:value i:type="d:ArrayOfguid" xmlns:d="http://schemas.microsoft.com/2003/10/Serialization/Arrays">' + recipientsXml + '            </c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>CCRecipients</c:key>' + '            <c:value i:type="d:ArrayOfguid" xmlns:d="http://schemas.microsoft.com/2003/10/Serialization/Arrays">' + ccRecipientsXml + '            </c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>RecurrencePattern</c:key>' + '            <c:value i:type="d:string" xmlns:d="http://www.w3.org/2001/XMLSchema" >' + this.recurrencePattern + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>StartDateTime</c:key>' + '            <c:value i:type="d:dateTime" xmlns:d="http://www.w3.org/2001/XMLSchema">' + Xrm.Sdk.DateTimeEx.toXrmStringUTC(Xrm.Sdk.DateTimeEx.localTimeToUTCFromSettings(this.startDateTime, Xrm.Sdk.OrganizationServiceProxy.getUserSettings())) + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '        </a:Parameters>' + '        <a:RequestId i:nil="true" />' + '        <a:RequestName>BulkDelete</a:RequestName>' + '      </request>');
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
// Xrm.Sdk.Messages.BulkDeleteResponse

Xrm.Sdk.Messages.BulkDeleteResponse = function Xrm_Sdk_Messages_BulkDeleteResponse(response) {
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.FetchXmlToQueryExpressionRequest

Xrm.Sdk.Messages.FetchXmlToQueryExpressionRequest = function Xrm_Sdk_Messages_FetchXmlToQueryExpressionRequest() {
}
Xrm.Sdk.Messages.FetchXmlToQueryExpressionRequest.prototype = {
    fetchXml: null,
    
    serialise: function Xrm_Sdk_Messages_FetchXmlToQueryExpressionRequest$serialise() {
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
        return String.format(requestXml, Xrm.Sdk.XmlHelper.encode(this.fetchXml));
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.FetchXmlToQueryExpressionResponse

Xrm.Sdk.Messages.FetchXmlToQueryExpressionResponse = function Xrm_Sdk_Messages_FetchXmlToQueryExpressionResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (Xrm.Sdk.XmlHelper.getNodeTextValue(key) === 'Query') {
            var queryNode = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            var queryXml = Xrm.Sdk.XmlHelper.serialiseNode(queryNode).substr(165);
            queryXml = queryXml.substr(0, queryXml.length - 10);
            this.query = queryXml;
        }
    }
}
Xrm.Sdk.Messages.FetchXmlToQueryExpressionResponse.prototype = {
    query: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveAllEntitiesRequest

Xrm.Sdk.Messages.RetrieveAllEntitiesRequest = function Xrm_Sdk_Messages_RetrieveAllEntitiesRequest() {
}
Xrm.Sdk.Messages.RetrieveAllEntitiesRequest.prototype = {
    
    serialise: function Xrm_Sdk_Messages_RetrieveAllEntitiesRequest$serialise() {
        return '\r\n                              <request i:type="a:RetrieveAllEntitiesRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">\r\n                                <a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">\r\n                                  <a:KeyValuePairOfstringanyType>\r\n                                    <b:key>EntityFilters</b:key>\r\n                                    <b:value i:type="c:EntityFilters" xmlns:c="http://schemas.microsoft.com/xrm/2011/Metadata">Entity</b:value>\r\n                                  </a:KeyValuePairOfstringanyType>\r\n                                  <a:KeyValuePairOfstringanyType>\r\n                                    <b:key>RetrieveAsIfPublished</b:key>\r\n                                    <b:value i:type="c:boolean" xmlns:c="http://www.w3.org/2001/XMLSchema">true</b:value>\r\n                                  </a:KeyValuePairOfstringanyType>\r\n                                </a:Parameters>\r\n                                <a:RequestId i:nil="true" />\r\n                                <a:RequestName>RetrieveAllEntities</a:RequestName>\r\n                              </request>\r\n                            ';
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveAllEntitiesResponse

Xrm.Sdk.Messages.RetrieveAllEntitiesResponse = function Xrm_Sdk_Messages_RetrieveAllEntitiesResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (Xrm.Sdk.XmlHelper.getNodeTextValue(key) === 'EntityMetadata') {
            var values = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.entityMetadata = new Array(values.childNodes.length);
            for (var i = 0; i < values.childNodes.length; i++) {
                var entity = values.childNodes[i];
                var metaData = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseEntityMetadata({}, entity);
                this.entityMetadata[i] = metaData;
            }
        }
    }
}
Xrm.Sdk.Messages.RetrieveAllEntitiesResponse.prototype = {
    entityMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveAttributeRequest

Xrm.Sdk.Messages.RetrieveAttributeRequest = function Xrm_Sdk_Messages_RetrieveAttributeRequest() {
}
Xrm.Sdk.Messages.RetrieveAttributeRequest.prototype = {
    entityLogicalName: null,
    logicalName: null,
    retrieveAsIfPublished: false,
    
    serialise: function Xrm_Sdk_Messages_RetrieveAttributeRequest$serialise() {
        return String.format('<request i:type="a:RetrieveAttributeRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">' + '<a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<b:key>EntityLogicalName</b:key>' + '<b:value i:type="c:string" xmlns:c="http://www.w3.org/2001/XMLSchema">{0}</b:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>MetadataId</b:key>' + '<b:value i:type="ser:guid"  xmlns:ser="http://schemas.microsoft.com/2003/10/Serialization/">00000000-0000-0000-0000-000000000000</b:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>RetrieveAsIfPublished</b:key>' + '<b:value i:type="c:boolean" xmlns:c="http://www.w3.org/2001/XMLSchema">{2}</b:value>' + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>LogicalName</b:key>' + '<b:value i:type="c:string" xmlns:c="http://www.w3.org/2001/XMLSchema">{1}</b:value>' + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>RetrieveAttribute</a:RequestName>' + '</request>', this.entityLogicalName, this.logicalName, this.retrieveAsIfPublished);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveAttributeResponse

Xrm.Sdk.Messages.RetrieveAttributeResponse = function Xrm_Sdk_Messages_RetrieveAttributeResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var metaData = Xrm.Sdk.XmlHelper.selectSingleNode(results.firstChild, 'value');
    var type = Xrm.Sdk.XmlHelper.getAttributeValue(metaData, 'i:type');
    switch (type) {
        case 'c:PicklistAttributeMetadata':
            this.attributeMetadata = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialisePicklistAttributeMetadata({}, metaData);
            break;
    }
}
Xrm.Sdk.Messages.RetrieveAttributeResponse.prototype = {
    attributeMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveEntityRequest

Xrm.Sdk.Messages.RetrieveEntityRequest = function Xrm_Sdk_Messages_RetrieveEntityRequest() {
}
Xrm.Sdk.Messages.RetrieveEntityRequest.prototype = {
    entityFilters: 0,
    logicalName: null,
    metadataId: null,
    retrieveAsIfPublished: false,
    
    serialise: function Xrm_Sdk_Messages_RetrieveEntityRequest$serialise() {
        return '<request i:type="a:RetrieveEntityRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts">' + '<a:Parameters xmlns:b="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '<a:KeyValuePairOfstringanyType>' + '<b:key>EntityFilters</b:key>' + Xrm.Sdk.Attribute.serialiseValue(this.entityFilters, 'EntityFilters') + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>MetadataId</b:key>' + Xrm.Sdk.Attribute.serialiseValue(this.metadataId, null) + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>RetrieveAsIfPublished</b:key>' + Xrm.Sdk.Attribute.serialiseValue(this.retrieveAsIfPublished, null) + '</a:KeyValuePairOfstringanyType>' + '<a:KeyValuePairOfstringanyType>' + '<b:key>LogicalName</b:key>' + Xrm.Sdk.Attribute.serialiseValue(this.logicalName, null) + '</a:KeyValuePairOfstringanyType>' + '</a:Parameters>' + '<a:RequestId i:nil="true" />' + '<a:RequestName>RetrieveEntity</a:RequestName>' + '</request>';
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveEntityResponse

Xrm.Sdk.Messages.RetrieveEntityResponse = function Xrm_Sdk_Messages_RetrieveEntityResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (Xrm.Sdk.XmlHelper.getNodeTextValue(key) === 'EntityMetadata') {
            var entity = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.entityMetadata = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseEntityMetadata({}, entity);
        }
    }
}
Xrm.Sdk.Messages.RetrieveEntityResponse.prototype = {
    entityMetadata: null
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveMetadataChangesRequest

Xrm.Sdk.Messages.RetrieveMetadataChangesRequest = function Xrm_Sdk_Messages_RetrieveMetadataChangesRequest() {
}
Xrm.Sdk.Messages.RetrieveMetadataChangesRequest.prototype = {
    clientVersionStamp: null,
    deletedMetadataFilters: null,
    query: null,
    
    serialise: function Xrm_Sdk_Messages_RetrieveMetadataChangesRequest$serialise() {
        return "<request i:type='a:RetrieveMetadataChangesRequest' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts'>\r\n                <a:Parameters xmlns:b='http://schemas.datacontract.org/2004/07/System.Collections.Generic'>\r\n                  <a:KeyValuePairOfstringanyType>\r\n                    <b:key>ClientVersionStamp</b:key>" + Xrm.Sdk.Attribute.serialiseValue(this.clientVersionStamp, null) + '\r\n                  </a:KeyValuePairOfstringanyType>\r\n                  <a:KeyValuePairOfstringanyType>\r\n                    <b:key>Query</b:key>\r\n                    ' + Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseEntityQueryExpression(this.query) + "\r\n                  </a:KeyValuePairOfstringanyType>\r\n                </a:Parameters>\r\n                <a:RequestId i:nil='true' />\r\n                <a:RequestName>RetrieveMetadataChanges</a:RequestName>\r\n              </request>";
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Messages.RetrieveMetadataChangesResponse

Xrm.Sdk.Messages.RetrieveMetadataChangesResponse = function Xrm_Sdk_Messages_RetrieveMetadataChangesResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        var value = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
        switch (Xrm.Sdk.XmlHelper.getNodeTextValue(key)) {
            case 'ServerVersionStamp':
                this.serverVersionStamp = Xrm.Sdk.XmlHelper.getNodeTextValue(value);
                break;
            case 'DeletedMetadata':
                break;
            case 'EntityMetadata':
                this.entityMetadata = [];
                for (var i = 0; i < value.childNodes.length; i++) {
                    var entity = value.childNodes[i];
                    var metaData = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseEntityMetadata({}, entity);
                    this.entityMetadata.add(metaData);
                }
                break;
        }
    }
}
Xrm.Sdk.Messages.RetrieveMetadataChangesResponse.prototype = {
    entityMetadata: null,
    serverVersionStamp: null
}


Type.registerNamespace('Xrm.Sdk.Metadata');

////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.AttributeRequiredLevel

Xrm.Sdk.Metadata.AttributeRequiredLevel = function() { };
Xrm.Sdk.Metadata.AttributeRequiredLevel.prototype = {
    None: 'None', 
    SystemRequired: 'SystemRequired', 
    ApplicationRequired: 'ApplicationRequired', 
    Recommended: 'Recommended'
}
Xrm.Sdk.Metadata.AttributeRequiredLevel.registerEnum('Xrm.Sdk.Metadata.AttributeRequiredLevel', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.AttributeTypeCode

Xrm.Sdk.Metadata.AttributeTypeCode = function() { };
Xrm.Sdk.Metadata.AttributeTypeCode.prototype = {
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
Xrm.Sdk.Metadata.AttributeTypeCode.registerEnum('Xrm.Sdk.Metadata.AttributeTypeCode', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.DateTimeFormat

Xrm.Sdk.Metadata.DateTimeFormat = function() { };
Xrm.Sdk.Metadata.DateTimeFormat.prototype = {
    DateOnly: 'DateOnly', 
    DateAndTime: 'DateAndTime'
}
Xrm.Sdk.Metadata.DateTimeFormat.registerEnum('Xrm.Sdk.Metadata.DateTimeFormat', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.IntegerFormat

Xrm.Sdk.Metadata.IntegerFormat = function() { };
Xrm.Sdk.Metadata.IntegerFormat.prototype = {
    None: 'None', 
    Duration: 'Duration', 
    TimeZone: 'TimeZone', 
    Language: 'Language', 
    Locale: 'Locale'
}
Xrm.Sdk.Metadata.IntegerFormat.registerEnum('Xrm.Sdk.Metadata.IntegerFormat', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.OptionSetType

Xrm.Sdk.Metadata.OptionSetType = function() { };
Xrm.Sdk.Metadata.OptionSetType.prototype = {
    Picklist: 'Picklist', 
    State: 'State', 
    Status: 'Status', 
    Boolean: 'Boolean'
}
Xrm.Sdk.Metadata.OptionSetType.registerEnum('Xrm.Sdk.Metadata.OptionSetType', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.StringFormat

Xrm.Sdk.Metadata.StringFormat = function() { };
Xrm.Sdk.Metadata.StringFormat.prototype = {
    Email: 'Email', 
    Text: 'Text', 
    TextArea: 'TextArea', 
    Url: 'Url', 
    TickerSymbol: 'TickerSymbol', 
    PhoneticGuide: 'PhoneticGuide', 
    VersionNumber: 'VersionNumber', 
    Phone: 'Phone'
}
Xrm.Sdk.Metadata.StringFormat.registerEnum('Xrm.Sdk.Metadata.StringFormat', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.MetadataSerialiser

Xrm.Sdk.Metadata.MetadataSerialiser = function Xrm_Sdk_Metadata_MetadataSerialiser() {
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseAttributeMetadata = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialiseAttributeMetadata(item, attribute) {
    var $enum1 = ss.IEnumerator.getEnumerator(attribute.childNodes);
    while ($enum1.moveNext()) {
        var node = $enum1.current;
        var itemValues = item;
        var localName = Xrm.Sdk.XmlHelper.getLocalName(node);
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
                itemValues[fieldName] = Xrm.Sdk.XmlHelper.getNodeTextValue(node);
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
                itemValues[fieldName] = Xrm.Sdk.Attribute.deSerialise(node, 'boolean');
                break;
            case 'ColumnNumber':
            case 'Precision':
            case 'DefaultFormValue':
            case 'MaxLength':
            case 'PrecisionSource':
                itemValues[fieldName] = Xrm.Sdk.Attribute.deSerialise(node, 'int');
                break;
            case 'Description':
            case 'DisplayName':
                var label = {};
                itemValues[fieldName] = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLabel(label, node);
                break;
            case 'OptionSet':
                var options = {};
                itemValues[fieldName] = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseOptionSetMetadata(options, node);
                break;
            case 'AttributeType':
                item.attributeType = Xrm.Sdk.XmlHelper.getNodeTextValue(node);
                break;
        }
    }
    return item;
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseEntityMetadata = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialiseEntityMetadata(item, entity) {
    var $enum1 = ss.IEnumerator.getEnumerator(entity.childNodes);
    while ($enum1.moveNext()) {
        var node = $enum1.current;
        var itemValues = item;
        var localName = Xrm.Sdk.XmlHelper.getLocalName(node);
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
                itemValues[fieldName] = Xrm.Sdk.XmlHelper.getNodeTextValue(node);
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
                itemValues[fieldName] = Xrm.Sdk.Attribute.deSerialise(node, 'boolean');
                break;
            case 'ActivityTypeMask':
            case 'ObjectTypeCode':
                itemValues[fieldName] = Xrm.Sdk.Attribute.deSerialise(node, 'int');
                break;
            case 'Attributes':
                item.attributes = [];
                var $enum2 = ss.IEnumerator.getEnumerator(node.childNodes);
                while ($enum2.moveNext()) {
                    var childNode = $enum2.current;
                    var a = {};
                    item.attributes.add(Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseAttributeMetadata(a, childNode));
                }
                break;
            case 'Description':
            case 'DisplayCollectionName':
            case 'DisplayName':
                var label = {};
                itemValues[fieldName] = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLabel(label, node);
                break;
        }
    }
    return item;
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLabel = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialiseLabel(item, metaData) {
    item.localizedLabels = [];
    var labels = Xrm.Sdk.XmlHelper.selectSingleNode(metaData, 'LocalizedLabels');
    if (labels != null && labels.childNodes != null) {
        var $enum1 = ss.IEnumerator.getEnumerator(labels.childNodes);
        while ($enum1.moveNext()) {
            var label = $enum1.current;
            item.localizedLabels.add(Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLocalizedLabel({}, label));
        }
        item.userLocalizedLabel = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLocalizedLabel({}, Xrm.Sdk.XmlHelper.selectSingleNode(metaData, 'UserLocalizedLabel'));
    }
    return item;
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLocalizedLabel = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialiseLocalizedLabel(item, metaData) {
    item.label = Xrm.Sdk.XmlHelper.selectSingleNodeValue(metaData, 'Label');
    item.languageCode = parseInt(Xrm.Sdk.XmlHelper.selectSingleNodeValue(metaData, 'LanguageCode'));
    return item;
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseOptionMetadata = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialiseOptionMetadata(item, metaData) {
    item.value = parseInt(Xrm.Sdk.XmlHelper.selectSingleNodeValue(metaData, 'Value'));
    item.label = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseLabel({}, Xrm.Sdk.XmlHelper.selectSingleNode(metaData, 'Label'));
    return item;
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseOptionSetMetadata = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialiseOptionSetMetadata(item, metaData) {
    var options = Xrm.Sdk.XmlHelper.selectSingleNode(metaData, 'Options');
    if (options != null) {
        item.options = [];
        var $enum1 = ss.IEnumerator.getEnumerator(options.childNodes);
        while ($enum1.moveNext()) {
            var option = $enum1.current;
            item.options.add(Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseOptionMetadata({}, option));
        }
    }
    return item;
}
Xrm.Sdk.Metadata.MetadataSerialiser.deSerialisePicklistAttributeMetadata = function Xrm_Sdk_Metadata_MetadataSerialiser$deSerialisePicklistAttributeMetadata(item, metaData) {
    var options = Xrm.Sdk.XmlHelper.selectSingleNode(metaData, 'OptionSet');
    if (options != null) {
        item.optionSet = Xrm.Sdk.Metadata.MetadataSerialiser.deSerialiseOptionSetMetadata({}, options);
    }
    return item;
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.MetadataCache

Xrm.Sdk.Metadata.MetadataCache = function Xrm_Sdk_Metadata_MetadataCache() {
}
Xrm.Sdk.Metadata.MetadataCache.getOptionSetValues = function Xrm_Sdk_Metadata_MetadataCache$getOptionSetValues(entityLogicalName, attributeLogicalName, allowEmpty) {
    if (allowEmpty == null) {
        allowEmpty = false;
    }
    var cacheKey = entityLogicalName + '.' + attributeLogicalName + '.' + allowEmpty.toString();
    if (Object.keyExists(Xrm.Sdk.Metadata.MetadataCache._optionsCache, cacheKey)) {
        return Xrm.Sdk.Metadata.MetadataCache._optionsCache[cacheKey];
    }
    else {
        var attribute = Xrm.Sdk.Metadata.MetadataCache._loadAttributeMetadata(entityLogicalName, attributeLogicalName);
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
        return opts;
    }
}
Xrm.Sdk.Metadata.MetadataCache.getEntityTypeCodeFromName = function Xrm_Sdk_Metadata_MetadataCache$getEntityTypeCodeFromName(typeName) {
    var entity = Xrm.Sdk.Metadata.MetadataCache._loadEntityMetadata(typeName);
    return entity.objectTypeCode;
}
Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl = function Xrm_Sdk_Metadata_MetadataCache$getSmallIconUrl(typeName) {
    var entity = Xrm.Sdk.Metadata.MetadataCache._loadEntityMetadata(typeName);
    if (entity.isCustomEntity != null && !!entity.isCustomEntity) {
        if (entity.iconSmallName != null) {
            return '../../' + entity.iconSmallName;
        }
        else {
            return '/_imgs/ico_16_customEnity.gif';
        }
    }
    else {
        return '/_imgs/ico_16_' + entity.objectTypeCode.toString() + '.gif';
    }
}
Xrm.Sdk.Metadata.MetadataCache._loadEntityMetadata = function Xrm_Sdk_Metadata_MetadataCache$_loadEntityMetadata(entityLogicalName) {
    var cacheKey = entityLogicalName;
    var metaData = Xrm.Sdk.Metadata.MetadataCache._entityMetaData[cacheKey];
    if (metaData == null) {
        var request = new Xrm.Sdk.Messages.RetrieveEntityRequest();
        request.entityFilters = 1;
        request.logicalName = entityLogicalName;
        request.retrieveAsIfPublished = true;
        request.metadataId = new Xrm.Sdk.Guid('00000000-0000-0000-0000-000000000000');
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(request);
        metaData = response.entityMetadata;
        Xrm.Sdk.Metadata.MetadataCache._entityMetaData[cacheKey] = metaData;
    }
    return metaData;
}
Xrm.Sdk.Metadata.MetadataCache._loadAttributeMetadata = function Xrm_Sdk_Metadata_MetadataCache$_loadAttributeMetadata(entityLogicalName, attributeLogicalName) {
    var cacheKey = entityLogicalName + '|' + attributeLogicalName;
    var metaData = Xrm.Sdk.Metadata.MetadataCache._attributeMetaData[cacheKey];
    if (metaData == null) {
        var request = new Xrm.Sdk.Messages.RetrieveAttributeRequest();
        request.entityLogicalName = entityLogicalName;
        request.logicalName = attributeLogicalName;
        request.retrieveAsIfPublished = true;
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(request);
        metaData = response.attributeMetadata;
        Xrm.Sdk.Metadata.MetadataCache._attributeMetaData[cacheKey] = metaData;
    }
    return metaData;
}


Type.registerNamespace('Xrm.Sdk.Metadata.Query');

////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.Query.DeletedMetadataFilters

Xrm.Sdk.Metadata.Query.DeletedMetadataFilters = function() { };
Xrm.Sdk.Metadata.Query.DeletedMetadataFilters.prototype = {
    default_: 'default_', 
    entity: 'entity', 
    attribute: 'attribute', 
    relationship: 'relationship', 
    label: 'label', 
    optionSet: 'optionSet', 
    all: 'all'
}
Xrm.Sdk.Metadata.Query.DeletedMetadataFilters.registerEnum('Xrm.Sdk.Metadata.Query.DeletedMetadataFilters', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.Query.MetadataConditionOperator

Xrm.Sdk.Metadata.Query.MetadataConditionOperator = function() { };
Xrm.Sdk.Metadata.Query.MetadataConditionOperator.prototype = {
    Equals: 'Equals', 
    NotEquals: 'NotEquals', 
    In: 'In', 
    NotIn: 'NotIn', 
    GreaterThan: 'GreaterThan', 
    LessThan: 'LessThan'
}
Xrm.Sdk.Metadata.Query.MetadataConditionOperator.registerEnum('Xrm.Sdk.Metadata.Query.MetadataConditionOperator', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.Query.LogicalOperator

Xrm.Sdk.Metadata.Query.LogicalOperator = function() { };
Xrm.Sdk.Metadata.Query.LogicalOperator.prototype = {
    And: 'And', 
    Or: 'Or'
}
Xrm.Sdk.Metadata.Query.LogicalOperator.registerEnum('Xrm.Sdk.Metadata.Query.LogicalOperator', false);


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.Query.MetadataSerialiser

Xrm.Sdk.Metadata.Query.MetadataSerialiser = function Xrm_Sdk_Metadata_Query_MetadataSerialiser() {
}
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseAttributeQueryExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseAttributeQueryExpression(item) {
    return Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataQueryExpression(item);
}
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseEntityQueryExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseEntityQueryExpression(item) {
    if (item != null) {
        var xml = "<b:value i:type='c:EntityQueryExpression' xmlns:c='http://schemas.microsoft.com/xrm/2011/Metadata/Query'>" + Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataQueryExpression(item) + '<c:AttributeQuery>' + Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseAttributeQueryExpression(item.attributeQuery) + '</c:AttributeQuery>\r\n                <c:LabelQuery>' + Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseLabelQueryExpression(item.labelQuery) + "</c:LabelQuery>\r\n                <c:RelationshipQuery i:nil='true' />\r\n                </b:value>";
        return xml;
    }
    else {
        return "<b:value i:nil='true'/>";
    }
}
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseLabelQueryExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseLabelQueryExpression(item) {
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
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataConditionExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataConditionExpression(item) {
    return '<c:MetadataConditionExpression>\r\n                            <c:ConditionOperator>' + item.conditionOperator + '</c:ConditionOperator>\r\n                            <c:PropertyName>' + item.propertyName + "</c:PropertyName>\r\n                            <c:Value i:type='d:string' xmlns:d='http://www.w3.org/2001/XMLSchema'>" + item.value + '</c:Value>\r\n                          </c:MetadataConditionExpression>';
}
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataFilterExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataFilterExpression(item) {
    if (item != null) {
        var xml = '<c:Conditions>';
        var $enum1 = ss.IEnumerator.getEnumerator(item.conditions);
        while ($enum1.moveNext()) {
            var ex = $enum1.current;
            xml += Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataConditionExpression(ex);
        }
        xml = xml + '</c:Conditions>\r\n                        <c:FilterOperator>' + item.filterOperator + '</c:FilterOperator>\r\n                        <c:Filters />';
        return xml;
    }
    return '';
}
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataPropertiesExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataPropertiesExpression(item) {
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
Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataQueryExpression = function Xrm_Sdk_Metadata_Query_MetadataSerialiser$serialiseMetadataQueryExpression(item) {
    if (item != null) {
        var xml = '<c:Criteria>' + Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataFilterExpression(item.criteria) + '</c:Criteria>\r\n                    <c:Properties>' + Xrm.Sdk.Metadata.Query.MetadataSerialiser.serialiseMetadataPropertiesExpression(item.properties) + ' </c:Properties>';
        return xml;
    }
    return '';
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Metadata.Query.MetadataQueryBuilder

Xrm.Sdk.Metadata.Query.MetadataQueryBuilder = function Xrm_Sdk_Metadata_Query_MetadataQueryBuilder() {
    this.request = new Xrm.Sdk.Messages.RetrieveMetadataChangesRequest();
    this.request.query = {};
    this.request.query.criteria = {};
    this.request.query.criteria.filterOperator = 'Or';
    this.request.query.criteria.conditions = [];
}
Xrm.Sdk.Metadata.Query.MetadataQueryBuilder.prototype = {
    request: null,
    
    addEntities: function Xrm_Sdk_Metadata_Query_MetadataQueryBuilder$addEntities(entityLogicalNames, propertiesToReturn) {
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
    
    addAttributes: function Xrm_Sdk_Metadata_Query_MetadataQueryBuilder$addAttributes(attributeLogicalNames, propertiesToReturn) {
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
    
    setLanguage: function Xrm_Sdk_Metadata_Query_MetadataQueryBuilder$setLanguage(lcid) {
        this.request.query.labelQuery = {};
        this.request.query.labelQuery.filterLanguages = [];
        this.request.query.labelQuery.filterLanguages.add(lcid);
    }
}


Type.registerNamespace('Xrm.Sdk.Ribbon');

////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Ribbon.RibbonButton

Xrm.Sdk.Ribbon.RibbonButton = function Xrm_Sdk_Ribbon_RibbonButton(Id, Sequence, LabelText, Command, Image16, Image32) {
    this.Id = Id;
    this.Sequence = Sequence;
    this.LabelText = LabelText;
    this.Command = Command;
    this.Image16by16 = Image16;
    this.Image32by32 = Image32;
}
Xrm.Sdk.Ribbon.RibbonButton.prototype = {
    Id: null,
    LabelText: null,
    Sequence: 0,
    Command: null,
    Image16by16: null,
    Image32by32: null,
    
    serialiseToRibbonXml: function Xrm_Sdk_Ribbon_RibbonButton$serialiseToRibbonXml(sb) {
        sb.appendLine('<Button Id="' + Xrm.Sdk.XmlHelper.encode(this.Id) + '" LabelText="' + Xrm.Sdk.XmlHelper.encode(this.LabelText) + '" Sequence="' + this.Sequence.toString() + '" Command="' + Xrm.Sdk.XmlHelper.encode(this.Command) + '"' + ((this.Image32by32 != null) ? (' Image32by32="' + Xrm.Sdk.XmlHelper.encode(this.Image32by32) + '"') : '') + ((this.Image16by16 != null) ? (' Image16by16="' + Xrm.Sdk.XmlHelper.encode(this.Image16by16) + '"') : '') + ' />');
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Ribbon.RibbonMenu

Xrm.Sdk.Ribbon.RibbonMenu = function Xrm_Sdk_Ribbon_RibbonMenu(Id) {
    this.sections = [];
    this.Id = Id;
}
Xrm.Sdk.Ribbon.RibbonMenu.prototype = {
    Id: null,
    
    serialiseToRibbonXml: function Xrm_Sdk_Ribbon_RibbonMenu$serialiseToRibbonXml() {
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
    
    addSection: function Xrm_Sdk_Ribbon_RibbonMenu$addSection(section) {
        Xrm.ArrayEx.add(this.sections, section);
        return this;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Sdk.Ribbon.RibbonMenuSection

Xrm.Sdk.Ribbon.RibbonMenuSection = function Xrm_Sdk_Ribbon_RibbonMenuSection(Id, LabelText, Sequence, DisplayMode) {
    this.buttons = [];
    this.Id = Id;
    this.Title = LabelText;
    this.Sequence = Sequence;
    this.DisplayMode = DisplayMode;
}
Xrm.Sdk.Ribbon.RibbonMenuSection.prototype = {
    Id: null,
    Title: null,
    Sequence: 0,
    DisplayMode: null,
    
    serialiseToRibbonXml: function Xrm_Sdk_Ribbon_RibbonMenuSection$serialiseToRibbonXml(sb) {
        sb.appendLine('<MenuSection Id="' + Xrm.Sdk.XmlHelper.encode(this.Id) + ((this.Title != null) ? '" Title="' + this.Title : '') + '" Sequence="' + this.Sequence.toString() + '" DisplayMode="' + this.DisplayMode + '">');
        sb.appendLine('<Controls Id="' + Xrm.Sdk.XmlHelper.encode(this.Id + '.Controls') + '">');
        var $enum1 = ss.IEnumerator.getEnumerator(this.buttons);
        while ($enum1.moveNext()) {
            var button = $enum1.current;
            button.serialiseToRibbonXml(sb);
        }
        sb.appendLine('</Controls>');
        sb.appendLine('</MenuSection>');
    },
    
    addButton: function Xrm_Sdk_Ribbon_RibbonMenuSection$addButton(button) {
        Xrm.ArrayEx.add(this.buttons, button);
        return this;
    }
}


Type.registerNamespace('Xrm.Services');

////////////////////////////////////////////////////////////////////////////////
// Xrm.Services.CachedOrganizationService

Xrm.Services.CachedOrganizationService = function Xrm_Services_CachedOrganizationService() {
}
Xrm.Services.CachedOrganizationService.retrieve = function Xrm_Services_CachedOrganizationService$retrieve(entityName, entityId, attributesList) {
    var result = Xrm.Services.CachedOrganizationService.cache.get(entityName, entityId);
    if (result == null) {
        result = Xrm.Sdk.OrganizationServiceProxy.retrieve(entityName, entityId, attributesList);
        Xrm.Services.CachedOrganizationService.cache.insert(entityName, entityId, result);
        return result;
    }
    else {
        return result;
    }
}
Xrm.Services.CachedOrganizationService.retrieveMultiple = function Xrm_Services_CachedOrganizationService$retrieveMultiple(fetchXml) {
    var result = Xrm.Services.CachedOrganizationService.cache.get('query', fetchXml);
    if (result == null) {
        result = Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchXml);
        Xrm.Services.CachedOrganizationService.cache.insert('query', fetchXml, result);
        return result;
    }
    else {
        return result;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Xrm.Services.OrganizationServiceCache

Xrm.Services.OrganizationServiceCache = function Xrm_Services_OrganizationServiceCache() {
    this._innerCache = {};
}
Xrm.Services.OrganizationServiceCache.prototype = {
    
    remove: function Xrm_Services_OrganizationServiceCache$remove(entityName, id) {
    },
    
    insert: function Xrm_Services_OrganizationServiceCache$insert(key, query, results) {
        this._innerCache[key + '_' + query] = results;
    },
    
    get: function Xrm_Services_OrganizationServiceCache$get(key, query) {
        return this._innerCache[key + '_' + query];
    }
}


Xrm.ArrayEx.registerClass('Xrm.ArrayEx');
Xrm.DelegateItterator.registerClass('Xrm.DelegateItterator');
Xrm.NumberEx.registerClass('Xrm.NumberEx');
Xrm.PageEx.registerClass('Xrm.PageEx');
Xrm.StringEx.registerClass('Xrm.StringEx');
Xrm.TabItem.registerClass('Xrm.TabItem');
Xrm.TabSection.registerClass('Xrm.TabSection');
Xrm.Sdk.Attribute.registerClass('Xrm.Sdk.Attribute');
Xrm.Sdk.AttributeTypes.registerClass('Xrm.Sdk.AttributeTypes');
Xrm.Sdk.UserSettingsAttributes.registerClass('Xrm.Sdk.UserSettingsAttributes');
Xrm.Sdk.Entity.registerClass('Xrm.Sdk.Entity', null, Xrm.ComponentModel.INotifyPropertyChanged);
Xrm.Sdk.UserSettings.registerClass('Xrm.Sdk.UserSettings', Xrm.Sdk.Entity);
Xrm.Sdk.DataCollectionOfEntity.registerClass('Xrm.Sdk.DataCollectionOfEntity', null, ss.IEnumerable);
Xrm.Sdk.DateTimeEx.registerClass('Xrm.Sdk.DateTimeEx');
Xrm.Sdk.EntityCollection.registerClass('Xrm.Sdk.EntityCollection');
Xrm.Sdk.EntityReference.registerClass('Xrm.Sdk.EntityReference');
Xrm.Sdk.Guid.registerClass('Xrm.Sdk.Guid');
Xrm.Sdk.Money.registerClass('Xrm.Sdk.Money');
Xrm.Sdk.OptionSetValue.registerClass('Xrm.Sdk.OptionSetValue');
Xrm.Sdk.OrganizationServiceProxy.registerClass('Xrm.Sdk.OrganizationServiceProxy');
Xrm.Sdk.Relationship.registerClass('Xrm.Sdk.Relationship');
Xrm.Sdk.XmlHelper.registerClass('Xrm.Sdk.XmlHelper');
Xrm.Sdk.Messages.BulkDeleteRequest.registerClass('Xrm.Sdk.Messages.BulkDeleteRequest', null, Object);
Xrm.Sdk.Messages.BulkDeleteResponse.registerClass('Xrm.Sdk.Messages.BulkDeleteResponse', null, Object);
Xrm.Sdk.Messages.FetchXmlToQueryExpressionRequest.registerClass('Xrm.Sdk.Messages.FetchXmlToQueryExpressionRequest', null, Object);
Xrm.Sdk.Messages.FetchXmlToQueryExpressionResponse.registerClass('Xrm.Sdk.Messages.FetchXmlToQueryExpressionResponse', null, Object);
Xrm.Sdk.Messages.RetrieveAllEntitiesRequest.registerClass('Xrm.Sdk.Messages.RetrieveAllEntitiesRequest', null, Object);
Xrm.Sdk.Messages.RetrieveAllEntitiesResponse.registerClass('Xrm.Sdk.Messages.RetrieveAllEntitiesResponse', null, Object);
Xrm.Sdk.Messages.RetrieveAttributeRequest.registerClass('Xrm.Sdk.Messages.RetrieveAttributeRequest', null, Object);
Xrm.Sdk.Messages.RetrieveAttributeResponse.registerClass('Xrm.Sdk.Messages.RetrieveAttributeResponse', null, Object);
Xrm.Sdk.Messages.RetrieveEntityRequest.registerClass('Xrm.Sdk.Messages.RetrieveEntityRequest', null, Object);
Xrm.Sdk.Messages.RetrieveEntityResponse.registerClass('Xrm.Sdk.Messages.RetrieveEntityResponse', null, Object);
Xrm.Sdk.Messages.RetrieveMetadataChangesRequest.registerClass('Xrm.Sdk.Messages.RetrieveMetadataChangesRequest', null, Object);
Xrm.Sdk.Messages.RetrieveMetadataChangesResponse.registerClass('Xrm.Sdk.Messages.RetrieveMetadataChangesResponse', null, Object);
Xrm.Sdk.Metadata.MetadataSerialiser.registerClass('Xrm.Sdk.Metadata.MetadataSerialiser');
Xrm.Sdk.Metadata.MetadataCache.registerClass('Xrm.Sdk.Metadata.MetadataCache');
Xrm.Sdk.Metadata.Query.MetadataSerialiser.registerClass('Xrm.Sdk.Metadata.Query.MetadataSerialiser');
Xrm.Sdk.Metadata.Query.MetadataQueryBuilder.registerClass('Xrm.Sdk.Metadata.Query.MetadataQueryBuilder');
Xrm.Sdk.Ribbon.RibbonButton.registerClass('Xrm.Sdk.Ribbon.RibbonButton');
Xrm.Sdk.Ribbon.RibbonMenu.registerClass('Xrm.Sdk.Ribbon.RibbonMenu');
Xrm.Sdk.Ribbon.RibbonMenuSection.registerClass('Xrm.Sdk.Ribbon.RibbonMenuSection');
Xrm.Services.CachedOrganizationService.registerClass('Xrm.Services.CachedOrganizationService');
Xrm.Services.OrganizationServiceCache.registerClass('Xrm.Services.OrganizationServiceCache');
Xrm.Sdk.AttributeTypes.string_ = 'string';
Xrm.Sdk.AttributeTypes.decimal_ = 'decimal';
Xrm.Sdk.AttributeTypes.int_ = 'int';
Xrm.Sdk.AttributeTypes.double_ = 'double';
Xrm.Sdk.AttributeTypes.dateTime_ = 'dateTime';
Xrm.Sdk.AttributeTypes.boolean_ = 'boolean';
Xrm.Sdk.AttributeTypes.entityReference = 'EntityReference';
Xrm.Sdk.AttributeTypes.guid_ = 'guid';
Xrm.Sdk.AttributeTypes.optionSetValue = 'OptionSetValue';
Xrm.Sdk.AttributeTypes.aliasedValue = 'AliasedValue';
Xrm.Sdk.AttributeTypes.entityCollection = 'EntityCollection';
Xrm.Sdk.AttributeTypes.money = 'Money';
Xrm.Sdk.UserSettingsAttributes.userSettingsId = 'usersettingsid';
Xrm.Sdk.UserSettingsAttributes.businessUnitId = 'businessunitid';
Xrm.Sdk.UserSettingsAttributes.calendarType = 'calendartype';
Xrm.Sdk.UserSettingsAttributes.currencyDecimalPrecision = 'currencydecimalprecision';
Xrm.Sdk.UserSettingsAttributes.currencyFormatCode = 'currencyformatcode';
Xrm.Sdk.UserSettingsAttributes.currencySymbol = 'currencysymbol';
Xrm.Sdk.UserSettingsAttributes.dateFormatCode = 'dateformatcode';
Xrm.Sdk.UserSettingsAttributes.dateFormatString = 'dateformatstring';
Xrm.Sdk.UserSettingsAttributes.dateSeparator = 'dateseparator';
Xrm.Sdk.UserSettingsAttributes.decimalSymbol = 'decimalsymbol';
Xrm.Sdk.UserSettingsAttributes.defaultCalendarView = 'defaultcalendarview';
Xrm.Sdk.UserSettingsAttributes.defaultDashboardId = 'defaultdashboardid';
Xrm.Sdk.UserSettingsAttributes.localeId = 'localeid';
Xrm.Sdk.UserSettingsAttributes.longDateFormatCode = 'longdateformatcode';
Xrm.Sdk.UserSettingsAttributes.negativeCurrencyFormatCode = 'negativecurrencyformatcode';
Xrm.Sdk.UserSettingsAttributes.negativeFormatCode = 'negativeformatcode';
Xrm.Sdk.UserSettingsAttributes.numberGroupFormat = 'numbergroupformat';
Xrm.Sdk.UserSettingsAttributes.numberSeparator = 'numberseparator';
Xrm.Sdk.UserSettingsAttributes.offlineSyncInterval = 'offlinesyncinterval';
Xrm.Sdk.UserSettingsAttributes.pricingDecimalPrecision = 'pricingdecimalprecision';
Xrm.Sdk.UserSettingsAttributes.showWeekNumber = 'showweeknumber';
Xrm.Sdk.UserSettingsAttributes.systemUserId = 'systemuserid';
Xrm.Sdk.UserSettingsAttributes.timeFormatCodestring = 'timeformatcodestring';
Xrm.Sdk.UserSettingsAttributes.timeFormatString = 'timeformatstring';
Xrm.Sdk.UserSettingsAttributes.timeSeparator = 'timeseparator';
Xrm.Sdk.UserSettingsAttributes.timeZoneBias = 'timezonebias';
Xrm.Sdk.UserSettingsAttributes.timeZoneCode = 'timezonecode';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightBias = 'timezonedaylightbias';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightDay = 'timezonedaylightday';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightDayOfWeek = 'timezonedaylightdayofweek';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightHour = 'timezonedaylighthour';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightMinute = 'timezonedaylightminute';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightMonth = 'timezonedaylightmonth';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightSecond = 'timezonedaylightsecond';
Xrm.Sdk.UserSettingsAttributes.timeZoneDaylightYear = 'timezonedaylightyear';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardBias = 'timezonestandardbias';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardDay = 'timezonestandardday';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardDayOfWeek = 'timezonestandarddayofweek';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardHour = 'timezonestandardhour';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardMinute = 'timezonestandardminute';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardMonth = 'timezonestandardmonth';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardSecond = 'timezonestandardsecond';
Xrm.Sdk.UserSettingsAttributes.timeZoneStandardYear = 'timezonestandardyear';
Xrm.Sdk.UserSettingsAttributes.transactionCurrencyId = 'transactioncurrencyid';
Xrm.Sdk.UserSettingsAttributes.uiLanguageId = 'uilanguageid';
Xrm.Sdk.UserSettingsAttributes.workdayStartTime = 'workdaystarttime';
Xrm.Sdk.UserSettingsAttributes.workdayStopTime = 'workdaystoptime';
Xrm.Sdk.UserSettings.entityLogicalName = 'usersettings';
Xrm.Sdk.Guid.empty = new Xrm.Sdk.Guid('00000000-0000-0000-0000-000000000000');
Xrm.Sdk.OrganizationServiceProxy.userSettings = null;
Xrm.Sdk.XmlHelper._encode_map = { '&': '&amp;', '"': '&quot;', '<': '&lt;', '>': '&gt;' };
Xrm.Sdk.XmlHelper._decode_map = { '&amp;': '&', '&quot;': '"', '&lt;': '<', '&gt;': '>' };
Xrm.Sdk.Metadata.MetadataCache._attributeMetaData = {};
Xrm.Sdk.Metadata.MetadataCache._entityMetaData = {};
Xrm.Sdk.Metadata.MetadataCache._optionsCache = {};
Xrm.Services.CachedOrganizationService.cache = new Xrm.Services.OrganizationServiceCache();
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
                    hasLoaded = typeof (window.jQuery) != "undefined";
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
