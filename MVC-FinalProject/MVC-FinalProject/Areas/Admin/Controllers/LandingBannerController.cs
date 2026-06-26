using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.LandingBanner;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class LandingBannerController : Controller
    {
        private readonly ILandingBannerService _landingBannerService;

        public LandingBannerController(ILandingBannerService landingBannerService)
        {
            _landingBannerService = landingBannerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedProducts = await _landingBannerService.GetPaginatedAsync(page, pageSize);
            ViewBag.TotalCount = paginatedProducts.TotalCount;
            return View(paginatedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LandingBannerCreate request)
        {
            if(!ModelState.IsValid) return View(request);
            var result = await _landingBannerService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var city = await _landingBannerService.GetByIdAsync(id);
            if (city == null) return NotFound();
            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _landingBannerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var city = await _landingBannerService.GetByIdAsync(id);
            if (city == null) return NotFound();

            var cityEdit = new LandingBannerEdit
            {
                Id = city.Id,
                Title = city.Title,
                Description = city.Description,
                ImageUrl = city.Image
            };
            return View(cityEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LandingBannerEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _landingBannerService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
