// Entity.cs
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Xrm.ComponentModel;

namespace Xrm.Sdk
{
    public enum EntityStates
    {
        Unchanged = 0,
        Created = 1,
        Changed = 2,
        Deleted = 3,
        ReadOnly = 4

    }
    public class Entity :INotifyPropertyChanged
    {
        #region Fields
        protected Dictionary _metaData = new Dictionary();
        public string LogicalName;
        public string Id;
        public EntityStates EntityState;
        
        private Dictionary<string,object> _attributes;
        public Dictionary<string, string> FormattedValues;
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
                    && !StringEx.IN(key, new string[] { "id","logicalName","entityState","formattedValues"}) && !key.StartsWith("$") && !key.StartsWith("_"))
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
                        if ((bool)Script.Literal("{0}<{1}", l, r))
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
    }
}
