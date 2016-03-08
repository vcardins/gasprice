using System.Threading.Tasks;
using GasPrice.Core.Common.Messaging.Models;

namespace GasPrice.Core.Common.Messaging.Interfaces
{
    public interface IEmailService 
    {
        Task SendAsync(EmailMessage message, EmailAttachment attachment = null);
    }

}
