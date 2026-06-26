namespace MVC_FinalProject.Models.Wishlist
{
    public class WishlistItem
    {
        public string AppUserId { get; set; }
        public List<WishlistProduct> Products { get; set; }
        public int ProductCount { get; set; }
    }
}
