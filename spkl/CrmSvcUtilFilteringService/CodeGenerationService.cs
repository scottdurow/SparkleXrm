
namespace spkl.CrmSvcUtilExtensions
{
    using Microsoft.Crm.Services.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xrm.Sdk.Metadata;
    using System.CodeDom;
    using System.Globalization;
    using System.IO;
    using System.CodeDom.Compiler;
    using Microsoft.Xrm.Sdk.Client;
    using System.Reflection;

    internal class CodeGenerationService : ICodeGenerationService
    {
        private ICodeGenerationService _defaultService;

       
        public CodeGenerationService(ICodeGenerationService defaultServce)
        {
            _defaultService = defaultServce;
        }
        public CodeGenerationType GetTypeForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return _defaultService.GetTypeForAttribute(entityMetadata, attributeMetadata, services);
        }

        public CodeGenerationType GetTypeForEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            return _defaultService.GetTypeForEntity(entityMetadata, services);
        }

        public CodeGenerationType GetTypeForMessagePair(SdkMessagePair messagePair, IServiceProvider services)
        {
            return _defaultService.GetTypeForMessagePair(messagePair, services);
        }

        public CodeGenerationType GetTypeForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services)
        {
            return _defaultService.GetTypeForOption(optionSetMetadata, optionMetadata, services);
        }

        public CodeGenerationType GetTypeForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            return _defaultService.GetTypeForOptionSet(entityMetadata, optionSetMetadata, services);
        }

        public CodeGenerationType GetTypeForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services)
        {
            return _defaultService.GetTypeForRequestField(request, requestField, services);
        }

        public CodeGenerationType GetTypeForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services)
        {
            return _defaultService.GetTypeForResponseField(response, responseField, services);
        }

        public void Write(IOrganizationMetadata organizationMetadata, string language, string outputFile, string targetNamespace, IServiceProvider services)
        {
            // Since the CodeGenerationSerivce has lots of static internal methdos - we have to use reflecion to call them

            var crmsvcCodeGenerationService = Type.GetType("Microsoft.Crm.Services.Utility.CodeGenerationService,CrmSvcUtil");
            var BuildCodeDom = crmsvcCodeGenerationService.GetMethod("BuildCodeDom", BindingFlags.Static | BindingFlags.NonPublic);
            CodeNamespace codeDom = (CodeNamespace) BuildCodeDom.Invoke(null, new object[] { organizationMetadata, targetNamespace, services });

            HashSet<string> typeNames = new HashSet<string>();
            List<CodeTypeDeclaration> duplicates = new List<CodeTypeDeclaration>();
            
            // Remove duplicate global optionsets
            foreach (CodeTypeDeclaration item in codeDom.Types)
            {
                if (typeNames.Contains(item.Name))
                {
                    duplicates.Add(item);
                }
                else
                {
                    typeNames.Add(item.Name);
                }
            }

            foreach (CodeTypeDeclaration duplicate in duplicates)
            {
                codeDom.Types.Remove(duplicate);
            }
           
            var WriteFile = crmsvcCodeGenerationService.GetMethod("WriteFile", BindingFlags.Static | BindingFlags.NonPublic);
            WriteFile.Invoke(null, new object[] { outputFile, language, codeDom, services });
        }

      
    }
}
