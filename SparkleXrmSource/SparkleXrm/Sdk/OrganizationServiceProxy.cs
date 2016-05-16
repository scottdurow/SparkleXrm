// OrganizationServiceProxy.cs
//


using System;
using System.Collections.Generic;
using System.Html;
using System.Net;
using System.Xml;
using Xrm;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk
{
    /// <summary>
    /// The OrganizationServiceProxy is a static class for ease of use
    /// We might need to make an instance class as well to aid sharing code between client and server
    /// </summary>
    public class OrganizationServiceProxy
    {
        #region Fields
        public static bool WithCredentials = false; // This should be set to true if using Integrated auth with Visual Studio debug server in chrome such that Organization.svc calls are cross domain
        public static UserSettings UserSettings = null;
        public static Dictionary<string, Type> ExecuteMessageResponseTypes = new Dictionary<string, Type>();
        public static OrganizationSettings OrganizationSettings = null;
        #endregion

        #region Methods
        public static void RegisterExecuteMessageResponseType(string responseTypeName, Type organizationResponseType)
        {
            ExecuteMessageResponseTypes[responseTypeName] = organizationResponseType;

        }
        public static UserSettings GetUserSettings()
        {

            if (OrganizationServiceProxy.UserSettings == null)
            {
                OrganizationServiceProxy.UserSettings = (UserSettings)OrganizationServiceProxy.Retrieve(UserSettings.EntityLogicalName, Page.Context.GetUserId(), new string[] { "AllColumns" });
                // Add the separator values
                UserSettings.TimeFormatString = UserSettings.TimeFormatString.Replace(":", OrganizationServiceProxy.UserSettings.TimeSeparator);
                UserSettings.DateFormatString = UserSettings.DateFormatString.Replace(@"/", OrganizationServiceProxy.UserSettings.DateSeparator);

                // We need to change the format string from CRM into the datepicker format which is:
                // mm/dd/yy   Default - mm/dd/yy
                // yy-mm-dd   ISO 8601 - yy-mm-dd
                // d M, y   Short - d M, y
                // d MM, y   Medium - d MM, y
                // DD, d MM, yy   Full - DD, d MM, yy
                // 'day' d 'of' MM 'in the year' yy   With text - 'day' d 'of' MM 'in the year' yy
                UserSettings.DateFormatString = UserSettings.DateFormatString.Replace("MM", "mm").Replace("yyyy", "UU").Replace("yy", "y").Replace("UU", "yy").Replace("M", "m");
            }

            if (OrganizationServiceProxy.OrganizationSettings == null)
            {
                string fetchXml = @"<fetch>
                                    <entity name='organization' >
                                        <attribute name='weekstartdaycode' />
                                    </entity>
                                </fetch>";

                OrganizationServiceProxy.OrganizationSettings = (OrganizationSettings)OrganizationServiceProxy.RetrieveMultiple(fetchXml).Entities[0];
            }
            return UserSettings;

        }

        /// <summary>
        /// Checks for an existing N:N relationship between two records by executing a fetch against the relationship
        /// association table.
        /// </summary>
        /// <param name="relationship">The Relationship to evaluate.</param>
        /// <param name="Entity1">EntityReference for the one of the entities to test.</param>
        /// <param name="Entity2">EntityReference for the second entity to test.</param>
        /// <returns>Boolean true if Entity1 and Entity2 have an existing relationship.</returns>
        public static bool DoesNNAssociationExist(Relationship relationship, EntityReference Entity1, EntityReference Entity2)
        {
            string fetchXml = "<fetch mapping='logical'>"
              + "  <entity name='" + relationship.SchemaName + "'>"
              + "    <all-attributes />"
              + "    <filter>"
              + "      <condition attribute='" + Entity1.LogicalName + "id' operator='eq' value ='" + Entity1.Id.Value + "' />"
              + "      <condition attribute='" + Entity2.LogicalName + "id' operator='eq' value='" + Entity2.Id.Value + "' />"
              + "    </filter>"
              + "  </entity>"
              + "</fetch>";

            EntityCollection result = OrganizationServiceProxy.RetrieveMultiple(fetchXml);

            if (result.Entities.Count > 0)
                return true;

            return false;
        }


        /// <summary>
        /// Associate one or more related entites to a target entity.
        /// </summary>
        /// <param name="entityName">The Logical Name of the target entity.</param>
        /// <param name="entityId">The GUID that uniquely defines the target entity.</param>
        /// <param name="relationship">The Relationship to use for the association.</param>
        /// <param name="relatedEntities">A list of related EntityReference records to associate to the target.</param>
        public static void Associate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {
            XmlDocument resultXml = GetResponse(GetAssociateRequest(entityName,entityId,relationship,relatedEntities), "Execute", null);

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;
        }

        public static void BeginAssociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            GetResponse(GetAssociateRequest(entityName, entityId, relationship, relatedEntities), "Execute", callBack);
        }

        public static void EndAssociate(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
            }
            else
            {
                throw (Exception)asyncState;
            }

        }

        private static string GetAssociateRequest(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {

            //convert the list of related entites into a snippet of xml that can be inserted into the soap body.
            string entityReferences = "";
            foreach(EntityReference item in relatedEntities)
            {
                entityReferences += item.ToSoap("a");
            }

            string xmlSoapBody = "<Execute xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";
            xmlSoapBody += "      <request i:type=\"a:AssociateRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\">";
            xmlSoapBody += "        <a:Parameters xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">";
            xmlSoapBody += "          <a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "            <b:key>Target</b:key>";
            xmlSoapBody += "            <b:value i:type=\"a:EntityReference\">";
            xmlSoapBody += "              <a:Id>" + entityId.Value + "</a:Id>";
            xmlSoapBody += "              <a:LogicalName>" + entityName + "</a:LogicalName>";
            xmlSoapBody += "              <a:Name i:nil=\"true\" />";
            xmlSoapBody += "            </b:value>";
            xmlSoapBody += "          </a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "          <a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "            <b:key>Relationship</b:key>";
            xmlSoapBody += "            <b:value i:type=\"a:Relationship\">";
            xmlSoapBody += "              <a:PrimaryEntityRole i:nil=\"true\" />";
            xmlSoapBody += "              <a:SchemaName>" + relationship.SchemaName + "</a:SchemaName>";
            xmlSoapBody += "            </b:value>";
            xmlSoapBody += "          </a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "          <a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "            <b:key>RelatedEntities</b:key>";
            xmlSoapBody += "            <b:value i:type=\"a:EntityReferenceCollection\">" + entityReferences + "</b:value>";
            xmlSoapBody += "          </a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "        </a:Parameters>";
            xmlSoapBody += "        <a:RequestId i:nil=\"true\" />";
            xmlSoapBody += "        <a:RequestName>Associate</a:RequestName>";
            xmlSoapBody += "      </request>";
            xmlSoapBody += "    </Execute>";


            return xmlSoapBody;

        }


        public static void Disassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {
            XmlDocument resultXml = GetResponse(GetDisassociateRequest(entityName, entityId, relationship, relatedEntities), "Execute", null);

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;
        }
        public static void BeginDisassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            GetResponse(GetDisassociateRequest(entityName, entityId, relationship, relatedEntities), "Execute", callBack);
        }

        public static void EndDisassociate(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
            }
            else
            {
                throw (Exception)asyncState;
            }

        }
        private static string GetDisassociateRequest(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {

            //convert the list of related entites into a snippet of xml that can be inserted into the soap body.
            string entityReferences = "";
            foreach (EntityReference item in relatedEntities)
            {
                entityReferences += item.ToSoap("a");
            }

            string xmlSoapBody = "<Execute xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";
            xmlSoapBody += "      <request i:type=\"a:DisassociateRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\">";
            xmlSoapBody += "        <a:Parameters xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">";
            xmlSoapBody += "          <a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "            <b:key>Target</b:key>";
            xmlSoapBody += "            <b:value i:type=\"a:EntityReference\">";
            xmlSoapBody += "              <a:Id>" + entityId.Value + "</a:Id>";
            xmlSoapBody += "              <a:LogicalName>" + entityName + "</a:LogicalName>";
            xmlSoapBody += "              <a:Name i:nil=\"true\" />";
            xmlSoapBody += "            </b:value>";
            xmlSoapBody += "          </a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "          <a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "            <b:key>Relationship</b:key>";
            xmlSoapBody += "            <b:value i:type=\"a:Relationship\">";
            xmlSoapBody += "              <a:PrimaryEntityRole i:nil=\"true\" />";
            xmlSoapBody += "              <a:SchemaName>" + relationship.SchemaName + "</a:SchemaName>";
            xmlSoapBody += "            </b:value>";
            xmlSoapBody += "          </a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "          <a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "            <b:key>RelatedEntities</b:key>";
            xmlSoapBody += "            <b:value i:type=\"a:EntityReferenceCollection\">" + entityReferences + "</b:value>";
            xmlSoapBody += "          </a:KeyValuePairOfstringanyType>";
            xmlSoapBody += "        </a:Parameters>";
            xmlSoapBody += "        <a:RequestId i:nil=\"true\" />";
            xmlSoapBody += "        <a:RequestName>Disassociate</a:RequestName>";
            xmlSoapBody += "      </request>";
            xmlSoapBody += "    </Execute>";

            return xmlSoapBody;

        }

        public static EntityCollection RetrieveMultiple(string fetchXml)
        {
            XmlDocument resultXml = GetResponse(GetRetrieveMultipleRequest(fetchXml), "RetrieveMultiple", null);
            EntityCollection entities = GetEntityCollectionResults(resultXml, typeof(Entity));

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;

            // Return
            return entities;
        }

        private static string GetRetrieveMultipleRequest(string fetchXml)
        {
            string xml = "<RetrieveMultiple xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" ><query i:type=\"a:FetchExpression\" ><a:Query>";
            xml += XmlHelper.Encode(fetchXml);
            xml += "</a:Query></query></RetrieveMultiple>";
            return xml;
        }

        public static void BeginRetrieveMultiple(string fetchXml, Action<object> callBack)
        {
            XmlDocument resultXml = GetResponse(GetRetrieveMultipleRequest(fetchXml), "RetrieveMultiple", callBack);
        }

        public static EntityCollection EndRetrieveMultiple(object asyncState, Type entityType)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
                EntityCollection entities = GetEntityCollectionResults(xmlDocument, entityType);
                return entities;

            }
            else
            {
                throw (Exception)asyncState;
            }

        }

        private static EntityCollection GetEntityCollectionResults(XmlDocument resultXml, Type entityType)
        {
            // Get the id of the created object from the returned xml
            XmlNode soapBody = resultXml.FirstChild.FirstChild;
            XmlNode resultNode = XmlHelper.SelectSingleNodeDeep(soapBody, "RetrieveMultipleResult");
            XmlNode results = XmlHelper.SelectSingleNode(resultNode, "Entities");

            // Parse result into array of Entity objects
            int resultCount = 0;
            if (results != null)
                resultCount = results.ChildNodes.Count;

            List<Entity> businessEntities = new List<Entity>();

            for (int i = 0; i < resultCount; i++)
            {
                XmlNode entityNode = results.ChildNodes[i];

                Entity entity = (Entity)Type.CreateInstance(entityType, null);
                entity.DeSerialise(entityNode);
                businessEntities[i] = entity;
            }
            EntityCollection entities = new EntityCollection(businessEntities);
            entities.MoreRecords = XmlHelper.SelectSingleNodeValue(resultNode, "MoreRecords") == "true";
            entities.PagingCookie = XmlHelper.SelectSingleNodeValue(resultNode, "PagingCookie");
            entities.TotalRecordCount = int.Parse(XmlHelper.SelectSingleNodeValue(resultNode, "TotalRecordCount"));
            entities.TotalRecordCountLimitExceeded = XmlHelper.SelectSingleNodeValue(resultNode, "TotalRecordCountLimitExceeded") == "true";
            return entities;
        }

        public static Entity Retrieve(string entityName, string entityId, string[] attributesList)
        {
            XmlDocument resultXml = GetResponse(GetRetrieveRequest(entityName, entityId, attributesList), "Retrieve", null);
            XmlNode entityNode = XmlHelper.SelectSingleNodeDeep(resultXml, "RetrieveResult");
            Entity entity = new Entity(entityName);
            entity.DeSerialise(entityNode);

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;
            return entity;
        }
        public static void BeginRetrieve(string entityName, string entityId, string[] attributesList, Action<object> callBack)
        {
            GetResponse(GetRetrieveRequest(entityName, entityId, attributesList), "Retrieve", callBack);
        }

        public static Entity EndRetrieve(object asyncState, Type entityType)
        {
            XmlDocument resultXml = (XmlDocument)asyncState;
            XmlNode entityNode = XmlHelper.SelectSingleNodeDeep(resultXml, "RetrieveResult");
            Entity entity = new Entity(null);
            entity.DeSerialise(entityNode);

            return entity;

        }

        private static string GetRetrieveRequest(string entityName, string entityId, string[] attributesList)
        {
            string xml = "<Retrieve xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\">";
            xml += "<entityName>" + entityName + "</entityName>";
            xml += "<id>" + entityId + "</id>";
            xml += "<columnSet xmlns:d4p1=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";


            if ((attributesList != null) && (attributesList[0] != "AllColumns"))
            {
                xml += "<d4p1:AllColumns>false</d4p1:AllColumns>";

                xml += "<d4p1:Columns xmlns:d5p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">";
                for (int i = 0; i < attributesList.Length; i++)
                {
                    xml += "<d5p1:string>" + attributesList[i] + "</d5p1:string>";
                }
                xml += "</d4p1:Columns>";
            }
            else
            {
                xml += "<d4p1:AllColumns>true</d4p1:AllColumns>";

            }
            xml += "</columnSet></Retrieve>";
            return xml;
        }


        /// <summary>
        /// Creates a new entity record using the supplied Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Guid Create(Entity entity)
        {
            XmlDocument resultXml = GetResponse(GetCreateRequest(entity), "Create", null);
            string newGuid = XmlHelper.SelectSingleNodeValueDeep(resultXml, "CreateResult");

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;

            return new Guid(newGuid);
        }
        private static string GetCreateRequest(Entity entity)
        {
            string xml = "<Create xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" ><entity>";
            xml += entity.Serialise(true);
            xml += "</entity></Create>";
            return xml;
        }
        public static void BeginCreate(Entity entity, Action<object> callBack)
        {
            GetResponse(GetCreateRequest(entity), "Create", callBack);
        }


        public static Guid EndCreate(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
                string newGuid = XmlHelper.SelectSingleNodeValueDeep(xmlDocument, "CreateResult");
                return new Guid(newGuid);
            }
            else
            {
                throw (Exception)asyncState;
            }

        }
        public static void SetState(Guid id, string entityName, int stateCode, int statusCode)
        {
            XmlDocument resultXml = GetResponse(GetSetStateRequest(id, entityName, stateCode, statusCode), "Execute", null);

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;

        }
        public static void BeginSetState(Guid id, string entityName, int stateCode, int statusCode, Action<object> callBack)
        {
            GetResponse(GetSetStateRequest(id, entityName, stateCode, statusCode), "Execute", callBack);
        }


        public static void EndSetState(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
            }
            else
            {
                throw (Exception)asyncState;
            }

        }
        private static string GetSetStateRequest(Guid id,string entityName,int stateCode, int statusCode)
        {
            return String.Format("<Execute xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\">" +
                "<request i:type=\"b:SetStateRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"+
                "<a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">" +
                    "<a:KeyValuePairOfstringanyType>" +
                        "<c:key>EntityMoniker</c:key>" +
                        "<c:value i:type=\"a:EntityReference\">" +
                          "<a:Id>{0}</a:Id>" +
                          "<a:LogicalName>{1}</a:LogicalName>" +
                          "<a:Name i:nil=\"true\" />" +
                        "</c:value>"+
                      "</a:KeyValuePairOfstringanyType>"+
                      "<a:KeyValuePairOfstringanyType>"+
                        "<c:key>State</c:key>"+
                        "<c:value i:type=\"a:OptionSetValue\">"+
                          "<a:Value>{2}</a:Value>"+
                        "</c:value>"+
                      "</a:KeyValuePairOfstringanyType>"+
                      "<a:KeyValuePairOfstringanyType>"+
                        "<c:key>Status</c:key>"+
                        "<c:value i:type=\"a:OptionSetValue\">"+
                          "<a:Value>{3}</a:Value>"+
                        "</c:value>"+
                      "</a:KeyValuePairOfstringanyType>"+
                "</a:Parameters>"+
                "<a:RequestId i:nil=\"true\" />"+
                "<a:RequestName>SetState</a:RequestName>"+
            "</request></Execute>",id.Value,entityName,stateCode,statusCode);
        }
        public static string Delete_(string entityName, Guid id)
        {
            XmlDocument resultXml = GetResponse(GetDeleteRequest(entityName, id), "Delete", null);
            string newGuid = XmlHelper.SelectSingleNodeValueDeep(resultXml, "DeleteResult");

            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;

            return newGuid;
        }

        private static string GetDeleteRequest(string entityName, Guid id)
        {
            string xml = String.Format("<Delete xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" ><entityName>{0}</entityName><id>{1}</id></Delete>",
                entityName, id.Value);
            return xml;
        }

        public static void BeginDelete(string entityName, Guid id, Action<object> callBack)
        {
            XmlDocument resultXml = GetResponse(GetDeleteRequest(entityName, id), "Delete", callBack);
        }

        public static void EndDelete(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success!
                return;
            }
            else
            {
                throw (Exception)asyncState;
            }

        }
        public static void Update(Entity entity)
        {
            string xml = "<Update xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" ><entity>";
            xml += entity.Serialise(true);
            xml += "</entity></Update>";
            XmlDocument resultXml = GetResponse(xml, "Update", null);
            // Tidy up
            Script.Literal("delete {0}", resultXml);
            resultXml = null;

        }

        public static void BeginUpdate(Entity entity, Action<object> callBack)
        {
            string xml = "<Update xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" ><entity>";
            xml += entity.Serialise(true);
            xml += "</entity></Update>";
            XmlDocument resultXml = GetResponse(xml, "Update", callBack);
        }

        public static void EndUpdate(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
                return;
            }
            else
            {
                throw (Exception)asyncState;
            }

        }

        public static OrganizationResponse Execute(OrganizationRequest request)
        {
            XmlDocument resultXml = GetResponse(GetExecuteRequest(request), "Execute", null);
            return EndExecute(resultXml);
        }

        private static string GetExecuteRequest(OrganizationRequest request)
        {
            string xml = "<Execute xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\">";
            xml += request.Serialise();
            xml += "</Execute>";
            return xml;
        }
        public static void BeginExecute(OrganizationRequest request, Action<object> callBack)
        {
            GetResponse(GetExecuteRequest(request), "Execute", callBack);
        }

        public static OrganizationResponse EndExecute(object asyncState)
        {
            XmlDocument xmlDocument = (XmlDocument)asyncState;

            if (xmlDocument.ChildNodes != null)
            {
                // Success
                XmlNode response = XmlHelper.SelectSingleNodeDeep(xmlDocument, "ExecuteResult");
                string type = XmlHelper.SelectSingleNodeValue(response, "ResponseName");
                switch (type)
                {
                    case "RetrieveAttribute":
                        return new RetrieveAttributeResponse(response);
                    case "RetrieveAllEntities":
                        return new RetrieveAllEntitiesResponse(response);
                    case "RetrieveEntity":
                        return new RetrieveEntityResponse(response);
                    case "BulkDeleteResponse":
                        return new BulkDeleteResponse(response);
                    case "FetchXmlToQueryExpression":
                        return new FetchXmlToQueryExpressionResponse(response);
                    case "RetrieveMetadataChanges":
                        return new RetrieveMetadataChangesResponse(response);
                    case "RetrieveRelationship":
                        return new RetrieveRelationshipResponse(response);
                    case "ExecuteWorkflow":
                        return new ExecuteWorkflowResponse(response);
                    case "Assign":
                        return new AssignResponse(response);
                    default:
                        // Allow custom actions/message types to be registered
                        if (ExecuteMessageResponseTypes.ContainsKey(type))
                        {
                            Type responseType = ExecuteMessageResponseTypes[type];
                            OrganizationResponse exectueResponse = (OrganizationResponse)Type.CreateInstance(responseType, response);
                            return exectueResponse;
                        }
                        else return null;

                }
            }
            else
            {
                throw (Exception)asyncState;
            }

        }

        /// <summary>
        /// Gets the SOAP Xml to send to the server
        /// </summary>
        /// <param name="payLoadXml"></param>
        /// <returns></returns>
        private static string getSoapEnvelope(string payLoadXml)
        {
            string xml = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:d=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\"  xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:b=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">" +
                "<s:Body>" + payLoadXml +
                "</s:Body>" +
                "</s:Envelope>";

            return xml;
        }
        private static string GetServerUrl()
        {
            // If we have the getClientUrl function (CRM2011 UR8+ & CRM2013, then use it)
            if (Script.Literal("typeof(Xrm.Page.context.getClientUrl)")=="undefined")
            {
                Context context = Page.Context;
                string crmServerUrl;

                if (context.IsOutlookClient() && !context.IsOutlookOnline())
                {
                    crmServerUrl = Window.Location.Protocol + "//" + Window.Location.Hostname;
                }
                else
                {
                    crmServerUrl = Page.Context.GetServerUrl();
                    crmServerUrl = crmServerUrl.ReplaceRegex(new RegularExpression(@"/^(http|https):\/\/([_a-zA-Z0-9\-\.]+)(:([0-9]{1,5}))?/"), Window.Location.Protocol + "//" + Window.Location.Hostname);
                    crmServerUrl = crmServerUrl.ReplaceRegex(new RegularExpression(@"/\/$/"), ""); // remove trailing slash if any
                }
                return crmServerUrl;
            }
            else
            {
                return Page.Context.GetClientUrl();
            }
        }


        private static XmlDocument GetResponse(string soapXmlPacket, string action, Action<object> asyncCallback)
        {
            bool isAsync = (asyncCallback != null);

            string xml = getSoapEnvelope(soapXmlPacket);
            Exception msg = null;
            XmlHttpRequest xmlHttpRequest = new XmlHttpRequest();



            xmlHttpRequest.Open("POST", GetServerUrl() + "/XRMServices/2011/Organization.svc/web", isAsync);
            xmlHttpRequest.SetRequestHeader("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/" + action);
            xmlHttpRequest.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");

            // This is only needed when debugging via localhost - and accessing the CRM webservices cross domain in chrome
            if (WithCredentials)
            {
                Script.Literal("{0}.withCredentials = true;", xmlHttpRequest);
            }

            if (isAsync)
            {
                xmlHttpRequest.OnReadyStateChange = delegate()
                {
                    if (xmlHttpRequest == null)
                        return;
                    if (xmlHttpRequest.ReadyState == ReadyState.Loaded)
                    {
                        // Capture the result
                        XmlDocument resultXml = xmlHttpRequest.ResponseXml;
                        Exception errorMsg = null;
                        if (xmlHttpRequest.Status != 200)
                        {
                            errorMsg = GetSoapFault(resultXml);
                        }
                        // Tidy Up
                        Script.Literal("delete {0}", xmlHttpRequest);

                        xmlHttpRequest = null;

                        if (errorMsg != null)
                        {
                            asyncCallback(errorMsg);
                        }
                        else
                        {
                            // Callback
                            asyncCallback(resultXml);
                        }
                    }
                };

                xmlHttpRequest.Send(xml);

                return null;
            }
            else
            {
                xmlHttpRequest.Send(xml);

                // Capture the result
                XmlDocument resultXml = xmlHttpRequest.ResponseXml;

                // Check for errors.
                if (xmlHttpRequest.Status != 200)
                {
                    msg = GetSoapFault(resultXml);
                }
                // Tidy Up
                Script.Literal("delete {0};", xmlHttpRequest);
                xmlHttpRequest = null;

                if (msg != null)
                {
                    throw msg;
                }
                else
                {

                    // Return
                    return resultXml;
                }
            }
        }

        private static Exception GetSoapFault(XmlDocument response)
        {
            string errorMsg = null;
            string traceDetails = null;
            string errorCode = null;
            if (response==null || response.FirstChild.Name != "s:Envelope")
            {
                return new Exception("No SOAP Envelope in response");
            }
            XmlNode soapResponseBody = response.FirstChild.FirstChild;

            // Check for errors.
            XmlNode errorNode = XmlHelper.SelectSingleNode(soapResponseBody, "Fault");
            if (errorNode != null)
            {
                XmlNode details = XmlHelper.SelectSingleNode(errorNode, "detail");
                if (details != null)
                {
                    XmlNode serviceFaultNode = XmlHelper.SelectSingleNode(details, "OrganizationServiceFault");
                    if (serviceFaultNode != null)
                    {
                        // Get the detail if there is any
                        errorMsg = XmlHelper.SelectSingleNodeValue(serviceFaultNode, "Message");          
                        traceDetails = XmlHelper.SelectSingleNodeValue(serviceFaultNode, "TraceText");
                        errorCode = XmlHelper.SelectSingleNodeValue(serviceFaultNode, "ErrorCode");
                    }
                }

                if (errorMsg == null)
                {
                    XmlNode faultMessage = XmlHelper.SelectSingleNode(errorNode, "faultstring");
                    if (faultMessage != null)
                    {
                        errorMsg = XmlHelper.GetNodeTextValue(faultMessage);
                    }
                }
            }

            Dictionary<string, string> info = new Dictionary<string, string>();
            info["Trace"] = traceDetails;
            info["ErrorCode"] = errorCode;
            return Exception.Create(errorMsg, info);
        }
        #endregion
    }
}
