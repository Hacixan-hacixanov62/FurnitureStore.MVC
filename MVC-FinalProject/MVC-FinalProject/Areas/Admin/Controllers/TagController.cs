using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Tag;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedProducts = await _tagService.GetPaginatedAsync(page, pageSize);
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
        public async Task<IActionResult> Create(TagCreate request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _tagService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _tagService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _tagService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var bannerImg = new TagEdit
            {
                Id = id,
                Name = res.Name,
            };
            return View(bannerImg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _tagService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
