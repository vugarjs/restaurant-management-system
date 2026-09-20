using RestorantApp.Entity.Entities;

namespace RestorantApp.Businnes.DTOs.OrderDtos;

public class OrderReturnDto
{
    public List<Order> Orders { get; set; } = new List<Order>(); // Navigation property to hold the list of order items
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
}
