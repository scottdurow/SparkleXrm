namespace spkl.CrmSvcUtilExtensions
{
    using Microsoft.Crm.Services.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Allows filtering of the message so that we only generate the actions we want
    /// </summary>
    public class MessageFilteringService : ICodeWriterMessageFilterService
    {
        private List<string> _messageFilter;

        public MessageFilteringService(ICodeWriterMessageFilterService defaultService)
        {
            this.DefaultService = defaultService;
            _messageFilter = Config.GetMessageFilter();
        }

        public bool GenerateSdkMessage(SdkMessage message, IServiceProvider services)
        {
            List<string> lowerCaseMessages = (from x in _messageFilter select x.ToLower()).ToList<string>();
            if (!message.IsCustomAction)
            {
                return false;
            }
            if (!lowerCaseMessages.Contains(message.Name.ToLower()))
            {
                return false;
            }
            return this.DefaultService.GenerateSdkMessage(message, services);
        }

        public bool GenerateSdkMessagePair(SdkMessagePair messagePair, IServiceProvider services)
        {
            return this.DefaultService.GenerateSdkMessagePair(messagePair, services);
        }
            
        private ICodeWriterMessageFilterService DefaultService { get; set; }
    }
}

