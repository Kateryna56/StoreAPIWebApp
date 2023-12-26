namespace StoreAPIWebApp.Models
{
    public class PaginatedResult<T>
    {
        public string NextLink { get; set; }
        public List<T> Values { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
