using GasPrice.Core.Common.Messaging.Enums;

namespace GasPrice.Core.Common.Messaging.Models
{
    /// <summary>
    /// Message Request class
    /// </summary>
    public class MessageRequest
    {

        /// <summary>
        /// Get or set message value
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get or set EventName
        /// </summary>
        public EventNameEnum EventName { get; set; }

    }
}
