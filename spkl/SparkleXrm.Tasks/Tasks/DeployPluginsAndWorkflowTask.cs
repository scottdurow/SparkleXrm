using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SparkleXrm.Tasks {

    /// <summary>
    /// Deployment task to deploy both plugin steps and custom workflow
    /// activity registrations on a single update.
    /// </summary>
    /// <remarks>
    /// https://github.com/scottdurow/SparkleXrm/issues/366
    /// </remarks>
    public class DeployPluginsAndWorkflowTask : BaseTask
    {

        /// <summary>
        /// When true plugin step and custom workflow activity registration
        /// information isn't updated when deploying assembly.
        /// </summary>
        public bool ExcludePluginSteps { get; set; }

        /// <summary>
        /// Creates a new instance. For more information see base class
        /// constructor
        /// <see cref="BaseTask(IOrganizationService, ITrace)"/>.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="trace"></param>
        public DeployPluginsAndWorkflowTask(IOrganizationService service,
                                            ITrace trace)
            : base(service, trace)
        {
          
        }

        /// <summary>
        /// Creates a new instance. For more information see base class
        /// constructor
        /// <see cref="BaseTask(OrganizationServiceContext, ITrace)" />.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="trace"></param>
        public DeployPluginsAndWorkflowTask(OrganizationServiceContext ctx,
                                            ITrace trace)
            : base(ctx, trace)
        {

        }

        /// <summary>
        /// See overrided method
        /// <see cref="BaseTask.ExecuteInternal(string, OrganizationServiceContext)"/>
        /// for intent.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="ctx"></param>
        protected override void ExecuteInternal(string folder,
                                                OrganizationServiceContext ctx)
        {
            _trace.WriteLine("Searching for plugin and workflow config in '{0}'",
                             folder);
            var configs = ServiceLocator.ConfigFileFactory.FindConfig(folder);

            foreach (var config in configs)
            {
                _trace.WriteLine("Using Config '{0}'", config.filePath);
                DeployPluginsAndWorkflows(ctx, config);
            }
            _trace.WriteLine("Processed {0} config(s)", configs.Count);
        }

        private void DeployPluginsAndWorkflows(OrganizationServiceContext ctx,
                                               ConfigFile config)
        {
            var plugins = config.GetPluginsConfig(this.Profile);
            
            foreach (var plugin in plugins)
            {
                List<string> assemblies = config.GetAssemblies(plugin);

                var pluginRegistration = new PluginRegistraton(_service, ctx, _trace);

                if (!string.IsNullOrEmpty(plugin.solution))
                {
                    pluginRegistration.SolutionUniqueName = plugin.solution;
                }

                foreach (var assemblyFilePath in assemblies)
                {
                    try
                    {
                        pluginRegistration.RegisterPluginAndWorkflow(assemblyFilePath,
                                                                     ExcludePluginSteps);
                    }

                    catch (ReflectionTypeLoadException ex)
                    {
                        // TODO One shouldn't throw System.Exception. See https://docs.microsoft.com/en-us/dotnet/standard/exceptions/. This is left unhandled to keep exception throwing similar between differend spkl task classes which also seem to throw System.Exception.
                        throw new Exception(ex.LoaderExceptions
                                              .First()
                                              .Message,
                                            ex);
                    }
                }
            }

        }
    }
}
