using FakeItEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Microsoft.Crm.Sdk.Fakes
{
    public class WorkflowPipeline : PipelineBase
    {
        #region Properties
        public Dictionary<string, object> Inputs { get; private set; }
        public Guid TargetId { get; private set; }
        public IWorkflowContext WorkflowContext { get; private set; }
        #endregion

        #region Constructors
        public WorkflowPipeline(Entity target, int depth, Dictionary<string, object> inputs)
            : this(target, depth, inputs, null)
        {

        }
        public WorkflowPipeline(Entity target, int depth, Dictionary<string, object> inputs, IOrganizationService service)
            : base(service)
        {
            Inputs = inputs;
            Dictionary<string, object> outputs = new Dictionary<string, object>();

            // IWorkflowContext
            WorkflowContext = A.Fake<IWorkflowContext>();
            A.CallTo(() => WorkflowContext.Depth).Returns(depth);
        }
        #endregion

        #region Public Methods
        public IDictionary<string, object> Execute(Activity activity)
        {
            // Workflow Invoker
            var invoker = new WorkflowInvoker(activity);
            invoker.Extensions.Add<ITracingService>(() => TracingService);
            invoker.Extensions.Add<IOrganizationServiceFactory>(() => Factory);
            invoker.Extensions.Add<IWorkflowContext>(() => WorkflowContext);
            A.CallTo(()=>WorkflowContext.PrimaryEntityId).Returns(Target.Id);
            var output = invoker.Invoke(Inputs);
            return output;
        }
        #endregion
    }
}
