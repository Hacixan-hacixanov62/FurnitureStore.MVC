using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.Color;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IBlogCategoryService
    {
        Task<HttpResponseMessage> CreateAsync(BlogCategoryCreate model);
        Task<HttpResponseMessage> EditAsync(BlogCategoryEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<BlogCategory>> GetAllAsync();
        Task<BlogCategory> GetByIdAsync(int id);

        Task<Dictionary<string, int>> GetCategoryPostCountsAsync();

    }
}
