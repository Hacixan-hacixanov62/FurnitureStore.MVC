using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Review;
using System.Net.Http.Headers;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.BlogReview;
using Newtonsoft.Json;
using System.Text;

namespace MVC_FinalProject.Services
{
    public class BlogReviewService : IBlogReviewService
    {
        private readonly HttpClient _httpClient;

        public BlogReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BlogReview>> GetAllAsync()
        {

            return await _httpClient.GetFromJsonAsync<IEnumerable<BlogReview>>($"{Urls.BlogReviewUrl}GetAllReviews");
        }

        public async Task<HttpResponseMessage> DeleteReviewAsync(int reviewid)
        {

            return await _httpClient.DeleteAsync($"{Urls.BlogReviewUrl}DeleteReview?id={reviewid}");
        }


        public async Task<BlogReview> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<BlogReview>($"{Urls.BlogReviewClientUrl}{id}");
        }

        public async Task<HttpResponseMessage> CreateAsync(BlogReviewCreateApi model, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Urls.BlogReviewClientUrl}CreateReview")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

      


        public async Task<HttpResponseMessage> EditAsync(BlogReviewEditApi model, int id)
        {
            return await _httpClient.PutAsJsonAsync($"{Urls.BlogReviewClientUrl}Edit/{id}", model);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Urls.BlogReviewClientUrl}DeleteReview/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }

        public Task<IEnumerable<BlogReview>> GetByPostIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogReview>> GetAllByPostIdAsync(int postId)
        {
            var url = $"{Urls.BlogReviewClientUrl}GetAllByPostId?postId={postId}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}, URL: {url}");
                    return Enumerable.Empty<BlogReview>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<BlogReview>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, URL: {url}");
                return Enumerable.Empty<BlogReview>();
            }
        }
    }
}
