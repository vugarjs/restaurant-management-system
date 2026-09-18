using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Businnes.Services.Interfaces;

public interface IMenuItemService
{
    public Task<IEnumerable<MenuItemReturnDto>> GetAllMenuItemsAsync();
    public Task<MenuItemReturnDto> GetMenuItemByIdAsync(int menuItemId);
    public Task AddMenuItemAsync(MenuItemCreateDto menuItemdto);
    public Task EditMenuItem(int id, MenuItemUpdateDto updateDto);
    public void RemoveMenuItem(int menuItemId);

    public Task<IEnumerable<MenuItemReturnDto>> GetMenuItemsByCategoryAsync(Category category);

    public Task<IEnumerable<MenuItemReturnDto>> GetMenuItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

    public Task<IEnumerable<MenuItemReturnDto>> GetMenuItemsByNameAsync(string name);

    public Task SaveChangesAsync();


}
