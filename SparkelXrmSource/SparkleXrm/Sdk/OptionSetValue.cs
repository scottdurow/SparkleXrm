// OptionSetValue.cs
//

using System;
using System.Collections.Generic;

namespace Xrm.Sdk
{
    public class OptionSetValue
    {
        public OptionSetValue(int? value)
        {
            this.Value = value;
        }

        public string Name;
        public int? Value;

        public static OptionSetValue Parse(string value)
        {

            if (string.IsNullOrEmpty(value))
                return new OptionSetValue(null);
            else
                return new OptionSetValue(int.Parse(value));

        }
    }
	
}
