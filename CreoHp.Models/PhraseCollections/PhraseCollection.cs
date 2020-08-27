using CreoHp.Models.Phrases;
using System;

namespace CreoHp.Models.PhraseCollections
{
    public class PhraseCollection : ModelBase
    {
        public Guid PhraseId { get; set; }
        public string OriginalText { get; set; }
        public int SearchCount { get; set; }
        public bool CanSearch { get; set; } = true;
        public virtual Phrase Phrase { get; set; }
    }
}
