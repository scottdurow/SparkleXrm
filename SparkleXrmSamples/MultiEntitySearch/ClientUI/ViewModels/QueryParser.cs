// QueryParser.cs
//

using jQueryApi;
using KnockoutApi;
using Slick;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;
using Xrm.Sdk.Metadata.Query;

namespace Client.MultiEntitySearch.ViewModels
{
    
    public class QueryParser
    {
      
        Dictionary<string, EntityQuery> EntityLookup = new Dictionary<string, EntityQuery>();
        Dictionary<string, EntityQuery> AliasEntityLookup = new Dictionary<string, EntityQuery>();
        Dictionary<string, AttributeQuery> LookupAttributes = new Dictionary<string, AttributeQuery>();
   
        public FetchQuerySettings Parse(string fetchXml, string layoutXml)
        {
           
            FetchQuerySettings querySettings = new FetchQuerySettings();
            
            querySettings.Columns = new List<Column>();

            jQueryObject fetchXmlDOM = jQuery.FromHtml("<query>" + fetchXml.Replace("{0}", "#Query#") + "</query>");
            jQueryObject fetchElement = fetchXmlDOM.Find("fetch");
            querySettings.FetchXml = fetchXmlDOM;

            ParseFetchXml(querySettings);
           
            querySettings.Columns = ParseLayoutXml(querySettings.RootEntity, layoutXml);

            return querySettings;

        }

        private List<Column> ParseLayoutXml(EntityQuery rootEntity, string layoutXml)
        {
            jQueryObject layout = jQuery.FromHtml(layoutXml);
            jQueryObject cells = layout.Find("cell");
            List<Column>  columns = new List<Column>();
           
            cells.Each(delegate(int index, Element element)
            {
                string cellName = element.GetAttribute("name").ToString();
                string logicalName = cellName;
                EntityQuery entity;
                AttributeQuery attribute;

                // Is this an alias attribute?
                int pos = cellName.IndexOf('.');
                if (pos > -1)
                {
                    // Aliased entity
                    string alias = cellName.Substr(0, pos);
                    logicalName = cellName.Substr(pos + 1);
                    entity = AliasEntityLookup[alias];
                }
                else
                {       
                    // Root entity
                    entity=rootEntity;
                }

                // Does the attribute allready exist?
                if (entity.Attributes.ContainsKey(logicalName))
                {
                    // Already exists
                    attribute = entity.Attributes[logicalName];
                }
                else
                {
                    // New
                    attribute = new AttributeQuery();
                    attribute.Columns = new List<Column>();
                    attribute.LogicalName = logicalName;
                    entity.Attributes[attribute.LogicalName] = attribute;
                }

                // Add column
                int width = int.Parse(element.GetAttribute("width").ToString());
                object disableSorting = element.GetAttribute("disableSorting");
                Column col = GridDataViewBinder.NewColumn(attribute.LogicalName, attribute.LogicalName, width); // Display name get's queried later
                col.Sortable = !(disableSorting != null && disableSorting.ToString() == "1");
                attribute.Columns.Add(col);
                columns.Add(col);

               
            });

            return columns;
        }

