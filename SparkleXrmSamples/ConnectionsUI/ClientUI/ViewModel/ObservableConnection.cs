// ObservableConnection.cs
//

using ClientUI.Model;
using KnockoutApi;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

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
        #endregion

        #region Private Fields
        private Dictionary<string, string> connectToTypes;
        #endregion

        #region Constructors
        public ObservableConnection(Dictionary<string, string> types)
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
            Action<EntityCollection> result = delegate(EntityCollection fetchResult)
            {
                resultsBack++;
                // Merge in the results
                mergedEntities.AddRange((Entity[])(object)fetchResult.Entities.Items());
                
                mergedEntities.Sort(delegate (Entity x, Entity y){
                    return string.Compare(x.GetAttributeValueString("name"), y.GetAttributeValueString("name"));
                });
                if (resultsBack == connectToTypes.Count)
                {
                    EntityCollection results = new EntityCollection(mergedEntities);
                    callback(results);
                }
            };

            foreach (string entity in connectToTypes.Keys)
            {
                SearchRecords(term, result, entity, connectToTypes[entity]);
            }
        }

        private void SearchRecords(string term, Action<EntityCollection> callback, string entityType, string entityNameAttribute)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true' count='25'>
                              <entity name='{1}'>
                                <attribute name='{2}' alias='name' />
                                <order attribute='{2}' descending='false' />
                                <filter type='and'>
                                  <condition attribute='{2}' operator='like' value='%{0}%' />
                                </filter>
                              </entity>
                            </fetch>";

            fetchXml = string.Format(fetchXml, XmlHelper.Encode(term), entityType, entityNameAttribute);
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
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
            string recordTypeFilter = string.Empty;

            if (typeName != null)
            {
                // find the entity type code from the type name
                int? etc = (int?)Script.Literal("Mscrm.EntityPropUtil.EntityTypeName2CodeMap[{0}]", typeName);
                // Filter by the currently select role
                recordTypeFilter = String.Format(@"
                                        <filter>
                                            <condition attribute='associatedobjecttypecode' operator='eq' value='{0}' />
                                        </filter>", etc);
            }
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
                                    <filter>
                                        <condition attribute='name' operator='like' value='%{0}%' />
                                    </filter>
                                </entity>
                            </fetch>";

            fetchXml = string.Format(fetchXml, XmlHelper.Encode(term), recordTypeFilter);
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
                callback(fetchResult);
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

            OrganizationServiceProxy.BeginCreate(connection,delegate(object state)
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
