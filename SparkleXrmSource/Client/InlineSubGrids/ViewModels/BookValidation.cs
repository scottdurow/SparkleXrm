
using SparkleXrm;
using System;
using Xrm.Sdk;
namespace Client.InlineSubGrids.ViewModels
{
    public class BookValidation
    {
        public static ValidationRules PublishDate(ValidationRules rules, object viewModel, object dataContext)
        {
        
            Book self = (Book)viewModel;
            return rules
                    .AddRule("Publish date can't be more than 1 year in the future", delegate(object value)
                    {
                        DateTime publishDate = (DateTime)value;
                        return (publishDate < DateTimeEx.DateAdd(DateInterval.Days, 365,DateTime.Today));

                    });

        }
        public static void Register(ValidationBinder binder)
        {
           
            binder.Register("publishdate", PublishDate);
           
        }
    }
}
