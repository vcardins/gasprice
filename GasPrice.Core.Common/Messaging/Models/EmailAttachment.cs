using System.IO;

namespace GasPrice.Core.Common.Messaging.Models
{
   
    public class EmailAttachment
    {
        public string Name { get; set; }

        public MemoryStream File { get; set; }

    }
}
