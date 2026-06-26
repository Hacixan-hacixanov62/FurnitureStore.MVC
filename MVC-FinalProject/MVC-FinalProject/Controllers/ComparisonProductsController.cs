using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Controllers
{
    public class ComparisonProductsController : Controller
    {
        private readonly IProductService _productService;

        public ComparisonProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComparisonProducts(int categoryId, int selectedProduct)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category ID");
            }

            var comparisonProducts = await _productService.GetComparisonProductsAsync(categoryId, selectedProduct);

            if (comparisonProducts == null || !comparisonProducts.Any())
            {
                return NotFound("No comparison products found for the given category.");
            }

            return View("GetComparisonProducts", comparisonProducts);
        }

    }
}
