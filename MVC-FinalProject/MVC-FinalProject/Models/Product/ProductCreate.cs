using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Product
{
    using System.ComponentModel.DataAnnotations;

    public class ProductCreate
    {
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s\-_,.()!%]+$",
        ErrorMessage = "Name must contain at least one letter and cannot be only numbers.")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive decimal number.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
        public int Stock { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9\s\-_,.()!%]+$", ErrorMessage = "Description must contain at least one letter and cannot be only numbers.")]
        public string Description { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<int> TagIds { get; set; } = new();

        [Required]
        public List<int> ColorIds { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
