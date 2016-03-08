using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Models.Admin;
using GasPrice.Data.EF6;

namespace GasPrice.Data.Seeders
{
    public partial class AppDataSeeder
    {
        public static void SeedSecurity(AppDataContext context)
        {
            
            if (!context.Roles.Any())
            {
                new List<Role> 
                {
                    new Role { BitMask = (int)UserRoleEnum.Admin, Name = "Administrador", Description = "Administrador", ObjectState = ObjectState.Added},
                    new Role { BitMask = (int)UserRoleEnum.Editor, Name = "Editor", Description = "Edicao Geral do Site", ObjectState = ObjectState.Added},
                    new Role { BitMask = (int)UserRoleEnum.Author, Name = "Autor", Description = "Edicao Noticias", ObjectState = ObjectState.Added},
                    new Role { BitMask = (int)UserRoleEnum.Reader, Name = "Leitor", Description = "Inserir e alterar novos arquivos no siste.", ObjectState = ObjectState.Added}
                }.ForEach(a => context.Roles.AddOrUpdate(a));
            }
            
            var adminRole = context.Roles.FirstOrDefault(x => x.BitMask == (int)UserRoleEnum.Admin);

        }
    }
}
