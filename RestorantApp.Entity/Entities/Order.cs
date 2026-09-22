using RestorantApp.Entity.Entities.Common;

namespace RestorantApp.Entity.Entities;

public class Order : BaseEntity
{
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
}
