using System.Runtime.CompilerServices;

namespace Xrm
{
    [Imported]
    public class FormSelector
    {
        /// <summary>
        /// A collection of all the form items accessible to the current user
        /// </summary>
        public FormSelectorItems Items;

        /// <summary>
        /// Method to return a reference to the form currently being shown
        /// </summary>
        public FormSelectorItem GetCurrentItem()
        {
            return null;
        }
    }
}
