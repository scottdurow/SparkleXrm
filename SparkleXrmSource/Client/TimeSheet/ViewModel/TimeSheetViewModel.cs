// TimeSheetViewModel.cs
//

using System;
using System.Collections.Generic;
using Xrm.Sdk;
using KnockoutApi;
using Xrm;
using SparkleXrm.GridEditor;
using SparkleXrm;

namespace Client.TimeSheet.ViewModel
{
    public class TimeSheetViewModel : ViewModelBase
    {
        #region Fields
        public SessionsViewModel SessionDataView;
        public DaysViewModel Days;
        public Observable<DateTime> StartTimeTime = Knockout.Observable<DateTime>();
        #endregion

        #region Constructors
        public TimeSheetViewModel()
        {


            SessionDataView = new SessionsViewModel();
            Days = new DaysViewModel(SessionDataView);

            SessionDataView.SetCurrentWeek(DateTime.Today);
            StartTimeTime.SetValue(DateTime.Parse("1975-01-16T13:00:00"));

        }
        #endregion

        #region Methods
        private void ReportError(Exception ex)
        {
 
            Script.Alert("There was a problem with your request. Please contact your system administrator.\n\n" + ex.Message);
            IsBusy.SetValue(false);

        }
        #endregion

        #region Commands
        public void RegardingObjectSearchCommand(string term, Action<EntityCollection> callback)
        {
            string regardingAccountFetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' />
                                    <attribute name='accountid' />
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statecode' operator='eq' value='0' />
                                      <condition attribute='name' operator='like' value='%{0}%' />
                                      {1}
                                    </filter>
                                   </entity>
                                </fetch>";
            string regardingOpportunityFetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                                  <entity name='opportunity'>
                                                    <attribute name='name' />
                                                    <attribute name='opportunityid' />
                                                    <order attribute='name' descending='false' />
                                                    <filter type='and'>
                                                      <condition attribute='statecode' operator='eq' value='0' />
                                                      <condition attribute='name' operator='like' value='%{0}%' />
                                                     {1}
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            string regardingIncidentFetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                              <entity name='incident'>
                                                <attribute name='title' />
                                                <attribute name='incidentid' />
                                                <order attribute='title' descending='false' />
                                                <filter type='and'>
                                                  <condition attribute='title' operator='like' value='%{0}%' />
                                                  {1}
                                                </filter>
                                              </entity>
                                            </fetch>";

            string accountCriteriaFetchXml = @"<condition attribute='{0}' operator='eq' value='{1}' />";
            DayEntry selectedItem = this.Days.SelectedItems[0];
            

            List<Entity> unionedResults = new List<Entity>();

            // We need to union the activities regarding the account directly with those that are regarding related records
            Action<string, string, string, Action, Action> unionSearch = delegate(string fetchXml,string nameAttribute,string accountAttribute, Action completeCallBack, Action errorCallBack)
            {
                // Add Account filter if an account is selected
                string additionalCriteria = "";
                if (selectedItem != null && selectedItem.Account != null)
                {
                    additionalCriteria = string.Format(accountCriteriaFetchXml, accountAttribute, selectedItem.Account.Id.Value);
                }
                string queryFetchXml = string.Format(fetchXml, XmlHelper.Encode(term),additionalCriteria);
                OrganizationServiceProxy.BeginRetrieveMultiple(queryFetchXml, delegate(object result)
                {
                    EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(ActivityPointer));
                    // Adjust the entity types
                    foreach (Entity a in fetchResult.Entities)
                    {
                       // Set name
                        a.SetAttributeValue("displayName", a.GetAttributeValueString(nameAttribute));
                        unionedResults.Add(a);
                    }

                    completeCallBack();
                });
            };

            TaskIterrator tasks = new TaskIterrator();

            // Add Searches
            // TODO
            /*
            tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(regardingAccountFetchXml,"name", "accountid", completeCallBack, errorCallBack); },null);
            tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(regardingOpportunityFetchXml, "name","customerid", completeCallBack, errorCallBack); },null);
            tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(regardingIncidentFetchXml, "title", "customerid",completeCallBack, errorCallBack); },null);
            */
            Action queryComplete = delegate()
            {
                //  Sort Alphabetically
                unionedResults.Sort(delegate(Entity a, Entity b) { return Entity.SortDelegate("displayName", a, b); });
                // Completed the queryies, so sort then and add to Entity Collection
                EntityCollection results = new EntityCollection(unionedResults);

                callback(results);
            };

