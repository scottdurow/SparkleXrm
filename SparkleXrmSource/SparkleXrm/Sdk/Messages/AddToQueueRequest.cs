// AddToQueueRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class AddToQueueRequest : OrganizationRequest, IWebAPIOrganizationRequest
    {
        [PreserveCase]
        public Guid DestinationQueueId;
        [PreserveCase]
        public EntityReference Target;

        public string Serialise()
        {
            return "<d:request>" +
          "<a:Parameters>" +
            "<a:KeyValuePairOfstringanyType>" +
              "<b:key>DestinationQueueId</b:key>" +
             ((DestinationQueueId == null) ? "<b:value i:nil=\"true\" />" :
             "<b:value i:type=\"e:guid\">" + DestinationQueueId.ToString() + "</b:value>") +
            "</a:KeyValuePairOfstringanyType>" +

            "<a:KeyValuePairOfstringanyType>" +
              "<b:key>Target</b:key>" +
             ((Target == null) ? "<b:value i:nil=\"true\" />" :
             Attribute.SerialiseValue(Target,null)) +
            "</a:KeyValuePairOfstringanyType>" +

          "</a:Parameters>" +
          "<a:RequestId i:nil=\"true\" />" +
          "<a:RequestName>AddToQueue</a:RequestName>" +
        "</d:request>";
        }
        public WebAPIOrgnanizationRequestProperties SerialiseWebApi()
        {
            WebAPIOrgnanizationRequestProperties request = new WebAPIOrgnanizationRequestProperties();
            request.OperationType = OperationTypeEnum.Action;
            request.BoundEntityId = DestinationQueueId;
            request.BoundEntityLogicalName = "queue";
            request.RequestName = "Microsoft.Dynamics.CRM.AddToQueue";
            Entity target = new Entity(Target.LogicalName);
            target.SetAttributeValue("activityid", Target.Id.Value);
            request.AdditionalProperties["Target"] = target; // TODO: Investigate the consistency of this - are EntityReferences always Entities in the Actions - unlike the Businessentity in the DetectDuplicates Function which is the other way around.
            return request;

        }
    }

    
}
