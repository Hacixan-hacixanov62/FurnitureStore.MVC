using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Models.Review
{
    public class ProductReviewPage
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<string> Colors { get; set; } = new();
        public List<MVC_FinalProject.Models.Color.Color> ColorList { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public List<string> Tags { get; set; } = new();
        public decimal Price { get; set; }
        public List<Review> Reviews { get; set; } = new();
        public ReviewCreate NewReview { get; set; } = new();
        public IEnumerable<SettingVM> Settings { get; set; }

    }
}
