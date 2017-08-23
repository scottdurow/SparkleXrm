using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SparkleXrm.Tasks
{
    public class DownloadPluginMetadataTask : BaseTask
    {
        public DownloadPluginMetadataTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }

        public DownloadPluginMetadataTask(OrganizationServiceContext ctx, ITrace trace) : base(ctx, trace)
        {
        }

        protected override void ExecuteInternal(string filePath, OrganizationServiceContext ctx)
        {
            _trace.WriteLine("Searching for classes in '{0}'", filePath);
            var targetFolder = new DirectoryInfo(filePath);
            var matches = DirectoryEx.Search(filePath, "*.cs", null);

            if (matches == null)
                return;

            var pluginRegistration = new PluginRegistraton(_service, ctx, _trace);
            int codeFilesUpdated = 0;

            // Create a spkl.json file here (or load an existing one)
            var files = ConfigFile.FindConfig(filePath, false);
            var file = files[0];

            foreach (var codeFile in matches)
            {
                try
                {
                    string customClassRegex = null;
                    // Support for custom base class regex 
                    var profile = file.GetPluginsConfig(this.Profile);
                    if (profile!=null && profile.Length>0 && !String.IsNullOrEmpty(profile[0].classRegex))
                    {
                        customClassRegex = profile[0].classRegex;
                    }

                    // Find if it contains any plugin/workflow classes
                    CodeParser parser = new CodeParser(new Uri(codeFile), customClassRegex);

                    if (parser.PluginCount > 0)
                    {
                        // Backup 
                        File.WriteAllText(parser.FilePath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak", parser.Code);
                        foreach (var pluginType in parser.ClassNames)
                        {
                            // Remove existing attributes
                            parser.RemoveExistingAttributes();

                            if (parser.IsPlugin(pluginType))
                            {
                                AddPluginAttributes(ctx, parser, pluginType);
                            }
                            else if (parser.IsWorkflowActivity(pluginType))
                            {
                                AddWorkflowActivityAttributes(ctx, parser, pluginType);
                            }
                            else
                            {
                                _trace.WriteLine("Cannot find Type Registration {0}", pluginType);
                            }
                        }
                        // Update 
                        File.WriteAllText(parser.FilePath, parser.Code);
                        codeFilesUpdated++;
                    }
                }

                catch (ReflectionTypeLoadException ex)
                {
                    throw new Exception(ex.LoaderExceptions.First().Message);
                }
            }
            _trace.WriteLine("{0} classes decorated with deployment attributes!", codeFilesUpdated);

           
            if (file.plugins == null)
            {
                file.plugins = new List<PluginDeployConfig>();
            }

            if (file.plugins.Where(a=>a.assemblypath == @"bin\Debug").FirstOrDefault()==null)
            {
                file.plugins.Add(new PluginDeployConfig()
                {
                    assemblypath = @"bin\Debug"
                });
            }
           
            file.filePath = filePath;
            file.Save();
        }

        private void AddWorkflowActivityAttributes(OrganizationServiceContext ctx, CodeParser parser, string pluginType)
        {
            // If so, search CRM for matches
            var steps = ctx.GetWorkflowPluginActivities(pluginType);

            if (steps != null)
            {
                _trace.WriteLine("Found Workflow Activity Type Registration {0}", pluginType);
                // Get the activities
                foreach (var activity in steps)
                {
                    // Create attribute
                    CrmPluginRegistrationAttribute attribute = new CrmPluginRegistrationAttribute(
                        activity.Name,
                        activity.FriendlyName,
                        activity.Description,
                        activity.WorkflowActivityGroupName,
                        activity.pluginassembly_plugintype.IsolationMode.Value == 2 ? IsolationModeEnum.Sandbox : IsolationModeEnum.None
                        )
                    ;
                    // Add attribute
                    parser.AddAttribute(attribute, activity.TypeName);
                }
            }
        }

        private void AddPluginAttributes(OrganizationServiceContext ctx, CodeParser parser, string pluginType)
        {
            // Get existing Steps
            var steps = ctx.GetPluginSteps(pluginType);

            // Check that there are no duplicates
            var duplicateNames = steps.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
            if (duplicateNames.Count() > 0)
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.DUPLICATE_STEP, String.Format("More than one step found with the same name for plugin type {0} - {1}",pluginType,string.Join(",",duplicateNames.Select(a=>a.Name))));
            }

            if (steps != null)
            {
                _trace.WriteLine("Found Plugin Type Registration {0}", pluginType);
                // Get the steps
                foreach (var step in steps)
                {
                    SdkMessageFilter filter = null;
                    // If there is an entity filter then get it
                    if (step.SdkMessageFilterId!=null)
                    {
                        filter = ctx.GetMessageFilter(step.SdkMessageFilterId.Id);
                    }

                    // Get the images
                    SdkMessageProcessingStepImage[] images = ctx.GetPluginStepImages(step);

                    // Only support two images - Why would you need more?!
                    if (images.Length > 2)
                        throw new Exception(String.Format("More than 2 images found on step {0}", step.Name));

                    // Create attribute
                    // we output the ID so that we can be independant of name - but it's not neededed for new attributes
                    CrmPluginRegistrationAttribute attribute = new CrmPluginRegistrationAttribute(
                        step.sdkmessageid_sdkmessageprocessingstep.Name,
                        filter==null ? "none" : filter.PrimaryObjectTypeCode,
                        (StageEnum)Enum.ToObject(typeof(StageEnum), step.Stage.Value),
                        step.Mode.Value == 0 ? ExecutionModeEnum.Synchronous : ExecutionModeEnum.Asynchronous,
                        step.FilteringAttributes,
                        step.Name,
                        step.Rank.HasValue ? step.Rank.Value : 1,
                        step.plugintypeid_sdkmessageprocessingstep.pluginassembly_plugintype.IsolationMode.Value == 2 
                            ? IsolationModeEnum.Sandbox : IsolationModeEnum.None
                        )
                    { Id = step.Id.ToString() };

                    // Image 1
                    if (images.Length >= 1)
                    {
                        var image = images[0];
                        attribute.Image1Type = (ImageTypeEnum)Enum.ToObject(typeof(ImageTypeEnum), image.ImageType.Value);
                        attribute.Image1Name = image.EntityAlias;
                        attribute.Image1Attributes = image.Attributes1;
                    }
                    // Image 2
                    if (images.Length >= 2)
                    {
                        var image = images[1];
                        attribute.Image2Type = (ImageTypeEnum)Enum.ToObject(typeof(ImageTypeEnum), image.ImageType.Value);
                        attribute.Image2Name = image.EntityAlias;
                        attribute.Image2Attributes = image.Attributes1;
                    }
                    // Add config
                    if (step.Configuration != null)
                        attribute.UnSecureConfiguration = step.Configuration;

                    if (step.Description != null)
                        attribute.Description = step.Description;

                    // Add attribute to code
                    parser.AddAttribute(attribute, step.plugintypeid_sdkmessageprocessingstep.TypeName);
                }
            }
        }
    }
}
