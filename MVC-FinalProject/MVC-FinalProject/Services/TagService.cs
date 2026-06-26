using MVC_FinalProject.Helpers;
using System.Text.Json;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Color;
using MVC_FinalProject.Models.LandingBanner;
using MVC_FinalProject.Models.Tag;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;
        public TagService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> CreateAsync(TagCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.TagUrl}Create", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.TagUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(TagEdit model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.TagUrl}Edit/{id}", model);
        }     

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Tag>>($"{Urls.TagUrl}GetAll");
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Tag>($"{Urls.TagUrl}GetById/{id}");
        }

        public async Task<PaginationResponse<Tag>> GetPaginatedAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.TagUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<Tag>.Create(new List<Tag>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<Tag>>(json, options);
            return PaginationResponse<Tag>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }
    }
}
