using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Response;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _brandService.GetAllAsync();
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
        public async Task<IActionResult> Create(BrandCreate request)
        {
            if(!ModelState.IsValid) return View(request);
            var result = await _brandService.CreateAsync(request);


            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                if (content.Contains("This brand has already exist"))
                {
                    ModelState.AddModelError("Name", "This brand already exists.");
                    return View(request);
                }

                ModelState.AddModelError("", "Something went wrong while creating the brand.");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _brandService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _brandService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var brandEdit = new BrandEdit
            {
                Id = res.Id,
                Name = res.Name,
                ImageUrl = res.Image

            };
            return View(brandEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await  _brandService.EditAsync(request, id);


            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                if (content.Contains("This brand name already exists"))
                {
                    ModelState.AddModelError("Name", "This brand name already exists.");
                    return View(request);
                }

                ModelState.AddModelError("", "Error editing brand.");
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
