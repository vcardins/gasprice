using GasPrice.Core.Common.Extensions;

namespace GasPrice.Core.Models
{
    public class Person : Entity
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName
        {
            get { return FirstName.GetDisplayName(LastName); }
        }
        public string FullName
        {
            get { return FirstName.GetFullName(LastName); }
        }
    }

}
