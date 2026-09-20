using RestorantApp.Businnes.DTOs.OrderItemDtos;

namespace RestorantApp.Businnes.DTOs.OrderDtos;



public class OrderCreateDto
{
    public List<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
}