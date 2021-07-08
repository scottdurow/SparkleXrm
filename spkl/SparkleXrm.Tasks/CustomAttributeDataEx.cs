using System;
using System.Linq;
using System.Reflection;

namespace SparkleXrm.Tasks
{
    public static class AttributeExtensions
    {
        public static CrmPluginRegistrationAttribute CreateFromData(this CustomAttributeData data)
        {
            CrmPluginRegistrationAttribute attribute = null;
            var arguments = data.ConstructorArguments.ToArray();
            // determine which constructor is being used by the first type
            if (data.ConstructorArguments.Count == 8 && data.ConstructorArguments[0].ArgumentType.Name == "String")
            {
                attribute = new CrmPluginRegistrationAttribute(
                    (string)arguments[0].Value,
                    (string)arguments[1].Value,
                    (StageEnum)Enum.ToObject(typeof(StageEnum), (int)arguments[2].Value),
                    (ExecutionModeEnum)Enum.ToObject(typeof(ExecutionModeEnum), (int)arguments[3].Value),
                    (string)arguments[4].Value,
                    (string)arguments[5].Value,
                    (int)arguments[6].Value,
                    (IsolationModeEnum)Enum.ToObject(typeof(IsolationModeEnum), (int)arguments[7].Value)
                    );

            }
            else if (data.ConstructorArguments.Count == 8 && data.ConstructorArguments[0].ArgumentType.Name == "MessageNameEnum")
            {
                attribute = new CrmPluginRegistrationAttribute(
                   (MessageNameEnum)Enum.ToObject(typeof(MessageNameEnum), (int)arguments[0].Value),
                   (string)arguments[1].Value,
                   (StageEnum)Enum.ToObject(typeof(StageEnum), (int)arguments[2].Value),
                   (ExecutionModeEnum)Enum.ToObject(typeof(ExecutionModeEnum), (int)arguments[3].Value),
                   (string)arguments[4].Value,
                   (string)arguments[5].Value,
                   (int)arguments[6].Value,
                   (IsolationModeEnum)Enum.ToObject(typeof(IsolationModeEnum), (int)arguments[7].Value)
                   );
            }
            else if (data.ConstructorArguments.Count == 5 && data.ConstructorArguments[0].ArgumentType.Name == "String")
            {
                attribute = new CrmPluginRegistrationAttribute(
                (string)arguments[0].Value,
                (string)arguments[1].Value,
                (string)arguments[2].Value,
                (string)arguments[3].Value,
                (IsolationModeEnum)Enum.ToObject(typeof(IsolationModeEnum), (int)arguments[4].Value)
                );
            }
            else if (data.ConstructorArguments.Count == 1 && data.ConstructorArguments[0].ArgumentType.Name == "String")
            {
                // Custom Api Registration
                attribute = new CrmPluginRegistrationAttribute(
                (string)arguments[0].Value
                );
               
            }
            foreach (var namedArgument in data.NamedArguments)
            {
                switch (namedArgument.MemberName)
                {
                    case "Id":
                        attribute.Id = (string)namedArgument.TypedValue.Value;
                        break;
                    case "FriendlyName":
                        attribute.FriendlyName = (string)namedArgument.TypedValue.Value;
                        break;
                    case "GroupName":
                        attribute.FriendlyName = (string)namedArgument.TypedValue.Value;
                        break;
                    case "Image1Name":
                        attribute.Image1Name = (string)namedArgument.TypedValue.Value;
                        break;
                    case "Image1Attributes":
                        attribute.Image1Attributes = (string)namedArgument.TypedValue.Value;
                        break;
                    case "Image2Name":
                        attribute.Image2Name = (string)namedArgument.TypedValue.Value;
                        break;
                    case "Image2Attributes":
                        attribute.Image2Attributes = (string)namedArgument.TypedValue.Value;
                        break;
                    case "Image1Type":
                        attribute.Image1Type = (ImageTypeEnum)namedArgument.TypedValue.Value;
                        break;
                    case "Image2Type":
                        attribute.Image2Type = (ImageTypeEnum)namedArgument.TypedValue.Value;
                        break;
                    case "Description":
                        attribute.Description = (string)namedArgument.TypedValue.Value;
                        break;
                    case "DeleteAsyncOperation":
                        attribute.DeleteAsyncOperation = (bool)namedArgument.TypedValue.Value;
                        break;
                    case "UnSecureConfiguration":
                        attribute.UnSecureConfiguration = (string)namedArgument.TypedValue.Value;
                        break;
                    case "SecureConfiguration":
                        attribute.SecureConfiguration = (string)namedArgument.TypedValue.Value;
                        break;
                    case "Offline":
                        attribute.Offline = (bool)namedArgument.TypedValue.Value;
                        break;
                    case "Server":
                        attribute.Server = (bool)namedArgument.TypedValue.Value;
                        break;
                    case "Action":
                        attribute.Action = (PluginStepOperationEnum)namedArgument.TypedValue.Value;
                        break;
                }
            }
            return attribute;
        }

