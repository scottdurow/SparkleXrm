namespace SparkleXrm.Tasks.CrmSvcUtil
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class SourceCodeSplitter
    {
        private const string CustomActions = "CustomActions";
        private const string Entities = "Entities";
        private const string OptionSets = "OptionSets";
        protected ITrace _trace;

        public SourceCodeSplitter(ITrace trace)
        {
            _trace = trace;
        }

        public void WriteToSeparateFiles(string destinationDirectoryPath, string sourceCode, string typeNamespace)
        {
            if (!destinationDirectoryPath.Trim().EndsWith("\\"))
            {
                destinationDirectoryPath = $"{destinationDirectoryPath.Trim()}\\";
            }

            var regex = new SourceCodeTypeExtractor();
            var result = regex.ExtractTypes(sourceCode);

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
               
                WriteTypeContentToFile(entry.Name, typeNamespace, destination, entry.Content);
            }
        }

        private void WriteTypeContentToFile(string typeName, string typeNamespace, string directoryPath, string content)
        {
            var fileName = $"{directoryPath}{typeName}.cs";
            _trace.WriteLine($"Writing code file {fileName}");
            using (var streamWriter = new StreamWriter(fileName, false))
            {
                var result = streamWriter.WriteAsync(GenerateTypeText(typeNamespace, content));
                result.Wait();
            }
        }

        private string GenerateTypeText(string typeNamespace, string content) 
        {
            var stringBuilder = new StringBuilder();
            var namespaceContent = !string.IsNullOrWhiteSpace(typeNamespace);
            if (namespaceContent)
            {
                stringBuilder.AppendLine($"namespace {typeNamespace}");
                stringBuilder.AppendLine("{");
            }

            stringBuilder.AppendLine(content);

            if (namespaceContent)
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