//! Client.debug.js
//
waitForScripts("client",["mscorlib","xrm","xrmui", "jquery", "jquery-ui"],
function () {

(function($){

////////////////////////////////////////////////////////////////////////////////
// ActivityPointer

window.ActivityPointer = function ActivityPointer() {
    ActivityPointer.initializeBase(this, [ 'activitypointer' ]);
}
ActivityPointer.prototype = {
    activityid: null,
    subject: null,
    activitytypecode: null
}


////////////////////////////////////////////////////////////////////////////////
// dev1_session

window.dev1_session = function dev1_session() {
    dev1_session.initializeBase(this, [ 'dev1_session' ]);
    this._metaData['dev1_duration'] = Xrm.Sdk.AttributeTypes.int_;
}
dev1_session.prototype = {
    createdon: null,
    dev1_description: null,
    dev1_duration: null,
    dev1_activityid: null,
    dev1_activitytypename: null,
    dev1_emailid: null,
    dev1_phonecallid: null,
    dev1_letterid: null,
    dev1_taskid: null,
    dev1_endtime: null,
    dev1_sessionid: null,
    dev1_starttime: null
}


Type.registerNamespace('AddressSearch');

////////////////////////////////////////////////////////////////////////////////
// AddressSearch.App

AddressSearch.App = function AddressSearch_App() {
}


Type.registerNamespace('Client.ContactEditor.ViewModels');

////////////////////////////////////////////////////////////////////////////////
// Client.ContactEditor.ViewModels.ContactValidation

Client.ContactEditor.ViewModels.ContactValidation = function Client_ContactEditor_ViewModels_ContactValidation() {
}
Client.ContactEditor.ViewModels.ContactValidation.firstName = function Client_ContactEditor_ViewModels_ContactValidation$firstName(rules, viewModel, dataContext) {
    var self = ko.utils.unwrapObservable(viewModel);
    return rules.addRequiredMsg('Enter a first name').addRule('Must be less than 200 chars', function(value) {
        var valueText = value;
        return (valueText.length < 200);
    }).addRule("Firstname can't be the same as the lastname", function(value) {
        var isValid = true;
        var lastName = ko.utils.unwrapObservable(self.lastname);
        if (lastName != null && value === lastName) {
            isValid = false;
        }
        return isValid;
    });
}
Client.ContactEditor.ViewModels.ContactValidation.birthDate = function Client_ContactEditor_ViewModels_ContactValidation$birthDate(rules, viewModel, dataContext) {
    var self = viewModel;
    return rules.addRule("Birthdate can't be in the future", function(value) {
        var birthdate = value;
        return (birthdate < Date.get_today());
    });
}
Client.ContactEditor.ViewModels.ContactValidation.accountRoleCode = function Client_ContactEditor_ViewModels_ContactValidation$accountRoleCode(rules, viewModel, dataContext) {
    return rules.addRule('Account Role is required.', function(value) {
        return (value != null) && (value).value != null;
    });
}
Client.ContactEditor.ViewModels.ContactValidation.register = function Client_ContactEditor_ViewModels_ContactValidation$register(binder) {
    binder.register('firstname', Client.ContactEditor.ViewModels.ContactValidation.firstName);
    binder.register('accountrolecode', Client.ContactEditor.ViewModels.ContactValidation.accountRoleCode);
    binder.register('birthdate', Client.ContactEditor.ViewModels.ContactValidation.birthDate);
}


////////////////////////////////////////////////////////////////////////////////
// Client.ContactEditor.ViewModels.ObservableContact

Client.ContactEditor.ViewModels.ObservableContact = function Client_ContactEditor_ViewModels_ObservableContact() {
    this.entityState = ko.observable();
    this.fullname = ko.observable();
    this.firstname = ko.observable();
    this.lastname = ko.observable();
    this.birthdate = ko.observable();
    this.accountrolecode = ko.observable();
    this.numberofchildren = ko.observable();
    this.transactioncurrencyid = ko.observable();
    this.creditlimit = ko.observable();
}
Client.ContactEditor.ViewModels.ObservableContact.prototype = {
    _value: null,
    
    setValue: function Client_ContactEditor_ViewModels_ObservableContact$setValue(value) {
        this._value = value;
        if (value == null) {
            this._value = new Client.ContactEditor.Model.Contact();
        }
        this.entityState(value.entityState);
        this.fullname(this._value.fullname);
        this.firstname(this._value.firstname);
        this.lastname(this._value.lastname);
        this.birthdate(this._value.birthdate);
        this.accountrolecode(this._value.accountrolecode);
        this.numberofchildren(this._value.numberofchildren);
        this.transactioncurrencyid(this._value.transactioncurrencyid);
        this.creditlimit(this._value.creditlimit);
    },
    
    commit: function Client_ContactEditor_ViewModels_ObservableContact$commit() {
        this._value.fullname = this.fullname();
        this._value.firstname = this.firstname();
        this._value.lastname = this.lastname();
        this._value.birthdate = this.birthdate();
        this._value.accountrolecode = this.accountrolecode();
        this._value.numberofchildren = this.numberofchildren();
        this._value.transactioncurrencyid = this.transactioncurrencyid();
        this._value.creditlimit = this.creditlimit();
        this._value.entityState = Xrm.Sdk.EntityStates.changed;
        return this._value;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.ContactEditor.ViewModels.ContactsEditorViewModel

Client.ContactEditor.ViewModels.ContactsEditorViewModel = function Client_ContactEditor_ViewModels_ContactsEditorViewModel() {
    Client.ContactEditor.ViewModels.ContactsEditorViewModel.initializeBase(this);
    this.selectedContact = ko.validatedObservable(new Client.ContactEditor.ViewModels.ObservableContact());
    this.contacts = new SparkleXrm.GridEditor.EntityDataViewModel(10, Client.ContactEditor.Model.Contact, true);
    this.contacts.add_onSelectedRowsChanged(ss.Delegate.create(this, this.onSelectedRowsChanged));
    this.contacts.set_fetchXml("<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>\r\n                <entity name='contact'>\r\n                    <attribute name='firstname' />\r\n                    <attribute name='lastname' />\r\n                    <attribute name='telephone1' />\r\n                    <attribute name='birthdate' />\r\n                    <attribute name='accountrolecode' />\r\n                    <attribute name='parentcustomerid'/>\r\n                    <attribute name='transactioncurrencyid'/>\r\n                    <attribute name='creditlimit'/>\r\n                    <attribute name='numberofchildren'/>\r\n                    <attribute name='contactid' />{3}\r\n                  </entity>\r\n                </fetch>");
    Client.ContactEditor.ViewModels.ContactValidation.register(this.contacts.validationBinder);
    Client.ContactEditor.ViewModels.ContactValidation.register(new SparkleXrm.ObservableValidationBinder(this.selectedContact));
}
Client.ContactEditor.ViewModels.ContactsEditorViewModel.prototype = {
    contacts: null,
    selectedContact: null,
    
    onSelectedRowsChanged: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$onSelectedRowsChanged() {
        var selectedContacts = this.contacts.getSelectedRows();
        if (selectedContacts.length > 0) {
            this.selectedContact().setValue(this.contacts.getItem(selectedContacts[0].fromRow));
        }
        else {
            this.selectedContact().setValue(null);
        }
    },
    
    _canAddNew$1: null,
    
    canAddNew: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$canAddNew() {
        if (this._canAddNew$1 == null) {
            var IsRegisterFormValidDependantProperty = {};
            IsRegisterFormValidDependantProperty.owner = this;
            IsRegisterFormValidDependantProperty.read = ss.Delegate.create(this, function() {
                var state = this.selectedContact().entityState();
                if (state != null) {
                    return state !== Xrm.Sdk.EntityStates.created;
                }
                else {
                    return true;
                }
            });
            this._canAddNew$1 = ko.dependentObservable(IsRegisterFormValidDependantProperty);
        }
        return this._canAddNew$1;
    },
    
    _saveSelectedContact$1: null,
    
    saveSelectedContact: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$saveSelectedContact() {
        if (this._saveSelectedContact$1 == null) {
            this._saveSelectedContact$1 = ss.Delegate.create(this, function() {
                if ((this.selectedContact).isValid()) {
                    var contact = this.selectedContact().commit();
                    var selectedContact = this.selectedContact();
                    if (selectedContact.entityState() === Xrm.Sdk.EntityStates.created) {
                        this.contacts.addItem(contact);
                        var paging = this.contacts.getPagingInfo();
                        var newRow = new Array(1);
                        newRow[0] = {};
                        newRow[0].fromRow = paging.totalRows - ((paging.totalPages - 1) * paging.pageSize) - 1;
                        this.contacts.raiseOnSelectedRowsChanged(newRow);
                        selectedContact.entityState(Xrm.Sdk.EntityStates.changed);
                    }
                    else {
                        this.contacts.refresh();
                    }
                }
                else {
                    this.selectedContact.errors.showAllMessages();
                }
            });
        }
        return this._saveSelectedContact$1;
    },
    
    _addNewContact$1: null,
    
    addNewContact: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$addNewContact() {
        if (this._addNewContact$1 == null) {
            this._addNewContact$1 = ss.Delegate.create(this, function() {
                var newContact = new Client.ContactEditor.Model.Contact();
                newContact.entityState = Xrm.Sdk.EntityStates.created;
                this.selectedContact().setValue(newContact);
                this.selectedContact.errors.showAllMessages(false);
            });
        }
        return this._addNewContact$1;
    },
    
    _resetCommand$1: null,
    
    resetCommand: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$resetCommand() {
        if (this._resetCommand$1 == null) {
            this._resetCommand$1 = ss.Delegate.create(this, function() {
                var confirmed = confirm(String.format('Are you sure you want to reset the grid? This will loose any values you have edited.'));
                if (!confirmed) {
                    return;
                }
                this.contacts.reset();
                this.contacts.refresh();
            });
        }
        return this._resetCommand$1;
    },
    
    _saveCommand$1: null,
    
    saveCommand: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$saveCommand() {
        if (this._saveCommand$1 == null) {
            this._saveCommand$1 = ss.Delegate.create(this, function() {
                var dirtyCollection = [];
                var $enum1 = ss.IEnumerator.getEnumerator(this.contacts.get_data());
                while ($enum1.moveNext()) {
                    var item = $enum1.current;
                    if (item != null && item.entityState !== Xrm.Sdk.EntityStates.unchanged) {
                        dirtyCollection.add(item);
                    }
                }
                var itemCount = dirtyCollection.length;
                if (!itemCount) {
                    return;
                }
                var confirmed = confirm(String.format('Are you sure that you want to save the {0} records edited in the Grid?', itemCount));
                if (!confirmed) {
                    return;
                }
                this.isBusy(true);
                var errorMessage = '';
                this._saveNextRecord$1(dirtyCollection, errorMessage, ss.Delegate.create(this, function() {
                    if (errorMessage.length > 0) {
                        alert('One or more records failed to save.\nPlease contact your System Administrator.\n\n' + errorMessage);
                    }
                    else {
                        alert('Save Complete!');
                    }
                    this.contacts.refresh();
                    this.isBusy(false);
                }));
            });
        }
        return this._saveCommand$1;
    },
    
    transactionCurrencySearchCommand: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$transactionCurrencySearchCommand(term, callback) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='transactioncurrency'>\r\n                                <attribute name='transactioncurrencyid' />\r\n                                <attribute name='currencyname' />\r\n                                <attribute name='isocurrencycode' />\r\n                                <attribute name='currencysymbol' />\r\n                                <attribute name='exchangerate' />\r\n                                <attribute name='currencyprecision' />\r\n                                <order attribute='currencyname' descending='false' />\r\n                                <filter type='and'>\r\n                                  <condition attribute='currencyname' operator='like' value='%{0}%' />\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    reportError: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$reportError(ex) {
    },
    
    _saveNextRecord$1: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$_saveNextRecord$1(dirtyCollection, errorMessage, callBack) {
        var contactToSave = dirtyCollection[0];
        if (contactToSave.contactid == null) {
            Xrm.Sdk.OrganizationServiceProxy.beginCreate(contactToSave, ss.Delegate.create(this, function(r) {
                try {
                    var newID = Xrm.Sdk.OrganizationServiceProxy.endCreate(r);
                    contactToSave.contactid = newID;
                    contactToSave.entityState = Xrm.Sdk.EntityStates.unchanged;
                }
                catch (ex) {
                    errorMessage = errorMessage + ex.message + '\n';
                }
                dirtyCollection.remove(contactToSave);
                if (!dirtyCollection.length) {
                    callBack();
                }
                else {
                    this._saveNextRecord$1(dirtyCollection, errorMessage, callBack);
                }
            }));
        }
        else {
            Xrm.Sdk.OrganizationServiceProxy.beginUpdate(contactToSave, ss.Delegate.create(this, function(r) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endUpdate(r);
                    contactToSave.entityState = Xrm.Sdk.EntityStates.unchanged;
                }
                catch (ex) {
                    errorMessage = errorMessage + ex.message + '\n';
                }
                dirtyCollection.remove(contactToSave);
                if (!dirtyCollection.length) {
                    callBack();
                }
                else {
                    this._saveNextRecord$1(dirtyCollection, errorMessage, callBack);
                }
            }));
        }
    },
    
    init: function Client_ContactEditor_ViewModels_ContactsEditorViewModel$init() {
        this.contacts.refresh();
    }
}


Type.registerNamespace('Client.DataGrouping.ViewModels');

////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.ViewModels.DataGroupingViewModel

Client.DataGrouping.ViewModels.DataGroupingViewModel = function Client_DataGrouping_ViewModels_DataGroupingViewModel() {
    Client.DataGrouping.ViewModels.DataGroupingViewModel.initializeBase(this);
    var options = {};
    options.groupItemMetadataProvider = new Slick.Data.GroupItemMetadataProvider();
    options.inlineFilters = true;
    this.projects = new Slick.Data.DataView(options);
    this.projects.beginUpdate();
    this.projects.setFilter(Client.DataGrouping.ViewModels.DataGroupingViewModel.myFilter);
    var filterArguments = { percentComplete: 0 };
    this.projects.setFilterArgs(filterArguments);
    this.loadData();
    this.setGrouping();
    this.projects.endUpdate();
}
Client.DataGrouping.ViewModels.DataGroupingViewModel.myFilter = function Client_DataGrouping_ViewModels_DataGroupingViewModel$myFilter(item, args) {
    return true;
}
Client.DataGrouping.ViewModels.DataGroupingViewModel.prototype = {
    projects: null,
    sortCol: null,
    sortDir: 0,
    
    setGrouping: function Client_DataGrouping_ViewModels_DataGroupingViewModel$setGrouping() {
        var durationGrouping = {};
        durationGrouping.getter = 'duration';
        durationGrouping.aggregators = [new Slick.Data.Aggregators.Sum('duration'), new Slick.Data.Aggregators.Sum('cost')];
        durationGrouping.aggregateCollapsed = false;
        var effortGrouping = {};
        effortGrouping.getter = 'effortDriven';
        effortGrouping.aggregators = [new Slick.Data.Aggregators.Sum('duration'), new Slick.Data.Aggregators.Sum('cost')];
        effortGrouping.collapsed = false;
        var percentGrouping = {};
        percentGrouping.getter = 'effortDriven';
        percentGrouping.aggregators = [new Slick.Data.Aggregators.Avg('percentComplete')];
        percentGrouping.collapsed = false;
        this.projects.setGrouping([durationGrouping, effortGrouping, percentGrouping]);
    },
    
    loadData: function Client_DataGrouping_ViewModels_DataGroupingViewModel$loadData() {
        var someDates = ['01/01/2009', '02/02/2009', '03/03/2009'];
        var data = [];
        for (var i = 0; i < 100; i++) {
            var d = new Client.DataGrouping.ViewModels.Project();
            Xrm.ArrayEx.add(data, d);
            d.id = 'id_' + i;
            d.num = i;
            d.title = 'Task ' + i;
            d.duration = Math.round(Math.random() * 14);
            d.percentComplete = Math.round(Math.random() * 100);
            d.start = someDates[Math.floor((Math.random() * 2))];
            d.finish = someDates[Math.floor((Math.random() * 2))];
            d.cost = Math.round(Math.random() * 10000) / 100;
            d.effortDriven = (!(i % 5));
        }
        this.projects.setItems(data);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.ViewModels.Project

Client.DataGrouping.ViewModels.Project = function Client_DataGrouping_ViewModels_Project() {
}
Client.DataGrouping.ViewModels.Project.prototype = {
    id: null,
    num: null,
    title: null,
    duration: null,
    percentComplete: null,
    start: null,
    finish: null,
    cost: null,
    effortDriven: null
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.ViewModels.TreeDataView

Client.DataGrouping.ViewModels.TreeDataView = function Client_DataGrouping_ViewModels_TreeDataView() {
    this._groupStates$1 = {};
    this._groups$1 = {};
    this._items = [];
    this._rows = [];
    Client.DataGrouping.ViewModels.TreeDataView.initializeBase(this);
}
Client.DataGrouping.ViewModels.TreeDataView.prototype = {
    _suspend$1: false,
    _refreshRowsAfter$1: null,
    _sortColumn$1: null,
    
    beginUpdate: function Client_DataGrouping_ViewModels_TreeDataView$beginUpdate() {
        this._suspend$1 = true;
    },
    
    endUpdate: function Client_DataGrouping_ViewModels_TreeDataView$endUpdate() {
        this._suspend$1 = false;
        this.refresh();
    },
    
    sort: function Client_DataGrouping_ViewModels_TreeDataView$sort(sorting) {
        this._sortColumn$1 = new SparkleXrm.GridEditor.SortCol(sorting.sortCol.field, sorting.sortAsc);
        this.refresh();
    },
    
    addItem: function Client_DataGrouping_ViewModels_TreeDataView$addItem(item) {
        this._items.add(item);
        this.refresh();
    },
    
    insertItem: function Client_DataGrouping_ViewModels_TreeDataView$insertItem(insertBefore, item) {
        this._items.insert(insertBefore, item);
        this.refresh();
    },
    
    removeItem: function Client_DataGrouping_ViewModels_TreeDataView$removeItem(id) {
        this._items.remove(id);
        this.refresh();
    },
    
    getItem: function Client_DataGrouping_ViewModels_TreeDataView$getItem(index) {
        return this._rows[index];
    },
    
    getLength: function Client_DataGrouping_ViewModels_TreeDataView$getLength() {
        return this._rows.length;
    },
    
    getItemMetadata: function Client_DataGrouping_ViewModels_TreeDataView$getItemMetadata(i) {
        return Client.DataGrouping.ViewModels.TreeDataView.callBaseMethod(this, 'getItemMetadata', [ i ]);
    },
    
    expandGroup: function Client_DataGrouping_ViewModels_TreeDataView$expandGroup(groupingKey) {
        if (Object.keyExists(this._groupStates$1, groupingKey)) {
            this._groupStates$1[groupingKey] = !this._groupStates$1[groupingKey];
        }
        this._refreshRowsAfter$1 = this._groups$1[groupingKey].count - 1;
        this.refresh();
    },
    
    collapseGroup: function Client_DataGrouping_ViewModels_TreeDataView$collapseGroup(groupingKey) {
        if (Object.keyExists(this._groupStates$1, groupingKey)) {
            this._groupStates$1[groupingKey] = !this._groupStates$1[groupingKey];
        }
        this._refreshRowsAfter$1 = this._groups$1[groupingKey].count - 1;
        this.refresh();
    },
    
    refresh: function Client_DataGrouping_ViewModels_TreeDataView$refresh() {
        if (this._suspend$1) {
            return;
        }
        var rows = [];
        this._flatternGroups$1(rows, null, null, 0);
        this._rows = rows;
        var args = {};
        args.rows = [];
        var startDiffRow = (this._refreshRowsAfter$1 != null) ? this._refreshRowsAfter$1 : 0;
        for (var i = startDiffRow; i < rows.length; i++) {
            args.rows.add(i);
        }
        this.onRowsChanged.notify(args, null, null);
    },
    
    _flatternGroups$1: function Client_DataGrouping_ViewModels_TreeDataView$_flatternGroups$1(rows, parent, parentGroup, parentLevel) {
        var items;
        var level = (parentLevel == null) ? 0 : parentLevel + 1;
        if (parent != null) {
            items = parent.childItems;
        }
        else {
            items = this._items;
        }
        var rowsToAdd = [];
        var sortedItems = [items];
        for (var i = 0; i < items.length; i++) {
            sortedItems[i] = items[i];
        }
        if (this._sortColumn$1 != null) {
            this._sortBy$1(this._sortColumn$1, sortedItems);
        }
        var $enum1 = ss.IEnumerator.getEnumerator(sortedItems);
        while ($enum1.moveNext()) {
            var item = $enum1.current;
            var groupedItem = item;
            if (typeof(groupedItem.childItems) !== 'undefined') {
                var groupingKey = ((parent != null) ? parent.id : '') + '|' + groupedItem.id;
                var group;
                if (Object.keyExists(this._groups$1, groupingKey)) {
                    group = this._groups$1[groupingKey];
                }
                else {
                    group = {};
                    this._groups$1[groupingKey] = group;
                }
                group.title = groupedItem.title.toString();
                group.rows = [];
                group.groupingKey = groupingKey;
                var collapsed = false;
                if (Object.keyExists(this._groupStates$1, groupingKey)) {
                    collapsed = this._groupStates$1[groupingKey];
                }
                else {
                    this._groupStates$1[groupingKey] = collapsed;
                }
                group.level = level;
                rowsToAdd.add(group);
                group.count = rows.length + rowsToAdd.length;
                group.collapsed = collapsed;
                if (!collapsed) {
                    this._flatternGroups$1(rowsToAdd, groupedItem, group, level);
                }
            }
            else {
                rowsToAdd.add(item);
            }
        }
        rows.addRange(rowsToAdd);
    },
    
    _sortBy$1: function Client_DataGrouping_ViewModels_TreeDataView$_sortBy$1(col, data) {
        if (!col.ascending) {
            data.reverse();
        }
        data.sort(function(a, b) {
            var l = (a)[col.attributeName];
            var r = (b)[col.attributeName];
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
                        if (l<r) {
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
        });
        if (!col.ascending) {
            data.reverse();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.ViewModels.TreeDataViewModel

Client.DataGrouping.ViewModels.TreeDataViewModel = function Client_DataGrouping_ViewModels_TreeDataViewModel() {
    Client.DataGrouping.ViewModels.TreeDataViewModel.initializeBase(this);
    this.items = new Client.DataGrouping.ViewModels.TreeDataView();
    this.items.beginUpdate();
    var idCounter = 1;
    var group1 = Client.DataGrouping.ViewModels.TreeDataViewModel._createGroup$1('Project 1', idCounter.toString());
    group1.childItems = [];
    for (var i = 2; i < 100; i++) {
        idCounter++;
        var group2 = new Client.DataGrouping.ViewModels.GroupedItems();
        group2.title = 'Project ' + idCounter.toString();
        group2.id = new Xrm.Sdk.Guid(idCounter.toString());
        group2.childItems = [];
        for (var j = 0; j < 25; j++) {
            idCounter++;
            Client.DataGrouping.ViewModels.TreeDataViewModel._addItem$1(group2, 'Item ' + idCounter.toString());
        }
        group1.childItems.add(group2);
    }
    this.items.addItem(group1);
    this.items.endUpdate();
}
Client.DataGrouping.ViewModels.TreeDataViewModel._createGroup$1 = function Client_DataGrouping_ViewModels_TreeDataViewModel$_createGroup$1(projectName, id) {
    var group1 = new Client.DataGrouping.ViewModels.GroupedItems();
    group1.title = projectName;
    group1.childItems = [];
    group1.id = new Xrm.Sdk.Guid(id);
    return group1;
}
Client.DataGrouping.ViewModels.TreeDataViewModel._addItem$1 = function Client_DataGrouping_ViewModels_TreeDataViewModel$_addItem$1(group2, title) {
    var item1 = new Client.DataGrouping.ViewModels.TreeItem();
    item1.id = new Xrm.Sdk.Guid('3');
    item1.status = '1';
    item1.project = title;
    group2.childItems.add(item1);
}
Client.DataGrouping.ViewModels.TreeDataViewModel.prototype = {
    items: null
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.ViewModels.TreeItem

Client.DataGrouping.ViewModels.TreeItem = function Client_DataGrouping_ViewModels_TreeItem() {
}
Client.DataGrouping.ViewModels.TreeItem.prototype = {
    id: null,
    status: null,
    project: null,
    date: null,
    employee: null,
    duration: null,
    items: null
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.ViewModels.GroupedItems

Client.DataGrouping.ViewModels.GroupedItems = function Client_DataGrouping_ViewModels_GroupedItems() {
}
Client.DataGrouping.ViewModels.GroupedItems.prototype = {
    id: null,
    title: null,
    childItems: null
}


Type.registerNamespace('Client.DataGrouping.Views');

////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.Views.DataGroupingView

Client.DataGrouping.Views.DataGroupingView = function Client_DataGrouping_Views_DataGroupingView() {
}
Client.DataGrouping.Views.DataGroupingView.init = function Client_DataGrouping_Views_DataGroupingView$init() {
    var vm = new Client.DataGrouping.ViewModels.DataGroupingViewModel();
    var numberFormatInfo = Xrm.NumberEx.getNumberFormatInfo();
    numberFormatInfo.minValue = 0;
    numberFormatInfo.maxValue = 1000;
    numberFormatInfo.precision = 2;
    var boolFormatInfo = {};
    boolFormatInfo.falseOptionDisplayName = 'No';
    boolFormatInfo.trueOptionDisplayName = 'Yes';
    var sumTotalsFormatterDelegate = Client.DataGrouping.Views.DataGroupingView.sumTotalsFormatter;
    var avgTotalsFormatterDelegate = Client.DataGrouping.Views.DataGroupingView.avgTotalsFormatter;
    var percentageFormatter = SparkleXrm.GridEditor.XrmNumberEditor.formatter;
    var checkboxFormatter = SparkleXrm.GridEditor.XrmBooleanEditor.formatter;
    var columns = [{ id: 'sel', name: '#', field: 'num', cssClass: 'cell-selection', width: 40, resizable: false, selectable: false, focusable: false }, { id: 'title', name: 'Title', field: 'title', width: 70, minWidth: 50, cssClass: 'cell-title', sortable: true, editor: SparkleXrm.GridEditor.XrmTextEditor.textEditor }, { id: 'duration', name: 'Duration', field: 'duration', width: 70, sortable: true, groupTotalsFormatter: sumTotalsFormatterDelegate }, { id: '%', name: '% Complete', field: 'percentComplete', width: 80, formatter: percentageFormatter, options: numberFormatInfo, sortable: true, groupTotalsFormatter: avgTotalsFormatterDelegate }, { id: 'start', name: 'Start', field: 'start', minWidth: 60, sortable: true }, { id: 'finish', name: 'Finish', field: 'finish', minWidth: 60, sortable: true }, { id: 'cost', name: 'Cost', field: 'cost', width: 90, sortable: true, groupTotalsFormatter: sumTotalsFormatterDelegate }, { id: 'effort-driven', name: 'Effort Driven', width: 80, minWidth: 20, cssClass: 'cell-effort-driven', field: 'effortDriven', formatter: checkboxFormatter, options: boolFormatInfo, sortable: true }];
    var options = {};
    options.enableCellNavigation = true;
    options.editable = true;
    var view = vm.projects;
    var binder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var grid = binder.dataBindDataViewGrid(vm.projects, columns, 'myGrid', null, true, false);
    grid.registerPlugin(new Slick.Data.GroupItemMetadataProvider());
    SparkleXrm.ViewBase.registerViewModel(vm);
}
Client.DataGrouping.Views.DataGroupingView.sumTotalsFormatter = function Client_DataGrouping_Views_DataGroupingView$sumTotalsFormatter(totals, columnDef) {
    var sum = totals['sum'];
    var val = (sum != null) ? sum[columnDef.field] : null;
    if (val != null) {
        return 'avg: ' + Math.round(val).toString() + ' %';
    }
    return '';
}
Client.DataGrouping.Views.DataGroupingView.avgTotalsFormatter = function Client_DataGrouping_Views_DataGroupingView$avgTotalsFormatter(totals, columnDef) {
    var avg = totals['avg'];
    var val = (avg != null) ? avg[columnDef.field] : null;
    if (val != null) {
        return 'total: ' + (Math.round(val * 100) / 100);
    }
    return '';
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.Views.GroupGridRowPlugin

Client.DataGrouping.Views.GroupGridRowPlugin = function Client_DataGrouping_Views_GroupGridRowPlugin() {
}
Client.DataGrouping.Views.GroupGridRowPlugin.prototype = {
    _grid: null,
    
    init: function Client_DataGrouping_Views_GroupGridRowPlugin$init(grid) {
        this._grid = grid;
        this._grid.onClick.subscribe(ss.Delegate.create(this, this.handleGridClick));
    },
    
    destroy: function Client_DataGrouping_Views_GroupGridRowPlugin$destroy() {
        if (this._grid != null) {
            this._grid.onClick.unsubscribe(ss.Delegate.create(this, this.handleGridClick));
        }
    },
    
    handleGridClick: function Client_DataGrouping_Views_GroupGridRowPlugin$handleGridClick(e, a) {
        var args = a;
        var item = this._grid.getDataItem(args.row);
        if (Type.getInstanceType(item) === Object) {
            var dataView = this._grid.getData();
            var group = item;
            if (group.collapsed) {
                dataView.expandGroup(group.groupingKey);
            }
            else {
                dataView.collapseGroup(group.groupingKey);
            }
            e.stopImmediatePropagation();
            e.preventDefault();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.DataGrouping.Views.TreeView

Client.DataGrouping.Views.TreeView = function Client_DataGrouping_Views_TreeView() {
}
Client.DataGrouping.Views.TreeView.init = function Client_DataGrouping_Views_TreeView$init() {
    var vm = new Client.DataGrouping.ViewModels.TreeDataViewModel();
    var binder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var columns = [{ id: 'status', name: 'Status', field: 'status', width: 70, minWidth: 50, cssClass: 'cell-title', sortable: true }, { id: 'project', name: 'Project', field: 'project', width: 70, minWidth: 50, cssClass: 'cell-title', sortable: true, editor: SparkleXrm.GridEditor.XrmTextEditor.textEditor }, { id: 'date', name: 'Date', field: 'date', width: 70, sortable: true }, { id: 'employee', name: 'Employee', field: 'employee', width: 80, sortable: true }, { id: 'duration', name: 'Duration', field: 'duration', minWidth: 60, sortable: true }];
    vm.items.add_onGetItemMetaData(Client.DataGrouping.Views.TreeView._items_OnGetItemMetaData);
    var grid = binder.dataBindXrmGrid(vm.items, columns, 'projectsGrid', null, true, false);
    grid.registerPlugin(new Client.DataGrouping.Views.GroupGridRowPlugin());
    SparkleXrm.ViewBase.registerViewModel(vm);
}
Client.DataGrouping.Views.TreeView._items_OnGetItemMetaData = function Client_DataGrouping_Views_TreeView$_items_OnGetItemMetaData(item) {
    var group = item;
    if (group.level != null) {
        return Client.DataGrouping.Views.TreeView.getGroupRowMetadata(item);
    }
    else {
        return null;
    }
}
Client.DataGrouping.Views.TreeView.getGroupRowMetadata = function Client_DataGrouping_Views_TreeView$getGroupRowMetadata(item) {
    var metaData = {};
    metaData.selectable = false;
    metaData.focusable = true;
    metaData.cssClasses = 'slick-group';
    metaData.columns = [];
    var col = {};
    metaData.columns.add(col);
    col.colspan = '*';
    col.formatter = Client.DataGrouping.Views.TreeView.groupCellFormatter;
    col.editor = null;
    return metaData;
}
Client.DataGrouping.Views.TreeView.groupCellFormatter = function Client_DataGrouping_Views_TreeView$groupCellFormatter(row, cell, value, columnDef, dataContext) {
    var item = dataContext;
    var indentation = (item.level * 15).toString() + 'px';
    return "<span class='slick-group-toggle " + ((item.collapsed) ? 'collapsed' : 'expanded') + "' style='margin-left:" + indentation + "'>" + '</span>' + "<span class='slick-group-title' level='" + item.level + "'>" + item.title + '</span>';
}


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
    salesrepid: null,
    
    _quoteProduct_PropertyChanged$1: function Client_QuoteLineItemEditor_Model_QuoteDetail$_quoteProduct_PropertyChanged$1(sender, e) {
        if (this.quantity != null && this.priceperunit != null) {
            this.extendedamount = new Xrm.Sdk.Money(this.quantity * this.priceperunit.value);
        }
        this.isproductoverridden = !String.isNullOrEmpty(this.productdescription);
    }
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
    _isSetting: false,
    innerQuoteDetail: null,
    
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


Type.registerNamespace('Client.ScheduledJobsEditor.Model');

////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.Model.BulkDeleteOperation

Client.ScheduledJobsEditor.Model.BulkDeleteOperation = function Client_ScheduledJobsEditor_Model_BulkDeleteOperation() {
    Client.ScheduledJobsEditor.Model.BulkDeleteOperation.initializeBase(this, [ 'bulkdeleteoperation' ]);
}
Client.ScheduledJobsEditor.Model.BulkDeleteOperation.prototype = {
    name: null,
    bulkdeleteoperationid: null,
    asyncoperationid: null,
    asyncoperation_statecode: null,
    asyncoperation_statuscode: null,
    asyncoperation_postponeuntil: null
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.Model.dev1_ScheduledJob

Client.ScheduledJobsEditor.Model.dev1_ScheduledJob = function Client_ScheduledJobsEditor_Model_dev1_ScheduledJob() {
    Client.ScheduledJobsEditor.Model.dev1_ScheduledJob.initializeBase(this, [ 'dev1_scheduledjob' ]);
}
Client.ScheduledJobsEditor.Model.dev1_ScheduledJob.prototype = {
    dev1_scheduledjobid: null,
    dev1_name: null,
    dev1_recurrancepattern: null,
    createdon: null,
    dev1_workflowname: null,
    dev1_startdate: null
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.Model.asyncoperation

Client.ScheduledJobsEditor.Model.asyncoperation = function Client_ScheduledJobsEditor_Model_asyncoperation() {
    Client.ScheduledJobsEditor.Model.asyncoperation.initializeBase(this, [ 'asyncoperation' ]);
}
Client.ScheduledJobsEditor.Model.asyncoperation.prototype = {
    asyncoperationid: null,
    name: null,
    recurrencepattern: null,
    postponeuntil: null,
    recurrencestarttime: null,
    bulkdeleteoperationid: null,
    data: null,
    statecode: null,
    statuscode: null
}


Type.registerNamespace('Client.ScheduledJobsEditor.ViewModels');

////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.ViewModels.ScheduledJob

Client.ScheduledJobsEditor.ViewModels.ScheduledJob = function Client_ScheduledJobsEditor_ViewModels_ScheduledJob() {
    this.scheduledJobId = ko.observable();
    this.name = ko.observable();
    this.startDate = ko.observable();
    this.startTime = ko.observable();
    this.endTime = ko.observable();
    this.recurrance = ko.observable();
    this.data = ko.observable();
    this.recurEvery = ko.observable(0);
    this.sunday = ko.observable(false);
    this.monday = ko.observable(false);
    this.tuesday = ko.observable(false);
    this.wednesday = ko.observable(false);
    this.thursday = ko.observable(false);
    this.friday = ko.observable(false);
    this.saturday = ko.observable(false);
    this.count = ko.observable();
    this.noEndDate = ko.observable(true);
    this.recurrancePattern = ko.observable();
    this.workflowId = ko.observable();
    this.addValidation();
}
Client.ScheduledJobsEditor.ViewModels.ScheduledJob.prototype = {
    
    reset: function Client_ScheduledJobsEditor_ViewModels_ScheduledJob$reset() {
        this.scheduledJobId(null);
        this.name('');
        this.workflowId(null);
        this.monday(false);
        this.tuesday(false);
        this.wednesday(false);
        this.thursday(false);
        this.friday(false);
        this.saturday(false);
        this.sunday(false);
        this.startDate(Date.get_now());
        this.recurEvery(1);
        this.recurrance(Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.get_recurranceFrequencies()[0]);
    },
    
    addValidation: function Client_ScheduledJobsEditor_ViewModels_ScheduledJob$addValidation() {
        var that = this;
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the name of the scheduled job').register(this.name);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the name of the workflow to run').register(this.workflowId);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the start date of the scheduled').register(this.startDate);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the recurrance frequency of the job').register(this.recurrance);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the interval of the recurrance.').addRule('Interval must be greater than zero', function(value) {
            if (!String.isNullOrEmpty(value.toString())) {
                return (parseInt(value.toString()) > 0);
            }
            else {
                return true;
            }
        }).register(this.recurEvery);
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel

Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel = function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel() {
    this.jobsViewModel = new SparkleXrm.GridEditor.EntityDataViewModel(2, Xrm.Sdk.Entity, true);
    this.bulkDeleteJobsViewModel = new SparkleXrm.GridEditor.EntityDataViewModel(10, Xrm.Sdk.Entity, true);
    this.recurranceFrequencies = Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.get_recurranceFrequencies();
    this.selectedJob = ko.validatedObservable(new Client.ScheduledJobsEditor.ViewModels.ScheduledJob());
    Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel.initializeBase(this);
    var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>\r\n                              <entity name='dev1_scheduledjob'>\r\n                                <attribute name='dev1_scheduledjobid' />\r\n                                <attribute name='dev1_name' />\r\n                                <attribute name='createdon' />\r\n                                <attribute name='dev1_workflowname' />\r\n                                <attribute name='dev1_recurrancepattern' />\r\n                                <attribute name='dev1_startdate' />\r\n                                <attribute name='dev1_enabled' />\r\n                                {3}\r\n                              </entity>\r\n                            </fetch>";
    this.jobsViewModel.set_fetchXml(fetchXml);
    this.jobsViewModel.add_onSelectedRowsChanged(ss.Delegate.create(this, this._jobsViewModel_OnSelectedRowsChanged$1));
}
Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel.prototype = {
    
    _jobsViewModel_OnSelectedRowsChanged$1: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$_jobsViewModel_OnSelectedRowsChanged$1() {
        var selectedRows = this.jobsViewModel.getSelectedRows();
        if (selectedRows.length > 0) {
            var job = this.selectedJob();
            var item = this.jobsViewModel.getItem(selectedRows[0].fromRow);
            job.recurrancePattern(item.dev1_recurrancepattern);
            Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.deSerialise(job, item.dev1_recurrancepattern);
            job.scheduledJobId(item.dev1_scheduledjobid);
            job.name(item.dev1_name);
            job.startDate(item.dev1_startdate);
            job.recurrancePattern(item.dev1_recurrancepattern);
            var entityName = new Xrm.Sdk.EntityReference(null, null, item.dev1_workflowname);
            job.workflowId(entityName);
            var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>\r\n                            <entity name='bulkdeleteoperation'>\r\n                            <attribute name='name' />\r\n                            <attribute name='createdon' />\r\n                            <attribute name='asyncoperationid' />\r\n                            <filter type='and'>\r\n                            <condition attribute='name' operator='like' value='%" + item.dev1_scheduledjobid.value + "%' />\r\n                            </filter>\r\n                            <link-entity name='asyncoperation' to='asyncoperationid' from='asyncoperationid' link-type='inner' alias='a0'>\r\n                            <attribute name='postponeuntil' alias='asyncoperation_postponeuntil' />\r\n                            <attribute name='statecode' alias='asyncoperation_statecode' />\r\n                            <attribute name='statuscode'  alias='asyncoperation_statuscode' />\r\n                            <attribute name='recurrencepattern'  alias='asyncoperation_recurrencepattern' />\r\n                            </link-entity>{3}\r\n                            </entity>\r\n                            </fetch>";
            this.bulkDeleteJobsViewModel.set_fetchXml(fetchXml);
            this.bulkDeleteJobsViewModel.reset();
            this.bulkDeleteJobsViewModel.refresh();
        }
    },
    
    workflowSearchCommand: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$workflowSearchCommand(term, callback) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='workflow'>\r\n                                <attribute name='workflowid' />\r\n                                <attribute name='name' />\r\n                                <order attribute='modifiedon' descending='false' />\r\n                                <filter type='and'>\r\n                                    <condition attribute='name' operator='like' value='%{0}%' />\r\n                                    <condition attribute='type' value='1' operator='eq'/>\r\n                                    <condition attribute='category' value='0' operator='eq'/>\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    _deleteCommand$1: null,
    
    deleteCommand: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$deleteCommand() {
        if (this._deleteCommand$1 == null) {
            this._deleteCommand$1 = ss.Delegate.create(this, function() {
                var ranges = this.jobsViewModel.getSelectedRows();
                var rows = SparkleXrm.GridEditor.DataViewBase.rangesToRows(ranges);
                var confirmed = confirm(String.format('Are you sure you want to delete these {0} job(s)?', rows.length));
                if (!confirmed) {
                    return;
                }
                this.isBusy(true);
                Xrm.DelegateItterator.callbackItterate(ss.Delegate.create(this, function(index, nextCallback, errorCallBack) {
                    var job = this.jobsViewModel.getItem(rows[index]);
                    this._deleteBulkDeleteJobs$1(job.dev1_scheduledjobid, ss.Delegate.create(this, function() {
                        Xrm.Sdk.OrganizationServiceProxy.beginDelete(Client.ScheduledJobsEditor.Model.dev1_ScheduledJob.entityLogicalName, job.dev1_scheduledjobid, ss.Delegate.create(this, function(result) {
                            try {
                                Xrm.Sdk.OrganizationServiceProxy.endDelete(result);
                                this.isBusyProgress((index / rows.length) * 100);
                                nextCallback();
                            }
                            catch (ex) {
                                errorCallBack(ex);
                            }
                        }));
                    }));
                }), rows.length, ss.Delegate.create(this, function() {
                    this.isBusy(false);
                    this.jobsViewModel.reset();
                    this.jobsViewModel.refresh();
                }), ss.Delegate.create(this, function(ex) {
                    this._reportError$1(ex);
                }));
            });
        }
        return this._deleteCommand$1;
    },
    
    _newCommand$1: null,
    
    newCommand: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$newCommand() {
        if (this._newCommand$1 == null) {
            this._newCommand$1 = ss.Delegate.create(this, function() {
                var rows = new Array(0);
                this.jobsViewModel.raiseOnSelectedRowsChanged(rows);
                var job = this.selectedJob();
                job.reset();
            });
        }
        return this._newCommand$1;
    },
    
    _saveCommand$1: null,
    
    saveCommand: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$saveCommand() {
        if (this._saveCommand$1 == null) {
            this._saveCommand$1 = ss.Delegate.create(this, function() {
                if (!(this.selectedJob).isValid()) {
                    var validationResult = ko.validation.group(this.selectedJob());
                    validationResult.showAllMessages(true);
                    return;
                }
                var confirmed = confirm(String.format('Are you sure you want to save this schedule?'));
                if (!confirmed) {
                    return;
                }
                this.isBusy(true);
                this.isBusyProgress(0);
                this.isBusyMessage('Saving...');
                var jobToSave = new Client.ScheduledJobsEditor.Model.dev1_ScheduledJob();
                var job = this.selectedJob();
                jobToSave.dev1_name = job.name();
                jobToSave.dev1_startdate = job.startDate();
                jobToSave.dev1_workflowname = job.workflowId().name;
                jobToSave.dev1_recurrancepattern = Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.serialise(job);
                if (job.scheduledJobId() == null) {
                    Xrm.Sdk.OrganizationServiceProxy.beginCreate(jobToSave, ss.Delegate.create(this, function(createJobResponse) {
                        try {
                            job.scheduledJobId(Xrm.Sdk.OrganizationServiceProxy.endCreate(createJobResponse));
                            this._createBulkDeleteJobs$1(job);
                        }
                        catch (ex) {
                            this._reportError$1(ex);
                        }
                    }));
                }
                else {
                    jobToSave.dev1_scheduledjobid = job.scheduledJobId();
                    Xrm.Sdk.OrganizationServiceProxy.beginUpdate(jobToSave, ss.Delegate.create(this, function(createJobResponse) {
                        try {
                            Xrm.Sdk.OrganizationServiceProxy.endUpdate(createJobResponse);
                            this._deleteBulkDeleteJobs$1(job.scheduledJobId(), ss.Delegate.create(this, function() {
                                this._createBulkDeleteJobs$1(job);
                            }));
                        }
                        catch (ex) {
                            this._reportError$1(ex);
                        }
                    }));
                }
            });
        }
        return this._saveCommand$1;
    },
    
    _deleteBulkDeleteJobs$1: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$_deleteBulkDeleteJobs$1(scheduledJobId, callback) {
        this.isBusyMessage('Deleting existing schedule...');
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" + "<entity name='bulkdeleteoperation'>" + "<attribute name='name' />" + "<attribute name='asyncoperationid' />" + "<link-entity name='asyncoperation' alias='a0' to='asyncoperationid' from='asyncoperationid'>" + "<attribute name='statecode' alias='asyncoperation_statecode'/>" + "<attribute name='statuscode'  alias='asyncoperation_statuscode'/>" + '</link-entity>' + "<filter type='and'>" + "<condition attribute='name' operator='like' value='%" + scheduledJobId.toString() + "%' />" + '</filter>' + '</entity>' + '</fetch>';
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, ss.Delegate.create(this, function(fetchJobsResponse) {
            try {
                var jobs = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(fetchJobsResponse, Client.ScheduledJobsEditor.Model.BulkDeleteOperation);
                var deleteItems = [];
                this.isBusyProgress(0);
                var $enum1 = ss.IEnumerator.getEnumerator(jobs.get_entities());
                while ($enum1.moveNext()) {
                    var item = $enum1.current;
                    var deleteJobOperationRequest = new Client.ScheduledJobsEditor.ViewModels.PendingDelete();
                    deleteJobOperationRequest.entityName = Client.ScheduledJobsEditor.Model.BulkDeleteOperation.entityLogicalName;
                    deleteJobOperationRequest.id = item.bulkdeleteoperationid;
                    Xrm.ArrayEx.add(deleteItems, deleteJobOperationRequest);
                    var deleteAsyncOperationRequest = new Client.ScheduledJobsEditor.ViewModels.PendingDelete();
                    deleteAsyncOperationRequest.entityName = Client.ScheduledJobsEditor.Model.asyncoperation.entityLogicalName;
                    deleteAsyncOperationRequest.id = item.asyncoperationid.id;
                    deleteAsyncOperationRequest.cancelFirst = (item.asyncoperation_statecode.value === 1);
                    Xrm.ArrayEx.add(deleteItems, deleteAsyncOperationRequest);
                }
                this._deleteJob$1(deleteItems, callback);
            }
            catch (ex) {
                this._reportError$1(ex);
            }
        }));
    },
    
    _deleteJob$1: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$_deleteJob$1(items, completedCallback) {
        Xrm.DelegateItterator.callbackItterate(ss.Delegate.create(this, function(index, nextCallback, errorCallBack) {
            var pendingDeleteItem = items[index];
            if (pendingDeleteItem.cancelFirst) {
                var operationToUpdate = new Client.ScheduledJobsEditor.Model.asyncoperation();
                operationToUpdate.asyncoperationid = pendingDeleteItem.id;
                operationToUpdate.id = operationToUpdate.asyncoperationid.value;
                operationToUpdate.statecode = new Xrm.Sdk.OptionSetValue(3);
                Xrm.Sdk.OrganizationServiceProxy.update(operationToUpdate);
            }
            Xrm.Sdk.OrganizationServiceProxy.beginDelete(pendingDeleteItem.entityName, pendingDeleteItem.id, ss.Delegate.create(this, function(result) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endDelete(result);
                    this.isBusyProgress((index / items.length) * 100);
                    nextCallback();
                }
                catch (ex) {
                    errorCallBack(ex);
                }
            }));
        }), items.length, completedCallback, ss.Delegate.create(this, function(ex) {
            this._reportError$1(ex);
        }));
    },
    
    _createBulkDeleteJobs$1: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$_createBulkDeleteJobs$1(job) {
        this.isBusyMessage('Creating new schedule...');
        this.isBusyProgress(0);
        var fetchxml = "<fetch distinct='false' no-lock='false' mapping='logical'><entity name='lead'><attribute name='fullname' /><attribute name='statuscode' /><attribute name='createdon' /><attribute name='subject' /><attribute name='leadid' /><filter type='and'><condition attribute='ownerid' operator='eq-userid' /><condition attribute='statecode' operator='eq' value='0' /><condition attribute='address1_county' operator='eq' value='deleteme' /></filter><order attribute='createdon' descending='true' /></entity></fetch>";
        var convertRequest = new Xrm.Sdk.Messages.FetchXmlToQueryExpressionRequest();
        convertRequest.fetchXml = fetchxml;
        Xrm.Sdk.OrganizationServiceProxy.beginExecute(convertRequest, ss.Delegate.create(this, function(state) {
            var response = Xrm.Sdk.OrganizationServiceProxy.endExecute(state);
            var bulkDeleteRequests = [];
            if (job.recurrance().value !== 'DAILY') {
                var startDate = Xrm.Sdk.DateTimeEx.utcToLocalTimeFromSettings(job.startDate(), Xrm.Sdk.OrganizationServiceProxy.getUserSettings());
                var interval = 'days';
                var incrementAmount = 1;
                var dayInterval = 1;
                var recurranceCount = 0;
                var totalCount = 0;
                var freq = 'DAILY';
                switch (job.recurrance().value) {
                    case 'MINUTELY':
                        interval = 'minutes';
                        incrementAmount = job.recurEvery();
                        dayInterval = 1;
                        recurranceCount = (60 * 24) / incrementAmount;
                        break;
                    case 'HOURLY':
                        interval = 'hours';
                        incrementAmount = job.recurEvery();
                        dayInterval = 1;
                        recurranceCount = 24 / incrementAmount;
                        break;
                    case 'WEEKLY':
                    case 'YEARLY':
                        throw new Error('The selected schedule interval is currently not supported due to the limitation of bulk delete');
                }
                if (incrementAmount < 0) {
                    throw new Error('Invalid schedule');
                }
                for (var i = 0; i < recurranceCount; i++) {
                    var request = new Xrm.Sdk.Messages.BulkDeleteRequest();
                    request.querySet = response.query.replaceAll('<d:anyType ', '<d:anyType xmlns:e="http://www.w3.org/2001/XMLSchema" ');
                    request.sendEmailNotification = false;
                    request.startDateTime = startDate;
                    request.recurrencePattern = 'FREQ=DAILY;INTERVAL=' + dayInterval.toString();
                    request.jobName = 'Scheduled Job ' + i.format('0000') + ' ' + job.scheduledJobId().value;
                    Xrm.ArrayEx.add(bulkDeleteRequests, request);
                    startDate = Xrm.Sdk.DateTimeEx.dateAdd(interval, incrementAmount, startDate);
                }
            }
            else {
                var request = new Xrm.Sdk.Messages.BulkDeleteRequest();
                request.querySet = response.query.replaceAll('<d:anyType ', '<d:anyType xmlns:e="http://www.w3.org/2001/XMLSchema" ');
                request.sendEmailNotification = false;
                request.startDateTime = job.startDate();
                request.recurrencePattern = Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.serialise(job);
                request.jobName = 'Scheduled Job ' + job.scheduledJobId().value;
                Xrm.ArrayEx.add(bulkDeleteRequests, request);
            }
            this._batchCreateBulkDeleteJobs$1(bulkDeleteRequests, ss.Delegate.create(this, function() {
                this.isBusy(false);
                this.jobsViewModel.reset();
                this.jobsViewModel.refresh();
            }));
        }));
    },
    
    _batchCreateBulkDeleteJobs$1: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$_batchCreateBulkDeleteJobs$1(items, completedCallback) {
        Xrm.DelegateItterator.callbackItterate(ss.Delegate.create(this, function(index, nextCallback, errorCallBack) {
            var pendingDeleteItem = items[index];
            Xrm.Sdk.OrganizationServiceProxy.beginExecute(pendingDeleteItem, ss.Delegate.create(this, function(result) {
                try {
                    Xrm.Sdk.OrganizationServiceProxy.endExecute(result);
                    this.isBusyProgress((index / items.length) * 100);
                    nextCallback();
                }
                catch (ex) {
                    errorCallBack(ex);
                }
            }));
        }), items.length, completedCallback, ss.Delegate.create(this, function(ex) {
            this._reportError$1(ex);
        }));
    },
    
    _reportError$1: function Client_ScheduledJobsEditor_ViewModels_ScheduledJobsEditorViewModel$_reportError$1(ex) {
        debugger;
        alert('There was a problem saving. Please contact your system administrator.\n\n' + ex.message);
        this.isBusy(false);
        this.jobsViewModel.reset();
        this.jobsViewModel.refresh();
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.ViewModels.PendingDelete

Client.ScheduledJobsEditor.ViewModels.PendingDelete = function Client_ScheduledJobsEditor_ViewModels_PendingDelete() {
}
Client.ScheduledJobsEditor.ViewModels.PendingDelete.prototype = {
    entityName: null,
    id: null,
    cancelFirst: false
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames

Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames = function Client_ScheduledJobsEditor_ViewModels_RecurranceFrequencyNames() {
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.ViewModels.DayNames

Client.ScheduledJobsEditor.ViewModels.DayNames = function Client_ScheduledJobsEditor_ViewModels_DayNames() {
}


////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper

Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper = function Client_ScheduledJobsEditor_ViewModels_RecurrancePatternMapper() {
}
Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.get_recurranceFrequencies = function Client_ScheduledJobsEditor_ViewModels_RecurrancePatternMapper$get_recurranceFrequencies() {
    if (Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper._frequencies == null) {
        var values = new Array(5);
        values[0] = {};
        values[0].value = 'YEARLY';
        values[0].name = 'Yearly';
        values[1] = {};
        values[1].value = 'WEEKLY';
        values[1].name = 'Week';
        values[2] = {};
        values[2].value = 'DAILY';
        values[2].name = 'Day';
        values[3] = {};
        values[3].value = 'HOURLY';
        values[3].name = 'Hour';
        values[4] = {};
        values[4].value = 'MINUTELY';
        values[4].name = 'Minute';
        Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper._frequencies = values;
    }
    return Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper._frequencies;
}
Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.serialise = function Client_ScheduledJobsEditor_ViewModels_RecurrancePatternMapper$serialise(scheduledJob) {
    var parts = [];
    Xrm.ArrayEx.add(parts, 'FREQ=' + scheduledJob.recurrance().value);
    Xrm.ArrayEx.add(parts, 'INTERVAL=' + scheduledJob.recurEvery().toString());
    if (scheduledJob.recurrance().value === 'WEEKLY') {
        var days = [];
        if (scheduledJob.monday()) {
            days.add('MO');
        }
        if (scheduledJob.tuesday()) {
            days.add('TU');
        }
        if (scheduledJob.wednesday()) {
            days.add('WE');
        }
        if (scheduledJob.thursday()) {
            days.add('TH');
        }
        if (scheduledJob.friday()) {
            days.add('FR');
        }
        if (scheduledJob.saturday()) {
            days.add('SA');
        }
        if (scheduledJob.sunday()) {
            days.add('SU');
        }
        if (days.length > 0) {
            Xrm.ArrayEx.add(parts, 'BYDAY=' + days.join(','));
        }
    }
    if (!scheduledJob.noEndDate()) {
        Xrm.ArrayEx.add(parts, 'COUNT=' + scheduledJob.count());
    }
    var pattern = parts.join(';');
    return pattern;
}
Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.deSerialise = function Client_ScheduledJobsEditor_ViewModels_RecurrancePatternMapper$deSerialise(scheduledJob, patternString) {
    var pattern = patternString.split(';');
    var mon = false, tue = false, wed = false, thu = false, fri = false, sat = false, sun = false;
    var interval = 1;
    var count = null;
    var frequency = null;
    var $enum1 = ss.IEnumerator.getEnumerator(pattern);
    while ($enum1.moveNext()) {
        var part = $enum1.current;
        var value = part.split('=');
        switch (value[0]) {
            case 'FREQ':
                for (var i = 0; i < Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.get_recurranceFrequencies().length; i++) {
                    if (value[1] === Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.get_recurranceFrequencies()[i].value) {
                        frequency = Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.get_recurranceFrequencies()[i];
                        break;
                    }
                }
                break;
            case 'COUNT':
                count = parseInt(value[1]);
                break;
            case 'INTERVAL':
                interval = parseInt(value[1]);
                break;
            case 'BYDAY':
                var days = value[1].split(',');
                var $enum2 = ss.IEnumerator.getEnumerator(days);
                while ($enum2.moveNext()) {
                    var day = $enum2.current;
                    switch (day) {
                        case 'MO':
                            mon = true;
                            break;
                        case 'TU':
                            tue = true;
                            break;
                        case 'WE':
                            wed = true;
                            break;
                        case 'TH':
                            thu = true;
                            break;
                        case 'FR':
                            fri = true;
                            break;
                        case 'SA':
                            sat = true;
                            break;
                        case 'SU':
                            sun = true;
                            break;
                    }
                }
                break;
        }
    }
    scheduledJob.recurrance(frequency);
    scheduledJob.monday(mon);
    scheduledJob.tuesday(tue);
    scheduledJob.wednesday(wed);
    scheduledJob.thursday(thu);
    scheduledJob.friday(fri);
    scheduledJob.saturday(sat);
    scheduledJob.sunday(sun);
    scheduledJob.recurEvery(interval);
    scheduledJob.count(count);
    scheduledJob.noEndDate(count == null);
}


Type.registerNamespace('Client.ScheduledJobsEditor.Views');

////////////////////////////////////////////////////////////////////////////////
// Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView

Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView = function Client_ScheduledJobsEditor_Views_ScheduledJobsEditorView() {
    Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.initializeBase(this);
}
Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.init = function Client_ScheduledJobsEditor_Views_ScheduledJobsEditorView$init() {
    $(function() {
        ko.validation.registerExtenders();
        Xrm.Sdk.OrganizationServiceProxy.getUserSettings();
        var vm = new Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel();
        Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.setUpGrids(vm);
        SparkleXrm.ViewBase.registerViewModel(vm);
    });
}
Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.setUpGrids = function Client_ScheduledJobsEditor_Views_ScheduledJobsEditorView$setUpGrids(vm) {
    var jobsDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var jobCols = SparkleXrm.GridEditor.GridDataViewBinder.parseLayout('dev1_name,Name,300,dev1_recurrancepattern,Pattern,300,createdon,Created On, 300');
    Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.jobsGrid = jobsDataBinder.dataBindXrmGrid(vm.jobsViewModel, jobCols, 'jobsGrid', 'jobsGridPager', false, false);
    var bulkDeleteDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var bulkDeleteCols = SparkleXrm.GridEditor.GridDataViewBinder.parseLayout('name,Name,300,asyncoperation_statuscode,Status,100,asyncoperation_postponeuntil,Next Run,150,asyncoperation_recurrencepattern,Pattern,150,createdon,Created On,150');
    Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.bulkDeleteJobsGrid = bulkDeleteDataBinder.dataBindXrmGrid(vm.bulkDeleteJobsViewModel, bulkDeleteCols, 'bulkDeleteJobGrid', 'bulkDeleteJobGridPager', false, false);
    vm.jobsViewModel.refresh();
}


Type.registerNamespace('Client.ContactEditor.Model');

////////////////////////////////////////////////////////////////////////////////
// Client.ContactEditor.Model.Contact

Client.ContactEditor.Model.Contact = function Client_ContactEditor_Model_Contact() {
    Client.ContactEditor.Model.Contact.initializeBase(this, [ 'contact' ]);
    this._metaData['numberofchildren'] = Xrm.Sdk.AttributeTypes.int_;
    this._metaData['creditlimit'] = Xrm.Sdk.AttributeTypes.money;
}
Client.ContactEditor.Model.Contact.prototype = {
    contactid: null,
    fullname: null,
    firstname: null,
    lastname: null,
    birthdate: null,
    accountrolecode: null,
    numberofchildren: null,
    transactioncurrencyid: null,
    creditlimit: null
}


Type.registerNamespace('Client.Views');

////////////////////////////////////////////////////////////////////////////////
// Client.Views.ContactEditorView

Client.Views.ContactEditorView = function Client_Views_ContactEditorView() {
    Client.Views.ContactEditorView.initializeBase(this);
}
Client.Views.ContactEditorView.init = function Client_Views_ContactEditorView$init() {
    var vm = new Client.ContactEditor.ViewModels.ContactsEditorViewModel();
    var columns = SparkleXrm.GridEditor.GridDataViewBinder.parseLayout(',entityState,20,First Name,firstname,200,Last Name,lastname,200,Birth Date,birthdate,200,Account Role Code,accountrolecode,200,Number of Children,numberofchildren,100,Currency,transactioncurrencyid,200,Credit Limit,creditlimit,100');
    columns[0].formatter = function(row, cell, value, columnDef, dataContext) {
        var state = value;
        return ((state === Xrm.Sdk.EntityStates.changed) || (state === Xrm.Sdk.EntityStates.created)) ? "<span class='grid-edit-indicator'></span>" : '';
    };
    SparkleXrm.GridEditor.XrmTextEditor.bindColumn(columns[1]);
    SparkleXrm.GridEditor.XrmTextEditor.bindColumn(columns[2]);
    SparkleXrm.GridEditor.XrmDateEditor.bindColumn(columns[3], false);
    SparkleXrm.GridEditor.XrmOptionSetEditor.bindColumn(columns[4], 'contact', columns[4].field, true);
    SparkleXrm.GridEditor.XrmNumberEditor.bindColumn(columns[5], 0, 100, 0);
    SparkleXrm.GridEditor.XrmLookupEditor.bindColumn(columns[6], ss.Delegate.create(vm, vm.transactionCurrencySearchCommand), 'transactioncurrencyid', 'currencyname', '');
    SparkleXrm.GridEditor.XrmMoneyEditor.bindColumn(columns[7], -10000, 10000);
    var contactGridDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    var contactsGrid = contactGridDataBinder.dataBindXrmGrid(vm.contacts, columns, 'container', 'pager', true, false);
    SparkleXrm.ViewBase.registerViewModel(vm);
    window.setTimeout(function() {
        vm.init();
    }, 0);
}


Type.registerNamespace('Client.TimeSheet.Model');

////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.Model.Queries

Client.TimeSheet.Model.Queries = function Client_TimeSheet_Model_Queries() {
}


Type.registerNamespace('TimeSheet.Client.ViewModel');

////////////////////////////////////////////////////////////////////////////////
// TimeSheet.Client.ViewModel.ObservableActivityPointer

TimeSheet.Client.ViewModel.ObservableActivityPointer = function TimeSheet_Client_ViewModel_ObservableActivityPointer() {
    this.id = ko.observable();
    this.subject = ko.observable();
}


////////////////////////////////////////////////////////////////////////////////
// TimeSheet.Client.ViewModel.SessionVM

TimeSheet.Client.ViewModel.SessionVM = function TimeSheet_Client_ViewModel_SessionVM(vm, data) {
    this.dev1_sessionid = ko.observable();
    this.activity = ko.observable();
    this.dev1_starttime = ko.observable();
    this.dev1_endtime = ko.observable();
    this.dev1_description = ko.observable();
    this.dev1_duration = ko.observable();
    this.startTimeTime = ko.observable();
    this.endTimeTime = ko.observable();
    this.timeSoFar = ko.observable();
    this.test = ko.observable('test');
    if (data != null) {
        this.dev1_sessionid(data.dev1_sessionid);
        this.dev1_description(data.dev1_description);
        this.dev1_duration(data.dev1_duration);
        this.dev1_endtime(data.dev1_endtime);
        this.dev1_starttime(data.dev1_starttime);
        this._getTime(this.dev1_starttime, this.startTimeTime);
        this._getTime(this.dev1_endtime, this.endTimeTime);
        if (data.dev1_letterid != null) {
            this.activity(data.dev1_letterid);
        }
        else if (data.dev1_taskid != null) {
            this.activity(data.dev1_taskid);
        }
    }
    this.dev1_endtime.subscribe(ss.Delegate.create(this, function(value) {
        this.onStartEndDateChanged();
    }));
    this.dev1_starttime.subscribe(ss.Delegate.create(this, function(value) {
        this.onStartEndDateChanged();
    }));
    this.startTimeTime.subscribe(ss.Delegate.create(this, function(value) {
        this.onStartEndDateChanged();
    }));
    this.endTimeTime.subscribe(ss.Delegate.create(this, function(value) {
        this.onStartEndDateChanged();
    }));
    this.dev1_duration.subscribe(ss.Delegate.create(this, function(value) {
        this.onDurationChanged();
    }));
    this.addValidation();
}
TimeSheet.Client.ViewModel.SessionVM.calculateDuration = function TimeSheet_Client_ViewModel_SessionVM$calculateDuration(startTime, endTime) {
    var duration = null;
    if ((startTime != null) && (endTime != null)) {
        if ((startTime != null) && (endTime != null)) {
            duration = (endTime.getTime() - startTime.getTime()) / (1000 * 60);
        }
    }
    return duration;
}
TimeSheet.Client.ViewModel.SessionVM.prototype = {
    _supressEvents: false,
    
    onStartEndDateChanged: function TimeSheet_Client_ViewModel_SessionVM$onStartEndDateChanged() {
        if (this._supressEvents) {
            return;
        }
        this._supressEvents = true;
        var startTime = this.dev1_starttime();
        var endTime = this.dev1_endtime();
        var duration = null;
        startTime = Xrm.Sdk.DateTimeEx.addTimeToDate(startTime, this.startTimeTime());
        endTime = Xrm.Sdk.DateTimeEx.addTimeToDate(endTime, this.endTimeTime());
        if (endTime != null && startTime != null) {
            endTime.setDate(startTime.getDate());
            endTime.setMonth(startTime.getMonth());
            endTime.setFullYear(startTime.getFullYear());
        }
        duration = TimeSheet.Client.ViewModel.SessionVM.calculateDuration(startTime, endTime);
        this.dev1_duration(duration);
        this._supressEvents = false;
    },
    
    onDurationChanged: function TimeSheet_Client_ViewModel_SessionVM$onDurationChanged() {
        if (this._supressEvents) {
            return;
        }
        this._supressEvents = true;
        var startTime = this.dev1_starttime();
        var startTimeTime = this.startTimeTime();
        if ((startTime != null) && (startTimeTime != null)) {
            startTime = Xrm.Sdk.DateTimeEx.addTimeToDate(startTime, startTimeTime);
            var duration = this.dev1_duration();
            var startTimeMilliSeconds = startTime.getTime();
            startTimeMilliSeconds = startTimeMilliSeconds + (duration * 1000 * 60);
            var newEndDate = new Date(startTimeMilliSeconds);
            this.dev1_endtime(newEndDate);
            this._getTime(this.dev1_endtime, this.endTimeTime);
        }
        this._supressEvents = false;
    },
    
    getUpdatedModel: function TimeSheet_Client_ViewModel_SessionVM$getUpdatedModel() {
        var session = new dev1_session();
        this._setTime(this.dev1_starttime, this.startTimeTime);
        this._setTime(this.dev1_endtime, this.endTimeTime);
        var mapping = {};
        mapping.ignore = [ 'dev1_letterid', 'dev1_taskid', 'dev1_emailid' ];
        if (this.dev1_sessionid() != null) {
            session.dev1_sessionid = this.dev1_sessionid();
        }
        session.dev1_description = this.dev1_description();
        session.dev1_starttime = this.dev1_starttime();
        session.dev1_endtime = this.dev1_endtime();
        session.dev1_duration = this.dev1_duration();
        return session;
    },
    
    _getTime: function TimeSheet_Client_ViewModel_SessionVM$_getTime(dateProperty, time) {
        var dateValue = dateProperty();
        if (dateValue != null) {
            var timeFormatted = dateValue.format('h:mm tt');
            time(timeFormatted);
        }
    },
    
    _setTime: function TimeSheet_Client_ViewModel_SessionVM$_setTime(dateProperty, time) {
        var currentDate = dateProperty();
        currentDate = Xrm.Sdk.DateTimeEx.addTimeToDate(currentDate, time());
        dateProperty(currentDate);
    },
    
    addValidation: function TimeSheet_Client_ViewModel_SessionVM$addValidation() {
        var self = this;
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the activity you wish to stop').register(this.activity);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the start date of the session').addRule('Start date must be before the end date', function(value) {
            var isValid = true;
            var endTime = self.dev1_endtime();
            if ((endTime != null) && (value != null)) {
                isValid = endTime > value;
            }
            return isValid;
        }).register(this.dev1_starttime);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the end time of the session').register(this.dev1_endtime);
        SparkleXrm.ValidationRules.createRules().addRequiredMsg('Enter the duration of the session').register(this.dev1_duration);
    }
}


////////////////////////////////////////////////////////////////////////////////
// TimeSheet.Client.ViewModel.StartStopSessionViewModel

TimeSheet.Client.ViewModel.StartStopSessionViewModel = function TimeSheet_Client_ViewModel_StartStopSessionViewModel(activityToStartStop, sessionToStartStop) {
    this.openActvities = ko.observableArray();
    this.startNewSession = ko.observable(false);
    TimeSheet.Client.ViewModel.StartStopSessionViewModel.initializeBase(this);
    var newSession = new dev1_session();
    newSession.dev1_starttime = Date.get_now();
    this.startSession = ko.validatedObservable(new TimeSheet.Client.ViewModel.SessionVM(this, newSession));
    var session = Xrm.Sdk.OrganizationServiceProxy.retrieve('dev1_session', '{FD722AC2-B234-E211-A471-000C299FFE7D}', [ 'AllColumns' ]);
    var sessionVM = new TimeSheet.Client.ViewModel.SessionVM(this, session);
    this.stopSession = ko.validatedObservable(sessionVM);
    var isFormValidDependantProperty = {};
    isFormValidDependantProperty.owner = this;
    isFormValidDependantProperty.read = function() {
        var vm = isFormValidDependantProperty.owner;
        if (vm.startNewSession()) {
            return SparkleXrm.ValidationRules.areValid([ (isFormValidDependantProperty.owner).stopSession, (isFormValidDependantProperty.owner).startSession ]);
        }
        else {
            return SparkleXrm.ValidationRules.areValid([ (isFormValidDependantProperty.owner).stopSession ]);
        }
    };
    this.canSave = ko.dependentObservable(isFormValidDependantProperty);
}
TimeSheet.Client.ViewModel.StartStopSessionViewModel.prototype = {
    stopSession: null,
    startSession: null,
    canSave: null,
    
    isFormValid: function TimeSheet_Client_ViewModel_StartStopSessionViewModel$isFormValid() {
        return true;
    },
    
    activitySearchCommand: function TimeSheet_Client_ViewModel_StartStopSessionViewModel$activitySearchCommand(term, callback) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='activitypointer'>\r\n                                <attribute name='activitytypecode' />\r\n                                <attribute name='subject' />\r\n                                <attribute name='activityid' />\r\n                                <attribute name='instancetypecode' />\r\n                                <order attribute='modifiedon' descending='false' />\r\n                                <filter type='and'>\r\n                                  <condition attribute='ownerid' operator='eq-userid' />\r\n                                    <condition attribute='subject' operator='like' value='%{0}%' />\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, Xrm.Sdk.Entity);
            callback(fetchResult);
        });
    },
    
    _saveCommand$1: null,
    
    saveCommand: function TimeSheet_Client_ViewModel_StartStopSessionViewModel$saveCommand() {
        if (this._saveCommand$1 == null) {
            this._saveCommand$1 = ss.Delegate.create(this, function() {
                var confirmed = confirm(String.format('Are you sure save these sessions?'));
                if (!confirmed) {
                    return;
                }
                var stopSession = this.stopSession();
                var sessionToUpdate = stopSession.getUpdatedModel();
                try {
                    Xrm.Sdk.OrganizationServiceProxy.update(sessionToUpdate);
                    if (this.startNewSession()) {
                        var nextSession = this.startSession().getUpdatedModel();
                        Xrm.Sdk.OrganizationServiceProxy.create(nextSession);
                    }
                }
                catch (ex) {
                    alert('There was a problem saving your session. Please contact your system administrator.\n\n' + ex.message);
                }
            });
        }
        return this._saveCommand$1;
    },
    
    _cancelCommand$1: null,
    
    cancelCommand: function TimeSheet_Client_ViewModel_StartStopSessionViewModel$cancelCommand() {
        if (this._cancelCommand$1 == null) {
            this._cancelCommand$1 = function() {
                window.top.close();
            };
        }
        return this._cancelCommand$1;
    }
}


Type.registerNamespace('Client.TimeSheet.ViewModel');

////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.ViewModel.DayEntry

Client.TimeSheet.ViewModel.DayEntry = function Client_TimeSheet_ViewModel_DayEntry() {
    this.hours = new Array(6);
}
Client.TimeSheet.ViewModel.DayEntry.prototype = {
    date: null,
    activity: null,
    activityName: null,
    isTotalRow: false,
    icon: null,
    day0: null,
    day1: null,
    day2: null,
    day3: null,
    day4: null,
    day5: null,
    day6: null,
    
    flatternDays: function Client_TimeSheet_ViewModel_DayEntry$flatternDays() {
        this.day0 = this.hours[0];
        this.day1 = this.hours[1];
        this.day2 = this.hours[2];
        this.day3 = this.hours[3];
        this.day4 = this.hours[4];
        this.day5 = this.hours[5];
        this.day6 = this.hours[6];
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.ViewModel.DaysViewModel

Client.TimeSheet.ViewModel.DaysViewModel = function Client_TimeSheet_ViewModel_DaysViewModel(sessions) {
    this._rows$1 = [];
    Client.TimeSheet.ViewModel.DaysViewModel.initializeBase(this);
    this._sessions$1 = sessions;
    this._sessions$1.onRowsChanged.subscribe(ss.Delegate.create(this, function(args, data) {
        this.reCalculate();
    }));
    this.add_onSelectedRowsChanged(function() {
    });
    sessions.onRowsChanged.subscribe(ss.Delegate.create(this, function(args, data) {
        this.refresh();
    }));
}
Client.TimeSheet.ViewModel.DaysViewModel.prototype = {
    _sessions$1: null,
    _totals$1: null,
    _days$1: null,
    _selectedDay: null,
    
    get_selectedDay: function Client_TimeSheet_ViewModel_DaysViewModel$get_selectedDay() {
        return this._selectedDay;
    },
    set_selectedDay: function Client_TimeSheet_ViewModel_DaysViewModel$set_selectedDay(value) {
        this._selectedDay = null;
        var selectedItems = this.get_selectedItems();
        if (selectedItems != null && selectedItems.length > 0) {
            if (selectedItems[0] != null) {
                this._sessions$1.setCurrentActivity(selectedItems[0].activity, value);
                this._selectedDay = value;
            }
        }
        return value;
    },
    
    get_selectedItems: function Client_TimeSheet_ViewModel_DaysViewModel$get_selectedItems() {
        if (this._selectedRows == null) {
            return new Array(0);
        }
        var items = new Array(this._selectedRows.length);
        var i = 0;
        var $enum1 = ss.IEnumerator.getEnumerator(this._selectedRows);
        while ($enum1.moveNext()) {
            var row = $enum1.current;
            items[i] = this.getItem(row.fromRow);
            i++;
        }
        return items;
    },
    
    getPagingInfo: function Client_TimeSheet_ViewModel_DaysViewModel$getPagingInfo() {
        return null;
    },
    
    raiseOnDateChanged: function Client_TimeSheet_ViewModel_DaysViewModel$raiseOnDateChanged() {
        this.onRowsChanged.notify(null, null, this);
    },
    
    getLength: function Client_TimeSheet_ViewModel_DaysViewModel$getLength() {
        return this._rows$1.length;
    },
    
    getItem: function Client_TimeSheet_ViewModel_DaysViewModel$getItem(index) {
        return this._rows$1[index];
    },
    
    setPagingOptions: function Client_TimeSheet_ViewModel_DaysViewModel$setPagingOptions(p) {
    },
    
    refresh: function Client_TimeSheet_ViewModel_DaysViewModel$refresh() {
        if (this._rows$1 != null) {
            var args = {};
            args.from = 0;
            args.to = this._rows$1.length - 1;
            this.onDataLoaded.notify(args, null, null);
            this.onRowsChanged.notify(null, null, this);
        }
    },
    
    reCalculate: function Client_TimeSheet_ViewModel_DaysViewModel$reCalculate() {
        var sessionData = this._sessions$1.getCurrentWeek();
        var weekStart = this._sessions$1.weekStart;
        this._days$1 = {};
        this._totals$1 = new Client.TimeSheet.ViewModel.DayEntry();
        this._totals$1.isTotalRow = true;
        this._totals$1.activityName = 'Total';
        this._totals$1.activity = new Xrm.Sdk.EntityReference(null, null, null);
        this._totals$1.activity.name = 'Total';
        var $enum1 = ss.IEnumerator.getEnumerator(sessionData);
        while ($enum1.moveNext()) {
            var session = $enum1.current;
            var dayOfWeek = session.dev1_starttime.getDay();
            var activity = session.dev1_activityid;
            if (this._days$1[activity] == null) {
                var day = new Client.TimeSheet.ViewModel.DayEntry();
                this._days$1[activity] = day;
                day.activity = new Xrm.Sdk.EntityReference(new Xrm.Sdk.Guid(session.dev1_activityid), null, null);
                if (session.dev1_taskid != null) {
                    day.activity.name = session.dev1_taskid.name;
                }
                else if (session.dev1_letterid != null) {
                    day.activity.name = session.dev1_letterid.name;
                }
                else if (session.dev1_emailid != null) {
                    day.activity.name = session.dev1_emailid.name;
                }
                else if (session.dev1_phonecallid != null) {
                    day.activity.name = session.dev1_phonecallid.name;
                }
                day.activity.logicalName = session.dev1_activitytypename;
            }
            if (session.dev1_duration != null) {
                if (this._days$1[activity].hours[dayOfWeek] == null) {
                    this._days$1[activity].hours[dayOfWeek] = 0;
                }
                this._days$1[activity].hours[dayOfWeek] = this._days$1[activity].hours[dayOfWeek] + session.dev1_duration;
                if (this._totals$1.hours[dayOfWeek] == null) {
                    this._totals$1.hours[dayOfWeek] = 0;
                }
                this._totals$1.hours[dayOfWeek] = this._totals$1.hours[dayOfWeek] + session.dev1_duration;
            }
        }
        this._rows$1.clear();
        this._rows$1.add(this._totals$1);
        var $dict2 = this._days$1;
        for (var $key3 in $dict2) {
            var day = { key: $key3, value: $dict2[$key3] };
            day.value.flatternDays();
            this._rows$1.add(day.value);
        }
        this._totals$1.flatternDays();
        this.refresh();
    },
    
    addItem: function Client_TimeSheet_ViewModel_DaysViewModel$addItem(item) {
        var session = new dev1_session();
        var activity = item;
        if ((activity.activity != null) && (activity.activity.id != null)) {
            session.dev1_activityid = activity.activity.id.toString();
            session.dev1_activitytypename = activity.activity.logicalName;
            session.dev1_activityid = activity.activity.id.toString();
            session.dev1_starttime = this._sessions$1.weekStart;
            this._sessions$1.selectedActivity = activity.activity;
            this._sessions$1.addItem(session);
            this._selectedRows = [ {} ];
            this._selectedRows[0].fromRow = this._rows$1.length + 1;
            this._selectedRows[0].toRow = this._rows$1.length + 1;
            this.refresh();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.ViewModel.SessionsViewModel

Client.TimeSheet.ViewModel.SessionsViewModel = function Client_TimeSheet_ViewModel_SessionsViewModel() {
    this._weeks$1 = {};
    this._data$1 = [];
    Client.TimeSheet.ViewModel.SessionsViewModel.initializeBase(this);
}
Client.TimeSheet.ViewModel.SessionsViewModel._onDurationChanged$1 = function Client_TimeSheet_ViewModel_SessionsViewModel$_onDurationChanged$1(sender) {
    var session = sender;
    var startTime = session.dev1_starttime;
    if (startTime != null) {
        var duration = session.dev1_duration;
        var startTimeMilliSeconds = startTime.getTime();
        startTimeMilliSeconds = startTimeMilliSeconds + (duration * 1000 * 60);
        var newEndDate = new Date(startTimeMilliSeconds);
        session.dev1_endtime = newEndDate;
    }
}
Client.TimeSheet.ViewModel.SessionsViewModel._onStartEndDateChanged$1 = function Client_TimeSheet_ViewModel_SessionsViewModel$_onStartEndDateChanged$1(sender) {
    var session = sender;
    if (session.dev1_starttime != null && session.dev1_endtime != null) {
        session.dev1_endtime.setDate(session.dev1_starttime.getUTCDate());
        session.dev1_endtime.setMonth(session.dev1_starttime.getUTCMonth());
        session.dev1_endtime.setFullYear(session.dev1_starttime.getUTCFullYear());
    }
    session.dev1_duration = TimeSheet.Client.ViewModel.SessionVM.calculateDuration(session.dev1_starttime, session.dev1_endtime);
}
Client.TimeSheet.ViewModel.SessionsViewModel.prototype = {
    _selectedDate$1: null,
    weekStart: null,
    weekEnd: null,
    selectedDay: null,
    selectedActivityID: null,
    selectedActivity: null,
    
    getCurrentWeek: function Client_TimeSheet_ViewModel_SessionsViewModel$getCurrentWeek() {
        return this._weeks$1[this.weekStart.getTime()];
    },
    
    setCurrentWeek: function Client_TimeSheet_ViewModel_SessionsViewModel$setCurrentWeek(date) {
        this._selectedDate$1 = date;
        this.weekStart = Xrm.Sdk.DateTimeEx.firstDayOfWeek(date);
        this.weekEnd = Xrm.Sdk.DateTimeEx.lastDayOfWeek(date);
        this.refresh();
    },
    
    setCurrentActivity: function Client_TimeSheet_ViewModel_SessionsViewModel$setCurrentActivity(entityReference, day) {
        if (day > 0) {
            this.selectedDay = Xrm.Sdk.DateTimeEx.dateAdd('days', day - 1, this.weekStart);
        }
        else {
            this.selectedDay = null;
        }
        this.selectedActivity = entityReference;
        if (entityReference != null && entityReference.id != null) {
            this.selectedActivityID = entityReference.id;
        }
        else {
            this.selectedActivityID = null;
        }
        this._refreshActivityView$1();
    },
    
    _addSession$1: function Client_TimeSheet_ViewModel_SessionsViewModel$_addSession$1(sessions, session) {
        sessions.add(session);
        session.add_propertyChanged(ss.Delegate.create(this, this._onSessionPropertyChanged$1));
    },
    
    _onSessionPropertyChanged$1: function Client_TimeSheet_ViewModel_SessionsViewModel$_onSessionPropertyChanged$1(sender, e) {
        if ((e.propertyName === 'dev1_starttime') || (e.propertyName === 'dev1_endtime')) {
            Client.TimeSheet.ViewModel.SessionsViewModel._onStartEndDateChanged$1(sender);
        }
        else if (e.propertyName === 'dev1_duration') {
            Client.TimeSheet.ViewModel.SessionsViewModel._onDurationChanged$1(sender);
        }
        this.refresh();
    },
    
    _refreshActivityView$1: function Client_TimeSheet_ViewModel_SessionsViewModel$_refreshActivityView$1() {
        this._data$1 = [];
        var $enum1 = ss.IEnumerator.getEnumerator(this.getCurrentWeek());
        while ($enum1.moveNext()) {
            var session = $enum1.current;
            if (((this.selectedActivityID == null) || (session.dev1_activityid === this.selectedActivityID.toString())) && ((this.selectedDay == null) || (session.dev1_starttime.getDay() === this.selectedDay.getDay()))) {
                this._data$1.add(session);
            }
        }
        var args = {};
        args.from = 0;
        args.to = this._data$1.length - 1;
        this.onDataLoaded.notify(args, null, null);
        this.onRowsChanged.notify(null, null, this);
    },
    
    getEditedSessions: function Client_TimeSheet_ViewModel_SessionsViewModel$getEditedSessions() {
        var edited = [];
        var $enum1 = ss.IEnumerator.getEnumerator(this._weeks$1[this.weekStart.getTime()]);
        while ($enum1.moveNext()) {
            var session = $enum1.current;
            if (session.dev1_sessionid == null || session.entityState === Xrm.Sdk.EntityStates.changed || session.entityState === Xrm.Sdk.EntityStates.created) {
                edited.add(session);
            }
        }
        return edited;
    },
    
    getPagingInfo: function Client_TimeSheet_ViewModel_SessionsViewModel$getPagingInfo() {
        return null;
    },
    
    getLength: function Client_TimeSheet_ViewModel_SessionsViewModel$getLength() {
        if (this._data$1 != null) {
            return this._data$1.length;
        }
        else {
            return 0;
        }
    },
    
    removeItem: function Client_TimeSheet_ViewModel_SessionsViewModel$removeItem(id) {
        this._weeks$1[this.weekStart.getTime()].remove(id);
    },
    
    getItem: function Client_TimeSheet_ViewModel_SessionsViewModel$getItem(index) {
        return this._data$1[index];
    },
    
    setPagingOptions: function Client_TimeSheet_ViewModel_SessionsViewModel$setPagingOptions(p) {
    },
    
    refresh: function Client_TimeSheet_ViewModel_SessionsViewModel$refresh() {
        if (this._weeks$1[this.weekStart.getTime()] == null) {
            this.onDataLoading.notify(null, null, null);
            Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(String.format(Client.TimeSheet.Model.Queries.sessionsByWeekStartDate, Xrm.Sdk.DateTimeEx.toXrmString(this.weekStart), Xrm.Sdk.DateTimeEx.toXrmString(this.weekEnd)), ss.Delegate.create(this, function(result) {
                try {
                    var results = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, dev1_session);
                    var sessions = [];
                    this._weeks$1[this.weekStart.getTime()] = sessions;
                    var $enum1 = ss.IEnumerator.getEnumerator(results.get_entities());
                    while ($enum1.moveNext()) {
                        var e = $enum1.current;
                        this._addSession$1(sessions, e);
                    }
                    this._refreshActivityView$1();
                }
                catch (ex) {
                    alert('There was an error loading the Timesheet sessions\n' + ex.message);
                }
            }));
        }
        else {
            this._refreshActivityView$1();
        }
    },
    
    addItem: function Client_TimeSheet_ViewModel_SessionsViewModel$addItem(item) {
        if (this.selectedActivity != null) {
            var sessions = this.getCurrentWeek();
            var itemAdding = item;
            var newItem = new dev1_session();
            newItem.dev1_description = itemAdding.dev1_description;
            newItem.dev1_starttime = itemAdding.dev1_starttime;
            newItem.dev1_duration = itemAdding.dev1_duration;
            newItem.dev1_activityid = this.selectedActivity.id.toString();
            newItem.dev1_activitytypename = this.selectedActivity.logicalName;
            switch (this.selectedActivity.logicalName) {
                case 'phonecall':
                    newItem.dev1_phonecallid = this.selectedActivity;
                    break;
                case 'task':
                    newItem.dev1_taskid = this.selectedActivity;
                    break;
                case 'letter':
                    newItem.dev1_letterid = this.selectedActivity;
                    break;
                case 'email':
                    newItem.dev1_emailid = this.selectedActivity;
                    break;
            }
            newItem.entityState = Xrm.Sdk.EntityStates.created;
            if (newItem.dev1_starttime == null) {
                newItem.dev1_starttime = (this.selectedDay == null) ? this.weekStart : this.selectedDay;
            }
            this._addSession$1(sessions, newItem);
            this._data$1.add(newItem);
            var args = {};
            args.from = this._data$1.length - 1;
            args.to = this._data$1.length - 1;
            this.onDataLoaded.notify(args, null, null);
            this.onRowsChanged.notify(null, null, null);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.ViewModel.TimeSheetViewModel

Client.TimeSheet.ViewModel.TimeSheetViewModel = function Client_TimeSheet_ViewModel_TimeSheetViewModel() {
    this.startTimeTime = ko.observable();
    Client.TimeSheet.ViewModel.TimeSheetViewModel.initializeBase(this);
    this.sessionDataView = new Client.TimeSheet.ViewModel.SessionsViewModel();
    this.days = new Client.TimeSheet.ViewModel.DaysViewModel(this.sessionDataView);
    this.sessionDataView.setCurrentWeek(Date.get_today());
    this.startTimeTime(Date.parseDate('1975-01-16T13:00:00'));
}
Client.TimeSheet.ViewModel.TimeSheetViewModel.prototype = {
    sessionDataView: null,
    days: null,
    
    _reportError$1: function Client_TimeSheet_ViewModel_TimeSheetViewModel$_reportError$1(ex) {
        debugger;
        alert('There was a problem with your request. Please contact your system administrator.\n\n' + ex.message);
        this.isBusy(false);
    },
    
    activitySearchCommand: function Client_TimeSheet_ViewModel_TimeSheetViewModel$activitySearchCommand(term, callback) {
        var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='activitypointer'>\r\n                                <attribute name='activitytypecode' />\r\n                                <attribute name='subject' />\r\n                                <attribute name='activityid' />\r\n                                <attribute name='instancetypecode' />\r\n                                <order attribute='modifiedon' descending='false' />\r\n                                <filter type='and'>\r\n                                  <condition attribute='ownerid' operator='eq-userid' />\r\n                                    <condition attribute='subject' operator='like' value='%{0}%' />\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
        fetchXml = String.format(fetchXml, Xrm.Sdk.XmlHelper.encode(term));
        Xrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple(fetchXml, function(result) {
            var fetchResult = Xrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(result, ActivityPointer);
            var $enum1 = ss.IEnumerator.getEnumerator(fetchResult.get_entities());
            while ($enum1.moveNext()) {
                var a = $enum1.current;
                a.logicalName = a.activitytypecode;
            }
            callback(fetchResult);
        });
    },
    
    _saveCommand$1: null,
    
    saveCommand: function Client_TimeSheet_ViewModel_TimeSheetViewModel$saveCommand() {
        if (this._saveCommand$1 == null) {
            this._saveCommand$1 = ss.Delegate.create(this, function() {
                var confirmed = confirm(String.format('Are you sure save these sessions?'));
                if (!confirmed) {
                    return;
                }
                try {
                    this.isBusy(true);
                    this.isBusyProgress(0);
                    this.isBusyMessage('Saving...');
                    var editedSessions = this.sessionDataView.getEditedSessions();
                    Xrm.DelegateItterator.callbackItterate(ss.Delegate.create(this, function(index, nextCallback, errorCallBack) {
                        var session = editedSessions[index];
                        this.isBusyProgress((index / editedSessions.length) * 100);
                        if (session.dev1_sessionid == null) {
                            Xrm.Sdk.OrganizationServiceProxy.beginCreate(session, ss.Delegate.create(this, function(result) {
                                this.isBusyProgress((index / editedSessions.length) * 100);
                                try {
                                    session.dev1_sessionid = Xrm.Sdk.OrganizationServiceProxy.endCreate(result);
                                    session.entityState = Xrm.Sdk.EntityStates.unchanged;
                                    session.raisePropertyChanged('EntityState');
                                    nextCallback();
                                }
                                catch (ex) {
                                    alert(ex.message);
                                    nextCallback();
                                }
                            }));
                        }
                        else {
                            Xrm.Sdk.OrganizationServiceProxy.beginUpdate(session, function(result) {
                                try {
                                    Xrm.Sdk.OrganizationServiceProxy.endUpdate(result);
                                    session.entityState = Xrm.Sdk.EntityStates.unchanged;
                                    session.raisePropertyChanged('EntityState');
                                    nextCallback();
                                }
                                catch (ex) {
                                    alert(ex.message);
                                    nextCallback();
                                }
                            });
                        }
                    }), editedSessions.length, ss.Delegate.create(this, function() {
                        this.isBusyProgress(100);
                        this.isBusy(false);
                    }), ss.Delegate.create(this, function(ex) {
                        this._reportError$1(ex);
                    }));
                }
                catch (ex) {
                    this._reportError$1(ex);
                }
            });
        }
        return this._saveCommand$1;
    },
    
    _deleteCommand$1: null,
    
    deleteCommand: function Client_TimeSheet_ViewModel_TimeSheetViewModel$deleteCommand() {
        if (this._deleteCommand$1 == null) {
            this._deleteCommand$1 = ss.Delegate.create(this, function() {
                var selectedRows = SparkleXrm.GridEditor.DataViewBase.rangesToRows(this.sessionDataView.getSelectedRows());
                if (!selectedRows.length) {
                    return;
                }
                var confirmed = confirm(String.format('Are you sure you want to delete the {0} selected sessions?', selectedRows.length));
                if (!confirmed) {
                    return;
                }
                this.isBusyMessage('Deleting Sessions...');
                this.isBusyProgress(0);
                this.isBusy(true);
                var itemsToDelete = [];
                for (var i = 0; i < selectedRows.length; i++) {
                    itemsToDelete.add(this.sessionDataView.getItem(i));
                }
                Xrm.DelegateItterator.callbackItterate(ss.Delegate.create(this, function(index, nextCallback, errorCallBack) {
                    var session = itemsToDelete[index];
                    this.isBusyProgress((index / selectedRows.length) * 100);
                    Xrm.Sdk.OrganizationServiceProxy.beginDelete(session.logicalName, session.dev1_sessionid, ss.Delegate.create(this, function(result) {
                        try {
                            Xrm.Sdk.OrganizationServiceProxy.endDelete(result);
                            this.sessionDataView.removeItem(session);
                            this.sessionDataView.refresh();
                            nextCallback();
                        }
                        catch (ex) {
                            alert(ex.message);
                            nextCallback();
                        }
                    }));
                }), selectedRows.length, ss.Delegate.create(this, function() {
                    this.isBusyProgress(100);
                    this.isBusy(false);
                    this.sessionDataView.refresh();
                    this.days.reCalculate();
                }), ss.Delegate.create(this, function(ex) {
                    this._reportError$1(ex);
                }));
            });
        }
        return this._deleteCommand$1;
    },
    
    _resetCommand$1: null,
    
    resetCommand: function Client_TimeSheet_ViewModel_TimeSheetViewModel$resetCommand() {
        if (this._resetCommand$1 == null) {
        }
        return this._resetCommand$1;
    }
}


Type.registerNamespace('Client.TimeSheet.View');

////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.View.StartStopSession

Client.TimeSheet.View.StartStopSession = function Client_TimeSheet_View_StartStopSession() {
    Client.TimeSheet.View.StartStopSession.initializeBase(this);
}
Client.TimeSheet.View.StartStopSession.init = function Client_TimeSheet_View_StartStopSession$init() {
    var vm = new TimeSheet.Client.ViewModel.StartStopSessionViewModel(null, null);
    SparkleXrm.ViewBase.registerViewModel(vm);
}


////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.View.TimeSheetView

Client.TimeSheet.View.TimeSheetView = function Client_TimeSheet_View_TimeSheetView() {
    Client.TimeSheet.View.TimeSheetView.initializeBase(this);
}
Client.TimeSheet.View.TimeSheetView.init = function Client_TimeSheet_View_TimeSheetView$init() {
    $(function() {
        ko.validation.registerExtenders();
        Xrm.Sdk.OrganizationServiceProxy.getUserSettings();
        var vm = new Client.TimeSheet.ViewModel.TimeSheetViewModel();
        Client.TimeSheet.View.TimeSheetView.setUpGrids(vm);
        Client.TimeSheet.View.TimeSheetView._setUpDatePicker$1(vm);
        SparkleXrm.ViewBase.registerViewModel(vm);
    });
}
Client.TimeSheet.View.TimeSheetView._setUpDatePicker$1 = function Client_TimeSheet_View_TimeSheetView$_setUpDatePicker$1(vm) {
    var element = $('#datepicker');
    var options = {};
    options.numberOfMonths = 3;
    options.calculateWeek = true;
    var dateFormat = 'dd/MM/yy';
    if (Xrm.Sdk.OrganizationServiceProxy.userSettings != null) {
        dateFormat = Xrm.Sdk.OrganizationServiceProxy.userSettings.dateformatstring;
    }
    options.dateFormat = dateFormat.replaceAll('MM', 'mm').replaceAll('yyyy', 'yy').replaceAll('M', 'm');
    var options2 = {};
    options2.numberOfMonths = 3;
    options2.dateFormat = dateFormat.replaceAll('MM', 'mm').replaceAll('yyyy', 'yy').replaceAll('M', 'm');
    options2.onSelect = function(dateText, instance) {
        var controller = Client.TimeSheet.View.TimeSheetView._sessionsGrid$1.getEditController();
        var editCommited = controller.commitCurrentEdit();
        if (editCommited) {
            var date = element.datepicker('getDate');
            vm.sessionDataView.setCurrentWeek(date);
        }
    };
    element.datepicker(options2);
}
Client.TimeSheet.View.TimeSheetView.setUpGrids = function Client_TimeSheet_View_TimeSheetView$setUpGrids(vm) {
    var daysGridOpts = {};
    daysGridOpts.enableCellNavigation = true;
    daysGridOpts.enableColumnReorder = false;
    daysGridOpts.autoEdit = false;
    daysGridOpts.editable = true;
    daysGridOpts.rowHeight = 20;
    daysGridOpts.headerRowHeight = 25;
    daysGridOpts.forceFitColumns = false;
    daysGridOpts.enableAddRow = true;
    var daysDataView = vm.days;
    var columns = [];
    SparkleXrm.GridEditor.GridDataViewBinder.bindRowIcon(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, '', 50, 'icon'), 'activity');
    SparkleXrm.GridEditor.XrmLookupEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Activity', 300, 'activity'), ss.Delegate.create(vm, vm.activitySearchCommand), 'activityid', 'subject', 'activitytypecode');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Mon', 50, 'day0');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Tue', 50, 'day1');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Wed', 50, 'day2');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Thu', 50, 'day3');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Fri', 50, 'day4');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Sat', 50, 'day5');
    SparkleXrm.GridEditor.GridDataViewBinder.addColumn(columns, 'Sun', 50, 'day6');
    Client.TimeSheet.View.TimeSheetView._daysGrid$1 = new Slick.Grid('#timesheetGridContainer', daysDataView, columns, daysGridOpts);
    var daysDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    daysDataBinder.dataBindEvents(Client.TimeSheet.View.TimeSheetView._daysGrid$1, daysDataView, 'timesheetGridContainer');
    daysDataView.add_onGetItemMetaData(function(item) {
        var day = item;
        if (day != null && day.isTotalRow) {
            var metaData = {};
            metaData.editor = null;
            metaData.formatter = function(row, cell, value, columnDef, dataContext) {
                if (columnDef.field === 'activity') {
                    return 'Total';
                }
                else {
                    return SparkleXrm.GridEditor.Formatters.defaultFormatter(row, cell, value, columnDef, dataContext);
                }
            };
            metaData.cssClasses = 'days_total_row';
            return metaData;
        }
        else {
            return null;
        }
    });
    daysDataBinder.dataBindSelectionModel(Client.TimeSheet.View.TimeSheetView._daysGrid$1, daysDataView);
    var sessionsDataView = vm.sessionDataView;
    var sessionGridCols = [];
    SparkleXrm.GridEditor.GridDataViewBinder.addEditIndicatorColumn(sessionGridCols);
    SparkleXrm.GridEditor.XrmTextEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(sessionGridCols, 'Description', 200, 'dev1_description'));
    SparkleXrm.GridEditor.XrmDateEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(sessionGridCols, 'Date', 200, 'dev1_starttime'), true);
    SparkleXrm.GridEditor.XrmTimeEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(sessionGridCols, 'Start', 100, 'dev1_starttime')).validator = function(value, item) {
        var session = item;
        var newStartTime = value;
        var result = {};
        if (session.dev1_endtime != null) {
            result.valid = true;
            var valueText = value;
            var isValid = Xrm.Sdk.DateTimeEx.getTimeDuration(newStartTime) < Xrm.Sdk.DateTimeEx.getTimeDuration(session.dev1_endtime);
            result.valid = isValid;
            result.message = 'The start time must be before the end time';
        }
        else {
            result.valid = true;
        }
        return result;
    };
    SparkleXrm.GridEditor.XrmTimeEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(sessionGridCols, 'End', 100, 'dev1_endtime')).validator = function(value, item) {
        var session = item;
        var newEndTime = value;
        var result = {};
        if (session.dev1_starttime != null) {
            result.valid = true;
            var valueText = value;
            var isValid = Xrm.Sdk.DateTimeEx.getTimeDuration(session.dev1_starttime) < Xrm.Sdk.DateTimeEx.getTimeDuration(newEndTime);
            result.valid = isValid;
            result.message = 'The end time must be after the start time';
        }
        else {
            result.valid = true;
        }
        return result;
    };
    SparkleXrm.GridEditor.XrmDurationEditor.bindColumn(SparkleXrm.GridEditor.GridDataViewBinder.addColumn(sessionGridCols, 'Duration', 200, 'dev1_duration'));
    var sessionsDataBinder = new SparkleXrm.GridEditor.GridDataViewBinder();
    Client.TimeSheet.View.TimeSheetView._sessionsGrid$1 = sessionsDataBinder.dataBindXrmGrid(sessionsDataView, sessionGridCols, 'sessionsGridContainer', null, true, true);
    sessionsDataBinder.dataBindSelectionModel(Client.TimeSheet.View.TimeSheetView._sessionsGrid$1, sessionsDataView);
    Client.TimeSheet.View.TimeSheetView._daysGrid$1.onActiveCellChanged.subscribe(function(e, args) {
        var activeCell = Client.TimeSheet.View.TimeSheetView._daysGrid$1.getActiveCell();
        if (activeCell != null) {
            if (activeCell.cell < 2) {
                vm.days.set_selectedDay(null);
            }
            else {
                vm.days.set_selectedDay(activeCell.cell - 1);
            }
        }
    });
}


ActivityPointer.registerClass('ActivityPointer', Xrm.Sdk.Entity);
dev1_session.registerClass('dev1_session', Xrm.Sdk.Entity);
AddressSearch.App.registerClass('AddressSearch.App');
Client.ContactEditor.ViewModels.ContactValidation.registerClass('Client.ContactEditor.ViewModels.ContactValidation');
Client.ContactEditor.ViewModels.ObservableContact.registerClass('Client.ContactEditor.ViewModels.ObservableContact');
Client.ContactEditor.ViewModels.ContactsEditorViewModel.registerClass('Client.ContactEditor.ViewModels.ContactsEditorViewModel', SparkleXrm.ViewModelBase);
Client.DataGrouping.ViewModels.DataGroupingViewModel.registerClass('Client.DataGrouping.ViewModels.DataGroupingViewModel', SparkleXrm.ViewModelBase);
Client.DataGrouping.ViewModels.Project.registerClass('Client.DataGrouping.ViewModels.Project');
Client.DataGrouping.ViewModels.TreeDataView.registerClass('Client.DataGrouping.ViewModels.TreeDataView', SparkleXrm.GridEditor.DataViewBase, Object);
Client.DataGrouping.ViewModels.TreeDataViewModel.registerClass('Client.DataGrouping.ViewModels.TreeDataViewModel', SparkleXrm.ViewModelBase);
Client.DataGrouping.ViewModels.TreeItem.registerClass('Client.DataGrouping.ViewModels.TreeItem');
Client.DataGrouping.ViewModels.GroupedItems.registerClass('Client.DataGrouping.ViewModels.GroupedItems');
Client.DataGrouping.Views.DataGroupingView.registerClass('Client.DataGrouping.Views.DataGroupingView');
Client.DataGrouping.Views.GroupGridRowPlugin.registerClass('Client.DataGrouping.Views.GroupGridRowPlugin', null, Object);
Client.DataGrouping.Views.TreeView.registerClass('Client.DataGrouping.Views.TreeView');
Client.MultiEntitySearch.ViewModels.MultiSearchViewModel.registerClass('Client.MultiEntitySearch.ViewModels.MultiSearchViewModel', SparkleXrm.ViewModelBase);
Client.MultiEntitySearch.ViewModels.QueryParser.registerClass('Client.MultiEntitySearch.ViewModels.QueryParser');
Client.MultiEntitySearch.Views.MultiSearchView.registerClass('Client.MultiEntitySearch.Views.MultiSearchView');
Client.QuoteLineItemEditor.Model.QuoteDetail.registerClass('Client.QuoteLineItemEditor.Model.QuoteDetail', Xrm.Sdk.Entity);
Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail.registerClass('Client.QuoteLineItemEditor.ViewModels.ObservableQuoteDetail');
Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation.registerClass('Client.QuoteLineItemEditor.ViewModels.QuoteDetailValidation');
Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel.registerClass('Client.QuoteLineItemEditor.ViewModels.QuoteLineItemEditorViewModel', SparkleXrm.ViewModelBase);
Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView.registerClass('Client.QuoteLineItemEditor.Views.QuoteLineItemEditorView', SparkleXrm.ViewBase);
Client.ScheduledJobsEditor.Model.BulkDeleteOperation.registerClass('Client.ScheduledJobsEditor.Model.BulkDeleteOperation', Xrm.Sdk.Entity);
Client.ScheduledJobsEditor.Model.dev1_ScheduledJob.registerClass('Client.ScheduledJobsEditor.Model.dev1_ScheduledJob', Xrm.Sdk.Entity);
Client.ScheduledJobsEditor.Model.asyncoperation.registerClass('Client.ScheduledJobsEditor.Model.asyncoperation', Xrm.Sdk.Entity);
Client.ScheduledJobsEditor.ViewModels.ScheduledJob.registerClass('Client.ScheduledJobsEditor.ViewModels.ScheduledJob');
Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel.registerClass('Client.ScheduledJobsEditor.ViewModels.ScheduledJobsEditorViewModel', SparkleXrm.ViewModelBase);
Client.ScheduledJobsEditor.ViewModels.PendingDelete.registerClass('Client.ScheduledJobsEditor.ViewModels.PendingDelete');
Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames.registerClass('Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames');
Client.ScheduledJobsEditor.ViewModels.DayNames.registerClass('Client.ScheduledJobsEditor.ViewModels.DayNames');
Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper.registerClass('Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper');
Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.registerClass('Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView', SparkleXrm.ViewBase);
Client.ContactEditor.Model.Contact.registerClass('Client.ContactEditor.Model.Contact', Xrm.Sdk.Entity);
Client.Views.ContactEditorView.registerClass('Client.Views.ContactEditorView', SparkleXrm.ViewBase);
Client.TimeSheet.Model.Queries.registerClass('Client.TimeSheet.Model.Queries');
TimeSheet.Client.ViewModel.ObservableActivityPointer.registerClass('TimeSheet.Client.ViewModel.ObservableActivityPointer');
TimeSheet.Client.ViewModel.SessionVM.registerClass('TimeSheet.Client.ViewModel.SessionVM');
TimeSheet.Client.ViewModel.StartStopSessionViewModel.registerClass('TimeSheet.Client.ViewModel.StartStopSessionViewModel', SparkleXrm.ViewModelBase);
Client.TimeSheet.ViewModel.DayEntry.registerClass('Client.TimeSheet.ViewModel.DayEntry');
Client.TimeSheet.ViewModel.DaysViewModel.registerClass('Client.TimeSheet.ViewModel.DaysViewModel', SparkleXrm.GridEditor.DataViewBase);
Client.TimeSheet.ViewModel.SessionsViewModel.registerClass('Client.TimeSheet.ViewModel.SessionsViewModel', SparkleXrm.GridEditor.DataViewBase);
Client.TimeSheet.ViewModel.TimeSheetViewModel.registerClass('Client.TimeSheet.ViewModel.TimeSheetViewModel', SparkleXrm.ViewModelBase);
Client.TimeSheet.View.StartStopSession.registerClass('Client.TimeSheet.View.StartStopSession', SparkleXrm.ViewBase);
Client.TimeSheet.View.TimeSheetView.registerClass('Client.TimeSheet.View.TimeSheetView', SparkleXrm.ViewBase);
ActivityPointer.entityLogicalName = 'activitypointer';
ActivityPointer.entityTypeCode = 4200;
dev1_session.entityLogicalName = 'dev1_session';
dev1_session.entityTypeCode = 10000;
(function () {
})();
Client.ScheduledJobsEditor.Model.BulkDeleteOperation.entityLogicalName = 'bulkdeleteoperation';
Client.ScheduledJobsEditor.Model.dev1_ScheduledJob.entityLogicalName = 'dev1_scheduledjob';
Client.ScheduledJobsEditor.Model.asyncoperation.entityLogicalName = 'asyncoperation';
Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames.YEARLY = 'YEARLY';
Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames.WEEKLY = 'WEEKLY';
Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames.DAILY = 'DAILY';
Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames.HOURLY = 'HOURLY';
Client.ScheduledJobsEditor.ViewModels.RecurranceFrequencyNames.MINUTELY = 'MINUTELY';
Client.ScheduledJobsEditor.ViewModels.DayNames.monday = 'MO';
Client.ScheduledJobsEditor.ViewModels.DayNames.tuesday = 'TU';
Client.ScheduledJobsEditor.ViewModels.DayNames.wednesday = 'WE';
Client.ScheduledJobsEditor.ViewModels.DayNames.thursday = 'TH';
Client.ScheduledJobsEditor.ViewModels.DayNames.friday = 'FR';
Client.ScheduledJobsEditor.ViewModels.DayNames.saturday = 'SA';
Client.ScheduledJobsEditor.ViewModels.DayNames.sunday = 'SU';
Client.ScheduledJobsEditor.ViewModels.RecurrancePatternMapper._frequencies = null;
Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.jobsGrid = null;
Client.ScheduledJobsEditor.Views.ScheduledJobsEditorView.bulkDeleteJobsGrid = null;
Client.TimeSheet.Model.Queries.currentRunningActivities = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" + "<entity name='activitypointer'>" + "<attribute name='activitytypecode' />" + "<attribute name='subject' />" + "<attribute name='activityid' />" + "<attribute name='instancetypecode' />" + "<order attribute='modifiedon' descending='false' />" + "<filter type='and'>" + "<condition attribute='ownerid' operator='eq-userid' />" + '</filter>' + '</entity>' + '</fetch>';
Client.TimeSheet.Model.Queries.currentOpenActivitesWithSessions = "<fetch version='1.0' output-format='xml-platform' mapping='logical' aggregate='true'>" + "<entity name='activitypointer'>" + "<attribute name='subject' groupby='true' alias='a.subject'/>" + "<attribute name='activityid' groupby='true' alias='a.activityid'/>" + "<filter type='and'>" + "<condition attribute='ownerid' operator='eq-userid'  />" + "<condition attribute='statecode' operator='not-in'>" + '<value>1</value>' + '<value>2</value>' + '</condition>' + '</filter>' + "<link-entity name='dev1_session' from='dev1_activityid' to='activityid' alias='s'>" + "<attribute name='dev1_runningflag' aggregate='max' distinct='true' alias='isRunning'/>" + '</link-entity>' + '</entity>' + '</fetch>';
Client.TimeSheet.Model.Queries.sessionsByWeekStartDate = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" + "<entity name='dev1_session'>" + "<attribute name='dev1_sessionid' />" + "<attribute name='dev1_description' />" + "<attribute name='dev1_activityid' />" + "<attribute name='dev1_activitytypename' />" + "<attribute name='dev1_starttime' />" + "<attribute name='dev1_endtime' />" + "<attribute name='dev1_duration' />" + "<attribute name='dev1_taskid' />" + "<attribute name='dev1_letterid' />" + "<attribute name='dev1_emailid' />" + "<attribute name='dev1_phonecallid' />" + "<order attribute='dev1_description' descending='false' />" + "<filter type='and'>" + "<condition attribute='dev1_starttime' operator='on-or-after' value='{0}' />" + "<condition attribute='dev1_starttime' operator='on-or-before' value='{1}' />" + '</filter>' + '</entity>' + '</fetch>';
Client.TimeSheet.View.TimeSheetView._daysGrid$1 = null;
Client.TimeSheet.View.TimeSheetView._sessionsGrid$1 = null;
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
