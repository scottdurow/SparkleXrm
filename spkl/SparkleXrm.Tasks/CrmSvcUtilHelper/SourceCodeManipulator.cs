using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SparkleXrm.Tasks.CrmSvcUtilHelper
{
    public class SourceCodeManipulator
    {
        private const string CustomActions = "CustomActions";
        private const string Entities = "Entities";
        private const string OptionSets = "OptionSets";

        public void ProcessSourceCode(string destinationDirectoryPath, string sourceCode, string typeNamespace)
        {
            if (!destinationDirectoryPath.Trim().EndsWith("\\"))
            {
                destinationDirectoryPath = $"{destinationDirectoryPath.Trim()}\\";
            }


            var scru = new SourceCodeRegexUtility();
            var result = scru.ExtractTypes(sourceCode);

            var enumDeclarations = result.Where(x => x.Type == ContainerType.EnumContainer).ToList();  
            var classDeclarations = result.Where(x => x.Type != ContainerType.EnumContainer).ToList(); 
            
            CreateDirectories(destinationDirectoryPath, new List<string>() { CustomActions, Entities, OptionSets });

            foreach (var entry in enumDeclarations)
            {
                WriteTypeContentToFile(entry.Name, typeNamespace, $"{destinationDirectoryPath}{OptionSets}\\", entry.Content); 
            }

            foreach (var entry in classDeclarations)
            {
                string destination;
                switch (entry.Type)
                {
                    case ContainerType.OrganizationServiceContextContainer:
                        destination = destinationDirectoryPath;
                        break;
                    case ContainerType.OrganizationRequestContainer:
                    case ContainerType.OrganizationResponseContainer:
                        destination = $"{destinationDirectoryPath}{CustomActions}\\";
                        break;
                    default:
                        destination = $"{destinationDirectoryPath}{Entities}\\";
                        break;
                }

                WriteTypeContentToFile(entry.Name, typeNamespace, destination,
                    entry.Content);
              }
        }


        private void WriteTypeContentToFile(string typeName, string typeNamespace, string directoryPath, string content)
        {
            using (var streamWriter = new StreamWriter($"{directoryPath}{typeName}.cs", false))
            {
                var result = streamWriter.WriteAsync(GenerateTypeText(typeNamespace, content));
                result.Wait();
            }
        }

        private string GenerateTypeText(string typeNamespace, string content) 
        {
            var stringBuilder = new StringBuilder();
 
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