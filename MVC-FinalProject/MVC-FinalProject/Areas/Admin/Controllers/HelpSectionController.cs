using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.HelpSection;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class HelpSectionController : Controller
    {
        private readonly IHelpSectionService _helpSectionService;
        public HelpSectionController(IHelpSectionService helpSectionService)
        {
            _helpSectionService = helpSectionService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _helpSectionService.GetAllAsync();
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HelpSectionCreate request)
        {

            var result = await _helpSectionService.CreateAsync(request);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError(string.Empty, "Error creating");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _helpSectionService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _helpSectionService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _helpSectionService.GetByIdAsync(id);
            if (res == null) return NotFound();

            var helpService = new HelpSectionEdit
            {
                Id = id,
                PhoneNumber = res.PhoneNumber,
                CustomerServiceHours = res.CustomerServiceHours,
                IsWeekendClosed = res.IsWeekendClosed,          
            };
            return View(helpService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HelpSectionEdit request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _helpSectionService.EditAsync(request, id);
            if (result.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Error editing.");
            return View(request);
        }
    }
}
