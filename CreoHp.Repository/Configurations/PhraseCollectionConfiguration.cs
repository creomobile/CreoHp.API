using CreoHp.Common;
using CreoHp.Models.PhraseCollections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class PhraseCollectionConfiguration : ModelBaseConfiguration<PhraseCollection>
    {
        protected override void ConfigureModel(EntityTypeBuilder<PhraseCollection> builder)
        {
            builder
                .Property(p => p.OriginalText)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(Consts.MaxIndexString);

            builder.HasIndex(p => p.OriginalText).IsUnique();

            builder
                .HasOne(p => p.Phrase)
                .WithOne(p => p.Collection)
                .HasForeignKey<PhraseCollection>(p => p.PhraseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
