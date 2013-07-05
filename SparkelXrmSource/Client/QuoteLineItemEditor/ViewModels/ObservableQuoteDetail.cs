
using Client.QuoteLineItemEditor.Model;
using KnockoutApi;
using System.Html;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Client.QuoteLineItemEditor.ViewModels
{
    /// <summary>
    /// Observable entity class for QuoteDetail
    /// </summary>
    public class ObservableQuoteDetail
    {
        #region Fields
        private bool _isSetting = false;

        public QuoteDetail InnerQuoteDetail;

        [ScriptName("requestdeliveryby")]
        public Observable<DateTime> RequestDeliveryBy = Knockout.Observable<DateTime>();

        [ScriptName("salesrepid")]
        public Observable<EntityReference> SalesRepId = Knockout.Observable<EntityReference>();

        #endregion

        #region Constructor
        public ObservableQuoteDetail()
        {
            
            // Wire up onchange handlers to immediately commit the value to the model
            RequestDeliveryBy.Subscribe(delegate(DateTime value) {
                OnValueChange("requestdeliveryby", value);
            });

            SalesRepId.Subscribe(delegate(EntityReference value) {
                OnValueChange("salesrepid", value);
            });
        }

        #endregion

        #region Methods
        public void OnValueChange(string attribute,object newValue)
        {
            if (!_isSetting)
            {
                Window.SetTimeout(delegate() { this.Commit(); }, 0);
            }
        }
        public void SetValue(QuoteDetail value)
        {
            _isSetting = true;
            InnerQuoteDetail = value==null ? new QuoteDetail() : value;
            this.RequestDeliveryBy.SetValue(InnerQuoteDetail.RequestDeliveryBy);
            this.SalesRepId.SetValue(InnerQuoteDetail.SalesRepId);
            _isSetting = false;
        }

        public QuoteDetail Commit()
        {
            if (InnerQuoteDetail == null) return null;
            InnerQuoteDetail.RequestDeliveryBy = Knockout.UnwrapObservable<DateTime>(RequestDeliveryBy);
            InnerQuoteDetail.SalesRepId = Knockout.UnwrapObservable<EntityReference>(SalesRepId);
            InnerQuoteDetail.RaisePropertyChanged("");
            return InnerQuoteDetail;
        }
        #endregion
    }
}
