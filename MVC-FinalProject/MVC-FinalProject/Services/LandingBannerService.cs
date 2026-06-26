using System.Net.Http.Headers;
using MVC_FinalProject.Helpers;
using System.Text.Json;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.LandingBanner;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class LandingBannerService : ILandingBannerService
    {
        private readonly HttpClient _httpClient;
        public LandingBannerService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(LandingBannerCreate model)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                // Add the fields from SliderCreateVM to the multipart content
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");

                // Check if the file is provided in the form data
                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
                        multipartContent.Add(fileContent, "Image", model.Image.FileName);
                    }
                }
                return await _httpClient.PostAsync($"{Urls.LandingBannerUrl}Create", multipartContent);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.LandingBannerUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(LandingBannerEdit model, int id)
        {

            using (var multipartContent = new MultipartFormDataContent())
            {
                // Add the fields from SliderCreateVM to the multipart content
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");

                // Check if the file is provided in the form data
                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
                        multipartContent.Add(fileContent, "Image", model.Image.FileName);
                    }
                }
                return await _httpClient.PutAsync($"{Urls.LandingBannerUrl}Edit/{id}", multipartContent);
            }
        }

        public async Task<IEnumerable<LandingBanner>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<LandingBanner>>($"{Urls.LandingBannerUrl}GetAll");
        }

        public async Task<LandingBanner> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<LandingBanner>($"{Urls.LandingBannerUrl}GetById/{id}");
        }

        public async Task<PaginationResponse<LandingBanner>> GetPaginatedAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.LandingBannerUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<LandingBanner>.Create(new List<LandingBanner>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<LandingBanner>>(json, options);
            return PaginationResponse<LandingBanner>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }
    }
}
