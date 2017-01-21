// RetrieveAttributeRequest.cs
//


using System.Runtime.CompilerServices;
namespace Xrm.Sdk.Messages
{
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveAllEntitiesRequest : OrganizationRequest
    {
        public string Serialise()
        {     
            return @"
                              <request i:type=""a:RetrieveAllEntitiesRequest"" xmlns:a=""http://schemas.microsoft.com/xrm/2011/Contracts"">
                                <a:Parameters xmlns:b=""http://schemas.datacontract.org/2004/07/System.Collections.Generic"">
                                  <a:KeyValuePairOfstringanyType>
                                    <b:key>EntityFilters</b:key>
                                    <b:value i:type=""c:EntityFilters"" xmlns:c=""http://schemas.microsoft.com/xrm/2011/Metadata"">Entity</b:value>
                                  </a:KeyValuePairOfstringanyType>
                                  <a:KeyValuePairOfstringanyType>
                                    <b:key>RetrieveAsIfPublished</b:key>
                                    <b:value i:type=""c:boolean"" xmlns:c=""http://www.w3.org/2001/XMLSchema"">true</b:value>
                                  </a:KeyValuePairOfstringanyType>
                                </a:Parameters>
                                <a:RequestId i:nil=""true"" />
                                <a:RequestName>RetrieveAllEntities</a:RequestName>
                              </request>
                            ";
        }
    }
}
