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
    }
}
