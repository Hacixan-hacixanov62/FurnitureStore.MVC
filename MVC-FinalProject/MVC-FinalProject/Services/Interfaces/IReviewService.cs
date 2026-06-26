using MVC_FinalProject.Models.Review;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IReviewService
    {
            Task<IEnumerable<Review>> GetAllByProductIdAsync(int productId);

            Task<IEnumerable<Review>> GetAllAsync();
            Task<HttpResponseMessage> DeleteReviewAsync(int reviewid);

            Task<IEnumerable<Review>> GetByProductIdAsync(int productId); 
            Task<Review> GetByIdAsync(int id);  
            Task<HttpResponseMessage> CreateAsync(ReviewCreateApi model, string token); 
            Task<HttpResponseMessage> EditAsync(ReviewEditApi model, int id);
            Task<HttpResponseMessage> DeleteAsync(int id, string token);

    }
}
