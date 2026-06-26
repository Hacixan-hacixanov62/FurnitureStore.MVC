using MVC_FinalProject.Models.Category;

namespace MVC_FinalProject.Models.Product
{
    public class ProductDetail
    {
        public int id { get; set; }
        public int Stock { get; set; }
        public string name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<MVC_FinalProject.Models.Category.Category> Categories { get; set; }
        public IEnumerable<MVC_FinalProject.Models.Tag.Tag> Tags { get; set; }
        public IEnumerable<MVC_FinalProject.Models.Color.Color> colors { get; set; }
    }
}
