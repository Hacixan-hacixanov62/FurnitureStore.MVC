using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Category
{
    public class CategoryEdit
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&*\(\)\{\}\-=\+\[\]\\|? ]*$", ErrorMessage = "Category name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
