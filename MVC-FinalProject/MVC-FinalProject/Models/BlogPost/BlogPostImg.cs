namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPostImg
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int BlogPostId { get; set; }
        public bool IsMain { get; set; }
    }
}
