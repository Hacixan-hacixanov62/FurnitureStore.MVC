using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPostEdit
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s.,!%\-_:;()&'""—]+$", ErrorMessage = "Input must contain at least one letter and only valid characters.")] 
        public string Title { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s.,!%\-_:;()&'""—]+$", ErrorMessage = "Input must contain at least one letter and only valid characters.")]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s.,!%\-_:;()&'""—]+$", ErrorMessage = "Input must contain at least one letter and only valid characters.")]
        public string HighlightText { get; set; }

        [Required]
        public int BlogCategoryId { get; set; }
        public List<IFormFile> Images { get; set; } 
        public List<BlogPostImg> ExistingImages { get; set; } 
        public List<int> DeleteImageIds { get; set; } 
        public int? MainImageId { get; set; } 
     

    }
}
