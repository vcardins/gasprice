using GasPrice.Core.Common.Extensions;

namespace GasPrice.Core.ViewModels
{
    public class UserNameInfo
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string DisplayName
        {
            get { return Firstname.GetDisplayName(Lastname); }
        }

        public string FullName
        {
            get { return Firstname.GetFullName(Lastname); }
        }

        public string PhotoId { get; set; }

    }
}