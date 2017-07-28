// RetrieveDuplicatesRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{
  

    public class RetrieveDuplicatesRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        public Entity BusinessEntity;
        public string MatchingEntityName;
        public PagingInfo PagingInfo;
        public string Serialise()
        {
            return "<d:request>" +
           "<a:Parameters>" +
             "<a:KeyValuePairOfstringanyType>" +
               "<b:key>BusinessEntity</b:key>" +
              ((BusinessEntity == null) ? "<b:value i:nil=\"true\" />" :
              "<b:value i:type=\"a:Entity\">" + BusinessEntity.Serialise(true) + "</b:value>") + 
             "</a:KeyValuePairOfstringanyType>" +

             "<a:KeyValuePairOfstringanyType>" +
               "<b:key>MatchingEntityName</b:key>" +
              ((MatchingEntityName == null) ? "<b:value i:nil=\"true\" />" :
              "<b:value i:type=\"c:string\">" + MatchingEntityName + "</b:value>") +
             "</a:KeyValuePairOfstringanyType>" +

             "<a:KeyValuePairOfstringanyType>" +
               "<b:key>PagingInfo</b:key>" +
              ((PagingInfo == null) ? "<b:value i:nil=\"true\" />" :
              "<b:value i:type=\"a:PagingInfo\">" + PagingInfo.Serialise() + "</b:value>") +
             "</a:KeyValuePairOfstringanyType>" +

           "</a:Parameters>" +
           "<a:RequestId i:nil=\"true\" />" +
           "<a:RequestName>RetrieveDuplicates</a:RequestName>" +
         "</d:request>";

         
        }
        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.OperationType = OperationTypeEnum.FunctionCall;
            request.RequestName = "RetrieveDuplicates";
            if (BusinessEntity.Id != null)
            {
                request.AdditionalProperties["BusinessEntity"] = BusinessEntity.ToEntityReference();
            }
            else
            {
                request.AdditionalProperties["BusinessEntity"] = BusinessEntity;
            }
            request.AdditionalProperties["MatchingEntityName"] = MatchingEntityName;
            request.AdditionalProperties["PagingInfo"] = PagingInfo;
            return request;

        }

    }
}
