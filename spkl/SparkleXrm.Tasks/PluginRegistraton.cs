using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SparkleXrm.Tasks
{
    public class PluginRegistraton
    {
        private OrganizationServiceContext _ctx;
        private IOrganizationService _service;
        private ITrace _trace;
        private string[] _ignoredAssemblies = new string[] {
            "Microsoft.Crm.Sdk.Proxy.dll",
            "Microsoft.IdentityModel.dll",
            "Microsoft.Xrm.Sdk.dll",
            "Microsoft.Xrm.Sdk.Workflow.dll",
            "Microsoft.IdentityModel.Clients.ActiveDirectory.dll",
            "Microsoft.Extensions.FileSystemGlobbing.dll",
            "Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll",
            "Microsoft.Xrm.Sdk.Deployment.dll",
            "Microsoft.Xrm.Tooling.Connector.dll",
            "Newtonsoft.Json.dll",
            "SparkleXrm.Tasks.dll"
        };
        public PluginRegistraton(IOrganizationService service, OrganizationServiceContext context, ITrace trace)
        {
            _ctx = context;
            _service = service;
            _trace = trace;

        }
        /// <summary>
        /// If not null, components are added to this solution
        /// </summary>
        public string SolutionUniqueName { get; set; }

        public void RegisterWorkflowActivities(string path)
        {
            var assemblyFilePath = new FileInfo(path);
            if (_ignoredAssemblies.Contains(assemblyFilePath.Name))
                return;
            // Load each assembly 
            Assembly assembly = Reflection.ReflectionOnlyLoadAssembly(assemblyFilePath.FullName);

            if (assembly == null)
                return;

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, args) => Assembly.ReflectionOnlyLoad(args.Name);

            // Search for any types that interhit from IPlugin                  
            IEnumerable<Type> pluginTypes = Reflection.GetTypesInheritingFrom(assembly, typeof(System.Activities.CodeActivity));

            if (pluginTypes.Count() > 0)
            {
                var plugin = RegisterAssembly(assemblyFilePath, assembly, pluginTypes);
                if (plugin != null)
                {
                    RegisterActivities(pluginTypes, plugin);
                }
            }

        }

        private void RegisterActivities(IEnumerable<Type> pluginTypes, PluginAssembly plugin)
        {
            var sdkPluginTypes = ServiceLocator.Queries.GetPluginTypes(_ctx, plugin);

            foreach (var pluginType in pluginTypes)
            {

                // Search for the CrmPluginStepAttribute
                var pluginAttributes = pluginType.GetCustomAttributesData().Where(a => a.AttributeType.Name == typeof(CrmPluginRegistrationAttribute).Name);
                PluginType sdkPluginType = null;
                if (pluginAttributes.Count() > 0)
                {

                    if (pluginAttributes.Count() > 1)
                    {
                        Debug.WriteLine("Workflow Activities can only have a single registration");

                    }

                    var workflowActivitiy = pluginAttributes.First().CreateFromData();

                    // Check if the type is registered
                    sdkPluginType = sdkPluginTypes.Where(t => t.TypeName == pluginType.FullName).FirstOrDefault();

                    if (sdkPluginType == null)
                    {
                        sdkPluginType = new PluginType();
                    }

                    // Update values
                    sdkPluginType.Name = workflowActivitiy.Name;
                    sdkPluginType.PluginAssemblyId = plugin.ToEntityReference();
                    sdkPluginType.TypeName = pluginType.FullName;

                    sdkPluginType.FriendlyName = !string.IsNullOrEmpty(workflowActivitiy.FriendlyName) ? workflowActivitiy.FriendlyName : Guid.NewGuid().ToString();
                    sdkPluginType.WorkflowActivityGroupName = workflowActivitiy.GroupName;
                    sdkPluginType.Description = workflowActivitiy.Description;

                    if (sdkPluginType.Id == Guid.Empty)
                    {
                        _trace.WriteLine("Registering Workflow Activity Type {0}", workflowActivitiy.Name);
                        // Create
                        sdkPluginType.Id = _service.Create(sdkPluginType);
                    }
                    else
                    {
                        _trace.WriteLine("Updating Workflow Activity Type {0}", workflowActivitiy.Name);
                        // Update
                        _service.Update(sdkPluginType);
                    }



                }
            }
        }

        private void AddAssemblyToSolution(string solutionName, PluginAssembly assembly)
        {

            // Find solution
            AddSolutionComponentRequest addToSolution = new AddSolutionComponentRequest()
            {
                AddRequiredComponents = true,
                ComponentType = (int)componenttype.PluginAssembly,
                ComponentId = assembly.Id,
                SolutionUniqueName = solutionName
            };
            _trace.WriteLine("Adding to solution '{0}'", solutionName);
            _service.Execute(addToSolution);

        }

        private void AddTypeToSolution(string solutionName, PluginType sdkPluginType)
        {
            // Find solution
            AddSolutionComponentRequest addToSolution = new AddSolutionComponentRequest()
            {
                ComponentType = (int)componenttype.PluginType,
                ComponentId = sdkPluginType.Id,
                SolutionUniqueName = solutionName
            };
            _trace.WriteLine("Adding to solution '{0}'", solutionName);
            _service.Execute(addToSolution);
        }
        private void AddStepToSolution(string solutionName, SdkMessageProcessingStep sdkPluginType)
        {
            // Find solution
            AddSolutionComponentRequest addToSolution = new AddSolutionComponentRequest()
            {
                AddRequiredComponents = false,
                ComponentType = (int)componenttype.SDKMessageProcessingStep,
                ComponentId = sdkPluginType.Id,
                SolutionUniqueName = solutionName
            };
            _trace.WriteLine("Adding to solution '{0}'", solutionName);
            _service.Execute(addToSolution);

        }


        public void RegisterPlugin(string file, bool excludePluginSteps = false)
        {
            var assemblyFilePath = new FileInfo(file);

            if (_ignoredAssemblies.Contains(assemblyFilePath.Name))
                return;

            // Load each assembly 
            Assembly peekAssembly = Reflection.ReflectionOnlyLoadAssembly(assemblyFilePath.FullName);

            if (peekAssembly == null)
                return;
            _trace.WriteLine("Checking assembly '{0}' for plugins", assemblyFilePath.Name);

            // Search for any types that interhit from IPlugin                  
            IEnumerable<Type> pluginTypes = Reflection.GetTypesImplementingInterface(peekAssembly, typeof(Microsoft.Xrm.Sdk.IPlugin));

            if (pluginTypes.Count() > 0)
            {
                _trace.WriteLine("{0} plugin(s) found!", pluginTypes.Count());

                var plugin = RegisterAssembly(assemblyFilePath, peekAssembly, pluginTypes);

                if (plugin != null && !excludePluginSteps)
                {
                    RegisterPluginSteps(pluginTypes, plugin);
                }
            }

        }

        public void RegisterPluginAndWorkflow(string file,
                                              bool excludePluginSteps = false)
        {
            var assemblyFilePath = new FileInfo(file);

            if (_ignoredAssemblies.Contains(assemblyFilePath.Name)) {
                return;
            }

            // Load each assembly 
            var peekAssembly = Reflection.ReflectionOnlyLoadAssembly(assemblyFilePath.FullName);

            if (peekAssembly == null)
            {
                return;
            }
            _trace.WriteLine("Checking assembly '{0}' for plugins and workflows",
                             assemblyFilePath.Name);

            // Search for any types that interhit from IPlugin                  
            var pluginTypes = Reflection.GetTypesImplementingInterface(peekAssembly,
                                                                       typeof(IPlugin));
            var workflowTypes = Reflection.GetTypesInheritingFrom(peekAssembly,
                                                                  typeof(CodeActivity));
            var typesToRegister = pluginTypes.Union(workflowTypes);
            if (typesToRegister.Any())
            {
                _trace.WriteLine("{0} plugin(s) and {1} workflow activities found!",
                                 pluginTypes.Count(),
                                 workflowTypes.Count());

                var pluginAssembly = RegisterAssembly(assemblyFilePath,
                                                      peekAssembly,
                                                      typesToRegister);

                if (pluginAssembly == null) {
                    return;
                }
                if(!excludePluginSteps)
                {
                    RegisterPluginSteps(pluginTypes, pluginAssembly);
                }
                RegisterActivities(workflowTypes, pluginAssembly);
            }

        }

        private PluginAssembly RegisterAssembly(FileInfo assemblyFilePath, Assembly assembly, IEnumerable<Type> pluginTypes)
        {

            // Get the isolation mode of the first attribute
            var firstType = Reflection.GetAttributes(pluginTypes, typeof(CrmPluginRegistrationAttribute).Name).FirstOrDefault();
            if (firstType == null)
                return null;
            var firstTypeAttribute = firstType.CreateFromData() as CrmPluginRegistrationAttribute;
            // Is there any steps to register?
            if (firstTypeAttribute == null)
                return null;
            var assemblyProperties = assembly.GetName().FullName.Split(",= ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var assemblyName = assembly.GetName().Name;
            // If found then register or update it
            var plugin = (from p in _ctx.CreateQuery<PluginAssembly>()
                          where p.Name == assemblyName
                          select new PluginAssembly
                          {
                              Id = p.Id,
                              Name = p.Name
                          }).FirstOrDefault();

            string assemblyBase64 = Convert.ToBase64String(File.ReadAllBytes(assemblyFilePath.FullName));

            if (plugin == null)
            {
                plugin = new PluginAssembly();
            }

            // update
            plugin.Content = assemblyBase64;
            plugin.Name = assemblyProperties[0];
            plugin.Culture = assemblyProperties[4];
            plugin.Version = assemblyProperties[2];
            plugin.PublicKeyToken = assemblyProperties[6];
            plugin.SourceType = pluginassembly_sourcetype.Database; // database
            plugin.IsolationMode = firstTypeAttribute.IsolationMode == IsolationModeEnum.Sandbox ? pluginassembly_isolationmode.Sandbox : pluginassembly_isolationmode.None; // 1= none, 2 = sandbox

            if (plugin.Id == Guid.Empty)
            {
                _trace.WriteLine("Registering Plugin '{0}' from '{1}'", plugin.Name, assemblyFilePath.FullName);
                // Create
                plugin.Id = _service.Create(plugin);
            }
            else
            {
                UnregisterRemovedPluginTypes(pluginTypes, plugin);

                _trace.WriteLine("Updating Plugin '{0}' from '{1}'", plugin.Name, assemblyFilePath.FullName);
                // Update
                _service.Update(plugin);
            }

            // Add to solution
            if (SolutionUniqueName != null)
            {
                _trace.WriteLine("Adding Plugin '{0}' to solution '{1}'", plugin.Name, SolutionUniqueName);
                AddAssemblyToSolution(SolutionUniqueName, plugin);
            }
            return plugin;
        }

        private void UnregisterRemovedPluginTypes(IEnumerable<Type> pluginTypes, PluginAssembly plugin)
        {
            _trace.WriteLine("Checking for orphaned PluginTypes: '{0}' ", plugin.Name);

            var sdkPluginTypes = ServiceLocator.Queries.GetPluginTypes(_ctx, plugin);

            foreach (var sdkPluginType in sdkPluginTypes)
            {
                var pluginType = pluginTypes.Where(t => t.FullName == sdkPluginType.TypeName).FirstOrDefault();
                if (pluginType == null)
                {
                    _trace.WriteLine("Not Found, deleting: {0}", sdkPluginType.TypeName);

                    // First need to remove Steps on the type
                    var existingSteps = GetExistingSteps(sdkPluginType);
                    foreach (var step in existingSteps)
                    {
                        _trace.WriteLine("Deleting step '{0}'", step.Name);
                        _service.Delete(SdkMessageProcessingStep.EntityLogicalName, step.Id);
                    }
                    _trace.WriteLine("Deleting PluginType '{0}'", sdkPluginType.TypeName);
                    _service.Delete(PluginType.EntityLogicalName, sdkPluginType.Id);
                }
            }
        }

        private void RegisterPluginSteps(IEnumerable<Type> pluginTypes, PluginAssembly plugin)
        {
            var sdkPluginTypes = ServiceLocator.Queries.GetPluginTypes(_ctx, plugin);

            foreach (var pluginType in pluginTypes)
            {

                // Search for the CrmPluginStepAttribute
                var pluginAttributes = pluginType.GetCustomAttributesData().Where(a => a.AttributeType.Name == typeof(CrmPluginRegistrationAttribute).Name);
                PluginType sdkPluginType = null;
                if (pluginAttributes.Count() > 0)
                {
                    var pluginStepAttribute = pluginAttributes.First().CreateFromData() as CrmPluginRegistrationAttribute;
                    // Check if the type is registered
                    sdkPluginType = sdkPluginTypes.Where(t => t.TypeName == pluginType.FullName).FirstOrDefault();

                    if (sdkPluginType == null)
                    {
                        sdkPluginType = new PluginType();
                    }

                    // Update values
                    sdkPluginType.Name = pluginType.FullName;
                    sdkPluginType.PluginAssemblyId = plugin.ToEntityReference();
                    sdkPluginType.TypeName = pluginType.FullName;
                    sdkPluginType.FriendlyName = pluginType.FullName;

                    if (sdkPluginType.Id == Guid.Empty)
                    {
                        _trace.WriteLine("Registering Type '{0}'", sdkPluginType.Name);
                        // Create
                        sdkPluginType.Id = _service.Create(sdkPluginType);
                    }
                    else
                    {
                        _trace.WriteLine("Updating Type '{0}'", sdkPluginType.Name);
                        // Update
                        _service.Update(sdkPluginType);
                    }

                    var existingSteps = GetExistingSteps(sdkPluginType);

                    foreach (var pluginAttribute in pluginAttributes)
                    {
                        RegisterStep(sdkPluginType, existingSteps, pluginAttribute);
                    }

                    // Remove remaining Existing steps
                    foreach (var step in existingSteps)
                    {
                        _trace.WriteLine("Deleting step '{0}'", step.Name, step.Stage);
                        _service.Delete(SdkMessageProcessingStep.EntityLogicalName, step.Id);
                    }
                }
            }
        }

        private List<SdkMessageProcessingStep> GetExistingSteps(PluginType sdkPluginType)
        {
            // Get existing Steps
            var steps = (from s in _ctx.CreateQuery<SdkMessageProcessingStep>()
                         where s.PluginTypeId.Id == sdkPluginType.Id
                         select new SdkMessageProcessingStep()
                         {
                             Id = s.Id,
                             PluginTypeId = s.PluginTypeId,
                             SdkMessageId = s.SdkMessageId,
                             Mode = s.Mode,
                             Name = s.Name,
                             Rank = s.Rank,
                             Configuration = s.Configuration,
                             Description = s.Description,
                             Stage = s.Stage,
                             SupportedDeployment = s.SupportedDeployment,
                             FilteringAttributes = s.FilteringAttributes,
                             EventHandler = s.EventHandler,
                             AsyncAutoDelete = s.AsyncAutoDelete,
                             Attributes = s.Attributes,
                             SdkMessageFilterId = s.SdkMessageFilterId

                         }).ToList();

            return steps;

        }


        private void RegisterStep(PluginType sdkPluginType, List<SdkMessageProcessingStep> existingSteps, CustomAttributeData pluginAttribute)
        {
            var pluginStep = (CrmPluginRegistrationAttribute)pluginAttribute.CreateFromData();

            SdkMessageProcessingStep step = null;
            Guid stepId = Guid.Empty;
            if (pluginStep.Id != null)
            {
                stepId = new Guid(pluginStep.Id);
                // Get by ID
                step = existingSteps.Where(s => s.Id == stepId).FirstOrDefault();
            }

            if (step == null)
            {
                // Get by Name
                step = existingSteps.Where(s => s.Name == pluginStep.Name && s.SdkMessageId.Name == pluginStep.Message).FirstOrDefault();
            }

            // Register images
            if (step == null)
            {
                step = new SdkMessageProcessingStep();
            }
            Guid? sdkMessageId = null;
            Guid? sdkMessagefilterId = null;

            if (pluginStep.EntityLogicalName == "none")
            {
                var message = ServiceLocator.Queries.GetMessage(_ctx, pluginStep.Message);
                sdkMessageId = message.SdkMessageId;
            }
            else
            {
                var messageFilter = ServiceLocator.Queries.GetMessageFilter(_ctx, pluginStep.EntityLogicalName, pluginStep.Message);

                if (messageFilter == null)
                {
                    _trace.WriteLine("Warning: Cannot register step {0} on Entity {1}", pluginStep.Message, pluginStep.EntityLogicalName);
                    return;
                }

                sdkMessageId = messageFilter.SdkMessageId.Id;
                sdkMessagefilterId = messageFilter.SdkMessageFilterId;
            }

            // Update attributes
            step.Name = pluginStep.Name;
            step.Configuration = pluginStep.UnSecureConfiguration;
            step.Description = pluginStep.Description;
            step.Mode = pluginStep.ExecutionMode == ExecutionModeEnum.Asynchronous ? sdkmessageprocessingstep_mode.Asynchronous : sdkmessageprocessingstep_mode.Synchronous;
            step.Rank = pluginStep.ExecutionOrder;
            int stage = 10;
            switch (pluginStep.Stage)
            {
                case StageEnum.PreValidation:
                    stage = 10;
                    break;
                case StageEnum.PreOperation:
                    stage = 20;
                    break;
                case StageEnum.PostOperation:
                    stage = 40;
                    break;
            }

            step.Stage = (sdkmessageprocessingstep_stage)stage;
            int supportDeployment = 0;
            if (pluginStep.Server == true && pluginStep.Offline == true)
            {
                supportDeployment = 2; // Both
            }
            else if (!pluginStep.Server == true && pluginStep.Offline == true)
            {
                supportDeployment = 1; // Offline only
            }
            else
            {
                supportDeployment = 0; // Server Only
            }
            step.SupportedDeployment = (sdkmessageprocessingstep_supporteddeployment)supportDeployment;
            step.PluginTypeId = sdkPluginType.ToEntityReference();
            step.SdkMessageFilterId = sdkMessagefilterId != null ? new EntityReference(SdkMessageFilter.EntityLogicalName, sdkMessagefilterId.Value) : null;
            step.SdkMessageId = new EntityReference(SdkMessage.EntityLogicalName, sdkMessageId.Value);
            step.FilteringAttributes = pluginStep.FilteringAttributes;
            if (step.Id == Guid.Empty)
            {
                _trace.WriteLine("Registering Step '{0}'", step.Name);
                if (stepId != Guid.Empty)
                {
                    step.Id = stepId;
                }
                // Create
                step.Id = _service.Create(step);
            }
            else
            {
                _trace.WriteLine("Updating Step '{0}'", step.Name);
                // Update
                _service.Update(step);
                existingSteps.Remove(step);
            }

            // Get existing Images
            List<SdkMessageProcessingStepImage> existingImages = ServiceLocator.Queries.GetPluginStepImages(_ctx, step);

            var image1 = RegisterImage(pluginStep, step, existingImages, pluginStep.Image1Name, pluginStep.Image1Type, pluginStep.Image1Attributes);
            var image2 = RegisterImage(pluginStep, step, existingImages, pluginStep.Image2Name, pluginStep.Image2Type, pluginStep.Image2Attributes);

            // Remove Images no longer being registered
            foreach (var image in existingImages)
            {
                _trace.WriteLine("Deleting Image {0}", image.Name);
                _service.Delete(SdkMessageProcessingStepImage.EntityLogicalName, image.Id);
            }
            if (SolutionUniqueName != null)
            {
                AddStepToSolution(SolutionUniqueName, step);

            }
        }




        private SdkMessageProcessingStepImage RegisterImage(CrmPluginRegistrationAttribute stepAttribute, SdkMessageProcessingStep step, List<SdkMessageProcessingStepImage> existingImages, string imageName, ImageTypeEnum imagetype, string attributes)
        {
            if (String.IsNullOrWhiteSpace(imageName))
            {
                return null;
            }

            var image = existingImages.FirstOrDefault(
                            a => a.SdkMessageProcessingStepId.Id == step.Id
                                 && a.EntityAlias == imageName
                                 && a.ImageType == (sdkmessageprocessingstepimage_imagetype)imagetype) ??
                        new SdkMessageProcessingStepImage();

            image.Name = imageName;

            image.ImageType = (sdkmessageprocessingstepimage_imagetype)imagetype;
            image.SdkMessageProcessingStepId = new EntityReference(SdkMessageProcessingStep.EntityLogicalName, step.Id);
            image.Attributes1 = attributes;
            image.EntityAlias = imageName;

            switch (stepAttribute.Message)
            {
                case "Create":
                    image.MessagePropertyName = "Id";
                    break;
                case "SetState":
                case "SetStateDynamicEntity":
                    image.MessagePropertyName = "EntityMoniker";
                    break;
                case "Send":
                case "DeliverIncoming":
                case "DeliverPromote":
                    image.MessagePropertyName = "EmailId";
                    break;
                default:
                    image.MessagePropertyName = "Target";
                    break;
            }

            if (image.Id == Guid.Empty)
            {
                _trace.WriteLine("Registering Image '{0}'", image.Name);
                image.Id = _service.Create(image);
            }
            else
            {
                _trace.WriteLine("Updating Image '{0}'", image.Name);
                _service.Update(image);
                existingImages.Remove(image);
            }
            return image;
        }
    }
}
