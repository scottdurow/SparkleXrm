// ActivitySubGridViewModel.cs
//

using Client.MultiEntitySearch.ViewModels;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using Xrm.Sdk;

namespace Client.InlineSubGrids.ViewModels
{
    public class ActivitySubGridViewModel : ViewModelBase
    {
      
        public FetchQuerySettings ViewConfig;
        private QueryParser _parser;
        #region Constructors
        public ActivitySubGridViewModel()
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

            getviewfetchXml += @"<condition attribute='returnedtypecode' operator='eq' value='" + "4200" + @"'/>";
            
            getviewfetchXml += @"
                                    </filter>
                                 <condition attribute='isquickfindquery' operator='eq' value='1'/>
                                    <condition attribute='isdefault' operator='eq' value='1'/>
                                </filter>
                               
                              </entity>
                            </fetch>";
            // Get the Quick Find View
            EntityCollection quickFindQuery = OrganizationServiceProxy.RetrieveMultiple(getviewfetchXml);
            _parser = new QueryParser();
            Entity view = quickFindQuery.Entities[0];
            string fetchXml = view.GetAttributeValueString("fetchxml");
            string layoutXml = view.GetAttributeValueString("layoutxml");

            // Parse the fetch and layout to get the attributes and columns
            ViewConfig = _parser.Parse(fetchXml, layoutXml);
            ViewConfig.DataView = new EntityDataViewModel(10, typeof(Entity), true);
            
            // Load the display name metadata
            _parser.QueryDisplayNames();
            
           
        }
        #endregion

        #region Methods
        public void Init()
        {
            ViewConfig.DataView.FetchXml = _parser.GetFetchXmlForQuery(ViewConfig, "%" + "" + "%");
            ViewConfig.DataView.Reset();
            ViewConfig.DataView.ResetPaging();
            ViewConfig.DataView.Refresh();
        }
        #endregion

    }
}
