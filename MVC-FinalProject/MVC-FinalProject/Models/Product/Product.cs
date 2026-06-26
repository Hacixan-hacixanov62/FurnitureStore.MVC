using MVC_FinalProject.Models.Tag;

namespace MVC_FinalProject.Models.Product
{
    public class Product
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string MainImage { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Colors { get; set; }
        public List<string> Images { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
