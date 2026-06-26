using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedProducts = await _categoryService.GetPaginatedProductsAsync(page, pageSize);
            ViewBag.TotalCount = paginatedProducts.TotalCount;
            return View(paginatedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreate model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _categoryService.CreateAsync(model);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("This category has already exist"))
                {
                    ModelState.AddModelError("Name", "This category already exists.");
                    return View(model);
                }

                ModelState.AddModelError("", "Something went wrong while creating the category.");
                return View(model);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _categoryService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await  _categoryService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var categoryEdit = new CategoryEdit
            {
                Id = res.Id,
                Name = res.Name,
                ImageUrl = res.Image
               
            };
            return View(categoryEdit);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEdit request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _categoryService.EditAsync(request, id);

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
