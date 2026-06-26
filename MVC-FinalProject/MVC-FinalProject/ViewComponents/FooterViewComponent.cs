using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public FooterViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _settingService.GetAllAsync();
            return View(settings);
        }
    }
}
