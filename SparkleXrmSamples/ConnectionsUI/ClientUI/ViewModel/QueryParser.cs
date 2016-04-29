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

namespace ClientUI.ViewModels
{
    
    public class QueryParser
    {
        public const string ParentRecordPlaceholder = "#ParentRecordPlaceholder#";
        public IEnumerable<string> Entities;
        public Dictionary<string, EntityQuery> EntityLookup = new Dictionary<string, EntityQuery>();
        Dictionary<string, EntityQuery> AliasEntityLookup = new Dictionary<string, EntityQuery>();
        Dictionary<string, AttributeQuery> LookupAttributes = new Dictionary<string, AttributeQuery>();

        public QueryParser(IEnumerable<string> entities)
        {
            Entities = entities;

        }

        public void GetQuickFinds()
        {
            GetViewDefinition(true, null);
        }
        public void GetView(string entityLogicalName, string viewName)
        {
            GetViewDefinition(false, viewName);
        }
        private void GetViewDefinition(bool isQuickFind, string viewName)
        {
            // Get the Quick Find View for the entity
            string getviewfetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='savedquery'>
                                <attribute name='name' />
                                <attribute name='fetchxml' />
                                <attribute name='layoutxml' />
                                <attribute name='returnedtypecode' />
                                <filter type='and'>
                                <filter type='or'>";
            foreach (string entity in Entities)
            {

                int? typeCode = (int?)Script.Literal("Mscrm.EntityPropUtil.EntityTypeName2CodeMap[{0}]", entity);
                getviewfetchXml += @"<condition attribute='returnedtypecode' operator='eq' value='" + typeCode.ToString() + @"'/>";
            }
            getviewfetchXml += @"</filter>";

            if (isQuickFind)
            {
                getviewfetchXml += @"<condition attribute='isquickfindquery' operator='eq' value='1'/>
                                    <condition attribute='isdefault' operator='eq' value='1'/>";
            }
            else if (viewName != null && viewName.Length > 0)
            {
                getviewfetchXml += @"<condition attribute='name' operator='eq' value='" + XmlHelper.Encode(viewName) + @"'/>";

            }
            else
            {
                // Get default associated view
                getviewfetchXml += @"<condition attribute='querytype' operator='eq' value='2'/>
                                    <condition attribute='isdefault' operator='eq' value='1'/>";
            }

            getviewfetchXml += @"</filter>
                              </entity>
                            </fetch>";
            // Get the Quick Find View
            EntityCollection quickFindQuery = OrganizationServiceProxy.RetrieveMultiple(getviewfetchXml);
            Dictionary<string, Entity> entityLookup = new Dictionary<string, Entity>();

            // Preseve the requested view order
            foreach (Entity view in quickFindQuery.Entities)
            {
                entityLookup[view.GetAttributeValueString("returnedtypecode")] = view;
            }

            foreach (string typeName in Entities)
            {
                Entity view = entityLookup[typeName];
                string fetchXml = view.GetAttributeValueString("fetchxml");
                string layoutXml = view.GetAttributeValueString("layoutxml");
                EntityQuery query;
                if (EntityLookup.ContainsKey(typeName))
                {
                    query = EntityLookup[typeName];

                }
                else
                {
                    query = new EntityQuery();
                    query.LogicalName = typeName;
                    query.Views = new Dictionary<string, FetchQuerySettings>();
                    query.Attributes = new Dictionary<string, AttributeQuery>();
                    EntityLookup[typeName] = query;
                }

                // Parse the fetch and layout to get the attributes and columns
                FetchQuerySettings config = Parse(fetchXml, layoutXml);


                query.Views[view.GetAttributeValueString("name")] = config;
                if (isQuickFind)
                {
                    query.QuickFindQuery = config;
                }
            }
        }
        
        private FetchQuerySettings Parse(string fetchXml, string layoutXml)
        {
           
            FetchQuerySettings querySettings = new FetchQuerySettings();
            
           
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
                object widthAttribute = element.GetAttribute("width");
                if (widthAttribute != null)
                {
                    int width = int.Parse(element.GetAttribute("width").ToString());
                    object disableSorting = element.GetAttribute("disableSorting");
                    Column col = GridDataViewBinder.NewColumn(attribute.LogicalName, attribute.LogicalName, width); // Display name get's queried later
                    col.Sortable = !(disableSorting != null && disableSorting.ToString() == "1");
                    attribute.Columns.Add(col);
                    columns.Add(col);
                }
               
            });

