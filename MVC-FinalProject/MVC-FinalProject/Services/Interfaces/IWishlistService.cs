using MVC_FinalProject.Models.Wishlist;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistResult> AddWishlistAsync(Wishlist model);
        Task<WishlistItem> GetByUserIdAsync(string userId);
        Task<bool> DeleteProductFromWishlistAsync(string userId, int productId);
    }
}
