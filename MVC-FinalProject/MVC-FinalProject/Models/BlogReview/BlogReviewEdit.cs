using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.BlogReview
{
    public class BlogReviewEdit
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        [Required(ErrorMessage = "Comment required")]
        [StringLength(2000, MinimumLength = 3)]
        public string Comment { get; set; }
        public string AppUserId { get; set; }

    }

    public class BlogReviewEditApi
    {
        public string AppUserId { get; set; }
        public string Comment { get; set; }
    }
}