            return columns;
        }

        public void QueryMetadata()
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
                entityQuery.EntityTypeCode = entityMetadata.ObjectTypeCode;
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

            // Issue #35 - Add any lookup/picklist quick find fields that are not included in results attributes will cause a format execption
            // because we don't have the metadata - this means that 'name' is not appended to the attribute

            // Add the search string and adjust any lookup columns
            jQueryObject conditions = fetchElement.Find("filter[isquickfindfields='1']");
            conditions.First().Children().Each(delegate(int index, Element element)
            {
                logicalName = element.GetAttribute("attribute").ToString();
                jQueryObject e = jQuery.FromElement(element);
                jQueryObject p =e.Parents("link-entity");
                if (!querySettings.RootEntity.Attributes.ContainsKey(logicalName))
                {
                    AttributeQuery attribute = new AttributeQuery();
                    attribute.LogicalName = logicalName;
                    attribute.Columns = new List<Column>();
                    querySettings.RootEntity.Attributes[logicalName] = attribute;
                }
            });

        }
               
        public string GetFetchXmlForQuery(string entityLogicalName, string queryName, string searchTerm)
        {
            FetchQuerySettings config;
            if (queryName == "QuickFind")
            {
                config = EntityLookup[entityLogicalName].QuickFindQuery;
            }
            else
            {
                config = EntityLookup[entityLogicalName].Views[queryName];
            }
            jQueryObject fetchElement = config.FetchXml.Find("fetch");
         
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
            string fetchXml = config.FetchXml.GetHtml();//.Replace("</entity>", "{3}</entity>");

            // Add the Query term
            fetchXml = fetchXml.Replace("#Query#", XmlHelper.Encode(searchTerm));
            return fetchXml;
        }
        public static string GetFetchXmlParentFilter(FetchQuerySettings query, string parentAttribute)
        {
            
            jQueryObject fetchElement = query.FetchXml.Find("fetch");
            fetchElement.Attribute("count", "{0}");
            fetchElement.Attribute("paging-cookie", "{1}");
            fetchElement.Attribute("page", "{2}");
            fetchElement.Attribute("returntotalrecordcount", "true");
            fetchElement.Attribute("distinct", "true");
            fetchElement.Attribute("no-lock", "true");
            jQueryObject orderByElement = fetchElement.Find("order");
            // Get the default order by field - currently only supports a single sort by column
            query.OrderByAttribute = orderByElement.GetAttribute("attribute");
            query.OrderByDesending = orderByElement.GetAttribute("descending") == "true";

            orderByElement.Remove();

            // Get the root filter (if there is one)
            jQueryObject filter = fetchElement.Find("entity>filter");
           
            if (filter != null )
            {
                // Check that it is an 'and' filter
                string filterType = filter.GetAttribute("type");
                if (filterType == "or")
                {
                    // wrap up in an and filter
                    jQueryObject andFilter = jQuery.FromHtml("<filter type='and'>" + filter.GetHtml() + "</filter>");

                    // remove existing filter
                    filter.Remove();

                    filter = andFilter;
                    // Add in the existing filter
                    fetchElement.Find("entity").Append(andFilter);

                }
            }

            // Add in the parent query filter
            jQueryObject parentFilter = jQuery.FromHtml("<condition attribute='" + parentAttribute + "' operator='eq' value='" + ParentRecordPlaceholder + "'/>");
            filter.Append(parentFilter);

            // Add the order by placeholder for the EntityDataViewModel
            return query.FetchXml.GetHtml().Replace("</entity>", "{3}</entity>");
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
        public string OrderByAttribute;
        public bool OrderByDesending;
       
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
        public FetchQuerySettings QuickFindQuery;
        public Dictionary<string, FetchQuerySettings> Views;
        public int? EntityTypeCode;
        public string PrimaryImageAttribute;
        public Dictionary<string,AttributeQuery> Attributes;
       
       
       
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
