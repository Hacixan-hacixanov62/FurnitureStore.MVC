using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using System.Security.Claims;

namespace MVC_FinalProject.ViewComponents
{
    public class WishlistCountViewComponent : ViewComponent
    {
        private readonly IWishlistService _wishlistService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistCountViewComponent(IWishlistService wishlistService, IHttpContextAccessor httpContextAccessor)
        {
            _wishlistService = wishlistService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return View(0);

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var wishlist = await _wishlistService.GetByUserIdAsync(userId);

            int totalCount = wishlist?.ProductCount?? 0;
            return View(totalCount);
        }
    }
}
