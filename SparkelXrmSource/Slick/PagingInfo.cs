// PagingInfo.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class PagingInfo
    {
        public int? PageSize;
        public int? PageNum;
        public int? TotalRows;
        public int? TotalPages;
        public string extraInfo;
        public int? FromRecord;
        public int? ToRecord;

       
    }
}
