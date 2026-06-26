using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.HelpSection;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class HelpSectionService : IHelpSectionService
    {
        private readonly HttpClient _httpClient;
        public HelpSectionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> CreateAsync(HelpSectionCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.HelpSectionUrl}Create", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.HelpSectionUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(HelpSectionEdit model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.HelpSectionUrl}Edit/{id}", model);
        }

        public async Task<IEnumerable<HelpSection>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<HelpSection>>($"{Urls.HelpSectionUrl}GetAll");
        }

        public async Task<HelpSection> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<HelpSection>($"{Urls.HelpSectionUrl}GetById/{id}");
        }
    }
}
