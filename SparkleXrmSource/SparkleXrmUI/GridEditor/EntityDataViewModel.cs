// EntityDataView.cs
//

using System;
using System.Collections.Generic;
using Slick;
using Xrm.Sdk;
using jQueryApi;

namespace SparkleXrm.GridEditor
{
   
    public class EntityDataViewModel : DataViewBase
    {

        #region Fields
        
        private Entity[] _rows = new Entity[0];
        private List<Entity> _data;
        private Type _entityType;
        private string _fetchXml = "";
        private List<SortCol> _sortCols = new List<SortCol>();
        private bool _itemAdded = false;
        private bool _lazyLoadPages = true;

        public string ErrorMessage = "";
        public List<Entity> DeleteData;
        //scolson: Event allows viewmodel to save data before cache is cleared.
        public event Action OnBeginClearPageCache;
        #endregion

        #region Constructors
        public EntityDataViewModel(int pageSize, Type entityType, bool lazyLoadPages)
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
        public string FetchXml
        {
            get
            {
                return _fetchXml;
            }
            set
            {
                _fetchXml = value;
                //Refresh(); Removed since an explicit refresh is better to avoid unneccessary ones
            }

        }

        public override object GetItem(int index)
        {

            return _data[index + ((int)paging.PageNum * (int)paging.PageSize)];
        }



        public override void Reset()
        {
            // Reset the cache
            //scolson: use ClearPageCache method to clear data list.
            this.ClearPageCache();
            //this._data = new List<Entity>();
            this.DeleteData = new List<Entity>();

        }
        public void ResetPaging()
        {
            paging.PageNum = 0;
            //this.OnPagingInfoChanged.Notify(GetPagingInfo(), null, null);
        }

        #endregion

        #region IDataView
        public override void Sort(SortColData sorting)
        {


            SortCol col = new SortCol(sorting.SortCol.Field,sorting.SortAsc);
           
            SortBy(col);
        }

        public void SortBy(SortCol col)
        {
            _sortCols.Clear();
            _sortCols.Add(col);
            if (_lazyLoadPages)
            {
                // Clear page cache
                //scolson: Use ClearPageCache routine instead of nulling the data list.
                this.ClearPageCache();
                //_data = new List<Entity>();
                this.paging.extraInfo = "";
                Refresh();
            }
            else
            {
                // From SlickGrid : an extra reversal for descending sort keeps the sort stable
                // (assuming a stable native sort implementation, which isn't true in some cases)
                if (col.Ascending == false)
                {
                    _data.Reverse();
                }
                _data.Sort(delegate(Entity a, Entity b)
                {

                    object l = a.GetAttributeValue(col.AttributeName);
                    object r = b.GetAttributeValue(col.AttributeName);
                    decimal result = 0;

                    string typeName = "";
                    if (l != null)
                        typeName = l.GetType().Name;
                    else if (r != null)
                        typeName = r.GetType().Name;

                    if (l != r)
                    {
                        switch (typeName.ToLowerCase())
                        {
                            case "string":
                                l = l != null ? ((string)l).ToLowerCase() : null;
                                r = r != null ? ((string)r).ToLowerCase() : null;
                                if ((bool)Script.Literal("{0}<{1}", l, r))
                                    result = -1;
                                else
                                    result = 1;
                                break;
                            case "date":
                                if ((bool)Script.Literal("{0}<{1}", l, r))
                                    result = -1;
                                else
                                    result = 1;
                                break;
                            case "number":
                                decimal ln = l != null ? ((decimal)l) : 0;
                                decimal rn = r != null ? ((decimal)r) : 0;
                                result = (ln - rn);
                                break;
                            case "money":
                                decimal lm = l != null ? ((Money)l).Value : 0;
                                decimal rm = r != null ? ((Money)r).Value : 0;
                                result = (lm - rm);
                                break;
                            case "optionsetvalue":
                                int? lo = l != null ? ((OptionSetValue)l).Value : 0;
                                lo = lo != null ? lo : 0;
                                int? ro = r != null ? ((OptionSetValue)r).Value : 0;
                                ro = ro != null ? ro : 0;
                                result = (decimal)(lo - ro);
                                break;
                            case "entityreference":
                                string le = (l != null) && (((EntityReference)l).Name != null) ? ((EntityReference)l).Name : "";
                                string re = r != null && (((EntityReference)r).Name != null) ? ((EntityReference)r).Name : "";
                                if ((bool)Script.Literal("{0}<{1}", le, re))
                                    result = -1;
                                else
                                    result = 1;
                                break;

                        }
                    }
                    return (int)result;
                });
                
                if (col.Ascending == false)
                {
                    _data.Reverse();
                }
            }
        }

