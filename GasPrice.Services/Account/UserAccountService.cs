using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GasPrice.Core.Account;
using GasPrice.Core.Account.Configuration;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Account.Events;
using GasPrice.Core.Account.Repository;
using GasPrice.Core.Account.Validation;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Common.Messaging.Enums;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Core.Config.Security;
using GasPrice.Core.Constants;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.EventHandling;
using GasPrice.Core.ViewModels.UserAccount;
using GasPrice.Services.Account.Extensions;
using GasPrice.Services.Account.Messaging.Email;
using GasPrice.Services.Messaging;
using Omu.ValueInjecter;

//using GasPrice.Services.Account.Messaging.Sms;
//var smsFormatter = new MessagingFormatter<UserAccountEvent>(MessagingType.Sms, appInfo);
//Configuration.AddEventHandler(new SmsUserAccountEventsHandler(smsService, appInfo, smsFormatter));
//ISmsService smsService,

namespace GasPrice.Services.Account
{
    public class UserAccountService : Service<UserAccount>, IUserAccountService
    {
        private readonly IUserAccountRepository _userRepository;

        public MembershipRebootConfiguration Configuration { get; set; }

        readonly Lazy<AggregateValidator<UserAccount>> _usernameValidator;
        readonly Lazy<AggregateValidator<UserAccount>> _emailValidator;
        readonly Lazy<AggregateValidator<UserAccount>> _passwordValidator;
        private readonly ICrypto _crypto;

        private AccountManagementStatus? _accountStatus;

        public UserAccountService(IUnitOfWorkAsync unitOfWork,
                                  IEmailService emailService,                                  
                                  IApplicationInformation appInfo,
                                  ISecuritySettings securitySettings,
                                  ICrypto crypto)
            : base(unitOfWork)
        {
            _crypto = crypto;

            Configuration = new MembershipRebootConfiguration(securitySettings);

            _userRepository = new EventBusUserAccountRepository(this, unitOfWork,
             new AggregateEventBus { Configuration.ValidationBus },
             Configuration.EventBus);

            var htmlEmailFormatter = new MessagingFormatter<UserAccountEvent>(MessagingType.Email, appInfo);
            Configuration.AddEventHandler(new EmailUserAccountEventsHandler(emailService, appInfo, htmlEmailFormatter));
            
            _usernameValidator = new Lazy<AggregateValidator<UserAccount>>(() =>
            {
                var val = new AggregateValidator<UserAccount>();
                if (!Configuration.EmailIsUsername)
                {
                    val.Add(UserAccountValidation.UsernameDoesNotContainAtSign);
                    val.Add(UserAccountValidation.UsernameCanOnlyStartOrEndWithLetterOrDigit);
                    val.Add(UserAccountValidation.UsernameOnlyContainsValidCharacters);
                    val.Add(UserAccountValidation.UsernameOnlySingleInstanceOfSpecialCharacters);
                }
                val.Add(UserAccountValidation.UsernameMustNotAlreadyExist);
                val.Add(Configuration.UsernameValidator);
                return val;
            });

            _emailValidator = new Lazy<AggregateValidator<UserAccount>>(() =>
            {
                var val = new AggregateValidator<UserAccount>
                {
                    UserAccountValidation.EmailIsRequiredIfRequireAccountVerificationEnabled,
                    UserAccountValidation.EmailIsValidFormat,
                    UserAccountValidation.EmailMustNotAlreadyExist,
                    Configuration.EmailValidator
                };
                return val;
            });

            _passwordValidator = new Lazy<AggregateValidator<UserAccount>>(() =>
            {
                var val = new AggregateValidator<UserAccount>
                {
                    UserAccountValidation.PasswordMustBeDifferentThanCurrent,
                    Configuration.PasswordValidator
                };
                return val;
            });
        }

        protected void ValidateUsername(UserAccount account, string value)
        {
            var result = _usernameValidator.Value.Validate(this, account, value);
            if (result != null && result != ValidationResult.Success)
            {
                Tracing.Error("ValidateUsername failed: " + result.ErrorMessage);
                throw new ValidationException(result.ErrorMessage);
            }
        }
        protected void ValidatePassword(UserAccount account, string value)
        {
            // null is allowed (e.g. for external providers)
            if (value == null) return;

            var result = _passwordValidator.Value.Validate(this, account, value);
            if (result != null && result != ValidationResult.Success)
            {
                Tracing.Error("ValidatePassword failed: " + result.ErrorMessage);
                throw new ValidationException(result.ErrorMessage);
            }
        }
        protected void ValidateEmail(UserAccount account, string value)
        {
            var result = _emailValidator.Value.Validate(this, account, value);
            if (result != null && result != ValidationResult.Success)
            {
                Tracing.Error("ValidateEmail failed: " + result.ErrorMessage);
                throw new ValidationException(result.ErrorMessage);
            }
        }

        internal protected virtual void UpdateInternal(UserAccount account)
        {
            if (account == null)
            {
                Tracing.Error("[UserAccountService.UpdateInternal] called -- failed null account");
                throw new ArgumentNullException("account");
            }

            Tracing.Information("[UserAccountService.UpdateInternal] called for account: {0}", account.ID);
            _userRepository.Update(account);

        }

        public void Update(UserAccount account)
        {
            if (account == null)
            {
                Tracing.Error("[UserAccountService.Update] called -- failed null account");
                throw new ArgumentNullException("account");
            }

            Tracing.Information("[UserAccountService.Update] called for account: {0}", account.ID);

            account.LastUpdated = UtcNow;

            UpdateInternal(account);
        }

        public virtual UserAccount GetByUsername(string username)
        {
            return GetByUsername(null, username);
        }

        public virtual UserAccount GetByUsername(string tenant, string username)
        {
            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.GetByUsername] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.GetByUsername] called for tenant: {0}, username: {1}", tenant, username);

            if (!Configuration.UsernamesUniqueAcrossTenants && String.IsNullOrWhiteSpace(tenant)) return null;
            if (String.IsNullOrWhiteSpace(username)) return null;

            var account = Configuration.UsernamesUniqueAcrossTenants ?
                _userRepository.GetByUsername(username) :
                _userRepository.GetByUsername(tenant, username);

