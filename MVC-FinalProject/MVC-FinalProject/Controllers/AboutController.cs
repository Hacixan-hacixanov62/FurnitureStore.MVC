using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutBannerImgService _aboutBannerImgService;
        private readonly IBrandService _brandService;
        private readonly IAboutPromoService _aboutPromoService;
        private readonly ISettingService _settingService;
        private readonly IReviewService _reviewService;
        public AboutController(IAboutBannerImgService aboutBannerImgService,
                               IBrandService brandService,
                               IAboutPromoService aboutPromoService,
                               ISettingService settingService,
                               IReviewService reviewService)
        {
            _aboutBannerImgService = aboutBannerImgService;
            _brandService = brandService;
            _aboutPromoService = aboutPromoService;
            _settingService = settingService;
            _reviewService = reviewService;
        }
        public async Task<IActionResult> Index()
        {
            var bannerImg  = await _aboutBannerImgService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var promos = await _aboutPromoService.GetAllAsync();  
            var setting = await _settingService.GetAllAsync();
            var productReviews = await _reviewService.GetAllAsync();

            AboutVM model = new()
            {
                AboutBannerImgs = bannerImg,
                Brands = brands,
                AboutPromos = promos,
                Setting = setting,
                Reviews = productReviews
            };
            return View(model);
        }
    }
}
