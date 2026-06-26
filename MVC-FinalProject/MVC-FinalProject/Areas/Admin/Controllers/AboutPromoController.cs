using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.AboutPromo;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class AboutPromoController : Controller
    {
        private readonly IAboutPromoService _aboutPromoService;
        public AboutPromoController(IAboutPromoService aboutPromoService)
        {
            _aboutPromoService = aboutPromoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var promos = await _aboutPromoService.GetAllAsync();
            ViewBag.TotalCount = promos.Count();
            return View(promos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutPromoCreate request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _aboutPromoService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var promo = await _aboutPromoService.GetByIdAsync(id);
            if (promo == null) return NotFound();
            return View(promo);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _aboutPromoService.DeleteAsync(id);
            //return RedirectToAction(nameof(Index));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var promo = await _aboutPromoService.GetByIdAsync(id);
            if (promo == null) return NotFound();

            var promoEdit = new AboutPromoEdit
            {
                Id = promo.Id,
                Title = promo.Title,
                Description = promo.Description,
                ImageUrl = promo.Image
            };
            return View(promoEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutPromoEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _aboutPromoService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
