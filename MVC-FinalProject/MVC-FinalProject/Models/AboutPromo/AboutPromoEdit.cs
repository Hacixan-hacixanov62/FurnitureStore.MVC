using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.AboutPromo
{
    public class AboutPromoEdit
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Title must contain at least one letter and cannot be only numbers.")]
        public string Title { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Description must contain at least one letter and cannot be only numbers.")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
