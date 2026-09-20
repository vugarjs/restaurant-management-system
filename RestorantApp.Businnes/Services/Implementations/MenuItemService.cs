using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Businnes.Services.Implementations;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly RestorantContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    public MenuItemService(IMenuItemRepository menuItemRepository, RestorantContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _menuItemRepository = menuItemRepository;
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }
    public async Task AddMenuItemAsync(MenuItemCreateDto menuItemdto)
    {

        var menuItem = _mapper.Map<MenuItem>(menuItemdto);
        await _menuItemRepository.AddAsync(menuItem);
        return;
    }

    public void RemoveMenuItem(int menuItemId)
    {
        _menuItemRepository.Remove(menuItemId);
    }

    public async Task<IEnumerable<MenuItemReturnDto>> GetAllMenuItemsAsync()
    {
        var menuItems = await _menuItemRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MenuItemReturnDto>>(menuItems);
    }

    public async Task<MenuItemReturnDto> GetMenuItemByIdAsync(int menuItemId)
    {
        string cacheKey = $"MenuItem_{menuItemId}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var menu = await _menuItemRepository.FindSingleAsync(x => x.Id == menuItemId);

            return _mapper.Map<MenuItemReturnDto>(menu);
        });
    }

    public async Task<IEnumerable<MenuItemReturnDto>> GetMenuItemsByCategoryAsync(Category category)
    {
        var menuItems = await _menuItemRepository.GetMenuItemsByCategoryAsync(category);
        return _mapper.Map<IEnumerable<MenuItemReturnDto>>(menuItems);
    }

    public async Task<IEnumerable<MenuItemReturnDto>> GetMenuItemsByNameAsync(string name)
    {
        var menuItems = await _menuItemRepository.SearchByNameAsync(name);
        return _mapper.Map<IEnumerable<MenuItemReturnDto>>(menuItems);
    }

    public async Task<IEnumerable<MenuItemReturnDto>> GetMenuItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var menuItems = await _menuItemRepository.PriceBetweenAsync(minPrice, maxPrice);


        return _mapper.Map<IEnumerable<MenuItemReturnDto>>(menuItems);
    }

    public async Task EditMenuItem(int id, MenuItemUpdateDto updateDto)
    {
        if (id == updateDto.Id)
        {
            var menuItem = await _menuItemRepository.FindSingleAsync(x => x.Id == id);
            if (menuItem == null)
                throw new Exception("Menu item not found.");
            _mapper.Map(updateDto, menuItem);
            _menuItemRepository.Update(menuItem);
        }
        else
        {
            throw new Exception("Menu item ID mismatch.");
        }
    }

    public async Task SaveChangesAsync()
    {
        await _menuItemRepository.SaveChangesAsync();
    }
}
