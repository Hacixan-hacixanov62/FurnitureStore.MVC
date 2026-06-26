using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.BlogCategory
{
    public class BlogCategoryCreate
    {
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_.,()&!]+$", ErrorMessage = "Category name must contain at least one letter and only valid characters.")]
        public string Name { get; set; }

    }
}
