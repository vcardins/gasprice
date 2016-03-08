using System;
using GasPrice.Core.Common.Extensions;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class UserProfileOutput
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get { return FirstName.GetDisplayName(LastName); } }
        public string FullName { get { return FirstName.GetFullName(LastName); } }
        public DateTime? LastLogin { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
    }
}
