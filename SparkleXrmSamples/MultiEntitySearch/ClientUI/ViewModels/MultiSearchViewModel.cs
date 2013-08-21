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
using System.Html;
using Xrm;
using Xrm.Sdk;

namespace Client.MultiEntitySearch.ViewModels
{
    public class MultiSearchViewModel : ViewModelBase
    {
        public Observable<string> SearchTerm = Knockout.Observable<string>();
        public QueryParser parser;

        public ObservableArray<FetchQuerySettings> Config = Knockout.ObservableArray<FetchQuerySettings>();
        public MultiSearchViewModel()
        {
            // Get Config
            Dictionary<string, string> dataConfig = PageEx.GetWebResourceData();

            List<string> typeCodes = new List<string>();
            List<string> typeNames = new List<string>();

            // There is an odd behaviour with the savedquery fetchxml where you query with typecodes and get back typenames
            // so we need both to preserve the display order
            if (dataConfig.ContainsKey("typeCodes"))
            {
                typeCodes = (List<string>)(object)dataConfig["typeCodes"].Split(",");
                typeNames = (List<string>)(object)dataConfig["typeNames"].Split(",");
            }
            else
            {
                typeCodes = new List<string>("1", "2", "4", "4200", "3");
                typeNames = new List<string>("account", "contact", "lead", "activitypointer","opportunity");
            }

            // Get the Quick Find View for the entity
            string getviewfetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='savedquery'>
                                <attribute name='name' />
                                <attribute name='fetchxml' />
                                <attribute name='layoutxml' />
                                <attribute name='returnedtypecode' />
                                <filter type='and'>
                                <filter type='or'>";
            foreach (string view in typeCodes)
            {
                getviewfetchXml += @"<condition attribute='returnedtypecode' operator='eq' value='" + view + @"'/>";
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
            parser = new QueryParser();
            Dictionary<string, Entity> entityLookup = new Dictionary<string, Entity>();

            // Preseve the requested view order
            foreach (Entity view in quickFindQuery.Entities)
            {
                entityLookup[view.GetAttributeValueString("returnedtypecode")] = view;
            }
            foreach (string typeName in typeNames)
            {
                Entity view = entityLookup[typeName];
                string fetchXml = view.GetAttributeValueString("fetchxml");
                string layoutXml = view.GetAttributeValueString("layoutxml");

                // Parse the fetch and layout to get the attributes and columns
                FetchQuerySettings config = parser.Parse(fetchXml, layoutXml);
                config.DataView = new EntityDataViewModel(10, typeof(Entity), true);
                Config.Push(config);
            }

            // Load the display name metadata
            parser.QueryDisplayNames();

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
                return settings[index].RootEntity.DisplayCollectionName;
            }
           
        }
        public void SearchCommand()
        {

            // Configure the FetchXml for query by adding search term, parameterising the paging and adjusting columns for lookups
            foreach (FetchQuerySettings config in Config.GetItems())
            {
                config.DataView.FetchXml = parser.GetFetchXmlForQuery(config, "%" + SearchTerm.GetValue() + "%");
                config.DataView.Reset();
                config.DataView.ResetPaging();
                config.DataView.Refresh();
            }
        }

    }
  
}
