using CreoHp.Models.PhraseCollections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class BadCollectionUrlConfiguration : IEntityTypeConfiguration<BadCollectionUrl>
    {
        public void Configure(EntityTypeBuilder<BadCollectionUrl> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
