using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class BlogPostController : Controller
    {
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogCategoryService blogCategoryService,
                                 IBlogPostService blogPostService)
        {
            _blogCategoryService = blogCategoryService;
            _blogPostService = blogPostService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var blogPosts = await _blogPostService.GetPaginatedAsync(page, pageSize);
            ViewBag.TotalCount = blogPosts.TotalCount;
            var categories = await _blogCategoryService.GetAllAsync();
            ViewBag.BlogCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(blogPosts);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _blogCategoryService.GetAllAsync();
            ViewBag.BlogCategories = new SelectList(categories, "Id", "Name");
            return View(new BlogPostCreate());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogPostCreate model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _blogCategoryService.GetAllAsync();
                ViewBag.BlogCategories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            var response = await _blogPostService.CreateAsync(model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "An error occurred while creating the blog post.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var blogPost = await _blogPostService.GetByIdAsync(id);
            if (blogPost == null) return NotFound();
            var category = await _blogCategoryService.GetByIdAsync(blogPost.BlogCategoryId);
            ViewBag.BlogCategory = category?.Name ?? "Unknown";
            return View(blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogPostService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blogPost = await _blogPostService.GetByIdAsync(id);
            if (blogPost == null) return NotFound();

            var categories = await _blogCategoryService.GetAllAsync();
            ViewBag.BlogCategories = new SelectList(categories, "Id", "Name");

            var model = new BlogPostEdit
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Description = blogPost.Description,
                HighlightText = blogPost.HighlightText,
                BlogCategoryId = blogPost.BlogCategoryId,
                ExistingImages = blogPost.Images ?? new List<BlogPostImg>(),
                MainImageId = blogPost.Images.FirstOrDefault(i => i.IsMain)?.Id
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogPostEdit model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _blogCategoryService.GetAllAsync();
                ViewBag.BlogCategories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            var response = await _blogPostService.EditAsync(id, model, model.MainImageId);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error updating blog post.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int blogPostId, int blogPostImageId)
        {
            var response = await _blogPostService.DeleteImageAsync(blogPostId, blogPostImageId);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { message = "Error deleting image." });
            }

            return Ok(new { message = "Image deleted successfully." }); // ✅ Burada JSON qaytarırıq, redirect yox!
        }


    }
}