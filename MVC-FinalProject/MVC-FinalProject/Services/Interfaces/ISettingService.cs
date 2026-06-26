using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingVM>> GetAllAsync();
        Task<bool> EditAsync(int id, SettingEditVM model);
    }
}
