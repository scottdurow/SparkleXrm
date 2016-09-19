using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.ComponentModel;
using System;
using KnockoutApi;

namespace Client.ContactEditor.Model
{
    public class Contact : Entity
    {
        public Contact()
            : base("contact")
        {
            this._metaData["numberofchildren"] = AttributeTypes.Int_;
            this._metaData["creditlimit"] = AttributeTypes.Money;
        }
        [ScriptName("contactid")]
        public Guid ContactId;

        [ScriptName("fullname")]
        public string FullName;

        [ScriptName("firstname")]
        public string FirstName;

        [ScriptName("lastname")]
        public string LastName;

        [ScriptName("birthdate")]
        public DateTime BirthDate;

        [ScriptName("accountrolecode")]
        public int? AccountRoleCode;

        [ScriptName("numberofchildren")]
        public int? NumberOfChildren;

        [ScriptName("transactioncurrencyid")]
        public EntityReference TransactionCurrencyId;

        [ScriptName("creditlimit")]
        public Money CreditLimit;

        [ScriptName("entityimage_url")]
        public string EntityImage_Url;

        [ScriptName("ownerid")]
        public EntityReference OwnerId;

        [ScriptName("parentcustomerid")]
        public EntityReference ParentCustomerId;

    }

}

