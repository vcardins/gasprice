using System.ComponentModel.DataAnnotations;
using GasPrice.Core.Common.Extensions;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class RegisterInputModel
    {
        //[Required]
        public string Username { get; set; }

        //[Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName
        {
            get { return FirstName.GetDisplayName(LastName); }
        }

        public string MobilePhoneNumber { get; set; }

        //[MaxLength(2)]
        //[Required]
        public string CountryOfNationality { get; set; }

        //[MaxLength(2)]
        //[Required]
        public string CountryOfInterest { get; set; }

        //[Required]
        [EmailAddress]
        public string Email { get; set; }
        
        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        //[Required]
        [Compare("Password", ErrorMessage="Password confirmation must match password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}