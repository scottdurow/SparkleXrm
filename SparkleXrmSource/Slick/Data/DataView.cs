// DataView.cs
//

using Slick.Data.Aggregators;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick.Data
{
    [Imported]
    [ScriptName("DataView")]
    public class DataView //: IDataView, IDataProvider
    {
        /*
        public Event OnRowsChanged = new Event();
        public Event OnPagingInfoChanged = new Event();
        public Event OnDataLoading = new Event();
        public Event OnDataLoaded = new Event();
        public event OnGetItemMetaData OnGetItemMetaData;
        protected PagingInfo paging = new PagingInfo();
        public event Action OnSelectedRowsChanged;
        protected SelectedRange[] _selectedRows;
        public DataViewValidationBinder ValidationBinder = new DataViewValidationBinder();

        public void RaiseOnSelectedRowsChanged(SelectedRange[] rows)
        {
            this._selectedRows = rows;
            if (this.OnSelectedRowsChanged != null)
                this.OnSelectedRowsChanged();
        }
        public SelectedRange[] GetSelectedRows()
        {
            if (_selectedRows == null)
                _selectedRows = new SelectedRange[0];

            return _selectedRows;
        }
        public static List<int> RangesToRows(SelectedRange[] ranges)
        {
            List<int> rows = new List<int>();
            for (int i = 0; i < ranges.Length; i++)
            {
                for (int j = ranges[i].FromRow.Value; j <= ranges[i].ToRow; j++)
                {
                    rows.Add(j);
                }
            }
            return rows;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            OnRowsChanged.Notify(null, null, null);
        }

        public virtual PagingInfo GetPagingInfo()
        {
            return paging;
        }

        public virtual void SetPagingOptions(PagingInfo p)
        {
            if (p.PageSize != null)
            {
                this.paging.PageSize = p.PageSize;
                this.paging.PageNum = this.paging.PageSize != 0 ? (int)Math.Min(this.paging.PageNum, Math.Max(0, Math.Ceil(this.paging.TotalRows / this.paging.PageSize) - 1)) : 0;
            }

            if (p.PageNum != null)
            {
                this.paging.PageNum = (int)Math.Min(p.PageNum, Math.Max(0, Math.Ceil(this.paging.TotalRows / this.paging.PageSize) - 1));
            }


            OnPagingInfoChanged.Notify(this.paging, null, this);

            Refresh();
        }

        public virtual void Refresh()
        {

        }
        public virtual void Reset()
        {

        }
        public virtual void InsertItem(int insertBefore, object item)
        {

        }

        public virtual void AddItem(object item)
        {

        }
        public virtual void RemoveItem(object id)
        {

        }
        public virtual int GetLength()
        {
            return (int)Math.Min(paging.PageSize, paging.ToRecord - paging.FromRecord + 1);
        }


        public virtual object GetItem(int index)
        {
            return null;
        }

        public virtual ItemMetaData GetItemMetadata(int i)
        {
            if (OnGetItemMetaData != null)
                return OnGetItemMetaData(GetItem(i));
            else
                return null;
        }

        public virtual void Sort(SortColData sorting)
        {

        }

        public virtual Func<string, GridValidatorDelegate> GridValidationIndexer()
        {
            return ValidationBinder.GridValidationIndexer;
        }
         * */
        public DataView(DataViewOptions options)
        {

        }
        public void SetGrouping(List<GroupingDefinition> list)
        {
         
        }

        public void SetItems(object data)
        {
           
        }

        public void BeginUpdate()
        {
          
        }

        public void EndUpdate()
        {
            
        }

        public void SetFilter(FilterDelegate MyFilter)
        {
            
        }

        public void SetFilterArgs(Dictionary<string, object> filterArguments)
        {
            
        }

        public void Sort(Func<object,object,int> Comparer, bool p)
        {
           
        }
        public Event OnRowsChanged = new Event();
    }
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class DataViewOptions
    {

       public IMetadataProvider GroupItemMetadataProvider;
       public bool? InlineFilters;
             

    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class GroupingDefinition
    {
        public string Getter;
        public GroupingFormatterDelegate Formatter;
        public List<Aggregator> Aggregators;
        public bool AggregateCollapsed;
        public bool Collapsed;
    }
    public delegate string GroupingFormatterDelegate(Group group);
    public delegate bool FilterDelegate(object item, object args);

   
}
