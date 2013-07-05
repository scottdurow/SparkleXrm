// ViewBase.cs
//

using jQueryApi;
using KnockoutApi;
using Xrm.Sdk;

namespace SparkleXrm
{
    public class ViewBase
    {
        public static void RegisterViewModel(object viewModel)
        {
            jQuery.OnDocumentReady(delegate()
            {
                jQuery.Get("../../sparkle_/html/form.templates.htm", delegate(object template)
               {
                   jQuery.Select("body").Append((string)template);

                   ValidationApi.RegisterExtenders();

                   // Init settings
                   OrganizationServiceProxy.GetUserSettings();
                   Knockout.ApplyBindings(viewModel);
               });
            });
        }
       
    }
}
