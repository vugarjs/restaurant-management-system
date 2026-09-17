using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.Services.Interfaces;

public interface IOrderService
{
    void CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    void UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int orderId);
}
