namespace GasPrice.Core.EventHandling
{
    public interface IEventBus
    {
        void RaiseEvent(IEvent evt);
    }
}