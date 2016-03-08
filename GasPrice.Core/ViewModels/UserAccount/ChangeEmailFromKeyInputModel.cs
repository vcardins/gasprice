using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class ChangeEmailFromKeyInputModel
    {
        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Key { get; set; }
    }
}