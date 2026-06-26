using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.AskUsFrom;
using MVC_FinalProject.Models.Subscription;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHelpSectionService _helpSectionService;
        private readonly IAskUsFromService _askUsFromService;
        private readonly ISettingService _settingService;
        public ContactController(IHelpSectionService helpSectionService, IAskUsFromService askUsFromService, ISettingService settingService)
        {
            _helpSectionService = helpSectionService;
            _askUsFromService = askUsFromService;
            _settingService = settingService;
        }
        public async Task<IActionResult> Index()
        {
            var helpSections = await _helpSectionService.GetAllAsync();
            var setting = await _settingService.GetAllAsync();


            ContactVM model = new()
            {
                HelpSections = helpSections,
                Setting = setting
                
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] string email,[FromForm] string name, [FromForm] string message)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, message = "Email is required." });

            try
            {
                var dto = new AskUsFromCreate { Name = name,Email = email, Message = message};
                var response = await _askUsFromService.CreateQuestionAsync(dto);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Your message has been sent successfully !" });
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                if (errorMessage.Contains("First be register"))
                    return Json(new { success = false, message = "Please register before sending a message." });

                return Json(new { success = false, message = "An error occurred. Please try again later." });
            }
            catch
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}
