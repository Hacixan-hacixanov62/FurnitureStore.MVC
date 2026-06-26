using MVC_FinalProject.Helpers.Constants;
using System.Diagnostics.Metrics;
using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class ColorService : IColorService
    {
        private readonly HttpClient _httpClient;
        public ColorService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(ColorCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.ColorUrl}Create", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.ColorUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(ColorEdit model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.ColorUrl}Edit/{id}", model);
        }

        public async Task<IEnumerable<Color>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Color>>($"{Urls.ColorUrl}GetAll");
        }

        public async Task<Color> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Color>($"{Urls.ColorUrl}GetById/{id}");
        }
    }
}
