using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Category;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IAboutBannerImgService
    {
        Task<HttpResponseMessage> CreateAsync(AboutBannerImgCreate model);
        Task<HttpResponseMessage> EditAsync(AboutBannerImgEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<AboutBannerImg>> GetAllAsync();
        Task<AboutBannerImg> GetByIdAsync(int id);
    }
}
