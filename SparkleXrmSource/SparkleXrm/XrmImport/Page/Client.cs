using System.Runtime.CompilerServices;

namespace Xrm
{

    /// <summary>
    /// With Microsoft Dynamics CRM 2013 and Microsoft Dynamics CRM Online, the context includes the client object which contains the getClient and getClientState methods to get information about the client
    /// </summary>
    [Imported]
    public class Client
    {

        /// <summary>
        /// CRM2013 Only: Returns a value to indicate which client the script is executing in
        /// </summary>
        /// <returns>
        /// The values returned are:
        ///     Browser: Web
        ///     Outlook: Outlook
        ///     Mobile: Mobile
        /// </returns>
        /// <remarks>Use this instead of the deprecated isOutlookClient method.</remarks>
        public ClientType GetClient()
        { return ClientType.Web; }

        /// <summary>
        /// CRM2013 Only: Returns a value to indicate the state of the client
        /// </summary>
        /// <returns>
        /// The values returned are:
        ///     Online: Web, Outlook, Mobile
        ///     Offline: Outlook, Mobile
        /// </returns>
        /// <remarks>Use this instead of the deprecated isOutlookOnline method.</remarks>
        public ClientStateType GetClientState()
        { return ClientStateType.Online; }

    }

}