using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Review
{
    public class ReviewEdit
    {
            public int Id { get; set; }
            public int ProductId { get; set; }
            [Required(ErrorMessage = "Comment required")]
            [StringLength(2000, MinimumLength = 3)]
            public string Comment { get; set; }
            public string AppUserId { get; set; }
           
    }

    public class ReviewEditApi
    {
        public string AppUserId { get; set; }
        public string Comment { get; set; }
    }
}
