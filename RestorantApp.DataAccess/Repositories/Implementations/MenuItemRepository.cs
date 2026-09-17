using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;

namespace RestorantApp.DataAccess.Repositories.Implementations;

public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(RestorantContext context) : base(context)
    {

    }

    public async Task<List<MenuItem>> PriceBetweenAsync(decimal minPrice, decimal maxPrice)
    {
        return await FindAsync(mi => mi.Price >= minPrice && mi.Price <= maxPrice);
    }
    public async Task<List<MenuItem>> SearchByNameAsync(string name)
    {
        return await FindAsync(mi => mi.Name.Contains(name));
    }
    public async Task<List<MenuItem>> GetMenuItemsByCategoryAsync(Category category)
    {
        return await FindAsync(mi => mi.Category == category);
    }

    public async Task<MenuItem> GetMenuItemByIdAsync(int menuItemId)
    {
        return await Get(x => x.Id == menuItemId);
    }
}
