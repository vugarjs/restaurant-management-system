using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Businnes.Services.Interfaces;

internal interface IMenuItemService
{
    public Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
    public Task<MenuItem> GetMenuItemByIdAsync(int menuItemId);
    public void CreateMenuItemAsync(MenuItem menuItem);
    public void UpdateMenuItemAsync(MenuItem menuItem);
    public void DeleteMenuItemAsync(int menuItemId);

    public Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(Category category);

    public Task<IEnumerable<MenuItem>> GetMenuItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

    public Task<IEnumerable<MenuItem>> GetMenuItemsByNameAsync(string name);


}
