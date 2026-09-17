using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Repositories.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(RestorantContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Order>> GetOrderByDate(DateTime date)
    {
        return await FindAsync(o => o.OrderDate.Date == date.Date);
    }




    public async Task<Order?> GetOrderByNoAsync(int id)
    {
        return await FindAsync(o => o.Id == id).ContinueWith(t => t.Result.FirstOrDefault());
    }

    public async Task<IEnumerable<Order>> GetOrdersByDatesInterval(DateTime startDate, DateTime endDate)
    {
        return await FindAsync(o => o.OrderDate >= startDate && o.OrderDate <= endDate);
    }

    public async Task<IEnumerable<Order>> GetOrdersByPriceInterval(decimal minPrice, decimal maxPrice)
    {
        return await FindAsync(o => o.TotalAmount >= minPrice && o.TotalAmount < maxPrice);
    }
}
