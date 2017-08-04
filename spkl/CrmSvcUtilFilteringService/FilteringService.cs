namespace spkl.CrmSvcUtilExtensions
{
    using Microsoft.Crm.Services.Utility;
    using Microsoft.Xrm.Sdk.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Linq;

    /// <summary>
    /// Allows filtering of the metadata to only generate the entities that we want
    /// </summary>
    public sealed class FilteringService : ICodeWriterFilterService
    {
        private List<string> _entities = null;
        private string _entityLogicalName = null;

        public FilteringService(ICodeWriterFilterService defaultService)
        {
            this.DefaultService = defaultService;
            _entities = Config.GetEntities();
        }

        public bool GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateAttribute(attributeMetadata, services);
        } 

        public bool GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        { 
            if (!_entities.Contains(entityMetadata.LogicalName))
            {
                return false;
            }
            _entityLogicalName = entityMetadata.LogicalName;
            return this.DefaultService.GenerateEntity(entityMetadata, services);
        }

        public bool GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateOption(optionMetadata, services);
        }

        public bool GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {

            // Should we output a enum optionset or just plain OptionSetValue?
            var globalOptionsets = Config.GetConfig("globalEnums") == "true";
            var enums = Config.GetConfig("picklistEnums") == "true" && (optionSetMetadata.IsGlobal!=true || globalOptionsets);
            var states = Config.GetConfig("stateEnums") == "true";
  
            var optionsetValues = optionSetMetadata as OptionSetMetadata;
            if (optionsetValues!=null)
            {
                // check if there are any invalid names
                foreach (var option in optionsetValues.Options)
                {
                    string optionSetName = Regex.Replace(option.Label.UserLocalizedLabel.Label, "[^a-zA-Z0-9]", string.Empty);
                    if ((optionSetName.Length > 0) && !char.IsLetter(optionSetName, 0))
                    {
                        option.Label.UserLocalizedLabel.Label = "Number_" + optionSetName;
                    }
                    else if (optionSetName.Length == 0)
                    {
                        option.Label.UserLocalizedLabel.Label = "empty";
                    }

                    // Set all languages to the user localised version
                    foreach (var label in option.Label.LocalizedLabels)
                    {
                        label.Label = option.Label.UserLocalizedLabel.Label;
                    }
                }

                // check if the names are unique in optionset values
                var duplicateNames = optionsetValues.Options.GroupBy(x => x.Label.UserLocalizedLabel.Label).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                duplicateNames.ForEach(delegate(string duplicate){
                    var option = optionsetValues.Options.Where(x => x.Label.UserLocalizedLabel.Label == duplicate).First();
                    option.Label.UserLocalizedLabel.Label += option.Value.ToString();
                    // Also set the other labels
                    foreach (var label in option.Label.LocalizedLabels)
                    {
                        label.Label = option.Label.UserLocalizedLabel.Label;
                    }
                });  
            }

            var optionType = (OptionSetType)optionSetMetadata.OptionSetType.Value;
            switch (optionType)
            {
                case OptionSetType.State:
                    return states;
                case OptionSetType.Status:
                    return states;
                case OptionSetType.Picklist:
                    return enums;
                default:
                    return false;  
            }
        }

        public bool GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
        }

        public bool GenerateServiceContext(IServiceProvider services)
        {
            return true;
        }

        private ICodeWriterFilterService DefaultService { get; set; }
    }
}

