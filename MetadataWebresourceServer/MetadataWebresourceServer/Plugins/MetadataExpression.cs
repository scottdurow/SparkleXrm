using LitJson;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Plugins.MetadataWebresourceServer
{
    public enum TokenExpressionType{
        EntityMetadata,
        Function
    }
    /// <summary>
    /// Represents a token in metadata server webresource
    /// </summary>
    public class MetadataExpression
    {
        public const string EntityProperties = "all";

        #region Constructors
        public MetadataExpression(string expression, int startIndex, int length)
        {
            Expression = expression;
            StartIndex = startIndex;
            Length = length;

            int pos = expression.Substring(0,Math.Min(expression.Length,15)).IndexOf('=');
            ExpressionType = (pos>-1) ? TokenExpressionType.Function: TokenExpressionType.EntityMetadata;

            // Work out what type of expression it is
            // 1. Entity metadata
            // 2. Attribtue metadata
            // 3. Function
            if (ExpressionType == TokenExpressionType.EntityMetadata)
            {
                var parts = expression.Split('.');
                switch (parts.Length)
                {
                    case 2:
                        // Entity.Metadata
                        Entity = parts[0];
                        Attribute = EntityProperties;
                        PropertyName = parts[1];
                        break;
                    case 3:
                        // Entity.Attribute.Metadata
                        Entity = parts[0];
                        Attribute = parts[1];
                        PropertyName = parts[2];
                        break;

                }
            }
            else
            {
                FunctionName = expression.Substring(0, pos);
                Paramaters = expression.Substring(pos+1);
            }
        }
        #endregion

        #region Properties
        public TokenExpressionType ExpressionType { get; private set; }
        public string FunctionName { get; private set; }
        public string Paramaters { get; private set; }
        public string Entity { get; private set; }
        public string Attribute { get; private set; }
        public string PropertyName { get; private set; }
        public int StartIndex { get; private set; }
        public int Length { get; private set; }
        public string Expression { get; private set; }
        public string Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Convert the value into a json string for output
        /// </summary>
        /// <param name="value"></param>
        public void SerialiseValue(object value)
        {
            string valueType = value.GetType().Name;
            switch (valueType)
            {
                case "OptionSetMetadata":
                    var options = (OptionSetMetadata)value;
                    this.Value = SerialiseOptionSetMetadata(options);
                    break;
                case "Label":
                    var label = (Label)value;
                    string labelText = (label.LocalizedLabels.Count > 0) ? label.LocalizedLabels[0].Label : "";               
                    this.Value = SerialiseString(labelText);
                    break;
                case "EntityCollection":
                    var entities = (EntityCollection)value;
                    SerialiseEntityCollection(entities);
                    break;
                default:
                    this.Value = SerialiseString(value.ToString());
                    break;
            }   
        }

        private string SerialiseString(string stringValue)
        {
            return WCFJsonSerializer(stringValue);
        }

        private string SerialiseOptionSetMetadata(OptionSetMetadata options)
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteArrayStart();
            foreach (var option in options.Options)
            {
                writer.WriteObjectStart();
                writer.WritePropertyName("value");
                writer.Write(option.Value.Value);
                writer.WritePropertyName("label");

                string label = option.Label.LocalizedLabels.Count > 0 ? option.Label.LocalizedLabels[0].Label : "?";
                writer.Write(label);
                writer.WriteObjectEnd();
            }
            writer.WriteArrayEnd();
            return sb.ToString();
        }

        private void SerialiseEntityCollection(EntityCollection entities)
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            WriteString(writer, "totalRecordCount", entities.TotalRecordCount.ToString());
            writer.WritePropertyName("Entities");
            writer.WriteArrayStart();
            foreach (var entity in entities.Entities)
            {
                writer.WriteObjectStart();
                writer.WritePropertyName("logicalName");
                writer.Write(entity.LogicalName);
                writer.WritePropertyName("id");
                writer.Write(entity.Id.ToString());
                foreach (var attributeName in entity.Attributes.Keys)
                {
                    var attributeValue = entity[attributeName];
                    var typeName = attributeValue.GetType().Name;
                    switch (typeName)
                    {
                        case "EntityReference":
                            EntityReference entityReference = entity.GetAttributeValue<EntityReference>(attributeName);
                            writer.WritePropertyName(attributeName);
                            writer.WriteObjectStart();
                            WriteString(writer,"logicalName",entityReference.LogicalName);
                            WriteString(writer,"id",entityReference.Id.ToString("D"));
                            WriteString(writer,"name",entityReference.Name);
                            writer.WriteObjectEnd();
                            break;
                        case "Guid":
                            Guid guid = entity.GetAttributeValue<Guid>(attributeName);
                            writer.WritePropertyName(attributeName);
                            writer.WriteObjectStart();
                            WriteString(writer,"id",guid.ToString("D"));                           
                            writer.WriteObjectEnd();
                            break;
                        default:
                            WriteString(writer, attributeName, attributeValue.ToString()); 
                            
                            break;
                    }
                }
                writer.WriteObjectEnd();
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
            this.Value = sb.ToString();
        }

        private static void WriteString(JsonWriter writer, string propertyName, string stringValue)
        {
            writer.WritePropertyName(propertyName);
            writer.Write(stringValue);
        }
      
        /// <summary>
        /// Uses the WCF Json serialiser to serialise an object
        /// Good for primitives such as strings - but not for objects since it adds the __type attirbute and other unneeded values which bulks up the output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private string WCFJsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        #endregion
    }
}