        public List<Entity> GetDirtyItems()
        {
            
            List<Entity> dirtyCollection = new List<Entity>();
            // Add new/changed items
            foreach (Entity item in this._data)
            {
                if (item != null && item.EntityState != EntityStates.Unchanged)
                    dirtyCollection.Add(item);
            }

            // Add deleted items
            if (this.DeleteData != null)
            {
                foreach (Entity item in this.DeleteData)
                {
                    if (item.EntityState == EntityStates.Deleted)
                        dirtyCollection.Add(item);
                }
            }

            return dirtyCollection;
        }

        /// <summary>
        /// Check to see if the EntityDataViewModel contains the specified entity.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Contains(Entity Item)
        {
            foreach (Entity value in _data)
            {
                if (Item.LogicalName == value.LogicalName
                    && Item.Id == value.Id)
                {
                    return true;
                }
            }

            return false;
        }


        public override void Refresh()
        {
            // check if we have loaded this page yet
            int firstRowIndex = (int)paging.PageNum * (int)paging.PageSize;
            
            // If we have deleted all rows, we don't want to refresh the grid on the first page
            bool allDataDeleted = (paging.TotalRows == 0) && (DeleteData != null) && (DeleteData.Count > 0);
           
            if (_data[firstRowIndex] == null && !allDataDeleted)
            {
                this.OnDataLoading.Notify(null, null, null);

                string orderBy = ApplySorting();

                // We need to load the data from the server
                int? fetchPageSize;
                if (_lazyLoadPages)
                {
                    fetchPageSize = this.paging.PageSize;
                }
                else
                {
                    fetchPageSize = 1000; // Maximum 1000 records returned in non-lazy load grid
                    
                    this.paging.extraInfo = "";
                    
                }

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
                        args.From = 0;
                        args.To = (int)paging.PageSize - 1;

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

            this.OnRowsChanged.Notify(null, null, this);

        }
        public Func<object, Entity> NewItemFactory;


        public override void RemoveItem(object id)
        {
            if (id != null)
            {
                if (DeleteData == null) DeleteData = new List<Entity>();
                DeleteData.Add((Entity)id);
                _data.Remove((Entity)id);
                this.paging.TotalRows--;
            }
        }

        public override void AddItem(object newItem)
        {
            // If the items are empty - ensure there is a single page
            if (this.paging.TotalPages == 0)
            {
                this.paging.PageNum = 0;
                this.paging.TotalPages = 1;
            }

            Entity item;
            if (NewItemFactory == null)
            {
                item = (Entity)Type.CreateInstance(this._entityType);
                jQuery.Extend(item, newItem);
            }
            else
            {
                item = NewItemFactory(newItem);
            }

            _data[this.paging.TotalRows.Value] = ((Entity)item);


            // Do we need a new page?
            this._itemAdded = true;

            int lastPageSize = (paging.TotalRows.Value % paging.PageSize.Value);
            if (lastPageSize == paging.PageSize)
            {
                // Add a new page
                this.paging.TotalPages++;
                this.paging.PageNum=this.paging.TotalPages-1;
                
               
                
            }
            else
            {
                // Ensure the last page is loaded
                this.paging.PageNum = this.paging.TotalPages-1;
                this.paging.FromRecord = (this.paging.PageNum * this.paging.PageSize)+1;
                this.paging.ToRecord = this.paging.TotalRows+1;
                this.paging.TotalRows++;
            }

            item.RaisePropertyChanged(null);
            this.Refresh();
            
           
        }

        private string ApplySorting()
        {
            // Take the sorting and insert into the fetchxml
            string orderBy = string.Empty;
            foreach (SortCol col in _sortCols)
            {
                orderBy = orderBy + String.Format(@"<order attribute=""{0}"" descending=""{1}"" />", col.AttributeName, !col.Ascending ? "true" : "false");

            }
            return orderBy;
        }

        private void ClearPageCache()
        {
            //scolson: call any event handlers that need to take action before clearing the cache
            if (this.OnBeginClearPageCache != null)
            {
                this.OnBeginClearPageCache();
            }
                
            _data = new List<Entity>();
        }


        #endregion

        #region Properties
        public List<Entity> Data
        {
            get
            {
                return _data;
            }
        }
        #endregion


    }

     
}
