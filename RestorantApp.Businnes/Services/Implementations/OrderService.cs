using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async void CreateOrderAsync(Order order)
    {
        await _orderRepository.AddAsync(order);
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        _orderRepository.Remove(orderId);
        return true;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _orderRepository.GetOrderByNoAsync(orderId);
    }

    public void UpdateOrderAsync(Order order)
    {
        _orderRepository.Update(order);

    }
}
