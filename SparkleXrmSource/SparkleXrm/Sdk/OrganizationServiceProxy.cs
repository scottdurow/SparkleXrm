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
        public static UserSettings UserSettings = null;
        #endregion

        #region Methods
        public static UserSettings GetUserSettings()
        {

            if (OrganizationServiceProxy.UserSettings == null)
            {
                OrganizationServiceProxy.UserSettings = (UserSettings)OrganizationServiceProxy.Retrieve(UserSettings.EntityLogicalName, Page.Context.GetUserId(), new string[] { "AllColumns" });
            }

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
            return UserSettings;

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
                throw new Exception((string)asyncState);
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
                throw new Exception((string)asyncState);
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
                throw new Exception((string)asyncState);
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
                throw new Exception((string)asyncState);
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
                throw new Exception((string)asyncState);
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

                }
                return null;
            }
            else
            {
                throw new Exception((string)asyncState);
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


        private static XmlDocument GetResponse(string soapXmlPacket, string action, Action<object> asyncCallback)
        {
            bool isAsync = (asyncCallback != null);

            string xml = getSoapEnvelope(soapXmlPacket);
            string msg = null;
            XmlHttpRequest xmlHttpRequest = new XmlHttpRequest();

            // Script.Literal("{0}.withCredentials = true;", xmlHttpRequest);

            xmlHttpRequest.Open("POST", GetServerUrl() + "/XRMServices/2011/Organization.svc/web", isAsync);
            xmlHttpRequest.SetRequestHeader("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/" + action);
            xmlHttpRequest.SetRequestHeader("Content-Type", "text/xml; charset=utf-8"); 

            Script.Literal("{0}.withCredentials = true;", xmlHttpRequest);


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
                        string errorMsg = null;
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
                    throw new Exception("CRM SDK Call returned error: '" + msg + "'");
                }
                else
                {

                    // Return
                    return resultXml;
                }
            }
        }

        private static string GetSoapFault(XmlDocument response)
        {
            string errorMsg = null;

            if (response.FirstChild.Name != "s:Envelope")
            {
                return "No SOAP Envelope in response";
            }
            XmlNode soapResponseBody = response.FirstChild.FirstChild;

            // Check for errors.
            XmlNode errorNode = XmlHelper.SelectSingleNode(soapResponseBody, "Fault");
            if (errorNode != null)
            {
                // Get the detail if there is any
                XmlNode detailMessageNode = XmlHelper.SelectSingleNodeDeep(errorNode, "Message");
                if (detailMessageNode != null)
                {
                    errorMsg = XmlHelper.GetNodeTextValue(detailMessageNode);
                }
                else
                {
                    XmlNode faultMessage = XmlHelper.SelectSingleNode(errorNode, "faultstring");
                    if (faultMessage != null)
                    {
                        errorMsg = XmlHelper.GetNodeTextValue(faultMessage);
                    }
                }
            }
            return errorMsg;
        }
        #endregion
    }
}
