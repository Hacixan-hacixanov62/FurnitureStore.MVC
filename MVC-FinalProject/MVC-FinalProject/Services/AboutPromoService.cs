using System.Net.Http.Headers;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.AboutPromo;
using MVC_FinalProject.Models.Slider;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class AboutPromoService : IAboutPromoService
    {
        private readonly HttpClient _httpClient;
        public AboutPromoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(AboutPromoCreate model)
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
                return await _httpClient.PostAsync($"{Urls.AboutPromoUrl}Create", multipartContent);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.AboutPromoUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(AboutPromoEdit model, int id)
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
                return await _httpClient.PutAsync($"{Urls.AboutPromoUrl}Edit/{id}", multipartContent);
            }
        }

        public async Task<IEnumerable<AboutPromo>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AboutPromo>>($"{Urls.AboutPromoUrl}GetAll");
        }

        public async Task<AboutPromo> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AboutPromo>($"{Urls.AboutPromoUrl}GetById/{id}");
        }

    }
}
