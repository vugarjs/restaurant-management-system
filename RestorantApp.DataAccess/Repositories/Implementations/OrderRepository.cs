using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Repositories.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(RestorantContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Order>> GetOrderByDateAsync(DateTime date)
    {
        return await FindAsync(o => o.Date == date.Date);
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
