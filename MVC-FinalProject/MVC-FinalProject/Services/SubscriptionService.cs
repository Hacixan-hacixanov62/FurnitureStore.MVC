using System.Text;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Response;
using MVC_FinalProject.Models.Subscription;
using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC_FinalProject.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly HttpClient _httpClient;
        public SubscriptionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> SubscribeAsync(SubscriptionCreate model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.SubscriptionClientUrl}Subscribe", model);
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Subscription>>($"{Urls.SubscriptionUrl}GetAllSubscriptions");
        }

        public async Task<HttpResponseMessage> UnsubscribeAsync(string email)
        {
            return await _httpClient.DeleteAsync($"{Urls.SubscriptionUrl}Unsubscribe?email={email}");
        }
    }
}
