namespace CreoHp.Dto.Pagination
{
    public class DynamicPaginationCriteria<TItem> : PaginationCriteriaBase
    {
        public TItem FromItem { get; set; }
    }
}
