// IWebAPIOrganizationResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public interface IWebAPIOrganizationResponse
    {
        void DeserialiseWebApi(Dictionary<string,object> response);
    }
}
