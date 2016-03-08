using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using GasPrice.Core.Models.Admin;
using GasPrice.Core.Models.Infraestructure;

namespace GasPrice.Data.EF6.Mappings
{
    public static partial class DbModelBuilderExtensions
    {
        public static void ConfigureSecurityModels(this DbModelBuilder modelBuilder)
        {
            MapRole();
            MapUserRole();
            MapLog();
        }

        private static void MapRole()
        {
            var entity = _modelBuilder.Entity<Role>();
            entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            entity.Property(p => p.Name).HasMaxLength(40).IsRequired();
            entity.Property(p => p.Description).HasMaxLength(200);
        }

        private static void MapUserRole()
        {
            var entity = _modelBuilder.Entity<UserRole>();
            entity.HasKey(e => new {e.UserId, e.RoleId});

            entity.HasRequired(a => a.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(true);

            entity.HasRequired(a => a.Role)
               .WithMany(p => p.UserRoles)
               .HasForeignKey(a => a.RoleId)
               .WillCascadeOnDelete(true);
        }

        private static void MapLog()
        {
            var entity = _modelBuilder.Entity<Log>();
            entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            entity.HasOptional(a => a.User)
                .WithMany(p => p.Logs)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);
        }

    }

}