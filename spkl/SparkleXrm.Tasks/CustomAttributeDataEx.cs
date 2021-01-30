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
            var code = string.Empty;
            var targetType = (attribute.Stage != null) ? TargetType.Plugin : TargetType.WorkflowAcitivty;

            string additionalParmeters = "";

            // Image 1
            if (attribute.Image1Name != null)
                additionalParmeters += indentation + ",Image1Type = ImageTypeEnum." + attribute.Image1Type;
            if (attribute.Image1Name != null)
                additionalParmeters += indentation + ",Image1Name = \"" + attribute.Image1Name + "\"";
            if (attribute.Image1Name != null)
                additionalParmeters += indentation + ",Image1Attributes = \"" + attribute.Image1Attributes + "\"";

            // Image 2
            if (attribute.Image2Name != null)
                additionalParmeters += indentation + ",Image2Type = ImageTypeEnum." + attribute.Image2Type;
            if (attribute.Image2Name != null)
                additionalParmeters += indentation + ",Image2Name = \"" + attribute.Image2Name + "\"";
            if (attribute.Image2Attributes != null)
                additionalParmeters += indentation + ",Image2Attributes = \"" + attribute.Image2Attributes + "\"";


            if (targetType == TargetType.Plugin)
            {
                // Description is only option for plugins
                if (attribute.Description != null)
                    additionalParmeters += indentation + ",Description = \"" + attribute.Description + "\"";
                if (attribute.Offline)
                    additionalParmeters += indentation + ",Offline = " + attribute.Offline;
                if (!attribute.Server)
                    additionalParmeters += indentation + ",Server = " + attribute.Server;
            }
            if (attribute.Id != null)
                additionalParmeters += indentation + ",Id = \"" + attribute.Id + "\"";

            if (attribute.ExecutionMode == ExecutionModeEnum.Asynchronous && attribute.DeleteAsyncOperation == true)
                additionalParmeters += indentation + ",DeleteAsyncOperation = " + attribute.DeleteAsyncOperation;

            if (attribute.UnSecureConfiguration != null)
                additionalParmeters += indentation + ",UnSecureConfiguration = @\"" + attribute.UnSecureConfiguration.Replace("\"","\"\"") + "\"";

            if (attribute.SecureConfiguration != null)
                additionalParmeters += indentation + ",SecureConfiguration = @\"" + attribute.SecureConfiguration.Replace("\"", "\"\"") + "\"";

            if (attribute.Action != null)
                additionalParmeters += indentation + ",Action = PluginStepOperationEnum." + attribute.Action.ToString();

            // determine which template to use
            if (targetType == TargetType.Plugin)
            {
                // Plugin Step
                string template = "{9}[CrmPluginRegistration(\"{0}\", {9}\"{1}\", StageEnum.{2}, ExecutionModeEnum.{3},{9}\"{4}\",\"{5}\", {6}, {9}IsolationModeEnum.{7} {8} {9})]";

                code = String.Format(template,
                    attribute.Message,
                    attribute.EntityLogicalName,
                    attribute.Stage.ToString(),
                    attribute.ExecutionMode.ToString(),
                    attribute.FilteringAttributes,
                    attribute.Name,
                    attribute.ExecutionOrder.ToString(),
                    attribute.IsolationMode.ToString(),
                    additionalParmeters,
                    indentation);
            }
            else
            {
                // Workflow Step
                string template = "{6}[CrmPluginRegistration({6}\"{0}\", \"{1}\",\"{2}\",\"{3}\",IsolationModeEnum.{4}{6}{5})]";

                code = String.Format(template,
                    attribute.Name,
                    attribute.FriendlyName,
                    attribute.Description,
                    attribute.GroupName,
                    attribute.IsolationMode.ToString(),
                    additionalParmeters,
                    indentation);
            }
            return code;
        }
    }
}
