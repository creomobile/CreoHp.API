using CreoHp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    abstract class ModelBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : ModelBase
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.CreatedAt)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.UpdatedAt)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.ModifiedByUserId).IsRequired(false);

            builder
                .HasOne(_ => _.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(_ => !_.IsDeleted);

            ConfigureModel(builder);
        }

        protected abstract void ConfigureModel(EntityTypeBuilder<T> builder);
    }
}