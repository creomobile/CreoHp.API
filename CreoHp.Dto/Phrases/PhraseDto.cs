using System;

namespace CreoHp.Dto.Phrases
{
    public sealed class PhraseDto : CreatePhraseDto
    {
        public Guid Id { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
