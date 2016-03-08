using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class PasswordResetInputModel
    {
        //[Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}