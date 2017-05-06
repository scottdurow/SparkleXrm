namespace spkl.CrmSvcUtilExtensions
{
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Crm.Services.Utility;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    public sealed class MetadataProviderQueryService : IMetadataProviderQueryService
    {
        private static List<string> _excludedNamespaces;
        private IDictionary<string, string> _parameters;
        private List<string> _entities;
        private List<string> _messageFilter;

        public MetadataProviderQueryService()
        {
            _excludedNamespaces = new List<string>();
            _excludedNamespaces.Add("http://schemas.microsoft.com/xrm/2011/contracts");
            GetConfig();
        }

        public MetadataProviderQueryService(IDictionary<string, string> parameters)
        {
            this._parameters = parameters;
            GetConfig();
        }

        private void GetConfig()
        {
            _entities = Config.GetEntities();
            _messageFilter = Config.GetMessageFilter();
            _messageFilter.Add("create"); // Always add the create message
        }
        public EntityMetadata[] RetrieveEntities(IOrganizationService service)
        {    
            List<EntityMetadata> metadata = new List<EntityMetadata>();      
            foreach (string logicalName in _entities)
            {
                var request = new RetrieveEntityRequest {
                    EntityFilters = EntityFilters.Relationships | EntityFilters.Attributes | EntityFilters.Default,
                    LogicalName = logicalName.ToLower()
                };
                var response = (RetrieveEntityResponse) service.Execute(request);
                metadata.Add((EntityMetadata) response.Results["EntityMetadata"]);
            }
            return metadata.ToArray();
        }

        public OptionSetMetadataBase[] RetrieveOptionSets(IOrganizationService service)
        {
            return new OptionSetMetadataBase[0];
        }

        public SdkMessages RetrieveSdkRequests(IOrganizationService service)
        {
            SdkMessages messages = new SdkMessages(null);

            foreach (string messageLogicalName in _messageFilter)
            {
                string fetchQuery = @"<fetch distinct='true' version='1.0'>
                                        <entity name='sdkmessage'>
                                            <attribute name='name'/>
                                            <attribute name='isprivate'/>
                                            <attribute name='sdkmessageid'/>
                                            <attribute name='customizationlevel'/>
                                            <filter>
                                                <condition alias='sdmessagefilter' attribute='name' operator='eq' value='" + messageLogicalName.ToLower() + @"'/>
                                            </filter>
                                            <link-entity name='sdkmessagepair' alias='sdkmessagepair' to='sdkmessageid' from='sdkmessageid' link-type='inner'>
                                                <filter>
                                                    <condition alias='sdkmessagepair' attribute='endpoint' operator='eq' value='2011/Organization.svc' />
                                                </filter>
                                                <attribute name='sdkmessagepairid'/>
                                                <attribute name='namespace'/>
                                                <link-entity name='sdkmessagerequest' alias='sdkmessagerequest' to='sdkmessagepairid' from='sdkmessagepairid' link-type='outer'>
                                                    <attribute name='sdkmessagerequestid'/>
                                                    <attribute name='name'/>
                                                    <link-entity name='sdkmessagerequestfield' alias='sdkmessagerequestfield' to='sdkmessagerequestid' from='sdkmessagerequestid' link-type='outer'>
                                                        <attribute name='name'/>
                                                        <attribute name='optional'/>
                                                        <attribute name='position'/>
                                                        <attribute name='publicname'/>
                                                        <attribute name='clrparser'/>
                                                        <order attribute='sdkmessagerequestfieldid' descending='false' />
                                                    </link-entity>
                                                    <link-entity name='sdkmessageresponse' alias='sdkmessageresponse' to='sdkmessagerequestid' from='sdkmessagerequestid' link-type='outer'>
                                                        <attribute name='sdkmessageresponseid'/>
                                                        <link-entity name='sdkmessageresponsefield' alias='sdkmessageresponsefield' to='sdkmessageresponseid' from='sdkmessageresponseid' link-type='outer'>
                                                            <attribute name='publicname'/>
                                                            <attribute name='value'/>
                                                            <attribute name='clrformatter'/>
                                                            <attribute name='name'/>
                                                            <attribute name='position' />
                                                        </link-entity>
                                                    </link-entity>
                                                </link-entity>
                                            </link-entity>
                                            <link-entity name='sdkmessagefilter' alias='sdmessagefilter' to='sdkmessageid' from='sdkmessageid' link-type='inner'>
                                                <filter>
                                                    <condition alias='sdmessagefilter' attribute='isvisible' operator='eq' value='1' />
                                               </filter>
                                                <attribute name='sdkmessagefilterid'/>
                                                <attribute name='primaryobjecttypecode'/>
                                                <attribute name='secondaryobjecttypecode'/>
                                            </link-entity>
                                            <order attribute='sdkmessageid' descending='false' />
                                        </entity>
                                    </fetch>";

                MessagePagingInfo pageInfo = null;
                int pageNumber = 1;
                var request = new ExecuteFetchRequest();
                while ((pageInfo == null) || pageInfo.HasMoreRecords)
                {
                    string fetch = fetchQuery;
                    if (pageInfo != null)
                    {
                        fetch = this.SetPagingCookie(fetchQuery, pageInfo.PagingCookig, pageNumber);
                    }
                    request.FetchXml = fetch;
                    var response = (ExecuteFetchResponse)service.Execute(request);
                    pageInfo = SdkMessages.FromFetchResult(messages, (string) response.FetchXmlResult);
                    pageNumber++;
                }
            }
            return messages;
        }

        private string SetPagingCookie(string fetchQuery, string pagingCookie, int pageNumber)
        {
            XDocument document = XDocument.Parse(fetchQuery);
            if (pagingCookie != null)
            {
                document.Root.Add(new XAttribute(XName.Get("paging-cookie"), pagingCookie));
            }
            document.Root.Add(new XAttribute(XName.Get("page"), pageNumber.ToString(CultureInfo.InvariantCulture)));
            return document.ToString();
        }
    }
}

