using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Review;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Models.BlogReview
{
    public class PostReviewPage
    {
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string? BlogCategory { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? HighlightText { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public List<BlogReview> BlogReviews { get; set; } = new();
        public BlogReviewCreate NewReview { get; set; } = new();
        public List<BlogPostImg> BlogPostImgs { get; set; } = new();
        public IEnumerable<SettingVM> Settings { get; set; }


        public MVC_FinalProject.Models.BlogPost.BlogPost PreviousBlog { get; set; }
        public MVC_FinalProject.Models.BlogPost.BlogPost NextBlog { get; set; }
    }
}
