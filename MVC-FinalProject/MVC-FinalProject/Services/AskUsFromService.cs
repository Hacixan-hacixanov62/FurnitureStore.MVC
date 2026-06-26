using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Subscription;
using System.Net.Http;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.AskUsFrom;

namespace MVC_FinalProject.Services
{
    public class AskUsFromService : IAskUsFromService
    {
        private readonly HttpClient _httpClient;
        public AskUsFromService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> CreateQuestionAsync(AskUsFromCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.AskUsFromClientUrl}Create", model);
        }

        public async Task<IEnumerable<AskUsFrom>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AskUsFrom>>($"{Urls.AskUsFromUrl}GetAll");
        }

        public async Task ApproveMessageAsync(int id)
        {
            var response = await _httpClient.PutAsync($"{Urls.AskUsFromUrl}ApproveMessage?id={id}", null);
            response.EnsureSuccessStatusCode();
        }
        public async Task<IEnumerable<AskUsFrom>> GetApprovedMessagesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AskUsFrom>>($"{Urls.AskUsFromClientUrl}GetApprovedMessages");
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.AskUsFromUrl}Delete?id={id}");
        }
    }
}
