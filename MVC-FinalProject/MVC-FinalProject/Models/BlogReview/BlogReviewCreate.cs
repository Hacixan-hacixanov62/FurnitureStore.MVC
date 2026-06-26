namespace MVC_FinalProject.Models.BlogReview
{
    public class BlogReviewCreate
    {
        public int PostId { get; set; }
        public string Comment { get; set; }
    }

    public class BlogReviewCreateApi
    {
        public int PostId { get; set; }
        public string AppUserId { get; set; }
        public string Comment { get; set; }
    }
}
