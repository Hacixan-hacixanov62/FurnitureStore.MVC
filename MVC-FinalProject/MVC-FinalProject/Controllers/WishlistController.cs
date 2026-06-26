using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Wishlist;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IWishlistService _wishlistService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public WishlistController(ISettingService settingService, IWishlistService wishlistService,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _settingService = settingService;
            _wishlistService = wishlistService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var setting = await _settingService.GetAllAsync();

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            var wishlist = await _wishlistService.GetByUserIdAsync(userId);


            WishlistVM model = new WishlistVM
            {
                Setting = setting,
                Wishlist = wishlist
            };

            return View(model);
        }


        //[HttpPost]
        //public async Task<IActionResult> AddToWishlist(int productId)
        //{
        //    var token = HttpContext.Session.GetString("AuthToken");
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return Unauthorized();
        //    }
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(token);
        //    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized();
        //    }

        //    var dto = new Wishlist{
        //        AppUserId = userId,
        //        ProductId = productId
        //    };

        //    var result = await _wishlistService.AddWishlistAsync(dto);

        //    if (result.Success)
        //    {
        //        TempData["Success"] = result.Message;
        //    }
        //    else
        //    {
        //        TempData["Error"] = result.Message;
        //    }

        //    return RedirectToAction("Index", "Home", new { id = productId });
        //}


        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }

            var dto = new Wishlist
            {
                AppUserId = userId,
                ProductId = productId
            };

            var result = await _wishlistService.AddWishlistAsync(dto);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProductFromWishlist(string userId, int productId)
        {
            var result = await _wishlistService.DeleteProductFromWishlistAsync(userId, productId);

            if (!result)
                return BadRequest("Something wrong.");

            return Ok();
        }
    }
}
