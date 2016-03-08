namespace GasPrice.Core.EventHandling
{
    public class EventsHandler
    {

        private readonly EventBus _eventBus = new EventBus();

        public IEventBus EventBus
        {
            get { return _eventBus; }
        }

        public void AddEventHandler(params IEventHandler[] handlers)
        {
            _eventBus.AddRange(handlers);
        }

        private readonly EventBus _validationBus = new EventBus();

        public IEventBus ValidationBus
        {
            get { return _validationBus; }
        }

        public void AddValidationHandler(params IEventHandler[] handlers)
        {
            _validationBus.AddRange(handlers);
        }

    }
}
