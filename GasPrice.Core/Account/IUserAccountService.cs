using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GasPrice.Core.Account.Configuration;
using GasPrice.Core.EventHandling;
using GasPrice.Core.Services;
using GasPrice.Core.ViewModels.UserAccount;

namespace GasPrice.Core.Account
{
    public interface IUserAccountService : IService<UserAccount>
    {
        MembershipRebootConfiguration Configuration { get; set; }
        void AddCommandHandler(ICommandHandler handler);
        void ExecuteCommand(ICommand cmd);
        
        void Update(UserAccount account);
        UserAccount GetByUsername(string username);
        UserAccount GetByEmail(string email);
        UserAccount GetByEmail(string tenant, string email);
        UserAccount GetByID(Guid id);
        UserAccount GetById(int id);
        UserAccount GetByVerificationKey(string key);
        bool UsernameExists(string username);
        bool UsernameExists(string tenant, string username);
        bool EmailExists(string email);
        bool EmailExists(string tenant, string email);
        void CreateAccount(RegisterInputModel model);
        UserAccount CreateAccount(string username, string password, string email, Guid? id = null, DateTime? dateCreated = null);
        UserAccount CreateUserAccount();
        UserAccount CreateAccount(string tenant, string username, string password, string email, Guid? id = null, DateTime? dateCreated = null, UserAccount account = null);
        void RequesUserAccountVerification(Guid accountId);
        void CancelVerification(string key);
        void CancelVerification(string key, out bool accountClosed);
        void DeleteAccount(Guid accountId);
        void CloseAccount(Guid accountId);
        void ReopenAccount(string username, string password);
        void ReopenAccount(string tenant, string username, string password);
        void ReopenAccount(Guid accountId);
        void ReopenAccount(UserAccount account);
        bool Authenticate(string username, string password);
        bool Authenticate(string username, string password, out UserAccount account);
        Task<bool> Authenticate(string username, string password, out UserAccountAuthentication account);
        bool Authenticate(string tenant, string username, string password);
        bool Authenticate(string tenant, string username, string password, out UserAccount account);
        bool AuthenticateWithEmail(string email, string password);
        bool AuthenticateWithEmail(string email, string password, out UserAccount account);
        bool AuthenticateWithEmail(string tenant, string email, string password);
        bool AuthenticateWithEmail(string tenant, string email, string password, out UserAccount account);
        bool AuthenticateWithUsernameOrEmail(string userNameOrEmail, string password, out UserAccount account);
        bool AuthenticateWithUsernameOrEmail(string tenant, string userNameOrEmail, string password, out UserAccount account);
        bool AuthenticateWithCode(Guid accountId, string code);
        bool AuthenticateWithCode(Guid accountId, string code, out UserAccount account);
        void SetIsLoginAllowed(Guid accountId, bool isLoginAllowed);
        void SetRequiresPasswordReset(Guid accountId, bool requiresPasswordReset);
        void SetPassword(Guid accountId, string newPassword);
        void ChangePassword(Guid accountId, string oldPassword, string newPassword);
        void ResetPassword(Guid id);
        void ResetPassword(string email);
        void ResetPassword(string tenant, string email);
        bool ChangePasswordFromResetKey(string key, string newPassword);
        bool ChangePasswordFromResetKey(string key, string newPassword, out UserAccount account);
        void SendUsernameReminder(string email);
        void SendUsernameReminder(string tenant, string email);
        void ChangeUsername(Guid accountId, string newUsername);
        void ChangeEmailRequest(Guid accountId, string newEmail);
        void VerifyEmailFromKey(string key);
        void VerifyEmailFromKey(string key, out UserAccount account);
        void VerifyEmailFromKey(string key, string password);
        void VerifyEmailFromKey(string key, string password, out UserAccount account);
        void SetConfirmedEmail(Guid accountId, string email);
        bool IsPasswordExpired(Guid accountId);
        bool IsPasswordExpired(UserAccount account);
        void AddClaims(Guid accountId, UserClaimCollection claims);
        void RemoveClaims(Guid accountId, UserClaimCollection claims);

        void UpdateClaims(
            Guid accountId,
            UserClaimCollection additions = null,
            UserClaimCollection deletions = null);

        void AddClaim(Guid accountId, string type, string value);
        void RemoveClaim(Guid accountId, string type);
        void RemoveClaim(Guid accountId, string type, string value);
        IEnumerable<Claim> MapClaims(UserAccount account);
        bool EmailExistsOtherThan(UserAccount account, string value);
        bool VerifyHashedPassword(UserAccount account, string value);
    }
}
