// AttributeMetadata.cs
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
        public static AttributeMetadata DeSerialiseAttributeMetadata(AttributeMetadata item, XmlNode attribute)
        {

            // Get the attributemetadata
           
            foreach (XmlNode node in attribute.ChildNodes)
            {
                Dictionary<string, object> itemValues = (Dictionary<string, object>)(object)item;
                string localName = XmlHelper.GetLocalName(node);
                string fieldName = localName;
              
                // Check nil and don't set the value to save time/space
                if (node.Attributes.Count == 1 && node.Attributes[0].Name == "i:nil")
                {
                    continue;
                }

                // Non Type Specific properties
                switch (localName)
                {
                    // String values
                    case "AttributeOf":
                    case "DeprecatedVersion":
                    case "EntityLogicalName":
                    case "LogicalName":
                    case "SchemaName":
                    case "CalculationOf":
                        itemValues[fieldName] = XmlHelper.GetNodeTextValue(node);
                        break;

                    // Bool values
                    case "CanBeSecuredForCreate":
                    case "CanBeSecuredForRead":
                    case "CanBeSecuredForUpdate":
                    case "CanModifyAdditionalSettings":
                    case "IsAuditEnabled":
                    case "IsCustomAttribute":
                    case "IsCustomizable":
                    case "IsManaged":
                    case "IsPrimaryId":
                    case "IsPrimaryName":
                    case "IsRenameable":
                    case "IsSecured":
                    case "IsValidForAdvancedFind":
                    case "IsValidForCreate":
                    case "IsValidForRead":
                    case "IsValidForUpdate":
                    case "DefaultValue":
                        itemValues[fieldName] = Attribute.DeSerialise(node, AttributeTypes.Boolean_);
                        break;

                    // Int Values
                    case "ColumnNumber":
                    case "Precision":
                    case "DefaultFormValue":
                    case "MaxLength":
                    case "PrecisionSource":
                        itemValues[fieldName] = Attribute.DeSerialise(node, AttributeTypes.Int_);
                        break;
                    
                    // Label
                    case "Description":
                    case "DisplayName":
                        Label label = new Label();
                        itemValues[fieldName] = MetadataSerialiser.DeSerialiseLabel(label,node);
                        break;

                    //OptionSet
                    case "OptionSet":
                        OptionSetMetadata options= new OptionSetMetadata();
                        itemValues[fieldName] = MetadataSerialiser.DeSerialiseOptionSetMetadata(options, node);
                        break;

                    case "AttributeType":
                        item.AttributeType = (AttributeTypeCode)(object)XmlHelper.GetNodeTextValue(node);
                        break;
                    //Guid
                    // LinkedAttributeId

                    //AttributeRequiredLevel
                    //RequiredLevel

                    //DateTime
                    //MaxSupportedValue (DateTimeAttributeMetadata)
                    //MinSupportedValue (DateTimeAttributeMetadata)

                    //decimal 
                    //MaxValue (DecimalAttributeMetadata)

                    //IntegerFormat
                    //Format

                    //string[]
                    //Targets

                    
                  
                }

                // Type sepcific attributes

                //Boolean 
                //DefaultValue (OptionSetMetadata)

                //int
                // MaxValue (IntegerAttributeMetadata)
                // MinValue (IntegerAttributeMetadata)

                // StringFormat
                //Format (MemoAttributeMetadata,StringAttributeMetadata)

                //double
                //MaxValue (DoubleAttributeMetadata, MoneyAttributeMetadata)
                //MinValue (DoubleAttributeMetadata, MoneyAttributeMetadata)

                //DateTimeFormat
                //Format (DateTimeAttributeMetadata)

                //long 
                //MaxValue (BigIntAttributeMetadata)
                //MinValue (BigIntAttributeMetadata)

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
    public class AttributeMetadata
    {


        // Summary:
        //     Gets the name of that attribute that this attribute extends.
        //
        // Returns:
        //     Type: Returns_String The attribute name.
        [PreserveCase]
        public string AttributeOf;
        //
        // Summary:
        //     Gets the type for the attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode> The
        //     attribute type.
        [PreserveCase]
        public AttributeTypeCode AttributeType;

        // Summary:
        //     Gets whether field security can be applied to prevent a user from adding
        //     data to this attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the field security can be
        //     applied; otherwise, false.
        [PreserveCase]
        public bool? CanBeSecuredForCreate;
        //
        // Summary:
        //     Gets whether field level security can be applied to prevent a user from viewing
        //     data from this attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the field security can be
        //     applied; otherwise, false.
        [PreserveCase]
        public bool? CanBeSecuredForRead;
        //
        // Summary:
        //     Gets whether field level security can be applied to prevent a user from updating
        //     data for this attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the field security can be
        //     applied; otherwise, false.
        [PreserveCase]
        public bool? CanBeSecuredForUpdate;
        //
        // Summary:
        //     Gets or sets the property that determines whether any settings not controlled
        //     by managed properties can be changed.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether any settings not controlled by managed properties can be changed.
        [PreserveCase]
        public bool? CanModifyAdditionalSettings;
        //
        // Summary:
        //     Gets an organization specific id for the attribute used for auditing.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The organization specific id for the
        //     attribute used for auditing.
        [PreserveCase]
        public int? ColumnNumber;
        //
        // Summary:
        //     Gets the pn_microsoftcrm version that the attribute was deprecated in.
        //
        // Returns:
        //     Type: Returns_String The pn_microsoftcrm version that the attribute was deprecated
        //     in.
        [PreserveCase]
        public string DeprecatedVersion;
        //
        // Summary:
        //     Gets or sets the description of the attribute.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.LabelThe description of the attribute.
        [PreserveCase]
        public Label Description;
        //
        // Summary:
        //     Gets or sets the display name for the attribute.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.LabelThe display name of the attribute.
        [PreserveCase]
        public Label DisplayName;
        //
        // Summary:
        //     Gets the logical name of the entity that contains the attribute.
        //
        // Returns:
        //     Type: Returns_String The logical name of the entity that contains the attribute.
        [PreserveCase]
        public string EntityLogicalName;
        //
        // Summary:
        //     Gets or sets the property that determines whether the attribute is enabled
        //     for auditing.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedProperty The property that determines
        //     whether the attribute is enabled for auditing.
        [PreserveCase]
        public bool? IsAuditEnabled;
        //
        // Summary:
        //     Gets whether the attribute is a custom attribute.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the attribute is a custom
        //     attribute; otherwise, false.
        [PreserveCase]
        public bool? IsCustomAttribute;
        //
        // Summary:
        //     Gets or sets the property that determines whether the attribute allows customization.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the attribute allows customization.
        [PreserveCase]
        public bool? IsCustomizable;
        //
        // Summary:
        //     Gets whether the attribute is part of a managed solution.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the attribute is part of a
        //     managed solution; otherwise, false.
        [PreserveCase]
        public bool? IsManaged;
        //
        // Summary:
        //     Gets whether the attribute represents the unique identifier for the record.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the attribute is the unique
        //     identifier for the record; otherwise, false.
        [PreserveCase]
        public bool? IsPrimaryId;
        //
        // Summary:
        //     Gets or sets whether the attribute represents the primary attribute for the
        //     entity.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the attribute is primary attribute
        //     for the entity; otherwise, false.
        [PreserveCase]
        public bool? IsPrimaryName;
        //
        // Summary:
        //     Gets or sets the property that determines whether the attribute display name
        //     can be changed.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the attribute display name can be changed.
        [PreserveCase]
        public bool? IsRenameable;
        //
        // Summary:
        //     Gets or sets whether the attribute is secured for field level security.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the attribute is secured for
        //     field level security; otherwise, false.
        [PreserveCase]
        public bool? IsSecured;
        //
        // Summary:
        //     Gets or sets the property that determines whether the attribute appears in
        //     Advanced Find.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the attribute appears in Advanced Find.
        [PreserveCase]
        public bool? IsValidForAdvancedFind;
        //
        // Summary:
        //     Gets whether the value can be set when a record is created.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the value can be set when
        //     a record is created; otherwise, false.
        [PreserveCase]
        public bool? IsValidForCreate;
        //
        // Summary:
        //     Gets whether the value can be retrieved.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the value can be retrieved;
        //     otherwise, false.
        [PreserveCase]
        public bool? IsValidForRead;
        //
        // Summary:
        //     Gets whether the value can be updated.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the value can be updated;
        //     otherwise, false.
        [PreserveCase]
        public bool? IsValidForUpdate;
        //
        // Summary:
        //     Gets or sets an attribute that is linked between Appointments and Recurring
        //     appointments.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Guid> The attribute id that is linked between
        //     Appointments and Recurring appointments.
        [PreserveCase]
        public Guid LinkedAttributeId;
        //
        // Summary:
        //     Gets or sets the logical name for the attribute.
        //
        // Returns:
        //     Type: Returns_String The logical name for the attribute.
        [PreserveCase]
        public string LogicalName;
        //
        // Summary:
        //     Gets or sets the property that determines the data entry requirement level
        //     enforced for the attribute.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.AttributeRequiredLevelManagedPropertyThe
        //     property that determines the data entry requirement level enforced for the
        //     attribute.
        [PreserveCase]
        public AttributeRequiredLevel RequiredLevel;
        //
        // Summary:
        //     Gets or sets the schema name for the attribute.
        //
        // Returns:
        //     Type: Returns_String The schema name for the attribute.
        [PreserveCase]
        public string SchemaName;
    }
}
