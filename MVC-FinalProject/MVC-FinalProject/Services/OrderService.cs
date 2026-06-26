using MVC_FinalProject.Models.Order;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        public OrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task CreateOrderAsync(OrderCreate dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7004/api/admin/Order/CreateOrder", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7004/api/admin/Order/GetAll");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7004/api/admin/Order/GetByUser/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }

        public async Task ChangeStatusAsync(ChangeOrderStatus dto)
        {
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7004/api/admin/Order/ChangeStatus", dto);
            response.EnsureSuccessStatusCode();
        }

    }
}
