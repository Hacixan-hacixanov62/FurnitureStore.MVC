using MVC_FinalProject.Models.Wishlist;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _baseUrl;

        public WishlistService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _baseUrl = "https://localhost:7004/api/Wishlist";
        }

        public async Task<WishlistResult> AddWishlistAsync(Wishlist model)
        {
            // ✅ Session-dan token al
            var token = _contextAccessor.HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                return new WishlistResult
                {
                    Success = false,
                    Message = "Session is time out"
                };
            }

            // ✅ HTTP sorğusu qur
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_baseUrl}/AddWishlist?appUserId={model.AppUserId}&productId={model.ProductId}");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // ✅ FromQuery olduğu üçün content null ola bilər
            request.Content = null;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new WishlistResult
                {
                    Success = false,
                    Message = $"Wishlist əlavə olunmadı: {error}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<WishlistResult>();
            return result;
        }

        public async Task<WishlistItem> GetByUserIdAsync(string userId)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/GetWishlistByUserId/{userId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var wishlist = await response.Content.ReadFromJsonAsync<WishlistItem>();
            return wishlist;
        }
        public async Task<bool> DeleteProductFromWishlistAsync(string userId, int productId)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
                return false;

            var requestUrl = $"{_baseUrl}/DeleteProductFromWishlist/{userId}?productId={productId}";

            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

    }
}
