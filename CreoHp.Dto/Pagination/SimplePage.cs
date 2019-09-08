namespace CreoHp.Dto.Pagination
{
    public class SimplePage<T>
    {
        public T[] Items { get; set; }
        public bool HasMore { get; set; }
    }
}