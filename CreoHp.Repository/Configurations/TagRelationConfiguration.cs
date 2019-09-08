using CreoHp.Models.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class TagRelationConfiguration : IEntityTypeConfiguration<TagRelation>
    {
        public void Configure(EntityTypeBuilder<TagRelation> builder)
        {
            builder.HasKey(p => new {p.ChildTagId, p.ParentTagId});
        }
    }
}