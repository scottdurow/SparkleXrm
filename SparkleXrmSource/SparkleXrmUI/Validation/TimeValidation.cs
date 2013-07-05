

using System;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace SparkleXrm.Validation
{
    public class TimeValidation 
    {
        static TimeValidation()
        {
            if ((string)Script.Literal("typeof(ko)") != "undefined")
            {
                ValidationRule rule = new ValidationRule();
                rule.Message = "{0} is an invalid time.";
                rule.Validator = Validator;
                ValidationApi.Rules["timeValidation"] = rule;
            }
        }


        public static bool Validator(object val, object otherval)
        {

            DateTime parseDate = DateTimeEx.AddTimeToDate(DateTime.Now, (string)val);

            return parseDate!=null;
        }

        
    }
}
