using CreoHp.Dto.Pagination;
using System;

namespace CreoHp.Dto.Phrases
{
    public sealed class PhrasesRequestCriteria : DynamicPaginationCriteria<Guid?>
    {
        public string Q { get; set; }
        public Guid[] TagIds { get; set; }
    }
}
