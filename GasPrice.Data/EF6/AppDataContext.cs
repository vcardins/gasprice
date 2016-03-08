using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GasPrice.Core.Account;
using GasPrice.Core.Models;
using GasPrice.Core.Models.Admin;
using GasPrice.Core.Models.Infraestructure;
using GasPrice.Core.Models.Modules;
using GasPrice.Data.EF6.Mappings;

namespace GasPrice.Data.EF6
{

    public class AppDataContext : DataContext<AppDataContext>
    {
        public AppDataContext()
            : base("name=DataContext")
        {
        }

        public AppDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString ?? "name=DataContext")
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<FuelBrand> FuelBrands { get; set; }
        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<FuelService> FuelServices { get; set; }
        public DbSet<FuelPriceHistory> FuelPriceHistory { get; set; }        
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // Use singular table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureApplicationModels();
            modelBuilder.ConfigureMembershipRebootUserAccounts();
            modelBuilder.ConfigureSecurityModels();
        }

    }
}