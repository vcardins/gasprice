using System.Data.Entity.Migrations;
using GasPrice.Data.EF6;
using GasPrice.Data.Seeders;

namespace GasPrice.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(AppDataContext context)
        {
            AppDataSeeder.Seed(context);
        }
    }
}
