using InventarioEscolar.Domain.Enums;

namespace InventarioEscolar.Domain.Pagination
{
    public class PagedResult<T>(
    List<T> items,
    int totalCount,
    int page,
    int pageSize,
    string? searchTerm = null,
    ConservationState? conservationState = null) 
    {
        public List<T> Items { get; set; } = items;
        public int TotalCount { get; set; } = totalCount;
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;
        public string SearchTerm { get; set; } = searchTerm ?? string.Empty;
        public ConservationState conservationState { get; set; }

        public static PagedResult<T> Empty(int page, int pageSize, string? searchTerm = null, ConservationState? conservationState = null)
        {
            return new PagedResult<T>([], 0, page, pageSize, searchTerm, conservationState);
        }
    }
}