using System.Runtime.CompilerServices;

namespace Xrm
{

    [Imported]
    public class AsyncCallback
    {

        /// <summary>
        /// Exposes optional callback functions to be executed after an async operation is completed.  
        /// Save and Refresh functions
        /// </summary>
        /// <param name="SuccessCallbackHandler">A function to call when the operation succeeds</param>
        /// <param name="ErrorCallbackHandler">A function to call when the operation fails</param>
        public void Then(ParameterlessFunctionHandler SuccessCallbackHandler, ErrorCallbackHandler ErrorCallbackHandler)
        { }

    }

}
