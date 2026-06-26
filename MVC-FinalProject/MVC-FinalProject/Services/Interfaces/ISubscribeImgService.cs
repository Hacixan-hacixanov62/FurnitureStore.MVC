using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.SubscribeImg;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ISubscribeImgService
    {
        Task<HttpResponseMessage> CreateAsync(SubscribeImgCreate model);
        Task<HttpResponseMessage> EditAsync(SubscribeImgEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<SubscribeImg>> GetAllAsync();
        Task<SubscribeImg> GetByIdAsync(int id);
    }
}
