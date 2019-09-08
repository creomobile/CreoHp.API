using CreoHp.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class AppIdentityUserRoleConfiguration : IEntityTypeConfiguration<AppIdentityUserRole>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUserRole> builder)
        {
            builder.HasKey(_ => new {_.UserId, _.RoleId});

            builder.HasOne(_ => _.Role)
                .WithMany(_ => _.Users)
                .HasForeignKey(_ => _.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.User)
                .WithMany(_ => _.Roles)
                .HasForeignKey(_ => _.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}