using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using GasPrice.Core.Account;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Data.EF6;
using GasPrice.Infrastructure.Crypto;

namespace GasPrice.Data.Seeders
{
    public partial class AppDataSeeder
    {
        public static void SeedUsers(this AppDataContext context)
        {

            var admin = context.UserAccounts.SingleOrDefault(x => x.Username == "admin");
            if (admin != null) return;
            new List<UserAccount>
                {
                    new UserAccount
                    {
                        Tenant = "default",
                        Username = "admin",
                        ID = Guid.NewGuid(),
                        HashedPassword = new DefaultCrypto().HashPassword("admin", 10),
                        Email = "admin@gasprice.com",
                        LastUpdated = DateTime.Now,
                        IsLoginAllowed = true,
                        IsAccountVerified = true,
                        FirstName = "Gas Price",
                        LastName = "Admin",
                        Created = DateTime.Now,      
                        ClaimCollection = new UserClaimCollection
                        {
                            new UserClaim(ClaimTypes.Role, "Admin")
                            {
                                ObjectState = ObjectState.Added
                            }
                        },
                        ObjectState = ObjectState.Added
                    }
                   
                }.ForEach(x => context.Set<UserAccount>().AddOrUpdate(x));
        }

    }
}