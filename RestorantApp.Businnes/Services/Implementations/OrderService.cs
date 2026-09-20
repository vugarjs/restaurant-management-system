using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    public OrderService(IOrderRepository orderRepository, IMapper mapper, RestorantContext restorantContext, IMemoryCache memoryCache)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _context = restorantContext;
        _memoryCache = memoryCache;
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

        _memoryCache.Remove("AllOrders");
        await _orderRepository.AddAsync(order);

        return;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        if (await _orderRepository.FindSingleAsync(x => x.Id == orderId) == null)
        {
            return false;
        }

        _memoryCache.Remove($"Order_{orderId}");
        _memoryCache.Remove("AllOrders");
        _orderRepository.Remove(orderId);
        return true;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        var cacheKey = "AllOrders";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
          {
              return await _orderRepository.GetAllAsync(
         include: query => query.Include(o => o.OrderItems)
             .ThenInclude(oi => oi.MenuItem)
         );
          });
    }

    public async Task<IEnumerable<Order>> GetOrderByDateAsync(DateTime date)
    {
        var cacheKey = $"Orders_{date.ToShortDateString()}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            return await _orderRepository.GetOrderByDateAsync(date);
        });
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        string cacheKey = $"Order_{orderId}";

        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var order = await _orderRepository.FindSingleAsync(
              predicate: x => x.Id == orderId,
              include: query => query.Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.MenuItem)
              );
            return order;

        });
    }

    public async Task<IEnumerable<Order>> GetOrdersByDatesInterval(DateTime startDate, DateTime endDate)
    {
        var cacheKey = $"Orders_{startDate.ToShortDateString()}_{endDate.ToShortDateString()}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            return await _orderRepository.GetOrdersByDatesInterval(startDate, endDate);
        });
    }

    public async Task<IEnumerable<Order>> GetOrdersByPriceInterval(decimal minPrice, decimal maxPrice)
    {
        var cacheKey = $"Orders_{minPrice}_{maxPrice}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            return await _orderRepository.GetOrdersByPriceInterval(minPrice, maxPrice);
        });
    }

    public async Task SaveChangesAsync()
    {
        await _orderRepository.SaveChangesAsync();
    }

    public void UpdateOrder(Order order)
    {
        _memoryCache.Remove($"Order_{order.Id}");
        _memoryCache.Remove("AllOrders");
        _orderRepository.Update(order);

    }
}
