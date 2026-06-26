using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.Response;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class BlogCategoryController : Controller
    {
        private readonly IBlogCategoryService _blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _blogCategoryService.GetAllAsync();
            ViewBag.TotalCount = categories.Count();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCategoryCreate request)
        {
            if(!ModelState.IsValid) return View(request);
            var result = await _blogCategoryService.CreateAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                if (content.Contains("This Category has already exist"))
                {
                    ModelState.AddModelError("Name", "This category has already exists.");
                    return View(request);
                }

                ModelState.AddModelError("", "Something went wrong while creating the category.");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _blogCategoryService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogCategoryService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _blogCategoryService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var category = new BlogCategoryEdit
            {
                Id = id,
                Name = res.Name,
            };
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogCategoryEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _blogCategoryService.EditAsync(request, id);

            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                if (content.Contains("This category name already exists"))
                {
                    ModelState.AddModelError("Name", "This category name already exists.");
                    return View(request);
                }

                ModelState.AddModelError("", "Error editing category.");
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
