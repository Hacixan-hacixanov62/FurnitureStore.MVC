using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Basket;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISettingService _settingService;

        public BasketController(IBasketService basketService, IHttpContextAccessor httpContextAccessor, ISettingService settingService)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var basket = await _basketService.GetBasketByUserIdAsync(userId);
            var setting = await _settingService.GetAllAsync();

            BasketVM model = new BasketVM()
            {
                Basket= basket,
                Setting= setting
            };

            return View(model);

        }

        //[Authorize(Roles = "Admin,SuperAdmin,Member")]
        //[HttpPost]
        //public async Task<IActionResult> AddToBasket(int productId, int colorId)
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

        //    var basketDto = new BasketCreate
        //    {
        //        UserId = userId,
        //        ProductId = productId,
        //        ColorId = colorId
        //    };

        //    try
        //    {
        //        await _basketService.AddBasketAsync(basketDto);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["BasketError"] = ex.Message;
        //        return RedirectToAction("Detail", "Product", new { id = productId });
        //    }
        //}

        [Authorize(Roles = "Admin,SuperAdmin,Member")]
        [HttpPost]
        public async Task<IActionResult> AddToBasket(int productId, int colorId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "User is not authorized" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found" });
            }

            var basketDto = new BasketCreate
            {
                UserId = userId,
                ProductId = productId,
                ColorId = colorId
            };

            try
            {
                await _basketService.AddBasketAsync(basketDto);
                return Ok(new { success = true, message = "Product added to basket" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity([FromBody] BasketCreate basketCreate)
        {
            try
            {
                await _basketService.IncreaseQuantityAsync(basketCreate);
                return Ok(new { success = true, message = "Quantity increased." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity([FromBody] BasketCreate basketCreate)
        {
            try
            {
                await _basketService.DecreaseQuantityAsync(basketCreate);
                return Ok(new { success = true, message = "Quantity decreased." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> UpdateCart(List<BasketUpdate> items)
        //{
        //    var token = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
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

        //    foreach (var item in items)
        //    {
        //        if (item.NewQuantity > item.OldQuantity)
        //        {
        //            int diff = item.NewQuantity - item.OldQuantity;
        //            for (int i = 0; i < diff; i++)
        //            {
        //                var dto = new BasketCreate
        //                {
        //                    ProductId = item.ProductId,
        //                    UserId = userId,
        //                    ColorId = item.ColorId
        //                };
        //                await _basketService.IncreaseQuantityAsync(dto);
        //            }
        //        }
        //        else if (item.NewQuantity < item.OldQuantity)
        //        {
        //            int diff = item.OldQuantity - item.NewQuantity;
        //            for (int i = 0; i < diff; i++)
        //            {
        //                var dto = new BasketCreate
        //                {
        //                    ProductId = item.ProductId,
        //                    UserId = userId,
        //                    ColorId = item.ColorId
        //                };
        //                await _basketService.DecreaseQuantityAsync(dto);
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index", "Basket");
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateCart(List<BasketUpdate> items)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            foreach (var item in items)
            {
                if (item.NewQuantity > item.OldQuantity)
                {
                    int diff = item.NewQuantity - item.OldQuantity;
                    for (int i = 0; i < diff; i++)
                    {
                        var dto = new BasketCreate
                        {
                            ProductId = item.ProductId,
                            UserId = userId,
                            ColorId = item.ColorId
                        };
                        await _basketService.IncreaseQuantityAsync(dto);
                    }
                }
                else if (item.NewQuantity < item.OldQuantity)
                {
                    int diff = item.OldQuantity - item.NewQuantity;
                    for (int i = 0; i < diff; i++)
                    {
                        var dto = new BasketCreate
                        {
                            ProductId = item.ProductId,
                            UserId = userId,
                            ColorId = item.ColorId
                        };
                        await _basketService.DecreaseQuantityAsync(dto);
                    }
                }
            }
            var basket = await _basketService.GetBasketByUserIdAsync(userId);

            var updatedItems = basket.BasketProducts.Select(bp => new
            {
                productId = bp.ProductId,
                colorId = bp.ColorId,
                price = bp.Price,
                quantity = bp.Quantity,
                subtotal = bp.Price * bp.Quantity
            }).ToList();

            var cartTotal = updatedItems.Sum(x => x.subtotal);

            return Json(new { updatedItems, cartTotal });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            await _basketService.DeleteProductFromBasketAsync(productId, userId);
            return Ok();
        }
    }
}
