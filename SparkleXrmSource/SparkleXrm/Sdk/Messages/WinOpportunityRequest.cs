// WinOpportunityRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{

    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class WinOpportunityRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        [PreserveCase]
        public Entity OpportunityClose;
        [PreserveCase]
        public OptionSetValue Status;

        public string Serialise()
        {
            return "<d:request>" +
          "<a:Parameters>" +
            "<a:KeyValuePairOfstringanyType>" +
              "<b:key>OpportunityClose</b:key>" +
             ((OpportunityClose == null) ? "<b:value i:nil=\"true\" />" :
             "<b:value i:type=\"a:Entity\">" + OpportunityClose.Serialise(true) + "</b:value>") +
            "</a:KeyValuePairOfstringanyType>" +

            "<a:KeyValuePairOfstringanyType>" +
              "<b:key>Status</b:key>" +
             ((Status == null) ? "<b:value i:nil=\"true\" />" :
             "<b:value i:type=\"a:OptionSetValue\"><a:Value>" + Status.Value.ToString() + "</a:Value></b:value>") +
            "</a:KeyValuePairOfstringanyType>" +

          "</a:Parameters>" +
          "<a:RequestId i:nil=\"true\" />" +
          "<a:RequestName>WinOpportunity</a:RequestName>" +
        "</d:request>";
        }

        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.OperationType = OperationTypeEnum.Action;
            request.RequestName = "WinOpportunity";
            request.AdditionalProperties["OpportunityClose"] = OpportunityClose;
            request.AdditionalProperties["Status"] = Status;
            return request;

        }
    }
}
