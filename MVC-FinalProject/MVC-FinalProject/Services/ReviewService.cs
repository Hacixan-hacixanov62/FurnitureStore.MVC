using System.Net.Http.Headers;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Review;
using MVC_FinalProject.Services.Interfaces;   

namespace MVC_FinalProject.Services
{
    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;

        public ReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
           
            return await _httpClient.GetFromJsonAsync<IEnumerable<Review>>($"{Urls.ReviewUrl}GetAllReviews");
        }

        public async Task<HttpResponseMessage> DeleteReviewAsync(int reviewid)
        {
           
            return await _httpClient.DeleteAsync($"{Urls.ReviewUrl}DeleteReview?id={reviewid}");
        }


        public async Task<Review> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Review>($"{Urls.ReviewClientUrl}{id}");
        }

        public async Task<HttpResponseMessage> CreateAsync(ReviewCreateApi model, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Urls.ReviewClientUrl}CreateReview")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> EditAsync(ReviewEditApi model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.ReviewClientUrl}Edit/{id}", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Urls.ReviewClientUrl}DeleteReview/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        public Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Review>> GetAllByProductIdAsync(int productId)
        {
            var url = $"{Urls.ReviewClientUrl}GetAllByProductId?productId={productId}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}, URL: {url}");
                    return Enumerable.Empty<Review>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<Review>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, URL: {url}");
                return Enumerable.Empty<Review>();
            }
        }
    }
}
