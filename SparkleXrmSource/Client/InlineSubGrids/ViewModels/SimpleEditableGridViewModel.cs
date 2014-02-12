// SimpleEditableGridViewModel.cs
//

using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Client.InlineSubGrids.ViewModels
{
    /// <summary>
    /// Simple Entity for editable grid
    /// </summary>
    public partial class Book : Entity
    {
       
        public Book() :
            base("book")
        {
          

        }
        [ScriptName("title")]
        public string Title;

        [ScriptName("author")]
        public string Author;

        [ScriptName("publishdate")]
        public DateTime PublishDate;

        [ScriptName("formate")]
        public OptionSetValue Format;

        [ScriptName("price")]
        public Money Proce;

        [ScriptName("numberofcopies")]
        public double NumberOfCopies;

        [ScriptName("outofprint")]
        public bool OutOfPrint;

    }

    /// <summary>
    /// Simple Editable Grid View Model Example
    /// </summary>
    public class SimpleEditableGridViewModel : ViewModelBase
    {
        [ScriptName("Books")]
        public BooksCollection Books = new BooksCollection();

        public SimpleEditableGridViewModel()
        {
            Book book1 = new Book();
            book1.Title = "The Lord of the Rings";
            book1.Author = "J. R. R. Tolkien";
            Books.AddItem(book1);

            Book book2 = new Book();
            book2.Title = "The Hobbit";
            book2.Author = "J. R. R. Tolkien";
            Books.AddItem(book2);
        }
    }
}
