using System.Collections.Generic;
using System.Linq;
using GasPrice.Core.Account;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Services;

namespace GasPrice.Services.Services
{
    public class UserService : Service<UserAccount>, IUserService
    {
        public UserService(IUnitOfWorkAsync unitOfWork) : base(unitOfWork){}

        public IEnumerable<UserAccount> GetUsers()
        {
            var query = Query().Select().Distinct();
            var result = query.Select(s => new UserAccount
                   {
                       UserId = s.UserId,
                       FirstName = s.FirstName,
                       LastName = s.LastName
                   });
            return result;
        }
    }
    
}
