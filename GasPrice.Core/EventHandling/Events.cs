using System;

namespace GasPrice.Core.EventHandling
{
    public class DelegateEventHandler<T> : IEventHandler<T>
        where T : IEvent
    {
        readonly Action<T> action;
        public DelegateEventHandler(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            this.action = action;
        }

        public void Handle(T evt)
        {
            action(evt);
        }
    }
}
