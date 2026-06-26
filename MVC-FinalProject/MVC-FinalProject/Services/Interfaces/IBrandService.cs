using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Models.Category;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IBrandService
    {
        Task<HttpResponseMessage> CreateAsync(BrandCreate model);
        Task<HttpResponseMessage> EditAsync(BrandEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int id);
        Task<Dictionary<string, int>> GetBrandProductCountsAsync();

    }
}
