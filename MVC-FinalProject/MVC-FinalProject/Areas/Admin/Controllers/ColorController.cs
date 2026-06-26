using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.Instagram;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _colorService.GetAllAsync();
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
        public async Task<IActionResult> Create(ColorCreate request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _colorService.CreateAsync(request);


            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                if (content.Contains("This color has already exist"))
                {
                    ModelState.AddModelError("Name", "This color has already exists.");
                    return View(request);
                }

                ModelState.AddModelError("", "Something went wrong while creating the color.");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _colorService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _colorService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await  _colorService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new ColorEdit
            {
                Id = id,
               Name = res.Name,
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ColorEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _colorService.EditAsync(request, id);

            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                if (content.Contains("This color name already exists"))
                {
                    ModelState.AddModelError("Name", "This color name already exists.");
                    return View(request);
                }

                ModelState.AddModelError("", "Error editing color.");
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
