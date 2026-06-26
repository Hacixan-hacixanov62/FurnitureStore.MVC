using System.Net.Http.Headers;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.AboutBannerImg;
using MVC_FinalProject.Models.Category;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class AboutBannerImgService : IAboutBannerImgService
    {
        private readonly HttpClient _httpClient;
        public AboutBannerImgService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(AboutBannerImgCreate model)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                // Check if the file is provided in the form data
                if (model.Img != null && model.Img.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Img.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
                        multipartContent.Add(fileContent, "Img", model.Img.FileName);
                    }
                }
                return await _httpClient.PostAsync($"{Urls.AboutBannerImgUrl}Create", multipartContent);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.AboutBannerImgUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(AboutBannerImgEdit model, int id)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                if (model.Img != null && model.Img.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Img.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
                        multipartContent.Add(fileContent, "Img", model.Img.FileName);
                    }
                }
                return await _httpClient.PutAsync($"{Urls.AboutBannerImgUrl}Edit/{id}", multipartContent);
            }
        }

        public async Task<IEnumerable<AboutBannerImg>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AboutBannerImg>>($"{Urls.AboutBannerImgUrl}GetAll");
        }

        public async Task<AboutBannerImg> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AboutBannerImg>($"{Urls.AboutBannerImgUrl}GetById/{id}");
        }
    }
}
