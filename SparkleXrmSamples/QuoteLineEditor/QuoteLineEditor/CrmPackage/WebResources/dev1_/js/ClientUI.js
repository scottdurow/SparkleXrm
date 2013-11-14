//! ClientUI.debug.js
//
waitForScripts("client",["mscorlib","xrm","xrmui", "jquery", "jquery-ui"],
function () {

(function($){

Type.registerNamespace('Client.QuoteLineItemEditor.Model');

////////////////////////////////////////////////////////////////////////////////
// Client.QuoteLineItemEditor.Model.QuoteDetail

Client.QuoteLineItemEditor.Model.QuoteDetail = function Client_QuoteLineItemEditor_Model_QuoteDetail() {
    Client.QuoteLineItemEditor.Model.QuoteDetail.initializeBase(this, [ 'quotedetail' ]);
    this._metaData['quantity'] = Xrm.Sdk.AttributeTypes.decimal_;
    this._metaData['lineitemnumber'] = Xrm.Sdk.AttributeTypes.int_;
    this.add_propertyChanged(ss.Delegate.create(this, this._quoteProduct_PropertyChanged$1));
}
Client.QuoteLineItemEditor.Model.QuoteDetail.prototype = {
    
    _quoteProduct_PropertyChanged$1: function Client_QuoteLineItemEditor_Model_QuoteDetail$_quoteProduct_PropertyChanged$1(sender, e) {
        if (this.quantity != null && this.priceperunit != null) {
            this.extendedamount = new Xrm.Sdk.Money(this.quantity * this.priceperunit.value);
        }
        this.isproductoverridden = !String.isNullOrEmpty(this.productdescription);
    },
    
    quotedetailid: null,
    quoteid: null,
    lineitemnumber: null,
    isproductoverridden: false,
    productdescription: null,
    productid: null,
    uomid: null,
    ispriceoverridden: false,
    priceperunit: null,
    quantity: null,
    extendedamount: null,
    transactioncurrencyid: null,
    requestdeliveryby: null,
    salesrepid: null
}


Type.registerNamespace('Client.QuoteLineItemEditor.ViewModels');

////////////////////////////////////////////////////////////////////////////////
// Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail

Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail = function Client_QuoteLineItemEditor_ViewModels_ObservableQuoteDetail() {
    this.requestdeliveryby = ko.observable();
    this.salesrepid = ko.observable();
    this.requestdeliveryby.subscribe(ss.Delegate.create(this, function(value) {
        this.onValueChange('requestdeliveryby', value);
    }));
    this.salesrepid.subscribe(ss.Delegate.create(this, function(value) {
        this.onValueChange('salesrepid', value);
    }));
}
Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail.prototype = {
    innerQuoteDetail: null,
    _isSetting: false,
    
    onValueChange: function Client_QuoteLineItemEditor_ViewModels_ObservableQuoteDetail$onValueChange(attribute, newValue) {
        if (!this._isSetting) {
            window.setTimeout(ss.Delegate.create(this, function() {
                this.commit();
            }), 0);
        }
    },
    
    setValue: function Client_QuoteLineItemEditor_ViewModels_ObservableQuoteDetail$setValue(value) {
        this._isSetting = true;
        this.innerQuoteDetail = (value == null) ? new Client.QuoteLineItemEditor.Model.QuoteDetail() : value;
        this.requestdeliveryby(this.innerQuoteDetail.requestdeliveryby);
        this.salesrepid(this.innerQuoteDetail.salesrepid);
        this._isSetting = false;
    },
    
    commit: function Client_QuoteLineItemEditor_ViewModels_ObservableQuoteDetail$commit() {
        if (this.innerQuoteDetail == null) {
            return null;
        }
        this.innerQuoteDetail.requestdeliveryby = ko.utils.unwrapObservable(this.requestdeliveryby);
        this.innerQuoteDetail.salesrepid = ko.utils.unwrapObservable(this.salesrepid);
        this.innerQuoteDetail.raisePropertyChanged('');
        return this.innerQuoteDetail;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation

Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation = function Client_QuoteLineItemEditor_ViewModels_QuoteDetailValidation() {
}
Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.writeInProduct = function Client_QuoteLineItemEditor_ViewModels_QuoteDetailValidation$writeInProduct(rules, viewModel, dataContext) {
    var self = ko.utils.unwrapObservable(viewModel);
    return rules.addRule('Select either a product or provide a product description.', function(value) {
        var productid = ko.utils.unwrapObservable(self.productid);
        var isValid = String.isNullOrEmpty(value) || (productid == null);
        return isValid;
    });
}
Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.productId = function Client_QuoteLineItemEditor_ViewModels_QuoteDetailValidation$productId(rules, viewModel, dataContext) {
    var self = ko.utils.unwrapObservable(viewModel);
    return rules.addRule('Select either a product or provide a product description.', function(value) {
        var productDescription = ko.utils.unwrapObservable(self.productdescription);
        var isValid = String.isNullOrEmpty(productDescription) || (value == null);
        return isValid;
    });
}
Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.register = function Client_QuoteLineItemEditor_ViewModels_QuoteDetailValidation$register(binder) {
    binder.register('productdescription', Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.writeInProduct);
    binder.register('productid', Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.productId);
}


////////////////////////////////////////////////////////////////////////////////
// Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel

Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel = function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel() {
    this.selectedQuoteDetail = ko.observable();
    Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel.initializeBase(this);
    this.lines = new SparkleXrm.GridEditor.EntityDataViewModel(10, Client.QuoteLineItemEditor.Model.QuoteDetail, false);
    this.lines.add_onSelectedRowsChanged(ss.Delegate.create(this, this._onSelectedRowsChanged$1));
    this.lines.set_fetchXml("<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>\r\n                              <entity name='quotedetail'>\r\n                                <attribute name='productid' />\r\n                                <attribute name='productdescription' />\r\n                                <attribute name='priceperunit' />\r\n                                <attribute name='quantity' />\r\n                                <attribute name='extendedamount' />\r\n                                <attribute name='quotedetailid' />\r\n                                <attribute name='isproductoverridden' />\r\n                                <attribute name='ispriceoverridden' />\r\n                                <attribute name='manualdiscountamount' />\r\n                                <attribute name='lineitemnumber' />\r\n                                <attribute name='description' />\r\n                                <attribute name='transactioncurrencyid' />\r\n                                <attribute name='baseamount' />\r\n                                <attribute name='requestdeliveryby' />\r\n                                <attribute name='salesrepid' />\r\n                                <attribute name='uomid' />\r\n                                {3}\r\n                                <link-entity name='quote' from='quoteid' to='quoteid' alias='ac'>\r\n                                  <filter type='and'>\r\n                                    <condition attribute='quoteid' operator='eq' uiname='tes' uitype='quote' value='" + this.getQuoteId() + "' />\r\n                                  </filter>\r\n                                </link-entity>\r\n                              </entity>\r\n                            </fetch>");
    this.lines.sortBy(new SparkleXrm.GridEditor.SortCol('lineitemnumber', true));
    this.lines.newItemFactory = ss.Delegate.create(this, this.newLineFactory);
    Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.register(this.lines.validationBinder);
    this.selectedQuoteDetail(new Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail());
}
Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel.prototype = {
    lines: null,
    _transactionCurrencyId$1: null,
    
    isEditForm: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$isEditForm() {
        if (window.parent.Xrm.Page.ui != null) {
            return (window.parent.Xrm.Page.ui.getFormType() === 10*.2);
        }
        else {
            return true;
        }
    },
    
    getQuoteId: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$getQuoteId() {
        var quoteId = 'DB040ECD-5ED4-E211-9BE0-000C299FFE7D';
        if (window.parent.Xrm.Page.ui != null) {
            var guid = window.parent.Xrm.Page.data.entity.getId();
            if (guid != null) {
                quoteId = guid.replaceAll('{', '').replaceAll('}', '');
            }
            this._transactionCurrencyId$1 = (window.parent.Xrm.Page.getAttribute('transactioncurrencyid').getValue())[0].id;
        }
        return quoteId;
    },
    
    newLineFactory: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$newLineFactory(item) {
        var newLine = new Client.QuoteLineItemEditor.Model.QuoteDetail();
        $.extend(newLine, item);
        newLine.lineitemnumber = this.lines.getPagingInfo().totalRows + 1;
        newLine.quoteid = new Xrm.Sdk.EntityReference(new Xrm.Sdk.Guid(this.getQuoteId()), 'quote', null);
        if (this._transactionCurrencyId$1 != null) {
            newLine.transactioncurrencyid = new Xrm.Sdk.EntityReference(new Xrm.Sdk.Guid(this._transactionCurrencyId$1), 'transactioncurrency', '');
        }
        return newLine;
    },
    
    _onSelectedRowsChanged$1: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$_onSelectedRowsChanged$1() {
        var selectedContacts = this.lines.getSelectedRows();
        if (selectedContacts.length > 0) {
            var observableQuoteDetail = this.selectedQuoteDetail();
            if (observableQuoteDetail.innerQuoteDetail != null) {
                observableQuoteDetail.innerQuoteDetail.remove_propertyChanged(ss.Delegate.create(this, this._quote_PropertyChanged$1));
            }
            var selectedQuoteDetail = this.lines.getItem(selectedContacts[0].fromRow);
            if (selectedQuoteDetail != null) {
                selectedQuoteDetail.add_propertyChanged(ss.Delegate.create(this, this._quote_PropertyChanged$1));
            }
            this.selectedQuoteDetail().setValue(selectedQuoteDetail);
        }
        else {
            this.selectedQuoteDetail().setValue(null);
        }
    },
    
    _quote_PropertyChanged$1: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$_quote_PropertyChanged$1(sender, e) {
        window.setTimeout(ss.Delegate.create(this, function() {
            this.lines.refresh();
        }), 0);
    },
    
    productSearchCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$productSearchCommand(term, callback) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='product'>\r\n                                <attribute name='productid' />\r\n                                <attribute name='name' />\r\n                                <order attribute='name' descending='false' />\r\n                                <filter type='and'>\r\n                                  <condition attribute='name' operator='like' value='%{0}%' />\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    uoMSearchCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$uoMSearchCommand(term, callback) {
        var fetchXml = " <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                                  <entity name='uom'>\r\n                                    <attribute name='uomid' />\r\n                                    <attribute name='name' />\r\n                                    <order attribute='name' descending='false' />\r\n                                    <filter type='and'>\r\n                                      <condition attribute='name' operator='like' value='%{0}%' />\r\n                                    </filter>\r\n                                  </entity>\r\n                                </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    salesRepSearchCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$salesRepSearchCommand(term, callback) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                                  <entity name='systemuser'>\r\n                                    <attribute name='fullname' />\r\n                                    <attribute name='businessunitid' />\r\n                                    <attribute name='title' />\r\n                                    <attribute name='address1_telephone1' />\r\n                                    <attribute name='systemuserid' />\r\n                                    <order attribute='fullname' descending='false' />\r\n                                    <filter type='and'>\r\n                                      <condition attribute='fullname' operator='like' value='%{0}%' />\r\n                                    </filter>\r\n                                  </entity>\r\n                                </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    _saveNextRecord$1: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$_saveNextRecord$1(dirtyCollection, errorMessages, callBack) {
        var itemToSave = dirtyCollection[0];
        if (itemToSave.entityState === Xrm.Sdk.EntityStates.deleted) {
            Xrm.Sdk.OrganizationServiceProxy.beginDelete('quotedetail', itemToSave.quotedetailid, ss.Delegate.create(this, function(r) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endDelete(r);
                    itemToSave.entityState = Xrm.Sdk.EntityStates.unchanged;
                }
                catch (ex) {
                    errorMessages.add(ex.message);
                }
                this._finishSaveRecord$1(dirtyCollection, errorMessages, callBack, itemToSave);
            }));
        }
        else if (itemToSave.quotedetailid == null) {
            Xrm.Sdk.OrganizationServiceProxy.beginCreate(itemToSave, ss.Delegate.create(this, function(r) {
                try {
                    var newID = Xrm.Sdk.OrganizationServiceProxy.endCreate(r);
                    itemToSave.quotedetailid = newID;
                    itemToSave.entityState = Xrm.Sdk.EntityStates.unchanged;
                }
                catch (ex) {
                    errorMessages.add(ex.message);
                }
                this._finishSaveRecord$1(dirtyCollection, errorMessages, callBack, itemToSave);
            }));
        }
        else {
            Xrm.Sdk.OrganizationServiceProxy.beginUpdate(itemToSave, ss.Delegate.create(this, function(r) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endUpdate(r);
                    itemToSave.entityState = Xrm.Sdk.EntityStates.unchanged;
                }
                catch (ex) {
                    errorMessages.add(ex.message);
                }
                this._finishSaveRecord$1(dirtyCollection, errorMessages, callBack, itemToSave);
            }));
        }
    },
    
    _finishSaveRecord$1: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$_finishSaveRecord$1(dirtyCollection, errorMessages, callBack, itemToSave) {
        dirtyCollection.remove(itemToSave);
        if (!dirtyCollection.length) {
            callBack();
        }
        else {
            this._saveNextRecord$1(dirtyCollection, errorMessages, callBack);
        }
    },
    
    _saveCommand$1: null,
    
    saveCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$saveCommand() {
        if (this._saveCommand$1 == null) {
            this._saveCommand$1 = ss.Delegate.create(this, function() {
                if (!this.commitEdit()) {
                    return;
                }
                var dirtyCollection = [];
                var $enum1 = ss.IEnumerator.getEnumerator(this.lines.get_data());
                while ($enum1.moveNext()) {
                    var item = $enum1.current;
                    if (item != null && item.entityState !== Xrm.Sdk.EntityStates.unchanged) {
                        dirtyCollection.add(item);
                    }
                }
                if (this.lines.deleteData != null) {
                    var $enum2 = ss.IEnumerator.getEnumerator(this.lines.deleteData);
                    while ($enum2.moveNext()) {
                        var item = $enum2.current;
                        if (item.entityState === Xrm.Sdk.EntityStates.deleted) {
                            dirtyCollection.add(item);
                        }
                    }
                }
                var itemCount = dirtyCollection.length;
                if (!itemCount) {
                    return;
                }
                var confirmed = confirm(String.format('Are you sure that you want to save the {0} quote lines in the Grid?', itemCount));
                if (!confirmed) {
                    return;
                }
                this.isBusy(true);
                var errorMessages = [];
                this._saveNextRecord$1(dirtyCollection, errorMessages, ss.Delegate.create(this, function() {
                    if (errorMessages.length > 0) {
                        alert('One or more records failed to save.\nPlease contact your System Administrator.\n\n' + errorMessages.join(','));
                    }
                    if (this.lines.deleteData != null) {
                        this.lines.deleteData.clear();
                    }
                    this.lines.refresh();
                    this.isBusy(false);
                }));
            });
        }
        return this._saveCommand$1;
    },
    
    _deleteCommand$1: null,
    
    deleteCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$deleteCommand() {
        if (this._deleteCommand$1 == null) {
            this._deleteCommand$1 = ss.Delegate.create(this, function() {
                var selectedRows = SparkleXrm.GridEditor.DataViewBase.rangesToRows(this.lines.getSelectedRows());
                if (!selectedRows.length) {
                    return;
                }
                var confirmed = confirm(String.format('Are you sure that you want to delete the {0} quote lines in the Grid?', selectedRows.length));
                if (!confirmed) {
                    return;
                }
                var itemsToRemove = [];
                var $enum1 = ss.IEnumerator.getEnumerator(selectedRows);
                while ($enum1.moveNext()) {
                    var row = $enum1.current;
                    itemsToRemove.add(this.lines.getItem(row));
                }
                var $enum2 = ss.IEnumerator.getEnumerator(itemsToRemove);
                while ($enum2.moveNext()) {
                    var item = $enum2.current;
                    item.entityState = Xrm.Sdk.EntityStates.deleted;
                    this.lines.removeItem(item);
                }
                this.lines.refresh();
            });
        }
        return this._deleteCommand$1;
    },
    
    _moveUpCommand$1: null,
    
    moveUpCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$moveUpCommand() {
        if (this._moveUpCommand$1 == null) {
            this._moveUpCommand$1 = ss.Delegate.create(this, function() {
                var range = this.lines.getSelectedRows();
                var fromRow = range[0].fromRow;
                if (!fromRow) {
                    return;
                }
                var line = this.lines.getItem(fromRow);
                var lineBefore = this.lines.getItem(fromRow - 1);
                var lineItemNumber = line.lineitemnumber;
                line.lineitemnumber = lineBefore.lineitemnumber;
                lineBefore.lineitemnumber = lineItemNumber;
                line.raisePropertyChanged('LineItemNumber');
                lineBefore.raisePropertyChanged('LineItemNumber');
                range[0].fromRow--;
                this.lines.raiseOnSelectedRowsChanged(range);
                this.lines.sortBy(new SparkleXrm.GridEditor.SortCol('lineitemnumber', true));
                this.lines.refresh();
            });
        }
        return this._moveUpCommand$1;
    },
    
    _moveDownCommand$1: null,
    
    moveDownCommand: function Client_QuoteLineItemEditor_ViewModels_QuoteLineItemEditorViewModel$moveDownCommand() {
        if (this._moveDownCommand$1 == null) {
            this._moveDownCommand$1 = ss.Delegate.create(this, function() {
                var range = this.lines.getSelectedRows();
                var fromRow = range[0].fromRow;
                if (fromRow === this.lines.getLength() - 1) {
                    return;
                }
                var line = this.lines.getItem(fromRow);
                var lineAfter = this.lines.getItem(fromRow + 1);
                var lineItemNumber = line.lineitemnumber;
                line.raisePropertyChanged('LineItemNumber');
                lineAfter.raisePropertyChanged('LineItemNumber');
                line.lineitemnumber = lineAfter.lineitemnumber;
                lineAfter.lineitemnumber = lineItemNumber;
                range[0].fromRow++;
                this.lines.raiseOnSelectedRowsChanged(range);
                this.lines.sortBy(new SparkleXrm.GridEditor.SortCol('lineitemnumber', true));
                this.lines.refresh();
            });
        }
        return this._moveDownCommand$1;
    }
}


Type.registerNamespace('Client.QuoteLineItemEditor.Views');

////////////////////////////////////////////////////////////////////////////////
// Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView

Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView = function Client_QuoteLineItemEditor_Views_QuoteLineItemEditorView() {
    Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView.initializeBase(this);
}
Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView.init = function Client_QuoteLineItemEditor_Views_QuoteLineItemEditorView$init() {
    var vm = new Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel();
    var columns = [];
    SparkleXrm.GridEditor.GridDataViewBinder.addEditIndicatorColumn(columns);
    SparkleXrm.GridEditor.XrmNumberEditor.bindReadOnlyColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, '#', 40, 'lineitemnumber'), 0);
    SparkleXrm.GridEditor.XrmLookupEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Existing Product', 200, 'productid'), ss.Delegate.create(vm, vm.productSearchCommand), 'productid', 'name', '');
    SparkleXrm.GridEditor.XrmLookupEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Units', 100, 'uomid'), ss.Delegate.create(vm, vm.uoMSearchCommand), 'uomid', 'name', '');
    SparkleXrm.GridEditor.XrmTextEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Write-In Product', 200, 'productdescription'));
    SparkleXrm.GridEditor.XrmMoneyEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Price Per Unit', 200, 'priceperunit'), 0, 1000);
    SparkleXrm.GridEditor.XrmNumberEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Quantity', 200, 'quantity'), 0, 1000, 2);
    SparkleXrm.GridEditor.XrmMoneyEditor.bindReadOnlyColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Extended Amount', 100, 'extendedamount'));
    var contactGridDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var contactsGrid = contactGridDataBinder.dataBindXrmGrid(vm.lines, columns, 'quoteproductGrid', 'quoteproductPager', true, true);
    contactGridDataBinder.bindCommitEdit(vm);
    SparkleXrm.ViewBase.registerViewModel(vm);
    window.setTimeout(function() {
        vm.lines.refresh();
    }, 0);
}


Client.QuoteLineItemEditor.Model.QuoteDetail.registerClass('Client.QuoteLineItemEditor.Model.QuoteDetail', Xrm.Sdk.Entity);
Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail.registerClass('Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail');
Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.registerClass('Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation');
Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel.registerClass('Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel', SparkleXrm.ViewModelBase);
Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView.registerClass('Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView', SparkleXrm.ViewBase);
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
