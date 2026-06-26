using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Color;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IColorService
    {
        Task<HttpResponseMessage> CreateAsync(ColorCreate model);
        Task<HttpResponseMessage> EditAsync(ColorEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Color>> GetAllAsync();
        Task<Color> GetByIdAsync(int id);
    }
}
