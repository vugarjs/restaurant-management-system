using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RestorantApp.Businnes.Mappers;
using RestorantApp.Businnes.Services.Implementations;
using RestorantApp.Businnes.Services.Interfaces;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Implementations;
using RestorantApp.DataAccess.Repositories.Interfaces;
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
            services.AddScoped<OrderHelpers>();
            services.AddScoped<MainHelpers>();

            var provider = services.BuildServiceProvider();
            var mapper = provider.GetRequiredService<IMapper>();
            //helpers
            var helper = provider.GetRequiredService<MenuHelpers>();
            var orderHelper = provider.GetRequiredService<OrderHelpers>();
            var mainHelper = provider.GetRequiredService<MainHelpers>();




            var menuItemService = provider.GetRequiredService<IMenuItemService>();
            var orderService = provider.GetRequiredService<IOrderService>();

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                mainHelper.DisplayMainMenu();
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;

                switch (choice)
                {
                    case "1":
                        await MenuItemMenu(helper);
                        break;
                    case "2":
                        await OrderMenu(orderHelper);
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

        static async Task OrderMenu(OrderHelpers orderHelper)
        {
            while (true)
            {
                Console.Clear();
                orderHelper.DisplayOrder();
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await orderHelper.AddOrder();
                            break;

                        case "2":
                            await orderHelper.CancelOrder();
                            break;

                        case "3":
                            await orderHelper.ShowAllOrders();
                            break;

                        case "4":
                            await orderHelper.ShowOrdersByDateRange();
                            break;

                        case "5":
                            await orderHelper.ShowOrdersByAmountRange();
                            break;

                        case "6":
                            await orderHelper.ShowOrdersByDate();
                            break;

                        case "7":
                            await orderHelper.ShowById();
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
        static async Task MenuItemMenu(MenuHelpers helper)
        {
            while (true)
            {
                Console.Clear();
                helper.DisplayMenu();
                Console.Write("Seçiminizi edin: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await helper.AddMenuItem();
                            break;
                        case "2":
                            await helper.UpdateMenuItem();
                            break;
                        case "3":
                            await helper.DeleteMenuItem();
                            break;
                        case "4":
                            await helper.ListMenuItems();
                            break;
                        case "5":
                            await helper.ListMenuItemsByCategory();
                            break;
                        case "6":
                            await helper.PriceBetweenRange();
                            break;
                        case "7":
                            await helper.ListMenuItemsByName();
                            break;
                        case "8":
                            await helper.ListMenuItemsById();
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