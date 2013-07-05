using Client.QuoteLineItemEditor.Model;
using KnockoutApi;
using SparkleXrm;
using System;
using Xrm.Sdk;
namespace Client.QuoteLineItemEditor.ViewModels
{
    public class QuoteDetailValidation
    {
        #region Methods
        public static ValidationRules WriteInProduct(ValidationRules rules, object viewModel, object dataContext)
        {
            
            QuoteDetail self = Knockout.UnwrapObservable<QuoteDetail>(viewModel);
            return rules
                   .AddRule("Select either a product or provide a product description.", delegate(object value)
                   {
                       // Only a productdescription or productid can be selected
                       EntityReference productid = Knockout.UnwrapObservable<EntityReference>(self.ProductId);
                       bool isValid = String.IsNullOrEmpty((string)value) || (productid == null);                

                       return isValid;
                   }
               );

        }
        public static ValidationRules ProductId(ValidationRules rules, object viewModel, object dataContext)
        {

            QuoteDetail self = Knockout.UnwrapObservable<QuoteDetail>(viewModel);
            return rules
                   .AddRule("Select either a product or provide a product description.", delegate(object value)
                   {
                       // Only a productdescription or productid can be selected
                       string productDescription = Knockout.UnwrapObservable<string>(self.ProductDescription);
                       bool isValid = String.IsNullOrEmpty(productDescription) || (value == null);

                       return isValid;
                   }
               );

        }
        public static void Register(ValidationBinder binder)
        {
            binder.Register("productdescription", WriteInProduct);
            binder.Register("productid", ProductId);
        }
        #endregion
    }
}
