using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Slider;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<HttpResponseMessage> CreateAsync(CategoryCreate model);
        Task<HttpResponseMessage> EditAsync(CategoryEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<PaginationResponse<Category>> GetPaginatedProductsAsync(int page, int pageSize);
        Task<Dictionary<string, int>> GetCategoryProductCountsAsync();
    }
}
