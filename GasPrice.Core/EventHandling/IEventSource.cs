using System.Collections.Generic;

namespace GasPrice.Core.EventHandling
{
    public interface IEventSource
    {
        IEnumerable<IEvent> GetEvents();
        void Clear();
    }
}