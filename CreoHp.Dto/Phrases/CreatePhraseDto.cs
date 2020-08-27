using System;

namespace CreoHp.Dto.Phrases
{
    public class CreatePhraseDto
    {
        public string Text { get; set; }
        public string OriginalText { get; set; }
        public Guid[] TagIds { get; set; }
    }
}
