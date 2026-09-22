using Microsoft.EntityFrameworkCore;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Repositories.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly IRepository<Order> _orderRepository;
    public OrderRepository(RestorantContext context, IRepository<Order> orderRepository) : base(context)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> GetOrderByDateAsync(DateTime date)
    {
        var startDate = date.Date;
        var endDate = startDate.AddDays(1);

        return await _orderRepository.GetAllAsync(
            predicate: o => o.Date >= startDate && o.Date < endDate,
            include: query => query.Include(o => o.OrderItems)
        );
    }

    public async Task<Order?> GetOrdersByNoAsync(int id)
    {
        return await Get(x => x.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByDatesInterval(DateTime startDate, DateTime endDate)
    {
        return await FindAsync(o => o.Date >= startDate && o.Date <= endDate);
    }

    public async Task<IEnumerable<Order>> GetOrdersByPriceInterval(decimal minPrice, decimal maxPrice)
    {
        return await FindAsync(o => o.TotalAmount >= minPrice && o.TotalAmount <= maxPrice);
    }
}
