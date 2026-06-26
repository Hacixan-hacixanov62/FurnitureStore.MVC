using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using System.Security.Claims;

namespace MVC_FinalProject.ViewComponents
{
    public class BasketCountViewComponent : ViewComponent
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketCountViewComponent(IBasketService basketService, IHttpContextAccessor httpContextAccessor)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return View(0);

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var basket = await _basketService.GetBasketByUserIdAsync(userId);

            int totalCount = basket?.TotalProductCount ?? 0;
            return View(totalCount);
        }
    }

}
