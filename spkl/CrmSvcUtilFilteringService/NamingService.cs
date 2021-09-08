namespace spkl.CrmSvcUtilExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.Crm.Services.Utility;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Metadata;

    public sealed class NamingService : INamingService
    {
        /// <summary>
        /// Implement this class if you want to provide your own logic for building names that
        /// will appear in the generated code.
        /// </summary>
        private INamingService DefaultNamingService { get; }

        /// <summary>
        /// This field keeps track of the number of times that options with the same
        /// name would have been defined.
        /// </summary>
        private readonly Dictionary<OptionSetMetadataBase, Dictionary<String, int>> _optionNames;
        public NamingService(INamingService namingService)
        {
            DefaultNamingService = namingService;
            _optionNames = new Dictionary<OptionSetMetadataBase, Dictionary<String, int>>();
        }

        /// <summary>
        /// Provide a new implementation for finding a name for an OptionSet. If the
        /// OptionSet is not global, we want the name to be the concatenation of the Entity's
        /// name and the Attribute's name.  Otherwise, we can use the default implementation.
        /// </summary>
        public String GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            // Ensure that the OptionSet is not global before using the custom
            // implementation.
            if (optionSetMetadata.IsGlobal.HasValue && !optionSetMetadata.IsGlobal.Value)
            {
                // Find the attribute which uses the specified OptionSet.
                var attribute =
                    (from a in entityMetadata.Attributes
                     where a.AttributeType == AttributeTypeCode.Picklist && ((EnumAttributeMetadata)a).OptionSet.MetadataId == optionSetMetadata.MetadataId
                     select a).FirstOrDefault();

                // Check for null, since statuscode attributes on custom entities are not
                // global, but their optionsets are not included in the attribute
                // metadata of the entity, either.
                if (attribute != null)
                {
                    // Concatenate the name of the entity and the name of the attribute
                    // together to form the OptionSet name.
                    return $"{DefaultNamingService.GetNameForEntity(entityMetadata, services)}{DefaultNamingService.GetNameForAttribute(entityMetadata, attribute, services)}";
                }
            }

            return DefaultNamingService.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
        }

        #region other INamingService Methods

        public String GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForAttribute(entityMetadata, attributeMetadata, services);
        }

        public String GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForEntity(entityMetadata, services);
        }

        public String GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForEntitySet(entityMetadata, services);
        }

        public String GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForMessagePair(messagePair, services);
        }

        /// <summary>
        /// Handles building the name for an Option of an OptionSet.
        /// </summary>
        public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services)
        {
            int lngCode = 0;
            String[] arguments = Environment.GetCommandLineArgs();

            foreach (var arg in arguments)
            {
                Console.WriteLine("Argument" + arg);

                if (arg.Contains("lngCode"))
                {
                    var split = arg.Split(':');
                    if (split.Length == 2)
                    {
                        int.TryParse(split[1], out lngCode);
                    }
                }
            }


            var name = string.Empty;
            if (optionMetadata is StateOptionMetadata stateOptionMetadata)
            {
                name = stateOptionMetadata.InvariantName;
            }
            else
            {
                var myLng = optionMetadata.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lngCode);

                if (myLng != null)
                    name = myLng.Label;
                else
                {
                    var defLng = optionMetadata.Label.LocalizedLabels.FirstOrDefault();
                    if (defLng != null)
                        name = defLng.Label;
                }

                foreach (var localizedLabel in optionMetadata.Label.LocalizedLabels)
                {
                    if (localizedLabel.LanguageCode == 1033) // English or Finnish
                        name = localizedLabel.Label;
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                if (optionMetadata.Value != null) name = string.Format(CultureInfo.InvariantCulture, "UnknownLabel{0}", new object[] { optionMetadata.Value.Value });
            }

            name = CreateValidName(name);
            name = EnsureValidIdentifier(name);
            name = EnsureUniqueOptionName(optionSetMetadata, name);

            return name;
        }

        private static readonly Regex NameRegex = new Regex("[äöåa-z0-9_]*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static string CreateValidName(string name)
        {
            name = TransformToPascalCase(name);

            string input = name.Replace("$", "CurrencySymbol_").Replace("(", "_");
            var stringBuilder = new StringBuilder();
            for (Match match = NameRegex.Match(input); match.Success; match = match.NextMatch())
                stringBuilder.Append(match.Value);

            var newName = stringBuilder.ToString();
            Console.WriteLine(newName);

            return newName;
        }


        /// <summary>
        /// Checks to make sure that the name begins with a valid character. If the name
        /// does not begin with a valid character, then add an underscore to the
        /// beginning of the name.
        /// </summary>
        private static String EnsureValidIdentifier(String name)
        {

            // Check to make sure that the option set begins with a word character
            // or underscore.
            var pattern = @"^[ÄäÖöÅåA-Za-z_][ÄäÖöÅåA-Za-z0-9_]*$";
            if (!Regex.IsMatch(name, pattern))
            {
                // Prepend an underscore to the name if it is not valid.
                name = $"_{name}";
                Trace.TraceInformation($"Name of the option changed to {name}");
            }
            return name;
        }

        /// <summary>
        /// Checks to make sure that the name does not already exist for the OptionSet
        /// to be generated.
        /// </summary>
        private String EnsureUniqueOptionName(OptionSetMetadataBase metadata, String name)
        {
            if (_optionNames.ContainsKey(metadata))
            {
                if (_optionNames[metadata].ContainsKey(name))
                {
                    // Increment the number of times that an option with this name has
                    // been found.
                    ++_optionNames[metadata][name];

                    // Append the number to the name to create a new, unique name.
                    var newName = $"{name}_{_optionNames[metadata][name]}";

                    Trace.TraceInformation($"The {metadata.Name} OptionSet already contained a definition for {name}. Changed to {newName}");

                    // Call this function again to make sure that our new name is unique.
                    return EnsureUniqueOptionName(metadata, newName);
                }
            }
            else
            {
                // This is the first time this OptionSet has been encountered. Add it to
                // the dictionary.
                _optionNames[metadata] = new Dictionary<string, int>();
            }

            // This is the first time this name has been encountered. Begin keeping track
            // of the times we've run across it.
            _optionNames[metadata][name] = 1;

            return name;
        }

        public String GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForRelationship(entityMetadata, relationshipMetadata, reflexiveRole, services);
        }

        public String GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForRequestField(
                request, requestField, services);
        }

        public String GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services)
        {
            return DefaultNamingService.GetNameForResponseField(response, responseField, services);
        }

        public String GetNameForServiceContext(IServiceProvider services)
        {
            return DefaultNamingService.GetNameForServiceContext(services);
        }

        #endregion

        public static string TransformToPascalCase(string s)
        {
            TextInfo txtInfo = new CultureInfo(CultureInfo.InvariantCulture.Name, false).TextInfo;
            return Regex.Replace(txtInfo.ToTitleCase(s), @"\W", "");
        }


        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}