            // Start processing queue   
            tasks.Start(queryComplete, null);
        }

        public void ActivitySearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get the option set values

                string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='activitypointer'>
                                    <attribute name='activitytypecode' />
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='instancetypecode' />
                                    <attribute name='regardingobjectid' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='ownerid' operator='eq-userid' />
                                        <condition attribute='subject' operator='like' value='%{0}%' />
                                          <condition attribute='activitytypecode' operator='in'>
                                            <value>4202</value>
                                            <value>4207</value>
                                            <value>4210</value>
                                            <value>4212</value>
                                          </condition>
                                            {2}
                                    </filter>
                                    {1}
                                  </entity>
                                </fetch>";
            // Get the account to filter on if there is one
            string regardingObjectIdFilterFetchXml = @"<condition attribute='regardingobjectid' operator='eq' value='{0}'/>";

            string opportunityAccountFilterFetchXml = @"<link-entity name='opportunity' from='opportunityid' to='regardingobjectid' visible='false' link-type='inner' alias='opportunity' >
                                        <attribute name='customerid' />
                                        <filter type='and' >
                                            <condition attribute='customerid' operator='eq' value='{0}' />
                                        </filter>
                                        </link-entity>";

            string incidentAccountFilterFetchXml = @" <link-entity name='incident' from='incidentid' to='regardingobjectid' visible='false' link-type='inner' alias='incident' >
                                        <attribute name='customerid' />
                                        <filter type='and' >
                                            <condition attribute='customerid' operator='eq' value='{0}' />
                                        </filter>
                                        </link-entity>";
            
            string contractAccountFilterFetchXml = @" <link-entity name='contract' from='contractid' to='regardingobjectid' visible='false' link-type='inner' alias='contract' >
                                        <attribute name='customerid' />
                                        <filter type='and' >
                                            <condition attribute='customerid' operator='eq' value='{0}' />
                                        </filter>
                                        </link-entity>";

            string regardingAccountFilterFetchXml = @" <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='inner' alias='account' >
                                        <attribute name='accountid'/>
                                        <attribute name='name'/>
                                        <filter type='and' >
                                            <condition attribute='accountid' operator='eq' value='{0}' />
                                        </filter>
                                        </link-entity>";

            DayEntry selectedItem = this.Days.SelectedItems[0];
            string regardingObjectFilter = String.Empty;
            string regardingAccountFilter = String.Empty;
            string opportunityAccountFilter = String.Empty;
            string incidentAccountFilter = String.Empty;
            string contractAccountFilter = String.Empty;

            if (selectedItem != null && selectedItem.RegardingObjectId != null)
            {
                regardingObjectFilter = string.Format(regardingObjectIdFilterFetchXml, selectedItem.RegardingObjectId.Id.Value);
            }

            if (selectedItem != null && selectedItem.Account!=null)
            {
                // Add in the regarding account filter
                regardingAccountFilter = string.Format(regardingAccountFilterFetchXml, selectedItem.Account.Id.Value);
                opportunityAccountFilter = string.Format(opportunityAccountFilterFetchXml, selectedItem.Account.Id.Value);
                incidentAccountFilter = string.Format(incidentAccountFilterFetchXml, selectedItem.Account.Id.Value);
                contractAccountFilter = string.Format(contractAccountFilterFetchXml, selectedItem.Account.Id.Value);
            }
            
            List<Entity> unionedResults = new List<Entity>();

