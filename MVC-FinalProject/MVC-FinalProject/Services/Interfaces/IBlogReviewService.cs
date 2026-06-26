using MVC_FinalProject.Models.BlogReview;
using MVC_FinalProject.Models.Review;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IBlogReviewService
    {
        Task<IEnumerable<BlogReview>> GetAllByPostIdAsync(int postId);

        Task<IEnumerable<BlogReview>> GetAllAsync();
        Task<HttpResponseMessage> DeleteReviewAsync(int reviewid);

        Task<IEnumerable<BlogReview>> GetByPostIdAsync(int postId);
        Task<BlogReview> GetByIdAsync(int id);
        Task<HttpResponseMessage> CreateAsync(BlogReviewCreateApi model, string token);
        Task<HttpResponseMessage> EditAsync(BlogReviewEditApi model, int id);
        Task<HttpResponseMessage> DeleteAsync(int id, string token);
    }
}
