using AutoMapper;
using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Presentation.HelperMethods;

internal class MenuHelpers
{
    private readonly IMenuItemService _menuItemService;
    private readonly IMapper _mapper;

    public MenuHelpers(IMenuItemService menuItemService, IMapper mapper)
    {
        _menuItemService = menuItemService;
        _mapper = mapper;
    }
    internal async Task AddMenuItem()
    {
        Console.Write("Məhsulun adı: ");
        string name = Console.ReadLine()!;
        Console.Write("Qiyməti: ");
        decimal.TryParse(Console.ReadLine(), out decimal price);
        Console.WriteLine("Kateqoriyanı seçin:");

        foreach (var cat in Enum.GetValues(typeof(Category)))
            Console.WriteLine($"{(int)cat} - {cat}");

        Console.Write("Seçim (rəqəm və ya ad): ");
        string categoryInput = Console.ReadLine()!;
        if (Enum.TryParse(typeof(Category), categoryInput, true, out var parsedCategory))
        {
            var newDto = new MenuItem
            {
                Name = name,
                Price = price,
                Category = (Category)parsedCategory
            };
            var cevrilmis = _mapper.Map<MenuItemCreateDto>(newDto);

            if (string.IsNullOrWhiteSpace(name) || price <= 0)
            {
                throw new Exception("Məhsul adı boş ola bilməz və qiymət sıfırdan böyük olmalıdır!");
            }
            if (name == _menuItemService.GetMenuItemsByNameAsync(name).Result.FirstOrDefault()?.Name)
            {
                throw new Exception("Bu adda məhsul artıq mövcuddur!");
            }
            await _menuItemService.AddMenuItemAsync(cevrilmis);
            await _menuItemService.SaveChangesAsync();

            Console.WriteLine("Menyu elementi uğurla əlavə edildi!");
        }

    }
    internal async Task UpdateMenuItem()
    {
        Console.WriteLine("\n--- Mövcud Məhsullar ---");
        foreach (var iteqm in await _menuItemService.GetAllMenuItemsAsync())
        {
            Console.WriteLine($"ID: {iteqm.Id} | Ad: {iteqm.Name} | Qiymət: {iteqm.Price} AZN");
        }
        Console.Write("Düzəliş ediləcək Məhsulun ID-si: ");
        var upid = int.Parse(Console.ReadLine()!);

        var item = await _menuItemService.GetMenuItemByIdAsync(upid);

        if (item != null)
        {
            Console.Write("Yeni ad: ");
            var newName = Console.ReadLine()!;

            Console.Write("Yeni qiymət: ");
            var newPrice = decimal.Parse(Console.ReadLine()!);

            item.Name = newName;
            item.Price = newPrice;

            var updateDto = _mapper.Map<MenuItemUpdateDto>(item);

            if (string.IsNullOrWhiteSpace(updateDto.Name) || updateDto.Price <= 0)
            {
                Console.WriteLine("Məhsul adı boş ola bilməz və qiymət sıfırdan böyük olmalıdır!");
            }
            if (newName == _menuItemService.GetMenuItemsByNameAsync(item.Name).Result.FirstOrDefault()?.Name)
            {
                throw new Exception("Bu adda məhsul artıq mövcuddur!");
            }
            await _menuItemService.EditMenuItem(upid, updateDto);
            await _menuItemService.SaveChangesAsync();

            Console.WriteLine("Məhsul bazada uğurla yeniləndi!");
        }
        else
        {
            Console.WriteLine("Bu ID-də məhsul tapılmadı.");
        }
    }
    internal async Task DeleteMenuItem()
    {
        Console.WriteLine("\n--- Mövcud Məhsullar ---");
        foreach (var iteqm in await _menuItemService.GetAllMenuItemsAsync())
        {
            Console.WriteLine($"ID: {iteqm.Id} | Ad: {iteqm.Name} | Qiymət: {iteqm.Price} AZN");
        }
        Console.Write("Silinəcək Məhsulun ID-si: ");
        var deleteId = int.Parse(Console.ReadLine()!);
        var itemss = await _menuItemService.GetAllMenuItemsAsync();

        if (await _menuItemService.GetMenuItemByIdAsync(deleteId) == null)
        {
            throw new Exception("Bu ID-də məhsul tapılmadı!");
        }

        _menuItemService.RemoveMenuItem(deleteId);
        await _menuItemService.SaveChangesAsync();
        Console.WriteLine("Məhsul uğurla silindi!");
    }
    internal async Task ListMenuItems()
    {
        var items = await _menuItemService.GetAllMenuItemsAsync();
        Console.WriteLine("--- Bütün Menyu Elementləri ---");
        foreach (var i in items)
        {
            Console.WriteLine($"ID {i.Id} | Ad: {i.Name} | Kateqoriya: {i.Category} | Qiymət: {i.Price} AZN");
        }
    }
    internal async Task ListMenuItemsByCategory()
    {
        Console.WriteLine("Kateqoriyanı seçin:");
        foreach (var cat in Enum.GetValues(typeof(Category)))
            Console.WriteLine($"{cat}");

        Console.Write("Seçim ad : ");
        string catInput = Console.ReadLine()!;

        if (Enum.TryParse(typeof(Category), catInput, true, out var parsedCat))
        {
            var catItems = await _menuItemService.GetMenuItemsByCategoryAsync((Category)parsedCat);

            if (!catItems.Any())
            {
                throw new Exception("Bu kateqoriyada məhsul tapılmadı!");
            }
            else
            {
                foreach (var i in catItems)
                {
                    Console.WriteLine($"Ad: {i.Name} | Qiymət: {i.Price} AZN");
                }
            }
        }
        else
        {
            throw new Exception("Yanlış kateqoriya daxil edildi!");
        }
    }
    internal async Task PriceBetweenRange()
    {
        Console.Write("Minimum qiymət: ");
        decimal.TryParse(Console.ReadLine(), out decimal minPrice);
        Console.Write("Maksimum qiymət: ");
        decimal.TryParse(Console.ReadLine(), out decimal maxPrice);
        var rangeItems = await _menuItemService.GetMenuItemsByPriceRangeAsync(minPrice, maxPrice);
        if (!rangeItems.Any())
        {
            Console.WriteLine("Bu qiymət aralığında məhsul tapılmadı.");
        }
        foreach (var i in rangeItems)
        {
            Console.WriteLine($" Ad: {i.Name} | Qiymət: {i.Price} AZN");
        }
    }
    internal async Task ListMenuItemsByName()
    {
        Console.Write("Axtarış sözü: ");
        string searchText = Console.ReadLine()!;
        var searchItems = await _menuItemService.GetMenuItemsByNameAsync(searchText);
        if (!searchItems.Any())
        {
            Console.WriteLine("Bu axtarış sözünə uyğun məhsul tapılmadı.");
        }
        if (searchText.Length < 3)
        {
            throw new Exception("Axtarış sözü ən azı 3 simvol olmalıdır!");
        }
        foreach (var i in searchItems)
        {
            Console.WriteLine($"Ad: {i.Name} | Qiymət: {i.Price} AZN");
        }
    }
    internal async Task ListMenuItemsById()
    {
        Console.Write("Axtarış üçün ID daxil edin: ");
        int.TryParse(Console.ReadLine(), out int id);
        var idItem = await _menuItemService.GetMenuItemByIdAsync(id);
        if (idItem != null)
        {
            Console.WriteLine($"Ad: {idItem.Name} | Qiymət: {idItem.Price} AZN");
        }
        else
        {
            throw new Exception("İstənilən ID ilə item tapılmadı.");
        }
    }
    internal void DisplayMenu()
    {
        Console.WriteLine("--------- MENYU ƏMƏLİYYATLARI ----------");
        Console.WriteLine("- 1. Yeni item əlavə et                -");
        Console.WriteLine("- 2. İtem üzərində düzəliş et          -");
        Console.WriteLine("- 3. İtem sil                          -");
        Console.WriteLine("- 4. Bütün item-ları göstər            -");
        Console.WriteLine("- 5. Kateqoriyasına görə item göstər   -");
        Console.WriteLine("- 6. Qiymət aralığına görə item göstər -");
        Console.WriteLine("-------------- AXTARIŞ -----------------");
        Console.WriteLine("- 7. Ada görə axtarış et (Search)      -");
        Console.WriteLine("- 8. ID görə axtarış et (Search)       -");
        Console.WriteLine("- 0. Ana menyuya qayıt                 -");
        Console.WriteLine("----------------------------------------");
    }
}
