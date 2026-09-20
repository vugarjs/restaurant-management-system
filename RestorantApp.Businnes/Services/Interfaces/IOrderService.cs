using RestorantApp.Businnes.DTOs.OrderDtos;
using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.Services.Interfaces;

public interface IOrderService
{
    Task? AddOrderAsync(OrderCreateDto createDto);
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetOrderByDateAsync(DateTime date);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    void UpdateOrder(Order order);
    Task<bool> DeleteOrderAsync(int orderId);

    Task<IEnumerable<Order>> GetOrdersByDatesInterval(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Order>> GetOrdersByPriceInterval(decimal minPrice, decimal maxPrice);

    public Task SaveChangesAsync();
}
