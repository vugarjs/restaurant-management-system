using AutoMapper;
using RestorantApp.Businnes.DTOs.MenuItemDtos;
using RestorantApp.Businnes.DTOs.OrderDtos;
using RestorantApp.Businnes.DTOs.OrderItemDtos;
using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // MenuItem
        CreateMap<MenuItemCreateDto, MenuItem>();
        CreateMap<MenuItem, MenuItemCreateDto>();
        CreateMap<MenuItem, MenuItemReturnDto>();
        CreateMap<MenuItemUpdateDto, MenuItem>();
        CreateMap<MenuItemReturnDto, MenuItem>();

        // Orders
        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderItemCreateDto, OrderItem>();

        CreateMap<Order, OrderReturnDto>();
        CreateMap<OrderItem, OrderReturnDto>();

        CreateMap<MenuItemReturnDto, MenuItemUpdateDto>();
    }
}

