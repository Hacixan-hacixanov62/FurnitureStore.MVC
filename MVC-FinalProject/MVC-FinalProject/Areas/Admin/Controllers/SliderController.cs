using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.Response;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
           _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var paginatedProducts = await _sliderService.GetPaginatedAsync(page, pageSize);
            ViewBag.TotalCount =paginatedProducts.TotalCount;

            return View(paginatedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreate request)
        {
            if (!ModelState.IsValid) return View(request);

            var response = await _sliderService.CreateAsync(request);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            if (response.StatusCode == HttpStatusCode.Forbidden)
                return View("AccessDenied");

            ModelState.AddModelError(string.Empty, "Error creating slider.");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.DeleteAsync(id);
            //return RedirectToAction(nameof(Index));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null) return NotFound();

            var sliderEdit = new SliderEdit
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description,
                ImageUrl = slider.Image          
            };       
            return View(sliderEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _sliderService.EditAsync(request, id);
            if (result.StatusCode == HttpStatusCode.Forbidden)
                return View("AccessDenied");
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
