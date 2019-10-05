namespace CreoHp.Dto.Pagination
{
    public class Page<T> : SimplePage<T>
    {
        public int Total { get; set; }
    }
}
