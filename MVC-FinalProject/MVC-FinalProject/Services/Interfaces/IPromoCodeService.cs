using MVC_FinalProject.Models.PromoCode;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IPromoCodeService
    {
        Task CreateAsync(PromoCodeCreate model);
        Task<List<PromoCodeListItem>> GetAllAsync();
        Task<bool> UsePromoCodeAsync(string code);
        Task<PromoCodeResult> GetByCodeAsync(string code);
        Task<bool> DeleteAsync(int id);
    }
}
