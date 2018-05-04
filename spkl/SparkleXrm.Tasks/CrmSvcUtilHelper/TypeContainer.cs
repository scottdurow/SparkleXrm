using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SparkleXrm.Tasks.CrmSvcUtilHelper
{
    public class TypeContainer
    {
        const string ClassName = @"public(\s)+partial(\s)+class(\s)+[a-zA-Z0-9_]+";
        const string EnumName = @"public(\s)+enum(\s)+[a-zA-Z0-9_]+";
        public TypeContainer(ContainerType type, string content)
        {
            Type = type;
            Content = content;
            ComputeName();
        }
        public string Content { get; }
        public string Name { get; set; }
        public ContainerType Type { get; private set; }

        private void ComputeName()
        {
            var regex = new Regex(Type == ContainerType.ClassContainer ? ClassName : EnumName);
            var match = regex.Match(Content);
            Name = match.Value.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Last();

            if (Type == ContainerType.ClassContainer)
            {
                if (Content.Contains("Microsoft.Xrm.Sdk.Client.OrganizationServiceContext"))
                {
                    Type = ContainerType.OrganizationServiceContextContainer;
                }
                else if (Content.Contains("Microsoft.Xrm.Sdk.OrganizationRequest"))
                {
                    Type = ContainerType.OrganizationRequestContainer;
                }
                else if (Content.Contains("Microsoft.Xrm.Sdk.OrganizationResponse"))
                {
                    Type = ContainerType.OrganizationResponseContainer;
                }
            }
        }
    }
}