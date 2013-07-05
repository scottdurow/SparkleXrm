// EntityBuiltInAttributes.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class EntityBuiltInAttributes
    {
        [ScriptName("createdon")]
        public object CreatedOn;
        [ScriptName("modifiedon")]
        public object ModifiedOn;
        [ScriptName("createdby")]
        public object CreatedBy;
        [ScriptName("modifiedby")]
        public object ModifiedBy;
        [ScriptName("transactioncurrencyid")]
        public object TransactionCurrencyId;
    }
}
