using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    [Area("Admin")]
    public class BlogReviewController : Controller
    {
        private readonly IBlogReviewService _blogReviewService;
        public BlogReviewController(IBlogReviewService blogReviewService)
        {
            _blogReviewService = blogReviewService;
        }
        public async Task<IActionResult> Index()
        {
            var reviews = await _blogReviewService.GetAllAsync();
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _blogReviewService.DeleteReviewAsync(id);
            return Ok();
        }
    }
}