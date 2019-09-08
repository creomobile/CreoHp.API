using CreoHp.Common;
using CreoHp.Repository.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository
{
    static class ConfigurationHelper
    {
        public static void ApplyConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppIdentityUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppIdentityUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new PhraseConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new TagRelationConfiguration());
            modelBuilder.ApplyConfiguration(new PhraseTagConfiguration());
        }

        public static PropertyBuilder<TProperty> SetDefaultShortMaxLength<TProperty>(
            this PropertyBuilder<TProperty> builder)
            => builder
                .HasMaxLength(Consts.ShortStringMaxLength);
    }
}