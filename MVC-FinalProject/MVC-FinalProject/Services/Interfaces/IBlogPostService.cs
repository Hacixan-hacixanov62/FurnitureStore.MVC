using MVC_FinalProject.Helpers;
using MVC_FinalProject.Models.BlogCategory;
using MVC_FinalProject.Models.BlogPost;
using MVC_FinalProject.Models.Brand;
using MVC_FinalProject.Models.Product;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<HttpResponseMessage> CreateAsync(BlogPostCreate model);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> GetByIdAsync(int id);
        Task<HttpResponseMessage> DeleteAsync(int id);
        Task<HttpResponseMessage> EditAsync(int id, BlogPostEdit model, int? mainImageId);  
        Task<HttpResponseMessage> DeleteImageAsync(int blogPostId, int blogPostImageId);
        Task<PaginationResponse<BlogPost>> GetPaginatedAsync(int page, int pageSize);
        Task<IEnumerable<BlogPost>> SearchByCategoryAndName(string name);
        Task<IEnumerable<BlogPost>> FilterAsync(string categoryName);
        Task<BlogPost> GetPreviousAsync(int id);
        Task<BlogPost> GetNextAsync(int id);
    }
}
