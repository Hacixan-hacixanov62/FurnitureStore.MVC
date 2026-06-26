namespace MVC_FinalProject.Models.Basket
{
    public class Basket
    {
        public string AppUserId { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }
        public int TotalProductCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
