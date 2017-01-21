using System.Runtime.CompilerServices;


namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class Money
    {
     
        public Money(decimal value)
        {
            Value = value;
        }
        public decimal Value;
    }
}
