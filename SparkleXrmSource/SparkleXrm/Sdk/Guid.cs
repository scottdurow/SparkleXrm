// Guid.cs
//

using System;
using System.Collections.Generic;

namespace Xrm.Sdk
{
    public class Guid
    {
        public static Guid Empty = new Guid("00000000-0000-0000-0000-000000000000");
        public Guid(string Value)
        {
            this.Value = Value;
        }
        public string Value;
        public override string ToString()
        {
            return Value;
        }
    }
}
