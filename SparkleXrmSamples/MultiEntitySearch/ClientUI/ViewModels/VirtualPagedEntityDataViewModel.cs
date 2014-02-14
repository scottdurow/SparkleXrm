// EntityDataView.cs
//

using System;
using System.Collections.Generic;
using Slick;
using Xrm.Sdk;
using jQueryApi;
using Xrm;
using System.Diagnostics;

namespace SparkleXrm.GridEditor
{

    public class VirtualPagedEntityDataViewModel : EntityDataViewModel
    {
        private int _pageLoaded = 0;
        private int _batchSize = 10;
        private bool _suspendRefresh = false;

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
            if ((this.paging.TotalRows > 0) 
                && 
                (index > ((_pageLoaded+1) * paging.PageSize)) 
                && 
                (index <= this.paging.TotalRows))
            {
                _pageLoaded++;
                this.paging.PageNum = _pageLoaded;
                Refresh();
                Script.Literal("console.log({0})", String.Format("{0} {1}",index, _pageLoaded));
            };
            
            //return _data[index + ((int)paging.PageNum * (int)paging.PageSize)];
            return _data[index];
        }

       
        #endregion

        #region IDataView
        public override void Reset()
        {
            base.Reset();
            this._pageLoaded = 0;
            this.GetPagingInfo().PageNum = 0;
            this.GetPagingInfo().TotalRows = 0;
            this.GetPagingInfo().FromRecord = 0;
            this.GetPagingInfo().ToRecord = 0;
            SetPagingOptions(this.GetPagingInfo());


        }
        public override void Refresh()
        {
            //if (_suspendRefresh)
            //    return;

            _suspendRefresh = true;
            // check if we have loaded this page yet
            int firstRowIndex = (int)paging.PageNum * (int)paging.PageSize;
            
            // If we have deleted all rows, we don't want to refresh the grid on the first page
            bool allDataDeleted = (paging.TotalRows == 0) && (DeleteData != null) && (DeleteData.Count > 0);
            List<int> rows = new List<int>();
            if (firstRowIndex >= _pageLoaded)
            {
                this.OnDataLoading.Notify(null, null, null);

                string orderBy = ApplySorting();

                // We need to load the data from the server
                int? fetchPageSize;

                fetchPageSize = this.paging.PageSize ;
                if (String.IsNullOrEmpty(_fetchXml))
                    return;
                string parameterisedFetchXml = String.Format(_fetchXml, fetchPageSize, XmlHelper.Encode(this.paging.extraInfo), this.paging.PageNum + 1, orderBy);
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

                       


                        // Notify
                        DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                        args.From = firstRowIndex;
                        args.To = firstRowIndex+(int)paging.PageSize - 1;

                       
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
                        this.OnDataLoaded.Notify(args, null, null);
                    }
                    catch (Exception ex)
                    {
                        this.ErrorMessage = ex.Message;
                        DataLoadedNotifyEventArgs args = new DataLoadedNotifyEventArgs();
                        args.ErrorMessage = ex.Message;
                        this.OnDataLoaded.Notify(args, null, null);
                    }
                });
            }
            else
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
                
            }

            OnRowsChangedEventArgs refreshArgs = new OnRowsChangedEventArgs();
            refreshArgs.Rows = rows;
            
            this.OnRowsChanged.Notify(refreshArgs, null, this);
            _suspendRefresh = false;
        }
        #endregion

        #region Properties
      
        #endregion


    }

     
}
