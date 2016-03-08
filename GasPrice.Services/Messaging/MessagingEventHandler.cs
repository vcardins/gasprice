using System.Collections.Generic;
using System.ComponentModel;
using GasPrice.Core.Common.Information;
using GasPrice.Core.EventHandling;

namespace GasPrice.Services.Messaging
{
    public abstract class MessagingEventHandler<TEvent> : IEventHandler
    {
        protected readonly string Module;
        protected IApplicationInformation AppInfo;
        protected MessagingEventHandler(IApplicationInformation appInfo) 
        {
            AppInfo = appInfo;
            var evtStr = typeof (TEvent).Name;
            if (!string.IsNullOrEmpty(evtStr))
            {
                Module = evtStr.Replace("Event", "");
            } 
        }

        protected string GetEvent(TEvent t)
        {
            var evtStr = GetEventType(t).Replace(Module, "");
            return evtStr;
        }

        protected string GetEventType(TEvent t)
        {
            var evtStr = t.GetType().Name;
            if (!string.IsNullOrEmpty(evtStr))
            {
                evtStr = evtStr.Replace("Event", "");
            }
            return evtStr;
        }

        protected Dictionary<string, string> GetExtraProperties(object extra = null)
        {
            var data = new Dictionary<string, string>();
            data["ApplicationName"] = AppInfo.ApplicationName;

            if (extra == null)
                return data;

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(extra))
            {
                var obj2 = descriptor.GetValue(extra);
                if (obj2 != null)
                {
                    data.Add(descriptor.Name, obj2.ToString());
                }
            }
            return data;
        }
    }
}
