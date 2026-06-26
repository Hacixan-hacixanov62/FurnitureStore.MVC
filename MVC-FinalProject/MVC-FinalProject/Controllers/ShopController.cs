using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;
using System.Linq;

namespace MVC_FinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IBrandService _brandService;
        private readonly ISettingService _settingService;
        public ShopController(IProductService productService, 
                              ICategoryService categoryService,
                              IBrandService brandService,
                              ITagService tagService,
                              ISettingService settingService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _settingService = settingService;
        }


		public async Task<IActionResult> Index(
	string categoryName,
	string colorName,
	string tagName,
	string brandName,
	string sortType,
	decimal? minPrice,
	decimal? maxPrice)
		{
			var categories = await _categoryService.GetAllAsync();
			var brands = await _brandService.GetAllAsync();
			var tags = await _tagService.GetAllAsync();
			var productCount = await _productService.GetProductsCountAsync();
			var categoryProductCounts = await _categoryService.GetCategoryProductCountsAsync();
			var brandProductCounts = await _brandService.GetBrandProductCountsAsync();
			var setting = await _settingService.GetAllAsync();

			var maxProductPrice = await _productService.GetMaxPriceAsync();
			ViewBag.MaxPrice = maxProductPrice;

			List<Product> products;

			// Əgər category, brand, color və tag filtrləri varsa
			if (!string.IsNullOrEmpty(categoryName) ||
				!string.IsNullOrEmpty(colorName) ||
				!string.IsNullOrEmpty(tagName) ||
				!string.IsNullOrEmpty(brandName))
			{
				products = (await _productService.FilterAsync(categoryName, colorName, tagName, brandName)).ToList();
			}
			else if (minPrice.HasValue || maxPrice.HasValue)
			{
				// Əgər yalnız price filter varsa
				products = (await _productService.FilterByPriceAsync(minPrice, maxPrice)).ToList();
			}
			else
			{
				// Əsas səhifə yüklənəndə
				products = (await _productService.GetAllTakenAsync(6, 0)).ToList();
			}

			// Sort varsa, onu tətbiq et (yalnız ümumi sort üçün, filter-sort birlikdə istənmirsə, bunu ayrıca idarə edə bilərik)
			if (!string.IsNullOrEmpty(sortType) &&
				string.IsNullOrEmpty(categoryName) &&
				string.IsNullOrEmpty(colorName) &&
				string.IsNullOrEmpty(tagName) &&
				string.IsNullOrEmpty(brandName) &&
				!minPrice.HasValue && !maxPrice.HasValue)
			{
				products = (await _productService.GetSortedProductsAsync(sortType)).ToList();
			}

			// Show More yalnız filter və sort yoxdursa görünsün
			bool isFilteredOrSorted =
				!string.IsNullOrEmpty(categoryName) ||
				!string.IsNullOrEmpty(colorName) ||
				!string.IsNullOrEmpty(tagName) ||
				!string.IsNullOrEmpty(brandName) ||
				!string.IsNullOrEmpty(sortType) ||
				minPrice.HasValue || maxPrice.HasValue;

			ViewBag.ProductsCount = productCount;
			ViewBag.CategoryProductCounts = categoryProductCounts;
			ViewBag.BrandProductCounts = brandProductCounts;

			var model = new ShopVM
			{
				Categories = categories,
				Brands = brands,
				Tags = tags,
				Products = products,
				TotalProductCount = productCount,
				Setting = setting,
				ShowMoreVisible = !isFilteredOrSorted
			};

			return View(model);
		}

		[HttpGet]
        public async Task<IActionResult> ShowMore(int skip)
        {
            var products = await _productService.GetAllTakenAsync(6, skip);

            if (products == null || !products.Any())
            {
                return Content("");
            }

            return PartialView("_ProductPartial", products);
        }

    }
}
