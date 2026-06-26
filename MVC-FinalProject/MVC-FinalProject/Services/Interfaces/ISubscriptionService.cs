using MVC_FinalProject.Models.Subscription;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<HttpResponseMessage> SubscribeAsync(SubscriptionCreate model);
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task<HttpResponseMessage> UnsubscribeAsync(string email);
    }
}