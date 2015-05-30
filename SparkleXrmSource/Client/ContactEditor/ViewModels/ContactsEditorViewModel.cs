// ContactsEditorViewModel.cs
//

using System;
using System.Collections.Generic;
using Xrm.Sdk;
using KnockoutApi;
using Slick;
using Client.TimeSheet.ViewModel;
using SparkleXrm.GridEditor;
using SparkleXrm;
using Client.ContactEditor.ViewModels;
using Client.ContactEditor.Model;

namespace Client.ContactEditor.ViewModels
{
    public class ContactsEditorViewModel :ViewModelBase
    {
        #region Fields
       
        public EntityDataViewModel Contacts;
        public Observable<ObservableContact> SelectedContact;
        
        #endregion

        #region Constructors
        public ContactsEditorViewModel()
        {
          
            SelectedContact = (Observable<ObservableContact>)ValidatedObservableFactory.ValidatedObservable(new ObservableContact());

            Contacts = new EntityDataViewModel(10, typeof(Contact),true);
           
           
            Contacts.OnSelectedRowsChanged += OnSelectedRowsChanged;
            Contacts.FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>
                <entity name='contact'>
                    <attribute name='firstname' />
                    <attribute name='lastname' />
                    <attribute name='telephone1' />
                    <attribute name='birthdate' />
                    <attribute name='accountrolecode' />
                    <attribute name='parentcustomerid'/>
                    <attribute name='transactioncurrencyid'/>
                    <attribute name='creditlimit'/>
                    <attribute name='numberofchildren'/>
                    <attribute name='contactid' />
                    <attribute name='ownerid'/>{3}
                  </entity>
                </fetch>";

            // Register validation
            ContactValidation.Register(Contacts.ValidationBinder);
            ContactValidation.Register(new ObservableValidationBinder(this.SelectedContact));
        }
        #endregion

        #region Events
        public void OnSelectedRowsChanged()
        {
            SelectedRange[] selectedContacts = Contacts.GetSelectedRows();
            if (selectedContacts.Length>0)
            {
                SelectedContact.GetValue().SetValue((Contact)Contacts.GetItem(selectedContacts[0].FromRow.Value));

                
            }
            else
                SelectedContact.GetValue().SetValue(null);

        }
        #endregion

        #region Commands
        
        private DependentObservable<bool> _canAddNew;
        public DependentObservable<bool> CanAddNew()
        {
            if (_canAddNew==null)
            {
                DependentObservableOptions<bool> IsRegisterFormValidDependantProperty = new DependentObservableOptions<bool>();
                IsRegisterFormValidDependantProperty.Model = this;
                IsRegisterFormValidDependantProperty.GetValueFunction = new Func<bool>(delegate
                {
                    EntityStates state = SelectedContact.GetValue().EntityState.GetValue();
                    if (state != null)
                    {
                        return state != EntityStates.Created;
                    }
                    else
                        return true;

                });
                _canAddNew = Knockout.DependentObservable<bool>(IsRegisterFormValidDependantProperty);

            }
            return _canAddNew;
        }
        private Action _saveSelectedContact;
        public Action SaveSelectedContact()
        {
            
            if (_saveSelectedContact == null)
            {
                _saveSelectedContact = delegate()
                {
                    if (((IValidatedObservable)SelectedContact).IsValid())
                    {
                        Contact contact = SelectedContact.GetValue().Commit();
                        ObservableContact selectedContact = SelectedContact.GetValue();

                        if (selectedContact.EntityState.GetValue() == EntityStates.Created)
                        {
                            this.Contacts.AddItem(contact);

                            // Move to the last page
                            PagingInfo paging = this.Contacts.GetPagingInfo();

                            SelectedRange[] newRow = new SelectedRange[1];
                            newRow[0] = new SelectedRange();
                            newRow[0].FromRow = paging.TotalRows - ((paging.TotalPages - 1) * paging.PageSize)-1;
                            this.Contacts.RaiseOnSelectedRowsChanged(newRow);
                            selectedContact.EntityState.SetValue(EntityStates.Changed);
                        }
                        else
                        {
                            Contacts.Refresh();
                        }
                    }
                    else
                    {
                        Script.Literal("{0}.errors.showAllMessages()", SelectedContact);
                    }
                };
            }
            return _saveSelectedContact;

        }
        private Action _addNewContact;
        public Action AddNewContact()
        {

            if (_addNewContact == null)
            {
                _addNewContact = delegate()
                {
                    
                    Contact newContact = new Contact();
                    newContact.EntityState = EntityStates.Created;
                  
                    this.SelectedContact.GetValue().SetValue(newContact);
                    Script.Literal("{0}.errors.showAllMessages(false)", SelectedContact);
                    
                   
                    
                };
            }
            return _addNewContact;

        }
        private Action _resetCommand;
        public Action ResetCommand()
        {
            if (_resetCommand == null)
            {
                _resetCommand = delegate()
                {
                    bool confirmed = Script.Confirm(String.Format("Are you sure you want to reset the grid? This will loose any values you have edited."));
                    if (!confirmed)
                        return;

                    Contacts.Reset();
                    Contacts.Refresh();

                };
            }

            return _resetCommand;

        }
        private Action _saveCommand;
        public Action SaveCommand()
        {
            if (_saveCommand == null)
            {
                _saveCommand = delegate()
                {

                    List<Contact> dirtyCollection = new List<Contact>();
                    foreach (Entity item in this.Contacts.Data)
                    {
                        if (item!=null && item.EntityState!=EntityStates.Unchanged)
                            dirtyCollection.Add((Contact)item);

                    }
                    int itemCount = dirtyCollection.Count;
                    if (itemCount == 0)
                        return;

                    bool confirmed = Script.Confirm(String.Format("Are you sure that you want to save the {0} records edited in the Grid?", itemCount));
                    if (!confirmed)
                        return;

                    IsBusy.SetValue(true);

                    string errorMessage = "";
                    SaveNextRecord(dirtyCollection, errorMessage, delegate()
                    {
                        if (errorMessage.Length > 0)
                        {
                            Script.Alert("One or more records failed to save.\nPlease contact your System Administrator.\n\n" + errorMessage);
                        }
                        else
                        {
                            Script.Alert("Save Complete!");
                        }
                        Contacts.Refresh();
                        IsBusy.SetValue(false);
                    });




                };
            }
            return _saveCommand;
        }

