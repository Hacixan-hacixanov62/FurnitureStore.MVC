namespace MVC_FinalProject.Models.BlogReview
{
    public class BlogReview
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string AppUserId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
