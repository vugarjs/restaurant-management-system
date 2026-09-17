using RestorantApp.Entity.Entities.Common;

namespace RestorantApp.Entity.Entities;

public class Order : BaseEntity
{
    public List<OrderItem> OrderItems { get; set; } = null!; // Navigation property to hold the list of order items
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
