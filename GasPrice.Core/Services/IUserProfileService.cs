using GasPrice.Core.Account;
using GasPrice.Core.ViewModels.UserAccount;

namespace GasPrice.Core.Services
{
    public interface IUserProfileService : IService<UserAccount>
    {
        UserProfileOutput GetProfile(int id);
    }
}
