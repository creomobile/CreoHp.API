using System.Collections.Generic;
using CreoHp.Models.PhraseCollections;
using CreoHp.Models.Tags;

namespace CreoHp.Models.Phrases
{
    public class Phrase : ModelBase
    {
        public string Text { get; set; }
        public virtual ICollection<PhraseTag> Tags { get; set; }
        public virtual PhraseCollection Collection { get; set; }
    }
}