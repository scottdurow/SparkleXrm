
using KnockoutApi;
using Slick;
using SparkleXrm.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SparkleXrm
{
   [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
   public class ValidationOptions
   {

      
    public bool registerExtenders = true;
    public bool decorateElement = true;
    public string errorClass = "error";
    public string errorElementClass = "error";
    public bool messagesOnModified = true;
    public bool insertMessages = true;
    public bool parseInputAttributes = true;
    public string messageTemplate = null;

   }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("ko.validation")]
    public class ValidationApi
    {
        public static Dictionary Rules;
        public static ValidationErrors Group(object viewModel)
        {
            return null;
        }
        public static void Configure(ValidationOptions options)
        {

        }
        public static void RegisterExtenders()
        {

        }
        public static void MakeBindingHandlerValidatable(string name)
        {

        }


    }
    public delegate ValidationRules ValidationRuleDelegate(ValidationRules rules, object viewModel, object dataContext);

    public class ValidationRules
    {

       
        public static bool AreValid(object[] fields)
        {
            bool valid = true;
            foreach (object field in fields)
            {
                valid = valid && ((IValidatedObservable)field).IsValid();
                if (!valid) break;
            }
            return valid;
        }

        public static ValidationRules CreateRules()
        {
            return new ValidationRules();
        }
        public void Register(object model)
        {
            ((Observable<object>)model).Extend((Dictionary)(object)this);
        }
        public ValidationRules AddRequired()
        {
            Dictionary.GetDictionary(this)["required"] = true;
            return this;
        }
        public ValidationRules AddRequiredMsg(string message)
        {
            Dictionary.GetDictionary(this)["required"] = new ValidationMessage(message); 
            return this;
        }
        public ValidationRules AddRule(string message, Func<object,bool> validator)
        {
            AnonymousRule rule = new AnonymousRule();
            List<AnonymousRule> anonRules = (List<AnonymousRule>) Dictionary.GetDictionary(this)["validation"];
            if (anonRules == null)
            {
                anonRules = new List<AnonymousRule>();
                Dictionary.GetDictionary(this)["validation"] = anonRules;
            }
            rule.message = message;
            rule.validator = validator;
            anonRules.Add(rule);
         
            return this;
        }

        public ValidationRules AddPattern(string message, string pattern)
        {
            PatternValidation patternOptions = new PatternValidation();
            patternOptions.Message = message;
            patternOptions.Pattern = pattern;
            Dictionary.GetDictionary(this)["pattern"] = Dictionary.GetDictionary(patternOptions);
            return this;
        }
        public ValidationRules AddCustom(string type, object options)
        {
            Dictionary.GetDictionary(this)[type] = options;
            return this;
        }
        public static GridValidatorDelegate ConvertToGridValidation(ValidationRuleDelegate ruleDelegate)
        {
            GridValidatorDelegate validationFunction =  delegate(object value, object item)
                {
                    ValidationRules rules = new ValidationRules();
                    rules = ruleDelegate(rules,item, null);
                    ValidationResult result = new ValidationResult();
                    result.Valid = true;
                    Dictionary validationRules = Dictionary.GetDictionary(rules);
                    foreach (string key in validationRules.Keys)
                    {
                        // Find the ko validation rule
                        if (ValidationApi.Rules.ContainsKey(key))
                        {
                            ValidationRule targetRule = (ValidationRule)ValidationApi.Rules[key];
                            ValidationRule sourceRule = (ValidationRule)validationRules[key];
                            result.Valid = targetRule.Validator(value, sourceRule.Params==null ? targetRule.Params : sourceRule.Params);
                            result.Message = String.IsNullOrEmpty(targetRule.Message) ? sourceRule.Message : targetRule.Message;
                        }
                        else if (key == "validation")
                        {
                            // Anon rule - can be either a single or array - we assume it's an array created by the ValidationRules class
                            List<AnonymousRule> anonRules = (List<AnonymousRule>)validationRules[key];
                            foreach (AnonymousRule rule in anonRules)
                            {
                                result.Valid = rule.validator(value);
                                result.Message = rule.message;
                                if (!result.Valid)
                                    break;
                            }

                        }
                        if (!result.Valid)
                            break;
                    }
                   
                    return result;
                };

            return validationFunction;

        }
    }
   public class AnonymousRule
   {
       public Func<object,bool> validator;
       public string message;
   }


   public class ValidationMessage
   {
        public ValidationMessage(string message)
        {
            this.message = message;
        }
       public string message;
   }
   
   [ScriptName("Object")]
   [Imported]
   [IgnoreNamespace]
   public class PatternValidation
   {
       public string Message;
        [ScriptName("params")]
       public string Pattern;
   }

   [ScriptName("Object")]
   [Imported]
   [IgnoreNamespace]
   public interface IValidatedObservable
   {
        bool IsValid();
        bool IsAnyMessageShown();
        [IntrinsicProperty]
        IErrorsCollection Errors { get; set; }
   }

   [ScriptName("Object")]
   [Imported]
   [IgnoreNamespace]
   public interface IErrorsCollection
   {
       void ShowAllMessages(bool visible);
   }
   [Imported]
   [IgnoreNamespace]
   [ScriptName("ko.validatedObservable")]
   public static class ValidatedObservableFactory
   {
       //[ScriptName("")]
       //public static Observable<T> ValidatedObservable<T>(object viewModel)
       //{
       //    return null;
       //}

       [ScriptName("")]
       public static object ValidatedObservable(object viewModel)
       {
           return null;
       }
       


   }

   [Imported]
   public class IValidatedViewModel
   {
       public  ValidationErrors Errors;
   }
   [Imported]
   public class ValidationErrors
   {
       public void ShowAllMessages(bool value)
       {

       }
   }

   public class ValidationBinder
   {
      
       public virtual void Register(string fieldName, ValidationRuleDelegate rule)
       {

       }

   }

   public class DataViewValidationBinder : ValidationBinder
   {
     
       private Dictionary<string, ValidationRuleDelegate> _validationRules = new Dictionary<string, ValidationRuleDelegate>();
       public DataViewValidationBinder()
       {
           
       }
      

       public override void Register(string fieldName, ValidationRuleDelegate rule)
       {
           _validationRules[fieldName] = rule;
       }

       public GridValidatorDelegate GridValidationIndexer(string attributeLogicalName)
       {
           if (_validationRules[attributeLogicalName] != null)
           {
               return ValidationRules.ConvertToGridValidation(_validationRules[attributeLogicalName]);
           }
           else
           {
               return null;

           }
       }

   }
   public class ObservableValidationBinder : ValidationBinder
   {
       private object _observable;

       public ObservableValidationBinder(object observable)
       {
           _observable = observable;

       }
    

       public override void Register(string fieldName, ValidationRuleDelegate ruleDelegate)
       {     
       
           // Get observable field
           Dictionary viewModel = Knockout.UnwrapObservable<Dictionary>(_observable);
           object observableField = viewModel[fieldName];

           // Get the rule
           object rule = ruleDelegate(ValidationRules.CreateRules(), _observable, null);

           // Register the Validation rule on the observable
           ((Observable<object>)observableField).Extend((Dictionary)(object)rule);
        
       }

   }
  
}
