namespace MVC_FinalProject.Models.Wishlist
{
    public class WishlistProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int Stock { get; set; }
    }
}
