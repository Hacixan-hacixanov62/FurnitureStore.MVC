using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.Slider;
using System.Reflection.Metadata;
using System.Net.Http.Headers;
using System.Net.Http;
using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.LandingBanner;
using System.Text.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace MVC_FinalProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SliderService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> CreateAsync(SliderCreate model)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");

                if (model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        multipartContent.Add(fileContent, "Image", model.Image.FileName);
                    }
                }

                return await _httpClient.PostAsync($"{Urls.SliderUrl}Create", multipartContent);
            }
        }


        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.DeleteAsync($"{Urls.SliderUrl}Delete?id={id}");
        }

        //public async Task<HttpResponseMessage> EditAsync(SliderEdit model, int id)
        //{

        //    using (var multipartContent = new MultipartFormDataContent())
        //    {
        //        // Add the fields from SliderCreateVM to the multipart content
        //        multipartContent.Add(new StringContent(model.Title), "Title");
        //        multipartContent.Add(new StringContent(model.Description), "Description");

        //        // Check if the file is provided in the form data
        //        if (model.Image != null && model.Image.Length > 0)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                await model.Image.CopyToAsync(memoryStream);
        //                byte[] fileBytes = memoryStream.ToArray();

        //                var fileContent = new ByteArrayContent(fileBytes);
        //                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // Set the correct media type if known
        //                multipartContent.Add(fileContent, "Image", model.Image.FileName);
        //            }
        //        }
        //        return await _httpClient.PutAsync($"{Urls.SliderUrl}Edit/{id}", multipartContent);
        //    }         
        //}

        public async Task<HttpResponseMessage> EditAsync(SliderEdit model, int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

            using var request = new HttpRequestMessage(HttpMethod.Put, $"{Urls.SliderUrl}Edit/{id}");
            using var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new StringContent(model.Title), "Title");
            multipartContent.Add(new StringContent(model.Description), "Description");

            if (model.Image != null && model.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await model.Image.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                multipartContent.Add(fileContent, "Image", model.Image.FileName);
            }

            request.Content = multipartContent;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.SendAsync(request);
        }


        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Slider>>($"{Urls.SliderUrl}GetAll");
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Slider>($"{Urls.SliderUrl}GetById/{id}");
        }

        public async Task<PaginationResponse<Slider>> GetPaginatedAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.SliderUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<Slider>.Create(new List<Slider>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<Slider>>(json, options);
            return PaginationResponse<Slider>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }
    }
}