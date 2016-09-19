using Client.ContactEditor.Model;
using KnockoutApi;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace Client.ContactEditor.ViewModels
{
    public partial class ObservableContact
    {
        public Observable<EntityStates> EntityState = Knockout.Observable<EntityStates>();

        private Contact _value;
        [ScriptName("fullname")]
        public Observable<string> FullName = Knockout.Observable<string>();

        [ScriptName("firstname")]
        public Observable<string> FirstName = Knockout.Observable<string>();

        [ScriptName("lastname")]
        public Observable<string> LastName = Knockout.Observable<string>();

        [ScriptName("birthdate")]
        public Observable<DateTime> BirthDate = Knockout.Observable<DateTime>();

        [ScriptName("accountrolecode")]
        public Observable<int> AccountRoleCode = Knockout.Observable<int>();

        [ScriptName("numberofchildren")]
        public Observable<int> NumberOfChildren = Knockout.Observable<int>();

        [ScriptName("transactioncurrencyid")]
        public Observable<EntityReference> TransactionCurrencyId = Knockout.Observable<EntityReference>();

        [ScriptName("parentcustomerid")]
        public Observable<EntityReference> ParentCustomerId = Knockout.Observable<EntityReference>();

        [ScriptName("creditlimit")]
        public Observable<Money> CreditLimit = Knockout.Observable<Money>();

        public void SetValue(Contact value)
        {
            _value = value;
            if (value == null)
            {
                _value = new Contact();
            }
            this.EntityState.SetValue(_value.EntityState);

            FullName.SetValue(_value.FullName);
            FirstName.SetValue(_value.FirstName);
            LastName.SetValue(_value.LastName);
            BirthDate.SetValue(_value.BirthDate);
            AccountRoleCode.SetValue(_value.AccountRoleCode.Value);
            NumberOfChildren.SetValue(_value.NumberOfChildren.Value);
            TransactionCurrencyId.SetValue(_value.TransactionCurrencyId);
            ParentCustomerId.SetValue(_value.ParentCustomerId);
            CreditLimit.SetValue(_value.CreditLimit);
        }

        public Contact Commit()
        {
            _value.FullName = FullName.GetValue();
            _value.FirstName = FirstName.GetValue();
            _value.LastName = LastName.GetValue();
            _value.BirthDate = BirthDate.GetValue();
            _value.AccountRoleCode = AccountRoleCode.GetValue();
            _value.NumberOfChildren = NumberOfChildren.GetValue();
            _value.TransactionCurrencyId = TransactionCurrencyId.GetValue();
            _value.CreditLimit = CreditLimit.GetValue();
            _value.EntityState = EntityStates.Changed;
            _value.ParentCustomerId = ParentCustomerId.GetValue();
            return _value;
        }


    }
}
