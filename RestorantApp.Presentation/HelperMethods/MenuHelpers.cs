using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.Businnes.Services.Implementations;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Presentation.HelperMethods;

internal class MenuHelpers
{
    private readonly IMenuItemService _menuItemService;

    public MenuHelpers(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }
    internal  async Task AddMenuItem(string name, decimal price, string categoryInput)
    {
        if (Enum.TryParse(typeof(Category), categoryInput, true, out var parsedCategory))
        {
            var newDto = new MenuItemCreateDto
            {
                Name = name,
                Price = price,
                Category = (Category)parsedCategory
            };

            if (string.IsNullOrWhiteSpace(newDto.Name) || newDto.Price <= 0)
            {
                throw new Exception("Məhsul adı boş ola bilməz və qiymət sıfırdan böyük olmalıdır!");
            }

            //if (newDto.Name == _menuItemService.GetMenuItemsByNameAsync(newDto.Name).Result.FirstOrDefault()?.Name)
            //{
            //    throw new Exception("Bu adda məhsul artıq mövcuddur!");
            //}

            await _menuItemService.AddMenuItemAsync(newDto);
            await _menuItemService.SaveChangesAsync();

            Console.WriteLine("Menyu elementi uğurla əlavə edildi!");
        }
    }

}
