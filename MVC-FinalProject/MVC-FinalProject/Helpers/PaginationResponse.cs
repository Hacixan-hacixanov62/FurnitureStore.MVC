
namespace MVC_FinalProject.Helpers
{
    public class PaginationResponse<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginationResponse<T> Create(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            return new PaginationResponse<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }

}
