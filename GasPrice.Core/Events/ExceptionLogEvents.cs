using GasPrice.Core.Common.Enums;
using GasPrice.Core.Models.Infraestructure;

namespace GasPrice.Core.Events
{
    public abstract class ExceptionLogEvent : BaseEvent
    {
        public int ExceptionLogId { get; set; }
        public ExceptionLog Exception { get; set; }

        protected ExceptionLogEvent()
        {
            ObjectType = ModelType.ExceptionLog;
        }
    }

    public class ExceptionLogCreatedEvent : ExceptionLogEvent
    {
        public ExceptionLogCreatedEvent(ExceptionLog err)
        {
            Exception = err;
        }
    }

}