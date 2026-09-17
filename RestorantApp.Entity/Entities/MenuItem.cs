using RestorantApp.Entity.Entities.Common;
using RestorantApp.Entity.Enums;

namespace RestorantApp.Entity.Entities;

public class MenuItem : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public Category Category { get; set; }


}
