using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Product
{
    public class ProductEdit
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s\-_, -.()!%]+$",
              ErrorMessage = "Name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s.,!?-]*$", ErrorMessage = "Description must contain at least one letter and only letters, digits, spaces and common punctuation are allowed")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Brand is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a valid brand")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a valid category")]
        public int CategoryId { get; set; }
        
        public List<int> TagIds { get; set; } = new();

        public List<int> ColorIds { get; set; } = new();

        public List<IFormFile>? UploadImages { get; set; } // Optional
        public List<int> DeleteImageIds { get; set; }
        public int? MainImageId { get; set; }
    }
}
