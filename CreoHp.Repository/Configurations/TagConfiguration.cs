using System;
using CreoHp.Common;
using CreoHp.Models.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreoHp.Repository.Configurations
{
    sealed class TagConfiguration : ModelBaseConfiguration<Tag>
    {
        protected override void ConfigureModel(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(_ => _.Name).IsUnicode().SetDefaultShortMaxLength();
            builder.HasIndex(_ => new { _.Name, _.Type }).IsUnique();

            builder.HasMany(_ => _.Parents)
                .WithOne(_ => _.Child)
                .HasForeignKey(_ => _.ChildTagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(_ => _.Children)
                .WithOne(_ => _.Parent)
                .HasForeignKey(_ => _.ParentTagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(_ => _.Phrases)
                .WithOne(_ => _.Tag)
                .HasForeignKey(_ => _.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                // PhraseСharacter
                new Tag
                {
                    Id = new Guid("AB3A769A-E800-4E48-97B5-A577BAF5AC86"),
                    Name = "Neutral",
                    Type = TagType.PhraseCharacter,
                    Position = 0,
                },
                new Tag
                {
                    Id = new Guid("C1F30EE9-EC04-448C-A5CC-3FE22C64E52A"),
                    Name = "Positive",
                    Type = TagType.PhraseCharacter,
                    Position = 1,
                },
                new Tag
                {
                    Id = new Guid("A21869D5-C093-48CB-9CE0-C80D2E08D5BD"),
                    Name = "Negative",
                    Type = TagType.PhraseCharacter,
                    Position = 2,
                },

                // PhraseSubject
                new Tag
                {
                    Id = new Guid("CCA290DE-81F5-482D-B27C-C5AEA33B0886"),
                    Name = "Common",
                    Type = TagType.PhraseSubject,
                    Position = 0,
                },
                new Tag
                {
                    Id = new Guid("B472DE0A-951A-4518-ABBE-0528043E53A4"),
                    Name = "Health",
                    Type = TagType.PhraseSubject,
                    Position = 1,
                },
                new Tag
                {
                    Id = new Guid("92FD3C2D-32E1-4E99-A17C-1E1814AE538E"),
                    Name = "Family",
                    Type = TagType.PhraseSubject,
                    Position = 2,
                },
                new Tag
                {
                    Id = new Guid("DDAC2FA4-964E-475F-B1FF-31FA9105FBD7"),
                    Name = "Children",
                    Type = TagType.PhraseSubject,
                    Position = 3,
                },
                new Tag
                {
                    Id = new Guid("66956A46-CFA5-4C4F-AF55-718D9D97D850"),
                    Name = "Love",
                    Type = TagType.PhraseSubject,
                    Position = 4,
                },
                new Tag
                {
                    Id = new Guid("F07F9DF2-FCB0-4027-BFC8-67438342458F"),
                    Name = "Friendship",
                    Type = TagType.PhraseSubject,
                    Position = 5,
                },
                new Tag
                {
                    Id = new Guid("32EB46B5-6D6F-4C41-8495-47AD37E4B28D"),
                    Name = "Study",
                    Type = TagType.PhraseSubject,
                    Position = 6,
                },
                new Tag
                {
                    Id = new Guid("454FC2A6-7C0B-4A89-840C-D9378DA0C77D"),
                    Name = "Сareer",
                    Type = TagType.PhraseSubject,
                    Position = 7,
                },
                new Tag
                {
                    Id = new Guid("48F1D60C-B834-415E-BAE1-4B2C867EA93D"),
                    Name = "Money",
                    Type = TagType.PhraseSubject,
                    Position = 8,
                },
                new Tag
                {
                    Id = new Guid("82415242-C3B9-45B8-B854-92DCDB5E2CFB"),
                    Name = "Creation",
                    Type = TagType.PhraseSubject,
                    Position = 9,
                },
                new Tag
                {
                    Id = new Guid("134E8818-A5C8-49EA-B279-29FCA5FF6CE9"),
                    Name = "Travels",
                    Type = TagType.PhraseSubject,
                    Position = 10,
                },
                new Tag
                {
                    Id = new Guid("5AF6AC0B-D645-4A2A-9943-08599A63573E"),
                    Name = "Intertainment",
                    Type = TagType.PhraseSubject,
                    Position = 11,
                },
                new Tag
                {
                    Id = new Guid("7EC86883-C05D-4E55-92BB-58E7931D92AF"),
                    Name = "Shopping",
                    Type = TagType.PhraseSubject,
                    Position = 12,
                },

                // PhraseType
                new Tag
                {
                    Id = new Guid("778BE6B6-9233-454B-9511-9AFEF5DD9B53"),
                    Name = "Info",
                    Type = TagType.PhraseType,
                    Position = 0,
                },
                new Tag
                {
                    Id = new Guid("0BDCFDBD-2B7E-480B-98E0-93C2FC61B458"),
                    Name = "Tip",
                    Type = TagType.PhraseType,
                    Position = 1,
                },
                new Tag
                {
                    Id = new Guid("B8D72746-F8BE-40F6-B905-23D9E2D46741"),
                    Name = "Warning",
                    Type = TagType.PhraseType,
                    Position = 2,
                }
            );
        }
    }
}