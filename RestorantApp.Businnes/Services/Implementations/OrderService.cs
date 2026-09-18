using AutoMapper;
using RestorantApp.Businnes.DTOs.OrderDtos;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly RestorantContext _context;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository orderRepository, IMapper mapper, RestorantContext restorantContext)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _context = restorantContext;
    }
    public async Task? AddOrderAsync(OrderCreateDto createDto)
    {
        var order = _mapper.Map<Order>(createDto);

        decimal totalAmount = 0;

        foreach (var itemDto in createDto.OrderItems)
        {
            // Həmin menyu elementinin bazadan qiymətini tapırıq
            var menuItem = await _context.MenuItems.FindAsync(itemDto.MenuItemId);
            if (menuItem != null)
            {
                totalAmount += menuItem.Price * itemDto.Count;
            }
        }

        // Hesablanmış ümumi məbləği sifarişə təyin edirik
        order.TotalAmount = totalAmount;

        await _orderRepository.AddAsync(order);
        await _orderRepository.AddAsync(order);

        return;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        if (await _orderRepository.FindSingleAsync(x => x.Id == orderId) == null)
        {
            return false;
        }
        _orderRepository.Remove(orderId);
        return true;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Order>> GetOrderByDateAsync(DateTime date)
    {
        return await _orderRepository.GetOrderByDateAsync(date);
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository.FindSingleAsync(x => x.Id == orderId);
        return order;
    }

    public async Task<IEnumerable<Order>> GetOrdersByDatesInterval(DateTime startDate, DateTime endDate)
    {
        return await _orderRepository.GetOrdersByDatesInterval(startDate, endDate);
    }

    public async Task<IEnumerable<Order>> GetOrdersByPriceInterval(decimal minPrice, decimal maxPrice)
    {
        return await _orderRepository.GetOrdersByPriceInterval(minPrice, maxPrice);
    }

    public async Task SaveChangesAsync()
    {
        await _orderRepository.SaveChangesAsync();
    }

    public void UpdateOrder(Order order)
    {
        _orderRepository.Update(order);

    }
}
