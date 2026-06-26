using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.AboutPromo;
using MVC_FinalProject.Models.Slider;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IAboutPromoService
    {
        Task<HttpResponseMessage> CreateAsync(AboutPromoCreate model);
        Task<HttpResponseMessage> EditAsync(AboutPromoEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<AboutPromo>> GetAllAsync();
        Task<AboutPromo> GetByIdAsync(int id);
    }
}
