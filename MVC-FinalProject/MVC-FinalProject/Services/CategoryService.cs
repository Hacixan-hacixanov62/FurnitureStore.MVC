using System.Net.Http.Headers;
using MVC_FinalProject.Helpers;
using System.Text.Json;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.Brand;

namespace MVC_FinalProject.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> CreateAsync(CategoryCreate model)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                // Add the fields from SliderCreateVM to the multipart content
                multipartContent.Add(new StringContent(model.Name), "Name");
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
                return await _httpClient.PostAsync($"{Urls.CategoryUrl}Create", multipartContent);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.CategoryUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(CategoryEdit model, int id)
        {

            using (var multipartContent = new MultipartFormDataContent())
            {
                // Add the fields from SliderCreateVM to the multipart content
                multipartContent.Add(new StringContent(model.Name), "Name");

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
                return await _httpClient.PutAsync($"{Urls.CategoryUrl}Edit/{id}", multipartContent);
            }
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Category>>($"{Urls.CategoryUrl}GetAll");
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Category>($"{Urls.CategoryUrl}GetById/{id}");
        }
        public async Task<PaginationResponse<Category>> GetPaginatedProductsAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.CategoryUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<Category>.Create(new List<Category>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<Category>>(json, options);
            return PaginationResponse<Category>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }

        public async Task<Dictionary<string, int>> GetCategoryProductCountsAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.CategoryUrl}GetCategoryProductCounts");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not found.");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var productCounts = JsonSerializer.Deserialize<Dictionary<string, int>>(jsonString);

            return productCounts ?? new Dictionary<string, int>();
        }


    }
}
