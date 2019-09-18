using CreoHp.Models.Phrases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class PhraseConfiguration : ModelBaseConfiguration<Phrase>
    {
        protected override void ConfigureModel(EntityTypeBuilder<Phrase> builder)
        {
            builder.Property(_ => _.Text).IsRequired().IsUnicode();

            builder.HasIndex(_ => _.Text).IsUnique();

            builder
                .HasMany(_ => _.Tags)
                .WithOne(_ => _.Phrase)
                .HasForeignKey(_ => _.PhraseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
