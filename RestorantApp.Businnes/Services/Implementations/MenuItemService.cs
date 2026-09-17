using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Businnes.Services.Implementations;

internal class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    public MenuItemService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }
    public void CreateMenuItemAsync(MenuItem menuItem)
    {
        _menuItemRepository.AddAsync(menuItem);
    }

    public void DeleteMenuItemAsync(int menuItemId)
    {
        _menuItemRepository.Remove(menuItemId);
    }

    public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
    {
        return await _menuItemRepository.GetAllAsync();
    }

    public async Task<MenuItem> GetMenuItemByIdAsync(int menuItemId)
    {
        return await _menuItemRepository.Get(x => x.Id == menuItemId);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(Category category)
    {
        return await _menuItemRepository.GetMenuItemsByCategoryAsync(category);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByNameAsync(string name)
    {
        return await _menuItemRepository.SearchByNameAsync(name);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _menuItemRepository.PriceBetweenAsync(minPrice, maxPrice);
    }

    public void UpdateMenuItemAsync(MenuItem menuItem)
    {
        _menuItemRepository.Update(menuItem);
    }
}
