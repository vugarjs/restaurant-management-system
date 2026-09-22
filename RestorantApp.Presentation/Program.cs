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
using RestorantApp.Entity.Entities;
using RestorantApp.Entity.Enums;
using RestorantApp.Presentation.HelperMethods;
using System.Text;

namespace RestorantApp.Presentation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var services = new ServiceCollection();
            services.AddDbContext<RestorantContext>();

            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IMenuItemService, MenuItemService>();


            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddAutoMapper(cfg => { }, typeof(MapperProfile).Assembly);
            services.AddLogging();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            services.AddMemoryCache(); // caching üçün əlavə olunur

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<MenuHelpers>();

            var provider = services.BuildServiceProvider();
            var mapper = provider.GetRequiredService<IMapper>();
            var helper = provider.GetRequiredService<MenuHelpers>();




            var menuItemService = provider.GetRequiredService<IMenuItemService>();
            var orderService = provider.GetRequiredService<IOrderService>();

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("========= RESTORAN İDARƏETMƏ SİSTEMİ ==========");
                Console.WriteLine("= 1. Menyu üzərində əməliyyat aparmaq         =");
                Console.WriteLine("= 2. Sifarişlər üzərində əməliyyat aparmaq    =");
                Console.WriteLine("= 0. Sistemdən çıxmaq                         =");
                Console.WriteLine("===============================================");
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;

                switch (choice)
                {
                    case "1":
                        await MenuItemMenu(menuItemService, mapper, helper);
                        break;
                    case "2":
                        await OrderMenu(orderService, mapper, menuItemService);
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

        static async Task MenuItemMenu(IMenuItemService menuItemService, IMapper mapper, MenuHelpers helper)
        {
            while (true)
            {
                Console.Clear();
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
                            await helper.AddMenuItem(name, price, categoryInput);
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

                                if(string.IsNullOrWhiteSpace(updateDto.Name) || updateDto.Price <= 0)
                                {
                                    Console.WriteLine("Məhsul adı boş ola bilməz və qiymət sıfırdan böyük olmalıdır!");
                                    break;
                                }
                                if(newName == menuItemService.GetMenuItemsByNameAsync(item.Name).Result.FirstOrDefault()?.Name)
                                {
                                    throw new Exception("Bu adda məhsul artıq mövcuddur!");
                                }
                                await menuItemService.EditMenuItem(upid, updateDto);
                                await menuItemService.SaveChangesAsync();

                                Console.WriteLine("Məhsul bazada uğurla yeniləndi!");
                            }
                            else
                            {
                                Console.WriteLine("Bu ID-də məhsul tapılmadı.");
                            }



                            break;
                        case "3":
                            Console.WriteLine("\n--- Mövcud Məhsullar ---");
                            foreach (var iteqm in await menuItemService.GetAllMenuItemsAsync())
                            {
                                Console.WriteLine($"ID: {iteqm.Id} | Ad: {iteqm.Name} | Qiymət: {iteqm.Price} AZN");
                            }
                            Console.Write("Silinəcək Məhsulun ID-si: ");
                            var deleteId = int.Parse(Console.ReadLine()!);
                            var itemss = await menuItemService.GetAllMenuItemsAsync();

                            if (await menuItemService.GetMenuItemByIdAsync(deleteId) == null)
                            {
                                throw new Exception("Bu ID-də məhsul tapılmadı!");
                            }

                            menuItemService.RemoveMenuItem(deleteId);
                            await menuItemService.SaveChangesAsync();
                            Console.WriteLine("Məhsul uğurla silindi!");
                            break;

                        case "4":
                           
                            var items = await menuItemService.GetAllMenuItemsAsync();
                            Console.WriteLine("--- Bütün Menyu Elementləri ---");
                            foreach (var i in items)
                            {
                                Console.WriteLine($"ID {i.Id} | Ad: {i.Name} | Kateqoriya: {i.Category} | Qiymət: {i.Price} AZN");
                            }
                            break;

                        case "5":
                            Console.WriteLine("Kateqoriyanı seçin:");
                            foreach (var cat in Enum.GetValues(typeof(Category)))
                                Console.WriteLine($"{cat}");

                            Console.Write("Seçim ad : ");
                            string catInput = Console.ReadLine()!;

                            if (Enum.TryParse(typeof(Category), catInput, true, out var parsedCat))
                            {
                                var catItems = await menuItemService.GetMenuItemsByCategoryAsync((Category)parsedCat);

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
                            break;

                        case "6":
                            Console.Write("Minimum qiymət: ");
                            decimal.TryParse(Console.ReadLine(), out decimal minPrice);
                            Console.Write("Maksimum qiymət: ");
                            decimal.TryParse(Console.ReadLine(), out decimal maxPrice);
                            var rangeItems = await menuItemService.GetMenuItemsByPriceRangeAsync(minPrice, maxPrice);
                            if(!rangeItems.Any())
                            {
                                Console.WriteLine("Bu qiymət aralığında məhsul tapılmadı.");
                            }
                            foreach (var i in rangeItems)
                            {
                                Console.WriteLine($" Ad: {i.Name} | Qiymət: {i.Price} AZN");
                            }
                            break;

                        case "7":
                            Console.Write("Axtarış sözü: ");
                            string searchText = Console.ReadLine()!;
                            var searchItems = await menuItemService.GetMenuItemsByNameAsync(searchText);
                            if(!searchItems.Any())
                            {
                                Console.WriteLine("Bu axtarış sözünə uyğun məhsul tapılmadı.");
                            }
                            if(searchText.Length < 3)
                            {
                                throw new Exception("Axtarış sözü ən azı 3 simvol olmalıdır!");
                            }
                            foreach (var i in searchItems)
                            {
                                Console.WriteLine($"Ad: {i.Name} | Qiymət: {i.Price} AZN");
                            }
                            break;

                        case "8":
                            Console.Write("Axtarış üçün ID daxil edin: ");
                            int.TryParse(Console.ReadLine(), out int id);
                            var idItem = await menuItemService.GetMenuItemByIdAsync(id);
                            if (idItem != null)
                            {
                                Console.WriteLine($"Ad: {idItem.Name} | Qiymət: {idItem.Price} AZN");
                            }
                            else
                            {
                                throw new Exception("İstənilən ID ilə item tapılmadı.");
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
        static async Task OrderMenu(IOrderService orderService, IMapper mapper, IMenuItemService menuItemService)
        {
            while (true)
            {
                Console.Clear();
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
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var orderCreateDto = new OrderCreateDto();

                            while (true)
                            {   
                                Console.WriteLine("\n--- Mövcud Məhsullar ---");
                                foreach (var item in await menuItemService.GetAllMenuItemsAsync())
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

                                var orderItemDto = new OrderItemCreateDto
                                {
                                    MenuItemId = menuItemId,
                                    Count = count
                                };

                                orderCreateDto.OrderItems.Add(orderItemDto);

                                Console.WriteLine("Məhsul səbətə əlavə edildi.");
                            }

                            if (orderCreateDto.OrderItems.Count > 0)
                            {

                                await orderService.AddOrderAsync(orderCreateDto)!;
                                await orderService.SaveChangesAsync();
                                Console.WriteLine("Sifariş uğurla tamamlandı!");
                            }
                            else
                            {
                                Console.WriteLine("Səbət boşdur, sifariş yaradılmadı.");
                            }
                            break;

                        case "2":
                            Console.WriteLine("\n--- Mövcud Sifarişlər ---");

                            foreach (var item in await orderService.GetAllOrdersAsync())
                            {
                                Console.WriteLine($"ID: {item.Id} | Tarix: {item.Date} | Məbləğ: {item.TotalAmount} AZN");
                            }

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

                            if(!dateOrders.Any())
                            {
                                Console.WriteLine("Bu tarix aralığında sifariş tapılmadı.");
                            }
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
                            if(!amountOrders.Any())
                            {
                                Console.WriteLine("Bu məbləğ aralığında sifariş tapılmadı.");
                            }
                            foreach (var o in amountOrders)
                            {
                                Console.WriteLine($"ID: {o.Id} | Tarix: {o.Date} | Məbləğ: {o.TotalAmount} AZN");
                            }
                            break;

                        case "6":
                            Console.Write("Tarix daxil edin (YYYY-MM-DD): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime targetDate))
                            {
                                var exactDateOrders = await orderService.GetOrderByDateAsync(targetDate);

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
                            break;

                        case "7":
                            Console.WriteLine("\n--- Mövcud Sifarişlər ---");

                            foreach (var item in await orderService.GetAllOrdersAsync())
                            {
                                Console.WriteLine($"ID: {item.Id} | Tarix: {item.Date} | Məbləğ: {item.TotalAmount} AZN");
                            }
                            Console.WriteLine("ID Daxil edin: ");
                            if (int.TryParse(Console.ReadLine(), out int orderId))
                            {
                                var order = await orderService.GetOrderByIdAsync(orderId);
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
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Yanlış seçim!");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("XƏTA: Bash verdi zehmet olmasa admine muraciet edin.");
                }

                Console.WriteLine("\nDavam etmək üçün hər hansı bir düyməyə basın...");
                Console.ReadKey();
            }
        }
    }
}