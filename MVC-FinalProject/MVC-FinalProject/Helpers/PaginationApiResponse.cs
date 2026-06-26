namespace MVC_FinalProject.Helpers
{
    public class PaginationApiResponse<T>
    {
        public List<T> Datas { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext => CurrentPage < TotalPage;
        public bool HasPrevious => CurrentPage > 1;
    }

}
