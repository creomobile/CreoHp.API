using System.Collections.Generic;
using CreoHp.Common;

namespace CreoHp.Models.Tags
{
    public class Tag : ModelBase
    {
        public string Name { get; set; }
        public TagType Type { get; set; }
        public int Position { get; set; }

        public virtual ICollection<TagRelation> Parents { get; set; }
        public virtual ICollection<TagRelation> Children { get; set; }

        public virtual ICollection<PhraseTag> Phrases { get; set; }
    }
}