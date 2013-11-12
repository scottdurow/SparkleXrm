using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xrm
{
    [Imported]
    public class Context
    {

    #region methods

        /// <summary>
        /// With Microsoft Dynamics CRM 2013 and Microsoft Dynamics CRM Online, the context includes the client object which contains the getClient and getClientState methods to get information about the client
        /// </summary>
        public Client Client;

        /// <summary>
        /// Returns the base URL that was used to access the application
        /// </summary>
        public string GetClientUrl()
        { return null; }

        /// <summary>
        /// Returns a string representing the current Microsoft Office Outlook theme chosen by the user
        /// </summary>
        public string GetCurrentTheme()
        { return null; }

        /// <summary>
        /// Returns the LCID value that represents the base language for the organization
        /// </summary>
        public int GetOrgLcid()
        { return -1; }

        /// <summary>
        /// Returns the unique text value of the organization’s name
        /// </summary>
        public string GetOrgUniqueName()
        { return null; }

        /// <summary>
        /// Returns a dictionary object of key value pairs that represent the query string arguments that were passed to the page
        /// </summary>
        public Dictionary<string, string> GetQueryStringParameters()
        { return null; }

        /// <summary>
        /// Returns the GUID of the SystemUser.Id value for the current user
        /// </summary>
        public string GetUserId()
        { return null; }

        /// <summary>
        /// Returns the LCID value that represents the Microsoft Dynamics CRM Language Pack that is the user selected as their preferred language
        /// </summary>
        public int GetUserLcid()
        { return -1; }

        /// <summary>
        /// Returns the name of the current user
        /// </summary>
        public int GetUserName()
        { return -1; }

        /// <summary>
        /// Returns an array of strings that represent the GUID values of each of the security roles that the user is associated with or any teams that the user is associated with
        /// </summary>
        public string[] GetUserRoles()
        { return null; }

        /// <summary>
        /// Prepends the organization name to the specified path
        /// </summary>
        /// <param name="sPath">A local path to a resource</param>
        /// <returns>The value returned follows this pattern: "/"+ OrgName + sPath</returns>
        public string PrependOrgName(string sPath)
        { return null; }

    #endregion

    }
}
