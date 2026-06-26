using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IColorService _colorService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ProductController(
            IProductService productService,
            IColorService colorService,
            ITagService tagService,
            ICategoryService categoryService,
            IBrandService brandService)
        {
            _productService = productService;
            _colorService = colorService;
            _tagService = tagService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        private async Task PopulateDropdownsAsync()
        {
            var colors = await _colorService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Colors = colors.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Tags = tags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            ViewBag.Brands = brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            ViewBag.Categories = categories.Select(cat => new SelectListItem
            {
                Value = cat.Id.ToString(),
                Text = cat.Name
            }).ToList();
        }

        private async Task<List<ProductImage>> GetProductImagesForView(int id)
        {
            var product = await _productService.GetByIdWithImagesAsync(id);
            return product.ProductImages
                .Select(img => new ProductImage
                {
                    Id = img.Id,
                    Img = img.Img,
                    IsMain = img.IsMain
                })
                .ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var paginatedProducts = await _productService.GetPaginatedProductsAsync(page, pageSize);
            ViewBag.TotalCount =paginatedProducts.TotalCount;

            return View(paginatedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreate model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(); 
                return View(model);
            }
            var response = await _productService.CreateAsync(model);

            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "Product creation failed.");
            await PopulateDropdownsAsync();
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteAsync(id);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }          
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdWithImagesAsync(id);

            foreach (var img in product.ProductImages)
            {
                System.Diagnostics.Debug.WriteLine($"Id={img.Id}, Img='{img.Img}', IsMain={img.IsMain}");
            }

            if (product == null) return NotFound();

            var allTags = await _tagService.GetAllAsync();
            var allColors = await _colorService.GetAllAsync();
            var allBrands = await _brandService.GetAllAsync();
            var allCategories = await _categoryService.GetAllAsync();

            var tagIds = allTags.Where(t => product.Tags.Contains(t.Name)).Select(t => t.Id).ToList();
            var colorIds = allColors.Where(c => product.Colors.Contains(c.Name)).Select(c => c.Id).ToList();
            var brandId = allBrands.FirstOrDefault(b => b.Name == product.Brand)?.Id ?? 0;
            var categoryId = allCategories.FirstOrDefault(c => c.Name == product.Category)?.Id ?? 0;

            var editModel = new ProductEdit
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
                Description = product.Description,
                BrandId = brandId,
                CategoryId = categoryId,
                TagIds = tagIds,
                ColorIds = colorIds,
                MainImageId = product.ProductImages.FirstOrDefault(i => i.IsMain)?.Id
            };

            await PopulateDropdownsAsync();

            ViewBag.ExistingImages = product.ProductImages
                .Select(img => new ProductImage
                {
                    Id = img.Id,
                    Img = img.Img,
                    IsMain = img.IsMain
                })
                .ToList();


            return View(editModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductEdit model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                ViewBag.ExistingImages = await GetProductImagesForView(id);
                return View(model);
            }

            var response = await _productService.EditAsync(id, model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Update failed. API response: {error}");

            await PopulateDropdownsAsync();
            ViewBag.ExistingImages = await GetProductImagesForView(id);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteImage(int productId, int productImageId)
        {
            var response = await _productService.DeleteImageAsync(productId, productImageId);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return BadRequest(new { message = "Error deleting image.", error = errorMsg });
            }

            return Ok(new { message = "Image deleted successfully." });
        }

    }
}