            if (account == null)
            {
                Tracing.Warning("[UserAccountService.GetByUsername] failed to locate account: {0}, {1}", tenant, username);
            }
            return account;
        }

        public virtual UserAccount GetByEmail(string email)
        {
            return GetByEmail(null, email);
        }

        public virtual UserAccount GetByEmail(string tenant, string email)
        {
            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.GetByEmail] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.GetByEmail] called for tenant: {0}, email: {1}", tenant, email);

            if (String.IsNullOrWhiteSpace(tenant)) return null;
            if (String.IsNullOrWhiteSpace(email)) return null;

            var account = _userRepository.GetByEmail(tenant, email);
            if (account == null)
            {
                Tracing.Warning("[UserAccountService.GetByEmail] failed to locate account: {0}, {1}", tenant, email);
            }
            return account;
        }

        public virtual UserAccount GetByID(Guid id)
        {
            Tracing.Information("[UserAccountService.GetByID] called for id: {0}", id);

            var account = _userRepository.GetByID(id);
            if (account == null)
            {
                Tracing.Warning("[UserAccountService.GetByID] failed to locate account: {0}", id);
            }
            return account;
        }

        public UserAccount GetById(int id)
        {
            Tracing.Information("[UserAccountService.GetByID] called for id: {0}", id);

            var account = _userRepository.GetById(id);
            if (account == null)
            {
                Tracing.Warning("[UserAccountService.GetByID] failed to locate account: {0}", id);
            }
            return account;
        }
    
        private static AccessRole GetRoles(UserAccount user)
        {
            var claims = user.GetClaimValues(ClaimTypes.Role).ToList();
            if (!claims.Any()) { return new AccessRole { Name = UserRoleEnum.Reader }; }

            AccessRole role = null;
            foreach (var r in claims)
            {
                UserRoleEnum result;
                if (!Enum.TryParse(r, out result)) continue;
                role = new AccessRole { Name = result };
                break;
            }
            return role;
        }

        public virtual UserAccount GetByVerificationKey(string key)
        {
            Tracing.Information("[UserAccountService.GetByVerificationKey] called for key: {0}", key);

            if (String.IsNullOrWhiteSpace(key)) return null;
            
            key = _crypto.Hash(key);

            var account = _userRepository.GetByVerificationKey(key);
            if (account == null)
            {
                Tracing.Warning("[UserAccountService.GetByVerificationKey] failed to locate account: {0}", key);
            }
            return account;
        }

        public virtual bool UsernameExists(string username)
        {
            return UsernameExists(null, username);
        }

        public virtual bool UsernameExists(string tenant, string username)
        {
            Tracing.Information("[UserAccountService.UsernameExists] called for tenant: {0}, username; {1}", tenant, username);

            if (String.IsNullOrWhiteSpace(username)) return false;

            if (Configuration.UsernamesUniqueAcrossTenants)
            {
                return _userRepository.GetByUsername(username) != null;
            }
            else
            {
                if (!Configuration.MultiTenant)
                {
                    Tracing.Verbose("[UserAccountService.UsernameExists] applying default tenant");
                    tenant = Configuration.DefaultTenant;
                }

                if (String.IsNullOrWhiteSpace(tenant)) return false;

                return _userRepository.GetByUsername(tenant, username) != null;
            }
        }

        public virtual bool EmailExists(string email)
        {
            return EmailExists(null, email);
        }

        public virtual bool EmailExists(string tenant, string email)
        {
            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.EmailExists] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.EmailExists] called for tenant: {0}, email; {1}", tenant, email);

            if (String.IsNullOrWhiteSpace(tenant)) return false;
            if (String.IsNullOrWhiteSpace(email)) return false;

            return _userRepository.GetByEmail(tenant, email) != null;
        }      

        public bool EmailExistsOtherThan(UserAccount account, string email)
        {
            if (account == null) throw new ArgumentNullException("account");

            Tracing.Information("[UserAccountService.EmailExistsOtherThan] called for account id: {0}, email; {1}", account.ID, email);

            if (String.IsNullOrWhiteSpace(email)) return false;

            var acct2 = _userRepository.GetByEmail(account.Tenant, email);
            if (acct2 != null)
            {
                return account.ID != acct2.ID;
            }
            return false;
        }

        public virtual void CreateAccount(RegisterInputModel model)
        {            
            string tenant = null;

            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying email is username");
                model.Username = model.Email;
            }

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.CreateAccount] called: {0}, {1}, {2}", tenant, model.Username, model.Email);

            var claim = new UserClaim(ClaimTypes.Role, "User") {ObjectState = ObjectState.Added};
            var account = new UserAccount
            {
                ObjectState = ObjectState.Added, 
                Created = DateTime.Now,
                ClaimCollection = new UserClaimCollection { claim }
            };
            account.InjectFrom(model);

            Init(account, tenant, model.Username, model.Password, model.Email);

            ValidateEmail(account, model.Email);
            ValidateUsername(account, model.Username);
            //ValidatePassword(account, model.Password);

            Tracing.Verbose("[UserAccountService.CreateAccount] success");
            _userRepository.Add(account);          
        }

        public virtual UserAccount CreateAccount(string username, string password, string email, Guid? id = null, DateTime? dateCreated = null)
        {
            return CreateAccount(null, username, password, email, id, dateCreated);
        }

        public virtual UserAccount CreateUserAccount()
        {
            return _userRepository.Create();
        }

        public virtual UserAccount CreateAccount(string tenant, string username, string password, string email, Guid? id = null, DateTime? dateCreated = null, UserAccount account = null)
        {
            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying email is username");
                username = email;
            }

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.CreateAccount] called: {0}, {1}, {2}", tenant, username, email);

            account = account ?? CreateUserAccount();
            Init(account, tenant, username, password, email, id, dateCreated);

            ValidateEmail(account, email);
            ValidateUsername(account, username);
            ValidatePassword(account, password);

            Tracing.Verbose("[UserAccountService.CreateAccount] success");

            _userRepository.Add(account);

            return account;
        }
     
        protected void Init(UserAccount account, string tenant, string username, string password, string email, Guid? id = null, 
            DateTime? dateCreated = null)
        {
            Tracing.Information("[UserAccountService.Init] called");

            if (account == null)
            {
                Tracing.Error("[UserAccountService.Init] failed -- null account");
                throw new ArgumentNullException("account");
            }

            if (String.IsNullOrWhiteSpace(tenant))
            {
                Tracing.Error("[UserAccountService.Init] failed -- no tenant");
                throw new ArgumentNullException("tenant");
            }

            if (String.IsNullOrWhiteSpace(username))
            {
                Tracing.Error("[UserAccountService.Init] failed -- no username");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.UsernameRequired));
            }

            if (password != null && String.IsNullOrWhiteSpace(password.Trim()))
            {
                Tracing.Error("[UserAccountService.Init] failed -- no password");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.PasswordRequired));
            }

            if (account.ID != Guid.Empty)
            {
                Tracing.Error("[UserAccountService.Init] failed -- ID already assigned");
                throw new Exception("Can't call Init if UserAccount is already assigned an ID");
            }

            var now = UtcNow;
            if (dateCreated > now)
            {
                Tracing.Error("[UserAccountService.Init] failed -- date created in the future");
                throw new Exception("dateCreated can't be in the future");
            }

            account.ID = id ?? Guid.NewGuid();
            account.Tenant = tenant;
            account.Username = username;
            account.Email = email;
            account.Created = dateCreated ?? now;
            account.LastUpdated = now;
            account.HashedPassword = password != null ?
                _crypto.HashPassword(password, Configuration.PasswordHashingIterationCount) : null;
            account.PasswordChanged = password != null ? now : (DateTime?)null;
            account.IsAccountVerified = false;

            account.IsLoginAllowed = Configuration.AllowLoginAfterAccountCreation;
            Tracing.Verbose("[UserAccountService.CreateAccount] SecuritySettings.AllowLoginAfterAccountCreation is set to: {0}", account.IsLoginAllowed);

            string key = null;
            if (!String.IsNullOrWhiteSpace(account.Email))
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] Email was provided, so creating email verification request");
                key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: account.Email);
            }

            AddEvent(new AccountCreatedEvent
            {
                Account = account, 
                InitialPassword = password, 
                VerificationKey = key
            });
        }

        public virtual void RequesUserAccountVerification(Guid accountId)
        {
            Tracing.Information("[UserAccountService.RequestAccountVerification] called for account id: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null)
            {
                Tracing.Error("[UserAccountService.RequestAccountVerification] invalid account id");
                throw new Exception("Invalid Account ID");
            }

            if (String.IsNullOrWhiteSpace(account.Email))
            {
                Tracing.Error("[UserAccountService.RequestAccountVerification] email empty");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.EmailRequired));
            }

            if (account.IsAccountVerified)
            {
                Tracing.Error("[UserAccountService.RequestAccountVerification] account already verified");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.AccountAlreadyVerified));
            }

            Tracing.Verbose("[UserAccountService.RequestAccountVerification] creating a new verification key");
            var key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: account.Email);
            AddEvent(new EmailChangeRequestedEvent { Account = account, NewEmail = account.Email, VerificationKey = key });

            UpdateInternal(account);
        }

        public virtual void CancelVerification(string key)
        {
            bool closed;
            CancelVerification(key, out closed);
        }

        public virtual void CancelVerification(string key, out bool accountClosed)
        {
            Tracing.Information("[UserAccountService.CancelVerification] called: {0}", key);

            accountClosed = false;

            if (String.IsNullOrWhiteSpace(key))
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- key null");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            var account = GetByVerificationKey(key);
            if (account == null)
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- account not found from key");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            if (account.VerificationPurpose == null)
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- no purpose");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            var result = _crypto.VerifyHash(key, account.VerificationKey);
            if (!result)
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- key verification failed");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            if (account.VerificationPurpose == VerificationKeyPurpose.ChangeEmail &&
                account.IsNew())
            {
                Tracing.Verbose("[UserAccountService.CancelVerification] account is new (deleting account)");
                // if last login is null then they've never logged in so we can delete the account
                DeleteAccount(account);
                accountClosed = true;
            }
            else
            {
                Tracing.Verbose("[UserAccountService.CancelVerification] account is not new (canceling clearing verification key)");
                ClearVerificationKey(account);
                UpdateInternal(account);
            }

            Tracing.Verbose("[UserAccountService.CancelVerification] succeeded");
        }

        public virtual void DeleteAccount(Guid accountId)
        {
            Tracing.Information("[UserAccountService.DeleteAccount] called: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            DeleteAccount(account);
        }

        protected virtual void DeleteAccount(UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            Tracing.Verbose("[UserAccountService.DeleteAccount] marking account as closed: {0}", account.ID);

            CloseAccount(account);
            Update(account);

            if (Configuration.AllowAccountDeletion || account.IsNew())
            {
                Tracing.Verbose("[UserAccountService.DeleteAccount] removing account record: {0}", account.ID);
                _userRepository.Remove(account);
            }
        }

        public virtual void CloseAccount(Guid accountId)
        {
            Tracing.Information("[UserAccountService.CloseAccount] called: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            CloseAccount(account);
        }

        protected virtual void CloseAccount(UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            Tracing.Information("[UserAccountService.CloseAccount] called for accountId: {0}", account.ID);

            ClearVerificationKey(account);

            if (!account.IsAccountClosed)
            {
                Tracing.Verbose("[UserAccountService.CloseAccount] success");

                account.IsAccountClosed = true;
                account.AccountClosed = UtcNow;

                AddEvent(new AccountClosedEvent { Account = account });
            }
            else
            {
                Tracing.Warning("[UserAccountService.CloseAccount] account already closed");
            }
        }

        public virtual void ReopenAccount(string username, string password)
        {
            ReopenAccount(null, username, password);
        }

        public virtual void ReopenAccount(string tenant, string username, string password)
        {
            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.ReopenAccount] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.ReopenAccount] called: {0}, {1}", tenant, username);

            var account = GetByUsername(tenant, username);
            if (account == null)
            {
                Tracing.Error("[UserAccountService.ReopenAccount] invalid account");
                _accountStatus = AccountManagementStatus.InvalidCredentials;
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidUsername));
            }

            if (!VerifyPassword(account, password))
            {
                Tracing.Error("[UserAccountService.ReopenAccount] invalid password");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidPassword));
            }

            ReopenAccount(account);
        }

        public virtual void ReopenAccount(Guid accountId)
        {
            Tracing.Information("[UserAccountService.ReopenAccount] called: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            ReopenAccount(account);
        }

        public virtual void ReopenAccount(UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            if (!account.IsAccountClosed)
            {
                Tracing.Warning("[UserAccountService.ReopenAccount] account is not closed");
                return;
            }

            if (String.IsNullOrWhiteSpace(account.Email))
            {
                Tracing.Error("[UserAccountService.ReopenAccount] no email to confirm reopen request");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.ReopenErrorNoEmail));
            }

            // this will require the user to confirm via email before logging in
            account.IsAccountVerified = false;
            ClearVerificationKey(account);
            var key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: account.Email);
            AddEvent(new AccountReopenedEvent { Account = account, VerificationKey = key });

            account.IsAccountClosed = false;
            account.AccountClosed = null;

            Update(account);

            Tracing.Verbose("[UserAccountService.ReopenAccount] success");
        }

        public virtual bool Authenticate(string username, string password)
        {
            return Authenticate(null, username, password);
        }

        public Task<bool> Authenticate(string username, string password, out UserAccountAuthentication accountAuthentication)
        {
            UserAccount account;
            var isAuthenticated = Authenticate(null, username, password, out account);
            accountAuthentication = new UserAccountAuthentication { Status = _accountStatus };
            if (!isAuthenticated)
            {
                return Task.FromResult(false);
            }

            accountAuthentication.InjectFrom(account);
            accountAuthentication.Claims = account.Claims.Select(uc => new Claim(uc.Type, uc.Value)).ToList();

            return Task.FromResult(true);

        }

        public virtual bool Authenticate(string username, string password, out UserAccount account)
        {
            return Authenticate(null, username, password, out account);
        }

        public virtual bool Authenticate(string tenant, string username, string password)
        {
            UserAccount account;
            return Authenticate(tenant, username, password, out account);
        }
        public virtual bool Authenticate(string tenant, string username, string password, out UserAccount account)
        {
            account = null;

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.Authenticate] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.Authenticate] called: {0}, {1}", tenant, username);

            if (!Configuration.UsernamesUniqueAcrossTenants && String.IsNullOrWhiteSpace(tenant)) return false;
            if (String.IsNullOrWhiteSpace(username)) return false;
            if (String.IsNullOrWhiteSpace(password))
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- empty password");
                return false;
            }

            account = GetByUsername(tenant, username);
            if (account != null) return Authenticate(account, password);
            _accountStatus = AccountManagementStatus.InvalidCredentials;
            return false;
        }

        public virtual bool AuthenticateWithEmail(string email, string password)
        {
            return AuthenticateWithEmail(null, email, password);
        }
        public virtual bool AuthenticateWithEmail(string email, string password, out UserAccount account)
        {
            return AuthenticateWithEmail(null, email, password, out account);
        }

        public virtual bool AuthenticateWithEmail(string tenant, string email, string password)
        {
            UserAccount account;
            return AuthenticateWithEmail(null, email, password, out account);
        }
        public virtual bool AuthenticateWithEmail(string tenant, string email, string password, out UserAccount account)
        {
            account = null;

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.AuthenticateWithEmail] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.AuthenticateWithEmail] called: {0}, {1}", tenant, email);

            if (String.IsNullOrWhiteSpace(tenant)) return false;
            if (String.IsNullOrWhiteSpace(email)) return false;
            if (String.IsNullOrWhiteSpace(password))
            {
                Tracing.Error("[UserAccountService.AuthenticateWithEmail] failed -- empty password");
                return false;
            }

            account = GetByEmail(tenant, email);
            if (account == null) return false;

            return Authenticate(account, password);
        }

        public virtual bool AuthenticateWithUsernameOrEmail(string userNameOrEmail, string password, out UserAccount account)
        {
            return AuthenticateWithUsernameOrEmail(null, userNameOrEmail, password, out account);
        }

        public virtual bool AuthenticateWithUsernameOrEmail(string tenant, string userNameOrEmail, string password, out UserAccount account)
        {
            account = null;

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.AuthenticateWithUsernameOrEmail] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.AuthenticateWithUsernameOrEmail] called {0}, {1}", tenant, userNameOrEmail);

            if (String.IsNullOrWhiteSpace(tenant)) return false;
            if (String.IsNullOrWhiteSpace(userNameOrEmail)) return false;
            if (String.IsNullOrWhiteSpace(password))
            {
                Tracing.Error("[UserAccountService.AuthenticateWithUsernameOrEmail] failed -- empty password");
                return false;
            }

            if (!Configuration.EmailIsUsername && userNameOrEmail.Contains("@"))
            {
                Tracing.Verbose("[UserAccountService.AuthenticateWithUsernameOrEmail] email detected");
                return AuthenticateWithEmail(tenant, userNameOrEmail, password, out account);
            }
            else
            {
                Tracing.Verbose("[UserAccountService.AuthenticateWithUsernameOrEmail] username detected");
                return Authenticate(tenant, userNameOrEmail, password, out account);
            }
        }

        protected virtual bool Authenticate(UserAccount account, string password)
        {
            Tracing.Verbose("[UserAccountService.Authenticate] for account: {0}", account.ID);

            var result = VerifyPassword(account, password);

            if (result)
            {
                try
                {
                    if (!account.IsLoginAllowed)
                    {
                        Tracing.Error("[UserAccountService.Authenticate] failed -- account not allowed to login");
                        AddEvent(new AccountLockedEvent { Account = account });
                        _accountStatus = AccountManagementStatus.LoginNotAllowed;
                        return false;
                    }

                    if (account.IsAccountClosed)
                    {
                        Tracing.Error("[UserAccountService.Authenticate] failed -- account closed");
                        AddEvent(new InvalidAccountEvent { Account = account });
                        _accountStatus = AccountManagementStatus.AccountNotVerified;
                        return false;
                    }

                    if (Configuration.RequireAccountVerification && !account.IsAccountVerified)
                    {
                        Tracing.Error("[UserAccountService.Authenticate] failed -- account not verified");
                        AddEvent(new AccountNotVerifiedEvent { Account = account });
                        _accountStatus = AccountManagementStatus.AccountNotVerified;
                        result = false;
                    }

                    Tracing.Verbose("[UserAccountService.Authenticate] authentication success");
                    account.LastLogin = UtcNow;
                    AddEvent(new SuccessfulPasswordLoginEvent { Account = account });

                    Tracing.Verbose("[UserAccountService.Authenticate] setting two factor auth status to None");
                }
                finally
                {
                    UpdateInternal(account);
                }
            }

            Tracing.Verbose("[UserAccountService.Authenticate] authentication outcome: {0}", result ? "Successful Login" : "Failed Login");

            return result;
        }

        protected virtual bool VerifyPassword(UserAccount account, string password)
        {
            Tracing.Information("[UserAccountService.VerifyPassword] called for accountId: {0}", account.ID);

            if (String.IsNullOrWhiteSpace(password))
            {
                Tracing.Error("[UserAccountService.VerifyPassword] failed -- no password");
                return false;
            }

            if (!account.HasPassword())
            {
                Tracing.Error("[UserAccountService.VerifyPassword] failed -- account does not have a password");
                return false;
            }

            try
            {
                if (CheckHasTooManyRecentPasswordFailures(account))
                {
                    Tracing.Error("[UserAccountService.VerifyPassword] failed -- account in lockout due to failed login attempts");
                    AddEvent(new TooManyRecentPasswordFailuresEvent { Account = account });
                    return false;
                }

                var valid = VerifyHashedPassword(account, password);
                if (valid)
                {
                    Tracing.Verbose("[UserAccountService.VerifyPassword] success");
                    account.FailedLoginCount = 0;
                }
                else
                {
                    Tracing.Error("[UserAccountService.VerifyPassword] failed -- invalid password");
                    _accountStatus = AccountManagementStatus.InvalidCredentials;
                    RecordInvalidLoginAttempt(account);
                    AddEvent(new InvalidPasswordEvent { Account = account });
                }

                return valid;
            }
            finally
            {
                UpdateInternal(account);
            }
        }

        public bool VerifyHashedPassword(UserAccount account, string password)
        {
            if (!account.HasPassword()) return false;
            return _crypto.VerifyHashedPassword(account.HashedPassword, password);
        }

        protected virtual bool CheckHasTooManyRecentPasswordFailures(UserAccount account)
        {
            var result = false;
            if (Configuration.AccountLockoutFailedLoginAttempts <= account.FailedLoginCount)
            {
                result = account.LastFailedLogin >= UtcNow.Subtract(Configuration.AccountLockoutDuration);
                if (!result)
                {
                    // if we're past the lockout window, then reset to zero
                    account.FailedLoginCount = 0;
                }
            }

            if (result)
            {
                account.FailedLoginCount++;
            }

            return result;
        }

        protected virtual void RecordInvalidLoginAttempt(UserAccount account)
        {
            account.LastFailedLogin = UtcNow;
            if (account.FailedLoginCount <= 0)
            {
                account.FailedLoginCount = 1;
            }
            else
            {
                account.FailedLoginCount++;
            }
        }

        public virtual bool AuthenticateWithCode(Guid accountId, string code)
        {
            UserAccount account;
            return AuthenticateWithCode(accountId, code, out account);
        }

        public virtual bool AuthenticateWithCode(Guid accountId, string code, out UserAccount account)
        {
            Tracing.Information("[UserAccountService.AuthenticateWithCode] called {0}", accountId);

            account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            Tracing.Information("[UserAccountService.AuthenticateWithCode] called for accountId: {0}", account.ID);

            if (code == null)
            {
                Tracing.Error("[UserAccountService.AuthenticateWithCode] failed - null code");
                return false;
            }

            if (account.IsAccountClosed)
            {
                Tracing.Error("[UserAccountService.AuthenticateWithCode] failed -- account closed");
                return false;
            }

            if (!account.IsLoginAllowed)
            {
                Tracing.Error("[UserAccountService.AuthenticateWithCode] failed -- login not allowed");
                return false;
            }
            account.LastLogin = UtcNow;

            Tracing.Verbose("[UserAccountService.AuthenticateWithCode] success ");

            UpdateInternal(account);

            return true;
        }


        public virtual void SetIsLoginAllowed(Guid accountId, bool isLoginAllowed)
        {
            Tracing.Information("[UserAccountService.SetIsLoginAllowed] called: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            account.IsLoginAllowed = isLoginAllowed;

            Tracing.Verbose("[UserAccountService.SetIsLoginAllowed] success");

            Update(account);
        }

        public virtual void SetRequiresPasswordReset(Guid accountId, bool requiresPasswordReset)
        {
            Tracing.Information("[UserAccountService.SetRequiresPasswordReset] called: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            account.RequiresPasswordReset = requiresPasswordReset;

            Tracing.Verbose("[UserAccountService.SetRequiresPasswordReset] success");

            Update(account);
        }

        public virtual void SetPassword(Guid accountId, string newPassword)
        {
            Tracing.Information("[UserAccountService.SetPassword] called: {0}", accountId);

            if (String.IsNullOrWhiteSpace(newPassword))
            {
                Tracing.Error("[UserAccountService.SetPassword] failed -- null newPassword");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidNewPassword));
            }

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            ValidatePassword(account, newPassword);

            SetPassword(account, newPassword);

            // setting failed count to zero here (and not in SetPassword(account, newPassword))
            // since this API is meant to be an admin-API to reset user's passwords
            account.FailedLoginCount = 0;

            Update(account);

            Tracing.Verbose("[UserAccountService.SetPassword] success");
        }

        public virtual void ChangePassword(Guid accountId, string oldPassword, string newPassword)
        {
            Tracing.Information("[UserAccountService.ChangePassword] called: {0}", accountId);

            if (String.IsNullOrWhiteSpace(oldPassword))
            {
                Tracing.Error("[UserAccountService.ChangePassword] failed -- null oldPassword");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidOldPassword));
            }
            if (String.IsNullOrWhiteSpace(newPassword))
            {
                Tracing.Error("[UserAccountService.ChangePassword] failed -- null newPassword");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidNewPassword));
            }

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            ValidatePassword(account, newPassword);

            if (!VerifyPassword(account, oldPassword))
            {
                Tracing.Error("[UserAccountService.ChangePassword] failed -- failed authN");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidOldPassword));
            }

            Tracing.Verbose("[UserAccountService.ChangePassword] success");

            SetPassword(account, newPassword);
            Update(account);
        }

        public virtual void ResetPassword(Guid id)
        {
            var account = GetByID(id);
            if (account == null) throw new ArgumentException("Invalid ID");

            ResetPassword(account);
            UpdateInternal(account);
        }

        public virtual void ResetPassword(string email)
        {
            ResetPassword(null, email);
        }

        public virtual void ResetPassword(string tenant, string email)
        {
            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.ResetPassword] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.ResetPassword] called: {0}, {1}", tenant, email);

            if (String.IsNullOrWhiteSpace(tenant))
            {
                Tracing.Error("[UserAccountService.ResetPassword] failed -- null tenant");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidTenant));
            }
            if (String.IsNullOrWhiteSpace(email))
            {
                Tracing.Error("[UserAccountService.ResetPassword] failed -- null email");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidEmail));
            }

            var account = GetByEmail(tenant, email);
            if (account == null) throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidEmail));

            Tracing.Verbose("[UserAccountService.ResetPassword] success");

            ResetPassword(account);
            UpdateInternal(account);
        }

        public virtual bool ChangePasswordFromResetKey(string key, string newPassword)
        {
            UserAccount account;
            return ChangePasswordFromResetKey(key, newPassword, out account);
        }

        public virtual bool ChangePasswordFromResetKey(string key, string newPassword, out UserAccount account)
        {
            Tracing.Information("[UserAccountService.ChangePasswordFromResetKey] called: {0}", key);

            if (String.IsNullOrWhiteSpace(key))
            {
                Tracing.Error("[UserAccountService.ChangePasswordFromResetKey] failed -- null key");
                account = null;
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            if (String.IsNullOrWhiteSpace(newPassword))
            {
                Tracing.Error("[UserAccountService.ChangePasswordFromResetKey] failed -- newPassword empty/null");
                account = null;
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.PasswordRequired));
            }

            account = GetByVerificationKey(key);
            if (account == null) return false;

            ValidatePassword(account, newPassword);

            if (!account.IsAccountVerified)
            {
                Tracing.Error("[UserAccountService.ChangePasswordFromResetKey] failed -- account not verified");
                return false;
            }

            if (!IsVerificationKeyValid(account, VerificationKeyPurpose.ResetPassword, key))
            {
                Tracing.Error("[UserAccountService.ChangePasswordFromResetKey] failed -- key verification failed");
                return false;
            }

            Tracing.Verbose("[UserAccountService.ChangePasswordFromResetKey] success");

            ClearVerificationKey(account);
            SetPassword(account, newPassword);
            Update(account);

            return true;
        }


        public virtual void SendUsernameReminder(string email)
        {
            SendUsernameReminder(null, email);
        }

        public virtual void SendUsernameReminder(string tenant, string email)
        {
            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.SendUsernameReminder] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.SendUsernameReminder] called: {0}, {1}", tenant, email);

            if (String.IsNullOrWhiteSpace(tenant))
            {
                Tracing.Error("[UserAccountService.SendUsernameReminder] failed -- null tenant");
                throw new ArgumentNullException("tenant");
            }
            if (String.IsNullOrWhiteSpace(email))
            {
                Tracing.Error("[UserAccountService.SendUsernameReminder] failed -- null email");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidEmail));
            }

            var account = GetByEmail(tenant, email);
            if (account == null) throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidEmail));

            if (!account.IsAccountVerified)
            {
                Tracing.Error("[UserAccountService.SendUsernameReminder] failed -- account not verified");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.AccountNotVerified));
            }

            Tracing.Verbose("[UserAccountService.SendUsernameReminder] success");

            AddEvent(new UsernameReminderRequestedEvent { Account = account });

            UpdateInternal(account);
        }

        public virtual void ChangeUsername(Guid accountId, string newUsername)
        {
            Tracing.Information("[UserAccountService.ChangeUsername] called account id: {0}, new username: {1}", accountId, newUsername);

            if (Configuration.EmailIsUsername)
            {
                Tracing.Error("[UserAccountService.ChangeUsername] failed -- SecuritySettings.EmailIsUsername is true, use ChangeEmail API instead");
                throw new Exception("EmailIsUsername is enabled in SecuritySettings -- use ChangeEmail APIs instead.");
            }

            if (String.IsNullOrWhiteSpace(newUsername))
            {
                Tracing.Error("[UserAccountService.ChangeUsername] failed -- null newUsername");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidUsername));
            }

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            ValidateUsername(account, newUsername);

            Tracing.Verbose("[UserAccountService.ChangeUsername] success");

            account.Username = newUsername;

            AddEvent(new UsernameChangedEvent { Account = account });

            Update(account);
        }

        public virtual void ChangeEmailRequest(Guid accountId, string newEmail)
        {
            Tracing.Information("[UserAccountService.ChangeEmailRequest] called: {0}, {1}", accountId, newEmail);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            ValidateEmail(account, newEmail);

            var oldEmail = account.Email;

            Tracing.Verbose("[UserAccountService.ChangeEmailRequest] creating a new reset key");
            var key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: newEmail);

            if (!Configuration.RequireAccountVerification)
            {
                Tracing.Verbose("[UserAccountService.ChangeEmailRequest] RequireAccountVerification false, changing email");
                account.IsAccountVerified = false;
                account.Email = newEmail;
                AddEvent(new EmailChangedEvent { Account = account, OldEmail = oldEmail, VerificationKey = key });
                Update(account);
            }
            else
            {
                Tracing.Verbose("[UserAccountService.ChangeEmailRequest] RequireAccountVerification true, sending changing email");
                AddEvent(new EmailChangeRequestedEvent { Account = account, OldEmail = oldEmail, NewEmail = newEmail, VerificationKey = key });
                UpdateInternal(account);
            }

            Tracing.Verbose("[UserAccountService.ChangeEmailRequest] success");
        }

        public virtual void VerifyEmailFromKey(string key)
        {
            UserAccount account;
            VerifyEmailFromKey(key, out account);
        }

        public virtual void VerifyEmailFromKey(string key, out UserAccount account)
        {
            VerifyEmailFromKey(key, null, out account);
        }

        public virtual void VerifyEmailFromKey(string key, string password)
        {
            UserAccount account;
            VerifyEmailFromKey(key, password, out account);
        }

        public virtual void VerifyEmailFromKey(string key, string password, out UserAccount account)
        {
            Tracing.Information("[UserAccountService.VerifyEmailFromKey] called");

            if (String.IsNullOrWhiteSpace(key))
            {
                Tracing.Error("[UserAccountService.VerifyEmailFromKey] failed -- null key");
                account = null;
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            account = GetByVerificationKey(key);
            if (account == null)
            {
                Tracing.Error("[UserAccountService.VerifyEmailFromKey] failed -- invalid key");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            Tracing.Information("[UserAccountService.VerifyEmailFromKey] account located: id: {0}", account.ID);

            if (!IsVerificationKeyValid(account, VerificationKeyPurpose.ChangeEmail, key))
            {
                Tracing.Error("[UserAccountService.VerifyEmailFromKey] failed -- key verification failed");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            //if (account.HasPassword())
            //{
            //    if (String.IsNullOrWhiteSpace(password))
            //    {
            //        Tracing.Error("[UserAccountService.VerifyEmailFromKey] failed -- null password");
            //        throw new ValidationException(GetValidationMessage(AppConstants.ValidationMessages.InvalidPassword));    
            //    }
            //    if (!VerifyPassword(account, password))
            //    {
            //        Tracing.Error("[UserAccountService.VerifyEmailFromKey] failed -- authN failed");
            //        throw new ValidationException(GetValidationMessage(AppConstants.ValidationMessages.InvalidPassword));
            //    }    
            //}
            

            if (String.IsNullOrWhiteSpace(account.VerificationStorage))
            {
                Tracing.Verbose("[UserAccountService.VerifyEmailFromKey] failed -- verification storage empty");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidKey));
            }

            // one last check
            ValidateEmail(account, account.VerificationStorage);

            account.Email = account.VerificationStorage;
            account.IsAccountVerified = true;
            account.LastLogin = UtcNow;

            ClearVerificationKey(account);

            AddEvent(new EmailVerifiedEvent { Account = account });

            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.VerifyEmailFromKey] security setting EmailIsUsername is true and AllowEmailChangeWhenEmailIsUsername is true, so changing username: {0}, to: {1}", account.Username, account.Email);
                account.Username = account.Email;
            }

            Update(account);

            Tracing.Verbose("[UserAccountService.VerifyEmailFromKey] success");
        }

        public virtual void SetConfirmedEmail(Guid accountId, string email)
        {
            Tracing.Information("[UserAccountService.SetConfirmedEmail] called: {0}, {1}", accountId, email);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            ValidateEmail(account, email);

            account.IsAccountVerified = true;
            account.Email = email;

            ClearVerificationKey(account);

            AddEvent(new EmailVerifiedEvent { Account = account });

            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.SetConfirmedEmail] security setting EmailIsUsername is true and AllowEmailChangeWhenEmailIsUsername is true, so changing username: {0}, to: {1}", account.Username, account.Email);
                account.Username = account.Email;
            }

            Update(account);

            Tracing.Verbose("[UserAccountService.SetConfirmedEmail] success");
        }


        public virtual bool IsPasswordExpired(Guid accountId)
        {
            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            return IsPasswordExpired(account);
        }

        public virtual bool IsPasswordExpired(UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            Tracing.Information("[UserAccountService.IsPasswordExpired] called: {0}", account.ID);

            if (Configuration.PasswordResetFrequency <= 0)
            {
                Tracing.Verbose("[UserAccountService.PasswordResetFrequency ] PasswordResetFrequency not set, returning false");
                return false;
            }

            if (!account.HasPassword())
            {
                Tracing.Verbose("[UserAccountService.PasswordResetFrequency ] HashedPassword is null, returning false");
                return false;
            }

            if (account.PasswordChanged == null)
            {
                Tracing.Warning("[UserAccountService.PasswordResetFrequency ] PasswordChanged is null, returning false");
                return false;
            }

            var now = UtcNow;
            var last = account.PasswordChanged.Value;
            var result = last.AddDays(Configuration.PasswordResetFrequency) <= now;

            Tracing.Verbose("[UserAccountService.PasswordResetFrequency ] result: {0}", result);

            return result;
        }

        protected virtual string SetVerificationKey(UserAccount account, VerificationKeyPurpose purpose, string key = null, string state = null)
        {
            if (key == null) key = StripUglyBase64(_crypto.GenerateSalt());

            account.VerificationKey = _crypto.Hash(key);
            account.VerificationPurpose = purpose;
            account.VerificationKeySent = UtcNow;
            account.VerificationStorage = state;

            return key;
        }

        protected virtual bool IsVerificationKeyValid(UserAccount account, VerificationKeyPurpose purpose, string key)
        {
            if (!IsVerificationPurposeValid(account, purpose))
            {
                return false;
            }

            var result = _crypto.VerifyHash(key, account.VerificationKey);
            if (!result)
            {
                Tracing.Warning("[UserAccountService.IsVerificationKeyValid] failed -- verification key doesn't match");
                return false;
            }

            Tracing.Verbose("[UserAccountService.IsVerificationKeyValid] success -- verification key valid");
            return true;
        }

        protected virtual bool IsVerificationPurposeValid(UserAccount account, VerificationKeyPurpose purpose)
        {
            if (account.VerificationPurpose != purpose)
            {
                Tracing.Warning("[UserAccountService.IsVerificationPurposeValid] failed -- verification purpose invalid");
                return false;
            }

            if (IsVerificationKeyStale(account))
            {
                Tracing.Warning("[UserAccountService.IsVerificationPurposeValid] failed -- verification key stale");
                return false;
            }

            Tracing.Verbose("[UserAccountService.IsVerificationPurposeValid] success -- verification purpose valid");
            return true;
        }

        protected virtual bool IsVerificationKeyStale(UserAccount account)
        {
            if (account.VerificationKeySent == null)
            {
                return true;
            }

            if (account.VerificationKeySent < UtcNow.Subtract(Configuration.VerificationKeyLifetime))
            {
                return true;
            }

            return false;
        }

        protected virtual void ClearVerificationKey(UserAccount account)
        {
            account.VerificationKey = null;
            account.VerificationPurpose = null;
            account.VerificationKeySent = null;
            account.VerificationStorage = null;
        }

        protected virtual void SetPassword(UserAccount account, string password)
        {
            if (account == null) throw new ArgumentNullException("account");

            Tracing.Information("[UserAccountService.SetPassword] called for accountId: {0}", account.ID);

            if (String.IsNullOrWhiteSpace(password))
            {
                Tracing.Error("[UserAccountService.SetPassword] failed -- no password provided");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidPassword));
            }

            Tracing.Verbose("[UserAccountService.SetPassword] setting new password hash");

            account.HashedPassword = _crypto.HashPassword(password, Configuration.PasswordHashingIterationCount);
            account.PasswordChanged = UtcNow;
            account.RequiresPasswordReset = false;

            AddEvent(new PasswordChangedEvent { Account = account, NewPassword = password });
        }

        protected virtual void ResetPassword(UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            Tracing.Information("[UserAccountService.ResetPassword] called for accountId: {0}", account.ID);

            if (String.IsNullOrWhiteSpace(account.Email))
            {
                Tracing.Error("[UserAccountService.ResetPassword] no email to use for password reset");
                throw new ValidationException(GetValidationMessage(UserAccountConstants.ValidationMessages.PasswordResetErrorNoEmail));
            }

            if (!account.IsAccountVerified)
            {
                // if they've not yet verified then don't allow password reset
                if (account.IsNew())
                {
                    // instead request an initial account verification
                    Tracing.Verbose("[UserAccountService.ResetPassword] account not verified -- raising account created email event to resend initial email");
                    var key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: account.Email);
                    AddEvent(new AccountCreatedEvent { Account = account, VerificationKey = key });
                }
                else
                {
                    Tracing.Verbose("[UserAccountService.ResetPassword] account not verified -- raising change email event to resend email verification");
                    var key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: account.Email);
                    AddEvent(new EmailChangeRequestedEvent { Account = account, NewEmail = account.Email, VerificationKey = key });
                }
            }
            else
            {
                Tracing.Verbose("[UserAccountService.ResetPassword] creating new verification keys");
                var key = SetVerificationKey(account, VerificationKeyPurpose.ResetPassword);

                Tracing.Verbose("[UserAccountService.ResetPassword] account verified -- raising event to send reset notification");
                AddEvent(new PasswordResetRequestedEvent { Account = account, VerificationKey = key });
            }
        }

        public virtual void AddClaims(Guid accountId, UserClaimCollection claims)
        {
            Tracing.Information("[UserAccountService.AddClaims] called for accountId: {0}", accountId);
            UpdateClaims(accountId, claims);
        }

        public virtual void RemoveClaims(Guid accountId, UserClaimCollection claims)
        {
            Tracing.Information("[UserAccountService.RemoveClaims] called for accountId: {0}", accountId);
            UpdateClaims(accountId, null, claims);
        }

        public virtual void UpdateClaims(
            Guid accountId,
            UserClaimCollection additions = null,
            UserClaimCollection deletions = null)
        {
            Tracing.Information("[UserAccountService.UpdateClaims] called for accountId: {0}", accountId);

            if ((additions == null || !additions.Any()) &&
                (deletions == null || !deletions.Any()))
            {
                Tracing.Verbose("[UserAccountService.UpdateClaims] no additions or deletions -- exiting");
                return;
            }

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID");

            foreach (var addition in additions ?? UserClaimCollection.Empty)
            {
                AddClaim(account, addition);
            }
            foreach (var deletion in deletions ?? UserClaimCollection.Empty)
            {
                RemoveClaim(account, deletion.Type, deletion.Value);
            }
            Update(account);
        }

        public virtual void AddClaim(Guid accountId, string type, string value)
        {
            Tracing.Information("[UserAccountService.AddClaim] called for accountId: {0}", accountId);

            if (String.IsNullOrWhiteSpace(type))
            {
                Tracing.Error("[UserAccountService.AddClaim] failed -- null type");
                throw new ArgumentException("type");
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                Tracing.Error("[UserAccountService.AddClaim] failed -- null value");
                throw new ArgumentException("value");
            }

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID", "accountId");

            AddClaim(account, new UserClaim(type, value));
            Update(account);
        }

        private void AddClaim(UserAccount account, UserClaim claim)
        {
            if (claim == null) throw new ArgumentNullException("claim");

            if (!account.HasClaim(claim.Type, claim.Value))
            {
                account.AddClaim(claim);
                AddEvent(new ClaimAddedEvent { Account = account, Claim = claim });

                Tracing.Verbose("[UserAccountService.AddClaim] claim added");
            }
        }

        public virtual void RemoveClaim(Guid accountId, string type)
        {
            Tracing.Information("[UserAccountService.RemoveClaim] called for accountId: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID", "accountId");

            if (String.IsNullOrWhiteSpace(type))
            {
                Tracing.Error("[UserAccountService.RemoveClaim] failed -- null type");
                throw new ArgumentException("type");
            }

            var claimsToRemove =
                from claim in account.Claims
                where claim.Type == type
                select claim;
            foreach (var claim in claimsToRemove.ToArray())
            {
                account.RemoveClaim(claim);
                AddEvent(new ClaimRemovedEvent { Account = account, Claim = claim });
                Tracing.Verbose("[UserAccountService.RemoveClaim] claim removed");
            }

            Update(account);
        }

        public virtual void RemoveClaim(Guid accountId, string type, string value)
        {
            Tracing.Information("[UserAccountService.RemoveClaim] called for accountId: {0}", accountId);

            var account = GetByID(accountId);
            if (account == null) throw new ArgumentException("Invalid AccountID", "accountId");

            RemoveClaim(account, type, value);
            Update(account);
        }

        private void RemoveClaim(UserAccount account, string type, string value)
        {
            if (String.IsNullOrWhiteSpace(type))
            {
                Tracing.Error("[UserAccountService.RemoveClaim] failed -- null type");
                throw new ArgumentException("type");
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                Tracing.Error("[UserAccountService.RemoveClaim] failed -- null value");
                throw new ArgumentException("value");
            }

            var claimsToRemove =
                from claim in account.Claims
                where claim.Type == type && claim.Value == value
                select claim;
            foreach (var claim in claimsToRemove.ToArray())
            {
                account.RemoveClaim(claim);
                AddEvent(new ClaimRemovedEvent { Account = account, Claim = claim });
                Tracing.Verbose("[UserAccountService.RemoveClaim] claim removed");
            }
        }

        public virtual IEnumerable<Claim> MapClaims(UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            var cmd = new MapClaimsFromAccount { Account = account };
            ExecuteCommand(cmd);
            return cmd.MappedClaims ?? Enumerable.Empty<Claim>();
        }

        internal protected virtual DateTime UtcNow
        {
            get
            {
                return DateTime.Now;
            }
        }

        static readonly string[] UglyBase64 = { "+", "/", "=" };
        protected virtual string StripUglyBase64(string s)
        {
            if (s == null) return s;
            foreach (var ugly in UglyBase64)
            {
                s = s.Replace(ugly, "");
            }
            return s;
        }
    }
    
}
