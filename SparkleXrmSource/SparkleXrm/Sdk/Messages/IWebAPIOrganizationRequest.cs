// IWebAPIOrganizationRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;


namespace Xrm.Sdk.Messages
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public interface IWebAPIOrganizationRequest
    {
        WebAPIOrgnanizationRequestProperties SerialiseWebApi();
    }

    public delegate void WebAPIOrgnanizatioCustomRequest(OrganizationRequest request, Action<object> callback, Action<object> errorCallback, bool async);
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class WebAPIOrgnanizationRequestProperties
    {
        /// <summary>
        /// Leave null if the Action/Function is not bound to a specific entity
        /// </summary>
        public string BoundEntityLogicalName;

        /// <summary>
        /// Leave null if the Action/Function is not bound to a specific entity
        /// otherwise provide the Id of the record to run the bound operation upon.
        /// </summary>
        public Guid BoundEntityId;

        /// <summary>
        /// The name of the function/action eg. WhoAmI or Microsoft.Dynamics.CRM.AddToQueue
        /// </summary>
        public string RequestName;

        /// <summary>
        /// Define how the request is mapped to the WebAPI
        /// Functions use GET and Actions use POST
        /// </summary>
        public OperationTypeEnum OperationType;

        /// <summary>
        /// Additional properties to provide to the call
        /// these will be serialised into the url
        /// </summary>
        public Dictionary<string, object> AdditionalProperties = new Dictionary<string, object>();

        public WebAPIOrgnanizatioCustomRequest CustomImplementation;
    }


    [NamedValues]
    public enum OperationTypeEnum
    {
        FunctionCall = 0,
        Action = 1
    }
}
