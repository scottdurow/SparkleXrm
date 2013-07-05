// EntityMetadata.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Metadata
{
    public static partial class MetadataSerialiser
    {
        public static EntityMetadata DeSerialiseEntityMetadata(EntityMetadata item, XmlNode entity)
        {
            foreach (XmlNode node in entity.ChildNodes)
            {
                Dictionary<string,object> itemValues = (Dictionary<string,object>)(object)item;
                string localName = XmlHelper.GetLocalName(node);
                string fieldName = localName.Substr(0,1).ToLowerCase() + localName.Substr(1);
                // Bool values
                switch (localName)
                {
                        // String values
                    case "SchemaName":
                    case "IconSmallName":
                        itemValues[fieldName] = XmlHelper.GetNodeTextValue(node);
                        break;

                        // Bool values
                    case "IsValidForAdvancedFind":
                    case "IsCustomEntity":
                        itemValues[fieldName] = Attribute.DeSerialise(node,AttributeTypes.Boolean_);
                        break;

                        // Int Values
                    case "ObjectTypeCode":
                        itemValues[fieldName] = Attribute.DeSerialise(node, AttributeTypes.Int_);
                        break;
                }
          
            }
            return item;
        }

    }
    // Summary:
    //     Contains the metadata for an entity.
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class EntityMetadata
    {
        // Summary:
        //     constructor_initializesMicrosoft.Xrm.Sdk.Metadata.EntityMetadata class
        public EntityMetadata()
        {

        }
    
        // Summary:
        //     Gets or sets whether a custom activity should appear in the activity menus
        //     in the Web application.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The value indicates whether a custom
        //     activity should appear in the activity menus in the Web application.

        public int? ActivityTypeMask;
        //
        // Summary:
        //     Gets the array of attribute metadata for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.AttributeMetadata[]The array of attribute
        //     metadata for the entity.
       
        public AttributeMetadata[] Attributes;
        //
        // Summary:
        //     Gets or sets whether to automatically move records to the owner’s default
        //     queue when a record of this type is created or assigned.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the record will automatically
        //     move to the owner’s default queue when a record of this type is created or
        //     assigned; otherwise, false.
      
        public bool? AutoRouteToOwnerQueue;
        //
        // Summary:
        //     Gets the property that determines whether the entity can be in a Many-to-Many
        //     entity relationship.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the entity can be in a Many-to-Many entity relationship.
      
        public bool? CanBeInManyToMany;
        //
        // Summary:
        //     Gets the property that determines whether the entity can be the referenced
        //     entity in a One-to-Many entity relationship.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the entity can be the referenced entity in a One-to-Many entity relationship.
        public bool? CanBePrimaryEntityInRelationship;
        //
        // Summary:
        //     Gets the property that determines whether the entity can be the referencing
        //     entity in a One-to-Many entity relationship.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the entity can be the referencing entity in a One-to-Many entity
        //     relationship.
        public bool? CanBeRelatedEntityInRelationship;
        //
        // Summary:
        //     Gets or sets the property that determines whether additional attributes can
        //     be added to the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether additional attributes can be added to the entity.
        public bool? CanCreateAttributes;
        //
        // Summary:
        //     Gets or sets the property that determines whether new charts can be created
        //     for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether new charts can be created for the entity.
        
        public bool? CanCreateCharts;
        //
        // Summary:
        //     Gets or sets the property that determines whether new forms can be created
        //     for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether new forms can be created for the entity.

        public bool? CanCreateForms;
        //
        // Summary:
        //     Gets or sets the property that determines whether new views can be created
        //     for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether new views can be created for the entity.

        public bool? CanCreateViews;
        //
        // Summary:
        //     Gets or sets the property that determines whether any other entity properties
        //     not represented by a managed property can be changed.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether any other entity properties not represented by a managed property
        //     can be changed.

        public bool? CanModifyAdditionalSettings;
        //
        // Summary:
        //     Gets whether the entity can trigger a workflow process.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity can trigger a workflow
        //     process; otherwise, false.

        public bool? CanTriggerWorkflow;
        //
        // Summary:
        //     Gets or sets the label containing the description for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.LabelThe label containing the description for the
        //     entity.
        
        public Label Description;
        //
        // Summary:
        //     Gets or sets the label containing the plural display name for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.LabelThe label containing the plural display name
        //     for the entity.
        
        public Label DisplayCollectionName;
        //
        // Summary:
        //     Gets or sets the label containing the display name for the entity.
        //
        // Returns:
        //     Type Microsoft.Xrm.Sdk.LabelThe label containing the display name for the
        //     entity.
        
        public Label DisplayName;
        //
        // Summary:
        //     Gets or sets the name of the image web resource for the large icon for the
        //     entity.
        //
        // Returns:
        //     Type: Returns_String The name of the image web resource for the large icon
        //     for the entity..
        
        public string IconLargeName;
        //
        // Summary:
        //     Gets or sets the name of the image web resource for the medium icon for the
        //     entity.
        //
        // Returns:
        //     Type: Returns_String The name of the image web resource for the medium icon
        //     for the entity..
        
        public string IconMediumName;
        //
        // Summary:
        //     Gets or sets the name of the image web resource for the small icon for the
        //     entity.
        //
        // Returns:
        //     Type: Returns_String The name of the image web resource for the small icon
        //     for the entity..
        
        public string IconSmallName;
        //
        // Summary:
        //     Gets or sets whether the entity is an activity.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is an activity;
        //     otherwise, false.
        
        public bool? IsActivity;
        //
        // Summary:
        //     Gets or sets whether the email messages can be sent to an email address stored
        //     in a record of this type.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if emails can be sent to an email
        //     address stored in a record of this type; otherwise, false.
        
        public bool? IsActivityParty;
        //
        // Summary:
        //     Gets or sets the property that determines whether auditing has been enabled
        //     for the entity.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.BooleanManagedProperty>The property
        //     that determines whether auditing has been enabled for the entity.

        public bool? IsAuditEnabled;
        //
        // Summary:
        //     Gets or sets whether the entity is available offline.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is available offline;
        //     otherwise, false.
        
        public bool? IsAvailableOffline;
        //
        // Summary:
        //     Gets whether the entity is a child entity.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is a child entity;
        //     otherwise, false.
        
        public bool? IsChildEntity;
        //
        // Summary:
        //     Gets or sets the property that determines whether connections are enabled
        //     for this entity.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.BooleanManagedProperty>The property
        //     that determines whether connections are enabled for this entity.

        public bool? IsConnectionsEnabled;
        //
        // Summary:
        //     Gets whether the entity is a custom entity.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is a custom entity;
        //     otherwise, false.
        
        public bool? IsCustomEntity;
        //
        // Summary:
        //     Gets or sets the property that determines whether the entity is customizable.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the entity is customizable.

        public bool? IsCustomizable;
        //
        // Summary:
        //     Gets or sets the property that determines whether document management is
        //     enabled.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.BooleanManagedProperty>The property
        //     that determines whether document management is enabled..
        
        public bool? IsDocumentManagementEnabled;
        //
        // Summary:
        //     Gets or sets the property that determines whether duplicate detection is
        //     enabled.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether duplicate detection is enabled..

        public bool? IsDuplicateDetectionEnabled;
        //
        // Summary:
        //     Gets whether charts are enabled.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if charts are enabled; otherwise,
        //     false.
        
        public bool? IsEnabledForCharts;
        //
        // Summary:
        //     Gets whether the entity can be imported using the Import Wizard.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity can be imported
        //     using the Import Wizard; otherwise, false.
        
        public bool? IsImportable;
        //
        // Summary:
        //     Gets whether the entity is an intersection table for two other entities.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is an intersection
        //     table for two other entities.; otherwise, false
        
        public bool? IsIntersect;
        //
        // Summary:
        //     Gets or sets the property that determines whether mail merge is enabled for
        //     this entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether mail merge is enabled for this entity..

        public bool? IsMailMergeEnabled;
        //
        // Summary:
        //     Gets whether the entity is part of a managed solution.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is part of a managed
        //     solution; otherwise, false.
        
        public bool? IsManaged;
        //
        // Summary:
        //     Gets or sets the property that determines whether entity mapping is available
        //     for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether entity mapping is available for the entity..

        public bool? IsMappable;
        //
        // Summary:
        //     internal
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>internal
        
        public bool? IsReadingPaneEnabled;
        //
        // Summary:
        //     Gets or sets the property that determines whether the entity Microsoft.Xrm.Sdk.Metadata.EntityMetadata.DisplayName
        //     and Microsoft.Xrm.Sdk.Metadata.EntityMetadata.DisplayCollectionName can be
        //     changed by editing the entity in the application.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the entity Microsoft.Xrm.Sdk.Metadata.EntityMetadata.DisplayName
        //     and Microsoft.Xrm.Sdk.Metadata.EntityMetadata.DisplayCollectionName can be
        //     changed by editing the entity in the application.

        public bool? IsRenameable;
        //
        // Summary:
        //     Gets or sets whether the entity is will be shown in Advanced Find.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Boolean>true if the entity is will be shown
        //     in Advanced Find.; otherwise, false.
        
        public bool? IsValidForAdvancedFind;
        //
        // Summary:
        //     Gets or sets the property that determines whether the entity is enabled for
        //     queues.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether the entity is enabled for queues.

        public bool? IsValidForQueue;
        //
        // Summary:
        //     Gets or sets the property that determines whether pn_Mobile_Express_long
        //     users can see data for this entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.BooleanManagedPropertyThe property that determines
        //     whether pn_Mobile_Express_long users can see data for this entity.

        public bool? IsVisibleInMobile;
        //
        // Summary:
        //     Gets or sets the logical name for the entity.
        //
        // Returns:
        //     Type: Returns_String The logical name for the entity.
        
        public string LogicalName;
        //
        // Summary:
        //     Gets the array of many-to-many relationships for the entity.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.ManyToManyRelationshipMetadata[]the array
        //     of many-to-many relationships for the entity.
        
        //public ManyToManyRelationshipMetadata[] ManyToManyRelationships;
        //
        // Summary:
        //     Gets the array of many-to-one relationships for the entity.
        //
        // Returns:
        //     Returns Microsoft.Xrm.Sdk.Metadata.OneToManyRelationshipMetadata[].
        
        //public OneToManyRelationshipMetadata[] ManyToOneRelationships;
        //
        // Summary:
        //     Gets the entity type code.
        //
        // Returns:
        //     Type: Returns_Nullable<Returns_Int32> The entity type code.
        
        public int? ObjectTypeCode;
        //
        // Summary:
        //     Gets the array of one-to-many relationships for the entity.
        //
        // Returns:
        //     Type :Microsoft.Xrm.Sdk.Metadata.OneToManyRelationshipMetadata[]The array
        //     of one-to-many relationships for the entity.
        
        //public OneToManyRelationshipMetadata[] OneToManyRelationships;
        //
        // Summary:
        //     Gets or sets the ownership type for the entity.
        //
        // Returns:
        //     Type: Returns_Nullable<Microsoft.Xrm.Sdk.Metadata.OwnershipTypes> The ownership
        //     type for the entity..
        
        //public OwnershipTypes? OwnershipType;
        //
        // Summary:
        //     Gets the name of the attribute that is the primary id for the entity.
        //
        // Returns:
        //     Type: Returns_String The name of the attribute that is the primary id for
        //     the entity.
        
        public string PrimaryIdAttribute;
        //
        // Summary:
        //     Gets the name of the primary attribute for an entity.
        //
        // Returns:
        //     Type: Returns_String The name of the primary attribute for an entity..
        
        public string PrimaryNameAttribute;
        //
        // Summary:
        //     Gets the privilege metadata for the entity.
        //
        // Returns:
        //     Returns Microsoft.Xrm.Sdk.Metadata.SecurityPrivilegeMetadata[]The privilege
        //     metadata for the entity..
        
        //public SecurityPrivilegeMetadata[] Privileges;
        //
        // Summary:
        //     Gets the name of the entity that is recurring.
        //
        // Returns:
        //     Type: Returns_String The name of the entity that is recurring.
        
        public string RecurrenceBaseEntityLogicalName;
        //
        // Summary:
        //     Gets the name of the report view for the entity.
        //
        // Returns:
        //     Type: Returns_String The name of the report view for the entity..
        
        public string ReportViewName;
        //
        // Summary:
        //     Gets or sets the schema name for the entity.
        //
        // Returns:
        //     Type: Returns_String The schema name for the entity.
        
        public string SchemaName;
    }
}
