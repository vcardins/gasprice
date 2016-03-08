using GasPrice.Core.Common.Enums;
using GasPrice.Core.EventHandling;

namespace GasPrice.Core.Events
{
    public abstract class BaseEvent : IEvent
    {
        public int ObjectId { get; set; }
        public ModelType ObjectType { get; set; }
        public ModelAction Action { get; set; }
        public string Message { get; set; }
        
    }
   
}