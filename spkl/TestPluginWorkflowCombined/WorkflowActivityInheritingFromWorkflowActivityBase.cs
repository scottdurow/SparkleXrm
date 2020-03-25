using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Workflow;
using CrmVSSolution4.WorkflowActivityLibrary1;

namespace TestWFSolution.WorkflowActivityLibrary1
{
    [CrmPluginRegistration(
   "WorkflowActivityInheritingFromWorkflowActivityBase", "93AB069C-188B-4929-BF1E-9622DEDD5209", "Description", "Group Name", IsolationModeEnum.Sandbox
   )]
    public class WorkflowActivityInheritingFromWorkflowActivityBase : WorkFlowActivityBase
    {
        
        /// <summary>
        /// Executes the WorkFlow.
        /// </summary>
        /// <param name="crmWorkflowContext">The <see cref="LocalWorkflowContext"/> which contains the
        /// <param name="executionContext" > <see cref="CodeActivityContext"/>
        /// </param>       
        /// <remarks>
        /// For improved performance, Microsoft Dynamics 365 caches WorkFlow instances.
        /// The WorkFlow's Execute method should be written to be stateless as the constructor
        /// is not called for every invocation of the WorkFlow. Also, multiple system threads
        /// could execute the WorkFlow at the same time. All per invocation state information
        /// is stored in the context. This means that you should not use global variables in WorkFlows.
        /// </remarks>
        public override void ExecuteCRMWorkFlowActivity(CodeActivityContext executionContext, LocalWorkflowContext crmWorkflowContext)
        {

            if (crmWorkflowContext == null)
            {
                throw new ArgumentNullException("crmWorkflowContext");
            }

            try
            {
                // TODO: Implement your custom activity handling.
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                // Handle the exception.
                throw e;
            }
        }
    }
}
