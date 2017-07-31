// Attribute.cs
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
	public class Attribute
    {
        #region Fields
        public string AttributeName;
		public string TypeName;
		public string FormattedValue;
		public object Value;
		public string Id;
		public string LogicalName;
		public string Name;
        #endregion

        #region Constructors
        public Attribute(string attributeName, string typeName)
		{

			this.AttributeName = attributeName;
			this.TypeName = typeName;
			this.FormattedValue = null;
			this.Value = null;

			// Entity Referece
			this.Id = null;
			this.LogicalName = null;
			this.Name = null;
		}
        #endregion

        #region Methods
        public static object DeSerialise(XmlNode node,string overrideType)
		{          
			// Check if the value is null
			bool isNil = (XmlHelper.GetAttributeValue(node, "i:nil") == "true");
			object value = null;
			if (!isNil)
			{

                string typeName = overrideType;
                if (typeName==null) typeName = _removeNsPrefix(XmlHelper.GetAttributeValue(node, "i:type"));

				string stringValue = XmlHelper.GetNodeTextValue(node);
				switch (typeName)
				{
					case AttributeTypes.EntityReference:
						EntityReference entityReferenceValue = new EntityReference(
                            new Guid(XmlHelper.SelectSingleNodeValue(node, "Id")),
                            XmlHelper.SelectSingleNodeValue(node, "LogicalName"),
                            XmlHelper.SelectSingleNodeValue(node, "Name"));
						
						value = entityReferenceValue;
						break;
					case AttributeTypes.AliasedValue:
						value = DeSerialise(XmlHelper.SelectSingleNode(node, "Value"),null);
						break;
					case AttributeTypes.Boolean_:
						value = (stringValue == "true");
						break;
                    case AttributeTypes.Double_:
                        value = double.Parse(stringValue);
                        break;
					case AttributeTypes.Decimal_:
						value = decimal.Parse(stringValue);
						break;
					case AttributeTypes.DateTime_:
						DateTime dateValue = DateTimeEx.Parse(stringValue);

                        // We need it in the CRM Users timezone
                        UserSettings settings = OrganizationServiceProxy.UserSettings;
                        if (settings != null)
                        {
                            // Remove the local date formating so that it is in UTC irrespective of the local timezone
                            dateValue.SetTime(dateValue.GetTime() + (dateValue.GetTimezoneOffset() * 60 * 1000));

                            DateTime localDateValue = DateTimeEx.UTCToLocalTimeFromSettings(dateValue, settings);
                            value = localDateValue;
                        }
                        else
                            value = dateValue;

						break;
					case "guid":
						value = new Guid(stringValue);
						break;
					case AttributeTypes.Int_:
						value = int.Parse(stringValue);
						break;
					case AttributeTypes.OptionSetValue:
						value = OptionSetValue.Parse(XmlHelper.SelectSingleNodeValue(node, "Value"));
						break;
                    case AttributeTypes.Money:
                        value = new Money(decimal.Parse(XmlHelper.SelectSingleNodeValue(node, "Value")));   
                        break;
					case AttributeTypes.EntityCollection:
                        value = EntityCollection.DeSerialise(node);
                        break;
                    default:
						value = stringValue;
						break;

				}
			}
			return value;
		}
		
        /// <summary>
        /// Serialises the attribute to xml to pass to the SDK webservices
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
		public static string Serialise(string attributeName, object value, Dictionary metaData)
		{
			string xml = "<a:KeyValuePairOfstringanyType><b:key>" + attributeName + "</b:key>";
            // Is there a metadata type because we can't infer the type from the JavaScript type (e.g. int, decimal, double (float))
            string typeName = value.GetType().Name;
            if (value != null && metaData != null && metaData.ContainsKey(attributeName))
                typeName = (string)metaData[attributeName];

            xml += SerialiseValue( value,typeName); ;
			xml += "</a:KeyValuePairOfstringanyType>";
			return xml;
		}

        public static string SerialiseValue( object value,string overrideTypeName)
        {
            string valueXml = "";
            string typeName = overrideTypeName;
            if (typeName == null)
                typeName = value.GetType().Name;

            switch (typeName)
            {
                case "String":
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.String_) + "\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">";
                    valueXml += XmlHelper.Encode((string)value);
                    valueXml += "</b:value>";
                    break;
                case "Boolean":
                case "bool":
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.Boolean_) + "\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">";
                    valueXml += XmlHelper.Encode(value.ToString());
                    valueXml += "</b:value>";
                    break;
                case "Date":
                    DateTime dateValue = (DateTime)value;
                    string dateString = null;

                    UserSettings settings = OrganizationServiceProxy.UserSettings;
                    if (settings != null)
                    {
                        DateTime utcDateValue = DateTimeEx.LocalTimeToUTCFromSettings(dateValue, settings);
                        dateString = DateTimeEx.ToXrmString(utcDateValue);
                    }
                    else
                        dateString = DateTimeEx.ToXrmStringUTC(dateValue);


                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.DateTime_) + "\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">";
                    valueXml += XmlHelper.Encode(dateString);
                    valueXml += "</b:value>";
                    break;
                case "decimal":
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.Decimal_) + "\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">";
                    string decStringValue = null;
                    if (value != null)
                        decStringValue = value.ToString();
                    valueXml += XmlHelper.Encode(decStringValue);
                    valueXml += "</b:value>";
                    break;
                case "double":
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.Double_) + "\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">";
                    string doubleStringValue = null;
                    if (value != null)
                        doubleStringValue = value.ToString();
                    valueXml += XmlHelper.Encode(doubleStringValue);
                    valueXml += "</b:value>";
                    break;
                case "int":
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.Int_) + "\" xmlns:c=\"http://www.w3.org/2001/XMLSchema\">";
                    string intStringValue = null;
                    if (value != null)
                        intStringValue = value.ToString();
                    valueXml += XmlHelper.Encode(intStringValue);
                    valueXml += "</b:value>";
                    break;
                case "Guid":
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(AttributeTypes.Guid_) + "\" xmlns:c=\"http://schemas.microsoft.com/2003/10/Serialization/\">";
                    valueXml += ((Guid)value).Value;
                    valueXml += "</b:value>";
                    break;
                case AttributeTypes.EntityReference:
                    EntityReference entityReferenceValue = (EntityReference)value;

                    valueXml += "<b:value i:type=\"" + _addNsPrefix(typeName) + "\">";

                    valueXml += "<a:Id>" + entityReferenceValue.Id + "</a:Id><a:LogicalName>" + entityReferenceValue.LogicalName + "</a:LogicalName>";
                    valueXml += "</b:value>";
                    break;
                case AttributeTypes.OptionSetValue:
                    OptionSetValue opt = (OptionSetValue)value;
                    if (opt.Value != null)
                    {
                        valueXml += "<b:value i:type=\"" + _addNsPrefix(typeName) + "\">";
                        valueXml += "<a:Value>" + opt.Value + "</a:Value>";
                        valueXml += "</b:value>";
                    }
                    else
                        valueXml += "<b:value i:type=\"" + _addNsPrefix(typeName) + "\" i:nil=\"true\"/>";
                    break;
                
                case AttributeTypes.EntityCollection:
                    valueXml += "<b:value i:type=\"" + _addNsPrefix(typeName) + "\">";
                    // Serialise each entity in the collection

                    valueXml += EntityCollection.Serialise((EntityCollection)value);
                    valueXml += "</b:value>";
                    break;
                case AttributeTypes.Money:
                    Money money = (Money)value;

                    if (money != null && (decimal?)money.Value!=null)
                    {
                        valueXml += "<b:value i:type=\"" + _addNsPrefix(typeName) + "\">";
                        valueXml += "<a:Value>" + money.Value.ToString() + "</a:Value>";
                        valueXml += "</b:value>";
                    }
                    else
                        valueXml += "<b:value i:type=\"" + _addNsPrefix(typeName) + "\" i:nil=\"true\"/>";
                    break;
                case "EntityFilters":
                    int entityFilterValue = (int)value;
                    List<string> entityFilterValues = new List<string>();
                    if ((1 & entityFilterValue)==1) entityFilterValues.Add("Entity");
                    if ((2 & entityFilterValue)==2) entityFilterValues.Add("Attributes");
                    if ((4 & entityFilterValue)==4) entityFilterValues.Add("Privileges");
                    if ((8 & entityFilterValue)==8) entityFilterValues.Add("Relationships");
                    valueXml +="<b:value i:type=\"c:EntityFilters\" xmlns:c=\"http://schemas.microsoft.com/xrm/2011/Metadata\">" + XmlHelper.Encode(entityFilterValues.Join(" ")) + "</b:value>";
                    break;
 
                default:
                    valueXml += "<b:value i:nil=\"true\"/>";
                    break;
            }
            return valueXml;
        }

		private static string _removeNsPrefix(string node)
		{
			int i = node.IndexOf(":");
			return node.Substring(i + 1, node.Length - i + 1);
		}
		private static string _addNsPrefix(string type)
		{

			switch (type)
			{
				case "String":
				case "Guid":
				case "DateTime":
				case AttributeTypes.String_:
				case AttributeTypes.Decimal_:
                case AttributeTypes.Double_:
				case AttributeTypes.Boolean_:
				case AttributeTypes.DateTime_:
				case AttributeTypes.Guid_:
				case AttributeTypes.Int_:
					return "c:" + type;
				case AttributeTypes.EntityReference:
				case AttributeTypes.OptionSetValue:
				case AttributeTypes.AliasedValue:
				case AttributeTypes.EntityCollection:
                case AttributeTypes.Money:
					return "a:" + type;
			}
			throw new Exception("Could not add node prefix for type " + type);
        }
        #endregion

        #region WebAPI
        internal static void SerialiseWebApiAttribute(Type attributeType, object attributeValue, Action<object> callback, Action<object> errorCallBack, bool async)
        {
            // For optimal performance - ensure we already have the metadata to resolve the entity set names for the lookups
            Type parameterType = attributeValue.GetType();
            if (parameterType == typeof(EntityReference))
            {            
                Dictionary<string, string> entityRef = new Dictionary<string, string>();       
                EntityReference parameterEntityref = (EntityReference)attributeValue;
                if (parameterEntityref.Id == null || parameterEntityref.Id.Value == null)
                {
                    throw new Exception("Id not set on EntityReference");
                }
                WebApiOrganizationServiceProxy.GetEntityMetadata(parameterEntityref.LogicalName, delegate (WebApiEntityMetadata metadata)
                 {
                     string entitySetName = metadata.EntitySetName;
                     entityRef["@odata.id"] = WebApiOrganizationServiceProxy.GetResource(entitySetName, parameterEntityref.Id.Value);
                     callback(entityRef);
                 }, errorCallBack, async);
            }
            else if (parameterType == typeof(EntityCollection))
            {
                EntityCollection collection = (EntityCollection)attributeValue;
                EntityCollection.SerialiseWebApi(collection, callback, errorCallBack, async);
            }
            else if (parameterType == typeof(Entity))
            {
                Entity entity = (Entity)attributeValue;
                Entity.SerialiseWebApi(entity, callback, errorCallBack, async);
            }
            else if (parameterType == typeof(OptionSetValue))
            {
                OptionSetValue optionValue = (OptionSetValue)attributeValue;
                callback(optionValue.Value);
            }
            else if (parameterType == typeof(Guid))
            {
                Guid guidValue = (Guid)attributeValue;
                callback(guidValue.Value);
            }
            else if (parameterType == typeof(Money))
            {
                Money moneyValue = (Money)attributeValue;
                callback(moneyValue.Value);
            }
            else
            {
                callback(attributeValue);
            }
        }

        #endregion
    }
}