        public static string GetAttributeCode(this CrmPluginRegistrationAttribute attribute, string indentation)
        {
            string lineIndent = $"\r\n{indentation}{indentation}";  // = newline + 2x indent

            TargetType targetType;
            if (attribute.Stage != null)
                targetType = TargetType.Plugin;
            else if (attribute.Name == null && attribute.Message != null)
                targetType = TargetType.CustomApi;
            else
                targetType = TargetType.WorkflowAcitivty;

            string additionalParmeters = "";

            // Image 1
            if (attribute.Image1Name != null)
            {
                additionalParmeters = $",{lineIndent}Image1Type = ImageTypeEnum.{attribute.Image1Type}, Image1Name = \"{attribute.Image1Name}\"";

                if (!string.IsNullOrWhiteSpace(attribute.Image1Attributes))
                    additionalParmeters += $", Image1Attributes = \"{attribute.Image1Attributes}\"";
            }

            // Image 2
            if (attribute.Image2Name != null)
            {
                additionalParmeters += $",{lineIndent}Image2Type = ImageTypeEnum.{attribute.Image2Type}, Image2Name = \"{attribute.Image2Name}\"";

                if (!string.IsNullOrWhiteSpace(attribute.Image2Attributes))
                    additionalParmeters += $", Image2Attributes = \"{attribute.Image2Attributes}\"";
            }

            if (targetType == TargetType.Plugin)
            {
                // Description is only option for plugins
                if (!string.IsNullOrEmpty(attribute.Description))
                    additionalParmeters += $",{lineIndent}Description = \"{attribute.Description}\"";
                if (attribute.Offline)
                    additionalParmeters += $",{lineIndent}Offline = {attribute.Offline}";
                if (!attribute.Server)
                    additionalParmeters += $",{lineIndent}Server = {attribute.Server}";
            }
            if (attribute.Id != null)
                additionalParmeters += $",{lineIndent}Id = \"{attribute.Id}\"";

            if (attribute.ExecutionMode == ExecutionModeEnum.Asynchronous && attribute.DeleteAsyncOperation == true)
                additionalParmeters += $",{lineIndent}DeleteAsyncOperation = true";

            if (attribute.UnSecureConfiguration != null)
                additionalParmeters += $",{lineIndent}UnSecureConfiguration = @\"{attribute.UnSecureConfiguration.Replace("\"","\"\"")}\"";

            if (attribute.SecureConfiguration != null)
                additionalParmeters += $",{lineIndent}SecureConfiguration = @\"{attribute.SecureConfiguration.Replace("\"", "\"\"")}\"";

            if (attribute.Action != null)
                additionalParmeters += $",{lineIndent}Action = PluginStepOperationEnum.{attribute.Action}";

            string code;
            // determine which template to use
            if (targetType == TargetType.Plugin)
            {
                // Plugin Step
                string template = "{9}[CrmPluginRegistration(\"{0}\", \"{1}\", StageEnum.{2}, ExecutionModeEnum.{3},{10}\"{4}\",{10}\"{5}\", {6}, IsolationModeEnum.{7}{8}\r\n{9})]\r\n";

                code = String.Format(template,
                    attribute.Message,              // 0
                    attribute.EntityLogicalName,    // 1
                    attribute.Stage,                // 2
                    attribute.ExecutionMode,        // 3
                    attribute.FilteringAttributes,  // 4
                    attribute.Name,                 // 5
                    attribute.ExecutionOrder,       // 6
                    attribute.IsolationMode,        // 7
                    additionalParmeters,            // 8
                    indentation,                    // 9
                    lineIndent);                    // 10
            }
            else if (targetType == TargetType.CustomApi)
            {
                // Custom Api
                code = $"{indentation}[CrmPluginRegistration(\"{attribute.Message}\")]\r\n";
            }
            else
            {
                // Workflow Step
                string template = "{6}[CrmPluginRegistration(\"{0}\",{7}\"{1}\", \"{2}\", \"{3}\",{7}IsolationModeEnum.{4}{5}\r\n{6})]\r\n";

                code = String.Format(template,
                    attribute.Name,             // 0
                    attribute.FriendlyName,     // 1
                    attribute.Description,      // 2
                    attribute.GroupName,        // 3
                    attribute.IsolationMode,    // 4
                    additionalParmeters,        // 5
                    indentation,                // 6
                    lineIndent);                // 7
            }
            return code;
        }
    }
}
