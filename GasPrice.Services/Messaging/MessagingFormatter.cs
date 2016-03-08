using System;
using System.Collections.Generic;
using System.Linq;
using GasPrice.Core.Common;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Common.Messaging.Enums;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Core.Common.Messaging.Models;

namespace GasPrice.Services.Messaging
{
    public class MessagingFormatter<TEvent> : IMessagingFormatter<TEvent>
    {
        private readonly string _module;
        private readonly MessagingType _type;
        private readonly string _bodyTemplatePath;
        private readonly string _subjectTemplatePath;
        private readonly IApplicationInformation _appInfo;

        public MessagingFormatter(MessagingType type, IApplicationInformation appInfo)
        {
            _module = typeof (TEvent).Name.Replace("Event", string.Empty); // module;
            _type = type;
            _appInfo = appInfo;

            switch (_type)
            {
                case MessagingType.Email: 
                    _bodyTemplatePath = AppConstants.MessageTemplatesPath.EmailBody;
                    _subjectTemplatePath = AppConstants.MessageTemplatesPath.EmailSubject; 
                    break;
            }
        }

        public IApplicationInformation ApplicationInformation
        {
            get { return _appInfo; }
        }

        public string GetMessageBody(string module, string key, IDictionary<string, string> values)
        {

            if (string.IsNullOrEmpty(_module)) throw new ArgumentNullException(_module);

            string template = null;

            switch (_type)
            {
                case MessagingType.Email :
                    template = _module.GetHtmlTemplate<EmailTemplate>(_bodyTemplatePath, key);
                    break;
            }

            if (template == null)
                return null;

            if (!values.ContainsKey("ApplicationName"))
                values["ApplicationName"] = _appInfo.ApplicationName;

            return values.Aggregate(template, (current, v) => current.Replace("{{" + v.Key + "}}", v.Value));
        }

        public string GetMessageSubject(string module, string key, IDictionary<string, string> values)
        {
            if (string.IsNullOrEmpty(_module)) throw new ArgumentNullException(_module);

            var tpl = _module.GetTemplate<EmailSubject>(_subjectTemplatePath, key);

            if (tpl.Subject == null)
                return null;

            return values.Aggregate(tpl.Subject, (current, v) => current.Replace("{{" + v.Key + "}}", v.Value));
        }
       
    }
  
}
