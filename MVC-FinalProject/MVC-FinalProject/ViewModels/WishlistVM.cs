using MVC_FinalProject.Models.Wishlist;

namespace MVC_FinalProject.ViewModels
{
    public class WishlistVM
    {
        public IEnumerable<SettingVM> Setting { get; set; }
        public WishlistItem Wishlist { get; set; }

    }
}