            // We need to union the activities regarding the account directly with those that are regarding related records
            Action<string, string, Action, Action> unionSearch = delegate(string additionalFilter, string additionalCriteria, Action completeCallBack, Action errorCallBack)
            {
                string queryFetchXml = string.Format(fetchXml, XmlHelper.Encode(term), additionalFilter,additionalCriteria);
                OrganizationServiceProxy.BeginRetrieveMultiple(queryFetchXml, delegate(object result)
                {
                    EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(ActivityPointer));
                    // Adjust the entity types
                    foreach (ActivityPointer a in fetchResult.Entities)
                    {
                        a.UpdateCalculatedFields();
                        unionedResults.Add(a);
                    }

                    completeCallBack();
                });
            };

            TaskIterrator tasks = new TaskIterrator();

            // TODO
            /*
            // Default Search
            tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(regardingAccountFilter, regardingObjectFilter, completeCallBack, errorCallBack); }, null);
            
            // Associated record searches
            if (opportunityAccountFilter!=String.Empty)
                tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(opportunityAccountFilter, String.Empty, completeCallBack, errorCallBack); },null);
            if (incidentAccountFilter!=String.Empty)
                tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(incidentAccountFilter, String.Empty, completeCallBack, errorCallBack); },null);
            if (contractAccountFilter!=String.Empty)
                tasks.AddTask(delegate(Action completeCallBack, Action errorCallBack, object state) { unionSearch(contractAccountFilter, String.Empty, completeCallBack, errorCallBack); },null);
            */

            Action queryComplete = delegate()
            {
                // Sort
                unionedResults.Sort(delegate(Entity a, Entity b) { return Entity.SortDelegate("subject", a, b); });
                // Completed the queryies, so sort then and add to Entity Collection
                EntityCollection results = new EntityCollection(unionedResults);
                
                callback(results);
            };

            // Start processing queue   
            tasks.Start(queryComplete, null);

           
        }
        public void AccountSeachCommand(string term, Action<EntityCollection> callback)
        {
            // Get the option set values

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' />
                                <attribute name='accountid' />
                                <order attribute='name' descending='false' />
                                <filter type='and'>
                                  <filter type='or'>
                                    <condition attribute='name' operator='like' value='%{0}%' />
                                    <condition attribute='accountnumber' operator='like' value='%{0}%' />
                                  </filter>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            fetchXml = string.Format(fetchXml, XmlHelper.Encode(term));
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
               
                callback(fetchResult);


            });
        }
        private Action _saveCommand = null;

        public Action SaveCommand()
        {
            if (_saveCommand == null)
            {
                _saveCommand = delegate()
                {
                    bool confirmed = Script.Confirm(String.Format("Are you sure save these sessions?"));
                    if (!confirmed)
                        return;

                    try
                    {
                        IsBusy.SetValue(true);
                        IsBusyProgress.SetValue(0);
                        IsBusyMessage.SetValue("Saving...");
                        // Create a an array of sessions to be saved
                        List<dev1_session> editedSessions = SessionDataView.GetEditedSessions();


                        DelegateItterator.CallbackItterate(delegate(int index, Action nextCallback, ErrorCallBack errorCallBack)
                        {
                            dev1_session session = editedSessions[index];
                            // Create verson of session to save
                            dev1_session sessionToSave = new dev1_session();
                            sessionToSave.dev1_sessionId = session.dev1_sessionId;
                            sessionToSave.dev1_ActivityId = session.dev1_ActivityId;
                            sessionToSave.dev1_ActivityTypeName = session.dev1_ActivityTypeName;
                            sessionToSave.dev1_Description = session.dev1_Description;
                            sessionToSave.dev1_Duration = session.dev1_Duration;
                            sessionToSave.dev1_EmailId = session.dev1_EmailId;
                            sessionToSave.dev1_EndTime = session.dev1_EndTime;
                            sessionToSave.dev1_LetterId = session.dev1_LetterId;
                            sessionToSave.dev1_PhoneCallId = session.dev1_PhoneCallId;
                            sessionToSave.dev1_sessionId = session.dev1_sessionId;
                            sessionToSave.dev1_StartTime = session.dev1_StartTime;
                            sessionToSave.dev1_TaskId = session.dev1_TaskId;
                            sessionToSave.dev1_Row = session.dev1_Row;
                            IsBusyProgress.SetValue((index / editedSessions.Count) * 100);
                            // Create/Update the session
                            if (session.dev1_sessionId == null)
                            {
                                if (sessionToSave.dev1_Duration != null && sessionToSave.dev1_Duration > 0)
                                {
                                    OrganizationServiceProxy.BeginCreate(sessionToSave, delegate(object result)
                                    {
                                        IsBusyProgress.SetValue((index / editedSessions.Count) * 100);
                                        try
                                        {
                                            session.dev1_sessionId = OrganizationServiceProxy.EndCreate(result);
                                            session.EntityState = EntityStates.Unchanged;
                                            session.RaisePropertyChanged("EntityState");
                                            nextCallback();
                                        }
                                        catch (Exception ex)
                                        {
                                            // TODO: Mark error row
                                            Script.Alert(ex.Message);
                                            nextCallback();
                                        }
                                    });
                                }
                                else
                                    nextCallback();

                            }
                            else
                            {
                                OrganizationServiceProxy.BeginUpdate(sessionToSave, delegate(object result)
                                {
                                    try
                                    {
                                        OrganizationServiceProxy.EndUpdate(result);
                                        session.EntityState = EntityStates.Unchanged;
                                        session.RaisePropertyChanged("EntityState");
                                        nextCallback();
                                    }
                                    catch (Exception ex)
                                    {
                                        // TODO: Mark error row
                                        Script.Alert(ex.Message);
                                        nextCallback();
                                    }
                                });

                            }

                        },
                        editedSessions.Count,
                        delegate()
                        {
                            // Completed
                            IsBusyProgress.SetValue(100);
                            IsBusy.SetValue(false);

                        },
                        delegate(Exception ex)
                        {
                            // Error
                            ReportError(ex);

                        });
                    }
                    catch (Exception ex)
                    {
                        ReportError(ex);
                    }
                };
            }
            return _saveCommand;
        }

        private Action _deleteCommand = null;

        public Action DeleteCommand()
        {
            if (_deleteCommand == null)
            {
                _deleteCommand = delegate()
               {
                   List<int> selectedRows = DataViewBase.RangesToRows(this.SessionDataView.GetSelectedRows());
                   if (selectedRows.Count == 0)
                       return;

                   bool confirmed = Script.Confirm(String.Format("Are you sure you want to delete the {0} selected sessions?", selectedRows.Count));
                   if (!confirmed)
                       return;

                   
                   IsBusyMessage.SetValue("Deleting Sessions...");
                   IsBusyProgress.SetValue(0);
                   IsBusy.SetValue(true);
                   
                   // Get each item to remove
                   List<dev1_session> itemsToDelete = new List<dev1_session>();
                   for (int i=0;i<selectedRows.Count;i++)
                   {
                       itemsToDelete.Add((dev1_session)this.SessionDataView.GetItem(i));
                   }
                   DelegateItterator.CallbackItterate(delegate(int index, Action nextCallback, ErrorCallBack errorCallBack)
                    {
                        dev1_session session = itemsToDelete[index];
                        IsBusyProgress.SetValue((index / selectedRows.Count) * 100);
                        if (session.dev1_sessionId != null)
                        {
                            OrganizationServiceProxy.BeginDelete(session.LogicalName, session.dev1_sessionId, delegate(object result)
                            {
                                try
                                {
                                    OrganizationServiceProxy.EndDelete(result);
                                    this.SessionDataView.RemoveItem(session);
                                    this.SessionDataView.Refresh();
                                    nextCallback();
                                }
                                catch (Exception ex)
                                {
                                    Script.Alert(ex.Message);
                                    nextCallback();
                                }

                            });
                        }
                        else
                        {
                            this.SessionDataView.RemoveItem(session);
                            this.SessionDataView.Refresh();
                            nextCallback();
                        }
                    },
                    selectedRows.Count,
                    delegate()
                    {
                        // Complete
                        IsBusyProgress.SetValue(100);
                        IsBusy.SetValue(false);
                        SessionDataView.Refresh();
                        Days.ReCalculate();


                    },
                    delegate(Exception ex)
                   {
                       ReportError(ex);
                   }
                    );



               };
            }
            return _deleteCommand;
        }

        private Action _resetCommand = null;

        public Action ResetCommand()
        {
            if (_resetCommand == null)
            {

            }
            return _resetCommand;
        }
        #endregion

    }
}
