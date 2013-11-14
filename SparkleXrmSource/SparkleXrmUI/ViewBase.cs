// ViewBase.cs
//

using jQueryApi;
using KnockoutApi;
using Xrm.Sdk;

namespace SparkleXrm
{
    public class ViewBase
    {
        // LED: 9/24/2013 Add support to ensure the template is not loaded twice and the ValidationApi.RegisterExtenders is not performed multiple times.
        private static bool _templateLoaded = false;

        // LED: 9/24/2013 Allow the caller to override the default path to the form.templates.htm file if a custom template is needed.
        public static string sparkleXrmTemplatePath = "../../sparkle_/html/form.templates.htm";

        public static void RegisterViewModel(object viewModel)
        {
            jQuery.OnDocumentReady(delegate()
            {
                if (!_templateLoaded)
                {
                    jQuery.Get("../../sparkle_/html/form.templates.htm", delegate(object template)
                   {
                       jQuery.Select("body").Append((string)template);

                       ValidationApi.RegisterExtenders();

                       // Init settings
                       OrganizationServiceProxy.GetUserSettings();

                       // set the flag so we do not perform the above steps again.
                       _templateLoaded = true;

                       Knockout.ApplyBindings(viewModel);
                   });
                }
                else
                {
                    Knockout.ApplyBindings(viewModel);
                }
            });
        }
    }
}
