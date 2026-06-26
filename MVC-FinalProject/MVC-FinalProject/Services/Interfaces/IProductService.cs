using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.Product;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginationResponse<Product>> GetPaginatedProductsAsync(int page, int pageSize);
        //Task<Dictionary<string, object>> GetAllDropdownDataAsync();
        Task<HttpResponseMessage> CreateAsync(ProductCreate model);
        Task<HttpResponseMessage> EditAsync(int id, ProductEdit model);
        Task<HttpResponseMessage> DeleteImageAsync(int productId, int productImageId);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<ProductWithImage> GetByIdWithImagesAsync(int id);
        Task<IEnumerable<Product>> GetAllTakenAsync(int take, int? skip = null);
        Task<int> GetProductsCountAsync();

        Task<IEnumerable<Product>> FilterAsync(string categoryName, string colorName, string tagName, string brandName);
        Task<IEnumerable<Product>> GetSortedProductsAsync(string sortType);

        Task<IEnumerable<Product>> SearchByNameAsync(string search);
        Task<IEnumerable<Product>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice);
        Task<IEnumerable<Product>> GetComparisonProductsAsync(int categoryId, int selectedProduct);
        Task<IEnumerable<Product>> SearchByProductNameAsync(string search);
        Task<decimal> GetMaxPriceAsync();
    }
}
