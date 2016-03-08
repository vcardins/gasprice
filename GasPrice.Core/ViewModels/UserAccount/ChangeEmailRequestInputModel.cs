using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class ChangeEmailRequestInputModel
    {
        //[Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}