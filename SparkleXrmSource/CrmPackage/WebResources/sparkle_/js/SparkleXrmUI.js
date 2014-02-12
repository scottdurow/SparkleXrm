//! SparkleXrmUI.debug.js
//
waitForScripts("xrmui",["mscorlib","xrm","jquery", "jquery-ui"],
function () {

(function($){

Type.registerNamespace('SparkleXrm.CustomBinding');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.EnterKeyBinding

SparkleXrm.CustomBinding.EnterKeyBinding = function SparkleXrm_CustomBinding_EnterKeyBinding() {
    SparkleXrm.CustomBinding.EnterKeyBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.EnterKeyBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_EnterKeyBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        ko.utils.registerEventHandler(element, 'keydown', function(sender, e) {
            var eventTyped = sender;
            if (eventTyped.keyCode === 13) {
                eventTyped.preventDefault();
                eventTyped.target.blur();
                valueAccessor().call(viewModel);
            }
        });
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmCurrencySymbolBinding

SparkleXrm.CustomBinding.XrmCurrencySymbolBinding = function SparkleXrm_CustomBinding_XrmCurrencySymbolBinding() {
    SparkleXrm.CustomBinding.XrmCurrencySymbolBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmCurrencySymbolBinding.getCurrencySymbol = function SparkleXrm_CustomBinding_XrmCurrencySymbolBinding$getCurrencySymbol(valueAccessor) {
    var value = ko.utils.unwrapObservable(valueAccessor());
    if (value != null && value.id != null) {
        return Xrm.NumberEx.getCurrencySymbol(value.id);
    }
    return '';
}
SparkleXrm.CustomBinding.XrmCurrencySymbolBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmCurrencySymbolBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
    },
    
    update: function SparkleXrm_CustomBinding_XrmCurrencySymbolBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var currencyIndicatorSpan = $(element).find('.sparkle-input-currencyprefix-part');
        var interceptAccesor = function() {
            var currencySymbol = SparkleXrm.CustomBinding.XrmCurrencySymbolBinding.getCurrencySymbol(valueAccessor);
            return currencySymbol;
        };
        ko.bindingHandlers.text.update(currencyIndicatorSpan.get(0),interceptAccesor,allBindingsAccessor,viewModel,context);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmMoneyBinding

SparkleXrm.CustomBinding.XrmMoneyBinding = function SparkleXrm_CustomBinding_XrmMoneyBinding() {
    SparkleXrm.CustomBinding.XrmMoneyBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmMoneyBinding._getNumberFormatInfo$1 = function SparkleXrm_CustomBinding_XrmMoneyBinding$_getNumberFormatInfo$1(allBindingsAccessor) {
    var format = Xrm.NumberEx.getCurrencyEditFormatInfo();
    if (allBindingsAccessor()['minvalue'] == null) {
        format.minValue = -2147483648;
    }
    else {
        format.minValue = allBindingsAccessor()['minvalue'];
    }
    if (allBindingsAccessor()['maxvalue'] == null) {
        format.maxValue = 2147483647;
    }
    else {
        format.maxValue = allBindingsAccessor()['maxvalue'];
    }
    return format;
}
SparkleXrm.CustomBinding.XrmMoneyBinding._formatNumber$1 = function SparkleXrm_CustomBinding_XrmMoneyBinding$_formatNumber$1(value, format) {
    if (value != null) {
        return Xrm.NumberEx.format(value.value, format);
    }
    else {
        return '';
    }
}
SparkleXrm.CustomBinding.XrmMoneyBinding._trySetObservable$1 = function SparkleXrm_CustomBinding_XrmMoneyBinding$_trySetObservable$1(valueAccessor, inputField, value, format) {
    var observable = valueAccessor();
    var isValid = true;
    var numericValue = Xrm.NumberEx.parse(value, format);
    if (!isNaN(numericValue) && numericValue >= format.minValue && numericValue <= format.maxValue) {
        numericValue = Xrm.NumberEx.round(numericValue, format.precision);
        var newValue = new Xrm.Sdk.Money(numericValue);
        observable(newValue);
        if ((typeof(observable.isValid)) !== 'undefined') {
            isValid = !!(observable).isValid();
        }
        if (isValid) {
            var formattedNumber = SparkleXrm.CustomBinding.XrmMoneyBinding._formatNumber$1(newValue, format);
            inputField.val(formattedNumber);
            inputField.blur();
        }
    }
    else {
        alert(String.format('You must enter a number between {0} and {1}', format.minValue, format.maxValue));
        var currentValue = observable();
        var formattedNumber = SparkleXrm.CustomBinding.XrmMoneyBinding._formatNumber$1(currentValue, format);
        inputField.val(formattedNumber);
        inputField.focus();
    }
}
SparkleXrm.CustomBinding.XrmMoneyBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmMoneyBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var textBox = $(element).find('.sparkle-input-textbox-part');
        var format = SparkleXrm.CustomBinding.XrmMoneyBinding._getNumberFormatInfo$1(allBindingsAccessor);
        var args = arguments;
        var onChangeHandler = function(e) {
            var observable = valueAccessor();
            var newValue = textBox.val();
            SparkleXrm.CustomBinding.XrmMoneyBinding._trySetObservable$1(valueAccessor, textBox, newValue, format);
        };
        textBox.change(onChangeHandler);
    },
    
    update: function SparkleXrm_CustomBinding_XrmMoneyBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var textBox = $(element).find('.sparkle-input-textbox-part');
        var format = SparkleXrm.CustomBinding.XrmMoneyBinding._getNumberFormatInfo$1(allBindingsAccessor);
        var interceptAccesor = function() {
            var value = (valueAccessor())();
            if (value != null) {
                return Xrm.NumberEx.format(value.value, format);
            }
            else {
                return '';
            }
        };
        ko.bindingHandlers.value.update(textBox.get(0),interceptAccesor,allBindingsAccessor,viewModel,context);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmNumericBinding

SparkleXrm.CustomBinding.XrmNumericBinding = function SparkleXrm_CustomBinding_XrmNumericBinding() {
    SparkleXrm.CustomBinding.XrmNumericBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmNumericBinding._getNumberFormatInfo$1 = function SparkleXrm_CustomBinding_XrmNumericBinding$_getNumberFormatInfo$1(allBindingsAccessor) {
    var format = Xrm.NumberEx.getNumberFormatInfo();
    format.precision = parseInt(allBindingsAccessor()['precision']);
    if (allBindingsAccessor()['minvalue'] == null) {
        format.minValue = -2147483648;
    }
    else {
        format.minValue = allBindingsAccessor()['minvalue'];
    }
    if (allBindingsAccessor()['maxvalue'] == null) {
        format.maxValue = 2147483647;
    }
    else {
        format.maxValue = allBindingsAccessor()['maxvalue'];
    }
    return format;
}
SparkleXrm.CustomBinding.XrmNumericBinding._formatNumber$1 = function SparkleXrm_CustomBinding_XrmNumericBinding$_formatNumber$1(value, format) {
    if (value != null) {
        return Xrm.NumberEx.format(value, format);
    }
    else {
        return '';
    }
}
SparkleXrm.CustomBinding.XrmNumericBinding._trySetObservable$1 = function SparkleXrm_CustomBinding_XrmNumericBinding$_trySetObservable$1(valueAccessor, inputField, value, format) {
    var observable = valueAccessor();
    var isValid = true;
    var numericValue = Xrm.NumberEx.parse(value, format);
    if (!isNaN(numericValue) && numericValue >= format.minValue && numericValue <= format.maxValue) {
        numericValue = Xrm.NumberEx.round(numericValue, format.precision);
        observable(numericValue);
        if ((typeof(observable.isValid)) !== 'undefined') {
            isValid = !!(observable).isValid();
        }
        if (isValid) {
            var formattedNumber = SparkleXrm.CustomBinding.XrmNumericBinding._formatNumber$1(numericValue, format);
            inputField.val(formattedNumber);
            inputField.blur();
        }
    }
    else {
        alert(String.format('You must enter a number between {0} and {1}', format.minValue, format.maxValue));
        var currentValue = observable();
        var formattedNumber = SparkleXrm.CustomBinding.XrmNumericBinding._formatNumber$1(currentValue, format);
        inputField.val(formattedNumber);
        inputField.focus();
    }
}
SparkleXrm.CustomBinding.XrmNumericBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmNumericBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var textBox = $(element).find('.sparkle-input-textbox-part');
        var format = SparkleXrm.CustomBinding.XrmNumericBinding._getNumberFormatInfo$1(allBindingsAccessor);
        var onChangeHandler = function(e) {
            var observable = valueAccessor();
            var newValue = textBox.val();
            SparkleXrm.CustomBinding.XrmNumericBinding._trySetObservable$1(valueAccessor, textBox, newValue, format);
        };
        textBox.change(onChangeHandler);
    },
    
    update: function SparkleXrm_CustomBinding_XrmNumericBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var textBox = $(element).find('.sparkle-input-textbox-part');
        var format = SparkleXrm.CustomBinding.XrmNumericBinding._getNumberFormatInfo$1(allBindingsAccessor);
        var interceptAccesor = function() {
            var value = (valueAccessor())();
            if (value != null) {
                return Xrm.NumberEx.format(value, format);
            }
            else {
                return '';
            }
        };
        ko.bindingHandlers.value.update(textBox.get(0),interceptAccesor,allBindingsAccessor,viewModel,context);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmOptionSetBinding

SparkleXrm.CustomBinding.XrmOptionSetBinding = function SparkleXrm_CustomBinding_XrmOptionSetBinding() {
    SparkleXrm.CustomBinding.XrmOptionSetBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmOptionSetBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmOptionSetBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var select = $(element).find('.sparkle-input-optionset-part');
        var onChangeHandler = function(e) {
            var observable = valueAccessor();
            var newValue = select.val();
            var newValueInt = null;
            if (!String.isNullOrEmpty(newValue)) {
                newValueInt = parseInt(newValue);
            }
            var newValueOptionSetValue = new Xrm.Sdk.OptionSetValue(newValueInt);
            newValueOptionSetValue.name = select.find('option:selected').text();
            observable(newValueOptionSetValue);
        };
        select.change(onChangeHandler);
        allBindingsAccessor()['optionsValue'] = 'value';
        allBindingsAccessor()['optionsText'] = 'name';
        var optionSetOptions = (allBindingsAccessor()['optionSetOptions']);
        var optionsValueAccessor = function() {
            return Xrm.Sdk.Metadata.MetadataCache.getOptionSetValues(optionSetOptions.entityLogicalName, optionSetOptions.attributeLogicalName, optionSetOptions.allowEmpty);
        };
        ko.bindingHandlers.options.update(select.get(0),optionsValueAccessor,allBindingsAccessor,viewModel,context);
    },
    
    update: function SparkleXrm_CustomBinding_XrmOptionSetBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var select = $(element).find('.sparkle-input-optionset-part');
        var observable = valueAccessor();
        var value = observable();
        var newValue = '';
        if (value != null && value.value != null) {
            newValue = value.value.toString();
        }
        select.val(newValue);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.AnimateVisible

SparkleXrm.CustomBinding.AnimateVisible = function SparkleXrm_CustomBinding_AnimateVisible() {
    SparkleXrm.CustomBinding.AnimateVisible.initializeBase(this);
}
SparkleXrm.CustomBinding.AnimateVisible.prototype = {
    
    init: function SparkleXrm_CustomBinding_AnimateVisible$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var observable = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(observable));
    },
    
    update: function SparkleXrm_CustomBinding_AnimateVisible$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var observable = valueAccessor();
        var effectIn = (allBindingsAccessor()['effectIn']);
        var effectOut = (allBindingsAccessor()['effectOut']);
        var item = $(element);
        var effect = (ko.utils.unwrapObservable(observable)) ? effectIn : effectOut;
        switch (effect) {
            case 'fadeIn':
                item.fadeIn();
                break;
            case 'fadeOut':
                item.fadeOut();
                break;
            case 'slideUp':
                item.slideUp();
                break;
            case 'slideDown':
                item.slideDown();
                break;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.AutocompleteBinding

SparkleXrm.CustomBinding.AutocompleteBinding = function SparkleXrm_CustomBinding_AutocompleteBinding() {
    SparkleXrm.CustomBinding.AutocompleteBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.AutocompleteBinding._trySetObservable$1 = function SparkleXrm_CustomBinding_AutocompleteBinding$_trySetObservable$1(valueAccessor, inputField, value) {
    var observable = valueAccessor();
    var isValid = true;
    observable(value);
    if ((typeof(observable.isValid)) !== 'undefined') {
        isValid = !!(observable).isValid();
    }
    if (isValid) {
        inputField.blur();
    }
}
SparkleXrm.CustomBinding.AutocompleteBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_AutocompleteBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var inputField = $(element);
        var options = (allBindingsAccessor()['autocompleteOptions']);
        options.select = function(e, uiEvent) {
            var value = (uiEvent.item)['value'].toString();
            SparkleXrm.CustomBinding.AutocompleteBinding._trySetObservable$1(valueAccessor, inputField, value);
        };
        inputField = inputField.autocomplete(options);
        var selectButton = inputField.siblings('.timeSelectButton');
        selectButton.click(function(e) {
            inputField.autocomplete('search');
        });
        ko.utils.registerEventHandler(element, 'change', function(sender, e) {
            var value = inputField.val();
            SparkleXrm.CustomBinding.AutocompleteBinding._trySetObservable$1(valueAccessor, inputField, value);
        });
        var disposeCallBack = function() {
            $(element).autocomplete("destroy");
        };
        ko.utils.domNodeDisposal.addDisposeCallback(element, disposeCallBack);
        ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, null, null);
    },
    
    update: function SparkleXrm_CustomBinding_AutocompleteBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).val(value);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmBooleanBinding

SparkleXrm.CustomBinding.XrmBooleanBinding = function SparkleXrm_CustomBinding_XrmBooleanBinding() {
    SparkleXrm.CustomBinding.XrmBooleanBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmBooleanBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmBooleanBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var observableBool = valueAccessor();
        var interceptor = {};
        interceptor.read = function() {
            return observableBool().toString();
        };
        interceptor.write = function(newValue) {
            observableBool(newValue === 'true');
        };
        var targetBinding = (allBindingsAccessor()['targetBinding']);
        var bindings = {};
        bindings[targetBinding] = ko.computed(interceptor);
        ko.applyBindingsToNode(element, bindings);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmLookupBinding

SparkleXrm.CustomBinding.XrmLookupBinding = function SparkleXrm_CustomBinding_XrmLookupBinding() {
    SparkleXrm.CustomBinding.XrmLookupBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmLookupBinding._trySetObservable$1 = function SparkleXrm_CustomBinding_XrmLookupBinding$_trySetObservable$1(valueAccessor, inputField, value) {
    var observable = valueAccessor();
    var isValid = true;
    observable(value);
    if ((typeof(observable.isValid)) !== 'undefined') {
        isValid = !!(observable).isValid();
    }
    if (isValid) {
        inputField.blur();
    }
}
SparkleXrm.CustomBinding.XrmLookupBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmLookupBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var inputField = container.find('.sparkle-input-lookup-part');
        var selectButton = container.find('.sparkle-input-lookup-button-part');
        var _value = new Xrm.Sdk.EntityReference(null, null, null);
        var options = {};
        options.minLength = 100000;
        options.delay = 0;
        var justSelected = false;
        options.select = function(e, uiEvent) {
            var item = uiEvent.item;
            if (_value == null) {
                _value = new Xrm.Sdk.EntityReference(null, null, null);
            }
            var value = item.label;
            inputField.val(value);
            _value.id = (item.value);
            _value.name = item.label;
            _value.logicalName = 'activitypointer';
            justSelected = true;
            SparkleXrm.CustomBinding.XrmLookupBinding._trySetObservable$1(valueAccessor, inputField, _value);
            return false;;
        };
        var queryCommand = (allBindingsAccessor()['queryCommand']);
        var nameAttribute = (allBindingsAccessor()['nameAttribute']);
        var idAttribute = (allBindingsAccessor()['idAttribute']);
        var typeCodeAttribute = (allBindingsAccessor()['typeCodeAttribute']);
        var queryDelegate = function(request, response) {
            var queryCallBack = function(fetchResult) {
                var results = new Array(fetchResult.get_entities().get_count());
                for (var i = 0; i < fetchResult.get_entities().get_count(); i++) {
                    results[i] = {};
                    results[i].label = fetchResult.get_entities().get_item(i).getAttributeValue(nameAttribute);
                    results[i].value = fetchResult.get_entities().get_item(i).getAttributeValue(idAttribute);
                    var typeCodeName = fetchResult.get_entities().get_item(i).logicalName;
                    if (!String.isNullOrEmpty(typeCodeAttribute)) {
                        typeCodeName = fetchResult.get_entities().get_item(i).getAttributeValue(typeCodeAttribute).toString();
                    }
                    results[i].image = Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl(typeCodeName);
                }
                response(results);
                var disableOption = {};
                disableOption.minLength = 100000;
                inputField.autocomplete(disableOption);
            };
            queryCommand.call(context.$parent,request.term,queryCallBack);
        };
        options.source = queryDelegate;
        options.focus = function(e, uiEvent) {
            return false;;
        };
        inputField = inputField.autocomplete(options);
        (inputField.data('ui-autocomplete'))._renderItem = function(ul, item) {
            return $('<li>').append("<a class='sparkle-menu-item'><span class='sparkle-menu-item-img'><img src='" + item.image + "'/></span><span class='sparkle-menu-item-label'>" + item.label + '</span></a>').appendTo(ul);
        };
        selectButton.click(function(e) {
            var enableOption = {};
            enableOption.minLength = 0;
            inputField.autocomplete(enableOption);
            inputField.autocomplete('search');
        });
        inputField.change(function(e) {
            if (inputField.val() !== _value.name) {
                SparkleXrm.CustomBinding.XrmLookupBinding._trySetObservable$1(valueAccessor, inputField, null);
            }
        });
        var disposeCallBack = function() {
            $(element).autocomplete("destroy");
        };
        ko.utils.domNodeDisposal.addDisposeCallback(element, disposeCallBack);
        ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, null, null);
        inputField.keydown(function(e) {
            if (e.which === 13 && !justSelected) {
                selectButton.click();
            }
            else if (e.which === 13) {
                return;
            }
            switch (e.which) {
                case 13:
                case 38:
                case 40:
                    e.preventDefault();
                    e.stopPropagation();
                    break;
            }
            justSelected = false;
        });
    },
    
    update: function SparkleXrm_CustomBinding_XrmLookupBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var inputField = container.find('.sparkle-input-lookup-part');
        var value = ko.utils.unwrapObservable(valueAccessor());
        var displayName = '';
        if (value != null) {
            displayName = value.name;
        }
        inputField.val(displayName);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmTextBinding

SparkleXrm.CustomBinding.XrmTextBinding = function SparkleXrm_CustomBinding_XrmTextBinding() {
    SparkleXrm.CustomBinding.XrmTextBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmTextBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmTextBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var textBox = $(element).find('.sparkle-input-textbox-part');
        var onChangeHandler = function(e) {
            var observable = valueAccessor();
            var newValue = textBox.val();
            observable(newValue);
        };
        textBox.change(onChangeHandler);
        textBox.keyup(onChangeHandler);
    },
    
    update: function SparkleXrm_CustomBinding_XrmTextBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var textBox = $(element).find('.sparkle-input-textbox-part');
        ko.bindingHandlers.value.update(textBox.get(0),valueAccessor,allBindingsAccessor,viewModel,context);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmDatePickerBinding

SparkleXrm.CustomBinding.XrmDatePickerBinding = function SparkleXrm_CustomBinding_XrmDatePickerBinding() {
    SparkleXrm.CustomBinding.XrmDatePickerBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmDatePickerBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmDatePickerBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var dateTime = container.find('.sparkle-input-datepicker-part');
        var dateButton = container.find('.sparkle-input-datepicker-button-part');
        var options = {};
        options.showOn = '';
        options.buttonImageOnly = true;
        options.firstDay = (Xrm.Sdk.OrganizationServiceProxy.organizationSettings != null) ? Xrm.Sdk.OrganizationServiceProxy.organizationSettings.weekstartdaycode.value : 0;
        var dateFormat = 'dd/MM/yy';
        if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
            dateFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring;
        }
        options.dateFormat = dateFormat;
        dateTime.datepicker(options);
        dateButton.click(function(e) {
            dateTime.datepicker('show');
        });
        ko.utils.registerEventHandler(dateTime.get(0), 'change', function(sender, e) {
            var observable = valueAccessor();
            var isValid = true;
            if ((typeof(observable.IsValid)) !== 'undefined') {
                isValid = !!(observable).isValid();
            }
            if (isValid) {
                var selectedDate = dateTime.datepicker('getDate');
                var currentValue = observable();
                Xrm.Sdk.DateTimeEx.setTime(selectedDate, currentValue);
                observable(selectedDate);
            }
            dateTime.blur();
        });
        var disposeCallBack = function() {
            $(element).datepicker("destroy");
        };
        ko.utils.domNodeDisposal.addDisposeCallback(element, disposeCallBack);
    },
    
    update: function SparkleXrm_CustomBinding_XrmDatePickerBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var dateTime = container.find('.sparkle-input-datepicker-part');
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (typeof(value) === 'string') {
            value = Date.parseDate(value);
        }
        dateTime.datepicker('setDate', value);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmDurationBinding

SparkleXrm.CustomBinding.XrmDurationBinding = function SparkleXrm_CustomBinding_XrmDurationBinding() {
    SparkleXrm.CustomBinding.XrmDurationBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmDurationBinding._trySetObservable$1 = function SparkleXrm_CustomBinding_XrmDurationBinding$_trySetObservable$1(valueAccessor, inputField, value) {
    var observable = valueAccessor();
    var isValid = true;
    var isEmpty = (value == null) || (!value.length);
    var pattern = '/([0-9]*)[ ]?((h(our)?[s]?)|(m(inute)?[s]?)|(d(ay)?[s]?))/g';
    var regex = RegExp.parse(pattern);
    var match = regex.exec(value);
    if (isEmpty) {
        observable(null);
    }
    else if (match != null && match.length > 0) {
        var durationNumber = parseFloat(match[1]);
        switch (match[2].substr(0, 1).toLowerCase()) {
            case 'd':
                durationNumber = durationNumber * 60 * 24;
                break;
            case 'h':
                durationNumber = durationNumber * 60;
                break;
        }
        observable(durationNumber);
        if ((typeof(observable.isValid)) !== 'undefined') {
            isValid = !!(observable).isValid();
        }
        if (isValid) {
            inputField.blur();
        }
    }
    else {
        alert('Invalid Duration Format');
        var currentValue = observable();
        var durationString = SparkleXrm.CustomBinding.XrmDurationBinding._formatDuration$1(currentValue);
        inputField.val(durationString);
        inputField.focus();
    }
}
SparkleXrm.CustomBinding.XrmDurationBinding._formatDuration$1 = function SparkleXrm_CustomBinding_XrmDurationBinding$_formatDuration$1(duration) {
    var durationString = null;
    if (duration != null) {
        if (duration > (60 * 24)) {
            durationString = String.format('{0} d', duration / (60 * 24));
        }
        else if (duration === (60 * 24)) {
            durationString = String.format('{0} d', duration / (60 * 24));
        }
        else if (duration > 60) {
            durationString = String.format('{0} h', duration / (60));
        }
        else if (duration === 60) {
            durationString = String.format('{0} h', duration / (60));
        }
        else {
            durationString = String.format('{0} m', duration);
        }
    }
    else {
        durationString = null;
    }
    return durationString;
}
SparkleXrm.CustomBinding.XrmDurationBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmDurationBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var inputField = container.find('.sparkle-input-duration-part');
        var selectButton = container.find('.sparkle-input-duration-button-part');
        var options = {};
        options.source = [ '1 m', '2 m', '1 h', '2 h', '1 d' ];
        options.delay = 0;
        options.minLength = 0;
        options.select = function(e, uiEvent) {
            var value = (uiEvent.item)['value'].toString();
            SparkleXrm.CustomBinding.XrmDurationBinding._trySetObservable$1(valueAccessor, inputField, value);
        };
        inputField = inputField.autocomplete(options);
        selectButton.click(function(e) {
            inputField.autocomplete('search', '');
        });
        ko.utils.registerEventHandler(element, 'change', function(sender, e) {
            var value = inputField.val();
            SparkleXrm.CustomBinding.XrmDurationBinding._trySetObservable$1(valueAccessor, inputField, value);
        });
        var disposeCallBack = function() {
            $(element).autocomplete("destroy");
        };
        ko.utils.domNodeDisposal.addDisposeCallback(element, disposeCallBack);
    },
    
    update: function SparkleXrm_CustomBinding_XrmDurationBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var inputField = container.find('.sparkle-input-duration-part');
        var value = ko.utils.unwrapObservable(valueAccessor());
        var duration = value;
        var durationString = SparkleXrm.CustomBinding.XrmDurationBinding._formatDuration$1(duration);
        inputField.val(durationString);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.FadeVisibleBinding

SparkleXrm.CustomBinding.FadeVisibleBinding = function SparkleXrm_CustomBinding_FadeVisibleBinding() {
    SparkleXrm.CustomBinding.FadeVisibleBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.FadeVisibleBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_FadeVisibleBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var observable = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(observable));
    },
    
    update: function SparkleXrm_CustomBinding_FadeVisibleBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var observable = valueAccessor();
        if (ko.utils.unwrapObservable(observable)) {
            $(element).fadeIn();
        }
        else {
            $(element).fadeOut();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.ProgressBarBinding

SparkleXrm.CustomBinding.ProgressBarBinding = function SparkleXrm_CustomBinding_ProgressBarBinding() {
    SparkleXrm.CustomBinding.ProgressBarBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.ProgressBarBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_ProgressBarBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        $(element).progressbar();
    },
    
    update: function SparkleXrm_CustomBinding_ProgressBarBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var observable = valueAccessor();
        var value = ko.utils.unwrapObservable(observable);
        $(element).progressbar('value', value);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.CustomBinding.XrmTimeOfDayBinding

SparkleXrm.CustomBinding.XrmTimeOfDayBinding = function SparkleXrm_CustomBinding_XrmTimeOfDayBinding() {
    SparkleXrm.CustomBinding.XrmTimeOfDayBinding.initializeBase(this);
}
SparkleXrm.CustomBinding.XrmTimeOfDayBinding._trySetObservable$1 = function SparkleXrm_CustomBinding_XrmTimeOfDayBinding$_trySetObservable$1(valueAccessor, inputField, value) {
    var observable = valueAccessor();
    var isValid = true;
    var testDate = Xrm.Sdk.DateTimeEx.addTimeToDate(observable(), value);
    var newValue = (testDate == null) ? '' : testDate.toString();
    var originalValue = (observable() == null) ? '' : observable().toString();
    if (newValue === originalValue) {
        return;
    }
    if (testDate == null) {
        alert('Invalid Time');
        inputField.focus();
        var currentValue = observable();
        SparkleXrm.CustomBinding.XrmTimeOfDayBinding._formatterUpdate$1(inputField, currentValue);
    }
    else {
        observable(testDate);
        if ((typeof(observable.isValid)) !== 'undefined') {
            isValid = !!(observable).isValid();
        }
        if (isValid) {
            inputField.blur();
        }
    }
}
SparkleXrm.CustomBinding.XrmTimeOfDayBinding._formatterUpdate$1 = function SparkleXrm_CustomBinding_XrmTimeOfDayBinding$_formatterUpdate$1(inputField, value) {
    var formatString = SparkleXrm.CustomBinding.XrmTimeOfDayBinding._getFormatString$1();
    var formattedValue = '';
    if (value != null) {
        formattedValue = value.format(formatString);
    }
    inputField.val(formattedValue);
}
SparkleXrm.CustomBinding.XrmTimeOfDayBinding._getFormatString$1 = function SparkleXrm_CustomBinding_XrmTimeOfDayBinding$_getFormatString$1() {
    var timeFormatString = 'h:mm tt';
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        timeFormatString = Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring;
    }
    return timeFormatString;
}
SparkleXrm.CustomBinding.XrmTimeOfDayBinding.prototype = {
    
    init: function SparkleXrm_CustomBinding_XrmTimeOfDayBinding$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var formatString = SparkleXrm.CustomBinding.XrmTimeOfDayBinding._getFormatString$1();
        var container = $(element);
        var inputField = container.find('.sparkle-input-timeofday-part');
        var selectButton = container.find('.sparkle-input-timeofday-button-part');
        var options = SparkleXrm.GridEditor.XrmTimeEditor.getTimePickerAutoCompleteOptions(formatString);
        options.select = function(e, uiEvent) {
            var value = (uiEvent.item)['value'].toString();
            SparkleXrm.CustomBinding.XrmTimeOfDayBinding._trySetObservable$1(valueAccessor, inputField, value);
        };
        inputField = inputField.autocomplete(options);
        selectButton.click(function(e) {
            inputField.autocomplete('search', '');
        });
        ko.utils.registerEventHandler(inputField.get(0), 'change', function(sender, e) {
            var value = inputField.val();
            SparkleXrm.CustomBinding.XrmTimeOfDayBinding._trySetObservable$1(valueAccessor, inputField, value);
        });
        var disposeCallBack = function() {
            $(element).autocomplete("destroy");
        };
        ko.utils.domNodeDisposal.addDisposeCallback(element, disposeCallBack);
    },
    
    update: function SparkleXrm_CustomBinding_XrmTimeOfDayBinding$update(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var container = $(element);
        var inputField = container.find('.sparkle-input-timeofday-part');
        var value = ko.utils.unwrapObservable(valueAccessor());
        var formatString = SparkleXrm.CustomBinding.XrmTimeOfDayBinding._getFormatString$1();
        var formattedValue = Xrm.Sdk.DateTimeEx.formatTimeSpecific(value, formatString);
        inputField.val(formattedValue);
    }
}


