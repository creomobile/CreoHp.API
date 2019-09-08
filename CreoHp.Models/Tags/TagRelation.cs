using System;

namespace CreoHp.Models.Tags
{
    public class TagRelation
    {
        public Guid ParentTagId { get; set; }
        public Guid ChildTagId { get; set; }

        public virtual Tag Parent { get; set; }
        public virtual Tag Child { get; set; }
    }
}
