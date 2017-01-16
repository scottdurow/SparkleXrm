// LocalisationTests.cs
//


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Testing;
using Xrm;

namespace SparkleXrm.UnitTests
{
    public class LocalisationTests
    {
        [PreserveCase]
        public bool NumberParse()
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.DecimalSymbol = ",";
            format.NumberSepartor = ".";
            Script.Literal("debugger");
            Number value1 = NumberEx.Parse("22,10", format);
            Assert.AreEqual(value1, 22.10);

            Number value2 = NumberEx.Parse("1.022,10", format);
            Assert.AreEqual(value2, 1022.10);

            return true;
        }
    }
}
