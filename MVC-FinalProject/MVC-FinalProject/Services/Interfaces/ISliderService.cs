using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.Slider;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ISliderService
    {
        Task<HttpResponseMessage> CreateAsync(SliderCreate model);
        Task<HttpResponseMessage> EditAsync(SliderEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int id);
        Task<PaginationResponse<Slider>> GetPaginatedAsync(int page, int pageSize);
    }
}
