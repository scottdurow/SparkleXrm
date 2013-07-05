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
            Script.Literal("debugger");
            Script.Alert("There was a problem with your request. Please contact your system administrator.\n\n" + ex.Message);
            IsBusy.SetValue(false);

        }
        #endregion

        #region Commands
        public void ActivitySearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get the option set values

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='activitypointer'>
                                <attribute name='activitytypecode' />
                                <attribute name='subject' />
                                <attribute name='activityid' />
                                <attribute name='instancetypecode' />
                                <order attribute='modifiedon' descending='false' />
                                <filter type='and'>
                                  <condition attribute='ownerid' operator='eq-userid' />
                                    <condition attribute='subject' operator='like' value='%{0}%' />
                                </filter>
                              </entity>
                            </fetch>";

            fetchXml = string.Format(fetchXml, XmlHelper.Encode(term));
            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object result)
            {

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(ActivityPointer));
                // Adjust the entity types
                foreach (ActivityPointer a in fetchResult.Entities)
                {
                    a.LogicalName = a.ActivityTypeCode;
                }
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
                            IsBusyProgress.SetValue((index / editedSessions.Count) * 100);
                            // Create/Update the session
                            if (session.dev1_sessionId == null)
                            {
                                OrganizationServiceProxy.BeginCreate(session, delegate(object result)
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
                            {
                                OrganizationServiceProxy.BeginUpdate(session, delegate(object result)
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