        public void QueryDisplayNames()
        {
            // Load the display Names
            MetadataQueryBuilder builder = new MetadataQueryBuilder();
            List<string> entities = new List<string>();
            List<string> attributes = new List<string>();
            
            foreach (string entityLogicalName in EntityLookup.Keys)
            {
                entities.Add(entityLogicalName);
                EntityQuery entity = EntityLookup[entityLogicalName];
                foreach (string attributeLogicalName in entity.Attributes.Keys)
                {
                    AttributeQuery attribute = entity.Attributes[attributeLogicalName];
                    string fieldName = attribute.LogicalName;
                    int pos =  fieldName.IndexOf('.');
                    if (entity.AliasName != null && pos>-1)
                    {
                        fieldName = fieldName.Substr(pos);
                    }
                    attributes.Add(fieldName);
                }
            }
            builder.AddEntities(entities, new List<string>("Attributes", "DisplayName", "DisplayCollectionName", "PrimaryImageAttribute")); 
            builder.AddAttributes(attributes, new List<string>("DisplayName", "AttributeType", "IsPrimaryName"));
            builder.SetLanguage((int)Script.Literal("USER_LANGUAGE_CODE"));

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse) OrganizationServiceProxy.Execute(builder.Request);
            // Update the display names
            // TODO: Add the lookup relationship in brackets for alias entitie
            foreach (EntityMetadata entityMetadata in response.EntityMetadata)
            {
                // Get the entity
                EntityQuery entityQuery = EntityLookup[entityMetadata.LogicalName];
                entityQuery.DisplayName = entityMetadata.DisplayName.UserLocalizedLabel.Label;
                entityQuery.DisplayCollectionName = entityMetadata.DisplayCollectionName.UserLocalizedLabel.Label;
                entityQuery.PrimaryImageAttribute = entityMetadata.PrimaryImageAttribute;
                
                foreach (AttributeMetadata attribute in entityMetadata.Attributes)
                {
                    if (entityQuery.Attributes.ContainsKey(attribute.LogicalName))
                    {
                        // Set the type
                        AttributeQuery attributeQuery = entityQuery.Attributes[attribute.LogicalName];
                        attributeQuery.AttributeType = attribute.AttributeType;
                        switch (attribute.AttributeType)
                        {
                            case AttributeTypeCode.Lookup:
                            case AttributeTypeCode.Picklist:
                            case AttributeTypeCode.Customer:
                            case AttributeTypeCode.Owner:
                            case AttributeTypeCode.Status:
                            case AttributeTypeCode.State:
                            case AttributeTypeCode.Boolean_:
                                LookupAttributes[attribute.LogicalName] = attributeQuery;
                                break;
                        }

                        attributeQuery.IsPrimaryName = attribute.IsPrimaryName;
                       
                        // If the type is a lookup, then add the 'name' on to the end in the fetchxml
                        // this is so that we search the text value and not the numeric/guid value
                        foreach (Column col in attributeQuery.Columns)
                        {
                            col.Name = attribute.DisplayName.UserLocalizedLabel.Label;
                            col.DataType = attribute.IsPrimaryName.Value ? "PrimaryNameLookup" : attribute.AttributeType.ToString();
                        }
                    }
                }
            }

        }
        private void ParseFetchXml(FetchQuerySettings querySettings)
        {
            jQueryObject fetchElement = querySettings.FetchXml;

            // Get the entities and link entities - only support 1 level deep
            jQueryObject entityElement = fetchElement.Find("entity");
            string logicalName = entityElement.GetAttribute("name");

            EntityQuery rootEntity;
            // Get query from cache or create new
            if (!EntityLookup.ContainsKey(logicalName))
            {
                rootEntity = new EntityQuery();
                rootEntity.LogicalName = logicalName;
                rootEntity.Attributes = new Dictionary<string, AttributeQuery>();
                EntityLookup[rootEntity.LogicalName] = rootEntity;
            }
            else
            {
                rootEntity = EntityLookup[logicalName];
            }

            // Get Linked Entities(1 deep)
            jQueryObject linkEntities = entityElement.Find("link-entity");
            linkEntities.Each(delegate(int index, Element element)
            {
                EntityQuery link = new EntityQuery();
                link.Attributes = new Dictionary<string, AttributeQuery>();
                link.AliasName = element.GetAttribute("alias").ToString();
                link.LogicalName =  element.GetAttribute("name").ToString();

                if (!EntityLookup.ContainsKey(link.LogicalName))
                {
                    EntityLookup[link.LogicalName] = link;
                }
                else
                {
                    string alias = link.AliasName;
                    link = EntityLookup[link.LogicalName];
                    link.AliasName = alias;
                }

                if (!AliasEntityLookup.ContainsKey(link.AliasName))
                {
                    AliasEntityLookup[link.AliasName] = link;
                }
            });

            querySettings.RootEntity = rootEntity;

        }
       
        public string GetFetchXmlForQuery(FetchQuerySettings config, string searchTerm)
        {
         
            jQueryObject fetchElement = config.FetchXml.Find("fetch");
            fetchElement.Attribute("count", "{0}");
            fetchElement.Attribute("paging-cookie", "{1}");
            fetchElement.Attribute("page", "{2}");
            fetchElement.Attribute("returntotalrecordcount", "true");
            fetchElement.Attribute("distinct", "true");
            fetchElement.Attribute("no-lock", "true");

            jQueryObject orderByElement = fetchElement.Find("order");
            orderByElement.Remove();

            // Add the search string and adjust any lookup columns
            jQueryObject conditions = fetchElement.Find("filter[isquickfindfields='1']");

            conditions.First().Children().Each(delegate(int index, Element element)
            {
                // Is this a lookup column?
                string logicalName = element.GetAttribute("attribute").ToString();
                if (LookupAttributes.ContainsKey(logicalName))
                {
                    element.SetAttribute("attribute", logicalName + "name");
                }
            });
            // Add the sort order placeholder
            string fetchXml = config.FetchXml.GetHtml().Replace("</entity>", "{3}</entity>");

            // Add the Query term
            fetchXml = fetchXml.Replace("#Query#", XmlHelper.Encode(searchTerm));
            return fetchXml;
        }
    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class FetchQuerySettings
    {
        public string DisplayName;
        public List<Column> Columns;
        public EntityDataViewModel DataView;
        public jQueryObject FetchXml;
        public EntityQuery RootEntity;
        public Observable<string> RecordCount;
    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class EntityQuery
    {
        public string DisplayCollectionName;
        public string DisplayName;
        public string LogicalName;
        public string AliasName;
        public string PrimaryImageAttribute;
        public Dictionary<string,AttributeQuery> Attributes;
        //public List<EntityQuery> LinkedEntities;
       
       
    }

    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class AttributeQuery
    {
        public List<Column> Columns;
        public string LogicalName;
        public AttributeTypeCode AttributeType;
        public bool? IsPrimaryName;
      
    }
}
