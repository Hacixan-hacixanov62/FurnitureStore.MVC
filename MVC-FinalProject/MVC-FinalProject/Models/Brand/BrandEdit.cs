using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Brand
{
    public class BrandEdit
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s%\-_,.()!]+$", ErrorMessage = "Brand name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
