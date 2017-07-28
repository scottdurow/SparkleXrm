// Entity.cs
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.ComponentModel;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public enum EntityStates
    {
        Unchanged = 0,
        Created = 1,
        Changed = 2,
        Deleted = 3,
        ReadOnly = 4

    }
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class Entity :INotifyPropertyChanged
    {
        #region Fields
        protected Dictionary _metaData = new Dictionary();
        public string _entitySetName;
        public string LogicalName;
        public string Id;
        public EntityStates EntityState;
        
        private Dictionary<string,object> _attributes;
        public Dictionary<string, string> FormattedValues;
        //
        // Summary:
        //     Gets or sets a collection of entity references (references to records).
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.RelatedEntityCollection a collection of entity references
        //     (references to records).
        public Dictionary<string,EntityCollection> RelatedEntities;
        #endregion

        #region Constructors
        public Entity(string entityName)
        {
            this.LogicalName = entityName;
            this._attributes = new Dictionary();
            this.FormattedValues = new Dictionary();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the attribute values from SDK Webservice response on a business entity
        /// </summary>
        /// <param name="entityNode"></param>
        public void DeSerialise(XmlNode entityNode) {
            this.LogicalName = XmlHelper.SelectSingleNodeValue(entityNode, "LogicalName");
            this.Id = XmlHelper.SelectSingleNodeValue(entityNode, "Id");
            XmlNode attributes = XmlHelper.SelectSingleNode(entityNode,"Attributes");
            int attributeCount = attributes.ChildNodes.Count;
            for (int i = 0; i < attributeCount; i++) {
                XmlNode node = attributes.ChildNodes[i];
                try {
                    // Add typed attribute value
                    string attributeName = XmlHelper.SelectSingleNodeValue(node, "key");
                    object attributeValue = Attribute.DeSerialise(XmlHelper.SelectSingleNode(node, "value"),null);
                    this._attributes[attributeName] = attributeValue;

                    SetDictionaryValue(attributeName, attributeValue);
                }
                catch (Exception e) {
                    throw new Exception("Invalid Attribute Value :" + XmlHelper.GetNodeTextValue(node) + ":" + e.Message);
                }
            }
            // Get Formatted values
            XmlNode formattedValues = XmlHelper.SelectSingleNode(entityNode, "FormattedValues");
            if (formattedValues != null)
            {
                for (int i = 0; i < formattedValues.ChildNodes.Count;i++ )
                {
                    XmlNode node = formattedValues.ChildNodes[i];
                    string key = XmlHelper.SelectSingleNodeValue(node, "key");
                    string value = XmlHelper.SelectSingleNodeValue(node, "value");
                    SetDictionaryValue(key+"name", value);
                    FormattedValues[key+"name"] = value;
                    object att = this._attributes[key];
                    if (att != null)
                    {
                        Script.Literal("{0}.name={1}", att, value);
                    }
                }
            }
            // Get related entities
            XmlNode relatedEntities = XmlHelper.SelectSingleNode(entityNode, "RelatedEntities");
            if (relatedEntities != null)
            {
                Dictionary<string,EntityCollection> relatedEntitiesColection = new Dictionary<string,EntityCollection>();
                for (int i = 0; i < relatedEntities.ChildNodes.Count; i++)
                {
                    XmlNode node = relatedEntities.ChildNodes[i];
                    XmlNode key = XmlHelper.SelectSingleNode(node, "key");
                    string schemaName = XmlHelper.SelectSingleNodeValue(key, "SchemaName");
                    Relationship relationship = new Relationship(schemaName);
                    XmlNode value = XmlHelper.SelectSingleNode(node, "value");
                    EntityCollection entities = EntityCollection.DeSerialise(value);
                    relatedEntitiesColection[relationship.SchemaName] = entities;
                }
                this.RelatedEntities = relatedEntitiesColection;

            }
        }

        private void SetDictionaryValue(string key, object value)
        {
            object self = this;
            Dictionary thisAsDictionary = self as Dictionary;
            thisAsDictionary[key] = value;
        }

      
        //// ---------------------------------------------------------------
        /// <summary>
        /// Serialises the entity to xml to pass to the SDK webservices
        /// </summary>
        /// <param name="ommitRoot"></param>
        /// <returns></returns>
        public string Serialise(bool? ommitRoot) {
            string xml = "";
            if (ommitRoot == null || ommitRoot == false)
                xml += "<a:Entity>";

            xml += "<a:Attributes>";

            Dictionary<string, object> record = (Dictionary<string, object>)((object)this);
            // Check primary key attribute - Guid's can't have a null value
            if (record[this.LogicalName + "id"] == null)
            {
                record.Remove(this.LogicalName + "id");
            }
            foreach (string key in record.Keys)
            {
                // Exclude the built in properties
                if ((bool)Script.Literal(@"typeof({0}[{1}])!=""function""",record,key)
                    && (bool)Script.Literal("Object.prototype.hasOwnProperty.call({0}, {1})", this, key) 
                    && !StringEx.IN(key, new string[] { "id","logicalName","entityState","formattedValues","relatedEntities"}) && !key.StartsWith("$") && !key.StartsWith("_"))
                {
                    object attributeValue = record[key];
                    if (!FormattedValues.ContainsKey(key))
                        xml += Attribute.Serialise(key, attributeValue,_metaData);
                }
            }

            xml += "</a:Attributes>";
            xml += "<a:LogicalName>" + this.LogicalName + "</a:LogicalName>";
            if (this.Id != null) xml += "<a:Id>" + this.Id + "</a:Id>";
            if (ommitRoot == null || ommitRoot == false)
                xml += "</a:Entity>";

            return xml;
        }

        public void SetAttributeValue(string name, object value) {
            this._attributes[name] = value;
            SetDictionaryValue(name, value);
        }

        public object GetAttributeValue(string attributeName)
        {
            return Script.Literal("this[{0}]",attributeName);
        }
       
        public OptionSetValue GetAttributeValueOptionSet(string attributeName)
        {
            return (OptionSetValue)this.GetAttributeValue(attributeName);
        }

        public Guid GetAttributeValueGuid(string attributeName)
        {
            return (Guid)this.GetAttributeValue(attributeName);
        }

        public int? GetAttributeValueInt(string attributeName)
        {
            return (int?)this.GetAttributeValue(attributeName);
        }

        public float? GetAttributeValueFloat(string attributeName)
        {
            return (float?)this.GetAttributeValue(attributeName);
        }

        public string GetAttributeValueString(string attributeName)
        {
            return (string)this.GetAttributeValue(attributeName);
        }

        public EntityReference GetAttributeValueEntityReference(string attributeName)
        {
            return (EntityReference)this.GetAttributeValue(attributeName);
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs();
            args.PropertyName = propertyName;
            if (PropertyChanged!=null)
                PropertyChanged(this, args);

            if (propertyName != "EntityState" && EntityState == EntityStates.Unchanged && EntityState != EntityStates.Created)
                EntityState = EntityStates.Changed;
        }

        public EntityReference ToEntityReference()
        {
            return new EntityReference(new Guid(this.Id), this.LogicalName, "");
        }

        public static int SortDelegate(string attributeName, Entity a, Entity b)
        {

            object l = a.GetAttributeValue(attributeName);
            object r = b.GetAttributeValue(attributeName);
            decimal result = 0;

            string typeName = "";
            if (l != null)
                typeName = l.GetType().Name;
            else if (r != null)
                typeName = r.GetType().Name;
            
            if (l != r)
            {
                switch (typeName.ToLowerCase())
                {
                    case "string":
                        l = l != null ? ((string)l).ToLowerCase() : null;
                        r = r != null ? ((string)r).ToLowerCase() : null;
                        if ((bool)Script.Literal("{0}<{1}", l, r))
                            result = -1;
                        else
                            result = 1;
                        break;
                    case "date":
                        if (l == null)
                            result = -1;
                        else if (r == null)
                            result = 1;
                        else if ((bool)Script.Literal("{0}<{1}", l, r))
                            result = -1;
                        else
                            result = 1;
                        break;
                    case "number":
                        decimal ln = l != null ? ((decimal)l) : 0;
                        decimal rn = r != null ? ((decimal)r) : 0;
                        result = (ln - rn);
                        break;
                    case "money":
                        decimal lm = l != null ? ((Money)l).Value : 0;
                        decimal rm = r != null ? ((Money)r).Value : 0;
                        result = (lm - rm);
                        break;
                    case "optionsetvalue":
                        int? lo = l != null ? ((OptionSetValue)l).Value : 0;
                        lo = lo != null ? lo : 0;
                        int? ro = r != null ? ((OptionSetValue)r).Value : 0;
                        ro = ro != null ? ro : 0;
                        result = (decimal)(lo - ro);
                        break;
                    case "entityreference":
                        string le = (l != null) && (((EntityReference)l).Name != null) ? ((EntityReference)l).Name : "";
                        string re = r != null && (((EntityReference)r).Name != null) ? ((EntityReference)r).Name : "";
                        if ((bool)Script.Literal("{0}<{1}", le, re))
                            result = -1;
                        else
                            result = 1;
                        break;

                }
            }
            return (int)result;
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region WebAPI
        internal static void SerialiseWebApi(Entity entity, Action<object> completeCallback, Action<object> errorCallback, bool async)
        {

            // Add standard lookups
            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata(entity.LogicalName, "transactioncurrencyid", "transactioncurrency");
            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata(entity.LogicalName, "ownerid", "systemuser,team");
            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata(entity.LogicalName, "createdby", "systemuser");
            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata(entity.LogicalName, "modifiedby", "systemuser");
            WebApiOrganizationServiceProxy.AddNavigationPropertyMetadata(entity.LogicalName, "customerid", "account,contact");

            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            List<object> lookupsToResolve = new List<object>();
            Dictionary<string, string> lookupAttributes = new Dictionary<string, string>();

            // add the data type
            // e.g "@odata.type": "Microsoft.Dynamics.CRM.account"
            jsonObject["@odata.type"] = "Microsoft.Dynamics.CRM." + entity.LogicalName;

            foreach (string attribute in ((Dictionary<string, object>)(object)entity).Keys)
            {
                if (IsEntityAttribute(entity, attribute))
                {
                    // TODO:
                    // this can reuse the attribute.serialise but doing the lookup resolving first

                    Type attributeType = entity.GetAttributeValue(attribute).GetType();

                    string odataAttributeName = attribute;
                    string key = entity.LogicalName + "." + attribute;
                    object value = entity.GetAttributeValue(attribute);
                    // Is there a navigation property - this is where we can't use the logicalname
                    // but instead the _<attribtue>_value or _<attribute>_entityLogicalName
                    if (WebApiOrganizationServiceProxy.LogicalNameToNavMapping.ContainsKey(key))
                    {
                        attributeType = typeof(EntityReference);
                        // Get the matching type (unless the value is null)
                        odataAttributeName = attribute;
                        if (WebApiOrganizationServiceProxy.LogicalNameToNavMapping[key].Length > 1 && (value == null))
                        {
                            // When setting null we need to set all the navigation properties to null
                            // we use the suposedly 'readonly' lookup field
                            foreach (string type in WebApiOrganizationServiceProxy.LogicalNameToNavMapping[key])
                            {
                                jsonObject[odataAttributeName + "_" + type + "@odata.bind"] = null;
                            }
                            odataAttributeName = null;
                            attributeType = null;
                        }
                        else if (WebApiOrganizationServiceProxy.LogicalNameToNavMapping[key].Length > 1 && value != null)
                        {
                            EntityReference entityRef = (EntityReference)value;
                            foreach (string type in WebApiOrganizationServiceProxy.LogicalNameToNavMapping[key])
                            {
                                if (type == entityRef.LogicalName)
                                {
                                    odataAttributeName += "_" + type;
                                    break;
                                }
                            }
                        }
                    }

                    // Get type
                    if (attributeType == typeof(EntityReference))
                    {
                        // Convert to the odata
                        if (value != null)
                        {
                            lookupsToResolve.Add(entity.GetAttributeValueEntityReference(attribute));
                        }
                        lookupAttributes[attribute] = odataAttributeName;
                    }
                    else if (attributeType == typeof(OptionSetValue))
                    {
                        OptionSetValue optionValue = entity.GetAttributeValueOptionSet(attribute);
                        jsonObject[odataAttributeName] = optionValue.Value;
                    }
                    else if (attributeType == typeof(Money))
                    {
                        Money moneyValue = (Money)entity.GetAttributeValue(attribute);
                        jsonObject[odataAttributeName] = moneyValue.Value;
                    }
                    else if (attributeType == typeof(Guid))
                    {
                        Guid guidValue = (Guid)entity.GetAttributeValue(attribute);
                        jsonObject[odataAttributeName] = guidValue.Value;
                    }
                    else if (odataAttributeName != null)
                    {
                        jsonObject[odataAttributeName] = entity.GetAttributeValue(attribute);
                    }
                }
            }

            WebApiOrganizationServiceProxy.MapLookupsToEntitySets(lookupsToResolve, delegate ()
            {
                foreach (string attribute in lookupAttributes.Keys)
                {
                    EntityReference lookup = entity.GetAttributeValueEntityReference(attribute);
                    string lookupValue = null;
                    if (lookup != null)
                    {
                        string entitysetname = WebApiOrganizationServiceProxy.WebApiRequiredMetadataCache[lookup.LogicalName].EntitySetName;
                        lookupValue = lookup != null ? "/" + WebApiOrganizationServiceProxy.GetResource(entitysetname, lookup.Id.Value) : null;
                    }
                    jsonObject[lookupAttributes[attribute] + "@odata.bind"] = lookupValue;
                }

                completeCallback(jsonObject);
            },
            errorCallback, async);


        }

        private static bool IsEntityAttribute(Entity record, string key)
        {
            return ((bool)Script.Literal(@"typeof({0}[{1}])!=""function""", record, key)
                    && (bool)Script.Literal("Object.prototype.hasOwnProperty.call({0}, {1})", record, key)
                    && !StringEx.IN(key, new string[] { "id", "logicalName", "entityState", "formattedValues", "relatedEntities" }) && !key.StartsWith("$") && !key.StartsWith("_"));
        }

        public void DeSerialiseWebApi(Dictionary<string, object> pojoEntity)
        {
            // If there is not logical name, then find it from the pojoEntity
            if (String.IsNullOrEmpty(this.LogicalName))
            {
                if (pojoEntity.ContainsKey("@odata.type"))
                {
                    this.LogicalName = ((string)pojoEntity["@odata.type"]).Substr("#Microsoft.Dynamics.CRM.".Length);
                }
                else
                {
                    throw new Exception("Logical name not set");
                }
            }
            // Set ID using the webapi metadata primary attribute
            WebApiEntityMetadata metadata = WebApiOrganizationServiceProxy.WebApiRequiredMetadataCache[this.LogicalName];
            this.Id = (string)pojoEntity[metadata.PrimaryAttributeLogicalName];

            foreach (string key in pojoEntity.Keys)
            {

                int posAt = key.IndexOf("@");
                bool containsAt = posAt > -1;
                bool navigationProperty = key.EndsWith("@Microsoft.Dynamics.CRM.associatednavigationproperty");
                bool underscore = key.StartsWith("_");

                if ((!containsAt && !underscore) || navigationProperty)
                {

                    object attributeValue = null;
                    string attributeType = null;
                    string attributeLogicalName = key;
                    string navigationPropertyLogicalName = null;
                    string attributeNameWithoutAt = key.Substr(0, posAt);

                    // We need to determine which type the field is here

                    /*
                    ---Dates---
                    Dates we use the 'DateReviver' pattern 
                    however this is very inefficient since it runs the regex on every field value
                    */

                    /*
                    ---Guid---
                    We use a regex to determine this type - but again this is inefficient 
                    */

                    /*
                    ---EntityReferene---
                    Entity reference we can infer from the presense of the Microsoft.Dynamics.CRM.lookuplogicalname
                    and Microsoft.Dynamics.CRM.associatednavigationproperty
                    _parentcustomerid_value@Microsoft.Dynamics.CRM.associatednavigationproperty=parentcustomerid_account
                    _parentcustomerid_value@Microsoft.Dynamics.CRM.lookuplogicalname=account
                    _parentcustomerid_value@OData.Community.Display.V1.FormattedValue=xyz

                    _primarycontactid_value@Microsoft.Dynamics.CRM.associatednavigationproperty=primarycontactid
                    _primarycontactid_value@Microsoft.Dynamics.CRM.lookuplogicalname=contact
                    _primarycontactid_value@OData.Community.Display.V1.FormattedValue=xyz
                    */
                    if (navigationProperty)
                    {
                        navigationPropertyLogicalName = (string)pojoEntity[key];
                        attributeLogicalName = attributeNameWithoutAt;
                    }

                    string navigationPropertyName = attributeNameWithoutAt + "@Microsoft.Dynamics.CRM.associatednavigationproperty";
                    string lookupLogicalName = attributeNameWithoutAt + "@Microsoft.Dynamics.CRM.lookuplogicalname";
                    string baseLogicalName = attributeLogicalName + "_base";
                    string formattedValueName = attributeLogicalName + "@OData.Community.Display.V1.FormattedValue";

                    if (navigationProperty && pojoEntity.ContainsKey(lookupLogicalName))
                    {
                        attributeType = AttributeTypes.EntityReference;
                    }
                    /*
                    ---Money---
                    Since the value is returned as a decimal - there is no way of inferring the money type
                    if decimal and there is a _base value - assume it's money
                    */
                    else if (pojoEntity.ContainsKey(baseLogicalName))
                    {
                        attributeType = AttributeTypes.Money;
                    }
                    /*
                    ---OptionSetValue--
                    If integer and formatted value then assume it's an optionsetvalue
                    territorycode@OData.Community.Display.V1.FormattedValue=Default Value
                    */
                    else if (pojoEntity.ContainsKey(formattedValueName))
                    {
                        attributeType = AttributeTypes.OptionSetValue;
                    }

                    /*
                    ---Aliased Value---
                    There doesn't seem to be any way of determining of a returned field value is an aliased value 
                    This means that there is no way of determining the type from querying metadata.
                    */
                    else if (_metaData.ContainsKey(attributeLogicalName))
                    {
                        // Use the manually specified metadata type for when we can't determine the metdata
                        attributeType = (string)_metaData[attributeLogicalName];
                    }


                    switch (attributeType)
                    {
                        case AttributeTypes.EntityReference:
                            string entityType = (string)pojoEntity[lookupLogicalName];
                            attributeValue = new EntityReference(
                                       new Guid((string)pojoEntity[attributeLogicalName]),
                                       entityType,
                                       (string)pojoEntity[formattedValueName]);

                            string lookupAttributeName = (string)pojoEntity[navigationPropertyName];

                            // Get the actual logical name of the attribute
                            if (lookupAttributeName.EndsWith("_" + entityType))
                            {
                                int typePos = lookupAttributeName.LastIndexOf("_" + entityType);
                                attributeLogicalName = lookupAttributeName.Substr(0, typePos);
                            }
                            else
                            {
                                attributeLogicalName = lookupAttributeName;
                            }
                            break;
                        case AttributeTypes.Money:
                            attributeValue = new Money((decimal)pojoEntity[attributeLogicalName]);
                            break;
                        case AttributeTypes.OptionSetValue:
                            OptionSetValue optionSetValue = new OptionSetValue((int?)pojoEntity[attributeLogicalName]);
                            optionSetValue.Name = (string)pojoEntity[formattedValueName];
                            attributeValue = optionSetValue;
                            break;
                        default:
                            // Default - set primitive type value
                            attributeValue = pojoEntity[attributeLogicalName];
                            break;
                    }
                    this.SetAttributeValue(attributeLogicalName, attributeValue);
                }
            }

            
        }
        #endregion
    }
}
