using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Fakes;
using System;
using System.Diagnostics;

namespace Microsoft.Crm.Sdk.Fakes
{
    public abstract class PipelineBase : IDisposable
    {
        #region Private Members
        private IDisposable _shimContext;
        #endregion

        #region Properties
        public FakeOrganzationService FakeService { get; private set; }
        public IOrganizationService Service { get; private set; }
        public StubIPluginExecutionContext PluginExecutionContext { get; private set; }
        public StubITracingService TracingService { get; private set; }
        public IOrganizationServiceFactory Factory { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public ParameterCollection InputParameters { get; private set; }
        public ParameterCollection OutputParameters { get; private set; }
        public ParameterCollection SharedVariables { get; private set; }
        public Entity Target { get; private set; }

        public EntityImageCollection PreImages { get; private set; }
        public EntityImageCollection PostImages { get; private set; }
        #endregion

        #region Constructors
        public PipelineBase(FakeMessageNames message, FakeStages stage, Entity target, IOrganizationService service = null)
            : this(message.ToString(), stage, target, service)
        {

        }
        public PipelineBase(string message, FakeStages stage, Entity target, IOrganizationService service = null)
            : this(service)
        {

            // Set pipeline properties
            PluginExecutionContext.StageGet = () => { return (int)stage; };
            PluginExecutionContext.MessageNameGet = () => { return message; };
          
            if (target != null)
            {
                // Check that the entity target is populated with at least the logical name
                if (target.LogicalName == null)
                    throw new ArgumentNullException("target", "You must supply at least the target entity with a logical name");

                PluginExecutionContext.PrimaryEntityNameGet = () => { return target.LogicalName; };
                PluginExecutionContext.PrimaryEntityIdGet = () => { return target.Id; };
                InputParameters["Target"] = target;
            }
        }

        public PipelineBase(IOrganizationService service = null)
        {
            _shimContext = ShimsContext.Create();
            {
                if (service == null)
                {
                    FakeService = new FakeOrganzationService();
                    Service = FakeService;
                }
                else
                {
                    Service = service;
                }

                PreImages = new EntityImageCollection();
                PostImages = new EntityImageCollection();
                InputParameters = new ParameterCollection();
                OutputParameters = new ParameterCollection();
                PluginExecutionContext = new StubIPluginExecutionContext();
                PluginExecutionContext.PreEntityImagesGet = () => { return PreImages; };
                PluginExecutionContext.PostEntityImagesGet = () => { return PreImages; };
                PluginExecutionContext.InputParametersGet = () => { return InputParameters; };
                PluginExecutionContext.OutputParametersGet = () => { return OutputParameters; };

                // ITracingService
                TracingService = new StubITracingService();
                TracingService.TraceStringObjectArray = (format, values) =>
                {
                    if (values != null && values.Length > 0)
                    {
                        Trace.WriteLine(string.Format(format, values));
                    }
                    else
                    {
                        Trace.WriteLine(format);
                    }
                };

                // IOrganizationServiceFactory
                Factory = new StubIOrganizationServiceFactory
                {
                    CreateOrganizationServiceNullableOfGuid = id =>
                    {
                        return Service;
                    }
                };

                // IServiceProvider
                ServiceProvider = new System.Fakes.StubIServiceProvider
                {
                    GetServiceType = type =>
                    {
                        if (type == typeof(IPluginExecutionContext))
                        {
                            return PluginExecutionContext;
                        }
                        else if (type == typeof(ITracingService))
                        {
                            return TracingService;
                        }
                        else if (type == typeof(IOrganizationServiceFactory))
                        {
                            return Factory;
                        }

                        return null;
                    }
                };

            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    _shimContext.Dispose();
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
