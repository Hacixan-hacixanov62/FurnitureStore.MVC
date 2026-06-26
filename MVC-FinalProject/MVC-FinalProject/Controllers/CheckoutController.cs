using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Basket;
using MVC_FinalProject.Models.Order;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;
using Stripe.Checkout;
using Stripe.Climate;

namespace MVC_FinalProject.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        public CheckoutController(IHttpClientFactory httpClientFactory, IConfiguration configuration, 
                                  IPromoCodeService promoCodeService, IBasketService basketService, IOrderService orderService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["BaseUrl"]);
            _promoCodeService = promoCodeService;
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            string token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var basketJson = HttpContext.Session.GetString("LastOrder");
            if (string.IsNullOrEmpty(basketJson)) return View("Error");

            var basket = JsonSerializer.Deserialize<Basket>(basketJson);

            string promoCode = HttpContext.Session.GetString("ActivePromoCode");
            decimal? discountPercent = null;

            if (!string.IsNullOrEmpty(promoCode))
            {
                var promo = await _promoCodeService.GetByCodeAsync(promoCode);
                if (promo != null && promo.IsActive)
                {
                    discountPercent = promo.DiscountPercent;
                }
            }

            var dto = new OrderConfirmationEmailDto
            {
                ToEmail = email,
                FullName = fullName,
                Total = basket.TotalPrice,
                PromoCode = string.IsNullOrEmpty(promoCode) ? null : promoCode,
                DiscountPercent = discountPercent,
                Products = basket.BasketProducts.Select(p => new OrderedProductDto
                {
                    Name = p.ProductName,
                    Count = p.Quantity,
                    Price = p.Price
                }).ToList()
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7004/api/Order/SendConfirmationEmail", dto);
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API response: {response.StatusCode} - {result}");

            string stripeSessionId = Request.Query["session_id"];
            var orderCreateDto = new OrderCreate
            {
                AppUserId = userId,
                StripeSessionId = stripeSessionId,
                PromoCode = promoCode
            };

            await _orderService.CreateOrderAsync(orderCreateDto);


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Check([FromQuery] decimal data)
        {
            var domain = _httpClient.BaseAddress.ToString();

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Home/",
                CancelUrl = domain + "home/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };


            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)data * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "JoiFurn"
                    },
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionListItem);

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }


        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var domain = "https://localhost:7169/";

            string token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // 1. Səbəti əldə et
            var basket = await _basketService.GetBasketByUserIdAsync(userId);
            if (basket == null || !basket.BasketProducts.Any()) return BadRequest("Basket is empty.");

            // 2. Promokodu yoxla
            string promoCode = HttpContext.Session.GetString("ActivePromoCode");
            decimal discountPercent = 0;
            if (!string.IsNullOrEmpty(promoCode))
            {
                var promo = await _promoCodeService.GetByCodeAsync(promoCode);
                if (promo != null && promo.IsActive)
                {
                    discountPercent = promo.DiscountPercent;
                }
            }

            // 3. Stripe sessiya hazırlığı
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"checkout/orderconfirmation?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = domain + "home/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            // 4. Məhsulları əlavə et
            foreach (var item in basket.BasketProducts)
            {
                decimal unitPrice = item.Price;
                if (discountPercent > 0)
                {
                    var discountAmount = (unitPrice * discountPercent) / 100;
                    unitPrice -= discountAmount;
                }
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(unitPrice * 100),
                        Currency = "usd",

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                        },
                    },
                    Quantity = item.Quantity
                };

                options.LineItems.Add(lineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);


            var simplifiedBasket = new Basket
            {
                AppUserId = basket.AppUserId,
                TotalPrice = basket.TotalPrice,
                TotalProductCount = basket.TotalProductCount,
                BasketProducts = basket.BasketProducts.Select(p => new BasketProduct
                {
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList()
            };

            HttpContext.Session.SetString("LastOrder", JsonSerializer.Serialize(simplifiedBasket));          
            //await DeleteBasket(userId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        private async Task DeleteBasket(string userId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7004/api/Basket/DeleteBasketProduct/{userId}");

            if (!response.IsSuccessStatusCode) { }
        }
    }
}