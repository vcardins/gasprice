using System;
using System.Collections.Generic;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Models;
using GasPrice.Core.Models.Admin;
using GasPrice.Core.Models.Infraestructure;
using GasPrice.Core.Models.Modules;

namespace GasPrice.Core.Account
{
    public class UserAccount : Person
    {
        public virtual Guid ID { get; set; }
        public virtual string Tenant { get; set; }
        public virtual DateTime LastUpdated { get; set; }
        public virtual bool IsAccountClosed { get; set; }
        public virtual DateTime? AccountClosed { get; set; }
        public virtual bool IsLoginAllowed { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual DateTime? LastFailedLogin { get; set; }
        public virtual int FailedLoginCount { get; set; }
        public virtual DateTime? PasswordChanged { get; set; }
        public virtual bool RequiresPasswordReset { get; set; }
        public virtual string Email { get; set; }
        public virtual bool IsAccountVerified { get; set; }
        public virtual string VerificationKey { get; set; }
        public virtual VerificationKeyPurpose? VerificationPurpose { get; set; }
        public virtual DateTime? VerificationKeySent { get; set; }
        public virtual string VerificationStorage { get; set; }
        public virtual string HashedPassword { get; set; }
        public virtual ICollection<UserClaim> ClaimCollection { get; set; }
        public IEnumerable<UserClaim> Claims
        {
            get { return ClaimCollection; }
        }
        public void AddClaim(UserClaim item)
        {
            ClaimCollection.Add(new UserClaim
            {
                UserId = UserId, 
                Type = item.Type, 
                Value = item.Value
            });
        }
        public void RemoveClaim(UserClaim item)
        {
            ClaimCollection.Remove(item);
        }

        public bool IsNew()
        {
            return LastLogin.HasValue;
        }

        public virtual ICollection<ExceptionLog> ExceptionLogs { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserRoleEnum> UserRolesEnum { get; set; }

        public virtual ICollection<FuelPriceHistory> PriceHistory { get; set; }
        
        
    }
}
