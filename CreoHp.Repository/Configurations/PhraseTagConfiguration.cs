using CreoHp.Models.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class PhraseTagConfiguration : IEntityTypeConfiguration<PhraseTag>
    {
        public void Configure(EntityTypeBuilder<PhraseTag> builder)
        {
            builder.HasKey(p => new {p.PhraseId, p.TagId});
        }
    }
}