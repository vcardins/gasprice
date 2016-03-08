using GasPrice.Data.EF6;

namespace GasPrice.Data.Seeders
{
    public static partial class AppDataSeeder
    {
        //Assembly ass = Assembly.GetExecutingAssembly().Location;
        const string SeederPath = "GasPrice.Data.Seeders.Data";

        public static void Seed(AppDataContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            //return;
            
            SeedSecurity(context);
            SeedUsers(context);
            //SeedFuelBrand(context);
            //SeedGasStation(context);
        }

        private static string GetResourceFilename(string resouce)
        {
            var resourceName = string.Format("{0}.{1}.csv", SeederPath, resouce);
            return resourceName;
        }
    }
}