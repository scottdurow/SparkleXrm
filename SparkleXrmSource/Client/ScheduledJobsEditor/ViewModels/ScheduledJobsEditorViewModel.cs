// EditBulkDeleteJobsViewModel.cs
//

using SparkleXrm.GridEditor;
using Client.ScheduledJobsEditor.Model;
using Client.TimeSheet.ViewModel;
using KnockoutApi;
using SparkleXrm;

using System;
using System.Collections.Generic;
using System.Html;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Messages;
using Slick;

namespace Client.ScheduledJobsEditor.ViewModels
{

    public class ScheduledJobsEditorViewModel : ViewModelBase
    {
        public EntityDataViewModel JobsViewModel = new EntityDataViewModel(2,typeof(Entity),true);
        public EntityDataViewModel bulkDeleteJobsViewModel = new EntityDataViewModel(10, typeof(Entity),true);
        public RecurranceFrequency[] RecurranceFrequencies = RecurrancePatternMapper.RecurranceFrequencies;
        public Observable<ScheduledJob> SelectedJob = (Observable<ScheduledJob>)ValidatedObservableFactory.ValidatedObservable(new ScheduledJob());
        public ScheduledJobsEditorViewModel()
        {
           
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>
                              <entity name='dev1_scheduledjob'>
                                <attribute name='dev1_scheduledjobid' />
                                <attribute name='dev1_name' />
                                <attribute name='createdon' />
                                <attribute name='dev1_workflowname' />
                                <attribute name='dev1_recurrancepattern' />
                                <attribute name='dev1_startdate' />
                                <attribute name='dev1_enabled' />
                                {3}
                              </entity>
                            </fetch>";
            JobsViewModel.FetchXml = fetchXml;
            JobsViewModel.OnSelectedRowsChanged += jobsViewModel_OnSelectedRowsChanged;

            
        }

       
        void jobsViewModel_OnSelectedRowsChanged()
        {
            // Get the selected bulk delete
            SelectedRange[] selectedRows = JobsViewModel.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                ScheduledJob job = SelectedJob.GetValue();

                dev1_ScheduledJob item = (dev1_ScheduledJob)JobsViewModel.GetItem(selectedRows[0].FromRow.Value);

                job.RecurrancePattern.SetValue(item.dev1_RecurrancePattern);
                RecurrancePatternMapper.DeSerialise(job, item.dev1_RecurrancePattern);
                job.ScheduledJobId.SetValue(item.dev1_ScheduledJobId);
                job.Name.SetValue(item.dev1_Name);
                job.StartDate.SetValue(item.dev1_StartDate);
                job.RecurrancePattern.SetValue(item.dev1_RecurrancePattern);
                
                EntityReference entityName = new EntityReference(null,null,item.dev1_WorkflowName);
               
                job.WorkflowId.SetValue(entityName);

                // Update the dependant data grid
                string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' returntotalrecordcount='true' no-lock='true' distinct='false' count='{0}' paging-cookie='{1}' page='{2}'>
                            <entity name='bulkdeleteoperation'>
                            <attribute name='name' />
                            <attribute name='createdon' />
                            <attribute name='asyncoperationid' />
                            <filter type='and'>
                            <condition attribute='name' operator='like' value='%" + item.dev1_ScheduledJobId.Value + @"%' />
                            </filter>
                            <link-entity name='asyncoperation' to='asyncoperationid' from='asyncoperationid' link-type='inner' alias='a0'>
                            <attribute name='postponeuntil' alias='asyncoperation_postponeuntil' />
                            <attribute name='statecode' alias='asyncoperation_statecode' />
                            <attribute name='statuscode'  alias='asyncoperation_statuscode' />
                            <attribute name='recurrencepattern'  alias='asyncoperation_recurrencepattern' />
                            </link-entity>{3}
                            </entity>
                            </fetch>";

                bulkDeleteJobsViewModel.FetchXml = fetchXml;
                bulkDeleteJobsViewModel.Reset();
                bulkDeleteJobsViewModel.Refresh();
            }
        }

