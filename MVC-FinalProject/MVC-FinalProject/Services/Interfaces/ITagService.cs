using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.Tag;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ITagService
    {
        Task<HttpResponseMessage> CreateAsync(TagCreate model);
        Task<HttpResponseMessage> EditAsync(TagEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task<PaginationResponse<Tag>> GetPaginatedAsync(int page, int pageSize);
    }
}