        public void TransactionCurrencySearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get the option set values

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='transactioncurrency'>
                                <attribute name='transactioncurrencyid' />
                                <attribute name='currencyname' />
                                <attribute name='isocurrencycode' />
                                <attribute name='currencysymbol' />
                                <attribute name='exchangerate' />
                                <attribute name='currencyprecision' />
                                <order attribute='currencyname' descending='false' />
                                <filter type='and'>
                                  <condition attribute='currencyname' operator='like' value='%{0}%' />
                                </filter>
                              </entity>
                            </fetch>";

            fetchXml = string.Format(fetchXml, XmlHelper.Encode(term));
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
                callback(fetchResult);


            });
        }
        public void OwnerSearchCommand(string term, Action<EntityCollection> callback)
        {

            Dictionary<string, string> searchTypes = new Dictionary<string, string>();
            searchTypes["systemuser"] = "fullname";
            searchTypes["team"] = "name";

            int resultsBack = 0;
            List<Entity> mergedEntities = new List<Entity>();
            Action<EntityCollection> result = delegate(EntityCollection fetchResult)
            {
                resultsBack++;
                // Merge in the results
                mergedEntities.AddRange((Entity[])(object)fetchResult.Entities.Items());

                mergedEntities.Sort(delegate(Entity x, Entity y)
                {
                    return string.Compare(x.GetAttributeValueString("name"), y.GetAttributeValueString("name"));
                });
                if (resultsBack == searchTypes.Count)
                {
                    EntityCollection results = new EntityCollection(mergedEntities);
                    callback(results);
                }
            };

            foreach (string entity in searchTypes.Keys)
            {
                SearchRecords(term, result, entity, searchTypes[entity]);
            }
        }

        private void SearchRecords(string term, Action<EntityCollection> callback, string entityType, string entityNameAttribute)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true' count='25'>
                              <entity name='{1}'>
                                <attribute name='{2}' alias='name' />
                                <order attribute='{2}' descending='false' />
                                <filter type='and'>
                                  <condition attribute='{2}' operator='like' value='%{0}%' />
                                </filter>
                              </entity>
                            </fetch>";

            fetchXml = string.Format(fetchXml, XmlHelper.Encode(term), entityType, entityNameAttribute);
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
                callback(fetchResult);
            });
        }
        public void ReportError(Exception ex)
        {

        }
        private void SaveNextRecord(List<Contact> dirtyCollection,string errorMessage,Action callBack)
        {
            Contact contactToSave = dirtyCollection[0];
            if (contactToSave.ContactId == null)
            {
                OrganizationServiceProxy.BeginCreate(contactToSave, delegate(object r)
                {
                    try
                    {
                        Guid newID = OrganizationServiceProxy.EndCreate(r);
                        contactToSave.ContactId = newID;
                        contactToSave.EntityState = EntityStates.Unchanged;
                    }
                    catch (Exception ex)
                    {
                        // Something when wrong with create
                        errorMessage = errorMessage + ex.Message + "\n";
                    }
                    dirtyCollection.Remove(contactToSave);

                    if (dirtyCollection.Count == 0)
                        callBack();
                    else
                        SaveNextRecord(dirtyCollection, errorMessage, callBack);
                });

            }
            else
            {
                OrganizationServiceProxy.BeginUpdate(contactToSave, delegate(object r)
                {
                    try
                    {
                        OrganizationServiceProxy.EndUpdate(r);
                        contactToSave.EntityState = EntityStates.Unchanged;

                    }
                    catch (Exception ex)
                    {
                        // Something when wrong
                        errorMessage = errorMessage + ex.Message + "\n";
                    }

                    dirtyCollection.Remove(contactToSave);

                    if (dirtyCollection.Count == 0)
                        callBack();
                    else
                        SaveNextRecord(dirtyCollection, errorMessage, callBack);

                });
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            Contacts.Refresh();
        }
        #endregion
    }
}
