using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Instagram;
using MVC_FinalProject.Models.LandingBanner;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IInstagramService
    {
        Task<HttpResponseMessage> CreateAsync(InstagramCreate model);
        Task<HttpResponseMessage> EditAsync(InstagramEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Instagram>> GetAllAsync();
        Task<Instagram> GetByIdAsync(int id);
    }
}
