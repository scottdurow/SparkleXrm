using Microsoft.Xrm.Sdk;

namespace Microsoft.Crm.Sdk.Fakes
{
    public class PluginPipeline : PipelineBase
    {
        #region Constructors
        public PluginPipeline(FakeMessageNames message, FakeStages stage, Entity target)
            : this(message.ToString(), stage, target, null)
        {

        }
        public PluginPipeline(string message, FakeStages stage, Entity target)
            : this(message, stage, target, null)
        {

        }
        public PluginPipeline(string message, FakeStages stage, Entity target, IOrganizationService service)
            : base(message, stage, target, service)
        {

        }
        #endregion

        #region Public Methods
        public void Execute(IPlugin plugin)
        {
            plugin.Execute(this.ServiceProvider);
        }
        #endregion
    }
}
