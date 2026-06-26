
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    [Area("Admin")]
    public class AskUsFromController : Controller
    {
        private readonly IAskUsFromService _askUsFromService;
        public AskUsFromController(IAskUsFromService askUsFromService)
        {
            _askUsFromService = askUsFromService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _askUsFromService.GetAllAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _askUsFromService.ApproveMessageAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _askUsFromService.DeleteAsync(id);
            return Ok();
        }
    }
}

