namespace InventarioEscolar.Domain.Pagination
{
    public class PagedResult<T>(List<T> items, int totalCount, int page, int pageSize)
    {
        public List<T> Items { get; set; } = items;
        public int TotalCount { get; set; } = totalCount;
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;

        public static PagedResult<T> Empty(int page, int pageSize)
        {
            return new PagedResult<T>([], 0, page, pageSize);
        }
    }
}