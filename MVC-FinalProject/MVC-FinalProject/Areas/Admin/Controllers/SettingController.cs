using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
                _settingService = settingService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _settingService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var settings = await _settingService.GetAllAsync();
            var setting = settings.FirstOrDefault(s => s.Id == id);
            if (setting == null) return NotFound();

            var model = new SettingEditVM
            {
                Key = setting.Key,
                Value = setting.Value
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SettingEditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _settingService.EditAsync(id, model);
            if (!result)
            {
                ModelState.AddModelError("", "Update failed.");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
