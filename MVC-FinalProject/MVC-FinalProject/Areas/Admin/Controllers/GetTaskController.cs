using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Task;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
	[Authorize(Roles = "SuperAdmin,Admin")]
	[Area("Admin")]
    public class GetTaskController : Controller
    {
        private readonly ITaskService _taskService;
        public GetTaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name; 
            var tasks = await _taskService.GetTasksByUserAsync(username);
            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Complete([FromBody]CompleteTask model)
        {
            try
            {
                await _taskService.CompleteTaskAsync(model);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Something wrong.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkTaskAsSeen([FromBody] MarkSeen model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            try
            {
                await _taskService.MarkTaskAsSeenAsync(model);
                return Json(new { success = true, message = "Task marked as seen." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
