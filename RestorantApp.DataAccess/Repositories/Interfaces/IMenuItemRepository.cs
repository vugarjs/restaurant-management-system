using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;

namespace RestorantApp.DataAccess.Repositories.Interfaces;

public interface IMenuItemRepository : IRepository<MenuItem>
{
    public Task<List<MenuItem>> PriceBetweenAsync(decimal minPrice, decimal maxPrice);
    public Task<List<MenuItem>> SearchByNameAsync(string name);
    public Task<List<MenuItem>> GetMenuItemsByCategoryAsync(Category category);
    public Task<MenuItem> GetMenuItemByIdAsync(int menuItemId);
}
