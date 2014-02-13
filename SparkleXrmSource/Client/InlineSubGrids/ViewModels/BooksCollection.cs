// BooksCollection.cs
//

using Slick;
using SparkleXrm.GridEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Xrm;
using Xrm.Sdk;

namespace Client.InlineSubGrids.ViewModels
{
    /// <summary>
    /// Very simple DataView without any loading/saving from server
    /// </summary>
    public class BooksCollection : DataViewBase,IEnumerable
    {
        private bool _suspendRefresh = false;
        private bool _refreshPending= false;

        private List<Book> Books = new List<Book>();
        public void Suspend()
        {
            _suspendRefresh = true;
            _refreshPending = false;
        }
        public void Unsuspend()
        {
            _suspendRefresh = false;
            if (_refreshPending)
            {
                Refresh();
            }
            _refreshPending = false;
        }
        public override void AddItem(object item)
        {
            ArrayEx.Add(Books,item);
            Refresh();
        }
        public override object GetItem(int index)
        {
            return Books[index];
        }
        public override int GetLength()
        {
            return Books.Count;
        }
        public override void Refresh()
        {
            if (_suspendRefresh)
            {
                _refreshPending = true;
                return;
            }
            OnRowsChangedEventArgs args = new OnRowsChangedEventArgs();
            args.Rows = new List<int>();
            for (int i=0;i<Books.Count;i++)
            {
                args.Rows.Add(i);
            }

            this.OnDataLoaded.Notify(null, null, this);
            this.OnRowsChanged.Notify(args, null, this);
            
        }
        public override PagingInfo GetPagingInfo()
        {
            return null;
        }
        public override void Sort(SortColData sorting)
        {
            if (sorting.SortAsc == false)
            {
                Books.Reverse();
            }
            Books.Sort(delegate(Book a, Book b) { return Entity.SortDelegate(sorting.SortCol.Field, a, b); });

            if (sorting.SortAsc == false)
            {
                Books.Reverse();
            }
        }

        public override void RemoveItem(object id)
        {
            Books.Remove((Book)id);
            Refresh();
        }
        public  void Clear()
        {
            Books.Clear();
            Refresh();
        }
        public override void Reset()
        {
            // Clear selected rows
            this.RaiseOnSelectedRowsChanged(null);
        }
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)Books.GetEnumerator();
        }
    }
}
