using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.DTOs.OrderDtos;

public class OrderReturnDto
{
    public List<OrderItem> OrderItems { get; set; } = null!; // Navigation property to hold the list of order items
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
}