Type.registerNamespace('SparkleXrm.GridEditor');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.SortCol

SparkleXrm.GridEditor.SortCol = function SparkleXrm_GridEditor_SortCol(attributeName, ascending) {
    this.attributeName = attributeName;
    this.ascending = ascending;
}
SparkleXrm.GridEditor.SortCol.prototype = {
    attributeName: null,
    ascending: false
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmBooleanEditor

SparkleXrm.GridEditor.XrmBooleanEditor = function SparkleXrm_GridEditor_XrmBooleanEditor(args) {
    SparkleXrm.GridEditor.XrmBooleanEditor.initializeBase(this, [ args ]);
    this._input$1 = $("<input type='checkbox' class='editor-boolean'/>").appendTo(args.container).bind('keydown.nav', function(e) {
        if (e.which === 37 || e.which === 39) {
            e.stopImmediatePropagation();
        }
    }).focus().select();
}
SparkleXrm.GridEditor.XrmBooleanEditor.bindColumn = function SparkleXrm_GridEditor_XrmBooleanEditor$bindColumn(column, TrueOptionDisplayName, FalseOptionDisplayName) {
    column.editor = SparkleXrm.GridEditor.XrmBooleanEditor.booleanEditor;
    column.formatter = SparkleXrm.GridEditor.XrmBooleanEditor.formatter;
    var opts = {};
    opts.trueOptionDisplayName = TrueOptionDisplayName;
    opts.falseOptionDisplayName = FalseOptionDisplayName;
    column.options = opts;
    return column;
}
SparkleXrm.GridEditor.XrmBooleanEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmBooleanEditor$bindReadOnlyColumn(column, TrueOptionDisplayName, FalseOptionDisplayName) {
    column.formatter = SparkleXrm.GridEditor.XrmBooleanEditor.formatter;
    var opts = {};
    opts.trueOptionDisplayName = TrueOptionDisplayName;
    opts.falseOptionDisplayName = FalseOptionDisplayName;
    column.options = opts;
    return column;
}
SparkleXrm.GridEditor.XrmBooleanEditor.formatter = function SparkleXrm_GridEditor_XrmBooleanEditor$formatter(row, cell, value, columnDef, dataContext) {
    var trueValue = 'True';
    var falseValue = 'False';
    var opts = columnDef.options;
    if (opts != null && opts.trueOptionDisplayName != null) {
        trueValue = opts.trueOptionDisplayName;
    }
    if (opts != null && opts.falseOptionDisplayName != null) {
        falseValue = opts.falseOptionDisplayName;
    }
    if (value != null) {
        return (value) ? trueValue : falseValue;
    }
    else {
        return falseValue;
    }
}
SparkleXrm.GridEditor.XrmBooleanEditor.prototype = {
    _input$1: null,
    _defaultValue$1: false,
    
    destroy: function SparkleXrm_GridEditor_XrmBooleanEditor$destroy() {
        SparkleXrm.GridEditor.XrmBooleanEditor.callBaseMethod(this, 'destroy');
        this._input$1.remove();
    },
    
    focus: function SparkleXrm_GridEditor_XrmBooleanEditor$focus() {
        SparkleXrm.GridEditor.XrmBooleanEditor.callBaseMethod(this, 'focus');
        this._input$1.focus();
    },
    
    _getValue$1: function SparkleXrm_GridEditor_XrmBooleanEditor$_getValue$1() {
        return this._input$1.is(':checked');
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmBooleanEditor$loadValue(item) {
        this._defaultValue$1 = item[this._args.column.field];
        if (this._defaultValue$1) {
            this._input$1[0].setAttribute('checked', 'checked');
        }
        else {
            this._input$1[0].removeAttribute('checked');
        }
        this._input$1[0].setAttribute('defaultValue', this._defaultValue$1);
        this._input$1.select();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmBooleanEditor$serializeValue() {
        return this._getValue$1();
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmBooleanEditor$applyValue(item, state) {
        item[this._args.column.field] = state;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmBooleanEditor$isValueChanged() {
        var val = this._getValue$1();
        return (val !== this._defaultValue$1);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmMoneyEditor

SparkleXrm.GridEditor.XrmMoneyEditor = function SparkleXrm_GridEditor_XrmMoneyEditor(args) {
    SparkleXrm.GridEditor.XrmMoneyEditor.initializeBase(this, [ args ]);
    this._numberFormatInfo$1 = args.column.options;
    this._currencySymbol$1 = $('<SPAN/>').appendTo(args.container);
    this._input$1 = $("<INPUT type=text class='editor-text' />").appendTo(args.container).bind('keydown.nav', function(e) {
        if (e.which === 37 || e.which === 39) {
            e.stopImmediatePropagation();
        }
    }).focus().select();
}
SparkleXrm.GridEditor.XrmMoneyEditor.formatter = function SparkleXrm_GridEditor_XrmMoneyEditor$formatter(row, cell, value, columnDef, dataContext) {
    if (value != null) {
        var currencySymbol = SparkleXrm.GridEditor.XrmMoneyEditor.getCurrencySymbol((dataContext).transactioncurrencyid);
        var numeric = value;
        return currencySymbol + ' ' + Xrm.NumberEx.format(numeric.value, columnDef.options);
    }
    else {
        return '';
    }
}
SparkleXrm.GridEditor.XrmMoneyEditor.bindColumn = function SparkleXrm_GridEditor_XrmMoneyEditor$bindColumn(column, minValue, maxValue) {
    column.editor = SparkleXrm.GridEditor.XrmMoneyEditor.moneyEditor;
    column.formatter = SparkleXrm.GridEditor.XrmMoneyEditor.formatter;
    var numberFormatInfo = Xrm.NumberEx.getCurrencyEditFormatInfo();
    numberFormatInfo.minValue = minValue;
    numberFormatInfo.maxValue = maxValue;
    column.options = numberFormatInfo;
    return column;
}
SparkleXrm.GridEditor.XrmMoneyEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmMoneyEditor$bindReadOnlyColumn(column) {
    column.formatter = SparkleXrm.GridEditor.XrmMoneyEditor.formatter;
    var numberFormatInfo = Xrm.NumberEx.getCurrencyEditFormatInfo();
    column.options = numberFormatInfo;
    return column;
}
SparkleXrm.GridEditor.XrmMoneyEditor.getCurrencySymbol = function SparkleXrm_GridEditor_XrmMoneyEditor$getCurrencySymbol(currencyid) {
    if (currencyid != null && currencyid.id != null && currencyid.id.value != null) {
        return Xrm.NumberEx.getCurrencySymbol(currencyid.id);
    }
    return '';
}
SparkleXrm.GridEditor.XrmMoneyEditor.prototype = {
    _input$1: null,
    _currencySymbol$1: null,
    _defaultValue$1: null,
    _numberFormatInfo$1: null,
    
    destroy: function SparkleXrm_GridEditor_XrmMoneyEditor$destroy() {
        SparkleXrm.GridEditor.XrmMoneyEditor.callBaseMethod(this, 'destroy');
        this._input$1.remove();
        this._currencySymbol$1.remove();
    },
    
    focus: function SparkleXrm_GridEditor_XrmMoneyEditor$focus() {
        SparkleXrm.GridEditor.XrmMoneyEditor.callBaseMethod(this, 'focus');
        this._input$1.focus();
    },
    
    getValue: function SparkleXrm_GridEditor_XrmMoneyEditor$getValue() {
        return this._input$1.val();
    },
    
    setValue: function SparkleXrm_GridEditor_XrmMoneyEditor$setValue(value) {
        this._input$1.val(value);
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmMoneyEditor$loadValue(item) {
        var currencySymbolString = SparkleXrm.GridEditor.XrmMoneyEditor.getCurrencySymbol(((item)).transactioncurrencyid);
        this._currencySymbol$1.text(currencySymbolString + ' ');
        var value = item[this._args.column.field];
        this._defaultValue$1 = '';
        if (value != null) {
            this._defaultValue$1 = Xrm.NumberEx.format(value.value, this._numberFormatInfo$1);
        }
        this._input$1.val(this._defaultValue$1);
        this._input$1[0].setAttribute('defaultValue', this._defaultValue$1);
        this._input$1.select();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmMoneyEditor$serializeValue() {
        return this._input$1.val();
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmMoneyEditor$applyValue(item, state) {
        var money = new Xrm.Sdk.Money(Xrm.NumberEx.parse(state, this._numberFormatInfo$1));
        item[this._args.column.field] = money;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmMoneyEditor$isValueChanged() {
        return (!(!this._input$1.val() && this._defaultValue$1 == null)) && (this._input$1.val() !== this._defaultValue$1);
    },
    
    nativeValidation: function SparkleXrm_GridEditor_XrmMoneyEditor$nativeValidation(newValue) {
        var isValid = true;
        var newValueNumber = Xrm.NumberEx.parse(newValue, this._numberFormatInfo$1);
        isValid = !isNaN(newValueNumber);
        isValid = isValid && (newValueNumber >= this._numberFormatInfo$1.minValue) && (newValueNumber <= this._numberFormatInfo$1.maxValue);
        if (!isValid) {
            var result = {};
            result.valid = false;
            result.message = String.format('Please enter a number between {0} and {1}.', this._numberFormatInfo$1.minValue, this._numberFormatInfo$1.maxValue);
            return result;
        }
        return null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmNumberEditor

SparkleXrm.GridEditor.XrmNumberEditor = function SparkleXrm_GridEditor_XrmNumberEditor(args) {
    SparkleXrm.GridEditor.XrmNumberEditor.initializeBase(this, [ args ]);
    this._numberFormatInfo$1 = args.column.options;
    this._input$1 = $("<INPUT type=text class='editor-text' />").appendTo(args.container).bind('keydown.nav', function(e) {
        if (e.which === 37 || e.which === 39) {
            e.stopImmediatePropagation();
        }
    }).focus().select();
}
SparkleXrm.GridEditor.XrmNumberEditor.formatter = function SparkleXrm_GridEditor_XrmNumberEditor$formatter(row, cell, value, columnDef, dataContext) {
    if (value != null) {
        var numeric = value;
        return Xrm.NumberEx.format(numeric, columnDef.options);
    }
    else {
        return '';
    }
}
SparkleXrm.GridEditor.XrmNumberEditor.bindColumn = function SparkleXrm_GridEditor_XrmNumberEditor$bindColumn(column, minValue, maxValue, precision) {
    column.editor = SparkleXrm.GridEditor.XrmNumberEditor.numberEditor;
    column.formatter = SparkleXrm.GridEditor.XrmNumberEditor.formatter;
    var numberFormatInfo = Xrm.NumberEx.getNumberFormatInfo();
    numberFormatInfo.minValue = minValue;
    numberFormatInfo.maxValue = maxValue;
    numberFormatInfo.precision = precision;
    column.options = numberFormatInfo;
    return column;
}
SparkleXrm.GridEditor.XrmNumberEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmNumberEditor$bindReadOnlyColumn(column, precision) {
    column.formatter = SparkleXrm.GridEditor.XrmNumberEditor.formatter;
    var numberFormatInfo = Xrm.NumberEx.getNumberFormatInfo();
    numberFormatInfo.precision = precision;
    column.options = numberFormatInfo;
    return column;
}
SparkleXrm.GridEditor.XrmNumberEditor.prototype = {
    _input$1: null,
    _defaultValue$1: null,
    _numberFormatInfo$1: null,
    
    destroy: function SparkleXrm_GridEditor_XrmNumberEditor$destroy() {
        SparkleXrm.GridEditor.XrmNumberEditor.callBaseMethod(this, 'destroy');
        this._input$1.remove();
    },
    
    focus: function SparkleXrm_GridEditor_XrmNumberEditor$focus() {
        SparkleXrm.GridEditor.XrmNumberEditor.callBaseMethod(this, 'focus');
        this._input$1.focus();
    },
    
    getValue: function SparkleXrm_GridEditor_XrmNumberEditor$getValue() {
        return this._input$1.val();
    },
    
    setValue: function SparkleXrm_GridEditor_XrmNumberEditor$setValue(value) {
        this._input$1.val(value);
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmNumberEditor$loadValue(item) {
        this._defaultValue$1 = Xrm.NumberEx.format(item[this._args.column.field], this._numberFormatInfo$1);
        if (this._defaultValue$1 == null) {
            this._defaultValue$1 = '';
        }
        this._input$1.val(this._defaultValue$1);
        this._input$1[0].setAttribute('defaultValue', this._defaultValue$1);
        this._input$1.select();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmNumberEditor$serializeValue() {
        return this._input$1.val();
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmNumberEditor$applyValue(item, state) {
        item[this._args.column.field] = Xrm.NumberEx.parse(state, this._numberFormatInfo$1);
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmNumberEditor$isValueChanged() {
        return (!(!this._input$1.val() && this._defaultValue$1 == null)) && (this._input$1.val() !== this._defaultValue$1);
    },
    
    nativeValidation: function SparkleXrm_GridEditor_XrmNumberEditor$nativeValidation(newValue) {
        var isValid = true;
        var newValueNumber = Xrm.NumberEx.parse(newValue, this._numberFormatInfo$1);
        isValid = !isNaN(newValueNumber);
        isValid = isValid && (newValueNumber >= this._numberFormatInfo$1.minValue) && (newValueNumber <= this._numberFormatInfo$1.maxValue);
        if (!isValid) {
            var result = {};
            result.valid = false;
            result.message = String.format('Please enter a number between {0} and {1}.', this._numberFormatInfo$1.minValue, this._numberFormatInfo$1.maxValue);
            return result;
        }
        return null;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.EntityDataViewModel

SparkleXrm.GridEditor.EntityDataViewModel = function SparkleXrm_GridEditor_EntityDataViewModel(pageSize, entityType, lazyLoadPages) {
    this._rows = new Array(0);
    this._sortCols = [];
    SparkleXrm.GridEditor.EntityDataViewModel.initializeBase(this);
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
SparkleXrm.GridEditor.EntityDataViewModel.prototype = {
    _suspendRefresh: false,
    _data: null,
    _entityType: null,
    _fetchXml: '',
    _itemAdded: false,
    _lazyLoadPages: true,
    errorMessage: '',
    deleteData: null,
    
    add_onBeginClearPageCache: function SparkleXrm_GridEditor_EntityDataViewModel$add_onBeginClearPageCache(value) {
        this.__onBeginClearPageCache$1 = ss.Delegate.combine(this.__onBeginClearPageCache$1, value);
    },
    remove_onBeginClearPageCache: function SparkleXrm_GridEditor_EntityDataViewModel$remove_onBeginClearPageCache(value) {
        this.__onBeginClearPageCache$1 = ss.Delegate.remove(this.__onBeginClearPageCache$1, value);
    },
    
    __onBeginClearPageCache$1: null,
    
    get_fetchXml: function SparkleXrm_GridEditor_EntityDataViewModel$get_fetchXml() {
        return this._fetchXml;
    },
    set_fetchXml: function SparkleXrm_GridEditor_EntityDataViewModel$set_fetchXml(value) {
        this._fetchXml = value;
        return value;
    },
    
    getItem: function SparkleXrm_GridEditor_EntityDataViewModel$getItem(index) {
        if (index >= this.paging.pageSize) {
            return null;
        }
        else {
            return this._data[index + (this.paging.pageNum * this.paging.pageSize)];
        }
    },
    
    reset: function SparkleXrm_GridEditor_EntityDataViewModel$reset() {
        this.clearPageCache();
        this.deleteData = [];
    },
    
    resetPaging: function SparkleXrm_GridEditor_EntityDataViewModel$resetPaging() {
        this.paging.pageNum = 0;
    },
    
    sort: function SparkleXrm_GridEditor_EntityDataViewModel$sort(sorting) {
        var col = new SparkleXrm.GridEditor.SortCol(sorting.sortCol.field, sorting.sortAsc);
        this.sortBy(col);
    },
    
    sortBy: function SparkleXrm_GridEditor_EntityDataViewModel$sortBy(col) {
        this._sortCols.clear();
        this._sortCols.add(col);
        if (this._lazyLoadPages) {
            this.clearPageCache();
            this.paging.extraInfo = '';
            this.refresh();
        }
        else {
            if (!col.ascending) {
                this._data.reverse();
            }
            this._data.sort(function(a, b) {
                return Xrm.Sdk.Entity.sortDelegate(col.attributeName, a, b);
            });
            if (!col.ascending) {
                this._data.reverse();
            }
        }
    },
    
    getDirtyItems: function SparkleXrm_GridEditor_EntityDataViewModel$getDirtyItems() {
        var dirtyCollection = [];
        var $enum1 = ss.IEnumerator.getEnumerator(this._data);
        while ($enum1.moveNext()) {
            var item = $enum1.current;
            if (item != null && item.entityState !== Xrm.Sdk.EntityStates.unchanged) {
                dirtyCollection.add(item);
            }
        }
        if (this.deleteData != null) {
            var $enum2 = ss.IEnumerator.getEnumerator(this.deleteData);
            while ($enum2.moveNext()) {
                var item = $enum2.current;
                if (item.entityState === Xrm.Sdk.EntityStates.deleted) {
                    dirtyCollection.add(item);
                }
            }
        }
        return dirtyCollection;
    },
    
    contains: function SparkleXrm_GridEditor_EntityDataViewModel$contains(Item) {
        var $enum1 = ss.IEnumerator.getEnumerator(this._data);
        while ($enum1.moveNext()) {
            var value = $enum1.current;
            if (Item.logicalName === value.logicalName && Item.id === value.id) {
                return true;
            }
        }
        return false;
    },
    
    refresh: function SparkleXrm_GridEditor_EntityDataViewModel$refresh() {
        if (this._suspendRefresh) {
            return;
        }
        this._suspendRefresh = true;
        var firstRowIndex = this.paging.pageNum * this.paging.pageSize;
        var allDataDeleted = (!this.paging.totalRows) && (this.deleteData != null) && (this.deleteData.length > 0);
        if (this._data[firstRowIndex] == null && !allDataDeleted) {
            this.onDataLoading.notify(null, null, null);
            var orderBy = this.applySorting();
            var fetchPageSize;
            if (this._lazyLoadPages) {
                fetchPageSize = this.paging.pageSize;
            }
            else {
                fetchPageSize = 1000;
                this.paging.extraInfo = '';
                this.paging.pageNum = 0;
                firstRowIndex = 0;
            }
            if (String.isNullOrEmpty(this._fetchXml)) {
                return;
            }
            var parameterisedFetchXml = String.format(this._fetchXml, fetchPageSize, Xrm.Sdk.XmlHelper.encode(this.paging.extraInfo), this.paging.pageNum + 1, orderBy);
            Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(parameterisedFetchXml, ss.Delegate.create(this, function(result) {
                try {
                    var results = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, this._entityType);
                    var i = firstRowIndex;
                    if (this._lazyLoadPages) {
                        var $enum1 = ss.IEnumerator.getEnumerator(results.get_entities());
                        while ($enum1.moveNext()) {
                            var e = $enum1.current;
                            this._data[i] = e;
                            i = i + 1;
                        }
                    }
                    else {
                        this._data = results.get_entities().items();
                    }
                    var args = {};
                    args.from = 0;
                    args.to = this.paging.pageSize - 1;
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
                    this.calculatePaging(this.getPagingInfo());
                    this.onPagingInfoChanged.notify(this.paging, null, this);
                    this.onDataLoaded.notify(args, null, null);
                }
                catch (ex) {
                    this.errorMessage = ex.message;
                    var args = {};
                    args.errorMessage = ex.message;
                    this.onDataLoaded.notify(args, null, null);
                }
            }));
        }
        else {
            var args = {};
            args.from = 0;
            args.to = this.paging.pageSize - 1;
            this.paging.fromRecord = firstRowIndex + 1;
            this.paging.toRecord = Math.min(this.paging.totalRows, firstRowIndex + this.paging.pageSize);
            this.calculatePaging(this.getPagingInfo());
            this.onPagingInfoChanged.notify(this.paging, null, this);
            this.onDataLoaded.notify(args, null, null);
            this._itemAdded = false;
        }
        this.onRowsChanged.notify(null, null, this);
        this._suspendRefresh = false;
    },
    
    newItemFactory: null,
    
    removeItem: function SparkleXrm_GridEditor_EntityDataViewModel$removeItem(id) {
        if (id != null) {
            if (this.deleteData == null) {
                this.deleteData = [];
            }
            this.deleteData.add(id);
            this._data.remove(id);
            this.paging.totalRows--;
            this.setPagingOptions(this.getPagingInfo());
            this._selectedRows = null;
            this.raiseOnSelectedRowsChanged(null);
        }
    },
    
    addItem: function SparkleXrm_GridEditor_EntityDataViewModel$addItem(newItem) {
        if (!this.paging.totalPages) {
            this.paging.pageNum = 0;
            this.paging.totalPages = 1;
        }
        var item;
        if (this.newItemFactory == null) {
            item = new this._entityType();
            $.extend(item, newItem);
        }
        else {
            item = this.newItemFactory(newItem);
        }
        this._data[this.paging.totalRows] = (item);
        this._itemAdded = true;
        var lastPageSize = (this.paging.totalRows % this.paging.pageSize);
        if (lastPageSize === this.paging.pageSize) {
            this.paging.totalPages++;
            this.paging.pageNum = this.paging.totalPages - 1;
        }
        else {
            this.paging.totalRows++;
            this.paging.pageNum = this.getTotalPages();
        }
        item.raisePropertyChanged(null);
        this.setPagingOptions(this.getPagingInfo());
    },
    
    applySorting: function SparkleXrm_GridEditor_EntityDataViewModel$applySorting() {
        var orderBy = '';
        var $enum1 = ss.IEnumerator.getEnumerator(this._sortCols);
        while ($enum1.moveNext()) {
            var col = $enum1.current;
            orderBy = orderBy + String.format('<order attribute="{0}" descending="{1}" />', col.attributeName, (!col.ascending) ? 'true' : 'false');
        }
        return orderBy;
    },
    
    clearPageCache: function SparkleXrm_GridEditor_EntityDataViewModel$clearPageCache() {
        if (this.__onBeginClearPageCache$1 != null) {
            this.__onBeginClearPageCache$1();
        }
        this._data = [];
    },
    
    get_data: function SparkleXrm_GridEditor_EntityDataViewModel$get_data() {
        return this._data;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmDateEditor

SparkleXrm.GridEditor.XrmDateEditor = function SparkleXrm_GridEditor_XrmDateEditor(args) {
    SparkleXrm.GridEditor.XrmDateEditor.initializeBase(this, [ args ]);
    this._container$1 = $("<div ><table class='inline-edit-container' cellspacing='0' cellpadding='0'><tr>" + "<td><INPUT type=text class='sparkle-input-inline' /></td>" + "<td class='lookup-button-td'><input type=button class='sparkle-imagestrip-inlineedit_calendar_icon' /></td></tr></table></div>");
    this._container$1.appendTo(this._args.container);
    this._input$1 = this._container$1.find('.sparkle-input-inline');
    var selectButton = this._container$1.find('.sparkle-imagestrip-inlineedit_calendar_icon');
    this._input$1.focus().select();
    var options2 = {};
    options2.showOtherMonths = true;
    options2.firstDay = (Xrm.Sdk.OrganizationServiceProxy.organizationSettings != null) ? Xrm.Sdk.OrganizationServiceProxy.organizationSettings.weekstartdaycode.value : 0;
    options2.beforeShow = ss.Delegate.create(this, function() {
        this._calendarOpen$1 = true;
    });
    options2.onClose = ss.Delegate.create(this, function() {
        this._calendarOpen$1 = false;
        this._selectedValue$1 = this._getSelectedValue$1();
    });
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        this._dateFormat$1 = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring;
    }
    options2.dateFormat = this._dateFormat$1;
    this._input$1.datepicker(options2);
    selectButton.click(ss.Delegate.create(this, function(e) {
        this._input$1.datepicker('show');
    }));
}
SparkleXrm.GridEditor.XrmDateEditor.formatterDateOnly = function SparkleXrm_GridEditor_XrmDateEditor$formatterDateOnly(row, cell, value, columnDef, dataContext) {
    var dateFormat = columnDef.options;
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        dateFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring;
    }
    var dateValue = value;
    return Xrm.Sdk.DateTimeEx.formatDateSpecific(dateValue, dateFormat);
}
SparkleXrm.GridEditor.XrmDateEditor.formatterDateAndTime = function SparkleXrm_GridEditor_XrmDateEditor$formatterDateAndTime(row, cell, value, columnDef, dataContext) {
    var dateFormat = columnDef.options;
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        dateFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring + ' ' + Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring;
    }
    var dateValue = value;
    return Xrm.Sdk.DateTimeEx.formatDateSpecific(dateValue, dateFormat);
}
SparkleXrm.GridEditor.XrmDateEditor.bindColumn = function SparkleXrm_GridEditor_XrmDateEditor$bindColumn(column, dateOnly) {
    column.editor = SparkleXrm.GridEditor.XrmDateEditor.crmDateEditor;
    column.formatter = SparkleXrm.GridEditor.XrmDateEditor.formatterDateOnly;
    return column;
}
SparkleXrm.GridEditor.XrmDateEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmDateEditor$bindReadOnlyColumn(column, dateOnly) {
    column.formatter = SparkleXrm.GridEditor.XrmDateEditor.formatterDateOnly;
    return column;
}
SparkleXrm.GridEditor.XrmDateEditor.prototype = {
    _input$1: null,
    _container$1: null,
    _defaultValue$1: null,
    _calendarOpen$1: false,
    _selectedValue$1: null,
    _dateFormat$1: 'dd/mm/yy',
    
    destroy: function SparkleXrm_GridEditor_XrmDateEditor$destroy() {
        ($.datepicker.dpDiv).stop(true, true);
        this._input$1.datepicker('hide');
        this._input$1.datepicker('destroy');
        this.hide();
        this._container$1.remove();
    },
    
    show: function SparkleXrm_GridEditor_XrmDateEditor$show() {
        if (this._calendarOpen$1) {
            ($.datepicker.dpDiv).stop(true, true).show();
        }
    },
    
    hide: function SparkleXrm_GridEditor_XrmDateEditor$hide() {
        if (this._calendarOpen$1) {
            (s.datepicker.dpDiv).stop(true, true).hide();
        }
    },
    
    position: function SparkleXrm_GridEditor_XrmDateEditor$position(position) {
        if (!this._calendarOpen$1) {
            return;
        }
        ($.datepicker.dpDiv).css('top', (position.top + 30).toString()).css('left', position.left.toString());
    },
    
    focus: function SparkleXrm_GridEditor_XrmDateEditor$focus() {
        this._input$1.focus();
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmDateEditor$loadValue(item) {
        var currentValue = item[this._args.column.field];
        this._defaultValue$1 = (currentValue != null) ? currentValue : null;
        var valueFormated = (this._defaultValue$1 != null) ? this._defaultValue$1.toLocaleDateString() : '';
        if (this._args.column.formatter != null) {
            valueFormated = this._args.column.formatter(0, 0, this._defaultValue$1, this._args.column, null);
        }
        this._setSelectedValue$1(this._defaultValue$1);
        this._input$1.select();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmDateEditor$serializeValue() {
        return this._getSelectedValue$1();
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmDateEditor$applyValue(item, state) {
        var previousValue = item[this._args.column.field];
        var newValue = state;
        Xrm.Sdk.DateTimeEx.setTime(newValue, previousValue);
        item[this._args.column.field] = newValue;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmDateEditor$isValueChanged() {
        var selectedValue = this._getSelectedValue$1();
        var selected = (selectedValue == null) ? '' : selectedValue.toString();
        var defaultValueString = (this._defaultValue$1 == null) ? '' : this._defaultValue$1.toString();
        return (selected !== defaultValueString);
    },
    
    _getSelectedValue$1: function SparkleXrm_GridEditor_XrmDateEditor$_getSelectedValue$1() {
        var selectedValue = this._input$1.datepicker('getDate');
        return selectedValue;
    },
    
    _setSelectedValue$1: function SparkleXrm_GridEditor_XrmDateEditor$_setSelectedValue$1(date) {
        this._input$1.datepicker('setDate', date);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmDurationEditor

SparkleXrm.GridEditor.XrmDurationEditor = function SparkleXrm_GridEditor_XrmDurationEditor(args) {
    SparkleXrm.GridEditor.XrmDurationEditor.initializeBase(this, [ args ]);
    this._args = args;
    this._input$1 = $("<INPUT type=text class='editor-text' />");
    this._input$1.appendTo(this._args.container);
    this.focus();
}
SparkleXrm.GridEditor.XrmDurationEditor.formatter = function SparkleXrm_GridEditor_XrmDurationEditor$formatter(row, cell, value, columnDef, dataContext) {
    var durationValue = value;
    return Xrm.Sdk.DateTimeEx.formatDuration(durationValue);
}
SparkleXrm.GridEditor.XrmDurationEditor.bindColumn = function SparkleXrm_GridEditor_XrmDurationEditor$bindColumn(column) {
    column.editor = SparkleXrm.GridEditor.XrmDurationEditor.durationEditor;
    column.formatter = SparkleXrm.GridEditor.XrmDurationEditor.formatter;
    return column;
}
SparkleXrm.GridEditor.XrmDurationEditor.prototype = {
    _input$1: null,
    _originalValue$1: null,
    
    destroy: function SparkleXrm_GridEditor_XrmDurationEditor$destroy() {
        this._input$1.remove();
    },
    
    focus: function SparkleXrm_GridEditor_XrmDurationEditor$focus() {
        this._input$1.focus().select();
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmDurationEditor$loadValue(item) {
        var value = item[this._args.column.field];
        this._input$1.val(Xrm.Sdk.DateTimeEx.formatDuration(value));
        this._originalValue$1 = value;
        this.focus();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmDurationEditor$serializeValue() {
        var durationString = this._input$1.val();
        if (!durationString) {
            return null;
        }
        var duration = Xrm.Sdk.DateTimeEx.parseDuration(durationString);
        return duration;
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmDurationEditor$applyValue(item, state) {
        item[this._args.column.field] = state;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmDurationEditor$isValueChanged() {
        var durationString = this._input$1.val();
        var originalDurationString = Xrm.Sdk.DateTimeEx.formatDuration(this._originalValue$1);
        return originalDurationString !== durationString;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmLookupEditorOptions

SparkleXrm.GridEditor.XrmLookupEditorOptions = function SparkleXrm_GridEditor_XrmLookupEditorOptions() {
}
SparkleXrm.GridEditor.XrmLookupEditorOptions.prototype = {
    queryCommand: null,
    nameAttribute: null,
    idAttribute: null,
    typeCodeAttribute: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmLookupEditor

SparkleXrm.GridEditor.XrmLookupEditor = function SparkleXrm_GridEditor_XrmLookupEditor(args) {
    this._value$1 = new Xrm.Sdk.EntityReference(null, null, null);
    this._originalValue$1 = new Xrm.Sdk.EntityReference(null, null, null);
    SparkleXrm.GridEditor.XrmLookupEditor.initializeBase(this, [ args ]);
    this._args = args;
    this._container$1 = $("<div ><table class='inline-edit-container' cellspacing='0' cellpadding='0'><tr><td><INPUT type=text class='sparkle-input-inline' /></td><td class='lookup-button-td'><input type=button class='sparkle-lookup-button' /></td></tr></table></div>");
    this._container$1.appendTo(this._args.container);
    var inputField = this._container$1.find('.sparkle-input-inline');
    var selectButton = this._container$1.find('.sparkle-lookup-button');
    this._input$1 = inputField;
    this._input$1.focus().select();
    this._autoComplete$1 = inputField;
    var options = {};
    options.minLength = 100000;
    options.delay = 0;
    var justSelected = false;
    options.select = ss.Delegate.create(this, function(e, uiEvent) {
        if (this._value$1 == null) {
            this._value$1 = new Xrm.Sdk.EntityReference(null, null, null);
        }
        var item = uiEvent.item;
        var value = item.label;
        this._input$1.val(value);
        this._value$1.id = (item.value).id;
        this._value$1.name = (item.value).name;
        this._value$1.logicalName = (item.value).logicalName;
        justSelected = true;
        return false;;
    });
    options.focus = function(e, uiEvent) {
        return false;;
    };
    var editorOptions = args.column.options;
    var queryDelegate = ss.Delegate.create(this, function(request, response) {
        editorOptions.queryCommand(request.term, ss.Delegate.create(this, function(fetchResult) {
            var results = new Array(fetchResult.get_entities().get_count());
            for (var i = 0; i < fetchResult.get_entities().get_count(); i++) {
                results[i] = {};
                results[i].label = fetchResult.get_entities().get_item(i).getAttributeValue(editorOptions.nameAttribute);
                var id = new Xrm.Sdk.EntityReference(null, null, null);
                id.name = results[i].label;
                id.logicalName = fetchResult.get_entities().get_item(i).logicalName;
                id.id = fetchResult.get_entities().get_item(i).getAttributeValue(editorOptions.idAttribute);
                results[i].value = id;
                var typeCodeName = fetchResult.get_entities().get_item(i).logicalName;
                if (!String.isNullOrEmpty(editorOptions.typeCodeAttribute)) {
                    typeCodeName = fetchResult.get_entities().get_item(i).getAttributeValue(editorOptions.typeCodeAttribute).toString();
                }
                results[i].image = Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl(typeCodeName);
            }
            response(results);
            var disableOption = {};
            disableOption.minLength = 100000;
            this._autoComplete$1.autocomplete(disableOption);
        }));
    });
    options.source = queryDelegate;
    inputField = this._autoComplete$1.autocomplete(options);
    (inputField.data('ui-autocomplete'))._renderItem = function(ul, item) {
        return $('<li>').append("<a class='sparkle-menu-item'><span class='sparkle-menu-item-img'><img src='" + item.image + "'/></span><span class='sparkle-menu-item-label'>" + item.label + '</span></a>').appendTo(ul);
    };
    selectButton.click(ss.Delegate.create(this, function(e) {
        var enableOption = {};
        enableOption.minLength = 0;
        this._autoComplete$1.autocomplete(enableOption);
        this._autoComplete$1.autocomplete('search', inputField.val());
    }));
    this._input$1.keydown(ss.Delegate.create(this, function(e) {
        if (e.which === 13 && !justSelected) {
            if (inputField.val().length > 0) {
                selectButton.click();
            }
            else {
                this._value$1 = null;
                return;
            }
        }
        else if (e.which === 13) {
            return;
        }
        switch (e.which) {
            case 13:
            case 38:
            case 40:
                e.preventDefault();
                e.stopPropagation();
                break;
        }
        justSelected = false;
    }));
}
SparkleXrm.GridEditor.XrmLookupEditor.formatter = function SparkleXrm_GridEditor_XrmLookupEditor$formatter(row, cell, value, columnDef, dataContext) {
    if (value != null) {
        var entityRef = value;
        return "<a href='#' class='sparkle-lookup-link' entityid='" + entityRef.id + "' typename='" + entityRef.logicalName + "'>" + Xrm.Sdk.XmlHelper.encode(entityRef.name) + '</a>';
    }
    else {
        return '';
    }
}
SparkleXrm.GridEditor.XrmLookupEditor.bindColumn = function SparkleXrm_GridEditor_XrmLookupEditor$bindColumn(column, queryCommand, idAttribute, nameAttribute, typeCodeAttribute) {
    column.editor = SparkleXrm.GridEditor.XrmLookupEditor.lookupEditor;
    var currencyLookupOptions = new SparkleXrm.GridEditor.XrmLookupEditorOptions();
    currencyLookupOptions.queryCommand = queryCommand;
    currencyLookupOptions.idAttribute = idAttribute;
    currencyLookupOptions.nameAttribute = nameAttribute;
    currencyLookupOptions.typeCodeAttribute = typeCodeAttribute;
    column.options = currencyLookupOptions;
    column.formatter = SparkleXrm.GridEditor.XrmLookupEditor.formatter;
    return column;
}
SparkleXrm.GridEditor.XrmLookupEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmLookupEditor$bindReadOnlyColumn(column, typeCodeAttribute) {
    var currencyLookupOptions = new SparkleXrm.GridEditor.XrmLookupEditorOptions();
    currencyLookupOptions.typeCodeAttribute = typeCodeAttribute;
    column.options = currencyLookupOptions;
    column.formatter = SparkleXrm.GridEditor.XrmLookupEditor.formatter;
    return column;
}
SparkleXrm.GridEditor.XrmLookupEditor.prototype = {
    _input$1: null,
    _container$1: null,
    _autoComplete$1: null,
    
    destroy: function SparkleXrm_GridEditor_XrmLookupEditor$destroy() {
        this._input$1.autocomplete('close');
        this._input$1.autocomplete('destroy');
        this._container$1.remove();
        this._autoComplete$1.remove();
        this._autoComplete$1 = null;
    },
    
    show: function SparkleXrm_GridEditor_XrmLookupEditor$show() {
    },
    
    hide: function SparkleXrm_GridEditor_XrmLookupEditor$hide() {
    },
    
    position: function SparkleXrm_GridEditor_XrmLookupEditor$position(position) {
    },
    
    focus: function SparkleXrm_GridEditor_XrmLookupEditor$focus() {
        this._input$1.focus();
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmLookupEditor$loadValue(item) {
        this._originalValue$1 = item[this._args.column.field];
        this._value$1 = this._originalValue$1;
        if (this._originalValue$1 != null) {
            this._input$1.val(this._originalValue$1.name);
        }
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmLookupEditor$serializeValue() {
        return this._value$1;
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmLookupEditor$applyValue(item, state) {
        item[this._args.column.field] = state;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmLookupEditor$isValueChanged() {
        if (this._originalValue$1 != null && this._value$1 != null) {
            return this._originalValue$1.id !== this._value$1.id;
        }
        else {
            return ((this._originalValue$1 != null) || (this._value$1 != null));
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.CrmPagerControl

SparkleXrm.GridEditor.CrmPagerControl = function SparkleXrm_GridEditor_CrmPagerControl(dataView, grid, container) {
    this._dataView = dataView;
    this._grid = grid;
    this._container = container;
    $(ss.Delegate.create(this, function() {
        this.init();
    }));
}
SparkleXrm.GridEditor.CrmPagerControl.prototype = {
    _dataView: null,
    _grid: null,
    _container: null,
    
    init: function SparkleXrm_GridEditor_CrmPagerControl$init() {
        this._dataView.onPagingInfoChanged.subscribe(ss.Delegate.create(this, function(e, pagingInfo) {
            this.updatePager(pagingInfo);
        }));
        this._dataView.add_onSelectedRowsChanged(ss.Delegate.create(this, this._dataView_OnSelectedRowsChanged));
        this.constructPagerUI();
        this.updatePager(this._dataView.getPagingInfo());
    },
    
    _dataView_OnSelectedRowsChanged: function SparkleXrm_GridEditor_CrmPagerControl$_dataView_OnSelectedRowsChanged() {
        this.updatePager(this._dataView.getPagingInfo());
    },
    
    getNavState: function SparkleXrm_GridEditor_CrmPagerControl$getNavState() {
        var cannotLeaveEditMode = !Slick.GlobalEditorLock.commitCurrentEdit();
        var pagingInfo = this._dataView.getPagingInfo();
        var lastPage = pagingInfo.totalPages - 1;
        var info = {};
        info.canGotoFirst = !cannotLeaveEditMode && !!pagingInfo.pageSize && pagingInfo.pageNum > 0;
        info.canGotoLast = !cannotLeaveEditMode && !!pagingInfo.pageSize && pagingInfo.pageNum !== lastPage;
        info.canGotoPrev = !cannotLeaveEditMode && !!pagingInfo.pageSize && pagingInfo.pageNum > 0;
        info.canGotoNext = !cannotLeaveEditMode && !!pagingInfo.pageSize && pagingInfo.pageNum < lastPage;
        info.pagingInfo = pagingInfo;
        return info;
    },
    
    setPageSize: function SparkleXrm_GridEditor_CrmPagerControl$setPageSize(n) {
        this._dataView.setRefreshHints({isFilterUnchanged: true});
        var paging = {};
        paging.pageSize = n;
        this._dataView.setPagingOptions(paging);
    },
    
    gotoFirst: function SparkleXrm_GridEditor_CrmPagerControl$gotoFirst(e) {
        if (this.getNavState().canGotoFirst) {
            var paging = {};
            paging.pageNum = 0;
            this._dataView.setPagingOptions(paging);
        }
    },
    
    gotoLast: function SparkleXrm_GridEditor_CrmPagerControl$gotoLast(e) {
        var state = this.getNavState();
        if (state.canGotoLast) {
            var paging = {};
            paging.pageNum = state.pagingInfo.totalPages - 1;
            this._dataView.setPagingOptions(paging);
        }
    },
    
    gotoPrev: function SparkleXrm_GridEditor_CrmPagerControl$gotoPrev(e) {
        var state = this.getNavState();
        if (state.canGotoPrev) {
            var paging = {};
            paging.pageNum = state.pagingInfo.pageNum - 1;
            this._dataView.setPagingOptions(paging);
        }
    },
    
    gotoNext: function SparkleXrm_GridEditor_CrmPagerControl$gotoNext(e) {
        var state = this.getNavState();
        if (state.canGotoNext) {
            var paging = {};
            paging.pageNum = state.pagingInfo.pageNum + 1;
            this._dataView.setPagingOptions(paging);
        }
    },
    
    constructPagerUI: function SparkleXrm_GridEditor_CrmPagerControl$constructPagerUI() {
        this._container.empty();
        var pager = $("<table cellspacing='0' cellpadding='0' class='sparkle-grid-status'><tbody><tr>" + "<td class='sparkle-grid-status-label'>1 - 1 of 1&nbsp;(0 selected)</td>" + "<td class='sparkle-grid-status-paging'>" + "<img src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-paging-first'>" + "<img src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-paging-back'>" + "<span class='sparkle-grid-status-paging-page'>Page 1</span>" + "<img src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-paging-next'>" + '&nbsp;</td></tr></tbody></table>');
        var firstButton = pager.find('.sparkle-grid-paging-first');
        var backButton = pager.find('.sparkle-grid-paging-back');
        var nextButton = pager.find('.sparkle-grid-paging-next');
        var label = pager.find('.sparkle-grid-status-label');
        var pageInfo = pager.find('.sparkle-grid-status-paging-page');
        this._container.append(pager);
        firstButton.click(ss.Delegate.create(this, this.gotoFirst));
        backButton.click(ss.Delegate.create(this, this.gotoPrev));
        nextButton.click(ss.Delegate.create(this, this.gotoNext));
    },
    
    updatePager: function SparkleXrm_GridEditor_CrmPagerControl$updatePager(pagingInfo) {
        var state = this.getNavState();
        var firstButton = this._container.find('.sparkle-grid-paging-first');
        var backButton = this._container.find('.sparkle-grid-paging-back');
        var nextButton = this._container.find('.sparkle-grid-paging-next');
        var label = this._container.find('.sparkle-grid-status-label');
        var pageInfo = this._container.find('.sparkle-grid-status-paging-page');
        var status = this._container.find('.sparkle-grid-status-label');
        if (state.canGotoFirst) {
            firstButton.removeClass('disabled');
        }
        else {
            firstButton.addClass('disabled');
        }
        if (state.canGotoPrev) {
            backButton.removeClass('disabled');
        }
        else {
            backButton.addClass('disabled');
        }
        if (state.canGotoNext) {
            nextButton.removeClass('disabled');
        }
        else {
            nextButton.addClass('disabled');
        }
        status.text(String.format('{0} - {1} of {2} ({3} selected)', pagingInfo.fromRecord, pagingInfo.toRecord, pagingInfo.totalRows, this._dataView.getSelectedRows().length.toString()));
        pageInfo.text(String.format('Page {0}', pagingInfo.pageNum + 1));
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmTextEditor

SparkleXrm.GridEditor.XrmTextEditor = function SparkleXrm_GridEditor_XrmTextEditor(args) {
    SparkleXrm.GridEditor.XrmTextEditor.initializeBase(this, [ args ]);
    this._input$1 = $("<INPUT type=text class='editor-text' />").appendTo(args.container).bind('keydown.nav', function(e) {
        if (e.which === 37 || e.which === 39) {
            e.stopImmediatePropagation();
        }
    }).focus().select();
}
SparkleXrm.GridEditor.XrmTextEditor.bindColumn = function SparkleXrm_GridEditor_XrmTextEditor$bindColumn(column) {
    column.editor = SparkleXrm.GridEditor.XrmTextEditor.textEditor;
    column.formatter = SparkleXrm.GridEditor.XrmTextEditor.formatter;
    return column;
}
SparkleXrm.GridEditor.XrmTextEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmTextEditor$bindReadOnlyColumn(column) {
    column.formatter = SparkleXrm.GridEditor.XrmTextEditor.formatter;
    return column;
}
SparkleXrm.GridEditor.XrmTextEditor.formatter = function SparkleXrm_GridEditor_XrmTextEditor$formatter(row, cell, value, columnDef, dataContext) {
    if (value != null) {
        return value;
    }
    else {
        return '';
    }
}
SparkleXrm.GridEditor.XrmTextEditor.prototype = {
    _input$1: null,
    _defaultValue$1: null,
    
    destroy: function SparkleXrm_GridEditor_XrmTextEditor$destroy() {
        SparkleXrm.GridEditor.XrmTextEditor.callBaseMethod(this, 'destroy');
        this._input$1.remove();
    },
    
    focus: function SparkleXrm_GridEditor_XrmTextEditor$focus() {
        SparkleXrm.GridEditor.XrmTextEditor.callBaseMethod(this, 'focus');
        this._input$1.focus();
    },
    
    getValue: function SparkleXrm_GridEditor_XrmTextEditor$getValue() {
        return this._input$1.val();
    },
    
    setValue: function SparkleXrm_GridEditor_XrmTextEditor$setValue(value) {
        this._input$1.val(value);
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmTextEditor$loadValue(item) {
        this._defaultValue$1 = item[this._args.column.field];
        if (this._defaultValue$1 == null) {
            this._defaultValue$1 = '';
        }
        this._input$1.val(this._defaultValue$1);
        this._input$1[0].setAttribute('defaultValue', this._defaultValue$1);
        this._input$1.select();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmTextEditor$serializeValue() {
        return this._input$1.val();
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmTextEditor$applyValue(item, state) {
        item[this._args.column.field] = state;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmTextEditor$isValueChanged() {
        return (!(!this._input$1.val() && this._defaultValue$1 == null)) && (this._input$1.val() !== this._defaultValue$1);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmTimeEditor

SparkleXrm.GridEditor.XrmTimeEditor = function SparkleXrm_GridEditor_XrmTimeEditor(args) {
    SparkleXrm.GridEditor.XrmTimeEditor.initializeBase(this, [ args ]);
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        this._formatString$1 = Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring;
    }
    this._container$1 = $("<div ><table class='inline-edit-container' cellspacing='0' cellpadding='0'><tr><td><INPUT type=text class='sparkle-input-inline' /></td><td class='lookup-button-td'><input type=button class='autocompleteButton' /></td></tr></table></div>");
    this._container$1.appendTo(this._args.container);
    var inputField = this._container$1.find('.sparkle-input-inline');
    this._input$1 = inputField;
    this._input$1.focus().select();
    var timeFormatString = this._formatString$1;
    var options = SparkleXrm.GridEditor.XrmTimeEditor.getTimePickerAutoCompleteOptions(timeFormatString);
    options.select = function(e, uiEvent) {
    };
    inputField = inputField.autocomplete(options);
    var selectButton = this._container$1.find('.autocompleteButton');
    selectButton.click(function(e) {
        inputField.autocomplete('search', '');
    });
}
SparkleXrm.GridEditor.XrmTimeEditor.formatter = function SparkleXrm_GridEditor_XrmTimeEditor$formatter(row, cell, value, columnDef, dataContext) {
    var dateValue = value;
    return SparkleXrm.GridEditor.XrmTimeEditor._formatTime$1(dateValue, columnDef.options);
}
SparkleXrm.GridEditor.XrmTimeEditor._formatTime$1 = function SparkleXrm_GridEditor_XrmTimeEditor$_formatTime$1(dateValue, format) {
    var timeFormatted = '';
    if (dateValue != null) {
        timeFormatted = dateValue.format(format);
    }
    return timeFormatted;
}
SparkleXrm.GridEditor.XrmTimeEditor.getTimePickerAutoCompleteOptions = function SparkleXrm_GridEditor_XrmTimeEditor$getTimePickerAutoCompleteOptions(timeFormatString) {
    var options = {};
    var timeOptions = new Array(48);
    var autoCompleteDate = Date.parseDate('2000-01-01T00:00:00');
    for (var i = 0; i < 48; i++) {
        timeOptions[i] = SparkleXrm.GridEditor.XrmTimeEditor._formatTime$1(autoCompleteDate, timeFormatString);
        autoCompleteDate = Xrm.Sdk.DateTimeEx.dateAdd('minutes', 30, autoCompleteDate);
    }
    options.source = timeOptions;
    options.minLength = 0;
    options.delay = 0;
    return options;
}
SparkleXrm.GridEditor.XrmTimeEditor.bindColumn = function SparkleXrm_GridEditor_XrmTimeEditor$bindColumn(column) {
    column.editor = SparkleXrm.GridEditor.XrmTimeEditor.timeEditor;
    column.formatter = SparkleXrm.GridEditor.XrmTimeEditor.formatter;
    column.options = Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring;
    return column;
}
SparkleXrm.GridEditor.XrmTimeEditor.bindReadOnlyColumn = function SparkleXrm_GridEditor_XrmTimeEditor$bindReadOnlyColumn(column) {
    column.formatter = SparkleXrm.GridEditor.XrmTimeEditor.formatter;
    column.options = Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring;
    return column;
}
SparkleXrm.GridEditor.XrmTimeEditor.prototype = {
    _input$1: null,
    _container$1: null,
    _dateTimeValue$1: null,
    _originalDateTimeValue$1: null,
    _formatString$1: 'h:mm tt',
    
    destroy: function SparkleXrm_GridEditor_XrmTimeEditor$destroy() {
        this._input$1.autocomplete('close');
        this._input$1.autocomplete('destroy');
        this._container$1.remove();
    },
    
    show: function SparkleXrm_GridEditor_XrmTimeEditor$show() {
    },
    
    hide: function SparkleXrm_GridEditor_XrmTimeEditor$hide() {
    },
    
    position: function SparkleXrm_GridEditor_XrmTimeEditor$position(position) {
    },
    
    focus: function SparkleXrm_GridEditor_XrmTimeEditor$focus() {
        this._input$1.focus();
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmTimeEditor$loadValue(item) {
        SparkleXrm.GridEditor.XrmTimeEditor.callBaseMethod(this, 'loadValue', [ item ]);
        this._dateTimeValue$1 = item[this._args.column.field];
        this._originalDateTimeValue$1 = this._dateTimeValue$1;
        this._input$1.val(SparkleXrm.GridEditor.XrmTimeEditor._formatTime$1(this._dateTimeValue$1, this._formatString$1));
        this._input$1.select();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmTimeEditor$serializeValue() {
        var timeString = this._input$1.val();
        if (!timeString) {
            return null;
        }
        var currentDate = Xrm.Sdk.DateTimeEx.addTimeToDate(this._dateTimeValue$1, timeString);
        return currentDate;
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmTimeEditor$applyValue(item, state) {
        item[this._args.column.field] = state;
        this.raiseOnChange(item);
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmTimeEditor$isValueChanged() {
        var timeString = this._input$1.val();
        var originalDateString = SparkleXrm.GridEditor.XrmTimeEditor._formatTime$1(this._originalDateTimeValue$1, this._formatString$1);
        var newDateString = '';
        if (!!timeString) {
            var currentDate = Xrm.Sdk.DateTimeEx.addTimeToDate(this._dateTimeValue$1, timeString);
            newDateString = SparkleXrm.GridEditor.XrmTimeEditor._formatTime$1(currentDate, this._formatString$1);
        }
        return originalDateString !== newDateString;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.DataViewBase

SparkleXrm.GridEditor.DataViewBase = function SparkleXrm_GridEditor_DataViewBase() {
    this.onRowsChanged = new Slick.Event();
    this.onPagingInfoChanged = new Slick.Event();
    this.onDataLoading = new Slick.Event();
    this.onDataLoaded = new Slick.Event();
    this.paging = {};
    this.validationBinder = new SparkleXrm.DataViewValidationBinder();
}
SparkleXrm.GridEditor.DataViewBase.rangesToRows = function SparkleXrm_GridEditor_DataViewBase$rangesToRows(ranges) {
    var rows = [];
    for (var i = 0; i < ranges.length; i++) {
        for (var j = ranges[i].fromRow; j <= ranges[i].toRow; j++) {
            rows.add(j);
        }
    }
    return rows;
}
SparkleXrm.GridEditor.DataViewBase.prototype = {
    
    add_onGetItemMetaData: function SparkleXrm_GridEditor_DataViewBase$add_onGetItemMetaData(value) {
        this.__onGetItemMetaData = ss.Delegate.combine(this.__onGetItemMetaData, value);
    },
    remove_onGetItemMetaData: function SparkleXrm_GridEditor_DataViewBase$remove_onGetItemMetaData(value) {
        this.__onGetItemMetaData = ss.Delegate.remove(this.__onGetItemMetaData, value);
    },
    
    __onGetItemMetaData: null,
    
    add_onSelectedRowsChanged: function SparkleXrm_GridEditor_DataViewBase$add_onSelectedRowsChanged(value) {
        this.__onSelectedRowsChanged = ss.Delegate.combine(this.__onSelectedRowsChanged, value);
    },
    remove_onSelectedRowsChanged: function SparkleXrm_GridEditor_DataViewBase$remove_onSelectedRowsChanged(value) {
        this.__onSelectedRowsChanged = ss.Delegate.remove(this.__onSelectedRowsChanged, value);
    },
    
    __onSelectedRowsChanged: null,
    _selectedRows: null,
    
    raiseOnSelectedRowsChanged: function SparkleXrm_GridEditor_DataViewBase$raiseOnSelectedRowsChanged(rows) {
        this._selectedRows = rows;
        if (this.__onSelectedRowsChanged != null) {
            this.__onSelectedRowsChanged();
        }
    },
    
    getSelectedRows: function SparkleXrm_GridEditor_DataViewBase$getSelectedRows() {
        if (this._selectedRows == null) {
            this._selectedRows = new Array(0);
        }
        return this._selectedRows;
    },
    
    raisePropertyChanged: function SparkleXrm_GridEditor_DataViewBase$raisePropertyChanged(propertyName) {
        this.onRowsChanged.notify(null, null, null);
    },
    
    getPagingInfo: function SparkleXrm_GridEditor_DataViewBase$getPagingInfo() {
        return this.paging;
    },
    
    calculatePaging: function SparkleXrm_GridEditor_DataViewBase$calculatePaging(p) {
        if (p.pageSize != null) {
            this.paging.pageSize = p.pageSize;
            this.paging.pageNum = (!!this.paging.pageSize) ? Math.min(this.paging.pageNum, Math.max(0, Math.ceil(this.paging.totalRows / this.paging.pageSize) - 1)) : 0;
        }
        if (p.pageNum != null) {
            this.paging.pageNum = Math.min(p.pageNum, Math.max(0, Math.ceil(this.paging.totalRows / this.paging.pageSize) - 1));
        }
        this.paging.totalPages = this.getTotalPages();
        this.paging.fromRecord = (this.paging.pageNum * this.paging.pageSize) + 1;
        this.paging.toRecord = this.paging.totalRows;
    },
    
    setPagingOptions: function SparkleXrm_GridEditor_DataViewBase$setPagingOptions(p) {
        this.calculatePaging(p);
        this._selectedRows = null;
        this.raiseOnSelectedRowsChanged(null);
        this.onPagingInfoChanged.notify(this.paging, null, this);
        this.refresh();
    },
    
    getTotalPages: function SparkleXrm_GridEditor_DataViewBase$getTotalPages() {
        return Math.ceil(this.paging.totalRows / this.paging.pageSize);
    },
    
    refresh: function SparkleXrm_GridEditor_DataViewBase$refresh() {
    },
    
    reset: function SparkleXrm_GridEditor_DataViewBase$reset() {
    },
    
    insertItem: function SparkleXrm_GridEditor_DataViewBase$insertItem(insertBefore, item) {
    },
    
    addItem: function SparkleXrm_GridEditor_DataViewBase$addItem(item) {
    },
    
    removeItem: function SparkleXrm_GridEditor_DataViewBase$removeItem(id) {
    },
    
    getLength: function SparkleXrm_GridEditor_DataViewBase$getLength() {
        return Math.min(this.paging.pageSize, this.paging.toRecord - this.paging.fromRecord + 1);
    },
    
    getItem: function SparkleXrm_GridEditor_DataViewBase$getItem(index) {
        return null;
    },
    
    getItemMetadata: function SparkleXrm_GridEditor_DataViewBase$getItemMetadata(i) {
        if (this.__onGetItemMetaData != null) {
            return this.__onGetItemMetaData(this.getItem(i));
        }
        else {
            return null;
        }
    },
    
    sort: function SparkleXrm_GridEditor_DataViewBase$sort(sorting) {
    },
    
    gridValidationIndexer: function SparkleXrm_GridEditor_DataViewBase$gridValidationIndexer() {
        return ss.Delegate.create(this.validationBinder, this.validationBinder.gridValidationIndexer);
    },
    
    onBeforeEdit: function SparkleXrm_GridEditor_DataViewBase$onBeforeEdit(item) {
        return true;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.Formatters

SparkleXrm.GridEditor.Formatters = function SparkleXrm_GridEditor_Formatters() {
}
SparkleXrm.GridEditor.Formatters.defaultFormatter = function SparkleXrm_GridEditor_Formatters$defaultFormatter(row, cell, value, columnDef, dataContext) {
    if (value == null) {
        return '';
    }
    else {
        return value.toString().replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;');
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.GridDataViewBinder

SparkleXrm.GridEditor.GridDataViewBinder = function SparkleXrm_GridEditor_GridDataViewBinder() {
}
SparkleXrm.GridEditor.GridDataViewBinder._freezeColumns = function SparkleXrm_GridEditor_GridDataViewBinder$_freezeColumns(grid, freeze) {
    var cols = grid.getColumns();
    for (var i = 0; i < cols.length - 1; i++) {
        var col = cols[i];
        if (freeze) {
            col.maxWidth = col.width;
            col.minWidth = col.width;
        }
        else {
            col.maxWidth = null;
            col.minWidth = null;
        }
    }
}
SparkleXrm.GridEditor.GridDataViewBinder._showLoadingIndicator = function SparkleXrm_GridEditor_GridDataViewBinder$_showLoadingIndicator(loadingIndicator, gridContainerDivId) {
    var g = $('#' + gridContainerDivId);
    var vp = $('#' + gridContainerDivId + ' > .slick-viewport');
    loadingIndicator = g;
    var blockOpts = {};
    blockOpts.showOverlay = false;
    blockOpts.ignoreIfBlocked = true;
    var css = {};
    css.border = '0px';
    css.backgroundColor = 'transparent';
    var overlayCss = {};
    overlayCss.opacity = '0';
    blockOpts.css = css;
    blockOpts.message = "<span class='loading-indicator'><label>Loading...</label></span>";
    loadingIndicator.block(blockOpts);
    return loadingIndicator;
}
SparkleXrm.GridEditor.GridDataViewBinder.addColumn = function SparkleXrm_GridEditor_GridDataViewBinder$addColumn(cols, displayName, width, field) {
    var col = SparkleXrm.GridEditor.GridDataViewBinder.newColumn(field, displayName, width);
    Xrm.ArrayEx.add(cols, col);
    return col;
}
SparkleXrm.GridEditor.GridDataViewBinder.parseLayout = function SparkleXrm_GridEditor_GridDataViewBinder$parseLayout(layout) {
    var layoutParts = layout.split(',');
    var cols = [];
    for (var i = 0; i < layoutParts.length; i = i + 3) {
        var field = layoutParts[i + 1];
        var name = layoutParts[i];
        var width = parseInt(layoutParts[i + 2]);
        var col = SparkleXrm.GridEditor.GridDataViewBinder.newColumn(field, name, width);
        Xrm.ArrayEx.add(cols, col);
    }
    return cols;
}
SparkleXrm.GridEditor.GridDataViewBinder.newColumn = function SparkleXrm_GridEditor_GridDataViewBinder$newColumn(field, name, width) {
    var col = {};
    col.id = name;
    col.name = name;
    col.width = width;
    col.minWidth = col.width;
    col.field = field;
    col.sortable = true;
    col.formatter = SparkleXrm.GridEditor.GridDataViewBinder.columnFormatter;
    return col;
}
SparkleXrm.GridEditor.GridDataViewBinder.columnFormatter = function SparkleXrm_GridEditor_GridDataViewBinder$columnFormatter(row, cell, value, columnDef, dataContext) {
    var typeName;
    var returnValue = '';
    if (columnDef.dataType != null) {
        typeName = columnDef.dataType;
    }
    else {
        typeName = Type.getInstanceType(value).get_name();
    }
    var entityContext = dataContext;
    var unchanged = (entityContext.entityState == null) || (entityContext.entityState === Xrm.Sdk.EntityStates.unchanged);
    if (unchanged && entityContext.formattedValues != null && Object.keyExists(entityContext.formattedValues, columnDef.field + 'name')) {
        returnValue = entityContext.formattedValues[columnDef.field + 'name'];
        return returnValue;
    }
    if (value != null) {
        switch (typeName.toLowerCase()) {
            case 'string':
                returnValue = value.toString();
                break;
            case 'boolean':
            case 'bool':
                returnValue = value.toString();
                break;
            case 'dateTime':
            case 'date':
                var dateValue = value;
                var dateFormat = 'dd/mm/yy';
                var timeFormat = 'hh:MM';
                if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
                    dateFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring;
                    timeFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.timeformatstring;
                }
                returnValue = Xrm.Sdk.DateTimeEx.formatDateSpecific(dateValue, dateFormat) + ' ' + Xrm.Sdk.DateTimeEx.formatTimeSpecific(dateValue, timeFormat);
                break;
            case 'decimal':
                returnValue = value.toString();
                break;
            case 'double':
                returnValue = value.toString();
                break;
            case 'int':
                returnValue = value.toString();
                break;
            case 'guid':
                returnValue = value.toString();
                break;
            case 'money':
                var moneyValue = value;
                returnValue = moneyValue.value.toString();
                break;
            case 'customer':
            case 'owner':
            case 'lookup':
            case 'entityreference':
                var refValue = value;
                returnValue = '<a class="sparkle-grid-link" href="#" logicalName="' + refValue.logicalName + '" id="' + refValue.id + '">' + refValue.name + '</a>';
                break;
            case 'picklist':
            case 'status':
            case 'state':
            case 'optionsetvalue':
                var optionValue = value;
                returnValue = optionValue.name;
                break;
            case 'primarynamelookup':
                var lookupName = (value == null) ? '' : value.toString();
                returnValue = '<a class="sparkle-grid-link" href="#" primaryNameLookup="1">' + lookupName + '</a>';
                break;
        }
    }
    return returnValue;
}
SparkleXrm.GridEditor.GridDataViewBinder.bindRowIcon = function SparkleXrm_GridEditor_GridDataViewBinder$bindRowIcon(column, entityLogicalName) {
    column.formatter = SparkleXrm.GridEditor.GridDataViewBinder.rowIcon;
    column.options = entityLogicalName;
    return column;
}
SparkleXrm.GridEditor.GridDataViewBinder.rowIcon = function SparkleXrm_GridEditor_GridDataViewBinder$rowIcon(row, cell, value, columnDef, dataContext) {
    var item = dataContext;
    if (item == null) {
        return '';
    }
    else {
        var lookup = item[columnDef.options];
        if (lookup == null || lookup.logicalName == null) {
            return '';
        }
        else {
            return "<span class='sparkle-grid-row-img'><img src='" + Xrm.Sdk.Metadata.MetadataCache.getSmallIconUrl(lookup.logicalName) + "'/></span>";
        }
    }
}
SparkleXrm.GridEditor.GridDataViewBinder.addEditIndicatorColumn = function SparkleXrm_GridEditor_GridDataViewBinder$addEditIndicatorColumn(columns) {
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, '', 20, 'entityState').formatter = function(row, cell, value, columnDef, dataContext) {
        var state = value;
        switch (state) {
            case Xrm.Sdk.EntityStates.created:
            case Xrm.Sdk.EntityStates.changed:
                return "<span class='grid-edit-indicator'></span>";
            case Xrm.Sdk.EntityStates.readOnly:
                return "<span class='grid-readonly-indicator'></span>";
            default:
                return '';
        }
    };
}
SparkleXrm.GridEditor.GridDataViewBinder.prototype = {
    selectActiveRow: true,
    addCheckBoxSelectColumn: true,
    multiSelect: true,
    _sortColumnName: null,
    _grid: null,
    
    dataBindXrmGrid: function SparkleXrm_GridEditor_GridDataViewBinder$dataBindXrmGrid(dataView, columns, gridId, pagerId, editable, allowAddNewRow) {
        Xrm.ArrayEx.add(columns, {});
        var gridOptions = {};
        gridOptions.enableCellNavigation = true;
        gridOptions.autoEdit = editable;
        gridOptions.editable = editable;
        gridOptions.asyncEditorLoading = true;
        gridOptions.enableAddRow = allowAddNewRow;
        gridOptions.rowHeight = (Xrm.PageEx.majorVersion === 2013) ? 30 : 20;
        gridOptions.headerRowHeight = 25;
        gridOptions.enableColumnReorder = false;
        var checkBoxSelector = null;
        if (this.addCheckBoxSelectColumn) {
            var checkboxOptions = {};
            checkboxOptions.cssClass = 'sparkle-checkbox-column';
            checkBoxSelector = new Slick.CheckboxSelectColumn(checkboxOptions);
            var checkBoxColumn = checkBoxSelector.getColumnDefinition();
            columns.insert(0, checkBoxColumn);
        }
        var grid = new Slick.Grid('#' + gridId, dataView, columns, gridOptions);
        if (this.addCheckBoxSelectColumn) {
            grid.registerPlugin(checkBoxSelector);
        }
        this.dataBindSelectionModel(grid, dataView);
        if (!String.isNullOrEmpty(pagerId)) {
            var pager = new SparkleXrm.GridEditor.CrmPagerControl(dataView, grid, $('#' + pagerId));
        }
        this.dataBindEvents(grid, dataView, gridId);
        this.addValidation(grid, dataView);
        this.addRefreshButton(gridId, dataView);
        $(window).resize(function(e) {
            SparkleXrm.GridEditor.GridDataViewBinder._freezeColumns(grid, true);
            grid.resizeCanvas();
            SparkleXrm.GridEditor.GridDataViewBinder._freezeColumns(grid, false);
        });
        dataView.onDataLoaded.subscribe(function(e, o) {
            SparkleXrm.GridEditor.GridDataViewBinder._freezeColumns(grid, false);
        });
        this._grid = grid;
        return grid;
    },
    
    dataBindDataViewGrid: function SparkleXrm_GridEditor_GridDataViewBinder$dataBindDataViewGrid(dataView, columns, gridId, pagerId, editable, allowAddNewRow) {
        Xrm.ArrayEx.add(columns, {});
        var gridOptions = {};
        gridOptions.enableCellNavigation = true;
        gridOptions.autoEdit = editable;
        gridOptions.editable = editable;
        gridOptions.enableAddRow = allowAddNewRow;
        gridOptions.rowHeight = 20;
        gridOptions.headerRowHeight = 25;
        gridOptions.enableColumnReorder = false;
        var checkBoxSelector = null;
        if (this.addCheckBoxSelectColumn) {
            var checkboxOptions = {};
            checkboxOptions.cssClass = 'sparkle-checkbox-column';
            checkBoxSelector = new Slick.CheckboxSelectColumn(checkboxOptions);
            var checkBoxColumn = checkBoxSelector.getColumnDefinition();
            columns.insert(0, checkBoxColumn);
        }
        var grid = new Slick.Grid('#' + gridId, dataView, columns, gridOptions);
        grid.registerPlugin(checkBoxSelector);
        dataView.onRowsChanged.subscribe(function(e, a) {
            var args = a;
            if (args != null && args.rows != null) {
                grid.invalidateRows(args.rows);
                grid.render();
            }
        });
        $(window).resize(function(e) {
            SparkleXrm.GridEditor.GridDataViewBinder._freezeColumns(grid, true);
            grid.resizeCanvas();
            SparkleXrm.GridEditor.GridDataViewBinder._freezeColumns(grid, false);
        });
        var reset = function() {
        };
        dataView.reset=reset;
        this.addRefreshButton(gridId, dataView);
        var selectionModelOptions = {};
        selectionModelOptions.selectActiveRow = true;
        var selectionModel = new Slick.RowSelectionModel(selectionModelOptions);
        grid.setSelectionModel(selectionModel);
        var onSort = ss.Delegate.create(this, function(e, a) {
            var args = a;
            this._sortColumnName = args.sortCol.field;
            dataView.sort(ss.Delegate.create(this, this.comparer), args.sortAsc);
        });
        grid.onSort.subscribe(onSort);
        return grid;
    },
    
    comparer: function SparkleXrm_GridEditor_GridDataViewBinder$comparer(l, r) {
        var a = l;
        var b = r;
        var x = a[this._sortColumnName], y = b[this._sortColumnName];
        return ((x === y) ? 0 : ((x > y) ? 1 : -1));
    },
    
    bindClickHandler: function SparkleXrm_GridEditor_GridDataViewBinder$bindClickHandler(grid) {
        var openEntityRecord = function(logicalName, id) {
            Xrm.Utility.openEntityForm(logicalName, id, null);
        };
        grid.onClick.subscribe(function(e, sender) {
            var cell = sender;
            var handled = false;
            var element = e.srcElement;
            var logicalName = element.getAttribute('logicalName');
            var id = element.getAttribute('id');
            var primaryNameLookup = element.getAttribute('primaryNameLookup');
            if ((logicalName != null & id != null) === 1) {
                handled = true;
            }
            else if (primaryNameLookup != null) {
                handled = true;
                var entity = cell.grid.getDataItem(cell.row);
                logicalName = entity.logicalName;
                var activitytypecode = entity.getAttributeValueString('activitytypecode');
                if (activitytypecode != null) {
                    logicalName = activitytypecode;
                }
                id = entity.id;
            }
            if (handled) {
                openEntityRecord(logicalName, id);
                e.stopImmediatePropagation();
                e.stopPropagation();
            }
        });
        grid.onDblClick.subscribe(function(e, sender) {
            var cell = sender;
            var entity = cell.grid.getDataItem(cell.row);
            var logicalName = entity.logicalName;
            var activitytypecode = entity.getAttributeValueString('activitytypecode');
            if (activitytypecode != null) {
                logicalName = activitytypecode;
            }
            openEntityRecord(logicalName, entity.id);
            e.stopImmediatePropagation();
            e.stopPropagation();
        });
    },
    
    addValidation: function SparkleXrm_GridEditor_GridDataViewBinder$addValidation(grid, dataView) {
        var setValidator = function(attributeName, col) {
            col.validator = function(value, item) {
                var indexer = dataView.gridValidationIndexer();
                var validationRule = indexer(attributeName);
                if (validationRule != null) {
                    return validationRule(value, item);
                }
                else {
                    var result = {};
                    result.valid = true;
                    return result;
                }
            };
        };
        if (dataView.gridValidationIndexer() != null) {
            var $enum1 = ss.IEnumerator.getEnumerator(grid.getColumns());
            while ($enum1.moveNext()) {
                var col = $enum1.current;
                var fieldName = col.field;
                setValidator(fieldName, col);
            }
        }
    },
    
    dataBindSelectionModel: function SparkleXrm_GridEditor_GridDataViewBinder$dataBindSelectionModel(grid, dataView) {
        var selectionModelOptions = {};
        selectionModelOptions.selectActiveRow = this.selectActiveRow;
        selectionModelOptions.multiRowSelect = this.multiSelect;
        var selectionModel = new Slick.RowSelectionModel(selectionModelOptions);
        var inHandler = false;
        selectionModel.onSelectedRangesChanged.subscribe(function(e, args) {
            if (inHandler) {
                return;
            }
            inHandler = true;
            var selectedRows = dataView.getSelectedRows();
            var newSelectedRows = args;
            var changed = selectedRows.length !== newSelectedRows.length;
            if (!changed) {
                for (var i = 0; i < selectedRows.length; i++) {
                    if (selectedRows[i].fromRow !== newSelectedRows[i].fromRow) {
                        changed = true;
                        break;
                    }
                }
            }
            if (changed) {
                dataView.raiseOnSelectedRowsChanged(newSelectedRows);
            }
            inHandler = false;
        });
        dataView.add_onSelectedRowsChanged(function() {
            if (inHandler) {
                return;
            }
            inHandler = true;
            var ranges = dataView.getSelectedRows();
            var selectedRows = new Array(ranges.length);
            for (var i = 0; i < selectedRows.length; i++) {
                selectedRows[i] = ranges[i].fromRow;
            }
            grid.setSelectedRows(selectedRows);
            inHandler = false;
        });
        grid.setSelectionModel(selectionModel);
    },
    
    addRefreshButton: function SparkleXrm_GridEditor_GridDataViewBinder$addRefreshButton(gridId, dataView) {
        var gridDiv = $('#' + gridId);
        var refreshButton = $("<div id='refreshButton' class='sparkle-grid-refresh-button' style='left: auto; right: 0px; display: inline;'><a href='#' id='refreshButtonLink' tabindex='0'><img id='grid_refresh' src='../../sparkle_/css/images/transparent_spacer.gif' class='sparkle-grid-refresh-button-img' style='cursor:pointer' alt='Refresh list' title='Refresh list'></a></div>").appendTo(gridDiv);
        refreshButton.find('#refreshButtonLink').click(function(e) {
            dataView.reset();
            dataView.refresh();
        });
    },
    
    dataBindEvents: function SparkleXrm_GridEditor_GridDataViewBinder$dataBindEvents(grid, dataView, gridContainerDivId) {
        grid.onSort.subscribe(function(o, item) {
            var sorting = item;
            dataView.sort(sorting);
            grid.invalidate();
            grid.render();
        });
        grid.onAddNewRow.subscribe(function(o, item) {
            var data = item;
            dataView.addItem(data.item);
            var column = data.column;
            grid.invalidateRow(dataView.getLength() - 1);
            grid.updateRowCount();
            grid.render();
        });
        dataView.onRowsChanged.subscribe(function(e, a) {
            var args = a;
            if (args != null && args.rows != null) {
                grid.invalidateRows(args.rows);
                grid.render();
            }
            else {
                grid.invalidateRow(dataView.getLength());
                grid.updateRowCount();
                grid.render();
            }
        });
        var loadingIndicator = null;
        var validationIndicator = null;
        var clearValidationIndicator = function(e, a) {
            if (validationIndicator != null) {
                validationIndicator.hide();
                validationIndicator.remove();
            }
        };
        grid.onCellChange.subscribe(clearValidationIndicator);
        grid.onActiveCellChanged.subscribe(clearValidationIndicator);
        grid.onBeforeCellEditorDestroy.subscribe(clearValidationIndicator);
        grid.onValidationError.subscribe(function(e, a) {
            var args = a;
            var validationResult = args.validationResults;
            var activeCellNode = args.cellNode;
            var editor = args.editor;
            var errorMessage = '';
            if (validationResult.message != null) {
                errorMessage = validationResult.message;
            }
            var valid_result = validationResult.valid;
            if (!valid_result) {
                $(activeCellNode).attr('title', errorMessage);
                clearValidationIndicator(e, a);
                validationIndicator = $("<div class='popup-box-container'><div width='16px' height='16px' class='sparkle-imagestrip-inlineedit_warning popup-box-icon' alt='Error' id='icon'/><div class='popup-box validation-text'/></div>").appendTo(document.body);
                validationIndicator.find('.validation-text').text(errorMessage);
                validationIndicator.position({
                                            my: 'left bottom',
                                            at: 'left top',
                                            collision: 'fit fit',
                                            of: activeCellNode
                                        })
                                        .show({
                                        effect: 'blind'
                                        })
                                        .delay( 500000 )
                                        .hide({
                                            effect: 'fade',
                                            duration: 'slow', 
                                        },
                                            function() {
                                                $( this ).remove();
                                                
                                            });
                                        ;
            }
            else {
                clearValidationIndicator(e, a);
                $(activeCellNode).attr('title', '');
            }
        });
        dataView.onDataLoading.subscribe(function(e, a) {
            loadingIndicator = SparkleXrm.GridEditor.GridDataViewBinder._showLoadingIndicator(loadingIndicator, gridContainerDivId);
            var $enum1 = ss.IEnumerator.getEnumerator(grid.getColumns());
            while ($enum1.moveNext()) {
                var col = $enum1.current;
                if (col.maxWidth != null) {
                    col.maxWidth = 400;
                }
            }
        });
        dataView.onDataLoaded.subscribe(function(e, a) {
            var args = a;
            if (args.errorMessage == null) {
                for (var i = args.from; i <= args.to; i++) {
                    grid.invalidateRow(i);
                }
                grid.updateRowCount();
                grid.render();
            }
            else {
                alert('There was a problem refreshing the grid.\nPlease contact your system administrator:\n' + args.errorMessage);
            }
            if (loadingIndicator != null) {
                loadingIndicator.unblock();
            }
        });
        grid.onCellChange.subscribe(function(e, data) {
            var eventData = data;
            dataView.raisePropertyChanged('');
        });
    },
    
    bindCommitEdit: function SparkleXrm_GridEditor_GridDataViewBinder$bindCommitEdit(vm) {
        vm.add_onCommitEdit(ss.Delegate.create(this, function(sender, e) {
            if (this._grid.getEditorLock().isActive()) {
                e.cancel = !this._grid.getEditorLock().commitCurrentEdit();
            }
        }));
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.GridEditorBase

SparkleXrm.GridEditor.GridEditorBase = function SparkleXrm_GridEditor_GridEditorBase(args) {
    this._args = args;
}
SparkleXrm.GridEditor.GridEditorBase.prototype = {
    _args: null,
    _item: null,
    
    destroy: function SparkleXrm_GridEditor_GridEditorBase$destroy() {
    },
    
    show: function SparkleXrm_GridEditor_GridEditorBase$show() {
    },
    
    hide: function SparkleXrm_GridEditor_GridEditorBase$hide() {
    },
    
    position: function SparkleXrm_GridEditor_GridEditorBase$position(position) {
    },
    
    focus: function SparkleXrm_GridEditor_GridEditorBase$focus() {
    },
    
    loadValue: function SparkleXrm_GridEditor_GridEditorBase$loadValue(item) {
        this._item = item;
    },
    
    serializeValue: function SparkleXrm_GridEditor_GridEditorBase$serializeValue() {
        return null;
    },
    
    applyValue: function SparkleXrm_GridEditor_GridEditorBase$applyValue(item, state) {
    },
    
    raiseOnChange: function SparkleXrm_GridEditor_GridEditorBase$raiseOnChange(item) {
        var itemObject = Type.safeCast(item, Xrm.ComponentModel.INotifyPropertyChanged);
        if (itemObject != null) {
            itemObject.raisePropertyChanged(this._args.column.field);
        }
    },
    
    isValueChanged: function SparkleXrm_GridEditor_GridEditorBase$isValueChanged() {
        return false;
    },
    
    nativeValidation: function SparkleXrm_GridEditor_GridEditorBase$nativeValidation(newValue) {
        return null;
    },
    
    validate: function SparkleXrm_GridEditor_GridEditorBase$validate() {
        var newValue = this.serializeValue();
        var result = this.nativeValidation(newValue);
        if (result == null && this._args.column.validator != null) {
            var validationResults = this._args.column.validator(newValue, this._args.item);
            if (!validationResults.valid) {
                result = validationResults;
            }
        }
        if (result == null) {
            result = {};
            result.valid = true;
            result.message = null;
        }
        return result;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.GridEditor.XrmOptionSetEditor

SparkleXrm.GridEditor.XrmOptionSetEditor = function SparkleXrm_GridEditor_XrmOptionSetEditor(args) {
    this._defaultValue$1 = new Xrm.Sdk.OptionSetValue(null);
    SparkleXrm.GridEditor.XrmOptionSetEditor.initializeBase(this, [ args ]);
    var self = this;
    var opts = args.column.options;
    if (SparkleXrm.GridEditor.XrmOptionSetEditor._options$1 == null) {
        SparkleXrm.GridEditor.XrmOptionSetEditor._options$1 = Xrm.Sdk.Metadata.MetadataCache.getOptionSetValues(opts.entityLogicalName, opts.attributeLogicalName, opts.allowEmpty);
    }
    this.createSelect(self);
}
SparkleXrm.GridEditor.XrmOptionSetEditor.formatter = function SparkleXrm_GridEditor_XrmOptionSetEditor$formatter(row, cell, value, columnDef, dataContext) {
    var opt = value;
    return (opt == null) ? '' : opt.name;
}
SparkleXrm.GridEditor.XrmOptionSetEditor.bindColumn = function SparkleXrm_GridEditor_XrmOptionSetEditor$bindColumn(column, entityLogicalName, attributeLogicalName, allowEmpty) {
    column.editor = SparkleXrm.GridEditor.XrmOptionSetEditor.editorFactory;
    column.formatter = SparkleXrm.GridEditor.XrmOptionSetEditor.formatter;
    var opts = {};
    opts.attributeLogicalName = attributeLogicalName;
    opts.entityLogicalName = entityLogicalName;
    opts.allowEmpty = allowEmpty;
    column.options = opts;
    return column;
}
SparkleXrm.GridEditor.XrmOptionSetEditor.prototype = {
    _input$1: null,
    
    createSelect: function SparkleXrm_GridEditor_XrmOptionSetEditor$createSelect(self) {
        var optionSet = '<SELECT>';
        optionSet += String.format('<OPTION title="" value="" {0}></OPTION>', (self._defaultValue$1.value == null) ? 'selected' : '');
        var $enum1 = ss.IEnumerator.getEnumerator(SparkleXrm.GridEditor.XrmOptionSetEditor._options$1);
        while ($enum1.moveNext()) {
            var o = $enum1.current;
            optionSet += String.format('<OPTION title="{0}" value="{1}" {2}>{0}</OPTION>', o.name, o.value, (self._defaultValue$1.value === o.value) ? 'selected' : '');
        }
        optionSet += '</SELECT>';
        self._input$1 = $(optionSet);
        self._input$1.appendTo(this._args.container);
        self._input$1.focus().select();
    },
    
    destroy: function SparkleXrm_GridEditor_XrmOptionSetEditor$destroy() {
        if (this._input$1 != null) {
            this._input$1.remove();
        }
    },
    
    focus: function SparkleXrm_GridEditor_XrmOptionSetEditor$focus() {
        this._input$1.focus();
    },
    
    loadValue: function SparkleXrm_GridEditor_XrmOptionSetEditor$loadValue(item) {
        var opt = item[this._args.column.field];
        this._defaultValue$1 = opt;
        this._setDefaultValue$1();
    },
    
    serializeValue: function SparkleXrm_GridEditor_XrmOptionSetEditor$serializeValue() {
        if (this._input$1 != null) {
            var opt = new Xrm.Sdk.OptionSetValue(this._getValue$1());
            opt.name = $('option:selected', this._input$1).text();
            return opt;
        }
        else {
            return null;
        }
    },
    
    applyValue: function SparkleXrm_GridEditor_XrmOptionSetEditor$applyValue(item, state) {
        var opt = state;
        item[this._args.column.field] = opt;
        item[this._args.column.field + 'name'] = opt.name;
        var itemObject = Type.safeCast((item), Xrm.ComponentModel.INotifyPropertyChanged);
        if (itemObject != null) {
            itemObject.raisePropertyChanged(this._args.column.field);
        }
    },
    
    isValueChanged: function SparkleXrm_GridEditor_XrmOptionSetEditor$isValueChanged() {
        if (this._input$1 != null) {
            var valueAsString = (this._defaultValue$1 != null && this._defaultValue$1.value != null) ? this._defaultValue$1.value.toString() : '';
            return (this._input$1.val() !== valueAsString);
        }
        else {
            return false;
        }
    },
    
    _getValue$1: function SparkleXrm_GridEditor_XrmOptionSetEditor$_getValue$1() {
        var val = this._input$1.val();
        if (String.isNullOrEmpty(val)) {
            return null;
        }
        else {
            return parseInt(val);
        }
    },
    
    _setDefaultValue$1: function SparkleXrm_GridEditor_XrmOptionSetEditor$_setDefaultValue$1() {
        if (this._input$1 != null) {
            this._input$1.val((this._defaultValue$1 != null && this._defaultValue$1.value != null) ? this._defaultValue$1.value.toString() : null);
            this._input$1.select();
        }
    }
}


Type.registerNamespace('SparkleXrm');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.LocalisedContentLoader

SparkleXrm.LocalisedContentLoader = function SparkleXrm_LocalisedContentLoader() {
}
SparkleXrm.LocalisedContentLoader.prototype = {
    fallBackLCID: 1033,
    
    loadContent: function SparkleXrm_LocalisedContentLoader$loadContent(webresourceFileName, lcid, callback) {
        if (!SparkleXrm.LocalisedContentLoader.supportedLCIDs.contains(lcid)) {
            lcid = this.fallBackLCID;
        }
        var pos = webresourceFileName.lastIndexOf('.');
        var resourceFileName = webresourceFileName.substr(0, pos - 1) + '_' + lcid.toString() + webresourceFileName.substr(pos);
        var options = {};
        options.type = 'GET';
        options.url = resourceFileName;
        options.dataType = 'script';
        options.success = function(data, textStatus, request) {
            callback();
        };
        options.error = function(request, textStatus, error) {
            alert(String.format("Could not load resource file '{0}'. Please contact your system adminsitrator.", resourceFileName));
        };
        $.ajax(options);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ValidationRules

SparkleXrm.ValidationRules = function SparkleXrm_ValidationRules() {
}
SparkleXrm.ValidationRules.areValid = function SparkleXrm_ValidationRules$areValid(fields) {
    var valid = true;
    var $enum1 = ss.IEnumerator.getEnumerator(fields);
    while ($enum1.moveNext()) {
        var field = $enum1.current;
        valid = valid && (field).isValid();
        if (!valid) {
            break;
        }
    }
    return valid;
}
SparkleXrm.ValidationRules.createRules = function SparkleXrm_ValidationRules$createRules() {
    return new SparkleXrm.ValidationRules();
}
SparkleXrm.ValidationRules.convertToGridValidation = function SparkleXrm_ValidationRules$convertToGridValidation(ruleDelegate) {
    var validationFunction = function(value, item) {
        var rules = new SparkleXrm.ValidationRules();
        rules = ruleDelegate(rules, item, null);
        var result = {};
        result.valid = true;
        var validationRules = rules;
        var $enum1 = ss.IEnumerator.getEnumerator(Object.keys(validationRules));
        while ($enum1.moveNext()) {
            var key = $enum1.current;
            if (Object.keyExists(ko.validation.rules, key)) {
                var targetRule = ko.validation.rules[key];
                var sourceRule = validationRules[key];
                result.valid = targetRule.validator(value, (sourceRule.params == null) ? targetRule.params : sourceRule.params);
                result.message = (String.isNullOrEmpty(targetRule.message)) ? sourceRule.message : targetRule.message;
            }
            else if (key === 'validation') {
                var anonRules = validationRules[key];
                var $enum2 = ss.IEnumerator.getEnumerator(anonRules);
                while ($enum2.moveNext()) {
                    var rule = $enum2.current;
                    result.valid = rule.validator(value);
                    result.message = rule.message;
                    if (!result.valid) {
                        break;
                    }
                }
            }
            if (!result.valid) {
                break;
            }
        }
        return result;
    };
    return validationFunction;
}
SparkleXrm.ValidationRules.prototype = {
    
    register: function SparkleXrm_ValidationRules$register(model) {
        (model).extend(this);
    },
    
    addRequired: function SparkleXrm_ValidationRules$addRequired() {
        this['required'] = true;
        return this;
    },
    
    addRequiredMsg: function SparkleXrm_ValidationRules$addRequiredMsg(message) {
        this['required'] = new SparkleXrm.ValidationMessage(message);
        return this;
    },
    
    addRule: function SparkleXrm_ValidationRules$addRule(message, validator) {
        var rule = new SparkleXrm.AnonymousRule();
        var anonRules = this['validation'];
        if (anonRules == null) {
            anonRules = [];
            this['validation'] = anonRules;
        }
        rule.message = message;
        rule.validator = validator;
        anonRules.add(rule);
        return this;
    },
    
    addPattern: function SparkleXrm_ValidationRules$addPattern(message, pattern) {
        var patternOptions = {};
        patternOptions.message = message;
        patternOptions.params = pattern;
        this['pattern'] = patternOptions;
        return this;
    },
    
    addCustom: function SparkleXrm_ValidationRules$addCustom(type, options) {
        this[type] = options;
        return this;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.AnonymousRule

SparkleXrm.AnonymousRule = function SparkleXrm_AnonymousRule() {
}
SparkleXrm.AnonymousRule.prototype = {
    validator: null,
    message: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ValidationMessage

SparkleXrm.ValidationMessage = function SparkleXrm_ValidationMessage(message) {
    this.message = message;
}
SparkleXrm.ValidationMessage.prototype = {
    message: null
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ValidationBinder

SparkleXrm.ValidationBinder = function SparkleXrm_ValidationBinder() {
}
SparkleXrm.ValidationBinder.prototype = {
    
    register: function SparkleXrm_ValidationBinder$register(fieldName, rule) {
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.DataViewValidationBinder

SparkleXrm.DataViewValidationBinder = function SparkleXrm_DataViewValidationBinder() {
    this._validationRules$1 = {};
    SparkleXrm.DataViewValidationBinder.initializeBase(this);
}
SparkleXrm.DataViewValidationBinder.prototype = {
    
    register: function SparkleXrm_DataViewValidationBinder$register(fieldName, rule) {
        this._validationRules$1[fieldName] = rule;
    },
    
    gridValidationIndexer: function SparkleXrm_DataViewValidationBinder$gridValidationIndexer(attributeLogicalName) {
        if (this._validationRules$1[attributeLogicalName] != null) {
            return SparkleXrm.ValidationRules.convertToGridValidation(this._validationRules$1[attributeLogicalName]);
        }
        else {
            return null;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ObservableValidationBinder

SparkleXrm.ObservableValidationBinder = function SparkleXrm_ObservableValidationBinder(observable) {
    SparkleXrm.ObservableValidationBinder.initializeBase(this);
    this._observable$1 = observable;
}
SparkleXrm.ObservableValidationBinder.prototype = {
    _observable$1: null,
    
    register: function SparkleXrm_ObservableValidationBinder$register(fieldName, ruleDelegate) {
        var viewModel = ko.utils.unwrapObservable(this._observable$1);
        var observableField = viewModel[fieldName];
        var rule = ruleDelegate(SparkleXrm.ValidationRules.createRules(), this._observable$1, null);
        (observableField).extend(rule);
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ViewBase

SparkleXrm.ViewBase = function SparkleXrm_ViewBase() {
}
SparkleXrm.ViewBase.registerViewModel = function SparkleXrm_ViewBase$registerViewModel(viewModel) {
    $(function() {
        if (!SparkleXrm.ViewBase._templateLoaded) {
            $.get(SparkleXrm.ViewBase.sparkleXrmTemplatePath, function(template) {
                $('body').append(template);
                ko.validation.registerExtenders();
                Xrm.Sdk.OrganizationServiceProxy.getUserSettings();
                SparkleXrm.ViewBase._templateLoaded = true;
                ko.applyBindings(viewModel);
            });
        }
        else {
            ko.applyBindings(viewModel);
        }
    });
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.ViewModelBase

SparkleXrm.ViewModelBase = function SparkleXrm_ViewModelBase() {
    this.isBusy = ko.observable(false);
    this.isBusyProgress = ko.observable(null);
    this.isBusyMessage = ko.observable('Please Wait...');
}
SparkleXrm.ViewModelBase.prototype = {
    
    add_onCommitEdit: function SparkleXrm_ViewModelBase$add_onCommitEdit(value) {
        this.__onCommitEdit = ss.Delegate.combine(this.__onCommitEdit, value);
    },
    remove_onCommitEdit: function SparkleXrm_ViewModelBase$remove_onCommitEdit(value) {
        this.__onCommitEdit = ss.Delegate.remove(this.__onCommitEdit, value);
    },
    
    __onCommitEdit: null,
    
    commitEdit: function SparkleXrm_ViewModelBase$commitEdit() {
        if (this.__onCommitEdit != null) {
            var args = new ss.CancelEventArgs();
            this.__onCommitEdit(this, args);
            return !args.cancel;
        }
        return true;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.DoubleClickBindingHandler

SparkleXrm.DoubleClickBindingHandler = function SparkleXrm_DoubleClickBindingHandler() {
    SparkleXrm.DoubleClickBindingHandler.initializeBase(this);
}
SparkleXrm.DoubleClickBindingHandler.prototype = {
    _delay$1: 400,
    _clickTimeoutId$1: 0,
    
    init: function SparkleXrm_DoubleClickBindingHandler$init(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var hander = valueAccessor;
        $(null, element).click(ss.Delegate.create(this, function(e) {
            if (!!this._clickTimeoutId$1) {
                window.clearTimeout(0);
                this._clickTimeoutId$1 = 0;
            }
            else {
                this._clickTimeoutId$1 = window.setTimeout(ss.Delegate.create(this, function() {
                    this._clickTimeoutId$1 = 0;
                    hander();
                }), this._delay$1);
            }
        }));
    }
}


Type.registerNamespace('SparkleXrm.Validation');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Validation.DurationValidation

SparkleXrm.Validation.DurationValidation = function SparkleXrm_Validation_DurationValidation() {
}
SparkleXrm.Validation.DurationValidation.validator = function SparkleXrm_Validation_DurationValidation$validator(val, otherval) {
    var parseDate = Xrm.Sdk.DateTimeEx.addTimeToDate(Date.get_now(), val);
    return parseDate != null;
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.Validation.TimeValidation

SparkleXrm.Validation.TimeValidation = function SparkleXrm_Validation_TimeValidation() {
}
SparkleXrm.Validation.TimeValidation.validator = function SparkleXrm_Validation_TimeValidation$validator(val, otherval) {
    var parseDate = Xrm.Sdk.DateTimeEx.addTimeToDate(Date.get_now(), val);
    return parseDate != null;
}


SparkleXrm.CustomBinding.EnterKeyBinding.registerClass('SparkleXrm.CustomBinding.EnterKeyBinding', Object);
SparkleXrm.CustomBinding.XrmCurrencySymbolBinding.registerClass('SparkleXrm.CustomBinding.XrmCurrencySymbolBinding', Object);
SparkleXrm.CustomBinding.XrmMoneyBinding.registerClass('SparkleXrm.CustomBinding.XrmMoneyBinding', Object);
SparkleXrm.CustomBinding.XrmNumericBinding.registerClass('SparkleXrm.CustomBinding.XrmNumericBinding', Object);
SparkleXrm.CustomBinding.XrmOptionSetBinding.registerClass('SparkleXrm.CustomBinding.XrmOptionSetBinding', Object);
SparkleXrm.CustomBinding.AnimateVisible.registerClass('SparkleXrm.CustomBinding.AnimateVisible', Object);
SparkleXrm.CustomBinding.AutocompleteBinding.registerClass('SparkleXrm.CustomBinding.AutocompleteBinding', Object);
SparkleXrm.CustomBinding.XrmBooleanBinding.registerClass('SparkleXrm.CustomBinding.XrmBooleanBinding', Object);
SparkleXrm.CustomBinding.XrmLookupBinding.registerClass('SparkleXrm.CustomBinding.XrmLookupBinding', Object);
SparkleXrm.CustomBinding.XrmTextBinding.registerClass('SparkleXrm.CustomBinding.XrmTextBinding', Object);
SparkleXrm.CustomBinding.XrmDatePickerBinding.registerClass('SparkleXrm.CustomBinding.XrmDatePickerBinding', Object);
SparkleXrm.CustomBinding.XrmDurationBinding.registerClass('SparkleXrm.CustomBinding.XrmDurationBinding', Object);
SparkleXrm.CustomBinding.FadeVisibleBinding.registerClass('SparkleXrm.CustomBinding.FadeVisibleBinding', Object);
SparkleXrm.CustomBinding.ProgressBarBinding.registerClass('SparkleXrm.CustomBinding.ProgressBarBinding', Object);
SparkleXrm.CustomBinding.XrmTimeOfDayBinding.registerClass('SparkleXrm.CustomBinding.XrmTimeOfDayBinding', Object);
SparkleXrm.GridEditor.SortCol.registerClass('SparkleXrm.GridEditor.SortCol');
SparkleXrm.GridEditor.GridEditorBase.registerClass('SparkleXrm.GridEditor.GridEditorBase');
SparkleXrm.GridEditor.XrmBooleanEditor.registerClass('SparkleXrm.GridEditor.XrmBooleanEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.XrmMoneyEditor.registerClass('SparkleXrm.GridEditor.XrmMoneyEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.XrmNumberEditor.registerClass('SparkleXrm.GridEditor.XrmNumberEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.DataViewBase.registerClass('SparkleXrm.GridEditor.DataViewBase', null, Object, Object);
SparkleXrm.GridEditor.EntityDataViewModel.registerClass('SparkleXrm.GridEditor.EntityDataViewModel', SparkleXrm.GridEditor.DataViewBase);
SparkleXrm.GridEditor.XrmDateEditor.registerClass('SparkleXrm.GridEditor.XrmDateEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.XrmDurationEditor.registerClass('SparkleXrm.GridEditor.XrmDurationEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.XrmLookupEditorOptions.registerClass('SparkleXrm.GridEditor.XrmLookupEditorOptions');
SparkleXrm.GridEditor.XrmLookupEditor.registerClass('SparkleXrm.GridEditor.XrmLookupEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.CrmPagerControl.registerClass('SparkleXrm.GridEditor.CrmPagerControl');
SparkleXrm.GridEditor.XrmTextEditor.registerClass('SparkleXrm.GridEditor.XrmTextEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.XrmTimeEditor.registerClass('SparkleXrm.GridEditor.XrmTimeEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.GridEditor.Formatters.registerClass('SparkleXrm.GridEditor.Formatters');
SparkleXrm.GridEditor.GridDataViewBinder.registerClass('SparkleXrm.GridEditor.GridDataViewBinder');
SparkleXrm.GridEditor.XrmOptionSetEditor.registerClass('SparkleXrm.GridEditor.XrmOptionSetEditor', SparkleXrm.GridEditor.GridEditorBase);
SparkleXrm.LocalisedContentLoader.registerClass('SparkleXrm.LocalisedContentLoader');
SparkleXrm.ValidationRules.registerClass('SparkleXrm.ValidationRules');
SparkleXrm.AnonymousRule.registerClass('SparkleXrm.AnonymousRule');
SparkleXrm.ValidationMessage.registerClass('SparkleXrm.ValidationMessage');
SparkleXrm.ValidationBinder.registerClass('SparkleXrm.ValidationBinder');
SparkleXrm.DataViewValidationBinder.registerClass('SparkleXrm.DataViewValidationBinder', SparkleXrm.ValidationBinder);
SparkleXrm.ObservableValidationBinder.registerClass('SparkleXrm.ObservableValidationBinder', SparkleXrm.ValidationBinder);
SparkleXrm.ViewBase.registerClass('SparkleXrm.ViewBase');
SparkleXrm.ViewModelBase.registerClass('SparkleXrm.ViewModelBase');
SparkleXrm.DoubleClickBindingHandler.registerClass('SparkleXrm.DoubleClickBindingHandler', Object);
SparkleXrm.Validation.DurationValidation.registerClass('SparkleXrm.Validation.DurationValidation');
SparkleXrm.Validation.TimeValidation.registerClass('SparkleXrm.Validation.TimeValidation');
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['enterKey'] = new SparkleXrm.CustomBinding.EnterKeyBinding();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['xrmcurrencysymbol'] = new SparkleXrm.CustomBinding.XrmCurrencySymbolBinding();
        ko.validation.makeBindingHandlerValidatable('xrmcurrencysymbol');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['xrmmoney'] = new SparkleXrm.CustomBinding.XrmMoneyBinding();
        ko.validation.makeBindingHandlerValidatable('xrmmoney');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['xrmnumeric'] = new SparkleXrm.CustomBinding.XrmNumericBinding();
        ko.validation.makeBindingHandlerValidatable('xrmnumeric');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['optionset'] = new SparkleXrm.CustomBinding.XrmOptionSetBinding();
        ko.validation.makeBindingHandlerValidatable('optionset');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['animateVisible'] = new SparkleXrm.CustomBinding.AnimateVisible();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['autocomplete'] = new SparkleXrm.CustomBinding.AutocompleteBinding();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['booleanValue'] = new SparkleXrm.CustomBinding.XrmBooleanBinding();
        ko.validation.makeBindingHandlerValidatable('booleanValue');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['lookup'] = new SparkleXrm.CustomBinding.XrmLookupBinding();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['xrmtextbox'] = new SparkleXrm.CustomBinding.XrmTextBinding();
        ko.validation.makeBindingHandlerValidatable('xrmtextbox');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['datepicker'] = new SparkleXrm.CustomBinding.XrmDatePickerBinding();
        ko.validation.makeBindingHandlerValidatable('datepicker');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['duration'] = new SparkleXrm.CustomBinding.XrmDurationBinding();
        ko.validation.makeBindingHandlerValidatable('duration');
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['fadeVisible'] = new SparkleXrm.CustomBinding.FadeVisibleBinding();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['progressBar'] = new SparkleXrm.CustomBinding.ProgressBarBinding();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['timeofday'] = new SparkleXrm.CustomBinding.XrmTimeOfDayBinding();
    }
})();
SparkleXrm.GridEditor.XrmBooleanEditor.booleanEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmBooleanEditor.booleanEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmBooleanEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmMoneyEditor.moneyEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmMoneyEditor.moneyEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmMoneyEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmNumberEditor.numberEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmNumberEditor.numberEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmNumberEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmDateEditor.crmDateEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmDateEditor.crmDateEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmDateEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmDurationEditor.durationEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmDurationEditor.durationEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmDurationEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmLookupEditor.lookupEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmLookupEditor.lookupEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmLookupEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmTextEditor.textEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmTextEditor.textEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmTextEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmTimeEditor.timeEditor = null;
(function () {
    SparkleXrm.GridEditor.XrmTimeEditor.timeEditor = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmTimeEditor(args);
        return editor;
    };
})();
SparkleXrm.GridEditor.XrmOptionSetEditor.editorFactory = null;
SparkleXrm.GridEditor.XrmOptionSetEditor._options$1 = null;
(function () {
    SparkleXrm.GridEditor.XrmOptionSetEditor.editorFactory = function(args) {
        var editor = new SparkleXrm.GridEditor.XrmOptionSetEditor(args);
        return editor;
    };
})();
SparkleXrm.LocalisedContentLoader.supportedLCIDs = [];
SparkleXrm.ViewBase._templateLoaded = false;
SparkleXrm.ViewBase.sparkleXrmTemplatePath = '../../sparkle_/html/form.templates.htm';
(function () {
    if (typeof(ko) !== 'undefined') {
        ko.bindingHandlers['singleClick'] = new SparkleXrm.DoubleClickBindingHandler();
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        var rule = {};
        rule.message = '{0} is an invalid duration.';
        rule.validator = SparkleXrm.Validation.DurationValidation.validator;
        ko.validation.rules['durationValidation'] = rule;
    }
})();
(function () {
    if (typeof(ko) !== 'undefined') {
        var rule = {};
        rule.message = '{0} is an invalid time.';
        rule.validator = SparkleXrm.Validation.TimeValidation.validator;
        ko.validation.rules['timeValidation'] = rule;
    }
})();
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
