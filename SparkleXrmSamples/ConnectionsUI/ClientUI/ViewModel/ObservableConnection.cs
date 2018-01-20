// ObservableConnection.cs
//

using ClientUI.Model;
using ClientUI.ViewModels;
using ES6;
using KnockoutApi;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace ClientUI.ViewModel
{
    
    public class ObservableConnection  : ViewModelBase
    {
        #region Events
        public event Action<string> OnSaveComplete;
        #endregion

        #region Observable Fields
        [PreserveCase]
        public Observable<bool> AddNewVisible = Knockout.Observable<bool>(false);
        [ScriptName("connectiondid")]
        public Observable<Guid> ConnectionId = Knockout.Observable<Guid>();
        [ScriptName("record1id")]
        public Observable<EntityReference> Record1Id = Knockout.Observable<EntityReference>();
        [ScriptName("record2id")]
        public Observable<EntityReference> Record2Id = Knockout.Observable<EntityReference>();
        [ScriptName("record1roleid")]
        public Observable<EntityReference> Record1RoleId = Knockout.Observable<EntityReference>();
        [ScriptName("record2roleid")]
        public Observable<EntityReference> Record2RoleId = Knockout.Observable<EntityReference>();
        [ScriptName("description")]
        public Observable<string> Description = Knockout.Observable<string>();
        #endregion

        #region Private Fields
        private QueryParser _queryParser;
        private string[] connectToTypes;
        #endregion

        #region Constructors
        public ObservableConnection(string[] types)
        {
            connectToTypes = types;
            ObservableConnection.RegisterValidation(new ObservableValidationBinder(this));
                 
        }
        #endregion

        #region Commands
        [PreserveCase]
        public void RecordSearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get the option set values
            int resultsBack = 0;
            List<Entity> mergedEntities = new List<Entity>();
            Action<EntityCollection> result = delegate (EntityCollection fetchResult)
            {
                resultsBack++;
                FetchQuerySettings config = _queryParser.EntityLookup[fetchResult.EntityName].QuickFindQuery;
                // Add in the display Columns
                foreach (Dictionary<string, object> row in fetchResult.Entities)
                {
                    Entity entityRow = (Entity)(object)row;
                    int columnCount = config.Columns.Count < 3 ? config.Columns.Count : 3;
                    // Only get up to 3 columns
                    for (int i = 0; i < columnCount; i++)
                    {
                        // We use col<n> as the alias name so that we can show the correct values irrespective of the entity type
                        string aliasName = "col" + i.ToString();
                        row[aliasName] = row[config.Columns[i].Field];
                        if (entityRow.FormattedValues.ContainsKey(config.Columns[i].Field + "name"))
                        {
                            entityRow.FormattedValues[aliasName + "name"] = entityRow.FormattedValues[config.Columns[i].Field + "name"];
                        }
                        else
                        {
                            entityRow.FormattedValues[aliasName] = entityRow.GetAttributeValue(config.Columns[i].Field) as string;
                        }
                    }

                }
                // Merge in the results
                mergedEntities.AddRange((Entity[])(object)fetchResult.Entities.Items());

                mergedEntities.Sort(delegate (Entity x, Entity y) {
                    return string.Compare(x.GetAttributeValueString("name"), y.GetAttributeValueString("name"));
                });
                if (resultsBack == connectToTypes.Length)
                {
                    EntityCollection results = new EntityCollection(mergedEntities);
                    callback(results);
                }
            };

            if (_queryParser == null)
            {
                // Get the quick find metadata on first search
                _queryParser = new QueryParser(connectToTypes);
                _queryParser.GetQuickFinds().Then(delegate (object value) {
                    _queryParser.QueryMetadata();
                  
                    foreach (string entity in connectToTypes)
                    {
                        SearchRecords(term, result, entity);
                    }
                }); ;
            }
            else
            {
                foreach (string entity in _queryParser.Entities)
                {
                    SearchRecords(term, result, entity);
                }
            }
        }

        private void SearchRecords(string term, Action<EntityCollection> callback, string entityType)
        {
          
            string fetchXml = _queryParser.GetFetchXmlForQuery(entityType,"QuickFind", term, SearchTermOptions.PrefixWildcard | SearchTermOptions.SuffixWildcard);

            
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
                fetchResult.EntityName = entityType;
                callback(fetchResult);
            });
        }

        [PreserveCase]
        public void RoleSearchCommand(string term, Action<EntityCollection> callback)
        {
            EntityReference record = this.Record1Id.GetValue();
            RoleSearch(term, callback, record!=null ? record.LogicalName : null);
        }

        [PreserveCase]
        public static void RoleSearch(string term, Action<EntityCollection> callback,string typeName)
        {
            Script.Literal("debugger");
            // find the entity type code from the type name
            GetEntityTypeCodeFromName(typeName).Then(delegate (int? etc)
            {
                string recordTypeFilter = string.Empty;
                string fetchXml = @"
                            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true' >
                                <entity name='connectionrole' >
                                    <attribute name='category' />
                                    <attribute name='name' />
                                    <attribute name='connectionroleid' />
                                    <attribute name='statecode' />
                                    <order attribute='name' descending='false' />
                                    <link-entity name='connectionroleobjecttypecode' from='connectionroleid' to='connectionroleid' >
                                    {1}
                                    </link-entity>
                                    <filter type='and'>                                     
                                        <condition attribute='name' operator='like' value='%{0}%' />
                                    </filter>
                                </entity>
                            </fetch>";

                if (etc != null)
                {
                    // Filter by the currently select role
                    recordTypeFilter = String.Format(@"
                                    <filter type='or'>
                                        <condition attribute='associatedobjecttypecode' operator='eq' value='{0}' />
                                        <condition attribute='associatedobjecttypecode' operator='eq' value='0' />
                                    </filter>", etc);
                }
                fetchXml = string.Format(fetchXml, XmlHelper.Encode(term), recordTypeFilter);
                OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate (object result)
                {
                    EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
                    callback(fetchResult);
                });
            });
  
        }

        [PreserveCase]
        public void SaveCommand()
        {
            if (!((IValidatedObservable)this).IsValid())
            {
                ((IValidatedObservable)(object)this).Errors.ShowAllMessages(true);
                return;
            }

            this.IsBusy.SetValue(true);
            this.AddNewVisible.SetValue(false);

            Connection connection = new Connection();
            connection.Record1Id = Record1Id.GetValue();
            connection.Record2Id = Record2Id.GetValue();
            connection.Record1RoleId = Record1RoleId.GetValue();
            connection.Record2RoleId = Record2RoleId.GetValue();

            GetOppositeRole(connection.Record1RoleId,  connection.Record2Id)
                .Then(delegate(EntityReference oppositeRole)
            {
                connection.Record2RoleId = oppositeRole;

                OrganizationServiceProxy.BeginCreate(connection, delegate (object state)
                {
                    try
                    {
                        ConnectionId.SetValue(OrganizationServiceProxy.EndCreate(state));
                        OnSaveComplete(null);
                        Record1Id.SetValue(null);
                        Record1RoleId.SetValue(null);
                        ((IValidatedObservable)(object)this).Errors.ShowAllMessages(false);


                    }
                    catch (Exception ex)
                    {
                        // Something went wrong - report it
                        OnSaveComplete(ex.Message);
                    }
                    finally
                    {
                        this.IsBusy.SetValue(false);
                    }

                });

            });
           
           
        }

        private static Promise<int?> GetEntityTypeCodeFromName(string typeName)
        {
            if (typeName==null)
            {
                return new Promise<int?>(delegate (Action<int?> resolve)
                {
                    resolve(0);
                });
            }
            else
            {
                return Utility.GetEntityMetadata(typeName).Then(delegate (EntityMetadata metadata)
                {
                    return metadata.ObjectTypeCode;
                });
            }
        }

        public static Promise<EntityReference> GetOppositeRole(EntityReference role, EntityReference record)
        {   
            return GetEntityTypeCodeFromName(record.LogicalName).Then<EntityReference>(delegate(int? etc)
            {
                EntityReference oppositeRole = null;
                // Add the opposite connection role
                string getOppositeRole = String.Format(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' count='1'>
                          <entity name='connectionrole'>
                            <attribute name='category' />
                            <attribute name='name' />
                            <attribute name='connectionroleid' />
                            <attribute name='statecode' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                            </filter>
                            <link-entity name='connectionroleassociation' from='connectionroleid' to='connectionroleid' intersect='true'>
                                  <link-entity name='connectionrole' from='connectionroleid' to='associatedconnectionroleid' alias='ad'>
                                    <filter type='and'>
                                      <condition attribute='connectionroleid' operator='eq' value='{0}' />
                                    </filter>
                                  </link-entity>
                                 <link-entity name='connectionroleobjecttypecode' from='connectionroleid' to='connectionroleid' intersect='true' >
                                    <filter type='or' >
                                        <condition attribute='associatedobjecttypecode' operator='eq' value='{1}' />
                                        <condition attribute='associatedobjecttypecode' operator='eq' value='0' /> <!-- All types-->
                                    </filter>
                                </link-entity>
                            </link-entity>
                          </entity>
                        </fetch>", role.Id.ToString(), etc);

                EntityCollection results = (EntityCollection)OrganizationServiceProxy.RetrieveMultiple(getOppositeRole);

                if (results.Entities.Count > 0)
                {
                    oppositeRole = results.Entities[0].ToEntityReference();
                }
                return oppositeRole;
            }); 
        }

        [PreserveCase]
        public void CancelCommand()
        {
            this.AddNewVisible.SetValue(false);

        }

        public static ValidationRules ValidateRecord1Id(ValidationRules rules, object viewModel, object dataContext)
        {
            return rules
               .AddRule(ResourceStrings.RequiredMessage, delegate(object value)
               {
                   return (value != null) && ((EntityReference)value).Id != null;

               });
            


        }
        public static ValidationRules ValidateRecord1RoleId(ValidationRules rules, object viewModel, object dataContext)
        {
            return rules
               .AddRule(ResourceStrings.RequiredMessage, delegate(object value)
               {
                   return (value != null) && ((EntityReference)value).Id != null;

               });



        }
        public static void RegisterValidation(ValidationBinder binder)
        {
            binder.Register("record1id", ValidateRecord1Id);
            binder.Register("record1roleid", ValidateRecord1RoleId);
        }
        #endregion
    }
}
