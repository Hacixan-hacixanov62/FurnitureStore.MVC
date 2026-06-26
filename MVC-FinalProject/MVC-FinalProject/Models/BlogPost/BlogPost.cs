using System.Text.Json.Serialization;

namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HighlightText { get; set; }
        public int BlogCategoryId { get; set; }
        public string BlogCategory { get; set; }
        public List<BlogPostImg> Images { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