        #region Commands
        public void WorkflowSearchCommand(string term, Action<EntityCollection> callback)
        {
            // Get the workflows that match the search term

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='workflow'>
                                <attribute name='workflowid' />
                                <attribute name='name' />
                                <order attribute='modifiedon' descending='false' />
                                <filter type='and'>
                                    <condition attribute='name' operator='like' value='%{0}%' />
                                    <condition attribute='type' value='1' operator='eq'/>
                                    <condition attribute='category' value='0' operator='eq'/>
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
        private Action _deleteCommand = null;

        public Action DeleteCommand()
        {
            if (_deleteCommand == null)
            {
                _deleteCommand = delegate()
                {
                    // Delete all selected jobs in grid
                    SelectedRange[] ranges = JobsViewModel.GetSelectedRows();
                    List<int> rows = DataViewBase.RangesToRows(ranges);

                    bool confirmed = Script.Confirm(String.Format("Are you sure you want to delete these {0} job(s)?",rows.Count));
                    if (!confirmed)
                        return;

                  

                    IsBusy.SetValue(true);
                    DelegateItterator.CallbackItterate(delegate(int index, Action nextCallback, ErrorCallBack errorCallBack)
                    {
                        dev1_ScheduledJob job = (dev1_ScheduledJob)this.JobsViewModel.GetItem(rows[index]);
                        // First delete the scheduled jobs associated
                        DeleteBulkDeleteJobs(job.dev1_ScheduledJobId,delegate(){
                            OrganizationServiceProxy.BeginDelete(dev1_ScheduledJob.EntityLogicalName,job.dev1_ScheduledJobId, delegate(object result)
                            {
                                try
                                {
                                    OrganizationServiceProxy.EndDelete(result);
                                    IsBusyProgress.SetValue((index / rows.Count) * 100);
                                    nextCallback();
                                }
                                catch (Exception ex)
                                {
                                    errorCallBack(ex);
                                }
                            });

                        });
                    },
                    rows.Count,
                    delegate()
                    {
                        // Completed
                        IsBusy.SetValue(false);
                        JobsViewModel.Reset();
                        JobsViewModel.Refresh();
                    },
                    delegate(Exception ex)
                    {
                        // Error
                        ReportError(ex);

                    });

                   
                };
            }
            return _deleteCommand;
        }
        
        private Action _newCommand = null;

        public Action NewCommand()
        {
            if (_newCommand == null)
            {
                _newCommand = delegate()
                {
                    // Remove selected rows
                   SelectedRange[] rows = new SelectedRange[0];
                   this.JobsViewModel.RaiseOnSelectedRowsChanged(rows);

                   // Create new schedule
                   ScheduledJob job = SelectedJob.GetValue();
                   job.Reset();
                };
            }
            return _newCommand;
        }

        private Action _saveCommand = null;

        public Action SaveCommand()
        {
            if (_saveCommand == null)
            {
                _saveCommand = delegate()
                {
                    if (!((IValidatedObservable)SelectedJob).IsValid())
                    {
                        ValidationErrors validationResult = ValidationApi.Group(SelectedJob.GetValue());
                        validationResult.ShowAllMessages(true);
                        return;
                    }

                    bool confirmed = Script.Confirm(String.Format("Are you sure you want to save this schedule?"));
                    if (!confirmed)
                        return;


                    IsBusy.SetValue(true);
                    IsBusyProgress.SetValue(0);
                    IsBusyMessage.SetValue("Saving...");



                    // Create a new Scheduled Job
                    dev1_ScheduledJob jobToSave = new dev1_ScheduledJob();
                    ScheduledJob job = this.SelectedJob.GetValue();
                    jobToSave.dev1_Name = job.Name.GetValue();
                    jobToSave.dev1_StartDate = job.StartDate.GetValue();
                    jobToSave.dev1_WorkflowName = job.WorkflowId.GetValue().Name;
                    
                    jobToSave.dev1_RecurrancePattern = RecurrancePatternMapper.Serialise(job);

                    if (job.ScheduledJobId.GetValue() == null)
                    {
                        // Create the schedule
                       
                        OrganizationServiceProxy.BeginCreate(jobToSave, delegate(object createJobResponse)
                        {
                            try
                            {
                                job.ScheduledJobId.SetValue(OrganizationServiceProxy.EndCreate(createJobResponse));
                                CreateBulkDeleteJobs(job);
                              
                            }
                            catch (Exception ex)
                            {
                                ReportError(ex);
                            }

                        });
                    }
                    else
                    {
                        jobToSave.dev1_ScheduledJobId = job.ScheduledJobId.GetValue();
                        // Update the schedule
                        OrganizationServiceProxy.BeginUpdate(jobToSave, delegate(object createJobResponse)
                        {
                            try
                            {
                                OrganizationServiceProxy.EndUpdate(createJobResponse);
                                DeleteBulkDeleteJobs(job.ScheduledJobId.GetValue(), delegate()
                                {
                                    // Create new jobs
                                    CreateBulkDeleteJobs(job);
                                });

                            }
                            catch (Exception ex)
                            {
                                ReportError(ex);
                            }

                        });  
                    }
                };
            }

            return _saveCommand;

        }
        #endregion

        #region Methods

        private void DeleteBulkDeleteJobs(Guid scheduledJobId, Action callback)
        {
            IsBusyMessage.SetValue("Deleting existing schedule...");
            // Get each bulk delete using the name = Scheduled Job {xxxx}
            string fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                      "<entity name='bulkdeleteoperation'>" +
                        "<attribute name='name' />" +
                        "<attribute name='asyncoperationid' />" +
                          "<link-entity name='asyncoperation' alias='a0' to='asyncoperationid' from='asyncoperationid'>" +
                          "<attribute name='statecode' alias='asyncoperation_statecode'/>"+
                          "<attribute name='statuscode'  alias='asyncoperation_statuscode'/>" +
                          "</link-entity>"+
                        "<filter type='and'>" +
                          "<condition attribute='name' operator='like' value='%" + scheduledJobId.ToString() + "%' />" +
                        "</filter>" +
                      "</entity>" +
                    "</fetch>";

            OrganizationServiceProxy.BeginRetrieveMultiple(fetchXml, delegate(object fetchJobsResponse)
            {
                try
                {
                    // For each item, delete
                    EntityCollection jobs = OrganizationServiceProxy.EndRetrieveMultiple(fetchJobsResponse, typeof(BulkDeleteOperation));
                    List<PendingDelete> deleteItems = new List<PendingDelete>();

                    IsBusyProgress.SetValue(0);
 
                    foreach (BulkDeleteOperation item in jobs.Entities)
                    {
                        // First delete the job
                        PendingDelete deleteJobOperationRequest = new PendingDelete();
                        deleteJobOperationRequest.entityName = BulkDeleteOperation.EntityLogicalName;
                        deleteJobOperationRequest.id = item.BulkDeleteOperationId;
                        ArrayEx.Add(deleteItems, deleteJobOperationRequest);
                       

                        // then the async operation
                        PendingDelete deleteAsyncOperationRequest = new PendingDelete();
                        deleteAsyncOperationRequest.entityName = asyncoperation.EntityLogicalName;
                        deleteAsyncOperationRequest.id = item.AsyncOperationId.Id;

                        // if the job is suspended/waiting then cancel
                        deleteAsyncOperationRequest.cancelFirst = (item.AsyncOperation_StateCode.Value == 1);
                        ArrayEx.Add(deleteItems, deleteAsyncOperationRequest);
                        
                    }

                    // Delete each in turn
                   
                    DeleteJob(deleteItems, callback); 
                    
                }
                catch (Exception ex)
                {
                    ReportError(ex);
                }

            });

        }


        private void DeleteJob(List<PendingDelete> items, Action completedCallback)
        {

            DelegateItterator.CallbackItterate(delegate(int index,Action nextCallback,ErrorCallBack errorCallBack)
            {
                // Do work
                PendingDelete pendingDeleteItem = items[index];

                if (pendingDeleteItem.cancelFirst)
                {
                    // If the operation is not completed, we need to cancle it first
                    // TODO: Note this will fail if waiting for resources/in progress - so we should add a wait here until finished.
                    asyncoperation operationToUpdate = new asyncoperation();
                    operationToUpdate.AsyncOperationId = pendingDeleteItem.id;
                    operationToUpdate.Id = operationToUpdate.AsyncOperationId.Value;
                    operationToUpdate.StateCode = new OptionSetValue(3);
                    OrganizationServiceProxy.Update(operationToUpdate);
                }
                OrganizationServiceProxy.BeginDelete(pendingDeleteItem.entityName, pendingDeleteItem.id, delegate(object result)
                {
                    try
                    {
                        OrganizationServiceProxy.EndDelete(result);
                        IsBusyProgress.SetValue((index / items.Count) * 100);
                        nextCallback();  
                    }
                    catch (Exception ex)
                    {
                        errorCallBack(ex);
                    }
                });
            },
            items.Count,
            completedCallback,
            delegate(Exception ex)
            {
                // Error
                ReportError(ex);

            });

            
        }
        private void CreateBulkDeleteJobs(ScheduledJob job)
        {
            IsBusyMessage.SetValue("Creating new schedule...");
            IsBusyProgress.SetValue(0);
            // Convert bulk delete fetch into QueryExpression
            string fetchxml = "<fetch distinct='false' no-lock='false' mapping='logical'><entity name='lead'><attribute name='fullname' /><attribute name='statuscode' /><attribute name='createdon' /><attribute name='subject' /><attribute name='leadid' /><filter type='and'><condition attribute='ownerid' operator='eq-userid' /><condition attribute='statecode' operator='eq' value='0' /><condition attribute='address1_county' operator='eq' value='deleteme' /></filter><order attribute='createdon' descending='true' /></entity></fetch>";
            FetchXmlToQueryExpressionRequest convertRequest = new FetchXmlToQueryExpressionRequest();
            convertRequest.FetchXml = fetchxml;
            OrganizationServiceProxy.BeginExecute(convertRequest, delegate(object state)
            {
                FetchXmlToQueryExpressionResponse response = (FetchXmlToQueryExpressionResponse)OrganizationServiceProxy.EndExecute(state);

                List<BulkDeleteRequest> bulkDeleteRequests = new List<BulkDeleteRequest>();

                // If the recurrance is minutely, hourly, weekly we need to schedule multiple jobs
                if (job.Recurrance.GetValue().Value != RecurranceFrequencyNames.DAILY)
                {
                    DateTime startDate = DateTimeEx.UTCToLocalTimeFromSettings(job.StartDate.GetValue(),OrganizationServiceProxy.GetUserSettings());
                    
                    DateInterval interval =DateInterval.Days;
                    int incrementAmount = 1;
                    int dayInterval=1;
                    int recurranceCount = 0;
                    int totalCount = 0;
                    string freq = RecurranceFrequencyNames.DAILY;

                    switch (job.Recurrance.GetValue().Value)
                    {
                        case RecurranceFrequencyNames.MINUTELY:
                            interval = DateInterval.Minutes;
                            incrementAmount = job.RecurEvery.GetValue();
                            dayInterval = 1;
                            recurranceCount = (60 * 24) / incrementAmount;
                            break;
                        case RecurranceFrequencyNames.HOURLY:
                            interval = DateInterval.Hours;
                            incrementAmount = job.RecurEvery.GetValue();
                            dayInterval = 1;
                            recurranceCount = 24 / incrementAmount;
                            break;
                        case RecurranceFrequencyNames.WEEKLY:
                        case RecurranceFrequencyNames.YEARLY:
                            // To schedule weekly, me must create a job per week day for the whole year, and set recurrance to every 365 days
                            // but this doesn't deal with leap years, so we can't do it!
                            throw new Exception("The selected schedule interval is currently not supported due to the limitation of bulk delete");
                    }

                    if (incrementAmount < 0)
                        throw new Exception("Invalid schedule");

                    // Increment in the recurrency frequence
                    for (int i = 0; i < recurranceCount; i++)
                    {
                        BulkDeleteRequest request = new BulkDeleteRequest();
                        request.QuerySet = response.Query.Replace(@"<d:anyType ", @"<d:anyType xmlns:e=""http://www.w3.org/2001/XMLSchema"" ");
                        request.SendEmailNotification = false;
                        request.StartDateTime = startDate;
                        request.RecurrencePattern = "FREQ=DAILY;INTERVAL=" + dayInterval.ToString();
                        request.JobName = "Scheduled Job " + i.Format("0000") + " " + job.ScheduledJobId.GetValue().Value;
                        ArrayEx.Add(bulkDeleteRequests, request);
                        startDate = DateTimeEx.DateAdd(interval, incrementAmount, startDate);
                    }


                }
                else
                {
                    // Just a single request
                    BulkDeleteRequest request = new BulkDeleteRequest();
                    request.QuerySet = response.Query.Replace(@"<d:anyType ", @"<d:anyType xmlns:e=""http://www.w3.org/2001/XMLSchema"" ");
                    request.SendEmailNotification = false;
                    request.StartDateTime = job.StartDate.GetValue();
                    request.RecurrencePattern = RecurrancePatternMapper.Serialise(job);
                    request.JobName = "Scheduled Job " + job.ScheduledJobId.GetValue().Value;
                    ArrayEx.Add(bulkDeleteRequests, request);
                }

                BatchCreateBulkDeleteJobs(bulkDeleteRequests, delegate()
                {
                    IsBusy.SetValue(false);
                    JobsViewModel.Reset();
                    JobsViewModel.Refresh();
                   
                });

              

            });

        }

        private void BatchCreateBulkDeleteJobs(List<BulkDeleteRequest> items,Action completedCallback)
        {

            DelegateItterator.CallbackItterate(delegate(int index, Action nextCallback, ErrorCallBack errorCallBack)
            {
                BulkDeleteRequest pendingDeleteItem = items[index];
                OrganizationServiceProxy.BeginExecute(pendingDeleteItem, delegate(object result)
                {
                    try
                    {

                        OrganizationServiceProxy.EndExecute(result);
                        IsBusyProgress.SetValue((index / items.Count) * 100);
                        nextCallback();
                    }
                    catch (Exception ex)
                    {
                        errorCallBack(ex);
                    }
                });
            },
            items.Count,
            completedCallback,
            delegate(Exception ex)
            {
                // Error
                ReportError(ex);

            });

        }
        private void ReportError(Exception ex)
        {
          
            Script.Alert("There was a problem saving. Please contact your system administrator.\n\n" + ex.Message);
            IsBusy.SetValue(false); 
            JobsViewModel.Reset(); 
            JobsViewModel.Refresh();
        }
        #endregion
    }
    public class PendingDelete
    {
        public string entityName;
        public Guid id;
        public bool cancelFirst;
    }


}
