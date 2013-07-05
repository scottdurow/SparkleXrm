// QuoteProduct.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Client.QuoteLineItemEditor.Model
{
    public class QuoteDetail: Entity
    {
        public QuoteDetail()
            : base("quotedetail")
        {
            this._metaData["quantity"] = AttributeTypes.Decimal_;
            this._metaData["lineitemnumber"] = AttributeTypes.Int_;
            PropertyChanged += QuoteProduct_PropertyChanged;
        }

        void QuoteProduct_PropertyChanged(object sender, Xrm.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Quantity != null && PricePerUnit != null)
            {
                this.ExtendedAmount = new Money(Quantity.Value * PricePerUnit.Value);
            }
            this.IsProductOverridden = !String.IsNullOrEmpty(ProductDescription);
        }

        [ScriptName("quotedetailid")]
        public Guid QuoteDetailId;

        [ScriptName("quoteid")]
        public EntityReference QuoteId;

        [ScriptName("lineitemnumber")]
        public int? LineItemNumber;

        [ScriptName("isproductoverridden")]
        public bool IsProductOverridden;

        // Write In Product
        [ScriptName("productdescription")]
        public string ProductDescription;

        // Existing Product
        [ScriptName("productid")]
        public EntityReference ProductId;

        // Unit
        [ScriptName("uomid")]
        public EntityReference UoMId;
        
        // Pricing
        [ScriptName("ispriceoverridden")]
        public bool IsPriceOverridden;

        [ScriptName("priceperunit")]
        public Money PricePerUnit;

        [ScriptName("quantity")]
        public decimal? Quantity;

        [ScriptName("extendedamount")]
        public Money ExtendedAmount;

        [ScriptName("transactioncurrencyid")]
        public EntityReference TransactionCurrencyId;

        [ScriptName("requestdeliveryby")]
        public DateTime RequestDeliveryBy;

        [ScriptName("salesrepid")]
        public EntityReference SalesRepId;

    }
}
