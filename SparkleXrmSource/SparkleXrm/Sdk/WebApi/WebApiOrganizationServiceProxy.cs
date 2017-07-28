using SparkleXrm.SDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Serialization;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class WebApiOrganizationServiceProxy : IOrganizationService
    {
        private static string _clientUrl = null;
        private static string _webAPIVersion = "8.2";

        internal static Dictionary<string, string> NavigationToLogicalNameMapping = new Dictionary<string, string>();
        internal static Dictionary<string, string[]> LogicalNameToNavigationMapping = new Dictionary<string, string[]>();
        internal static Dictionary<string, WebApiEntityMetadata> WebApiMetadata = new Dictionary<string, WebApiEntityMetadata>();

        public static Dictionary<string, Type> ExecuteMessageResponseTypes = new Dictionary<string, Type>();

        static WebApiOrganizationServiceProxy()
        {
            AddMetadata("contact", "contacts", "contactid");
            AddMetadata("account", "accounts", "accountid");
            AddMetadata("systemuser", "systemusers", "systemuserid");
        }

        public bool DoesNNAssociationExist(Relationship relationship, EntityReference Entity1, EntityReference Entity2)
        {
            throw new Exception("Not Implemented");
        }

        public void SetState(Guid id, string entityName, int stateCode, int statusCode)
        {
            throw new Exception("Not Implemented");
        }

        public void BeginSetState(Guid id, string entityName, int stateCode, int statusCode, Action<object> callBack)
        {
            throw new Exception("Not Implemented");
        }

        public void EndSetState(object asyncState)
        {
            throw new Exception("Not Implemented");
        }

        public UserSettings GetUserSettings()
        {
            throw new Exception("Not Implemented");
        }

        public void RegisterExecuteMessageResponseType(string responseTypeName, Type organizationResponseType)
        {
            ExecuteMessageResponseTypes[responseTypeName] = organizationResponseType;
        }

        public static void AddNavigationPropertyMetadata(string entityLogicalName, string attributeLogicalName, string navigationProperties)
        {
            string[] navigation = navigationProperties.Split(',');
            foreach (string prop in navigation)
            {
                NavigationToLogicalNameMapping[entityLogicalName + "." + navigationProperties] = attributeLogicalName;
            }
            LogicalNameToNavigationMapping[entityLogicalName + "." + attributeLogicalName] = navigation;
        }

        public static void AddMetadata(string logicalName, string entitySetName, string primaryAttributeLogicalName)
        {
            WebApiEntityMetadata metadata = new WebApiEntityMetadata();
            metadata.LogicalName = logicalName;
            metadata.EntitySetName = entitySetName;
            metadata.PrimaryAttributeLogicalName = primaryAttributeLogicalName;
            WebApiMetadata[logicalName] = metadata;
        }

        public Guid Create(Entity entity)
        {
            return BeginCreateInternal(entity, null);
        }

        public void BeginCreate(Entity entity, Action<object> callBack)
        {
            BeginCreateInternal(entity, callBack);
        }

        private Guid BeginCreateInternal(Entity entity, Action<object> callBack)
        {
            Guid id = null;
            bool async = !Script.IsNullOrUndefined(callBack);
            Action<object> errorCallback = !async ? ThrowErrorCallback : callBack;
            Action<object> endCallback = !async ? (Action<object>)delegate (object state)
            {
                id = EndCreate(state);
            }
            : callBack;
            GetEntityMetadata(entity.LogicalName, delegate (WebApiEntityMetadata metadata)
            {
                Entity.SerialiseWebApi(entity, delegate (object jsonData)
                {
                    Dictionary<string, object> jsonDataDictionary = (Dictionary<string, object>)jsonData;
                    // Remove any navigation properties set ot null
                    foreach (string attribute in jsonDataDictionary.Keys)
                    {
                        if (attribute.EndsWith("@odata.bind") && jsonDataDictionary[attribute] == null)
                        {
                            Type.DeleteField(jsonData, attribute);
                        }
                    }

                    string json = Json.Stringify(jsonData);
                    SendRequest(entity.LogicalName, metadata.EntitySetName, null, "POST", json, async
                      , endCallback
                      , errorCallback);
                }, errorCallback, async);

            },
            errorCallback, async);
            return id;
        }

        public Guid EndCreate(object asyncState)
        {
            CheckEndException(asyncState);

            // Get the new Guid
            WebApiRequestResponse response = (WebApiRequestResponse)asyncState;
            string headerId = response.GetHeader("OData-EntityId");

            RegularExpression guidExpr = new RegularExpression(@"\(([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})\)", "g");
            string[] parts = guidExpr.Exec(headerId);
            if (parts.Length > 0)
            {
                return new Guid(parts[1]);
            }
            else
                throw new Exception("Invalid response");
        }

        public void Update(Entity entity)
        {
            BeginUpdateInternal(entity, null);
        }

        public void BeginUpdate(Entity entity, Action<object> callBack)
        {
            BeginUpdateInternal(entity, callBack);
        }

        public void BeginUpdateInternal(Entity entity, Action<object> callBack)
        {
            bool async = !Script.IsNullOrUndefined(callBack);
            Action<object> errorCallback = !async ? ThrowErrorCallback : callBack;
            Action<object> endCallback = !async ? EndUpdate : callBack;
            GetEntityMetadata(entity.LogicalName, delegate (WebApiEntityMetadata metadata)
            {

                Entity.SerialiseWebApi(entity, delegate (object jsonData)
                 {
                     Dictionary<string, object> jsonDataDictionary = (Dictionary<string, object>)jsonData;
                     List<string> lookupsToRemove = new List<string>();
                     foreach (string attribute in jsonDataDictionary.Keys)
                     {
                         if (attribute.EndsWith("@odata.bind") && jsonDataDictionary[attribute] == null)
                         {
                             lookupsToRemove.Add(attribute);
                         }
                     }
                    // We need to send a separate DELETE request for all lookups being nulled!
                    DelegateItterator.CallbackItterate(delegate (int index, Action nextCallBack, ErrorCallBack errorCallBack)
                     {
                        // Delete the reference
                        string attribute = lookupsToRemove[index];
                         string lookupattribute = attribute.Substr(0, attribute.Length - 11);
                         Type.DeleteField(jsonData, attribute);

                         SendRequest(entity.LogicalName, GetResource(metadata.EntitySetName, entity.Id) + "/" + lookupattribute + "/$ref", null, "DELETE", null, async,
                             delegate (object state)
                             {
                                 nextCallBack();
                             },
                             errorCallback
                             );

                     }, lookupsToRemove.Count, delegate ()
                     {
                         string json = Json.Stringify(jsonData);

                         SendRequest(entity.LogicalName, GetResource(metadata.EntitySetName, entity.Id), null, "PATCH", json, async
                         , endCallback
                         , errorCallback);

                     },
                     delegate (Exception ex)
                     {
                         errorCallback((object)ex);
                     });
                 }, errorCallback, async);
            },
            errorCallback, async);
        }

        public void EndUpdate(object asyncState)
        {
            CheckEndException(asyncState);
        }

        public string Delete_(string logicalName, Guid guid)
        {
            DeleteInternal(logicalName, guid, null);
            return null;
        }

        public void BeginDelete(string logicalName, Guid guid, Action<object> callBack)
        {
            DeleteInternal(logicalName, guid, callBack);
        }

        private void DeleteInternal(string logicalName, Guid guid, Action<object> callBack)
        {
            bool async = !Script.IsNullOrUndefined(callBack);
            Action<object> errorCallback = !async ? ThrowErrorCallback : callBack;
            Action<object> endCallback = !async ? EndDelete : callBack;
            GetEntityMetadata(logicalName, delegate (WebApiEntityMetadata metadata)
            {
                SendRequest(logicalName, GetResource(metadata.EntitySetName, guid.Value), null, "DELETE", null, async
                    , endCallback
                    , errorCallback);
            },
           errorCallback, async);
        }

        public void EndDelete(object asyncState)
        {
            CheckEndException(asyncState);
        }

        private void CheckEndException(object asyncState)
        {
            if (asyncState.GetType() == typeof(Exception))
            {
                throw (Exception)asyncState;
            }
        }

        internal static string GetResource(string setName, string id)
        {
            return setName + "(" + Guid.StripGuid(id) + ")";
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            return BeginExecuteInternal(request, null);
        }

        public void BeginExecute(OrganizationRequest request, Action<object> callBack)
        {
            BeginExecuteInternal(request, callBack);
        }

        private OrganizationResponse BeginExecuteInternal(OrganizationRequest request, Action<object> callBack)
        {
            IWebAPIOrganizationRequest webApiRequest = (IWebAPIOrganizationRequest)request;
            WebAPIOrgnanizationRequestProperties requestProperties = null;
            try
            {
                requestProperties = webApiRequest.SerialiseWebApi();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create webapi request " + ex.Message);
            }

            OrganizationResponse response = null;
            bool async = !Script.IsNullOrUndefined(callBack);
            Action<object> errorCallback = !async ? ThrowErrorCallback : callBack;
            string requestname = requestProperties.RequestName.Replace("Microsoft.Dynamics.CRM.", "");

            Action<object> endCallback = !async ? (Action<object>)delegate (object state)
            {
                Type.SetField(state, "_requestName", requestname);
                response = EndExecute(state);
            }
            : delegate (object state)
            {
                Type.SetField(state, "_requestName", requestname);
                callBack(state);
            };

            string operation = requestProperties.OperationType == OperationTypeEnum.FunctionCall ? "GET" : "POST";
            WebApiEntityMetadata requestMetadata = null;

            Action<Dictionary<string, object>> serialseParametersCallback = delegate (Dictionary<string, object> parameters)
            {
                // Serialies the parameters to json or the query string depending on what type of opperation
                string functionParametersString = "";
                string parametersValuesString = "";
                string jsonBody = "";
                if (requestProperties.OperationType == OperationTypeEnum.FunctionCall)
                {
                    List<string> functionParameters = new List<string>();
                    List<string> queryString = new List<string>();
                    int count = 1;
                    // Serialise to query string parmaeters
                    foreach (string key in parameters.Keys)
                    {
                        string parameterName = "@p" + count.ToString();
                        object parameterValue = parameters[key];
                        Type parameterType = parameterValue.GetType();
                        if (parameterType == typeof(string))
                        {
                            // Strangely we need to wrap the string in single quotes - not double quotes
                            parameterValue = "'" + ((string)parameterValue).Replace("'", "\'") + "'";
                        }
                        else
                        {
                            parameterValue = Json.Stringify(parameterValue);
                        }

                        functionParameters.Add(key + "=" + parameterName);
                        queryString.Add(parameterName + "=" + parameterValue);
                        count++;
                    }
                    functionParametersString = functionParameters.Join(",");
                    parametersValuesString = queryString.Join("&");
                }
                else
                {
                    // Serialise to body json
                    jsonBody = Json.Stringify(parameters);
                }

                string entitySetName = requestMetadata != null ? requestMetadata.EntitySetName : null;
                string boundEntityId = requestProperties.BoundEntityId != null ? requestProperties.BoundEntityId.Value : null;
                string resourceName = entitySetName != null ? GetResource(entitySetName, boundEntityId) + "/" : "";
                SendRequest(requestProperties.BoundEntityLogicalName,
                   resourceName + requestProperties.RequestName + "(" + functionParametersString + ")", parametersValuesString
                   , operation, jsonBody, async
                   , endCallback
                   , errorCallback);
            };

            if (requestProperties.BoundEntityLogicalName != null)
            {
                // Bound operation - so create a resource with id
                GetEntityMetadata(requestProperties.BoundEntityLogicalName, delegate (WebApiEntityMetadata metadata)
                {
                    // Functions - eg. GET [Organization URI]/api/data/v8.2/systemusers(af9b3cf6-f654-4cd9-97a6-cf9526662797)/Microsoft.Dynamics.CRM.RetrievePrincipalAccess(Target=@tid)?@tid={'@odata.id':'contacts(9f3162f6-804a-e611-80d1-00155d4333fa)'} 
                    // Actions - eg. POST [Organization URI]/api/data/v8.2/WinOpportunity
                    /*
                     {
                     "Status": 3,
                     "OpportunityClose": {
                     "subject": "Won Opportunity",
                      "opportunityid@odata.bind": "[Organization URI]/api/data/v8.2/opportunities(b3828ac8-917a-e511-80d2-00155d2a68d2)"
                     }
                    } 
                     */
                    requestMetadata = metadata;
                    SerialiseFunctionParameterString(requestProperties, serialseParametersCallback, errorCallback, async);
                },
                errorCallback, async);
            }
            else
            {
                SerialiseFunctionParameterString(requestProperties, serialseParametersCallback, errorCallback, async);
            }

            return response; // Only for sync
        }

        private void SerialiseFunctionParameterString(WebAPIOrgnanizationRequestProperties requestProperties, Action<Dictionary<string, object>> completeCallback, Action<object> errorCallBack, bool async)
        {
            // Turn the parameters into json objects - mapping based on their type
            Dictionary<string, object> properties = new Dictionary<string, object>();
            List<object> lookupsToResolve = new List<object>();

            Dictionary<string, object> additionalProperties = requestProperties.AdditionalProperties;
            // Functions - eg. GET [Organization URI]/api/data/v8.2/systemusers(af9b3cf6-f654-4cd9-97a6-cf9526662797)/Microsoft.Dynamics.CRM.RetrievePrincipalAccess(Target=@tid)?@tid={'@odata.id':'contacts(9f3162f6-804a-e611-80d1-00155d4333fa)'} 
            foreach (string key in additionalProperties.Keys)
            {
                // If there are entity referneces then we need to resolve the lookup to the entity set 
                object value = additionalProperties[key];
                Type valueType = value.GetType();
                if (valueType == typeof(EntityReference) || valueType == typeof(Entity))
                {
                    lookupsToResolve.Add(value);
                }
                properties[key] = value;
            }

            // Resolve the entityset names of the parameters          
            MapLookupsToEntitySets(lookupsToResolve, delegate ()
            {
                // Serailise to the parameter string
                DelegateItterator.CallbackItterate(delegate (int index, Action nextPropertyCallBack, ErrorCallBack propertyError)
                {
                    string key = properties.Keys[index];
                    object propertyValue = properties[key];
                    // Turn the propertyValue into a json object
                    Attribute.SerialiseWebApiAttribute(propertyValue.GetType(), propertyValue,
                        delegate (object value)
                        {
                            properties[key] = value;
                            nextPropertyCallBack();
                        }, errorCallBack, async);

                },
                properties.Count,
                delegate ()
                {
                    // Complete
                    completeCallback(properties);
                },
                delegate (Exception ex)
                {
                    // Error
                    errorCallBack((object)ex);
                });
            }, errorCallBack, async);
        }

        public static void MapLookupsToEntitySets(List<object> lookups, Action completeCallback, Action<object> errorCallBack, bool async)
        {
            // Resolve the entityset names of the parameters          
            DelegateItterator.CallbackItterate(delegate (int index, Action nextCallBack, ErrorCallBack itterateErorCallBack)
            {
                object lookup = lookups[index];
                string logicalName = null;
                Action<WebApiEntityMetadata> resolveMetadata = null;

                if (lookup.GetType() == typeof(EntityReference))
                {
                    logicalName = ((EntityReference)lookup).LogicalName;
                    resolveMetadata = delegate (WebApiEntityMetadata metadata)
                    {
                        nextCallBack();
                    };
                }
                else if (lookup.GetType() == typeof(Entity))
                {
                    logicalName = ((Entity)lookup).LogicalName;
                    resolveMetadata = delegate (WebApiEntityMetadata metadata)
                    {
                        ((Entity)lookup)._entitySetName = metadata.EntitySetName;
                        nextCallBack();
                    };
                }

                GetEntityMetadata(logicalName, resolveMetadata
                , delegate (object error)
                {
                    itterateErorCallBack((Exception)error);
                }, async);
            },
            lookups.Count,
            completeCallback,
            delegate (Exception ex)
            {
                errorCallBack(ex);
            }
            );
        }

        private void SerialiseRequestToJSON(WebAPIOrgnanizationRequestProperties requestProperties, Action<string, string, string> completeCallback, Action<object> errorCallBack, bool async)
        {
            DelegateItterator.CallbackItterate(delegate (int index, Action nextCallBack, ErrorCallBack parameterError)
            {

            }, requestProperties.AdditionalProperties.Count,
            delegate ()
            {
                // Complete callback

            },
            delegate (Exception ex)
            {
                // Error callback
                errorCallBack((object)ex);
            });
        }

        public OrganizationResponse EndExecute(object asyncState)
        {
            CheckEndException(asyncState);
            string type = (string)Type.GetField(asyncState, "_requestName");
            // Allow custom actions/message types to be registered
            if (ExecuteMessageResponseTypes.ContainsKey(type))
            {
                Type responseType = ExecuteMessageResponseTypes[type];
                IWebAPIOrganizationResponse response = (IWebAPIOrganizationResponse)Type.CreateInstance(responseType);
                string jsonResponse = (string)Type.GetField(asyncState, "response");
                Dictionary<string, object> data = null;
                if (!String.IsNullOrEmpty(jsonResponse))
                {
                    data = Json.ParseData<Dictionary<string, object>>(jsonResponse);
                }
                response.DeserialiseWebApi(data);
                return (OrganizationResponse)response;
            }
            return null;
        }

        public EntityCollection RetrieveMultiple(string fetchXml)
        {
            return BeginRetrieveMultipleInternal(fetchXml, typeof(Entity), null);
        }

        public void BeginRetrieveMultiple(string fetchXml, Action<object> callBack)
        {
            BeginRetrieveMultipleInternal(fetchXml, null, callBack);
        }

        private EntityCollection BeginRetrieveMultipleInternal(string fetchXml, Type entityType, Action<object> callBack)
        {
            EntityCollection response = null;
            bool async = !Script.IsNullOrUndefined(callBack);
            Action<object> errorCallback = !async ? ThrowErrorCallback : callBack;
            Action<object> endCallback = !async ? (Action<object>)delegate (object state)
            {
                response = EndRetrieveMultiple(state, entityType);
            }
            : callBack;

            string logicalName = "";
            try
            {
                XmlDocument doc = XmlHelper.LoadXml(fetchXml);
                XmlNode node = XmlHelper.SelectSingleNodeXpath(doc, "fetch/entity");
                logicalName = XmlHelper.GetAttributeValue(node, "name");
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid FetchXml " + ex.Message);
            }

            GetEntityMetadata(logicalName, delegate (WebApiEntityMetadata metadata)
            {
                string query = "fetchXml=" + fetchXml.EncodeUriComponent();
                string url = BuildRequestUrl(metadata.EntitySetName, query);
                string resource = metadata.EntitySetName;
                string method = "GET";
                string json = null;

                if (UrlTooLong(url))
                {
                    // Turn the request into a batch request
                    resource = "$batch";
                    query = null;
                    json = GetBatchRequest(url);
                    method = "POST";
                }

                SendRequest(logicalName, resource, query, method, json,
                    async, endCallback, errorCallback);

            },
            errorCallback, async);
            return response;
        }

        private bool UrlTooLong(string url)
        {
            return url.Length > 2048;
        }

        private string GetBatchRequest(string url)
        {
            return @"--batch_boundary
Content-Type: application/http
Content-Transfer-Encoding: binary

GET " + url.EncodeUri() + @" HTTP/1.1
Content-Type: application/json
OData-Version: 4.0
OData-MaxVersion: 4.0

--batch_boundary--";
        }

        public Entity Retrieve(string entityName, string entityId, string[] attributesList)
        {
            Entity result = null;
            int i = 0;
            // Resolve the navigation property names from metadata
            foreach (string attributeLogicalName in attributesList)
            {
                string key = entityName + "." + attributeLogicalName;
                if (LogicalNameToNavigationMapping.ContainsKey(key))
                {
                    attributesList[i] = "_" + attributeLogicalName + "_value"; // We query the lookup value rather than the navigation property so we get either the contact or the account etc.
                }
                i++;
            }
            GetEntityMetadata(entityName, delegate (WebApiEntityMetadata metadata)
            {
                string select = attributesList != null && attributesList.Length > 0 ? "$select=" + attributesList.Join(",") : String.Empty;
                SendRequest(metadata.LogicalName, GetRecordUrl(metadata, entityId), select, "GET", null, false,
                    delegate (object state)
                    {
                        WebApiRequestResponse response = (WebApiRequestResponse)state;
                        Dictionary<string, object> data = (Dictionary<string, object>)Json.Parse(response.Response, DateReviver);
                        result = new Entity(entityName);
                        result.DeSerialiseWebApi(data);
                    }
                    , ThrowErrorCallback);

            },
           ThrowErrorCallback, false);
            return result;
        }

        public void BeginRetrieve(string entityName, string entityId, string[] attributesList, Action<object> callBack)
        {
            throw new Exception("Not Implemented");
        }

        public Entity EndRetrieve(object asyncState, Type entityType)
        {
            throw new Exception("Not Implemented");
        }

        private string GetRecordUrl(WebApiEntityMetadata metadata, string id)
        {
            id = id.ReplaceRegex(new RegularExpression(@"/[{}]/", "g"), String.Empty);
            return metadata.EntitySetName + "(" + id + ")";
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {
            BeginAssociateInternal(entityName, true, entityId, relationship, relatedEntities, null);
        }

        public void BeginAssociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            BeginAssociateInternal(entityName, true, entityId, relationship, relatedEntities, callBack);
        }

        public void BeginAssociateInternal(string entityName, bool isAssociate, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            bool async = !Script.IsNullOrUndefined(callBack);
            Action<object> errorCallback = !async ? ThrowErrorCallback : callBack;
            Action<object> endCallback = !async ? (Action<object>)delegate (object state)
            {
                EndAssociate(state);
            }
            : callBack;

            List<string> logicalNames = new List<string>();
            logicalNames.Add(entityName);
            foreach (EntityReference entityRef in relatedEntities)
            {
                logicalNames.Add(entityRef.LogicalName);
            }

            GetEntityMetadataMultiple(logicalNames, delegate (List<WebApiEntityMetadata> metadata)
            {
                DelegateItterator.CallbackItterate(delegate (int index, Action nextCallBack, ErrorCallBack errorCallBack)
                {
                    EntityReference associateto = relatedEntities[index];
                    Attribute.SerialiseWebApiAttribute(typeof(EntityReference), associateto, delegate (object value)
                    {
                        string queryString = null;
                        string json = null;
                        string resource = GetWebAPIPath() + Type.GetField(value, "@odata.id");
                        // set the context
                        if (isAssociate)
                        {
                            Type.SetField(value, "@odata.id", resource);
                            json = Json.Stringify(value);
                        }
                        else
                        {
                            queryString = "$id=" + resource;
                        }

                        WebApiEntityMetadata targetMetadata = WebApiOrganizationServiceProxy.WebApiMetadata[entityName];
                        WebApiEntityMetadata associateMetadata = WebApiOrganizationServiceProxy.WebApiMetadata[associateto.LogicalName];
                        SendRequest(entityName, GetRecordUrl(targetMetadata, entityId.Value) + "/" + relationship.SchemaName + "/$ref", queryString,
                            isAssociate ? "POST" : "DELETE"
                            , json, false, endCallback, errorCallback);

                    }, errorCallback, async);
                },
                relatedEntities.Count,
                delegate ()
                {
                    callBack(null);
                },
                delegate (Exception ex)
                {
                    errorCallback((object)ex);
                });

            }, errorCallback, async);
        }

        public void EndAssociate(object asyncState)
        {
            CheckEndException(asyncState);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities)
        {
            BeginAssociateInternal(entityName, false, entityId, relationship, relatedEntities, null);
        }

        public void BeginDisassociate(string entityName, Guid entityId, Relationship relationship, List<EntityReference> relatedEntities, Action<object> callBack)
        {
            BeginAssociateInternal(entityName, false, entityId, relationship, relatedEntities, callBack);
        }

        public void EndDisassociate(object asyncState)
        {
            CheckEndException(asyncState);
        }

        private void ThrowErrorCallback(object exception)
        {
            if (exception.GetType() == typeof(Exception))
            {
                throw (Exception)exception;
            }
            else
            {
                throw new Exception((string)exception);
            }

        }
        private static void GetEntityMetadata(string logicalName, Action<WebApiEntityMetadata> callback, Action<object> error, bool async)
        {
            GetEntityMetadataMultiple(new List<string>(logicalName), delegate (List<WebApiEntityMetadata> metadata)
           {
               callback(metadata[0]);
           }, error, async);
        }

        private static void GetEntityMetadataMultiple(List<string> logicalNames, Action<List<WebApiEntityMetadata>> callback, Action<object> error, bool async)
        {
            List<WebApiEntityMetadata> metaData = new List<WebApiEntityMetadata>();
            List<string> logicalNamesRequest = new List<string>();

            // Is the metadata cached?
            foreach (string logicalName in logicalNames)
            {
                if (WebApiMetadata.ContainsKey(logicalName))
                {
                    metaData.Add(WebApiMetadata[logicalName]);
                }
                else
                {
                    logicalNamesRequest.Add(logicalName);
                }
            }

            // Do we need to request any metadata?
            if (logicalNamesRequest.Count == 0)
            {
                callback(metaData);
                return;
            }

            // Must lookup set name http://crm/Contoso/api/data/v8.1/EntityDefinitions?$select=EntitySetName,LogicalName,PrimaryIdAttribute
            SendRequest(
                null,
                "EntityDefinitions",
                "$select=EntitySetName,LogicalName,PrimaryIdAttribute&$filter=LogicalName+eq+'" + logicalNames.Join("' or LogicalName+eq+'") + "'",
                "GET",
                null,
                async,
                delegate (object entitySetState)
                {
                    Dictionary<string, object> results = GetResponseValue(endRequest(entitySetState));
                    object[] rows = (object[])results["value"];
                    if (rows == null || rows.Length != logicalNames.Count)
                    {
                        error(new Exception(String.Format("Invalid logical name(s) '{0}'", logicalNames)));
                        return;
                    }

                    foreach (Dictionary<string, string> row in rows)
                    {
                        WebApiEntityMetadata result = GetMetadata(row);
                        metaData.Add(result);
                    }

                    callback(metaData);
                },
                error);
        }

        public static WebApiEntityMetadata GetMetadata(Dictionary<string, string> row)
        {
            WebApiEntityMetadata metadata = new WebApiEntityMetadata();
            metadata.LogicalName = row["LogicalName"];
            metadata.PrimaryAttributeLogicalName = row["PrimaryIdAttribute"];
            metadata.EntitySetName = row["EntitySetName"];
            WebApiMetadata[metadata.LogicalName] = metadata;

            return metadata;
        }
        private static WebApiRequestResponse endRequest(object state)
        {
            if (state.GetType() == typeof(WebApiRequestResponse))
            {
                return (WebApiRequestResponse)state;
            }
            else
            {
                throw (Exception)state;
            }
        }

        private Dictionary<string, string>[] GetResponseValueArray(Dictionary<string, object> response)
        {
            return (Dictionary<string, string>[])response["value"];
        }

        private static void SendRequest(string logicalname, string resource, string query, string method, string data, bool isAsync, Action<object> callBack, Action<object> error)
        {
            XmlHttpRequest req = new XmlHttpRequest();
            string url = BuildRequestUrl(resource, query);

            req.Open(method, url.EncodeUri(), isAsync);
            SetHeaders(req, 2, resource.EndsWith("$batch"));
            if (isAsync)
            {
                req.OnReadyStateChange = delegate ()
                {
                    OnReadyCallBack(req, delegate (XmlHttpRequest request)
                    {
                        WebApiRequestResponse response = new WebApiRequestResponse(request, logicalname);
                        // Tidy Up
                        Script.Literal("delete {0}", req);
                        callBack(response);

                    }, error);
                };
                req.Send(data);
            }
            else
            {
                // Tidy Up
                Script.Literal("delete {0}", req);
                req.Send(data);
                OnReadyCallBack(req, delegate (XmlHttpRequest request)
                {
                    WebApiRequestResponse response = new WebApiRequestResponse(request, logicalname);
                    callBack(response);

                }, error);
            };
        }

        private static string BuildRequestUrl(string resource, string query)
        {
            string url = GetWebAPIPath() + resource;
            if (!string.IsNullOrEmpty(query))
            {
                url += ("?" + query);
            }

            return url;
        }

        private static void OnReadyCallBack(XmlHttpRequest req, Action<XmlHttpRequest> callBack, Action<object> errorCallBack)
        {
            if (req.ReadyState == ReadyState.Loaded)
            {
                req.OnReadyStateChange = null;

                switch (req.Status)
                {
                    case 200:
                    case 204:
                    case 304:
                        callBack(req);
                        break;
                    default:
                        Exception exception = null;

                        try
                        {
                            WebApiErrorResponse responseError = Json.ParseData<WebApiErrorResponse>(req.ResponseText, null);
                            exception = GetResponseException(responseError);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                        errorCallBack(exception);
                        break;
                }
            }
        }

        private static Exception GetResponseException(WebApiErrorResponse responseError)
        {
            Exception exception;
            string message = "Unknown";
            if (!String.IsNullOrEmpty(responseError.Message))
            {
                message = responseError.Message;
            }
            else if (responseError.error != null)
            {
                message = responseError.error.message;
            }
            exception = new Exception(message);
            if (responseError.error != null && responseError.error.stacktrace != null)
            {
                Debug.WriteLine(responseError.error.stacktrace);
            }
            return exception;
        }

        public static object DateReviver(string key, object value)
        {
            if (typeof(String) == value.GetType())
            {
                string[] d = new RegularExpression(@"/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d *)?)Z$/").Exec((string)value);
                if (d != null)
                {
                    return new Date(Date.UTC(+int.Parse(d[1]), +int.Parse(d[2]) - 1, +int.Parse(d[3]), +int.Parse(d[4]), +int.Parse(d[5]), +int.Parse(d[6])));
                }
                string[] g = new RegularExpression(@"^[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$").Exec((string)value);
                if (g != null)
                {
                    return new Guid((string)value);
                }
            }
            return value;
        }

        public EntityCollection EndRetrieveMultiple(object asyncState, Type entityType)
        {
            CheckEndException(asyncState);

            WebApiRequestResponse response = (WebApiRequestResponse)asyncState;

            Dictionary<string, object> results = GetResponseValue(response);
            EntityCollection collection = null;

            // Is this a batch result?
            if (results.ContainsKey("batches"))
            {
                WebApiBatchResponse batchResponse = (WebApiBatchResponse)(object)results;
                BatchResponse innerResponse = batchResponse.batches[0];
                // Check error
                if (innerResponse.HTTPResponseCode == 500 || innerResponse.HTTPResponseCode == 400)
                {
                    Exception error = GetResponseException((WebApiErrorResponse)(object)innerResponse.response);
                    throw error;
                }
                // Get the first batch
                collection = EntityCollection.DeserialiseWebApi(entityType, response.LogicalName, innerResponse.response);
            }
            else
            {
                collection = EntityCollection.DeserialiseWebApi(entityType, response.LogicalName, results);
            }
            return collection;
        }


        private static Dictionary<string, object> GetResponseValue(WebApiRequestResponse response)
        {
            string responseText = response.Response;
            if (responseText.StartsWith("--batchresponse"))
            {
                WebApiBatchResponse batchResponse = new WebApiBatchResponse();
                batchResponse.batches = new List<BatchResponse>();

                int parsePosition = 0;
                string batchName = "--batchresponse";

                while (true)
                {
                    parsePosition = responseText.IndexOf(batchName, parsePosition);
                    if (parsePosition == -1)
                        break;
                    int lfPos = responseText.IndexOf('\n', parsePosition);
                    batchName = responseText.Substring(parsePosition, lfPos - 1);

                    BatchResponse batch = new BatchResponse();
                    batch.BatchId = batchName;

                    // Get HTTP response code
                    string httpResponseHeader = "HTTP/1.1";
                    int httpPos = responseText.IndexOf(httpResponseHeader);
                    if (httpPos > -1)
                    {
                        int httpPosSpace = responseText.IndexOf(" ", httpPos + httpResponseHeader.Length + 1);
                        batch.HTTPResponseCode = int.Parse(responseText.Substring(httpPos + httpResponseHeader.Length + 1, httpPosSpace));

                    }
                    int batchEndPos = responseText.IndexOf(batchName, lfPos);
                    if (batchEndPos > -1)
                    {
                        int startPos = responseText.IndexOf("{");
                        string batchText = responseText.Substring(startPos, batchEndPos);
                        parsePosition = batchEndPos + 1;
                        batch.response = (Dictionary<string, object>)Json.Parse(batchText, DateReviver);
                        batchResponse.batches.Add(batch);
                    }
                };
                return (Dictionary<string, object>)(object)batchResponse;
            }
            else
            {
                return (Dictionary<string, object>)Json.Parse(responseText, DateReviver);
            }
        }

        private static void SetHeaders(XmlHttpRequest req, int? pageSize, bool isBatch)
        {
            req.SetRequestHeader("Accept", "application/json");
            if (isBatch)
            {
                req.SetRequestHeader("Content-Type", "multipart/mixed;boundary=batch_boundary");
            }
            else
            {
                req.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
            }
            req.SetRequestHeader("OData-MaxVersion", "4.0");
            req.SetRequestHeader("OData-Version", "4.0");
            req.SetRequestHeader("Prefer", "odata.include-annotations=\"*\"");
            if (pageSize != null)
            {
                req.SetRequestHeader("Prefer", "odata.maxpagesize=" + pageSize);
            }
            //if (callerId)
            //{
            //    req.setRequestHeader("MSCRMCallerID", callerId);
            //}

        }

        private static string GetWebAPIPath()
        {
            return GetClientUrl() + "/api/data/v" + _webAPIVersion + "/";
        }

        private static string GetClientUrl()
        {

            if (_clientUrl == null)
            {
                _clientUrl = Page.Context.GetClientUrl();

                //_webAPIVersion = Page.Context.GetVersion();
            }

            return _clientUrl;

        }
    }
}
