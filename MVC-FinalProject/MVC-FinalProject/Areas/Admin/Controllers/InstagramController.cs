using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Instagram;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class InstagramController : Controller
    {
        private readonly IInstagramService _instagramService;
        public InstagramController(IInstagramService instagramService)
        {
            _instagramService = instagramService; 
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _instagramService.GetAllAsync();
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
        public async Task<IActionResult> Create(InstagramCreate request)
        {
            if(!ModelState.IsValid) return View(request);   
            var result = await _instagramService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _instagramService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _instagramService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _instagramService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new InstagramEdit
            {
                Id = res.Id,
                ImageUrl = res.Img
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InstagramEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _instagramService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
