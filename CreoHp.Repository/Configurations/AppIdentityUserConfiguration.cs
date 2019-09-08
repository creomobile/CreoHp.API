using CreoHp.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class AppIdentityUserConfiguration : IEntityTypeConfiguration<AppIdentityUser>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
        {
            builder.Property(_ => _.FirstName)
                .IsUnicode()
                .SetDefaultShortMaxLength();
            builder.Property(_ => _.LastName)
                .IsUnicode()
                .SetDefaultShortMaxLength();
        }
    }
}