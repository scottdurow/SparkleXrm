using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SparkleXrm.CrmSvcUtil.Helper
{
    public class SourceCodeManipulator
    {
        private const string CustomActions = "CustomActions";
        private const string Entities = "Entities";
        private const string OptionSets = "OptionSets";

        public void ProcessSourceCode(string destinationDirectoryPath, string sourceCode)
        {
            var addProxyTypesAssemblyAttribute = true;

            if (!destinationDirectoryPath.Trim().EndsWith("\\"))
            {
                destinationDirectoryPath = $"{destinationDirectoryPath.Trim()}\\";
            }

            var tree = CSharpSyntaxTree.ParseText(sourceCode);
            var treeRoot = tree.GetRoot();

            var namespaceDeclarations = treeRoot.DescendantNodes().OfType<NamespaceDeclarationSyntax>().ToList();
            var enumDeclarations = treeRoot.DescendantNodes().OfType<EnumDeclarationSyntax>().ToList();
            var classDeclarations = treeRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            var typeNamespace = namespaceDeclarations.FirstOrDefault()?.Name.ToString();

            CreateDirectories(destinationDirectoryPath, new List<string>() { CustomActions, Entities, OptionSets });

            foreach (var entry in enumDeclarations)
            {
                WriteTypeContentToFile(entry.Identifier.Text, typeNamespace, $"{destinationDirectoryPath}{OptionSets}\\", entry.GetText().ToString(), addProxyTypesAssemblyAttribute);

                addProxyTypesAssemblyAttribute = false;
            }

            foreach (var entry in classDeclarations)
            {
                string destination;
                if (ImplementsType(entry, "Microsoft.Xrm.Sdk.Client.OrganizationServiceContext"))
                {
                    destination = destinationDirectoryPath;
                }
                else if (ImplementsType(entry, "Microsoft.Xrm.Sdk.OrganizationRequest"))
                {
                    destination = $"{destinationDirectoryPath}{CustomActions}\\";
                }
                else if (ImplementsType(entry, "Microsoft.Xrm.Sdk.OrganizationResponse"))
                {
                    destination = $"{destinationDirectoryPath}{CustomActions}\\";
                }
                else
                {
                    destination = $"{destinationDirectoryPath}{Entities}\\";
                }

                WriteTypeContentToFile(entry.Identifier.Text, typeNamespace, destination,
                    entry.GetText().ToString(), addProxyTypesAssemblyAttribute);

                addProxyTypesAssemblyAttribute = false;
            }
        }

        private static bool ImplementsType(ClassDeclarationSyntax entry, string typeFullname)
        {
            var isContextClass = false;

            if (entry.BaseList != null)
            {
                foreach (var baseType in entry.BaseList.Types.ToList())
                {
                    isContextClass = baseType.Type.ToString()
                        .Contains(typeFullname);
                }
            }

            return isContextClass;
        }

        private void WriteTypeContentToFile(string typeName, string typeNamespace, string directoryPath, string content, bool addProxyTypesAssemblyAttribute)
        {
            using (var streamWriter = new StreamWriter($"{directoryPath}{typeName}.cs", false))
            {
                var result = streamWriter.WriteAsync(GenerateTypeText(typeNamespace, content, addProxyTypesAssemblyAttribute));
                result.Wait();
            }
        }

        private string GenerateTypeText(string typeNamespace, string content, bool addProxyTypesAssemblyAttribute)
        {
            var stringBuilder = new StringBuilder();

            if (addProxyTypesAssemblyAttribute)
            {
                stringBuilder.AppendLine("[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]");
                stringBuilder.AppendLine("");
            }

            if (!string.IsNullOrWhiteSpace(typeNamespace))
            {
                stringBuilder.AppendLine($"public namespace {typeNamespace}");
                stringBuilder.AppendLine("{");
            }

            stringBuilder.AppendLine(content);

            if (!string.IsNullOrWhiteSpace(typeNamespace))
            {
                stringBuilder.AppendLine("}");
            }

            return stringBuilder.ToString();
        }

        private void CreateDirectories(string rootDirectory, IEnumerable<string> subDirectoriesToCreate)
        {
            var directory = new DirectoryInfo(rootDirectory);
            foreach (var directoryToCreate in subDirectoriesToCreate)
            {
                directory.CreateSubdirectory(directoryToCreate);
            }
        }
    }

}