using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using GasPrice.Core.Models.Infraestructure;
using GasPrice.Core.Models.Modules;

namespace GasPrice.Data.EF6.Mappings
{
    public static partial class DbModelBuilderExtensions
    {
        private static DbModelBuilder _modelBuilder;
        public static void ConfigureApplicationModels(this DbModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;

            MapFuelBrand();
            MapGasStation();
            MapExceptionLog();
            MapGasStationFuelService();
            MapPriceHistory();
        }

        private static void MapFuelBrand()
        {
            var entity = _modelBuilder.Entity<FuelBrand>();
            entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            entity.Property(p => p.Name).HasMaxLength(40).IsRequired();
        }

        private static void MapGasStation()
        {
            var entity = _modelBuilder.Entity<GasStation>();
            entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Address).HasMaxLength(400).IsRequired();         
        }
      
        private static void MapExceptionLog()
        {
            var entity = _modelBuilder.Entity<ExceptionLog>();
            entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            entity.Property(p => p.Message).IsRequired().HasMaxLength(800);
            entity.Property(p => p.Source).HasMaxLength(400);
            entity.Property(p => p.RequestUri).HasMaxLength(200);
            entity.Property(p => p.Method).HasMaxLength(20);

            entity.HasOptional(e => e.User)
              .WithMany(s => s.ExceptionLogs)
              .WillCascadeOnDelete(true);

        }

        private static void MapGasStationFuelService()
        {
            var entity = _modelBuilder.Entity<FuelServiceGasStation>();
            entity.HasKey(e => new {e.GasStationId, e.FuelServiceId});

            entity.HasRequired(a => a.GasStation)
             .WithMany(p => p.Services)
             .HasForeignKey(a => a.GasStationId)
             .WillCascadeOnDelete(false);

            entity.HasRequired(a => a.Service)
               .WithMany(p => p.GasStations)
               .HasForeignKey(u => u.FuelServiceId);
        }

        private static void MapPriceHistory()
        {
            var entity = _modelBuilder.Entity<FuelPriceHistory>();
            entity.HasKey(e => new { e.GasStationId, e.FuelServiceId, e.UserId, e.DateTime });
        }
    }
}