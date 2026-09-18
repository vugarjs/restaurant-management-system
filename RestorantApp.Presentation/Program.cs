using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.Businnes.DTOs.OrderDtos;
using RestorantApp.Businnes.DTOs.OrderItemDtos;
using RestorantApp.Businnes.Mappers;
using RestorantApp.Businnes.Services.Implementations;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Implementations;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Enums;
using System.Text;

namespace RestorantApp.Presentation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var services = new ServiceCollection();
            services.AddDbContext<RestorantContext>();

            // Menu Item qeydiyyatları
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IMenuItemService, MenuItemService>();

            // Order qeydiyyatları
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddAutoMapper(cfg => { }, typeof(MapperProfile).Assembly);
            services.AddLogging();

            var provider = services.BuildServiceProvider();
            var mapper = provider.GetRequiredService<IMapper>();


            var menuItemService = provider.GetRequiredService<IMenuItemService>();
            var orderService = provider.GetRequiredService<IOrderService>();

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== RESTORAN İDARƏETMƏ SİSTEMİ ===");
                Console.WriteLine("1. Menyu üzərində əməliyyat aparmaq");
                Console.WriteLine("2. Sifarişlər üzərində əməliyyat aparmaq");
                Console.WriteLine("0. Sistemdən çıxmaq");
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;

                switch (choice)
                {
                    case "1":
                        await MenuItemMenu(menuItemService, mapper);
                        break;
                    case "2":
                        await OrderMenu(orderService);
                        break;
                    case "0":
                        Console.WriteLine("Proqramdan çıxılır...");
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim! Davam etmək üçün hər hansı bir düyməyə basın.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static async Task MenuItemMenu(IMenuItemService menuItemService, IMapper mapper)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- MENYU ƏMƏLİYYATLARI ---");
                Console.WriteLine("1. Yeni item əlavə et");
                Console.WriteLine("2. İtem üzərində düzəliş et");
                Console.WriteLine("3. İtem sil");
                Console.WriteLine("4. Bütün item-ları göstər");
                Console.WriteLine("5. Kateqoriyasına görə menu item-ları göstər");
                Console.WriteLine("6. Qiymət aralığına görə menu item-ları göstər");
                Console.WriteLine("7. Ada görə axtarış et (Search)");
                Console.WriteLine("0. Ana menyuya qayıt");
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
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
                                var newDto = new MenuItemCreateDto
                                {
                                    Name = name,
                                    Price = price,
                                    Category = (Category)parsedCategory
                                };


                                await menuItemService.AddMenuItemAsync(newDto);
                                await menuItemService.SaveChangesAsync();

                                Console.WriteLine("Menyu elementi uğurla əlavə edildi!");
                            }
                            else
                            {
                                Console.WriteLine("Yanlış kateqoriya daxil edildi!");

                            }
                            break;

                        case "2":
                            Console.Write("Düzəliş ediləcək Məhsulun ID-si: ");
                            var upid = int.Parse(Console.ReadLine()!);

                            var item = await menuItemService.GetMenuItemByIdAsync(upid);

                            if (item != null)
                            {
                                Console.Write("Yeni ad: ");
                                var newName = Console.ReadLine()!;

                                Console.Write("Yeni qiymət: ");
                                var newPrice = decimal.Parse(Console.ReadLine()!);

                                item.Name = newName;
                                item.Price = newPrice;

                                var updateDto = mapper.Map<MenuItemUpdateDto>(item);

                                await menuItemService.EditMenuItem(upid, updateDto);

                                Console.WriteLine("Məhsul bazada uğurla yeniləndi!");
                            }
                            else
                            {
                                Console.WriteLine("Bu ID-də məhsul tapılmadı.");
                            }



                            break;

                        case "4":
                            Console.WriteLine("--- Bütün Menyu Elementləri ---");
                            var items = await menuItemService.GetAllMenuItemsAsync();
                            foreach (var i in items)
                            {
                                Console.WriteLine($"Ad: {i.Name} | Kateqoriya: {i.Category} | Qiymət: {i.Price} AZN");
                            }
                            break;

                        case "5":
                            Console.WriteLine("Kateqoriyanı seçin:");
                            foreach (var cat in Enum.GetValues(typeof(Category)))
                                Console.WriteLine($"{(int)cat} - {cat}");

                            Console.Write("Seçim (rəqəm və ya ad): ");
                            string catInput = Console.ReadLine()!;

                            if (Enum.TryParse(typeof(Category), catInput, true, out var parsedCat))
                            {
                                // Enum tipini birbaşa servisə ötürürük
                                var catItems = await menuItemService.GetMenuItemsByCategoryAsync((Category)parsedCat);

                                foreach (var i in catItems)
                                {
                                    Console.WriteLine($"Ad: {i.Name} | Qiymət: {i.Price} AZN");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Yanlış kateqoriya daxil edildi!");
                            }
                            break;

                        case "6":
                            Console.Write("Minimum qiymət: ");
                            decimal.TryParse(Console.ReadLine(), out decimal minPrice);
                            Console.Write("Maksimum qiymət: ");
                            decimal.TryParse(Console.ReadLine(), out decimal maxPrice);
                            var rangeItems = await menuItemService.GetMenuItemsByPriceRangeAsync(minPrice, maxPrice);
                            foreach (var i in rangeItems)
                            {
                                Console.WriteLine($" Ad: {i.Name} | Qiymət: {i.Price} AZN");
                            }
                            break;

                        case "7":
                            Console.Write("Axtarış sözü: ");
                            string searchText = Console.ReadLine()!;
                            var searchItems = await menuItemService.GetMenuItemsByNameAsync(searchText);
                            foreach (var i in searchItems)
                            {
                                Console.WriteLine($"Ad: {i.Name} | Qiymət: {i.Price} AZN");
                            }
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Yanlış seçim!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"XƏTA: {ex.Message}");
                }

                Console.WriteLine("\nDavam etmək üçün hər hansı bir düyməyə basın...");
                Console.ReadKey();
            }
        }

        // 2. SİFARİŞ ƏMƏLİYYATLARI
        static async Task OrderMenu(IOrderService orderService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- SİFARİŞ MENYUSU ---");
                Console.WriteLine("1. Yeni sifariş əlavə et");
                Console.WriteLine("2. Sifarişi ləğv et");
                Console.WriteLine("3. Bütün sifarişlərə bax");
                Console.WriteLine("4. Tarix aralığına görə göstər");
                Console.WriteLine("5. Məbləğ aralığına görə göstər");
                Console.WriteLine("6. Verilmiş tarixdə olan sifarişlər");
                Console.WriteLine("7. ID-yə görə sifariş detalı");
                Console.WriteLine("0. Ana menyuya qayıt");
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var orderCreateDto = new OrderCreateDto();
                            bool addingItems = true;

                            while (addingItems)
                            {
                                Console.Write("Sifariş veriləcək Məhsulun ID-si (Bitirmək üçün 0 yazın): ");
                                int.TryParse(Console.ReadLine(), out int menuItemId);
                                if (menuItemId == 0) break;

                                Console.Write("Sayını daxil edin: ");
                                int.TryParse(Console.ReadLine(), out int count);

                                orderCreateDto.OrderItems.Add(new OrderItemCreateDto
                                {
                                    MenuItemId = menuItemId,
                                    Count = count
                                });



                                Console.WriteLine("Məhsul səbətə əlavə edildi.");
                                await orderService.SaveChangesAsync();

                            }

                            if (orderCreateDto.OrderItems.Count > 0)
                            {
                                await orderService.AddOrderAsync(orderCreateDto)!;
                                await orderService.SaveChangesAsync();
                                Console.WriteLine("Sifariş uğurla yaradıldı!");
                            }
                            else
                            {
                                Console.WriteLine("Heç bir məhsul seçilmədi, sifariş ləğv edildi.");
                            }
                            break;

                        case "2":
                            Console.Write("Ləğv ediləcək Sifarişin ID-si: ");
                            if (int.TryParse(Console.ReadLine(), out int deleteId))
                            {
                                await orderService.DeleteOrderAsync(deleteId);
                                await orderService.SaveChangesAsync();
                                Console.WriteLine("Sifariş ləğv edildi!");
                            }
                            else Console.WriteLine("Düzgün rəqəm daxil edin.");
                            break;

                        case "3":
                            Console.WriteLine("--- Bütün Sifarişlər ---");
                            var orders = await orderService.GetAllOrdersAsync();
                            foreach (var o in orders)
                            {
                                Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
                            }
                            break;

                        case "4":
                            Console.Write("Başlanğıc tarixi (YYYY-MM-DD): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime startDate);
                            Console.Write("Son tarix (YYYY-MM-DD): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime endDate);
                            var dateOrders = await orderService.GetOrdersByDatesInterval(startDate, endDate);
                            foreach (var o in dateOrders)
                            {
                                Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
                            }
                            break;

                        case "5":
                            Console.Write("Min məbləğ: ");
                            decimal.TryParse(Console.ReadLine(), out decimal minAmount);
                            Console.Write("Max məbləğ: ");
                            decimal.TryParse(Console.ReadLine(), out decimal maxAmount);
                            var amountOrders = await orderService.GetOrdersByPriceInterval(minAmount, maxAmount);
                            foreach (var o in amountOrders)
                            {
                                Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
                            }
                            break;

                        case "6":
                            Console.Write("Tarix daxil edin (YYYY-MM-DD): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime targetDate);
                            var exactDateOrders = await orderService.GetOrderByDateAsync(targetDate);
                            foreach (var o in exactDateOrders)
                            {
                                Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
                            }
                            break;

                        case "7":
                            Console.Write("Sifariş ID-si: ");
                            if (int.TryParse(Console.ReadLine(), out int orderId))
                            {
                                var order = await orderService.GetOrderByIdAsync(orderId);
                                if (order != null)
                                {
                                    Console.WriteLine($"Sifariş ID: {order.Id} | Tarix: {order.Date} | Məbləğ: {order.TotalAmount} AZN");
                                    Console.WriteLine("Məhsullar:");
                                    foreach (var item in order.OrderItems)
                                    {
                                        Console.WriteLine($" - Məhsul: {item.MenuItem.Name} | Say: {item.Count}");
                                    }
                                }
                                else Console.WriteLine("Sifariş tapılmadı.");
                            }
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Yanlış seçim!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"XƏTA: {ex.Message}");
                }

                Console.WriteLine("\nDavam etmək üçün hər hansı bir düyməyə basın...");
                Console.ReadKey();
            }
        }
    }
}