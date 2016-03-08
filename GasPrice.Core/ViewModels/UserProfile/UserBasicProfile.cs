using System;
using GasPrice.Core.Common.Extensions;

namespace GasPrice.Core.ViewModels.UserProfile
{
    public class UserBasicProfile
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Iso2 { get; set; }
        public string DisplayName
        {
            get { return FirstName.GetDisplayName(LastName); }
        }
        public string PhotoPath { get; set; }
    }
}