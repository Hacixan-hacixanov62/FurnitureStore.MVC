using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using MVC_FinalProject.Helpers;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MVC_FinalProject.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly HttpClient _httpClient;
        public BlogPostService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> CreateAsync(BlogPostCreate model)
        {
            using (var multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Add(new StringContent(model.Title), "Title");
                multipartContent.Add(new StringContent(model.Description), "Description");
                multipartContent.Add(new StringContent(model.HighlightText), "HighlightText");
                multipartContent.Add(new StringContent(model.BlogCategoryId.ToString()), "BlogCategoryId");
                if (model.Images != null && model.Images.Count > 0)
                {
                    foreach (var image in model.Images)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            byte[] fileBytes = memoryStream.ToArray();

                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                            multipartContent.Add(fileContent, "Images", image.FileName);
                        }
                    }
                }
                return await _httpClient.PostAsync($"{Urls.BlogPostUrl}Create", multipartContent);
            }
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<BlogPost>>($"{Urls.BlogPostUrl}GetAll");
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Urls.BlogPostUrl}GetById/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<BlogPost>();
        }
        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.BlogPostUrl}Delete?id={id}");
        }

        public async Task<HttpResponseMessage> EditAsync(int id, BlogPostEdit model, int? mainImageId)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Title), "Title");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.HighlightText), "HighlightText");
            content.Add(new StringContent(model.BlogCategoryId.ToString()), "BlogCategoryId");
            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileContent = new ByteArrayContent(ms.ToArray());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(fileContent, "Images", image.FileName);
                }
            }
            if (mainImageId.HasValue)
                content.Add(new StringContent(mainImageId.Value.ToString()), "MainImageId");
            var response = await _httpClient.PutAsync($"{Urls.BlogPostUrl}Edit?id={id}", content);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteImageAsync(int blogPostId, int blogPostImageId)
        {
            return await _httpClient.DeleteAsync($"{Urls.BlogPostUrl}DeleteImage?blogPostId={blogPostId}&blogPostImageId={blogPostImageId}");           
        }

        public async Task<PaginationResponse<BlogPost>> GetPaginatedAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.BlogPostClientUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<BlogPost>.Create(new List<BlogPost>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<BlogPost>>(json, options);
            return PaginationResponse<BlogPost>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }

        public async Task<IEnumerable<BlogPost>> SearchByCategoryAndName(string name)
        {
            var response = await _httpClient.GetAsync($"{Urls.BlogPostClientUrl}Searchbyname?name={name}");

            if (!response.IsSuccessStatusCode)
                return new List<BlogPost>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<BlogPost>>(content);
        }


        public async Task<IEnumerable<BlogPost>> FilterAsync(string categoryName)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(categoryName))
                queryParams.Add($"categoryName={Uri.EscapeDataString(categoryName)}");

            var query = string.Join("&", queryParams);
            var url = $"{Urls.BlogPostClientUrl}Filter";

            if (!string.IsNullOrEmpty(query))
                url += "?" + query;

            var response = await _httpClient.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();
            return await _httpClient.GetFromJsonAsync<IEnumerable<BlogPost>>(url);
        }

        public async Task<BlogPost> GetPreviousAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Urls.BlogPostClientUrl}GetPrevious/get-previous/{id}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BlogPost>(content);
        }
   
        public async Task<BlogPost> GetNextAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Urls.BlogPostClientUrl}GetNext/get-next/{id}");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BlogPost>(content);
        }

    }
}
