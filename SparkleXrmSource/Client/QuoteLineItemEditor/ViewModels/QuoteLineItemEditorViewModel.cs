using Client.QuoteLineItemEditor.Model;
using jQueryApi;
using KnockoutApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Html;
using Xrm;
using Xrm.Sdk;

namespace Client.QuoteLineItemEditor.ViewModels
{
    public class QuoteLineItemEditorViewModel : ViewModelBase
    {
        #region Fields
        public EntityDataViewModel Lines;
        public Observable<ObservableQuoteDetail> SelectedQuoteDetail = Knockout.Observable<ObservableQuoteDetail>();
        private string _transactionCurrencyId;
        #endregion

        #region Constructors
        public QuoteLineItemEditorViewModel()
        {
            Lines = new EntityDataViewModel(10, typeof(QuoteDetail), false);

            Lines.OnSelectedRowsChanged += OnSelectedRowsChanged;
            Lines.FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>
                              <entity name='quotedetail'>
                                <attribute name='productid' />
                                <attribute name='productdescription' />
                                <attribute name='priceperunit' />
                                <attribute name='quantity' />
                                <attribute name='extendedamount' />
                                <attribute name='quotedetailid' />
                                <attribute name='isproductoverridden' />
                                <attribute name='ispriceoverridden' />
                                <attribute name='manualdiscountamount' />
                                <attribute name='lineitemnumber' />
                                <attribute name='description' />
                                <attribute name='transactioncurrencyid' />
                                <attribute name='baseamount' />
                                <attribute name='requestdeliveryby' />
                                <attribute name='salesrepid' />
                                <attribute name='uomid' />
                                {3}
                                <link-entity name='quote' from='quoteid' to='quoteid' alias='ac'>
                                  <filter type='and'>
                                    <condition attribute='quoteid' operator='eq' uiname='tes' uitype='quote' value='" + GetQuoteId() + @"' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            Lines.SortBy(new SortCol("lineitemnumber", true));
            Lines.NewItemFactory = NewLineFactory;
            QuoteDetailValidation.Register(Lines.ValidationBinder);
            SelectedQuoteDetail.SetValue(new ObservableQuoteDetail());
            
        }
        #endregion

        #region Methods
        public bool IsEditForm()
        {

            if (ParentPage.Ui != null)
                return (ParentPage.Ui.GetFormType() == FormTypes.Update);
            else
                return true; // Debug
        }
        public string GetQuoteId()
        {
            string quoteId = "DB040ECD-5ED4-E211-9BE0-000C299FFE7D"; // Debug
            
            if (ParentPage.Ui != null)
            {
                string guid = ParentPage.Data.Entity.GetId();
                if (guid != null)
                    quoteId = guid.Replace("{", "").Replace("}", "");

                this._transactionCurrencyId = ((Lookup[])ParentPage.GetAttribute("transactioncurrencyid").GetValue())[0].Id;
            }

            return quoteId;
        }
        public Entity NewLineFactory(object item)
        {
            QuoteDetail newLine = new QuoteDetail();
            jQuery.Extend(newLine, item);
            newLine.LineItemNumber = Lines.GetPagingInfo().TotalRows + 1;
            newLine.QuoteId = new EntityReference(new Guid(GetQuoteId()), "quote", null);
            if (_transactionCurrencyId!=null)
                newLine.TransactionCurrencyId = new EntityReference(new Guid(_transactionCurrencyId), "transactioncurrency", "");
            return newLine;
        }

        private void OnSelectedRowsChanged()
        {
            SelectedRange[] selectedContacts = Lines.GetSelectedRows();
            if (selectedContacts.Length > 0)
            {
                ObservableQuoteDetail observableQuoteDetail = SelectedQuoteDetail.GetValue();
                if (observableQuoteDetail.InnerQuoteDetail != null)
                    observableQuoteDetail.InnerQuoteDetail.PropertyChanged -= quote_PropertyChanged;

                QuoteDetail selectedQuoteDetail = (QuoteDetail)Lines.GetItem(selectedContacts[0].FromRow.Value);
                if (selectedQuoteDetail != null)
                    selectedQuoteDetail.PropertyChanged += quote_PropertyChanged;

                SelectedQuoteDetail.GetValue().SetValue(selectedQuoteDetail);
            }
            else
                SelectedQuoteDetail.GetValue().SetValue(null);
        }

