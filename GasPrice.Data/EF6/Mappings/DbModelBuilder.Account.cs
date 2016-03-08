/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure.Annotations;
using GasPrice.Core.Account;

namespace GasPrice.Data.EF6.Mappings
{
    public static partial class DbModelBuilderExtensions
    {
        public static void RegisterUserAccountChildTablesForDelete(this DbContext ctx)
        {
            ctx.Set<UserAccount>().Local.CollectionChanged +=
                delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (UserAccount account in e.NewItems)
                        {
                            account.ClaimCollection.RegisterDeleteOnRemove(ctx);
                        }
                    }
                };
        }

        internal static void RegisterDeleteOnRemove<TChild>(this ICollection<TChild> collection, DbContext ctx)
            where TChild : class
        {
            var entities = collection as EntityCollection<TChild>;
            if (entities != null)
            {
                entities.AssociationChanged += delegate(object sender, CollectionChangeEventArgs e)
                {
                    if (e.Action == CollectionChangeAction.Remove)
                    {
                        var entity = e.Element as TChild;
                        if (entity != null)
                        {
                            ctx.Entry(entity).State = EntityState.Deleted;
                        }
                    }
                };
            }
        }

        public static void ConfigureMembershipRebootUserAccounts<TUserClaim>(this DbModelBuilder modelBuilder, string schemaName)
            where TUserClaim : UserClaim, new()
        {
            var userEntity = modelBuilder.Entity<UserAccount>();

            userEntity.HasKey(x => x.UserId).ToTable("UserAccount", schemaName);

            userEntity.Property(p => p.Tenant).IsRequired().HasMaxLength(50);
        
            userEntity.Property(p => p.Username).IsRequired().HasMaxLength(254)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_USERNAME", 1) { IsUnique = true }));
            
            // Maximum length of a valid email address is 254 characters.
            // See Dominic Sayers answer at SO: http://stackoverflow.com/a/574698/99240
            userEntity.Property(p => p.Email).IsRequired().HasMaxLength(254)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_USER_EMAIL", 1) { IsUnique = true }));

            userEntity.Property(p => p.VerificationKey).HasMaxLength(100);
            userEntity.Property(p => p.VerificationStorage).HasMaxLength(100);
            userEntity.Property(p => p.HashedPassword).HasMaxLength(200);

            userEntity.HasMany(x => x.ClaimCollection)
                .WithRequired().HasForeignKey(x => x.UserId).WillCascadeOnDelete();
            modelBuilder.Entity<TUserClaim>()
                .HasKey(x => x.Id).ToTable("UserClaim", schemaName);

            modelBuilder.Entity<TUserClaim>().Property(p => p.Type).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<TUserClaim>().Property(p => p.Value).IsRequired().HasMaxLength(150);
        }
        
        public static void ConfigureMembershipRebootUserAccounts(this DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<UserClaim>(null);
        }

        public static void ConfigureMembershipRebootUserAccounts(this DbModelBuilder modelBuilder, string schemaName)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<UserClaim>(schemaName);
        }
      
    }
}
