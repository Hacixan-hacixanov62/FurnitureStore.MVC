using MVC_FinalProject.Helpers.Constants;
using System.Text.Json;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.Models.Product;
using MVC_FinalProject.Helpers;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text;
using System.Net;

namespace MVC_FinalProject.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<PaginationResponse<Product>> GetPaginatedProductsAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{Urls.ProductUrl}GetPaginateDatas?page={page}&take={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return PaginationResponse<Product>.Create(new List<Product>(), 0, page, pageSize);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<PaginationApiResponse<Product>>(json, options);
            return PaginationResponse<Product>.Create(apiResponse.Datas, apiResponse.TotalCount, apiResponse.CurrentPage, pageSize);
        }
        public async Task<HttpResponseMessage> CreateAsync(ProductCreate model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Name), "Name");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.Price.ToString()), "Price");
            content.Add(new StringContent(model.Stock.ToString()), "Stock");
            content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(model.BrandId.ToString()), "BrandId");


            if (model.TagIds != null && model.TagIds.Any(x => x > 0))
            {
                foreach (var tagId in model.TagIds.Where(x => x > 0))
                    content.Add(new StringContent(tagId.ToString()), "TagIds");
            }

            if (model.ColorIds != null)
            {
                foreach (var colorId in model.ColorIds)
                    content.Add(new StringContent(colorId.ToString()), "ColorIds");
            }


            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(byteContent, "Images", image.FileName);
                }
            }
            return await _httpClient.PostAsync($"{Urls.ProductUrl}Create", content);
        }
        public async Task<HttpResponseMessage> EditAsync(int id, ProductEdit model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Name), "Name");
            content.Add(new StringContent(model.Description), "Description");
            content.Add(new StringContent(model.Price.ToString()), "Price");
            content.Add(new StringContent(model.Stock.ToString()), "Stock");
            content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(model.BrandId.ToString()), "BrandId");

            if (model.TagIds != null)
            {
                foreach (var tagId in model.TagIds)
                    content.Add(new StringContent(tagId.ToString()), "TagIds");
            }

            if (model.ColorIds != null)
            {
                foreach (var colorId in model.ColorIds)
                    content.Add(new StringContent(colorId.ToString()), "ColorIds");
            }

            if (model.UploadImages != null)
            {
                foreach (var image in model.UploadImages)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(byteContent, "UploadImages", image.FileName);
                }
            }

            if (model.DeleteImageIds != null)
            {
                foreach (var imgId in model.DeleteImageIds)
                    content.Add(new StringContent(imgId.ToString()), "DeleteImageIds");
            }

            if (model.MainImageId.HasValue)
            {
                content.Add(new StringContent(model.MainImageId.Value.ToString()), "MainImageId");
            }

            return await _httpClient.PutAsync($"{Urls.ProductUrl}Edit/{id}", content);
        }
        public async Task<HttpResponseMessage> DeleteImageAsync(int productId, int productImageId)
        {
            var content = new FormUrlEncodedContent(new[]
            {
              new KeyValuePair<string, string>("productId", productId.ToString()),
              new KeyValuePair<string, string>("productImageId", productImageId.ToString())
            });

            return await _httpClient.PostAsync($"{Urls.ProductUrl}DeleteImage", content);
        }
        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{Urls.ProductUrl}Delete/{id}");
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Urls.ProductUrl}GetById/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode(); 

            return await response.Content.ReadFromJsonAsync<Product>();
        }
        public async Task<Product> GetByIdWithIncludesAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"{Urls.ProductUrl}GetByIdWithIncludesAsync/{id}");
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{Urls.ProductUrl}GetAll");       
        }
        public async Task<IEnumerable<Product>> GetAllTakenAsync(int take, int? skip = null)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{Urls.ProductClientUrl}GetAllTaken/take?take={take}&skip={skip}");
        }
        public async Task<int> GetProductsCountAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>($"{Urls.ProductClientUrl}GetProductsCount");
        }
        public async Task<IEnumerable<Product>> FilterAsync(string categoryName, string colorName, string tagName, string brandName)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(categoryName))
                queryParams.Add($"categoryName={Uri.EscapeDataString(categoryName)}");

            if (!string.IsNullOrWhiteSpace(colorName))
                queryParams.Add($"colorName={Uri.EscapeDataString(colorName)}");

            if (!string.IsNullOrWhiteSpace(tagName))
                queryParams.Add($"tagName={Uri.EscapeDataString(tagName)}");

            if (!string.IsNullOrWhiteSpace(brandName))
                queryParams.Add($"brandName={Uri.EscapeDataString(brandName)}");

            var query = string.Join("&", queryParams);
            var url = $"{Urls.ProductClientUrl}Filter";

            if (!string.IsNullOrEmpty(query))
                url += "?" + query;

            var response = await _httpClient.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText);


            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(url);
        }
        public async Task<IEnumerable<Product>> GetSortedProductsAsync(string sortType)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{Urls.ProductClientUrl}GetSortedProducts?sortType={sortType}");
        }
        public async Task<IEnumerable<Product>> GetComparisonProductsAsync(int categoryId, int selectedProduct)
        {
            if (categoryId <= 0) throw new ArgumentException("Invalid category ID");

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{Urls.ProductClientUrl}GetComparisonProducts?categoryId={categoryId}&selectedProduct={selectedProduct}");

            if (response == null || !response.Any())
            {
                throw new KeyNotFoundException($"No comparison products found for category ID: {categoryId}");
            }
            return response;
        }
        public async Task<IEnumerable<Product>> SearchByNameAsync(string search)
        {
            var searchQuery = search?.Trim() ?? string.Empty;

            var response = await _httpClient.GetAsync($"{Urls.ProductClientUrl}SearchByName?name={searchQuery}");
            response.EnsureSuccessStatusCode(); 

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<List<Product>>(json, options);

            return result ?? new List<Product>();
        }
        public async Task<ProductWithImage> GetByIdWithImagesAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Urls.ProductUrl}GetByIdWithImages/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductWithImage>(content);
        }
        public async Task<IEnumerable<Product>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice)
        {
            var filterDto = new ProductFilter
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(filterDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{Urls.ProductClientUrl}FilterByPrice", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<Product>();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<IEnumerable<Product>>(responseContent, options) ?? Enumerable.Empty<Product>();
        }
        public async Task<IEnumerable<Product>> SearchByProductNameAsync(string search)
        {
            var searchQuery = search?.Trim() ?? string.Empty;
            var response = await _httpClient.GetAsync($"{Urls.ProductUrl}SearchByName?name={searchQuery}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<List<Product>>(json, options);
            return result ?? new List<Product>();
        }

        public async Task<decimal> GetMaxPriceAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.ProductClientUrl}GetMaxPrice");

            if (!response.IsSuccessStatusCode) return 0;

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<decimal>(jsonString);
        }

    }
}
