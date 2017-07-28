// PagingInfo.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class PagingInfo
    {
        [PreserveCase]
        public int Count;
        [PreserveCase]
        public int PageNumber;
        [PreserveCase]
        public string PagingCookie;
        [PreserveCase]
        public bool ReturnTotalRecordCount;

        public string Serialise()
        {
            return "<a:Count>" + Count.ToString() + "</a:Count>" +
                "<a:PageNumber>" + PageNumber.ToString() + "</a:PageNumber>" +
                "<a:PageNumber>" + ReturnTotalRecordCount.ToString() + "</a:PageNumber>";
        }
    }
}
