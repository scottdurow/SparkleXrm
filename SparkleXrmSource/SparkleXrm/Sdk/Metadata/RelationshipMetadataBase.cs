// RelationshipMetadataBase.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.Sdk.Metadata.Query
{
    
    public static partial class MetadataSerialiser
    {
        public static RelationshipMetadataBase DeSerialiseRelationshipMetadata(XmlNode attribute)
        {
            // Determine what type the item is
            // Get the type
            RelationshipMetadataBase item;
            string type = XmlHelper.GetAttributeValue(attribute, "i:type");
            switch (type)
            {
                case "c:OneToManyRelationshipMetadata":
                    item = new OneToManyRelationshipMetadata();
                    break;
                case "c:ManyToManyRelationshipMetadata":
                    item = new ManyToManyRelationshipMetadata();
                    break;
                default:
                    throw new System.Exception("Unknown relationship type");
            }



            foreach (XmlNode node in attribute.ChildNodes)
            {
                Dictionary<string, object> itemValues = (Dictionary<string, object>)(object)item;
                string localName = XmlHelper.GetLocalName(node);
                string fieldName = localName.Substr(0, 1).ToLowerCase() + localName.Substr(1);

                // Check nil and don't set the value to save time/space
                if (node.Attributes.Count == 1 && node.Attributes[0].Name == "i:nil")
                {
                    continue;
                }

                // Non Type Specific properties
                switch (localName)
                {
                    // String values
                    case "SchemaName":
                    // OneToMany Attributes
                    case "ReferencedAttribute":
                    case "ReferencedEntity":
                    case "ReferencingAttribute":
                    case "ReferencingEntity":
                    // ManyToMany Attributes
                    case "Entity1IntersectAttribute":
                    case "Entity1LogicalName":
                    case "Entity2IntersectAttribute":
                    case "Entity2LogicalName":
                    case "IntersectEntityName":
                        itemValues[fieldName] = XmlHelper.GetNodeTextValue(node);
                        break;

                    // Bool values
                    case "IsCustomRelationship":
                    case "IsManaged":
                    case "IsValidForAdvancedFind":
                        itemValues[fieldName] = Attribute.DeSerialise(node, AttributeTypes.Boolean_);
                        break;
                    case "RelationshipType":
                        itemValues[fieldName] = (RelationshipType)(object)XmlHelper.GetNodeTextValue(node);
                        break;
                }
            }
            return item;

        }
    }
}
namespace Xrm.Sdk.Metadata
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class RelationshipMetadataBase
    {
        //public BooleanManagedProperty IsCustomizable;
        public bool? IsCustomRelationship;
        public bool? IsManaged;
        public bool? IsValidForAdvancedFind;
        public RelationshipType RelationshipType;
        public string SchemaName;
        //public SecurityTypes? SecurityTypes;
    }

    [NamedValues]
    [ScriptNamespace("SparkleXrm.Sdk.Metadata")]
    public enum RelationshipType
    {
        [ScriptName("OneToManyRelationship")]
        OneToManyRelationship = 0,
        [ScriptName("Default")]
        Default_ = 0,
        [ScriptName("ManyToManyRelationship")]
        ManyToManyRelationship = 1
    }
}
