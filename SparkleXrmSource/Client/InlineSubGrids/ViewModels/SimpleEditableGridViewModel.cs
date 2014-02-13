// SimpleEditableGridViewModel.cs
//

using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

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

        [ScriptName("format")]
        public OptionSetValue Format;

        [ScriptName("price")]
        public Money Price;

        [ScriptName("numberofcopies")]
        public double NumberOfCopies;

        [ScriptName("outofprint")]
        public bool OutOfPrint;

        [ScriptName("language")]
        public EntityReference Language;

        [ScriptName("audiolength")]
        public int AudioLength;

        [ScriptName("starttime")]
        public DateTime StartTime;

    }

    /// <summary>
    /// Simple Editable Grid View Model Example
    /// </summary>
    public class SimpleEditableGridViewModel : ViewModelBase
    {
        private List<Entity> _languages;

        [ScriptName("Books")]
        public BooksCollection Books = new BooksCollection();

        public SimpleEditableGridViewModel()
        {

            BookValidation.Register(Books.ValidationBinder);
        }
        public void LoadBooks()
        {
            Books.Suspend();
            int offset = Math.Random() * 10;
            int count = Math.Random() * 10;
            for (int i = offset; i <(count+offset); i++)
            {
                Book book1 = new Book();
                book1.Title = "The Lord of the Rings " + i.ToLocaleString();
                book1.Author = "J. R. R. Tolkien";
                book1.PublishDate = new DateTime(1954, 7, 29);
                book1.Format = new OptionSetValue(1);
                book1.Format.Name = "Paper Back";
                book1.Price = new Money(12.99);
                Books.AddItem(book1);

                Book book2 = new Book();
                book2.Title = "The Hobbit " + i.ToLocaleString();
                book2.Author = "J. R. R. Tolkien";
                book2.PublishDate = new DateTime(1932, 9, 21);
                book2.Format = new OptionSetValue(2);
                book2.Format.Name = "Hard Back";
                book2.Price = new Money(9.99);
                Books.AddItem(book2);
            }
            Books.Unsuspend();
        }
        public List<OptionSetItem> GetFormats(object viewModel)
        {
            OptionSetItem paperBack = new OptionSetItem();
            paperBack.Name = "Paper Back";
            paperBack.Value = 1;

            OptionSetItem hardBack = new OptionSetItem();
            hardBack.Name = "Hard Back";
            hardBack.Value = 2;
            return new List<OptionSetItem>(paperBack, hardBack);
        }

        public void GetLanguages(string term, Action<EntityCollection> callback)
        {
            // Get the languages
            if (_languages == null)
            {
                _languages = new List<Entity>();
           

                AddLanguage("00000000-0000-0000-0000-000000000001", "English");
                AddLanguage("00000000-0000-0000-0000-000000000002", "French");
                AddLanguage("00000000-0000-0000-0000-000000000003", "German");
                AddLanguage("00000000-0000-0000-0000-000000000004", "Japanese");
                AddLanguage("00000000-0000-0000-0000-000000000005", "Hungarian");


            }

            List<Entity> entities = new List<Entity>();
            foreach (Entity language in _languages)
            {
                if (language.GetAttributeValueString("name").ToLowerCase().IndexOf(term.ToLowerCase()) > -1)
                {
                    entities.Add(language);
                }
            }
            EntityCollection results = new EntityCollection(entities);
            callback(results);


            
        }

        private void AddLanguage(string id, string name)
        {
            Entity language = new Entity("language");
            language.Id = id;
            language.SetAttributeValue("name", name);
            _languages.Add(language);
        }

        public void ResetCommand()
        {
            bool confirmed = Script.Confirm(String.Format("Are you sure you want to reset the grid? This will loose any values you have edited."));
            if (!confirmed)
                return;

            Books.Suspend();

            Books.Clear();
            LoadBooks();

            Books.Reset();
            Books.Refresh();
            
        }
    }
}
