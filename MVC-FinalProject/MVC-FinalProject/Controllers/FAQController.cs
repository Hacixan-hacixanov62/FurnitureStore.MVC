using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class FAQController : Controller
    {
        private readonly IAskUsFromService _askUsFromService;
        private readonly ISettingService _settingService;

        public FAQController(IAskUsFromService askUsFromService, ISettingService settingService)
        {
            _askUsFromService = askUsFromService;
            _settingService = settingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var approvedMessages = await _askUsFromService.GetApprovedMessagesAsync();
            var setting = await _settingService.GetAllAsync();

            FAQVM model = new FAQVM()
            {
                AskUsFroms = approvedMessages,
                Setting = setting
            };
            
            return View(model);
        }
    }
}
