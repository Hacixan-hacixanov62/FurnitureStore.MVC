using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.SubscribeImg;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class SubscribeImgController : Controller
    {
        private readonly ISubscribeImgService _subscribeImgService;
        public SubscribeImgController(ISubscribeImgService subscribeImgService)
        {
            _subscribeImgService = subscribeImgService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _subscribeImgService.GetAllAsync();
            ViewBag.TotalCount = res.Count();

            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscribeImgCreate request)
        {
            if(!ModelState.IsValid) return View(request);
            var result = await _subscribeImgService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _subscribeImgService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _subscribeImgService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _subscribeImgService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new SubscribeImgEdit
            {
                Id = res.Id,
                ImageUrl = res.Img
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubscribeImgEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _subscribeImgService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
