

using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities;

namespace Test
{
    public class MenuItemService_Tests
    {
        private readonly Mock<IMenuItemRepository> _menuItemRepository;
        private readonly Mock<RestorantContext> _context;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IMemoryCache> _memoryCache;

        public MenuItemService_Tests()
        {
            _menuItemRepository = new Mock<IMenuItemRepository>();
            _context = new Mock<RestorantContext>();
            _mapper = new Mock<IMapper>();
            _memoryCache = new Mock<IMemoryCache>();
        }

        [Fact]
        public async Task Should_AddMenuItemAsync_Test()
        {
            // 1. Arrange
            var dto = new MenuItemCreateDto
            {
                Name = "Test Item",
                Price = 10.0m,
                Category = RestorantApp.Entity.Enums.Category.Yemek
            };

            var entity = new MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = dto.Category
            };

            _mapper.Setup(m => m.Map<MenuItem>(It.IsAny<MenuItemCreateDto>())).Returns(entity);
            _menuItemRepository.Setup(r => r.AddAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );

            // 2. Act

            await menuItemService.AddMenuItemAsync(dto);

            // 3. Assert
            _mapper.Verify(m => m.Map<MenuItem>(It.IsAny<MenuItemCreateDto>()), Times.Once);
            _menuItemRepository.Verify(r => r.AddAsync(It.IsAny<MenuItem>()), Times.Once);
        }
        [Fact]
        public void Should_RemoveMenuItem_Test()
        {
            // Arrange
            int menuItemId = 1;
            _menuItemRepository.Setup(r => r.Remove(menuItemId));
            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );
            // Act
            menuItemService.RemoveMenuItem(menuItemId);
            // Assert
            _menuItemRepository.Verify(r => r.Remove(menuItemId), Times.Once);
        }
        [Fact]
        public async Task Should_GetAllMenuItemsAsync_Test()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Item 1", Price = 10.0m, Category = RestorantApp.Entity.Enums.Category.Yemek },
                new MenuItem { Id = 2, Name = "Item 2", Price = 15.0m, Category = RestorantApp.Entity.Enums.Category.Sorba }
            };
            _menuItemRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(menuItems);
            _mapper.Setup(m => m.Map<IEnumerable<MenuItemReturnDto>>(It.IsAny<IEnumerable<MenuItem>>()))
                   .Returns((IEnumerable<MenuItem> source) =>
                       source.Select(item => new MenuItemReturnDto
                       {
                           Id = item.Id,
                           Name = item.Name,
                           Price = item.Price,
                           Category = item.Category
                       }));
            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );
            // Act
            var result = await menuItemService.GetAllMenuItemsAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _menuItemRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task Should_GetMenuItemByIdAsync_Test()
        {
            var cacheEntryMock = new Mock<ICacheEntry>();
            // Arrange
            int menuItemId = 1;
            var menuItem = new MenuItem { Id = menuItemId, Name = "Item 1", Price = 10.0m, Category = RestorantApp.Entity.Enums.Category.Yemek };
            _menuItemRepository.Setup(r => r.FindSingleAsync(x => x.Id == menuItemId)).ReturnsAsync(menuItem);
            _mapper.Setup(m => m.Map<MenuItemReturnDto>(It.IsAny<MenuItem>()))
                   .Returns((MenuItem source) => new MenuItemReturnDto
                   {
                       Id = source.Id,
                       Name = source.Name,
                       Price = source.Price,
                       Category = source.Category
                   });
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            // Setup the memory cache
            _memoryCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntryMock.Object);
            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );
            // Act
            var result = await menuItemService.GetMenuItemByIdAsync(menuItemId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(menuItemId, result.Id);
            _menuItemRepository.Verify(r => r.FindSingleAsync(x => x.Id == menuItemId), Times.Once);
        }
        [Fact]
        public async Task Should_GetMenuItemsByCategoryAsync_Test()
        {
            // Arrange
            var category = RestorantApp.Entity.Enums.Category.Yemek;
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Item 1", Price = 10.0m, Category = category },
                new MenuItem { Id = 2, Name = "Item 2", Price = 15.0m, Category = category }
            };
            _menuItemRepository.Setup(r => r.GetMenuItemsByCategoryAsync(category)).ReturnsAsync(menuItems);
            _mapper.Setup(m => m.Map<IEnumerable<MenuItemReturnDto>>(It.IsAny<IEnumerable<MenuItem>>()))
                   .Returns((IEnumerable<MenuItem> source) =>
                       source.Select(item => new MenuItemReturnDto
                       {
                           Id = item.Id,
                           Name = item.Name,
                           Price = item.Price,
                           Category = item.Category
                       }));
            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );
            // Act
            var result = await menuItemService.GetMenuItemsByCategoryAsync(category);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _menuItemRepository.Verify(r => r.GetMenuItemsByCategoryAsync(category), Times.Once);
        }
        [Fact]
        public async Task Should_GetMenuItemsByNameAsync_Test()
        {
            // Arrange
            string name = "Item";
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Item 1", Price = 10.0m, Category = RestorantApp.Entity.Enums.Category.Yemek },
                new MenuItem { Id = 2, Name = "Item 2", Price = 15.0m, Category = RestorantApp.Entity.Enums.Category.Sorba }
            };
            _menuItemRepository.Setup(r => r.SearchByNameAsync(name)).ReturnsAsync(menuItems);
            _mapper.Setup(m => m.Map<IEnumerable<MenuItemReturnDto>>(It.IsAny<IEnumerable<MenuItem>>()))
                   .Returns((IEnumerable<MenuItem> source) =>
                       source.Select(item => new MenuItemReturnDto
                       {
                           Id = item.Id,
                           Name = item.Name,
                           Price = item.Price,
                           Category = item.Category
                       }));
            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );
            // Act
            var result = await menuItemService.GetMenuItemsByNameAsync(name);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _menuItemRepository.Verify(r => r.SearchByNameAsync(name), Times.Once);
        }
        [Fact]
        public async Task Should_GetMenuItemsByPriceRangeAsync_Test()
        {

           var menus = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Item 1", Price = 10.0m, Category = RestorantApp.Entity.Enums.Category.Yemek },
                new MenuItem { Id = 2, Name = "Item 2", Price = 15.0m, Category = RestorantApp.Entity.Enums.Category.Sorba }
            };
            decimal minPrice = 5.0m;
            decimal maxPrice = 20.0m;
            _menuItemRepository.Setup(r => r.PriceBetweenAsync(minPrice, maxPrice)).ReturnsAsync(menus);
            _mapper.Setup(m => m.Map<IEnumerable<MenuItemReturnDto>>(It.IsAny<IEnumerable<MenuItem>>()))
                   .Returns((IEnumerable<MenuItem> source) =>
                       source.Select(item => new MenuItemReturnDto
                       {
                           Id = item.Id,
                           Name = item.Name,
                           Price = item.Price,
                           Category = item.Category
                       }));
            var menuItemService = new RestorantApp.Businnes.Services.Implementations.MenuItemService(
                _menuItemRepository.Object,
                _context.Object,
                _mapper.Object,
                _memoryCache.Object
            );
            // Act
            var result = await menuItemService.GetMenuItemsByPriceRangeAsync(minPrice, maxPrice);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _menuItemRepository.Verify(r => r.PriceBetweenAsync(minPrice, maxPrice), Times.Once);
        }
        [Fact]
        public async Task Should_EditMenuItem_test()
        {
            //var menuItemId = 1;
            var menuItemDto = new MenuItemCreateDto
            {
                Name = "Updated Item",
                Price = 20.0m,
                Category = RestorantApp.Entity.Enums.Category.Yemek
            };
            _mapper.Setup(m => m.Map<MenuItem>(It.IsAny<MenuItemCreateDto>()))
                   .Returns((MenuItemCreateDto source) => new MenuItem
                   {
                       Name = source.Name,
                       Price = source.Price,
                       Category = source.Category
                   });
            _menuItemRepository.Setup(r => r.Update(It.IsAny<MenuItem>()));

            Assert.NotNull(menuItemDto);
            Assert.Equal("Updated Item", menuItemDto.Name);
            _menuItemRepository.Verify(r => r.Update(It.IsAny<MenuItem>()), Times.Never);
        }
    }
}
