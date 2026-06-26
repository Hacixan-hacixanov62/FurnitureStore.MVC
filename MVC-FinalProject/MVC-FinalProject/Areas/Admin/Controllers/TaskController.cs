using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.Task;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("Admin")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IAccountService _accountService;
        public TaskController(ITaskService taskService, IAccountService accountService)
        {
            _taskService = taskService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllAsync();
            return View(tasks);
        }

        [HttpGet]
        public async  Task<IActionResult> Create()
        {
            var users = await _accountService.GetAllUsersAsync();
            var adminUsernames = users
        .Where(u => u.Roles.Contains("Admin"))
        .Select(u => new SelectListItem
        {
            Value = u.Username,
            Text = u.Username}).ToList();
            var model = new CreateTask
            {
                AdminUsers = adminUsernames
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTask model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _accountService.GetAllUsersAsync();
                model.AdminUsers = users
                    .Where(u => u.Roles.Contains("Admin"))
                    .Select(u => new SelectListItem
                    {
                        Value = u.Username,
                        Text = u.Username
                    }).ToList();

                return View(model);
            }

            var dto = new CreateTaskApi
            {
                Title = model.Title,
                Description = model.Description,
                AssignedTo = model.SelectedUsername 
            };

            try
            {
                await _taskService.CreateTaskAsync(dto);
                TempData["Success"] = "Yeni tapşırıq uğurla yaradıldı!";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Xəta baş verdi: {ex.Message}");

                var users = await _accountService.GetAllUsersAsync();
                model.AdminUsers = users
                    .Where(u => u.Roles.Contains("Admin"))
                    .Select(u => new SelectListItem
                    {
                        Value = u.Username,
                        Text = u.Username
                    }).ToList();

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _taskService.DeleteAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
