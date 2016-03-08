using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class SendUsernameReminderInputModel
    {
        //[Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}