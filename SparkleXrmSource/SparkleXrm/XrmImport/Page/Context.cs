using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Xrm
{
    [Imported]
    public class Context
    {
        public string GetAuthenticationHeader()
        {
            return null;
        }

        public string GetCurrentTheme()
        {
            return null;
        }

        public int GetOrgLcid()
        {
            return -1;
        }

        public string GetOrgUniqueName()
        {
            return null;
        }

        public Dictionary<string, string> GetQueryStringParameters()
        {
            return null;
        }

        public string GetServerUrl()
        {
            return null;
        }

        public string GetUserId()
        {
            return null;
        }

        public int GetUserLcid()
        {
            return -1;
        }

        public string[] GetUserRoles()
        {
            return null;
        }

        public bool IsOutlookClient()
        {
            return false;
        }

        public bool IsOutlookOnline()
        {
            return false;
        }

        public string PrependOrgName()
        {
            return null;
        }
    }
}
