using System.Net.Http.Headers;
using System.Text.Json;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _httpClient;
        public BrandService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> CreateAsync(BrandCreate model)
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
                return await _httpClient.PostAsync($"{Urls.BrandUrl}Create", multipartContent);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.BrandUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(BrandEdit model, int id)
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
                return await _httpClient.PutAsync($"{Urls.BrandUrl}Edit/{id}", multipartContent);
            }
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Brand>>($"{Urls.BrandUrl}GetAll");
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Brand>($"{Urls.BrandUrl}GetById/{id}");
        }

        public async Task<Dictionary<string, int>> GetBrandProductCountsAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.BrandUrl}GetBrandProductCounts");

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
