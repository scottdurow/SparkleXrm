
using Client.ContactEditor.Model;
using KnockoutApi;
using Slick;
using SparkleXrm;
using Xrm.Sdk;


namespace Client.ContactEditor.ViewModels
{
    public class ContactValidation
    {
        public static ValidationRules FirstName(ValidationRules rules, object viewModel,object dataContext)
        {

            Contact self = Knockout.UnwrapObservable<Contact>(viewModel);
            return rules
                    .AddRequiredMsg("Enter a first name")
                   .AddRule("Must be less than 200 chars", delegate(object value)
                   {                
                        string valueText = (string)value;
                        return (valueText.Length < 200);
                   })
                   .AddRule("Firstname can't be the same as the lastname", delegate(object value)
                    {

                        bool isValid = true;
                        string lastName = Knockout.UnwrapObservable<string>(self.LastName);
                        if (lastName != null && (string)value == lastName)
                            isValid = false;

                        return isValid;
                    }
               );

        }
        public static ValidationRules BirthDate(ValidationRules rules, object viewModel, object dataContext)
        {
            Contact self = (Contact)viewModel;
            return rules
                    .AddRule("Birthdate can't be in the future", delegate(object value)
                    {
                        DateTime birthdate =(DateTime)value;
                        return (birthdate < DateTime.Today);

                    });

        }
        public static ValidationRules AccountRoleCode(ValidationRules rules, object viewModel, object dataContext)
        {
            return rules
                .AddRule("Account Role is required.", delegate(object value)
                {
                   return (value!=null) && ((OptionSetValue)value).Value!=null;

                });

        }
        public static void Register(ValidationBinder binder)
        {
            binder.Register("firstname", FirstName);
            binder.Register("accountrolecode", AccountRoleCode);
            binder.Register("birthdate", BirthDate);
        }
       
    }
}
