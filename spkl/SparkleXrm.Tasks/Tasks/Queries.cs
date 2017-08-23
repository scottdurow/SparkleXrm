using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public static class Queries
    {
        public static List<SdkMessageProcessingStep> GetPluginSteps(this OrganizationServiceContext ctx, string pluginType)
        {
            return (from p in ctx.CreateQuery<PluginType>()
                    join s in ctx.CreateQuery<SdkMessageProcessingStep>()
                    on p.PluginTypeId equals s.PluginTypeId.Id
                    join a in ctx.CreateQuery<PluginAssembly>()
                    on p.PluginAssemblyId.Id equals a.PluginAssemblyId
                    join m in ctx.CreateQuery<SdkMessage>()
                    on s.SdkMessageId.Id equals m.SdkMessageId
                    where p.TypeName == pluginType
                    select new SdkMessageProcessingStep
                    {
                        SdkMessageProcessingStepId = s.SdkMessageProcessingStepId,
                        Name = s.Name,
                        Mode = s.Mode,
                        FilteringAttributes = s.FilteringAttributes,
                        Rank = s.Rank,
                        Stage = s.Stage,
                        SdkMessageFilterId = s.SdkMessageFilterId,
                        Configuration = s.Configuration,
                        Description = s.Description,
                        sdkmessageid_sdkmessageprocessingstep =  new SdkMessage
                        {
                            SdkMessageId = m.SdkMessageId,
                            Name = m.Name
                        },
                        plugintypeid_sdkmessageprocessingstep = new PluginType
                        {
                            TypeName = p.TypeName,
                            pluginassembly_plugintype = new PluginAssembly
                            {
                                IsolationMode = a.IsolationMode
                            }
                        }

                    }
            ).ToList();
        }

        public static SdkMessageFilter GetMessageFilter(this OrganizationServiceContext ctx,Guid MessageFilterId)
        {
            return (from f in ctx.CreateQuery<SdkMessageFilter>()
                    where f.SdkMessageFilterId == MessageFilterId
                    select new SdkMessageFilter
                    {
                        PrimaryObjectTypeCode = f.PrimaryObjectTypeCode
                    }).FirstOrDefault();
        }

        public static List<PluginType> GetWorkflowPluginActivities(this OrganizationServiceContext ctx, string pluginType)
        {
            return (from p in ctx.CreateQuery<PluginType>()
                    join a in ctx.CreateQuery<PluginAssembly>()
                    on p.PluginAssemblyId.Id equals a.PluginAssemblyId
                    where p.TypeName == pluginType
                    select new PluginType
                    {
                        TypeName = p.TypeName,
                        Name = p.Name,
                        FriendlyName = p.FriendlyName,
                        Description = p.Description,
                        WorkflowActivityGroupName = p.WorkflowActivityGroupName,
                        pluginassembly_plugintype = new PluginAssembly
                        {
                            IsolationMode = a.IsolationMode
                        }
                    }
            ).ToList();
        }

        public static SdkMessageProcessingStepImage[] GetPluginStepImages(this OrganizationServiceContext ctx, SdkMessageProcessingStep step)
        {
            var existingImages = (from i in ctx.CreateQuery<SdkMessageProcessingStepImage>()
                                  where i.SdkMessageProcessingStepId.Id == step.Id
                                  select new SdkMessageProcessingStepImage
                                  {
                                      Id = i.Id,
                                      Name = i.Name,
                                      ImageType = i.ImageType, //0 : PreImage 1 : PostImage 2 : Both
                                      Attributes1 = i.Attributes1,
                                      EntityAlias = i.EntityAlias,
                                      SdkMessageProcessingStepId = i.SdkMessageProcessingStepId,
                                  }).ToArray();
            return existingImages;
        }

        public static SdkMessage GetMessage(this OrganizationServiceContext ctx, string messageName)
        {
            return (
                   from a in ctx.CreateQuery<SdkMessage>()
                   where a.Name == messageName
                   select new SdkMessage
                   {
                       SdkMessageId = a.SdkMessageId
                   }).FirstOrDefault();
        }

        public static SdkMessageFilter GetMessageFilter(this OrganizationServiceContext ctx,string entityLogicalName, string messageName)
        {
            // Get the message and message filter
            return (from m in ctx.CreateQuery<SdkMessageFilter>()
                    join a in ctx.CreateQuery<SdkMessage>()
                    on m.SdkMessageId.Id equals a.SdkMessageId
                    where m.PrimaryObjectTypeCode == entityLogicalName
                    && a.Name == messageName
                    select new SdkMessageFilter
                    {
                        SdkMessageFilterId = m.SdkMessageFilterId,
                        SdkMessageId = m.SdkMessageId
                       

                    }).FirstOrDefault();
        }

        public static List<PluginType> GetPluginTypes(this OrganizationServiceContext ctx,PluginAssembly plugin)
        {
            // Get existing types
            return (from t in ctx.CreateQuery<PluginType>()
                    where t.PluginAssemblyId.Id == plugin.Id
                    select new PluginType
                    {
                        Id = t.Id,
                        Name = t.Name,
                        PluginAssemblyId = t.PluginAssemblyId,
                        TypeName = t.TypeName,
                        FriendlyName = t.FriendlyName,
                        Description = t.Description
                    }).ToList();
        }

        public static List<WebResource> GetWebresources(this OrganizationServiceContext ctx)
        {
            return (from w in ctx.CreateQuery<WebResource>()
                    where w.IsManaged == false
                    && w.IsHidden.Value == false
                    && w.IsCustomizable.Value == true
                    select new WebResource
                    {
                        WebResourceId = w.WebResourceId,
                        Name = w.Name,
                        WebResourceType = w.WebResourceType,
                        DisplayName = w.DisplayName

                    }).ToList();
        }

        public static WebResource GetWebResource(this OrganizationServiceContext ctx, string uniqueName)
        {

            // Register
            return (from w in ctx.CreateQuery<WebResource>()
                    where w.Name == uniqueName
                    select new WebResource
                    {
                        Id = w.Id
                    }).FirstOrDefault();
        }
    }
}

