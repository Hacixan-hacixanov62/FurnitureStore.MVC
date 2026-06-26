using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Slider
{
    public class SliderEdit
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&*\(\)\{\}\-=\+\[\]\\|? ]*$",
             ErrorMessage = "Title must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Title { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&*\(\)\{\}\-=\+\[\]\\|? ]*$",
            ErrorMessage = "Description must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}
