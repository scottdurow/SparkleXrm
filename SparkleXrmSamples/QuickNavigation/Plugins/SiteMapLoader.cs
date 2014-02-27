using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QuickNavigation.Plugins
{
    public class SiteMapLoader
    {
        private int _lcid;
        public SiteMapLoader(int lcid)
        {
            _lcid = lcid;
        }
        public void ParseSiteMapToJson(IOrganizationService service, ITracingService trace, StringWriter serialisedSiteMap)
        {
            using (var context = new OrganizationServiceContext(service))
            {
                List<String> privRequired = new List<string>();
                var siteMap = (from s in context.CreateQuery("sitemap")
                               select s.GetAttributeValue<string>("sitemapxml")).Take(1).FirstOrDefault();
                
                XmlDocument map = new XmlDocument();
                map.LoadXml(siteMap);

                trace.Trace("Loaded Site Map {0}", siteMap);

                // Find the Entities that we need to load
                var entityNodes = map.SelectNodes("//SubArea[@Entity!='']");
                List<string> entities = new List<string>();
                foreach (XmlElement node in entityNodes)
                {
                    var entityLogicalName = node.GetAttribute("Entity");
                    trace.Trace("Entity {0}", entityLogicalName);
                    if (!entities.Contains(entityLogicalName))
                        entities.Add(entityLogicalName);
                }
                MetadataQueryBuilder entityQuery = new MetadataQueryBuilder();

                entityQuery.AddEntities(entities, new List<string> { "DisplayCollectionName","ObjectTypeCode","IconSmallName" });
                var entityQueryResult = entityQuery.Execute(service);
                var entityDisplayNames = new Dictionary<string, Dictionary<int, string>>();
                var entityObjectTypeCodes = new Dictionary<string, int>();
                var entityMetaData = new Dictionary<string, EntityMetadata>();
                foreach (var entity in entityQueryResult)
                {
                    var labels = new Dictionary<int, string>();
                    foreach (var label in entity.DisplayCollectionName.LocalizedLabels)
                    {
                        labels[label.LanguageCode] = label.Label;
                    }
                    entityMetaData[entity.LogicalName] = entity;
                    entityDisplayNames[entity.LogicalName] = labels;
                    entityObjectTypeCodes[entity.LogicalName.ToLower()] = (int)entity.ObjectTypeCode;
                }



                SiteMap siteMapJson = new SiteMap();

                foreach (XmlElement area in map.SelectNodes("SiteMap/Area"))
                {
                    Area areaJson = new Area();
                    siteMapJson.areas.Add(areaJson);
                    areaJson.id = area.GetAttribute("Id");
                    areaJson.icon = area.GetAttribute("Icon");
                    OutputTitles(trace,areaJson, area);
                    if (area.GetAttribute("Url").Length > 0)
                        siteMapJson.url = area.GetAttribute("Url");
                       

                    foreach (XmlElement group in area.SelectNodes("Group"))
                    {
                        Group groupJson = new Group();
                        areaJson.groups.Add(groupJson);
                        groupJson.subareas = new List<SubArea>();
                      
                        groupJson.id = group.GetAttribute("Id");
                        groupJson.icon = group.GetAttribute("Icon");
                        OutputTitles(trace,groupJson, group);
                        foreach (XmlElement subarea in group.SelectNodes("SubArea"))
                        {
                            SubArea subAreaJson = new SubArea();
                            groupJson.subareas.Add(subAreaJson);
                            subAreaJson.id = subarea.GetAttribute("Id");

                            bool titleFound = OutputTitles(trace,subAreaJson, subarea);
                            string entity = subarea.GetAttribute("Entity");
                            string icon = subarea.GetAttribute("Icon");
                            string url = subarea.GetAttribute("Url");

                            trace.Trace("Sub Area Id={0} Entity={1} Icon={2} Url={3}", subAreaJson.id, entity, icon, url);
                            if (!String.IsNullOrEmpty(entity) && entityObjectTypeCodes.ContainsKey(entity))
                            {
                            
                                subAreaJson.objecttypecode = entityObjectTypeCodes[entity];
                                if (!String.IsNullOrEmpty(entityMetaData[entity.ToLower()].IconSmallName))
                                    subAreaJson.icon = "/WebResources/" + entityMetaData[entity.ToLower()].IconSmallName;
                                trace.Trace("ObjectTypeCode={0}", subAreaJson.objecttypecode);
                            }

                            if (!titleFound && entity.Length > 0)
                            {
                                var entityTitles = entityDisplayNames.ContainsKey(entity) ? entityDisplayNames[entity] :null;
                               
                                string titleString = "";

                                if (entityTitles!=null && entityTitles.ContainsKey(_lcid))
                                    titleString = entityTitles[_lcid];
                                else
                                {
                                    // Just use the first title irrespective of language
                                    if (entityTitles!=null && entityTitles.Count > 0)
                                    {
                                        entityTitles[_lcid] = entityTitles[entityTitles.Keys.FirstOrDefault()];
                                    }
                                }

                                if (entityTitles!=null && entityTitles.ContainsKey(_lcid))
                                    subAreaJson.title =  entityTitles[_lcid];

                                trace.Trace("titleString ={0}", subAreaJson.title);
                               
                            }


                            if (icon.Length > 0)
                                subAreaJson.icon = icon;
                             
                            if (entity.Length > 0)
                                subAreaJson.entity = entity;
                              
                            if (url.Length > 0)
                                subAreaJson.url = url;


                            trace.Trace("Output Pivs");
                            OutputPrivs(privRequired, subAreaJson, subarea);
                           

                        }
                      
                    }
                  
                }

                siteMapJson.privileges = new List<string>();


                for (int i = 0; i < privRequired.Count; i++)
                {
                    siteMapJson.privileges.Add(privRequired[i]);

                }

                var json = JsonSerializer<SiteMap>(siteMapJson);
                serialisedSiteMap.WriteLine(json);
            }
        }
        private string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }  

        private void OutputPrivs(List<String> privRequired, SubArea subArea, XmlElement subarea)
        {
            
            var privs = subarea.SelectNodes("Privilege");
            string entityPriv = "";
            subArea.privileges = new List<String>();
            // Always add the entity read privilege if this is an entity subarea
            if (!String.IsNullOrEmpty(subArea.entity))
            {
                entityPriv = "prvRead" + subArea.entity;
                privRequired.Add(entityPriv);
                subArea.privileges.Add(entityPriv);
            }
            if (privs.Count > 0)
            {
               
                foreach (XmlElement privilege in privs)
                {
                    var entity = privilege.GetAttribute("Entity");
                    var levels = privilege.GetAttribute("Privilege").Split(',');
                    foreach (string level in levels)
                    {
                        var priv = string.Format("prv{0}{1}", level,entity);
                        // Only add if different to entity priviledge
                        if (priv != entityPriv)
                        {
                            privRequired.Add(priv);
                            
                            subArea.privileges.Add(priv);
                        }
                      
                    }
                }
                
            }
        }

        private bool OutputTitles(ITracingService trace, SiteMapElement mapJson, XmlElement element)
        {
            bool titleFound = false;

            // Try and get deprecated element
            var titleAttribute = element.GetAttribute("Title");
            if (!string.IsNullOrEmpty(titleAttribute))
            {
                mapJson.title = element.GetAttribute("Title");
                return true;
            }

            var titles = element.SelectNodes("Titles/Title[@LCID='"+ _lcid.ToString() +"']");
            if (titles.Count == 0)
            {
                // Get just the first LCID as  fall back
                titles = element.SelectNodes("Titles/Title[0]");
            }

            if (titles.Count > 0)
            {
                //mapJson.WriteLine("Titles:{");
                foreach (XmlElement title in titles)
                {
                    mapJson.title = title.GetAttribute("Title");
                }
                //mapJson.WriteLine("},");
                titleFound = true;
            }
            else
            {
                // Get resource id and lookup
                string resourceId = element.GetAttribute("ResourceId");
                if (!string.IsNullOrEmpty(resourceId))
                {
                    // Lookup resource from resource dictionary for available lanaguages
                    mapJson.title = "$"  + resourceId;
                  
                }
            }
            trace.Trace("Title Found {0}", titleFound);
            return titleFound;
        }
    }
}
