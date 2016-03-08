namespace GasPrice.Core.ViewModels.UserProfile
{
    public class UserProfileInfo
    {
        public int UserId { get; set; }
        //[Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //[Required]
        public string Email { get; set; }
        //[Required]
        public string MobilePhoneNumber { get; set; }
    }
}
