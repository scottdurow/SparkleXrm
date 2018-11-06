// RetrieveMetadataChangesResponse.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk.Metadata;
using SparkleXrm.Sdk.Metadata.Query;
namespace Xrm.Sdk.Messages
{
    /*
     * The WebApi request would look like:
     * api/data/v9.0/RetrieveMetadataChanges(Query=@q)?@q=%7B%0A%09%22Criteria%22%3A+%7B%0A%09%09%22Conditions%22%3A%5B%0A%09%09%09%7B%0A%09%09%09%09%0A%09%09%09%09%22PropertyName%22%3A%22LogicalName%22%2C%0A%09%09%09%09%22ConditionOperator%22%3A%22Equals%22%2C%0A%09%09%09%09%22Value%22%3A%7B%0A%09%09%09%09%09%22Value%22%3A%22account%22%2C%0A%09%09%09%09%09%22Type%22%3A%22System.String%22%09%09%09%09%09%0A%09%09%09%09%09%7D%0A%09%09%09%7D%0A%09%09%0A%09%09%5D%2C%0A%09%09%22FilterOperator%22%3A%22And%22%0A%09%09%7D%2C%0A%09%09%22Properties%22%3A%7B%0A%09%09%09%22PropertyNames%22%3A%5B%22Attributes%22%5D%0A%09%09%7D%2C%0A%09%09%22AttributeQuery%22%3A%7B%0A%09%09%09%22Properties%22%3A%7B%0A%09%09%09%09%22PropertyNames%22%3A%5B%22OptionSet%22%5D%0A%09%09%09%7D%0A%09%09%7D%0A%7D
        {
	        "Criteria": {
		        "Conditions":[
			        {
				
				        "PropertyName":"LogicalName",
				        "ConditionOperator":"Equals",
				        "Value":{
					        "Value":"account",
					        "Type":"System.String"					
					        }
			        }
		
		        ],
		        "FilterOperator":"And"
		        },
		        "Properties":{
			        "PropertyNames":["Attributes"]
		        },
		        "AttributeQuery":{
			        "Properties":{
				        "PropertyNames":["OptionSet"]
			        }
		        }
        }
    */
    [ScriptNamespace("SparkleXrm.Sdk.Messages")]
    public class RetrieveMetadataChangesResponse : OrganizationResponse, IWebAPIOrganizationResponse
    {

        public RetrieveMetadataChangesResponse(XmlNode response)
        {
            if (response == null)
                return; 
            XmlNode results = XmlHelper.SelectSingleNode(response,"Results");

            foreach (XmlNode nameValuePair in results.ChildNodes)
            {
                XmlNode key = XmlHelper.SelectSingleNode(nameValuePair, "key");
                XmlNode value = XmlHelper.SelectSingleNode(nameValuePair, "value");
                switch (XmlHelper.GetNodeTextValue(key))
                {
                    case "ServerVersionStamp":
                        this.ServerVersionStamp = XmlHelper.GetNodeTextValue(value);
                        break;
                    case "DeletedMetadata":
                        break;
                    case "EntityMetadata":
                        EntityMetadata = new List<EntityMetadata>();
                        for (int i = 0; i < value.ChildNodes.Count; i++)
                        {
                            XmlNode entity = value.ChildNodes[i];
                            EntityMetadata metaData = MetadataSerialiser.DeSerialiseEntityMetadata(new EntityMetadata(), entity);
                            EntityMetadata.Add(metaData);          
                        }
                        break;
                }
            }           
        }
        // Summary:
        //     Gets the deleted metadata since the last request.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.Query.DeletedMetadataCollectionThe deleted
        //     metadata since the last request.
        //public DeletedMetadataCollection DeletedMetadata;
        //
        // Summary:
        //     Gets the metadata defined by the request.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.EntityMetadataCollectionThe metadata defined
        //     by the request.
        [PreserveCase]
        public List<EntityMetadata> EntityMetadata;
        //
        // Summary:
        //     Gets a timestamp identifier for the metadata retrieved.
        //
        // Returns:
        //     Type: Returns_String A timestamp identifier for the metadata retrieved..
        [PreserveCase]
        public string ServerVersionStamp;


        public void DeserialiseWebApi(Dictionary<string, object> response)
        {
            this.EntityMetadata = (List<EntityMetadata>)response["EntityMetadata"];
            this.ServerVersionStamp = (string)response["ServerVersionStamp"];
            

        }
    }
}
