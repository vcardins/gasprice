using GasPrice.Core.Account;
using GasPrice.Core.Common.Enums;

namespace GasPrice.Core.Models.Infraestructure
{
    public class Log : Entity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ModelAction Action { get; set; }
        public ModelType? Module { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public int? ObjectId { get; set; }
        public int? UserId { get; set; }
        public UserAccount User { get; set; }
    }
}
