using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Client;

namespace SparkleXrm.Tasks
{
    public interface IQueries
    {
        SdkMessage GetMessage(OrganizationServiceContext ctx, string messageName);
        SdkMessageFilter GetMessageFilter(OrganizationServiceContext ctx, Guid MessageFilterId);
        SdkMessageFilter GetMessageFilter(OrganizationServiceContext ctx, string entityLogicalName, string messageName);
        SdkMessageProcessingStepImage[] GetPluginStepImages(OrganizationServiceContext ctx, SdkMessageProcessingStep step);
        List<SdkMessageProcessingStep> GetPluginSteps(OrganizationServiceContext ctx, string pluginType);
        List<PluginType> GetPluginTypes(OrganizationServiceContext ctx, PluginAssembly plugin);
        WebResource GetWebResource(OrganizationServiceContext ctx, string uniqueName);
        List<WebResource> GetWebresources(OrganizationServiceContext ctx);
        List<PluginType> GetWorkflowPluginActivities(OrganizationServiceContext ctx, string pluginType);
    }
}