﻿using System.Collections.Generic;

namespace GasPrice.Core.Common.Messaging.Models
{
    public class EmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool AsHtml { get; set; }
    }
}
