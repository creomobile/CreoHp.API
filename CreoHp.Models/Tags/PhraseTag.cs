using System;
using CreoHp.Models.Phrases;

namespace CreoHp.Models.Tags
{
    public class PhraseTag
    {
        public Guid PhraseId { get; set; }
        public Guid TagId { get; set; }

        public virtual Phrase Phrase { get; set; }
        public virtual Tag Tag { get; set; }

        public override bool Equals(object obj) =>
            obj is PhraseTag phraseTag && phraseTag.PhraseId == PhraseId && phraseTag.TagId == TagId;
        public override int GetHashCode() => HashCode.Combine(PhraseId, TagId);
    }
}
