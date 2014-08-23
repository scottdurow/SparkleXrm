// EntityDataView.cs
//

using System;
using System.Collections.Generic;
using Slick;
using Xrm.Sdk;
using jQueryApi;
using Xrm;
using System.Diagnostics;
using System.Html;

namespace SparkleXrm.GridEditor
{
    
    public class VirtualPagedEntityDataViewModel : EntityDataViewModel
    {
        private const int REQUESTED = 0;
        private const int LOADING = 1;
        private const int LOADED = 2;

       
        private int _batchSize = 10;
        private Stack<int> _pendingRefresh = new Stack<int>();
        private List<int> _pagesLoaded = new List<int>();

        #region Constructors
        public VirtualPagedEntityDataViewModel(int pageSize, Type entityType, bool lazyLoadPages)
            : base(pageSize, entityType, lazyLoadPages)
        {
            // Create data for server load
         
            _entityType = entityType;
            _lazyLoadPages = lazyLoadPages;

            this._data = new List<Entity>();
            paging.PageSize = pageSize;
            paging.PageNum = 0;
            paging.TotalPages = 0;
            paging.TotalRows = 0;
            paging.FromRecord = 0;
            paging.ToRecord = 0;
        }
        #endregion

        #region IDataProvider
        public override int GetLength()
        {
            return this.paging.TotalRows.Value; 
        }
        public override object GetItem(int index)
        {
            // Check if we have reached the maximum of our loaded pages
            int requestedPage = Math.Floor(index / paging.PageSize);
            
            if ((this.paging.TotalRows > 0)
                &&
                (_pagesLoaded[requestedPage] ==null)
                )
            {
             
                if (_suspendRefresh)
                {
                    bool singlePageIncrement = true;
                    if (_pendingRefresh.Count > 0)
                    {
                        singlePageIncrement = Math.Abs(_pendingRefresh.Peek() - requestedPage) >3;
                    }

                    // Push a pending refresh so we don't clog up the data binding
                    if (!_pendingRefresh.Contains(requestedPage))
                        _pendingRefresh.Push(requestedPage);
                }
                else
                {
                    this.paging.PageNum = requestedPage;
                    Refresh();
                }
            };
                
            return _data[index];
        }

       
        #endregion

        #region IDataView
        public override void Reset()
        {
            base.Reset();
           
            this._pagesLoaded = new List<int>();
            this.GetPagingInfo().PageNum = 0;
            this.GetPagingInfo().TotalRows = 0;
            this.GetPagingInfo().FromRecord = 0;
            this.GetPagingInfo().ToRecord = 0;
            SetPagingOptions(this.GetPagingInfo());


        }
        public override void Refresh()
        {
            int requestedPage = paging.PageNum.Value;
            int pageLoadState = _pagesLoaded[requestedPage];

            if (_suspendRefresh == true)
            {
                return;
            }
           
            _suspendRefresh = true;
            
            // If we have deleted all rows, we don't want to refresh the grid on the first page
            bool allDataDeleted = (paging.TotalRows == 0) && (DeleteData != null) && (DeleteData.Count > 0);
            List<int> rows = new List<int>();
            int firstRowIndex = requestedPage * (int)paging.PageSize;

            if (pageLoadState!=LOADING && pageLoadState!=LOADED)
            {
                if (String.IsNullOrEmpty(_fetchXml))
                {
                    _suspendRefresh = false;
                    return;
                }
                _pagesLoaded[requestedPage] = LOADING;

                this.OnDataLoading.Notify(null, null, null);

                string orderBy = ApplySorting();

                // We need to load the data from the server
                int? fetchPageSize;

                fetchPageSize = this.paging.PageSize ;


                string parameterisedFetchXml = String.Format(_fetchXml, fetchPageSize, XmlHelper.Encode(this.paging.extraInfo), requestedPage + 1, orderBy);
                
                OrganizationServiceProxy.BeginRetrieveMultiple(parameterisedFetchXml, delegate(object result)
                {    
                    try
                    {
                        EntityCollection results = OrganizationServiceProxy.EndRetrieveMultiple(result, _entityType);

                        // Set data
                        int i = firstRowIndex;
                        if (_lazyLoadPages)
                        {
                            // We are returning just one page - so add it into the data
                            foreach (Entity e in results.Entities)
                            {
                                _data[i] = (Entity)e;
                                ArrayEx.Add(rows,i);
                                i = i + 1;
                            }
                        }
                        else
                        {
                            // We are returning all results in one go
                            _data = results.Entities.Items();
                        }

                        _pagesLoaded[requestedPage] = LOADED;
                        
                        this.paging.TotalRows = results.TotalRecordCount;
                        this.paging.extraInfo = results.PagingCookie;
                        this.paging.FromRecord = firstRowIndex + 1;
                        this.paging.TotalPages = Math.Ceil(results.TotalRecordCount / this.paging.PageSize);
                        this.paging.ToRecord = Math.Min(results.TotalRecordCount, firstRowIndex + paging.PageSize);
                        if (this._itemAdded)
                        {
                            this.paging.TotalRows++;
                            this.paging.ToRecord++;
                            this._itemAdded = false;
                        }
                        this.OnPagingInfoChanged.Notify(GetPagingInfo(), null, null);
                        // Notify
                        DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                        args.From = firstRowIndex;
                        args.To = firstRowIndex + (int)paging.PageSize - 1;
                        this.OnDataLoaded.Notify(args, null, null);
                        FinishSuspend();
                       
                    }
                    catch (Exception ex)
                    {
                        // Issue #40 - Check for Quick Find Limit
                        bool quickFindLimit = ex.Message.IndexOf("QuickFindQueryRecordLimit") > -1;
                        _pagesLoaded[requestedPage] = LOADED;
                        this.paging.TotalRows = 5001;
                        this.OnPagingInfoChanged.Notify(GetPagingInfo(), null, null);
                        DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                        if (!quickFindLimit)
                        {
                            this.ErrorMessage = ex.Message;
                            args.ErrorMessage = ex.Message;
                        }
                       
                        this.OnDataLoaded.Notify(args, null, null);
                        //_pendingRefresh = false;
                        FinishSuspend();
                    }
                });
            }
            else if (pageLoadState == LOADED)
            {
                // We already have the data
                DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                args.From = 0;
                args.To = (int)paging.PageSize - 1;
                this.paging.FromRecord = firstRowIndex + 1;
                this.paging.ToRecord = Math.Min(this.paging.TotalRows, firstRowIndex + paging.PageSize);

                this.OnPagingInfoChanged.Notify(GetPagingInfo(), null, null);
                this.OnDataLoaded.Notify(args, null, null);
                this._itemAdded = false;
                FinishSuspend();
               
            }
        }

        private void FinishSuspend()
        {
                _suspendRefresh = false;

                if (_pendingRefresh.Count>0)
                {
                    this.paging.PageNum = _pendingRefresh.Pop();
                    Refresh();
                }
        }
        #endregion

    }

     
}
