// BooksCollection.cs
//

using Slick;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;

namespace Client.InlineSubGrids.ViewModels
{
    /// <summary>
    /// Very simple DataView without any loading/saving from server
    /// </summary>
    public class BooksCollection : DataViewBase
    {
        private List<Book> Books = new List<Book>();
        public override void AddItem(object item)
        {
            Books.Add((Book)item);
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

            OnRowsChangedEventArgs args = new OnRowsChangedEventArgs();
            args.Rows = new List<int>();
            for (int i=0;i<Books.Count;i++)
            {
                args.Rows.Add(i);
            }
         
            this.OnRowsChanged.Notify(args, null, this);
            
        }
        
    }
}
