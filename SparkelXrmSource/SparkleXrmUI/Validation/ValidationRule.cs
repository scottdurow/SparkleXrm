// ValidationRule.cs
//

using System;
using System.Runtime.CompilerServices;

namespace SparkleXrm.Validation
{
    public delegate bool KoValidatorDelegate(object val,object otherval);

    [IgnoreNamespace]
    [Imported]
    [ScriptName("Object")]
    public class ValidationRule
    {
       public KoValidatorDelegate Validator;
       [IntrinsicProperty]
       public string Message
       {
           get
           {
               return "";
           }
           set
           {

           }
       }
       [IntrinsicProperty]
       public object Params
       {
           get
           {
               return "";
           }
           set
           {
           }
       }
    }
}
