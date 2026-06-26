using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.LandingBanner;
using MVC_FinalProject.Models.Slider;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ILandingBannerService
    {
        Task<HttpResponseMessage> CreateAsync(LandingBannerCreate model);
        Task<HttpResponseMessage> EditAsync(LandingBannerEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<LandingBanner>> GetAllAsync();
        Task<LandingBanner> GetByIdAsync(int id);
        Task<PaginationResponse<LandingBanner>> GetPaginatedAsync(int page, int pageSize);
    }
}
