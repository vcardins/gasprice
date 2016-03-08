using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using GasPrice.Core.Common.Extensions;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Models.Modules;
using GasPrice.Data.EF6;

namespace GasPrice.Data.Seeders
{
    public partial class AppDataSeeder
    {
        public static void SeedGasStation(AppDataContext context)
        {
            if (context.GasStations.Any()) return;
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(GetResourceFilename("gas_station")))
            {
                if (stream == null) return;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = "|";
                    var records = csvReader.GetRecords<GasStation>().ToArray();
                    records.ForEach(r => r.ObjectState = ObjectState.Added);
                    context.GasStations.AddOrUpdate(records);
                }
            }
        }
    }
}
