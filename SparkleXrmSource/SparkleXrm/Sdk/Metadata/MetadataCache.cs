// MetadataCache.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk.Metadata
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OptionSetItem
    {
        public string Name;
        public int Value;
    }

    /// <summary>
    /// Class to ensure that only a single set of metadata is downloaded to the client per page
    /// </summary>
    public class MetadataCache
    {
        private static Dictionary<string, AttributeMetadata> _attributeMetaData= new Dictionary<string,AttributeMetadata>();
        private static Dictionary<string, EntityMetadata> _entityMetaData = new Dictionary<string, EntityMetadata>();
        private static Dictionary<string, List<OptionSetItem>> _optionsCache = new Dictionary<string, List<OptionSetItem>>();

        // Allow access to the cache so that it can be overridden if needed
        public Dictionary<string, EntityMetadata> EntityMetaData
        {
            get { return _entityMetaData; }
        }
        public Dictionary<string, AttributeMetadata> AttributeMetaData
        {
            get { return _attributeMetaData; }
        }
        public Dictionary<string, List<OptionSetItem>> OptionsetMetaData
        {
            get { return _optionsCache; }
        }
        public static List<OptionSetItem> GetOptionSetValues(string entityLogicalName, string attributeLogicalName, bool? allowEmpty)
        {
            if (allowEmpty == null) allowEmpty = false;
            string cacheKey = entityLogicalName + "." + attributeLogicalName + "." + allowEmpty.ToString();
           

            if (_optionsCache.ContainsKey(cacheKey))
                return _optionsCache[cacheKey];
            else
            {
                AttributeMetadata attribute = LoadAttributeMetadata(entityLogicalName, attributeLogicalName);
                PicklistAttributeMetadata pickList = (PicklistAttributeMetadata)attribute;
                List<OptionSetItem> opts = new List<OptionSetItem>();
                if (allowEmpty.Value)
                    opts.Add(new OptionSetItem());

                foreach (OptionMetadata o in pickList.OptionSet.Options)
                {
                    OptionSetItem a = new OptionSetItem();
                    a.Name = o.Label.UserLocalizedLabel.Label;
                    a.Value = o.Value.Value;
                    opts.Add(a);
                }
                _optionsCache[cacheKey] = opts;
                return opts;
            }

        }
        public static int? GetEntityTypeCodeFromName(string typeName)
        {
            EntityMetadata entity = LoadEntityMetadata(typeName);
            return entity.ObjectTypeCode;
        }
        public static string GetSmallIconUrl(string typeName)
        {
            // Check if the entity is custom, if so does it have a custom icon?
            EntityMetadata entity = LoadEntityMetadata(typeName);
            if (entity.IsCustomEntity!=null && entity.IsCustomEntity.Value==true)
            {
                // Does it have a custom icon?
                if (entity.IconSmallName != null)
                    return "../../" + entity.IconSmallName; // This assumes that we are always at the location solutionprefix_/html/page.htm - so we benefit from cached images using the cache prefix
                else
                    return "../../../../_Common/icon.aspx?cache=1&iconType=NavigationIcon&objectTypeCode=" + entity.ObjectTypeCode.ToString(); 
                    //return "/_imgs/ico_16_customEnity.gif";
            }
            else
                return "/_imgs/ico_16_" + entity.ObjectTypeCode.ToString() + ".gif";
            

        }
        private static EntityMetadata LoadEntityMetadata(string entityLogicalName)
        {
            string cacheKey = entityLogicalName;
            EntityMetadata metaData = (EntityMetadata)_entityMetaData[cacheKey];

            if (metaData == null)
            {
                RetrieveEntityRequest request = new RetrieveEntityRequest();
                request.EntityFilters = EntityFilters.Entity;
                request.LogicalName = entityLogicalName;
                request.RetrieveAsIfPublished = true;
                request.MetadataId = new Guid("00000000-0000-0000-0000-000000000000");

                RetrieveEntityResponse response = (RetrieveEntityResponse)OrganizationServiceProxy.Execute(request);
                metaData = response.EntityMetadata;
                _entityMetaData[cacheKey] = metaData;
            }
            return metaData;
        }
        private static AttributeMetadata LoadAttributeMetadata(string entityLogicalName, string attributeLogicalName)
        {
            string cacheKey = entityLogicalName +"|" + attributeLogicalName;
            AttributeMetadata metaData = (AttributeMetadata)_attributeMetaData[cacheKey];

            if (metaData==null)
            {
                RetrieveAttributeRequest request = new RetrieveAttributeRequest();
                request.EntityLogicalName = entityLogicalName;
                request.LogicalName = attributeLogicalName;
                request.RetrieveAsIfPublished = true;

           
                RetrieveAttributeResponse response = (RetrieveAttributeResponse)OrganizationServiceProxy.Execute(request);
                metaData = response.AttributeMetadata;
                _attributeMetaData[cacheKey] = metaData;
            }
            return metaData;
        }
    }
}
