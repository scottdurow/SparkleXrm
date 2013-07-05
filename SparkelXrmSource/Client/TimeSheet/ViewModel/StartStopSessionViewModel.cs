// StartStopSessionViewModel.cs
//

using System;
using System.Collections.Generic;
using KnockoutApi;
using Xrm.Sdk;
using jQueryApi;
using Client;
using SparkleXrm;
using Client.TimeSheet.Model;
using SparkleXrm.GridEditor;
using Xrm.Sdk.Metadata;
using jQueryApi.UI.Widgets;
using System.Html;
using SparkleXrm;

namespace TimeSheet.Client.ViewModel
{
    public class StartStopSessionViewModel : ViewModelBase
    {
        #region Fields
        

        public ObservableArray<EntityReference> OpenActvities = Knockout.ObservableArray<EntityReference>();
        public Observable<SessionVM> StopSession;
        public Observable<SessionVM> StartSession;
        public Observable<bool> StartNewSession = Knockout.Observable<bool>(false);
        public DependentObservable<bool> CanSave;
        #endregion

        #region Constructors
        public StartStopSessionViewModel(Guid activityToStartStop, Guid sessionToStartStop)
        {
           
           
            // Create start session
            dev1_session newSession = new dev1_session();
            newSession.dev1_StartTime = DateTime.Now;
            StartSession = (Observable<SessionVM>)ValidatedObservableFactory.ValidatedObservable(new SessionVM(this, newSession));
           
            // Load the Sessions
            dev1_session session = (dev1_session)OrganizationServiceProxy.Retrieve(dev1_session.EntityLogicalName, "{FD722AC2-B234-E211-A471-000C299FFE7D}", new string[] { "AllColumns" });
            SessionVM sessionVM = new SessionVM(this, session);
            StopSession = (Observable<SessionVM>)ValidatedObservableFactory.ValidatedObservable(sessionVM);

            DependentObservableOptions<bool> isFormValidDependantProperty = new DependentObservableOptions<bool>();
            isFormValidDependantProperty.Model = this;
            
            isFormValidDependantProperty.GetValueFunction = new Func<bool>(delegate
            {
                StartStopSessionViewModel vm = (StartStopSessionViewModel)isFormValidDependantProperty.Model;
                
                if (vm.StartNewSession.GetValue())
                {
                    return ValidationRules.AreValid(
                       new object[] { ((StartStopSessionViewModel)isFormValidDependantProperty.Model).StopSession,
                       ((StartStopSessionViewModel)isFormValidDependantProperty.Model).StartSession });
                   
                }
                else
                {
                    
                    return ValidationRules.AreValid(
                       new object[] { ((StartStopSessionViewModel)isFormValidDependantProperty.Model).StopSession });
                    
                }

            });
            CanSave = Knockout.DependentObservable<bool>(isFormValidDependantProperty);

            
        }

        #endregion

        #region Methods
        public bool IsFormValid()
        {
            return true;
        }
        #endregion

        #region Commands
        public void ActivitySearchCommand(string term,Action<EntityCollection> callback)
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

                EntityCollection fetchResult = OrganizationServiceProxy.EndRetrieveMultiple(result, typeof(Entity));
                callback(fetchResult);

                
            });
        }

        private Action _saveCommand=null;

        public Action SaveCommand()
        {
            if (_saveCommand == null)
            {
                _saveCommand = delegate()
                {
                    bool confirmed = Script.Confirm(String.Format("Are you sure save these sessions?"));
                    if (!confirmed)
                        return;

                    SessionVM stopSession = StopSession.GetValue();
                    dev1_session sessionToUpdate = stopSession.GetUpdatedModel();

                    try
                    {
                        OrganizationServiceProxy.Update(sessionToUpdate);

                        // Save next session
                        if (StartNewSession.GetValue())
                        {
                            dev1_session nextSession = StartSession.GetValue().GetUpdatedModel();
                            OrganizationServiceProxy.Create(nextSession);
                        }
                    }
                    catch (Exception ex)
                    {
                        Script.Alert("There was a problem saving your session. Please contact your system administrator.\n\n" + ex.Message);
                    }
                };
            }

            return _saveCommand;

        }

        private Action _cancelCommand = null;

        public Action CancelCommand()
        {
            if (_cancelCommand == null)
            {
                _cancelCommand = delegate()
                {
                    Window.Top.Close();
                };
            }

            return _cancelCommand;

        }
        #endregion
    }
}
