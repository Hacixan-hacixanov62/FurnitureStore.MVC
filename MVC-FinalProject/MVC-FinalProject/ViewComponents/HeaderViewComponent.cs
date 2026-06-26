using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Basket;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderViewComponent(
            IProductService productService,
            ISettingService settingService,
            IBasketService basketService,
            IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _settingService = settingService;
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(string searchQuery)
        {
            var settings = await _settingService.GetAllAsync();

            var products = string.IsNullOrEmpty(searchQuery)
                ? new List<Product>()
                : await _productService.SearchByNameAsync(searchQuery);

            var searchResults = products.Select(p => new SearchProduct
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                BrandName = p.Brand,
                CategoryName = p.Category
            }).ToList();

            List<BasketItem> lastTwo = new();

            var authToken = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(authToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(authToken);

                var userId = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier || c.Type == "userId" || c.Type == "nameid")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    lastTwo = await _basketService.GetLastTwoAsync(userId);
                }
            }

            HeaderVMVC model = new()
            {
                Setting = settings,
                SearchProducts = searchResults,
                LastTwoBasketItems = lastTwo
            };

            return View(model);
        }


        public class HeaderVMVC { 
            public IEnumerable<SearchProduct> SearchProducts { get; set; }
            public IEnumerable<SettingVM>  Setting { get; set; }
            public List<BasketItem> LastTwoBasketItems { get; set; }
        }
    }

}
