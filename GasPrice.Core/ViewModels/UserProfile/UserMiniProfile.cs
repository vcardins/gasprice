using System.Runtime.Serialization;
using GasPrice.Core.Common.Extensions;

namespace GasPrice.Core.ViewModels.UserProfile
{
    [DataContract]
    public class UserMiniProfile
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataMember]
        public string DisplayName
        {
            get { return FirstName.GetDisplayName(LastName); }
        }
        [DataMember]
        public string PhotoPath { get; set; }
    }
}