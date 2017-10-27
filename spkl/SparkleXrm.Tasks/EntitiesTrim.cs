﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace SparkleXrm.Tasks
{
    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum WebResourceWebResourceType
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Webpage_HTML = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        StyleSheet_CSS = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Script_JScript = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Data_XML = 4,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PNGformat = 5,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        JPGformat = 6,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        GIFformat = 7,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Silverlight_XAP = 8,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        StyleSheet_XSL = 9,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ICOformat = 10,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SVGFormat = 11,
    }

    /// <summary>
    /// A solution which contains CRM customizations.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("solution")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class Solution : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Solution() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "solution";

        public const int EntityTypeCode = 7100;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// A link to an optional configuration page for this solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("configurationpageid")]
        public Microsoft.Xrm.Sdk.EntityReference ConfigurationPageId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("configurationpageid");
            }
            set
            {
                this.OnPropertyChanging("ConfigurationPageId");
                this.SetAttributeValue("configurationpageid", value);
                this.OnPropertyChanged("ConfigurationPageId");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the solution was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Description of the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// User display name for the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("friendlyname")]
        public string FriendlyName
        {
            get
            {
                return this.GetAttributeValue<string>("friendlyname");
            }
            set
            {
                this.OnPropertyChanging("FriendlyName");
                this.SetAttributeValue("friendlyname", value);
                this.OnPropertyChanged("FriendlyName");
            }
        }

        /// <summary>
        /// Date and time when the solution was installed/upgraded.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("installedon")]
        public System.Nullable<System.DateTime> InstalledOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("installedon");
            }
        }

        /// <summary>
        /// Indicates whether the solution is managed or unmanaged.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Indicates whether the solution is visible outside of the platform.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isvisible")]
        public System.Nullable<bool> IsVisible
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isvisible");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the solution was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who modified the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Unique identifier of the organization associated with the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Unique identifier of the parent solution. Should only be non-null if this solution is a patch.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentsolutionid")]
        public Microsoft.Xrm.Sdk.EntityReference ParentSolutionId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("parentsolutionid");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pinpointassetid")]
        public string PinpointAssetId
        {
            get
            {
                return this.GetAttributeValue<string>("pinpointassetid");
            }
        }

        /// <summary>
        /// Identifier of the publisher of this solution in Microsoft Pinpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pinpointpublisherid")]
        public System.Nullable<long> PinpointPublisherId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("pinpointpublisherid");
            }
        }

        /// <summary>
        /// Default locale of the solution in Microsoft Pinpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pinpointsolutiondefaultlocale")]
        public string PinpointSolutionDefaultLocale
        {
            get
            {
                return this.GetAttributeValue<string>("pinpointsolutiondefaultlocale");
            }
        }

        /// <summary>
        /// Identifier of the solution in Microsoft Pinpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pinpointsolutionid")]
        public System.Nullable<long> PinpointSolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("pinpointsolutionid");
            }
        }

        /// <summary>
        /// Unique identifier of the publisher.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("publisherid")]
        public Microsoft.Xrm.Sdk.EntityReference PublisherId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("publisherid");
            }
            set
            {
                this.OnPropertyChanging("PublisherId");
                this.SetAttributeValue("publisherid", value);
                this.OnPropertyChanged("PublisherId");
            }
        }

        /// <summary>
        /// Unique identifier of the solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
            set
            {
                this.OnPropertyChanging("SolutionId");
                this.SetAttributeValue("solutionid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SolutionId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SolutionId = value;
            }
        }

        /// <summary>
        /// Solution package source organization version
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionpackageversion")]
        public string SolutionPackageVersion
        {
            get
            {
                return this.GetAttributeValue<string>("solutionpackageversion");
            }
            set
            {
                this.OnPropertyChanging("SolutionPackageVersion");
                this.SetAttributeValue("solutionpackageversion", value);
                this.OnPropertyChanged("SolutionPackageVersion");
            }
        }

        /// <summary>
        /// The unique name of this solution
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("uniquename")]
        public string UniqueName
        {
            get
            {
                return this.GetAttributeValue<string>("uniquename");
            }
            set
            {
                this.OnPropertyChanging("UniqueName");
                this.SetAttributeValue("uniquename", value);
                this.OnPropertyChanged("UniqueName");
            }
        }

        /// <summary>
        /// Solution version, used to identify a solution for upgrades and hotfixes.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("version")]
        public string Version
        {
            get
            {
                return this.GetAttributeValue<string>("version");
            }
            set
            {
                this.OnPropertyChanging("Version");
                this.SetAttributeValue("version", value);
                this.OnPropertyChanged("Version");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

       
    }
    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum componenttype
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attribute = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Relationship = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AttributePicklistValue = 4,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AttributeLookupValue = 5,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ViewAttribute = 6,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        LocalizedLabel = 7,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RelationshipExtraCondition = 8,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        OptionSet = 9,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityRelationship = 10,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityRelationshipRole = 11,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityRelationshipRelationships = 12,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ManagedProperty = 13,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityKey = 14,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Role = 20,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RolePrivilege = 21,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DisplayString = 22,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DisplayStringMap = 23,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Form = 24,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Organization = 25,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SavedQuery = 26,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Workflow = 29,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Report = 31,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReportEntity = 32,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReportCategory = 33,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReportVisibility = 34,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attachment = 35,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EmailTemplate = 36,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ContractTemplate = 37,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        KBArticleTemplate = 38,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        MailMergeTemplate = 39,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DuplicateRule = 44,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DuplicateRuleCondition = 45,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityMap = 46,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AttributeMap = 47,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonCommand = 48,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonContextGroup = 49,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonCustomization = 50,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonRule = 52,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonTabToCommandMap = 53,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonDiff = 55,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SavedQueryVisualization = 59,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SystemForm = 60,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebResource = 61,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SiteMap = 62,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConnectionRole = 63,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        FieldSecurityProfile = 70,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        FieldPermission = 71,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PluginType = 90,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PluginAssembly = 91,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SDKMessageProcessingStep = 92,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SDKMessageProcessingStepImage = 93,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ServiceEndpoint = 95,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RoutingRule = 150,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RoutingRuleItem = 151,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLA = 152,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLAItem = 153,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConvertRule = 154,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConvertRuleItem = 155,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        HierarchyRule = 65,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        MobileOfflineProfile = 161,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        MobileOfflineProfileItem = 162,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SimilarityRule = 165,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        CustomControl = 66,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        CustomControlDefaultConfig = 68,
    }

    /// <summary>
    /// Assembly that contains one or more plug-in types.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("pluginassembly")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class PluginAssembly : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PluginAssembly() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "pluginassembly";

        public const int EntityTypeCode = 4605;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Bytes of the assembly, in Base64 format.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("content")]
        public string Content
        {
            get
            {
                return this.GetAttributeValue<string>("content");
            }
            set
            {
                this.OnPropertyChanging("Content");
                this.SetAttributeValue("content", value);
                this.OnPropertyChanged("Content");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the plug-in assembly was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the pluginassembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Culture code for the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("culture")]
        public string Culture
        {
            get
            {
                return this.GetAttributeValue<string>("culture");
            }
            set
            {
                this.OnPropertyChanging("Culture");
                this.SetAttributeValue("culture", value);
                this.OnPropertyChanged("Culture");
            }
        }

        /// <summary>
        /// Customization Level.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Description of the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Version in which the form is introduced.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Information that specifies whether this component should be hidden.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ishidden")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsHidden
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("ishidden");
            }
            set
            {
                this.OnPropertyChanging("IsHidden");
                this.SetAttributeValue("ishidden", value);
                this.OnPropertyChanged("IsHidden");
            }
        }

        /// <summary>
        /// Information that specifies whether this component is managed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Information about how the plugin assembly is to be isolated at execution time; None / Sandboxed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isolationmode")]
        public Microsoft.Xrm.Sdk.OptionSetValue IsolationMode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("isolationmode");
            }
            set
            {
                this.OnPropertyChanging("IsolationMode");
                this.SetAttributeValue("isolationmode", value);
                this.OnPropertyChanged("IsolationMode");
            }
        }

        /// <summary>
        /// Major of the assembly version.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("major")]
        public System.Nullable<int> Major
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("major");
            }
        }

        /// <summary>
        /// Minor of the assembly version.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("minor")]
        public System.Nullable<int> Minor
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("minor");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the plug-in assembly was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the pluginassembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the plug-in assembly is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// File name of the plug-in assembly. Used when the source type is set to 1.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("path")]
        public string Path
        {
            get
            {
                return this.GetAttributeValue<string>("path");
            }
            set
            {
                this.OnPropertyChanging("Path");
                this.SetAttributeValue("path", value);
                this.OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pluginassemblyid")]
        public System.Nullable<System.Guid> PluginAssemblyId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("pluginassemblyid");
            }
            set
            {
                this.OnPropertyChanging("PluginAssemblyId");
                this.SetAttributeValue("pluginassemblyid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("PluginAssemblyId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pluginassemblyid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.PluginAssemblyId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pluginassemblyidunique")]
        public System.Nullable<System.Guid> PluginAssemblyIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("pluginassemblyidunique");
            }
        }

        /// <summary>
        /// Public key token of the assembly. This value can be obtained from the assembly by using reflection.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("publickeytoken")]
        public string PublicKeyToken
        {
            get
            {
                return this.GetAttributeValue<string>("publickeytoken");
            }
            set
            {
                this.OnPropertyChanging("PublicKeyToken");
                this.SetAttributeValue("publickeytoken", value);
                this.OnPropertyChanged("PublicKeyToken");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Hash of the source of the assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sourcehash")]
        public string SourceHash
        {
            get
            {
                return this.GetAttributeValue<string>("sourcehash");
            }
            set
            {
                this.OnPropertyChanging("SourceHash");
                this.SetAttributeValue("sourcehash", value);
                this.OnPropertyChanged("SourceHash");
            }
        }

        /// <summary>
        /// Location of the assembly, for example 0=database, 1=on-disk.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sourcetype")]
        public Microsoft.Xrm.Sdk.OptionSetValue SourceType
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("sourcetype");
            }
            set
            {
                this.OnPropertyChanging("SourceType");
                this.SetAttributeValue("sourcetype", value);
                this.OnPropertyChanged("SourceType");
            }
        }

        /// <summary>
        /// Version number of the assembly. The value can be obtained from the assembly through reflection.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("version")]
        public string Version
        {
            get
            {
                return this.GetAttributeValue<string>("version");
            }
            set
            {
                this.OnPropertyChanging("Version");
                this.SetAttributeValue("version", value);
                this.OnPropertyChanged("Version");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// 1:N pluginassembly_plugintype
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("pluginassembly_plugintype")]
        public System.Collections.Generic.IEnumerable<PluginType> pluginassembly_plugintype
        {
            get
            {
                return this.GetRelatedEntities<PluginType>("pluginassembly_plugintype", null);
            }
            set
            {
                this.OnPropertyChanging("pluginassembly_plugintype");
                this.SetRelatedEntities<PluginType>("pluginassembly_plugintype", null, value);
                this.OnPropertyChanged("pluginassembly_plugintype");
            }
        }

      
    }

   
    /// <summary>
    /// Type that inherits from the IPlugin interface and is contained within a plug-in assembly.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("plugintype")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class PluginType : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PluginType() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "plugintype";

        public const int EntityTypeCode = 4602;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Full path name of the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("assemblyname")]
        public string AssemblyName
        {
            get
            {
                return this.GetAttributeValue<string>("assemblyname");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the plug-in type was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the plugintype.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Culture code for the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("culture")]
        public string Culture
        {
            get
            {
                return this.GetAttributeValue<string>("culture");
            }
        }

        /// <summary>
        /// Customization level of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Serialized Custom Activity Type information, including required arguments. For more information, see SandboxCustomActivityInfo.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customworkflowactivityinfo")]
        public string CustomWorkflowActivityInfo
        {
            get
            {
                return this.GetAttributeValue<string>("customworkflowactivityinfo");
            }
        }

        /// <summary>
        /// Description of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// User friendly name for the plug-in.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("friendlyname")]
        public string FriendlyName
        {
            get
            {
                return this.GetAttributeValue<string>("friendlyname");
            }
            set
            {
                this.OnPropertyChanging("FriendlyName");
                this.SetAttributeValue("friendlyname", value);
                this.OnPropertyChanged("FriendlyName");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Indicates if the plug-in is a custom activity for workflows.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isworkflowactivity")]
        public System.Nullable<bool> IsWorkflowActivity
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isworkflowactivity");
            }
        }

        /// <summary>
        /// Major of the version number of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("major")]
        public System.Nullable<int> Major
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("major");
            }
        }

        /// <summary>
        /// Minor of the version number of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("minor")]
        public System.Nullable<int> Minor
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("minor");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the plug-in type was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the plugintype.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the plug-in type is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in assembly that contains this plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pluginassemblyid")]
        public Microsoft.Xrm.Sdk.EntityReference PluginAssemblyId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("pluginassemblyid");
            }
            set
            {
                this.OnPropertyChanging("PluginAssemblyId");
                this.SetAttributeValue("pluginassemblyid", value);
                this.OnPropertyChanged("PluginAssemblyId");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        public System.Nullable<System.Guid> PluginTypeId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("plugintypeid");
            }
            set
            {
                this.OnPropertyChanging("PluginTypeId");
                this.SetAttributeValue("plugintypeid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("PluginTypeId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.PluginTypeId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeidunique")]
        public System.Nullable<System.Guid> PluginTypeIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("plugintypeidunique");
            }
        }

        /// <summary>
        /// Public key token of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("publickeytoken")]
        public string PublicKeyToken
        {
            get
            {
                return this.GetAttributeValue<string>("publickeytoken");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Fully qualified type name of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("typename")]
        public string TypeName
        {
            get
            {
                return this.GetAttributeValue<string>("typename");
            }
            set
            {
                this.OnPropertyChanging("TypeName");
                this.SetAttributeValue("typename", value);
                this.OnPropertyChanged("TypeName");
            }
        }

        /// <summary>
        /// Version number of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("version")]
        public string Version
        {
            get
            {
                return this.GetAttributeValue<string>("version");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Group name of workflow custom activity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("workflowactivitygroupname")]
        public string WorkflowActivityGroupName
        {
            get
            {
                return this.GetAttributeValue<string>("workflowactivitygroupname");
            }
            set
            {
                this.OnPropertyChanging("WorkflowActivityGroupName");
                this.SetAttributeValue("workflowactivitygroupname", value);
                this.OnPropertyChanged("WorkflowActivityGroupName");
            }
        }



        /// <summary>
        /// 1:N plugintype_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintype_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> plugintype_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("plugintype_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintype_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("plugintype_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintype_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// 1:N plugintypeid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintypeid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> plugintypeid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("plugintypeid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintypeid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("plugintypeid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintypeid_sdkmessageprocessingstep");
            }
        }


             
        /// <summary>
        /// N:1 pluginassembly_plugintype
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pluginassemblyid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("pluginassembly_plugintype")]
        public PluginAssembly pluginassembly_plugintype
        {
            get
            {
                return this.GetRelatedEntity<PluginAssembly>("pluginassembly_plugintype", null);
            }
            set
            {
                this.OnPropertyChanging("pluginassembly_plugintype");
                this.SetRelatedEntity<PluginAssembly>("pluginassembly_plugintype", null, value);
                this.OnPropertyChanged("pluginassembly_plugintype");
            }
        }
    }

    /// <summary>
    /// Message that is supported by the SDK.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessage")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class SdkMessage : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessage() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessage";

        public const int EntityTypeCode = 4606;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Information about whether the SDK message is automatically transacted.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("autotransact")]
        public System.Nullable<bool> AutoTransact
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("autotransact");
            }
            set
            {
                this.OnPropertyChanging("AutoTransact");
                this.SetAttributeValue("autotransact", value);
                this.OnPropertyChanged("AutoTransact");
            }
        }

        /// <summary>
        /// Identifies where a method will be exposed. 0 - Server, 1 - Client, 2 - both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("availability")]
        public System.Nullable<int> Availability
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("availability");
            }
            set
            {
                this.OnPropertyChanging("Availability");
                this.SetAttributeValue("availability", value);
                this.OnPropertyChanged("Availability");
            }
        }

        /// <summary>
        /// If this is a categorized method, this is the name, otherwise None.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("categoryname")]
        public string CategoryName
        {
            get
            {
                return this.GetAttributeValue<string>("categoryname");
            }
            set
            {
                this.OnPropertyChanging("CategoryName");
                this.SetAttributeValue("categoryname", value);
                this.OnPropertyChanged("CategoryName");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Indicates whether the SDK message should have its requests expanded per primary entity defined in its filters.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("expand")]
        public System.Nullable<bool> Expand
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("expand");
            }
            set
            {
                this.OnPropertyChanging("Expand");
                this.SetAttributeValue("expand", value);
                this.OnPropertyChanged("Expand");
            }
        }

        /// <summary>
        /// Information about whether the SDK message is active.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isactive")]
        public System.Nullable<bool> IsActive
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isactive");
            }
            set
            {
                this.OnPropertyChanging("IsActive");
                this.SetAttributeValue("isactive", value);
                this.OnPropertyChanged("IsActive");
            }
        }

        /// <summary>
        /// Indicates whether the SDK message is private.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isprivate")]
        public System.Nullable<bool> IsPrivate
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isprivate");
            }
            set
            {
                this.OnPropertyChanging("IsPrivate");
                this.SetAttributeValue("isprivate", value);
                this.OnPropertyChanged("IsPrivate");
            }
        }

        /// <summary>
        /// Identifies whether an SDK message will be ReadOnly or Read Write. false - ReadWrite, true - ReadOnly .
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isreadonly")]
        public System.Nullable<bool> IsReadOnly
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isreadonly");
            }
            set
            {
                this.OnPropertyChanging("IsReadOnly");
                this.SetAttributeValue("isreadonly", value);
                this.OnPropertyChanged("IsReadOnly");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isvalidforexecuteasync")]
        public System.Nullable<bool> IsValidForExecuteAsync
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isvalidforexecuteasync");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public System.Nullable<System.Guid> SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageId");
                this.SetAttributeValue("sdkmessageid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageidunique")]
        public System.Nullable<System.Guid> SdkMessageIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageidunique");
            }
        }

        /// <summary>
        /// Indicates whether the SDK message is a template.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("template")]
        public System.Nullable<bool> Template
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("template");
            }
            set
            {
                this.OnPropertyChanging("Template");
                this.SetAttributeValue("template", value);
                this.OnPropertyChanged("Template");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("throttlesettings")]
        public string ThrottleSettings
        {
            get
            {
                return this.GetAttributeValue<string>("throttlesettings");
            }
        }

        /// <summary>
        /// Number that identifies a specific revision of the SDK message. 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Whether or not the SDK message can be called from a workflow.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("workflowsdkstepenabled")]
        public System.Nullable<bool> WorkflowSdkStepEnabled
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("workflowsdkstepenabled");
            }
        }

        /// <summary>
        /// 1:N message_sdkmessagepair
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("message_sdkmessagepair")]
        public System.Collections.Generic.IEnumerable<SdkMessagePair> message_sdkmessagepair
        {
            get
            {
                return this.GetRelatedEntities<SdkMessagePair>("message_sdkmessagepair", null);
            }
            set
            {
                this.OnPropertyChanging("message_sdkmessagepair");
                this.SetRelatedEntities<SdkMessagePair>("message_sdkmessagepair", null, value);
                this.OnPropertyChanged("message_sdkmessagepair");
            }
        }

        /// <summary>
        /// 1:N sdkmessageid_sdkmessagefilter
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessagefilter")]
        public System.Collections.Generic.IEnumerable<SdkMessageFilter> sdkmessageid_sdkmessagefilter
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageFilter>("sdkmessageid_sdkmessagefilter", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessagefilter");
                this.SetRelatedEntities<SdkMessageFilter>("sdkmessageid_sdkmessagefilter", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessagefilter");
            }
        }

        /// <summary>
        /// 1:N sdkmessageid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> sdkmessageid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("sdkmessageid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("sdkmessageid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessageprocessingstep");
            }
        }

      

    }

    /// <summary>
    /// Filter that defines which SDK messages are valid for each type of entity.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessagefilter")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class SdkMessageFilter : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessageFilter() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessagefilter";

        public const int EntityTypeCode = 4607;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Identifies where a method will be exposed. 0 - Server, 1 - Client, 2 - both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("availability")]
        public System.Nullable<int> Availability
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("availability");
            }
            set
            {
                this.OnPropertyChanging("Availability");
                this.SetAttributeValue("availability", value);
                this.OnPropertyChanged("Availability");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message filter was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessagefilter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Indicates whether a custom SDK message processing step is allowed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomprocessingstepallowed")]
        public System.Nullable<bool> IsCustomProcessingStepAllowed
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("iscustomprocessingstepallowed");
            }
            set
            {
                this.OnPropertyChanging("IsCustomProcessingStepAllowed");
                this.SetAttributeValue("iscustomprocessingstepallowed", value);
                this.OnPropertyChanged("IsCustomProcessingStepAllowed");
            }
        }

        /// <summary>
        /// Indicates whether the filter should be visible.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isvisible")]
        public System.Nullable<bool> IsVisible
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isvisible");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message filter was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessagefilter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message filter is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Type of entity with which the SDK message filter is primarily associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("primaryobjecttypecode")]
        public string PrimaryObjectTypeCode
        {
            get
            {
                return this.GetAttributeValue<string>("primaryobjecttypecode");
            }
            set
            {
                this.OnPropertyChanging("PrimaryObjectTypeCode");
                this.SetAttributeValue("primaryobjecttypecode", value);
                this.OnPropertyChanged("PrimaryObjectTypeCode");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message filter entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        public System.Nullable<System.Guid> SdkMessageFilterId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessagefilterid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageFilterId");
                this.SetAttributeValue("sdkmessagefilterid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageFilterId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageFilterId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilteridunique")]
        public System.Nullable<System.Guid> SdkMessageFilterIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessagefilteridunique");
            }
        }

        /// <summary>
        /// Unique identifier of the related SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageId");
                this.SetAttributeValue("sdkmessageid", value);
                this.OnPropertyChanged("SdkMessageId");
            }
        }

        /// <summary>
        /// Type of entity with which the SDK message filter is secondarily associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("secondaryobjecttypecode")]
        public string SecondaryObjectTypeCode
        {
            get
            {
                return this.GetAttributeValue<string>("secondaryobjecttypecode");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Whether or not the SDK message can be called from a workflow.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("workflowsdkstepenabled")]
        public System.Nullable<bool> WorkflowSdkStepEnabled
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("workflowsdkstepenabled");
            }
        }

        /// <summary>
        /// 1:N sdkmessagefilterid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessagefilterid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> sdkmessagefilterid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("sdkmessagefilterid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessagefilterid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("sdkmessagefilterid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessagefilterid_sdkmessageprocessingstep");
            }
        }

       

        /// <summary>
        /// N:1 sdkmessageid_sdkmessagefilter
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessagefilter")]
        public SdkMessage sdkmessageid_sdkmessagefilter
        {
            get
            {
                return this.GetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessagefilter", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessagefilter");
                this.SetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessagefilter", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessagefilter");
            }
        }
    }

    /// <summary>
    /// For internal use only.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessagepair")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class SdkMessagePair : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessagePair() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessagepair";

        public const int EntityTypeCode = 4613;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message pair.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message pair was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessagepair.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Endpoint that the message pair is associated with.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("endpoint")]
        public string Endpoint
        {
            get
            {
                return this.GetAttributeValue<string>("endpoint");
            }
            set
            {
                this.OnPropertyChanging("Endpoint");
                this.SetAttributeValue("endpoint", value);
                this.OnPropertyChanged("Endpoint");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message pair.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message pair was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessagepair.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Namespace that the message pair is associated with.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("namespace")]
        public string Namespace
        {
            get
            {
                return this.GetAttributeValue<string>("namespace");
            }
            set
            {
                this.OnPropertyChanging("Namespace");
                this.SetAttributeValue("namespace", value);
                this.OnPropertyChanged("Namespace");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message pair is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagebindinginformation")]
        public string SdkMessageBindingInformation
        {
            get
            {
                return this.GetAttributeValue<string>("sdkmessagebindinginformation");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageBindingInformation");
                this.SetAttributeValue("sdkmessagebindinginformation", value);
                this.OnPropertyChanged("SdkMessageBindingInformation");
            }
        }

        /// <summary>
        /// Unique identifier of the message with which the SDK message pair is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageid");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message pair entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagepairid")]
        public System.Nullable<System.Guid> SdkMessagePairId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessagepairid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessagePairId");
                this.SetAttributeValue("sdkmessagepairid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessagePairId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagepairid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessagePairId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message pair.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagepairidunique")]
        public System.Nullable<System.Guid> SdkMessagePairIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessagepairidunique");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }


       


        /// <summary>
        /// N:1 message_sdkmessagepair
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("message_sdkmessagepair")]
        public SdkMessage message_sdkmessagepair
        {
            get
            {
                return this.GetRelatedEntity<SdkMessage>("message_sdkmessagepair", null);
            }
        }

     
    }

    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum SdkMessageProcessingStepState
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Enabled = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Disabled = 1,
    }

    /// <summary>
    /// Stage in the execution pipeline that a plug-in is to execute.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessageprocessingstep")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class SdkMessageProcessingStep : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessageProcessingStep() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessageprocessingstep";

        public const int EntityTypeCode = 4608;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Indicates whether the asynchronous system job is automatically deleted on completion.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("asyncautodelete")]
        public System.Nullable<bool> AsyncAutoDelete
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("asyncautodelete");
            }
            set
            {
                this.OnPropertyChanging("AsyncAutoDelete");
                this.SetAttributeValue("asyncautodelete", value);
                this.OnPropertyChanged("AsyncAutoDelete");
            }
        }

        /// <summary>
        /// Identifies whether a SDK Message Processing Step type will be ReadOnly or Read Write. false - ReadWrite, true - ReadOnly 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canusereadonlyconnection")]
        public System.Nullable<bool> CanUseReadOnlyConnection
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("canusereadonlyconnection");
            }
            set
            {
                this.OnPropertyChanging("CanUseReadOnlyConnection");
                this.SetAttributeValue("canusereadonlyconnection", value);
                this.OnPropertyChanged("CanUseReadOnlyConnection");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Step-specific configuration for the plug-in type. Passed to the plug-in constructor at run time.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("configuration")]
        public string Configuration
        {
            get
            {
                return this.GetAttributeValue<string>("configuration");
            }
            set
            {
                this.OnPropertyChanging("Configuration");
                this.SetAttributeValue("configuration", value);
                this.OnPropertyChanged("Configuration");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessageprocessingstep.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Description of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Unique identifier of the associated event handler.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
        public Microsoft.Xrm.Sdk.EntityReference EventHandler
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("eventhandler");
            }
            set
            {
                this.OnPropertyChanging("EventHandler");
                this.SetAttributeValue("eventhandler", value);
                this.OnPropertyChanged("EventHandler");
            }
        }

        /// <summary>
        /// Comma-separated list of attributes. If at least one of these attributes is modified, the plug-in should execute.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filteringattributes")]
        public string FilteringAttributes
        {
            get
            {
                return this.GetAttributeValue<string>("filteringattributes");
            }
            set
            {
                this.OnPropertyChanging("FilteringAttributes");
                this.SetAttributeValue("filteringattributes", value);
                this.OnPropertyChanged("FilteringAttributes");
            }
        }

        /// <summary>
        /// Unique identifier of the user to impersonate context when step is executed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("impersonatinguserid")]
        public Microsoft.Xrm.Sdk.EntityReference ImpersonatingUserId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("impersonatinguserid");
            }
            set
            {
                this.OnPropertyChanging("ImpersonatingUserId");
                this.SetAttributeValue("impersonatinguserid", value);
                this.OnPropertyChanged("ImpersonatingUserId");
            }
        }

        /// <summary>
        /// Version in which the form is introduced.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Identifies if a plug-in should be executed from a parent pipeline, a child pipeline, or both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invocationsource")]
        [System.ObsoleteAttribute()]
        public Microsoft.Xrm.Sdk.OptionSetValue InvocationSource
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invocationsource");
            }
            set
            {
                this.OnPropertyChanging("InvocationSource");
                this.SetAttributeValue("invocationsource", value);
                this.OnPropertyChanged("InvocationSource");
            }
        }

        /// <summary>
        /// Information that specifies whether this component can be customized.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
            }
            set
            {
                this.OnPropertyChanging("IsCustomizable");
                this.SetAttributeValue("iscustomizable", value);
                this.OnPropertyChanged("IsCustomizable");
            }
        }

        /// <summary>
        /// Information that specifies whether this component should be hidden.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ishidden")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsHidden
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("ishidden");
            }
            set
            {
                this.OnPropertyChanging("IsHidden");
                this.SetAttributeValue("ishidden", value);
                this.OnPropertyChanged("IsHidden");
            }
        }

        /// <summary>
        /// Information that specifies whether this component is managed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Run-time mode of execution, for example, synchronous or asynchronous.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mode")]
        public Microsoft.Xrm.Sdk.OptionSetValue Mode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("mode");
            }
            set
            {
                this.OnPropertyChanging("Mode");
                this.SetAttributeValue("mode", value);
                this.OnPropertyChanged("Mode");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessageprocessingstep.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of SdkMessage processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message processing step is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in type associated with the step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        public Microsoft.Xrm.Sdk.EntityReference PluginTypeId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("plugintypeid");
            }
            set
            {
                this.OnPropertyChanging("PluginTypeId");
                this.SetAttributeValue("plugintypeid", value);
                this.OnPropertyChanged("PluginTypeId");
            }
        }

        /// <summary>
        /// Processing order within the stage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("rank")]
        public System.Nullable<int> Rank
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("rank");
            }
            set
            {
                this.OnPropertyChanging("Rank");
                this.SetAttributeValue("rank", value);
                this.OnPropertyChanged("Rank");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageFilterId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessagefilterid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageFilterId");
                this.SetAttributeValue("sdkmessagefilterid", value);
                this.OnPropertyChanged("SdkMessageFilterId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageId");
                this.SetAttributeValue("sdkmessageid", value);
                this.OnPropertyChanged("SdkMessageId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepId");
                this.SetAttributeValue("sdkmessageprocessingstepid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageProcessingStepId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageProcessingStepId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepidunique")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepidunique");
            }
        }

        /// <summary>
        /// Unique identifier of the Sdk message processing step secure configuration.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageProcessingStepSecureConfigId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageprocessingstepsecureconfigid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepSecureConfigId");
                this.SetAttributeValue("sdkmessageprocessingstepsecureconfigid", value);
                this.OnPropertyChanged("SdkMessageProcessingStepSecureConfigId");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Stage in the execution pipeline that the SDK message processing step is in.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stage")]
        public Microsoft.Xrm.Sdk.OptionSetValue Stage
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("stage");
            }
            set
            {
                this.OnPropertyChanging("Stage");
                this.SetAttributeValue("stage", value);
                this.OnPropertyChanged("Stage");
            }
        }

        /// <summary>
        /// Status of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
        public System.Nullable<SdkMessageProcessingStepState> StateCode
        {
            get
            {
                Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
                if ((optionSet != null))
                {
                    return ((SdkMessageProcessingStepState)(System.Enum.ToObject(typeof(SdkMessageProcessingStepState), optionSet.Value)));
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.OnPropertyChanging("StateCode");
                if ((value == null))
                {
                    this.SetAttributeValue("statecode", null);
                }
                else
                {
                    this.SetAttributeValue("statecode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
                }
                this.OnPropertyChanged("StateCode");
            }
        }

        /// <summary>
        /// Reason for the status of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
        public Microsoft.Xrm.Sdk.OptionSetValue StatusCode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
            }
            set
            {
                this.OnPropertyChanging("StatusCode");
                this.SetAttributeValue("statuscode", value);
                this.OnPropertyChanged("StatusCode");
            }
        }

        /// <summary>
        /// Deployment that the SDK message processing step should be executed on; server, client, or both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("supporteddeployment")]
        public Microsoft.Xrm.Sdk.OptionSetValue SupportedDeployment
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("supporteddeployment");
            }
            set
            {
                this.OnPropertyChanging("SupportedDeployment");
                this.SetAttributeValue("supporteddeployment", value);
                this.OnPropertyChanged("SupportedDeployment");
            }
        }

        /// <summary>
        /// Number that identifies a specific revision of the SDK message processing step. 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

      

        /// <summary>
        /// 1:N sdkmessageprocessingstepid_sdkmessageprocessingstepimage
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageprocessingstepid_sdkmessageprocessingstepimage")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStepImage> sdkmessageprocessingstepid_sdkmessageprocessingstepimage
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStepImage>("sdkmessageprocessingstepid_sdkmessageprocessingstepimage", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageprocessingstepid_sdkmessageprocessingstepimage");
                this.SetRelatedEntities<SdkMessageProcessingStepImage>("sdkmessageprocessingstepid_sdkmessageprocessingstepimage", null, value);
                this.OnPropertyChanged("sdkmessageprocessingstepid_sdkmessageprocessingstepimage");
            }
        }

       

        /// <summary>
        /// N:1 plugintype_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintype_sdkmessageprocessingstep")]
        public PluginType plugintype_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<PluginType>("plugintype_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintype_sdkmessageprocessingstep");
                this.SetRelatedEntity<PluginType>("plugintype_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintype_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 plugintypeid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintypeid_sdkmessageprocessingstep")]
        public PluginType plugintypeid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<PluginType>("plugintypeid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintypeid_sdkmessageprocessingstep");
                this.SetRelatedEntity<PluginType>("plugintypeid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintypeid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 sdkmessagefilterid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessagefilterid_sdkmessageprocessingstep")]
        public SdkMessageFilter sdkmessagefilterid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<SdkMessageFilter>("sdkmessagefilterid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessagefilterid_sdkmessageprocessingstep");
                this.SetRelatedEntity<SdkMessageFilter>("sdkmessagefilterid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessagefilterid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 sdkmessageid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessageprocessingstep")]
        public SdkMessage sdkmessageid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessageprocessingstep");
                this.SetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep")]
        public SdkMessageProcessingStepSecureConfig sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<SdkMessageProcessingStepSecureConfig>("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep");
                this.SetRelatedEntity<SdkMessageProcessingStepSecureConfig>("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 serviceendpoint_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("serviceendpoint_sdkmessageprocessingstep")]
        public ServiceEndpoint serviceendpoint_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<ServiceEndpoint>("serviceendpoint_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("serviceendpoint_sdkmessageprocessingstep");
                this.SetRelatedEntity<ServiceEndpoint>("serviceendpoint_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("serviceendpoint_sdkmessageprocessingstep");
            }
        }
    }

    /// <summary>
    /// Copy of an entity's attributes before or after the core system operation.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessageprocessingstepimage")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class SdkMessageProcessingStepImage : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessageProcessingStepImage() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessageprocessingstepimage";

        public const int EntityTypeCode = 4615;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Comma-separated list of attributes that are to be passed into the SDK message processing step image.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("attributes")]
        public string Attributes1
        {
            get
            {
                return this.GetAttributeValue<string>("attributes");
            }
            set
            {
                this.OnPropertyChanging("Attributes1");
                this.SetAttributeValue("attributes", value);
                this.OnPropertyChanged("Attributes1");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message processing step image.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step image was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessageprocessingstepimage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message processing step image.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Description of the SDK message processing step image.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Key name used to access the pre-image or post-image property bags in a step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entityalias")]
        public string EntityAlias
        {
            get
            {
                return this.GetAttributeValue<string>("entityalias");
            }
            set
            {
                this.OnPropertyChanging("EntityAlias");
                this.SetAttributeValue("entityalias", value);
                this.OnPropertyChanged("EntityAlias");
            }
        }

        /// <summary>
        /// Type of image requested.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("imagetype")]
        public Microsoft.Xrm.Sdk.OptionSetValue ImageType
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("imagetype");
            }
            set
            {
                this.OnPropertyChanging("ImageType");
                this.SetAttributeValue("imagetype", value);
                this.OnPropertyChanged("ImageType");
            }
        }

        /// <summary>
        /// Version in which the form is introduced.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Information that specifies whether this component can be customized.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
            }
            set
            {
                this.OnPropertyChanging("IsCustomizable");
                this.SetAttributeValue("iscustomizable", value);
                this.OnPropertyChanged("IsCustomizable");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Name of the property on the Request message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("messagepropertyname")]
        public string MessagePropertyName
        {
            get
            {
                return this.GetAttributeValue<string>("messagepropertyname");
            }
            set
            {
                this.OnPropertyChanging("MessagePropertyName");
                this.SetAttributeValue("messagepropertyname", value);
                this.OnPropertyChanged("MessagePropertyName");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessageprocessingstepimage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of SdkMessage processing step image.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message processing step is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Name of the related entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relatedattributename")]
        public string RelatedAttributeName
        {
            get
            {
                return this.GetAttributeValue<string>("relatedattributename");
            }
            set
            {
                this.OnPropertyChanging("RelatedAttributeName");
                this.SetAttributeValue("relatedattributename", value);
                this.OnPropertyChanged("RelatedAttributeName");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageProcessingStepId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageprocessingstepid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepId");
                this.SetAttributeValue("sdkmessageprocessingstepid", value);
                this.OnPropertyChanged("SdkMessageProcessingStepId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step image entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepimageid")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepImageId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepimageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepImageId");
                this.SetAttributeValue("sdkmessageprocessingstepimageid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageProcessingStepImageId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepimageid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageProcessingStepImageId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step image.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepimageidunique")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepImageIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepimageidunique");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Number that identifies a specific revision of the step image. 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }


        /// <summary>
        /// N:1 sdkmessageprocessingstepid_sdkmessageprocessingstepimage
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageprocessingstepid_sdkmessageprocessingstepimage")]
        public SdkMessageProcessingStep sdkmessageprocessingstepid_sdkmessageprocessingstepimage
        {
            get
            {
                return this.GetRelatedEntity<SdkMessageProcessingStep>("sdkmessageprocessingstepid_sdkmessageprocessingstepimage", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageprocessingstepid_sdkmessageprocessingstepimage");
                this.SetRelatedEntity<SdkMessageProcessingStep>("sdkmessageprocessingstepid_sdkmessageprocessingstepimage", null, value);
                this.OnPropertyChanged("sdkmessageprocessingstepid_sdkmessageprocessingstepimage");
            }
        }
    }

    /// <summary>
    /// Non-public custom configuration that is passed to a plug-in's constructor.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessageprocessingstepsecureconfig")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class SdkMessageProcessingStepSecureConfig : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessageProcessingStepSecureConfig() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessageprocessingstepsecureconfig";

        public const int EntityTypeCode = 4616;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessageprocessingstepsecureconfig.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message processing step secure configuration.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessageprocessingstepsecureconfig.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message processing step is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step secure configuration.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepSecureConfigId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepsecureconfigid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepSecureConfigId");
                this.SetAttributeValue("sdkmessageprocessingstepsecureconfigid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageProcessingStepSecureConfigId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageProcessingStepSecureConfigId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigidunique")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepSecureConfigIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepsecureconfigidunique");
            }
        }

        /// <summary>
        /// Secure step-specific configuration for the plug-in type that is passed to the plug-in's constructor at run time.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("secureconfig")]
        public string SecureConfig
        {
            get
            {
                return this.GetAttributeValue<string>("secureconfig");
            }
            set
            {
                this.OnPropertyChanging("SecureConfig");
                this.SetAttributeValue("secureconfig", value);
                this.OnPropertyChanged("SecureConfig");
            }
        }

        /// <summary>
        /// 1:N sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep");
            }
        }  
    }

    /// <summary>
    /// Service endpoint that can be contacted.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("serviceendpoint")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class ServiceEndpoint : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ServiceEndpoint() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "serviceendpoint";

        public const int EntityTypeCode = 4618;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Specifies mode of authentication with SB
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("authtype")]
        public Microsoft.Xrm.Sdk.OptionSetValue AuthType
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("authtype");
            }
            set
            {
                this.OnPropertyChanging("AuthType");
                this.SetAttributeValue("authtype", value);
                this.OnPropertyChanged("AuthType");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Connection mode to contact the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionmode")]
        public Microsoft.Xrm.Sdk.OptionSetValue ConnectionMode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("connectionmode");
            }
            set
            {
                this.OnPropertyChanging("ConnectionMode");
                this.SetAttributeValue("connectionmode", value);
                this.OnPropertyChanged("ConnectionMode");
            }
        }

        /// <summary>
        /// Type of the endpoint contract.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("contract")]
        public Microsoft.Xrm.Sdk.OptionSetValue Contract
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("contract");
            }
            set
            {
                this.OnPropertyChanging("Contract");
                this.SetAttributeValue("contract", value);
                this.OnPropertyChanged("Contract");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the service endpoint was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Description of the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Version in which the form is introduced.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Information that specifies whether this component can be customized.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
            }
            set
            {
                this.OnPropertyChanging("IsCustomizable");
                this.SetAttributeValue("iscustomizable", value);
                this.OnPropertyChanged("IsCustomizable");
            }
        }

        /// <summary>
        /// Information that specifies whether this component is managed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("issaskeyset")]
        public System.Nullable<bool> IsSASKeySet
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("issaskeyset");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("issastokenset")]
        public System.Nullable<bool> IsSASTokenSet
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("issastokenset");
            }
        }

        /// <summary>
        /// Content type of the message
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("messageformat")]
        public Microsoft.Xrm.Sdk.OptionSetValue MessageFormat
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("messageformat");
            }
            set
            {
                this.OnPropertyChanging("MessageFormat");
                this.SetAttributeValue("messageformat", value);
                this.OnPropertyChanged("MessageFormat");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the service endpoint was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who modified the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of Service end point.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Full service endpoint address.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("namespaceaddress")]
        public string NamespaceAddress
        {
            get
            {
                return this.GetAttributeValue<string>("namespaceaddress");
            }
            set
            {
                this.OnPropertyChanging("NamespaceAddress");
                this.SetAttributeValue("namespaceaddress", value);
                this.OnPropertyChanged("NamespaceAddress");
            }
        }

        /// <summary>
        /// Format of Service Bus Namespace
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("namespaceformat")]
        public Microsoft.Xrm.Sdk.OptionSetValue NamespaceFormat
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("namespaceformat");
            }
            set
            {
                this.OnPropertyChanging("NamespaceFormat");
                this.SetAttributeValue("namespaceformat", value);
                this.OnPropertyChanged("NamespaceFormat");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the service endpoint is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Path to the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("path")]
        public string Path
        {
            get
            {
                return this.GetAttributeValue<string>("path");
            }
            set
            {
                this.OnPropertyChanging("Path");
                this.SetAttributeValue("path", value);
                this.OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Shared Access Key
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("saskey")]
        public string SASKey
        {
            get
            {
                return this.GetAttributeValue<string>("saskey");
            }
            set
            {
                this.OnPropertyChanging("SASKey");
                this.SetAttributeValue("saskey", value);
                this.OnPropertyChanged("SASKey");
            }
        }

        /// <summary>
        /// Shared Access Key Name
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("saskeyname")]
        public string SASKeyName
        {
            get
            {
                return this.GetAttributeValue<string>("saskeyname");
            }
            set
            {
                this.OnPropertyChanging("SASKeyName");
                this.SetAttributeValue("saskeyname", value);
                this.OnPropertyChanged("SASKeyName");
            }
        }

        /// <summary>
        /// Shared Access Token
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sastoken")]
        public string SASToken
        {
            get
            {
                return this.GetAttributeValue<string>("sastoken");
            }
            set
            {
                this.OnPropertyChanging("SASToken");
                this.SetAttributeValue("sastoken", value);
                this.OnPropertyChanged("SASToken");
            }
        }

        /// <summary>
        /// Unique identifier of the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceendpointid")]
        public System.Nullable<System.Guid> ServiceEndpointId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("serviceendpointid");
            }
            set
            {
                this.OnPropertyChanging("ServiceEndpointId");
                this.SetAttributeValue("serviceendpointid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("ServiceEndpointId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceendpointid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.ServiceEndpointId = value;
            }
        }

        /// <summary>
        /// Unique identifier of the service endpoint.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceendpointidunique")]
        public System.Nullable<System.Guid> ServiceEndpointIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("serviceendpointidunique");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Namespace of the App Fabric solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionnamespace")]
        public string SolutionNamespace
        {
            get
            {
                return this.GetAttributeValue<string>("solutionnamespace");
            }
            set
            {
                this.OnPropertyChanging("SolutionNamespace");
                this.SetAttributeValue("solutionnamespace", value);
                this.OnPropertyChanged("SolutionNamespace");
            }
        }

        /// <summary>
        /// Additional user claim value type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("userclaim")]
        public Microsoft.Xrm.Sdk.OptionSetValue UserClaim
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("userclaim");
            }
            set
            {
                this.OnPropertyChanging("UserClaim");
                this.SetAttributeValue("userclaim", value);
                this.OnPropertyChanged("UserClaim");
            }
        }

        /// <summary>
        /// 1:N serviceendpoint_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("serviceendpoint_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> serviceendpoint_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("serviceendpoint_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("serviceendpoint_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("serviceendpoint_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("serviceendpoint_sdkmessageprocessingstep");
            }
        }

       
    }

    /// <summary>
    /// Data equivalent to files used in Web development. Web resources provide client-side components that are used to provide custom user interface elements.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("webresource")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public partial class WebResource : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public WebResource() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "webresource";

        public const int EntityTypeCode = 9333;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Information that specifies whether this component can be deleted.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canbedeleted")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty CanBeDeleted
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("canbedeleted");
            }
            set
            {
                this.OnPropertyChanging("CanBeDeleted");
                this.SetAttributeValue("canbedeleted", value);
                this.OnPropertyChanged("CanBeDeleted");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Bytes of the web resource, in Base64 format.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("content")]
        public string Content
        {
            get
            {
                return this.GetAttributeValue<string>("content");
            }
            set
            {
                this.OnPropertyChanging("Content");
                this.SetAttributeValue("content", value);
                this.OnPropertyChanged("Content");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the web resource was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Description of the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Display name of the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("displayname")]
        public string DisplayName
        {
            get
            {
                return this.GetAttributeValue<string>("displayname");
            }
            set
            {
                this.OnPropertyChanging("DisplayName");
                this.SetAttributeValue("displayname", value);
                this.OnPropertyChanged("DisplayName");
            }
        }

        /// <summary>
        /// Version in which the form is introduced.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Information that specifies whether this component can be customized.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
            }
            set
            {
                this.OnPropertyChanging("IsCustomizable");
                this.SetAttributeValue("iscustomizable", value);
                this.OnPropertyChanged("IsCustomizable");
            }
        }

        /// <summary>
        /// Information that specifies whether this web resource is enabled for mobile client.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isenabledformobileclient")]
        public System.Nullable<bool> IsEnabledForMobileClient
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isenabledformobileclient");
            }
            set
            {
                this.OnPropertyChanging("IsEnabledForMobileClient");
                this.SetAttributeValue("isenabledformobileclient", value);
                this.OnPropertyChanged("IsEnabledForMobileClient");
            }
        }

        /// <summary>
        /// Information that specifies whether this component should be hidden.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ishidden")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsHidden
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("ishidden");
            }
            set
            {
                this.OnPropertyChanging("IsHidden");
                this.SetAttributeValue("ishidden", value);
                this.OnPropertyChanged("IsHidden");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Language of the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("languagecode")]
        public System.Nullable<int> LanguageCode
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("languagecode");
            }
            set
            {
                this.OnPropertyChanging("LanguageCode");
                this.SetAttributeValue("languagecode", value);
                this.OnPropertyChanged("LanguageCode");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the web resource was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who modified the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization associated with the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Silverlight runtime version number required by a silverlight web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("silverlightversion")]
        public string SilverlightVersion
        {
            get
            {
                return this.GetAttributeValue<string>("silverlightversion");
            }
            set
            {
                this.OnPropertyChanging("SilverlightVersion");
                this.SetAttributeValue("silverlightversion", value);
                this.OnPropertyChanged("SilverlightVersion");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Unique identifier of the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("webresourceid")]
        public System.Nullable<System.Guid> WebResourceId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("webresourceid");
            }
            set
            {
                this.OnPropertyChanging("WebResourceId");
                this.SetAttributeValue("webresourceid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("WebResourceId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("webresourceid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.WebResourceId = value;
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("webresourceidunique")]
        public System.Nullable<System.Guid> WebResourceIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("webresourceidunique");
            }
        }

        /// <summary>
        /// Drop-down list for selecting the type of the web resource.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("webresourcetype")]
        public Microsoft.Xrm.Sdk.OptionSetValue WebResourceType
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("webresourcetype");
            }
            set
            {
                this.OnPropertyChanging("WebResourceType");
                this.SetAttributeValue("webresourcetype", value);
                this.OnPropertyChanged("WebResourceType");
            }
        }

       
    }


}