        void quote_PropertyChanged(object sender, Xrm.ComponentModel.PropertyChangedEventArgs e)
        {
            Window.SetTimeout(delegate() { Lines.Refresh(); }, 0);
        }

        public void ProductSearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get products from the search term
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='productid' />
                                <attribute name='name' />
                                <order attribute='name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='name' operator='like' value='%{0}%' />
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

        public void UoMSearchCommand(string term, Action<EntityCollection> callback)
        {
            string fetchXml = @" <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='uom'>
                                    <attribute name='uomid' />
                                    <attribute name='name' />
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='name' operator='like' value='%{0}%' />
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
       
        public void SalesRepSearchCommand(string term, Action<EntityCollection> callback)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='systemuser'>
                                    <attribute name='fullname' />
                                    <attribute name='businessunitid' />
                                    <attribute name='title' />
                                    <attribute name='address1_telephone1' />
                                    <attribute name='systemuserid' />
                                    <order attribute='fullname' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='fullname' operator='like' value='%{0}%' />
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

        private void SaveNextRecord(List<QuoteDetail> dirtyCollection, List<string> errorMessages, Action callBack)
        {
            QuoteDetail itemToSave = dirtyCollection[0];
            if (itemToSave.EntityState == EntityStates.Deleted)
            {
                OrganizationServiceProxy.BeginDelete("quotedetail",itemToSave.QuoteDetailId, delegate(object r)
                {
                    try
                    {
                        OrganizationServiceProxy.EndDelete(r);
                        itemToSave.EntityState = EntityStates.Unchanged;
                    }
                    catch (Exception ex)
                    {
                        // Something when wrong with delete
                        errorMessages.Add(ex.Message);
                    }
                    FinishSaveRecord(dirtyCollection, errorMessages, callBack, itemToSave);
                });

            }
            else if (itemToSave.QuoteDetailId == null)
            {
                OrganizationServiceProxy.BeginCreate(itemToSave, delegate(object r)
                {
                    try
                    {
                        Guid newID = OrganizationServiceProxy.EndCreate(r);
                        itemToSave.QuoteDetailId = newID;
                        itemToSave.EntityState = EntityStates.Unchanged;
                    }
                    catch (Exception ex)
                    {
                        // Something when wrong with create
                        errorMessages.Add(ex.Message);
                    }
                    FinishSaveRecord(dirtyCollection, errorMessages, callBack, itemToSave);
                });

            }
            else
            {
                OrganizationServiceProxy.BeginUpdate(itemToSave, delegate(object r)
                {
                    try
                    {
                        OrganizationServiceProxy.EndUpdate(r);
                        itemToSave.EntityState = EntityStates.Unchanged;

                    }
                    catch (Exception ex)
                    {
                        // Something when wrong with update
                        errorMessages.Add(ex.Message);
                    }

                    FinishSaveRecord(dirtyCollection, errorMessages, callBack, itemToSave);

                });
            }
        }

        private void FinishSaveRecord(List<QuoteDetail> dirtyCollection, List<string> errorMessages, Action callBack, QuoteDetail itemToSave)
        {
            dirtyCollection.Remove(itemToSave);

            if (dirtyCollection.Count == 0)
                callBack();
            else
                SaveNextRecord(dirtyCollection, errorMessages, callBack);
        }
        #endregion

        #region Commands
        private Action _saveCommand;
        public Action SaveCommand()
        {
            if (_saveCommand == null)
            {
                _saveCommand = delegate()
                {
                    if (!CommitEdit())
                        return;

                    List<QuoteDetail> dirtyCollection = new List<QuoteDetail>();
                    // Add new/changed items
                    foreach (Entity item in this.Lines.Data)
                    {
                        if (item != null && item.EntityState != EntityStates.Unchanged)
                            dirtyCollection.Add((QuoteDetail)item);

                    }
                    // Add deleted items
                    if (this.Lines.DeleteData != null)
                    {
                        foreach (Entity item in this.Lines.DeleteData)
                        {
                            if (item.EntityState == EntityStates.Deleted)
                                dirtyCollection.Add((QuoteDetail)item);
                        }
                    }

                    int itemCount = dirtyCollection.Count;
                    if (itemCount == 0)
                        return;

                    bool confirmed = Script.Confirm(String.Format("Are you sure that you want to save the {0} quote lines in the Grid?", itemCount));
                    if (!confirmed)
                        return;

                    IsBusy.SetValue(true);

                    List<string> errorMessages = new List<string>();
                    SaveNextRecord(dirtyCollection, errorMessages, delegate()
                    {
                        if (errorMessages.Count > 0)
                        {
                            Script.Alert("One or more records failed to save.\nPlease contact your System Administrator.\n\n" + errorMessages.Join(","));
                        }
                        if (Lines.DeleteData != null)
                            Lines.DeleteData.Clear();
                        Lines.Refresh();
                        IsBusy.SetValue(false);
                    });


                };
            }
            return _saveCommand;
        }
        private Action _deleteCommand;
        public Action DeleteCommand()
        {
            if (_deleteCommand == null)
            {
                _deleteCommand = delegate()
                {
                    List<int> selectedRows = DataViewBase.RangesToRows(Lines.GetSelectedRows());
                    if (selectedRows.Count == 0)
                        return;

                    bool confirmed = Script.Confirm(String.Format("Are you sure that you want to delete the {0} quote lines in the Grid?", selectedRows.Count));
                    if (!confirmed)
                        return;
                    List<Entity> itemsToRemove = new List<Entity>();
                    foreach (int row in selectedRows)
                    {
                        itemsToRemove.Add((Entity)Lines.GetItem(row));
                    }
                    foreach (Entity item in itemsToRemove)
                    {
                        item.EntityState = EntityStates.Deleted;
                        Lines.RemoveItem(item);
                    }
                    Lines.Refresh();
                };
            }
            return _deleteCommand;
        }
        private Action _moveUpCommand;
        public Action MoveUpCommand()
        {

            if (_moveUpCommand == null)
            {
                _moveUpCommand = delegate()
                {

                    // reindex all the items by moving the selected index up
                    SelectedRange[] range = Lines.GetSelectedRows();
                    int fromRow = (int)range[0].FromRow;

                    if (fromRow == 0)
                        return;

                    QuoteDetail line = (QuoteDetail)Lines.GetItem(fromRow);
                    QuoteDetail lineBefore = (QuoteDetail)Lines.GetItem(fromRow - 1);
                    // swap the indexes from the item before
                    int? lineItemNumber = line.LineItemNumber;

                    line.LineItemNumber = lineBefore.LineItemNumber;
                    lineBefore.LineItemNumber = lineItemNumber;
                    line.RaisePropertyChanged("LineItemNumber");
                    lineBefore.RaisePropertyChanged("LineItemNumber");
                    range[0].FromRow--;
                    Lines.RaiseOnSelectedRowsChanged(range);
                    Lines.SortBy(new SortCol("lineitemnumber", true));
                    Lines.Refresh();               
                };
            }
            return _moveUpCommand;

        }

        private Action _moveDownCommand;
        public Action MoveDownCommand()
        {

            if (_moveDownCommand == null)
            {
                _moveDownCommand = delegate()
                {
                    // reindex all the items by moving the selected index up
                    SelectedRange[] range = Lines.GetSelectedRows();
                    int fromRow = (int)range[0].FromRow;

                    if (fromRow == Lines.GetLength() - 1)
                        return;

                    QuoteDetail line = (QuoteDetail)Lines.GetItem(fromRow);
                    QuoteDetail lineAfter = (QuoteDetail)Lines.GetItem(fromRow + 1);
                    // swap the indexes from the item before
                    int? lineItemNumber = line.LineItemNumber;
                    line.RaisePropertyChanged("LineItemNumber");
                    lineAfter.RaisePropertyChanged("LineItemNumber");
                    line.LineItemNumber = lineAfter.LineItemNumber;
                    lineAfter.LineItemNumber = lineItemNumber;
                    range[0].FromRow++;
                    Lines.RaiseOnSelectedRowsChanged(range);

                    Lines.SortBy(new SortCol("lineitemnumber", true));
                    Lines.Refresh();                  

                };
            }
            return _moveDownCommand;

        }

        #endregion
    }
}
