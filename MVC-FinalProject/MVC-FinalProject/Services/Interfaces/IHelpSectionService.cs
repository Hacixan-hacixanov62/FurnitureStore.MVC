using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.HelpSection;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IHelpSectionService
    {
        Task<HttpResponseMessage> CreateAsync(HelpSectionCreate model);
        Task<HttpResponseMessage> EditAsync(HelpSectionEdit model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<IEnumerable<HelpSection>> GetAllAsync();
        Task<HelpSection> GetByIdAsync(int id);
    }
}
