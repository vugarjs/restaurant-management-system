namespace RestorantApp.Entity.Entities;

public class OrderItem
{
    public int Count { get; set; }

    public int OrderId { get; set; } // Foreign key to the associated order
    public Order Order { get; set; } = null!; // Navigation property to hold the associated order


    public int MenuItemId { get; set; } // Foreign key to the associated menu item
    public MenuItem MenuItem { get; set; } = null!; // Navigation property to hold the associated 
}
