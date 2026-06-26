using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Subscription;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILandingBannerService _landingBannerService;
        private readonly IInstagramService _instagramService;
        private readonly ISubscribeImgService _subscribeImgService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ISettingService _settingService;

        public HomeController(ISliderService sliderService,
                              ICategoryService categoryService,
                              IProductService productService,
                              ILandingBannerService landingBannerService,
                              IInstagramService instagramService,
                              ISubscribeImgService subscribeImgService,
                              ISubscriptionService subscriptionService,
                              ISettingService settingService)
        {
            _sliderService = sliderService;
            _categoryService = categoryService;
            _productService = productService;
            _instagramService = instagramService;
            _landingBannerService = landingBannerService;
            _subscribeImgService = subscribeImgService;
            _subscriptionService = subscriptionService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index(string? searchQuery)
        {
            var sliders = await _sliderService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            var landingBanners = await _landingBannerService.GetAllAsync();
            var instagrams = await _instagramService.GetAllAsync();
            var subscribeImgs = await _subscribeImgService.GetAllAsync();
            var settings = await _settingService.GetAllAsync();

            var searchResults = string.IsNullOrEmpty(searchQuery) ? new List<Product>()
                               : await _productService.SearchByNameAsync(searchQuery);

            ViewBag.SearchQuery = searchQuery;

            HomeVM model = new HomeVM()
            {
                Sliders = sliders,
                Categories = categories,
                Products = products,
                LandingBanners = landingBanners,
                Instagrams = instagrams,
                SubscribeImgs = subscribeImgs,
                SearchResults = searchResults,
                Settings = settings
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return Ok(new List<Product>());
            }
            var products = await _productService.SearchByNameAsync(searchQuery);
            return Ok(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubscribeAjax([FromForm] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, message = "Email is required." });

            try
            {
                var dto = new SubscriptionCreate { Email = email };
                var response = await _subscriptionService.SubscribeAsync(dto);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "You have successfully subscribed!" });
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                if (errorMessage.Contains("First be register"))
                    return Json(new { success = false, message = "Please register before subscribing." });

                if (errorMessage.Contains("already exists"))
                    return Json(new { success = false, message = "You have already subscribed with this email." });

                return Json(new { success = false, message = "An error occurred. Please try again later." });
            }
            catch
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }      
    }
}