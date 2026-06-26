using MVC_FinalProject.Models.PromoCode;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly HttpClient _httpClient;
        private const string PromoCodeCreateUrl = "https://localhost:7004/api/admin/PromoCode/Create";
        private const string PromoCodeGetAllUrl = "https://localhost:7004/api/admin/PromoCode/GetAll";
        private const string PromoCodeGetByCodeUrl = "https://localhost:7004/api/admin/PromoCode/GetByCode/";
        private const string PromoCodeDeleteUrl = "https://localhost:7004/api/admin/PromoCode/Delete?id=";

        public PromoCodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(PromoCodeCreate model)
        {
            var response = await _httpClient.PostAsJsonAsync(PromoCodeCreateUrl, model);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<PromoCodeListItem>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(PromoCodeGetAllUrl);
            response.EnsureSuccessStatusCode();
            var promos = await response.Content.ReadFromJsonAsync<List<PromoCodeListItem>>();
            return promos ?? new List<PromoCodeListItem>();
        }

        public async Task<bool> UsePromoCodeAsync(string code)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7004/api/admin/PromoCode/UsePromoCode/use/{code}", null);

            return response.IsSuccessStatusCode;
        }

        public async Task<PromoCodeResult> GetByCodeAsync(string code)
        {
            var response = await _httpClient.GetAsync(PromoCodeGetByCodeUrl + code);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<PromoCodeResult>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(PromoCodeDeleteUrl + id);
            return response.IsSuccessStatusCode;
        }
    }

}
