using AutoMapper;
using RestorantApp.Businnes.DTOs.OrderDtos;
using RestorantApp.Businnes.DTOs.OrderItemDtos;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.Entity.Entities;

namespace RestorantApp.Presentation.HelperMethods;

internal class OrderHelpers
{
    private readonly IOrderService _orderService;
    private readonly IMenuItemService _menuItemService;
    private readonly IMapper _mapper;

    public OrderHelpers(IOrderService orderService, IMapper mapper, IMenuItemService menuItemService)
    {
        _orderService = orderService;
        _mapper = mapper;
        _menuItemService = menuItemService;
    }
    internal void DisplayOrder()
    {
        Console.WriteLine("-------------- SİFARİŞ -----------------");
        Console.WriteLine("- 1. Yeni sifariş əlavə et             -");
        Console.WriteLine("- 2. Sifarişi ləğv et                  -");
        Console.WriteLine("- 3. Bütün sifarişlərə bax             -");
        Console.WriteLine("- 4. Tarix aralığına görə göstər       -");
        Console.WriteLine("- 5. Məbləğ aralığına görə göstər      -");
        Console.WriteLine("- 6. Verilmiş tarixdə olan sifarişlər  -");
        Console.WriteLine("- 7. ID-yə görə sifariş detalı         -");
        Console.WriteLine("- 0. Ana menyuya qayıt                 -");
        Console.WriteLine("----------------------------------------");
    }
    internal async Task AddOrder()
    {
        var _orderCreateList = new OrderCreateDto();
        while (true)
        {

            Console.WriteLine("\n--- Mövcud Məhsullar ---");
            foreach (var item in await _menuItemService.GetAllMenuItemsAsync())
            {
                Console.WriteLine($"ID: {item.Id} | Ad: {item.Name} | Qiymət: {item.Price} AZN");
            }
            Console.WriteLine("------------------------");
            Console.Write("Sifariş veriləcək Məhsulun ID-si (Bitirmək üçün 0 yazın): ");

            if (!int.TryParse(Console.ReadLine(), out int menuItemId) || menuItemId == 0)
                break;

            Console.Write("Sayını daxil edin: ");

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Say düzgün daxil edilmədi.");
                continue;
            }

            var orderItemDto = new OrderItem
            {
                MenuItemId = menuItemId,
                Count = count
            };
            var orderItemDtoMapped = _mapper.Map<OrderItemCreateDto>(orderItemDto);

            _orderCreateList.OrderItems.Add(orderItemDtoMapped);

            Console.WriteLine("Məhsul səbətə əlavə edildi.");
        }

        if (_orderCreateList.OrderItems.Count > 0)
        {

            await _orderService.AddOrderAsync(_orderCreateList)!;
            await _orderService.SaveChangesAsync();
            Console.WriteLine("Sifariş uğurla tamamlandı!");
        }
        else
        {
            Console.WriteLine("Səbət boşdur, sifariş yaradılmadı.");
        }
    }
    internal async Task CancelOrder()
    {
        Console.WriteLine("\n--- Mövcud Sifarişlər ---");
        foreach (var item in await _orderService.GetAllOrdersAsync())
        {
            Console.WriteLine($"ID: {item.Id} | Tarix: {item.Date} | Məbləğ: {item.TotalAmount} AZN");
        }
        Console.Write("Ləğv ediləcək Sifarişin ID-si: ");
        if (int.TryParse(Console.ReadLine(), out int deleteId))
        {
            await _orderService.DeleteOrderAsync(deleteId);
            await _orderService.SaveChangesAsync();
            Console.WriteLine("Sifariş ləğv edildi!");
        }
        else Console.WriteLine("Düzgün rəqəm daxil edin.");
    }
    internal async Task ShowAllOrders()
    {
        Console.WriteLine("--- Bütün Sifarişlər ---");
        var orders = await _orderService.GetAllOrdersAsync();
        foreach (var o in orders)
        {
            Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
        }
    }
    internal async Task ShowOrdersByDateRange()
    {
        Console.Write("Başlanğıc tarixi (YYYY-MM-DD): ");
        DateTime.TryParse(Console.ReadLine(), out DateTime startDate);
        Console.Write("Son tarix (YYYY-MM-DD): ");
        DateTime.TryParse(Console.ReadLine(), out DateTime endDate);
        var dateOrders = await _orderService.GetOrdersByDatesInterval(startDate, endDate);

        if (!dateOrders.Any())
        {
            Console.WriteLine("Bu tarix aralığında sifariş tapılmadı.");
        }
        foreach (var o in dateOrders)
        {
            Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
        }

    }
    internal async Task ShowOrdersByAmountRange()
    {
        Console.Write("Min məbləğ: ");
        decimal.TryParse(Console.ReadLine(), out decimal minAmount);
        Console.Write("Max məbləğ: ");
        decimal.TryParse(Console.ReadLine(), out decimal maxAmount);
        var amountOrders = await _orderService.GetOrdersByPriceInterval(minAmount, maxAmount);
        if (!amountOrders.Any())
        {
            Console.WriteLine("Bu məbləğ aralığında sifariş tapılmadı.");
        }
        foreach (var o in amountOrders)
        {
            Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
        }
    }
    internal async Task ShowOrdersByDate()
    {
        Console.Write("Tarix daxil edin (YYYY-MM-DD): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime targetDate))
        {
            var exactDateOrders = await _orderService.GetOrderByDateAsync(targetDate);

            if (!exactDateOrders.Any())
            {
                Console.WriteLine("Bu tarixə uyğun sifariş tapılmadı.");
            }
            else
            {
                foreach (var o in exactDateOrders)
                {
                    Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
                }
            }
        }
        else
        {
            Console.WriteLine("Düzgün tarix formatı daxil edilmədi! (Məsələn: 2026-09-18)");
        }
    }
    internal async Task ShowById()
    {
        Console.WriteLine("\n--- Mövcud Sifarişlər ---");

        foreach (var item in await _orderService.GetAllOrdersAsync())
        {
            Console.WriteLine($"ID: {item.Id} | Tarix: {item.Date} | Məbləğ: {item.TotalAmount} AZN");
        }
        Console.WriteLine("ID Daxil edin: ");
        if (int.TryParse(Console.ReadLine(), out int orderId))
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                Console.WriteLine($"ID: {order.Id} | Tarix: {order.Date} | Məbləğ: {order.TotalAmount} AZN");
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"Ad: {item.MenuItem.Name} | Miqdar: {item.Count} | Qiymət: {item.MenuItem.Price} AZN");
                }
            }

            else
            {
                Console.WriteLine("Sifariş tapılmadı.");
            }
        }
        else
        {
            Console.WriteLine("Düzgün ID daxil edilmədi!");
        }
    }
}
