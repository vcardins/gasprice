using System;
using GasPrice.Core.Common.Messaging.Models;

namespace GasPrice.Core.Common.Messaging
{
    public class BroadCastEventArgs : EventArgs
    {
        #region Private Members

        /// <summary>
        /// Get or set MessageRequest value
        /// </summary>
        public MessageRequest MessageRequest { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Broadcast event argument class
        /// </summary>
        /// <param name="messageRequest">MessageRequest object value</param>
        public BroadCastEventArgs(MessageRequest messageRequest)
        {
            MessageRequest = messageRequest ?? new MessageRequest();
        }

        #endregion

    }
}
