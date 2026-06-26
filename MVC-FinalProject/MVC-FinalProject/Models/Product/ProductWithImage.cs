namespace MVC_FinalProject.Models.Product
{
    public class ProductWithImage
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Colors { get; set; }

        public List<ProductImage> ProductImages { get; set; }
    }
}
