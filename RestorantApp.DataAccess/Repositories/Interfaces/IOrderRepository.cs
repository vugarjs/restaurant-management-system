using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{

    public Task<IEnumerable<Order>> GetOrdersByDatesInterval(DateTime startDate, DateTime endDate);
    public Task<IEnumerable<Order>> GetOrderByDateAsync(DateTime date);

    public Task<IEnumerable<Order>> GetOrdersByPriceInterval(decimal minPrice, decimal maxPrice);

    public Task<Order?> GetOrdersByNoAsync(int id);


}
