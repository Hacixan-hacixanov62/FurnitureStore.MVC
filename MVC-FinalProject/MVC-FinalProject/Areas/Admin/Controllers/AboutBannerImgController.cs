using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class AboutBannerImgController : Controller
    {
        private readonly IAboutBannerImgService _bannerService;
        public AboutBannerImgController(IAboutBannerImgService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAllAsync();
            ViewBag.TotalCount = banners.Count();
            return View(banners);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutBannerImgCreate request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _bannerService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _bannerService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bannerService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _bannerService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new AboutBannerImgEdit
            {
                Id = res.Id,              
                ImageUrl = res.Img
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutBannerImgEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _bannerService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
