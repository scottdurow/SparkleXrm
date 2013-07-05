

using System;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace SparkleXrm.Validation
{
    public class DurationValidation 
    {
        static DurationValidation()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                ValidationRule rule = new ValidationRule();
                rule.Message = "{0} is an invalid duration.";
                rule.Validator = Validator;
                ValidationApi.Rules["durationValidation"] = rule;
            }
        }


        public static bool Validator(object val, object otherval)
        {

            DateTime parseDate = DateTimeEx.AddTimeToDate(DateTime.Now, (string)val);
            return parseDate!=null;
        }
       
    }
}
