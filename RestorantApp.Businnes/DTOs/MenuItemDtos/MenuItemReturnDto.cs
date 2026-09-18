using RestorantApp.Entity.Enums;

namespace RestorantApp.Businnes.DTOs.MenuItemDtos;

public class MenuItemReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public Category Category { get; set; }
}
