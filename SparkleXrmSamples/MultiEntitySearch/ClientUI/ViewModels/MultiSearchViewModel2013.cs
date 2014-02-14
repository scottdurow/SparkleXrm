// MultiSearchViewModel.cs
//

using Client.MultiEntitySearch.ViewModels;
using jQueryApi;
using KnockoutApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Html;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;
using Xrm.Sdk.Metadata.Query;

namespace Client.MultiEntitySearch.ViewModels
{
    public class MultiSearchViewModel2013 : ViewModelBase
    {
        public Observable<string> SearchTerm = Knockout.Observable<string>();
        public ObservableArray<FetchQuerySettings> Config = Knockout.ObservableArray<FetchQuerySettings>();

        private QueryParser _parser;
        private Dictionary<string, EntityMetadata> _entityMetadata;
        private List<string> _entityTypeNames;
      
        public MultiSearchViewModel2013()
        {
            OrganizationServiceProxy.WithCredentials = true;

            // Get Config
            Dictionary<string, string> dataConfig = PageEx.GetWebResourceData();
   
            // Query the quick search entities
            QueryQuickSearchEntities();
            Dictionary<string, Entity> views = GetViewQueries();

            _parser = new QueryParser();
           
            foreach (string typeName in _entityTypeNames)
            {
                Entity view = views[typeName];
                string fetchXml = view.GetAttributeValueString("fetchxml");
                string layoutXml = view.GetAttributeValueString("layoutxml");
                
                // Parse the fetch and layout to get the attributes and columns
                FetchQuerySettings config = _parser.Parse(fetchXml, layoutXml);
                config.RecordCount = Knockout.Observable<string>();
              
                config.DataView = new VirtualPagedEntityDataViewModel(25, typeof(Entity), true);
                config.RecordCount.SetValue(GetResultLabel(config));
                Config.Push(config);


                // Wire up record count change
                config.DataView.OnPagingInfoChanged.Subscribe(OnPagingInfoChanged(config));
                
            }

            _parser.QueryDisplayNames();

        }
        private Action<EventData,object> OnPagingInfoChanged(FetchQuerySettings config)
        {
            return delegate(EventData e, object p)
            {
                PagingInfo paging = (PagingInfo)p;
                config.RecordCount.SetValue(GetResultLabel(config));
            };
        }

        private Dictionary<string, Entity> GetViewQueries()
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
            foreach (string typeName in _entityMetadata.Keys)
            {
                getviewfetchXml += @"<condition attribute='returnedtypecode' operator='eq' value='" + _entityMetadata[typeName].ObjectTypeCode.ToString() + @"'/>";
            }
            getviewfetchXml += @"
                                    </filter>
                                 <condition attribute='isquickfindquery' operator='eq' value='1'/>
                                    <condition attribute='isdefault' operator='eq' value='1'/>
                                </filter>
                               
                              </entity>
                            </fetch>";

            // Get the Quick Find View
            EntityCollection quickFindQuery = OrganizationServiceProxy.RetrieveMultiple(getviewfetchXml);

            Dictionary<string, Entity> views = new Dictionary<string, Entity>();

            // Get a list of the views indexed by the entity type name so we can preserve the order they are defined in
            foreach (Entity view in quickFindQuery.Entities)
            {
                views[view.GetAttributeValueString("returnedtypecode")] = view;
            }
            return views;
        }

        private void QueryQuickSearchEntities()
        {
            OrganizationServiceProxy.RegisterExecuteMessageResponseType("ExecuteFetch", typeof(ExecuteFetchResponse));

            // Get the entities defined in the correct order
            string fetchxml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='multientitysearchentities'>
                                    <attribute name='entityname' />
                                    <order attribute='entityorder' descending='false' />
                                  </entity>
                                </fetch>";

            // We have to use the deprecated ExecuteFetchRequest because you can't access the multientitysearchentities through RetrieveMultiple
            ExecuteFetchRequest request = new ExecuteFetchRequest();
            request.FetchXml = fetchxml;
            ExecuteFetchResponse entityList = (ExecuteFetchResponse)OrganizationServiceProxy.Execute(request);
            jQueryObject entityListDOM = jQuery.FromHtml(entityList.FetchXmlResult);

           
            _entityTypeNames = new List<string>();
            jQueryObject results = entityListDOM.First().Find("result");
       
            results.Each(delegate(int index, Element element)
            {
                string entityName = XmlHelper.SelectSingleNodeValue((XmlNode)(object)element, "entityname");
                _entityTypeNames.Add(entityName);
            });
          
            MetadataQueryBuilder builder = new MetadataQueryBuilder();
            builder.AddEntities(_entityTypeNames, new List<string>("ObjectTypeCode","DisplayCollectionName")); 
            builder.SetLanguage((int)Script.Literal("USER_LANGUAGE_CODE"));

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse) OrganizationServiceProxy.Execute(builder.Request);
            _entityMetadata = new Dictionary<string, EntityMetadata>();
            foreach (EntityMetadata entity in response.EntityMetadata)
            {
                _entityMetadata[entity.LogicalName] = entity;
            }
           

        }
        public string GetResultLabel(FetchQuerySettings config)
        {
            string label = _entityMetadata[config.RootEntity.LogicalName].DisplayCollectionName.UserLocalizedLabel.Label;
            int? totalRows = config.DataView.GetLength();
            return label + "(" + totalRows.ToString() + ")";
        }
        public string GetEntityDisplayName(int index)
        {
            
            FetchQuerySettings[] settings = Config.GetItems();
           
            if (index >= settings.Length)
            {
                return string.Empty;
            }
            else
            {
                FetchQuerySettings config = settings[index];
                int? totalRows = config.DataView.GetLength();
                string label = _entityMetadata[config.RootEntity.LogicalName].DisplayCollectionName.UserLocalizedLabel.Label;
                return label + "(" + totalRows.ToString() + ")";
            }
           
        }
       
        public void SearchCommand()
        {
            
            // Configure the FetchXml for query by adding search term, parameterising the paging and adjusting columns for lookups
            foreach (FetchQuerySettings config in Config.GetItems())
            {
                config.DataView.ResetPaging();
                config.DataView.FetchXml = null;
                config.DataView.Data.Clear();
                config.DataView.Refresh();

                config.DataView.FetchXml = _parser.GetFetchXmlForQuery(config, "%" + SearchTerm.GetValue() + "%");

                if (config.RootEntity.PrimaryImageAttribute != null)
                {
                    
                    // Add the entityimage_url into the fetch xml
                    int startofAttributes = config.DataView.FetchXml.IndexOf("<attribute ");
                    config.DataView.FetchXml = config.DataView.FetchXml.Substr(0, startofAttributes) + "<attribute name=\"" + config.RootEntity.PrimaryImageAttribute + "_url\" alias='card_image_url'/>" + config.DataView.FetchXml.Substr(startofAttributes);
                   
                }
             
              
                
                
                config.DataView.Reset();
            }
        }

    }
   
}
