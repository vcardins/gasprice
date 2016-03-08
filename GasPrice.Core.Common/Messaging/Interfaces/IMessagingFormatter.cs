using System.Collections.Generic;
using GasPrice.Core.Common.Information;

namespace GasPrice.Core.Common.Messaging.Interfaces
{
    public interface IMessagingFormatter<TEvent>
    {
        string GetMessageBody(string module, string key, IDictionary<string, string> values);
        string GetMessageSubject(string module, string key, IDictionary<string, string> values);
        IApplicationInformation ApplicationInformation { get; }
    }
}
