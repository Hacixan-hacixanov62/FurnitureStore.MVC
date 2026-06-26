using MVC_FinalProject.Models.Order;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderCreate dto);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task ChangeStatusAsync(ChangeOrderStatus model); 

    }
}
