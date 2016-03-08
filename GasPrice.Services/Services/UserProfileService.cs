using System.Linq;
using GasPrice.Core.Account;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Services;
using GasPrice.Core.ViewModels.UserAccount;
using Omu.ValueInjecter;

namespace GasPrice.Services.Services
{
    public class UserProfileService : Service<UserAccount>, IUserProfileService
    {
        private readonly IRepositoryAsync<UserAccount> _repository;

        public UserProfileService(IUnitOfWorkAsync unitOfWork, IRepositoryAsync<UserAccount> repository)
            : base(unitOfWork)
        {
            _repository = repository;
        }

        public UserProfileOutput GetProfile(int id)
        {
            var entity = _repository.
                        Query(p => p.UserId == id).
                        Select( x => new
                        {
                            x.UserId,
                            x.Username,
                            x.FirstName,
                            x.LastName,
                            x.Email,
                            x.LastLogin
                        }).
                        SingleOrDefault();

            if (entity != null) return new UserProfileOutput().InjectFrom(entity) as UserProfileOutput;
            Tracing.Warning("[UserAccountService.GetByID] failed to locate account: {0}", id);
            return null;
        }

    }

}
