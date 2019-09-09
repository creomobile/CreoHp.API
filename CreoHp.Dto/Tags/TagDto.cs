using CreoHp.Common;
using System;

namespace CreoHp.Dto.Tags
{
    public sealed class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TagType Type { get; set; }
        public int Position { get; set; }
    }
}
