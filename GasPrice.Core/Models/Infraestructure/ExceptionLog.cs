
using GasPrice.Core.Account;

namespace GasPrice.Core.Models.Infraestructure
{
    #region

    

    #endregion

    public class ExceptionLog : Entity
    {
        public int Id { get; set; }
        public int HResult { get; set; }        
        public string Message { get; set; }
        public string Source { get; set; }
        public string RequestUri { get; set; }
        public string Method { get; set; }
        public string StackTrace { get; set; }
        public int? UserId { get; set; }
        public UserAccount User { get; set; }
    }
}
