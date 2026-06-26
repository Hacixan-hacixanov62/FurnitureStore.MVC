using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IProductService _productService;

        public DashboardController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult>Index(string search)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<Product> products = Enumerable.Empty<Product>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = await _productService.SearchByProductNameAsync(search);
            }

            return View(products);
        }
    }
}
