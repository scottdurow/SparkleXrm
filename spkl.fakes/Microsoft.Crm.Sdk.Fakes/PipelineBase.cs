﻿using FakeItEasy;
using Microsoft.Xrm.Sdk;
using System;
using System.Diagnostics;

namespace Microsoft.Crm.Sdk.Fakes
{
    public abstract class PipelineBase : IDisposable
    {

        #region Properties
        public FakeOrganzationService FakeService { get; private set; }
        public IOrganizationService Service { get; private set; }
        public IPluginExecutionContext PluginExecutionContext { get; private set; }
        public ITracingService TracingService { get; private set; }
        public IOrganizationServiceFactory Factory { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public ParameterCollection InputParameters { get; private set; }
        public ParameterCollection OutputParameters { get; private set; }
        public ParameterCollection SharedVariables { get; private set; }
        public Entity Target { get; private set; }

        public EntityImageCollection PreImages { get; private set; }
        public EntityImageCollection PostImages { get; private set; }

        public Guid UserId { get; set; }
        public Guid InitiatingUserId { get; set; }
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
            A.CallTo(() => PluginExecutionContext.Stage).Returns((int)stage);
            A.CallTo(() => PluginExecutionContext.MessageName).Returns(message);

            if (target != null)
            {
                // Check that the entity target is populated with at least the logical name
                if (target.LogicalName == null)
                    throw new ArgumentNullException("target", "You must supply at least the target entity with a logical name");
                A.CallTo(() => PluginExecutionContext.PrimaryEntityName).Returns(target.LogicalName);
                A.CallTo(() => PluginExecutionContext.PrimaryEntityId).Returns(target.Id);
                A.CallTo(() => PluginExecutionContext.UserId).Returns(UserId);
                A.CallTo(() => PluginExecutionContext.InitiatingUserId).Returns(InitiatingUserId);
                A.CallTo(() => PluginExecutionContext.CorrelationId).Returns(Guid.NewGuid());
                InputParameters["Target"] = target;
            }
        }

        public PipelineBase(IOrganizationService service = null)
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
            PluginExecutionContext = A.Fake<IPluginExecutionContext>(a => a.Strict());
            A.CallTo(() => PluginExecutionContext.PreEntityImages).Returns(PreImages);
            A.CallTo(() => PluginExecutionContext.PostEntityImages).Returns(PostImages);
            A.CallTo(() => PluginExecutionContext.InputParameters).Returns(InputParameters);
            A.CallTo(() => PluginExecutionContext.OutputParameters).Returns(OutputParameters);

            // ITracingService
            TracingService = A.Fake<ITracingService>((a) => a.Strict());
            A.CallTo(() => TracingService.Trace(A<string>.Ignored, A<object[]>.Ignored))
                .Invokes((string format, object[] args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Trace.WriteLine(string.Format(format, args));
                    }
                    else
                    {
                        Trace.WriteLine(format);
                    }
                });


            // IOrganizationServiceFactory
            Factory = A.Fake<IOrganizationServiceFactory>((a) => a.Strict());
            A.CallTo(() => Factory.CreateOrganizationService(A<Guid?>.Ignored)).Returns(Service);

            // IServiceProvider
            ServiceProvider = A.Fake<IServiceProvider>((a) => a.Strict());
            A.CallTo(() => ServiceProvider.GetService(A<Type>.Ignored))
                .ReturnsLazily((objectcall) =>
                {
                    var type = (Type) objectcall.Arguments[0];
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
                });
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
